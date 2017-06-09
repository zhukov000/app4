using App3.Class.Singleton;
using App3.Class.Synchronization;
using App3.Web;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

namespace App3.Class.Static
{
	internal static class Synchronizer
	{
		public enum Status_Sync
		{
			RUN,
			SUSPEND
		}

		private static Synchronizer.Status_Sync status;
        /// <summary>
        /// Справочник объектов синхронизации, ключ - имя
        /// </summary>
		public static IDictionary<string, Entity> SYNC_NEW;

		private static Thread backgroundThread;

		public static bool NeedUpdate;

		public static event Action SyncStart;

		public static event Action SyncStop;

		public static Synchronizer.Status_Sync Status
		{
			get
			{
				return Synchronizer.status;
			}
		}

        public static List<SyncResult> ListData = null;

        /// <summary>
        /// Запуск потока синхронизации
        /// </summary>
        static public void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    while (true)
                    {
                        Synchronizer.Run(ref ListData);
                        if (ListData.Count > 0)
                        {
                            Logger.Instance.WriteToLog("Произведена автоматическая синхронизация с узлами", Logger.LogLevel.EVENTS);
                            foreach (SyncResult current in ListData)
                            {
                                Logger.Instance.WriteToLog(string.Format("IP-адрес: {0} Район: {5} Статус: {1} Получено байт: {2} Изменено: {3} Удалено: {4}", new object[]
                                {
                                    current.IpAddress,
                                    current.Status,
                                    current.Bytes,
                                    current.Reserved,
                                    current.Deleted,
                                    current.Description
                                }), Logger.LogLevel.EVENTS);
                            }
                        }
                        // синхронизация с узлами выполняется автоматически - пауза синхронизации
                        Thread.Sleep(Config.Get("SynchSleepMinutes").ToInt() * 60 * 1000);
                    }
                }
            ));
            backgroundThread.Start();
        }

        /// <summary>
        /// Получить имя таблицы для объекта по его имени
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
		public static string getTableName(string Name)
		{
            Entity entity = null;
            if (Synchronizer.SYNC_NEW.TryGetValue(Name, out entity))
                return entity.getTableName();
            return "";
        }

        /// <summary>
        /// Метод получения объектов для изменения по умолчанию
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<KeyValuePair<string, object>[]> getReversed(string table, DateTime dt)
		{
			return DataBase.RowSelect(string.Format("SELECT * FROM {0} WHERE dt > '{1}'", table, dt.ToString()), false);
		}

        /// <summary>
        /// Метод получения объектов для удаления по умолчанию
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
		public static List<object[]> getDeleted(string table, DateTime dt)
		{
			return DataBase.RowSelect(string.Format("SELECT * FROM deleted WHERE tablename = '{0}' and dt > '{1}'", table, dt.ToString()));
		}
        
        /// <summary>
        /// Выполнение синхронизации для объекта синхронизации
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
		private static int[] SyncObject(JsonNewObject obj)
		{
            // счетчики числа измененных и числа удаленных
			int cntdel = 0;
			int cntrev = 0;
            // ключевое поле
			string field = obj.keyfield;
            // шаблоны запросов на добавление/обновление/удаление
			string qinsert = "INSERT INTO {2} ({0}) VALUES({1})";
			string qdelete = "DELETE FROM {3} WHERE {1} = {2}";
            string qupdate = "UPDATE {0} SET {1} WHERE {2} = {3}";

            #region Шаг 1: Удаление объектов
            foreach (Deleted current2 in obj.deleted)
            {
                try
                {
                    cntdel += DataBase.RunCommand(string.Format(qdelete, field, current2.id, getTableName(obj.objectname)));
                }
                catch (Exception ex2)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex2.Message), Logger.LogLevel.ERROR);
                }
            }
            #endregion

            #region Шаг 2: Изменение объектов
            foreach (List<Attr> synobj in obj.reversed)
            {
                // подготовка изменений в БД
                List<string> insnames = new List<string>(); // список имен полей добавления
                List<string> insvals = new List<string>();  // список значений полей добавления
                List<string> updlist = new List<string>();  // список обновления: имя = значение

                #region проход по атрибутам объекта и подготовка данных для запросов
                string keyval = "";
                foreach (Attr current4 in synobj)
                {
                    if (current4.key == field) // если ключевое поле
                    {
                        if (keyval == "")
                            keyval = current4.val;
                    }
                    else
                    { // если неключевое поле
                        insnames.Add(current4.key);
                        if (current4.val != "")
                            insvals.Add(current4.val.Q());
                        else
                            insvals.Add("null");
                    }
                    if (current4.val != "")
                    {
                        updlist.Add(string.Format("{0} = '{1}'", current4.key, current4.val));
                    }
                    else
                    {
                        updlist.Add(string.Format("{0} = null", current4.key));
                    }
                }
                #endregion
                
                // количество обновленных
                int cntupd = 0;
                // если найдено ключевое значение и объект предназначен для обновления
                if (keyval != "" && obj.type == ReversedType.UPDATE.ToString() && updlist.Count > 0)
                { // пробуем обновить
                    try
                    {
                        cntupd = DataBase.RunCommand(string.Format(qupdate,
                                getTableName(obj.objectname), // имя таблицы
                                string.Join(", ", updlist), // список полей обновления
                                field, // ключевое поле
                                keyval // ключевое значение
                            ));
                    }
                    catch (Exception ex3)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex3.Message),Logger.LogLevel.ERROR);
                    }
                }
                // если записи не были обновлены - попробовать добавить их
                if (cntupd == 0 && insnames.Count > 0 && insvals.Count > 0)
                {
                    try
                    {
                        string ss = string.Format(qinsert,
                                string.Join(",", insnames),  // именя столбцов
                                string.Join(",", insvals),   // значения полей
                                getTableName(obj.objectname) // имя таблицы
                            );
                        Logger.Instance.WriteToLog(ss, Logger.LogLevel.DEBUG);
                        cntrev += DataBase.RunCommand(ss);
                    }
                    catch (Exception ex4)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex4.Message),Logger.LogLevel.ERROR);
                    }
                }
                cntrev += cntupd;
            }
            return new int[] { cntdel, cntrev };

            #endregion


            #region DEPRECATED => MUST DELETE
            /*
			if (obj.objectname == "event")
			{
				using (List<List<Attr>>.Enumerator enumerator = obj.reversed.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						List<Attr> arg_7E_0 = enumerator.Current;
						List<string> list = new List<string>();
						List<string> list2 = new List<string>();
						foreach (Attr current in arg_7E_0)
						{
							if (!(current.key == "id") && current.val != "")
							{
								list.Add(current.key);
								list2.Add(current.val.Q());
							}
						}

						if (list.Count > 0)
						{
							bool flag = true;
							int num3 = 0;
							while (flag)
							{
								string text = string.Format(format, string.Join(",", list), string.Join(",", list2));
								try
								{
									num2 += DataBase.RunCommand(text);
									flag = false;
								}
								catch (Exception ex)
								{
									num3++;
									Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
									Logger.Instance.WriteToLog(text);
									if (num3 > 3)
									{
										break;
									}
								}
							}
						}
					}
                    return new int[] { num, num2 };
                }
			} */
            #endregion

        }

        /// <summary>
        /// Выполнение синхронизации через http
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
		public static List<string> Run(ref List<SyncResult> Data)
		{
			if (Data == null) Data = new List<SyncResult>();
            // list - отчет о синхронизации
			List<string> list = new List<string>();
			object obj = new object();
            // блокировка
            lock (obj)
			{
                // событие о начале синхронизации
				if (Synchronizer.SyncStart != null)
				{
					try
					{
						Synchronizer.SyncStart();
					}
					catch (Exception ex)
					{
						Logger.Instance.WriteToLog("Событие SyncStart вызвало исключение: " + ex.Message, Logger.LogLevel.ERROR);
					}
				}
				Data.Clear();
				list.Add("запуск...");
				Synchronizer.status = Synchronizer.Status_Sync.RUN;
                // получаем список узлов, с которыми нужно выполнить синхронизацию
                // для них в таблице syn_nodes должен быть установлен флаг в поле synin
                foreach (object[] current in DataBase.RowSelect("select sn.id, sn.ipv4, sn.wport, ip.description from syn_nodes sn left join oko.ipaddresses ip on ipv4 = ip.ipaddress where sn.synin and sn.wsync"))
				{
                    // если адрес не является локальным - не совпадает с адресом компьютера, на котором запущена синхронизация
					if (!Utils.IsLocalIpAddress(current[1].ToString()))
					{
						SyncResult syncResult = new SyncResult();
						int num = 0;
						int num2 = 0;
                        // засекаем время
                        DateTime now = DateTime.Now;
						Stopwatch stopwatch = new Stopwatch();
						stopwatch.Start();
                        // получаем дату синхронизации
                        // дата синхронизации не превосходит 3х дней, если об узле нет информации в таблице синхронизации
                        DateTime dt = DateTime.Now.AddDays(-3.0); 
						object obj2 = DataBase.First(string.Format("select id, start from synchronize where node_id = {0} order by start desc", current[0]), "start");
						if (obj2 != null)
						{
							dt = DateTime.Parse(obj2.ToString());
						}
                        // получение данных с узла по протоколу HTTP, метод get_new
                        list.Add("соединяюсь с " + current[1] + "...");
						string text = string.Format("http://{0}:{3}/get_new/{1}/{2}", new object[]
						{
							current[1],
							Utils.DateTimeToString(dt),
							current[0],
							current[2]
						});

						syncResult.IpAddress = current[1].ToString();
						Logger.Instance.WriteToLog(string.Format("соединение с {0}: {1}", current[1], text), Logger.LogLevel.EVENTS);
						string htmlContent = Utils.getHtmlContent(text);
						syncResult.Description = current[3].ToString();
                        // если контент с удаленного узла получен
						if (htmlContent.Length > 0)
						{
							syncResult.Status = SyncStatus.OK;
							list.Add("успех: " + htmlContent.Length + " байт");
							syncResult.Bytes = htmlContent.Length;
                            // десереализация массива объектов
							JsonNewObject[] sync_objects = new JavaScriptSerializer
							{
								MaxJsonLength = 2147483647
							}.Deserialize<JsonNewObject[]>(htmlContent);

                            // если адрес узла синхронизации совпадает с адресм сервера, то узел - пропускается
							bool flag2 = current[1].ToString() == DBDict.Settings["SERVER_ADDRESS"].ToString();

							JsonNewObject[] array = sync_objects;
							for (int i = 0; i < array.Length; i++)
							{
								JsonNewObject jsonNewObject = array[i];
								list.Add(jsonNewObject.objectname);
                                // 
								if (!flag2) /*deprecated || !(jsonNewObject.objectname != "event") */
                                {
                                    // вызов метода синхронизации
									int[] array2 = Synchronizer.SyncObject(jsonNewObject);
                                    // метод возвращает два счетчика: число удаленных, число измененных (обновлено/добавлено)
									num2 += array2[0];
									num += array2[1];
									string text2 = string.Format("{2} удалено {0}, изменено {1}", array2[0], array2[1], jsonNewObject.objectname);
									list.Add(text2);
									Logger.Instance.WriteToLog(text2, Logger.LogLevel.DEBUG);
								}
							}
							list.Add(string.Format("Всего: удалено {0}, изменено {1}", num2, num));
							syncResult.Reserved = num;
							syncResult.Deleted = num2;
                            Logger.Instance.WriteToLog(string.Format("Всего: удалено {0}, изменено {1}", num2, num), Logger.LogLevel.EVENTS);
						}
						else
						{
							syncResult.Status = SyncStatus.EMPTY;
							list.Add("получен пустой ответ");
						}
						stopwatch.Stop();
                        syncResult.Timestamp = now;
                        DateTime dateTime = now + stopwatch.Elapsed;
                        // остановка таймера, запись результата в базу и журнал
						DataBase.RunCommandInsert("synchronize", new Dictionary<string, object>
						{
							{
								"start",
								now.ToString().Q()
							},
							{
								"finish",
								dateTime.ToString().Q()
							},
							{
								"reversed",
								num
							},
							{
								"deleted",
								num2
							},
							{
								"node_id",
								current[0].ToInt()
							}
						});
						Data.Add(syncResult);
					}
					else
					{
						list.Add(string.Format("Пропуск {0} - локальный адрес", current[1]));
					}
					list.Add("... синхронизация завершена");
				}
                // установка статуса синхронизации - приостановлена
				Synchronizer.status = Synchronizer.Status_Sync.SUSPEND;
                // событие - синхронизацие остановлена
				if (Synchronizer.SyncStop != null)
				{
					try
					{
						Synchronizer.SyncStop();
					}
					catch (Exception ex2)
					{
						Logger.Instance.WriteToLog("Событие SyncStop вызвало исключение: " + ex2.Message, Logger.LogLevel.ERROR);
					}
				}
			}
			return list;
		}

		static Synchronizer()
		{
			Synchronizer.SyncStart = null;
			Synchronizer.SyncStop = null;
			Synchronizer.status = Synchronizer.Status_Sync.SUSPEND;
            Entity[] entities = new Entity[]
			{
				// new EventSync(),
                new SyncObjectStatus()/*,
				new SyncAddresses(),
				new SyncClassifier(),
				new SyncCompany(),
				new SyncContact(),
				new SyncContract(),
				new SyncCustomer(),
				new SyncIPAddresses(),
				new SyncMessageText(),
				new SyncObject(),
				new SyncObjectInContract(),
				new SyncObjectProperties(),
				new SyncRealObject(),
				new SyncRegionStatus(),
				new SyncTContact(),
				new SyncTMessages(),
				new SyncTNumber(),
				new SyncTState(),
				new SyncTStatus(),
				new SyncZone()*/
			};

            Synchronizer.SYNC_NEW = new Dictionary<string, Entity>();
            foreach (var entity in entities)
            {
                Synchronizer.SYNC_NEW[entity.Name] = entity;
            }

            Synchronizer.backgroundThread = null;
			Synchronizer.NeedUpdate = false;
		}
	}
}
