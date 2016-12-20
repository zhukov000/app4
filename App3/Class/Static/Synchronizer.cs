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

		public static Entity[] SYNC_NEW;

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

        static public void Start()
        {
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    while (true)
                    {
                        List<SyncResult> list = null;
                        Synchronizer.Run(ref list);
                        if (list.Count > 0)
                        {
                            Logger.Instance.WriteToLog("Произведена автоматическая синхронизация с узлами");
                            foreach (SyncResult current in list)
                            {
                                Logger.Instance.WriteToLog(string.Format("IP-адрес: {0} Район: {5} Статус: {1} Получено байт: {2} Изменено: {3} Удалено: {4}", new object[]
                                {
                                    current.IpAddress,
                                    current.Status,
                                    current.Bytes,
                                    current.Reserved,
                                    current.Deleted,
                                    current.Description
                                }));
                            }
                        }
                        Thread.Sleep(Config.Get("SynchSleepMinutes").ToInt() * 60 * 1000);
                    }
                }
            ));
            backgroundThread.Start();
        }

		public static string getTableName(string Name)
		{
			string result = "";
			Entity[] sYNC_NEW = Synchronizer.SYNC_NEW;
			for (int i = 0; i < sYNC_NEW.Length; i++)
			{
				Entity entity = sYNC_NEW[i];
				if (entity.ToString() == Name)
				{
					result = entity.getTableName();
					break;
				}
			}
			return result;
		}

		public static List<KeyValuePair<string, object>[]> getReversed(string table, DateTime dt)
		{
			return DataBase.RowSelect(string.Format("SELECT * FROM {0} WHERE dt > '{1}'", table, dt.ToString()), false);
		}

		public static List<object[]> getDeleted(string table, DateTime dt)
		{
			return DataBase.RowSelect(string.Format("SELECT * FROM deleted WHERE tablename = '{0}' and dt > '{1}'", table, dt.ToString()));
		}

		private static int[] SyncObject(JsonNewObject obj)
		{
			int num = 0;
			int num2 = 0;
			string field = "id";

			string format = "INSERT INTO oko." + obj.objectname + "({0}) VALUES({1})";
			string format2 = "DELETE FROM oko." + obj.objectname + " WHERE {1} = {2}";
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
			}
			if (obj.objectname == "object")
			{
				field = "osm_id";
			}
			foreach (Deleted current2 in obj.deleted)
			{
				try
				{
					num += DataBase.RunCommand(string.Format(format2, field, current2.id));
				}
				catch (Exception ex2)
				{
					Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex2.Message));
				}
			}
			foreach (List<Attr> current3 in obj.reversed)
			{
				List<string> list3 = new List<string>();
				List<string> list4 = new List<string>();
				List<string> list5 = new List<string>();
				string a = "";
				foreach (Attr current4 in current3)
				{
					if (current4.key == field)
					{
						a = current4.val;
					}
					list3.Add(current4.key);
					if (current4.val != "")
					{
						list4.Add(current4.val.Q());
						list5.Add(string.Format("{0} = '{1}'", current4.key, current4.val));
					}
					else
					{
						list4.Add("null");
						list5.Add(string.Format("{0} = null", current4.key));
					}
				}
				int num4 = 0;
				if (a != "")
				{
					try
					{
						string[] expr_36C = new string[8];
						expr_36C[0] = "UPDATE oko.";
						expr_36C[1] = obj.objectname;
						expr_36C[2] = " SET ";
						expr_36C[3] = string.Join(", ", list5);
						expr_36C[4] = " WHERE ";
						expr_36C[5] = field;
						expr_36C[6] = " = ";
                        expr_36C[7] = current3.Where(x => x.key == field).First().val;
                        num4 = DataBase.RunCommand(string.Concat(expr_36C));
					}
					catch (Exception ex3)
					{
						Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex3.Message));
					}
				}
				if (num4 == 0)
				{
					try
					{
						num2 += DataBase.RunCommand(string.Format(format, string.Join(",", list3), string.Join(",", list4)));
						continue;
					}
					catch (Exception ex4)
					{
						Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex4.Message));
						continue;
					}
				}
				num2 += num4;
			}
			return new int[] { num, num2 };
		}

		public static List<string> Run(ref List<SyncResult> Data)
		{
			if (Data == null)
			{
				Data = new List<SyncResult>();
			}
			List<string> list = new List<string>();
			object obj = new object();
			lock (obj)
			{
				if (Synchronizer.SyncStart != null)
				{
					try
					{
						Synchronizer.SyncStart();
					}
					catch (Exception ex)
					{
						Logger.Instance.WriteToLog("Событие SyncStart вызвало исключение: " + ex.Message);
					}
				}
				Data.Clear();
				list.Add("запуск...");
				Synchronizer.status = Synchronizer.Status_Sync.RUN;
				foreach (object[] current in DataBase.RowSelect("select sn.id, sn.ipv4, sn.port, ip.description from syn_nodes sn left join oko.ipaddresses ip on ipv4 = ip.ipaddress where sn.synin"))
				{
					if (!Utils.IsLocalIpAddress(current[1].ToString()))
					{
						SyncResult syncResult = new SyncResult();
						int num = 0;
						int num2 = 0;
						DateTime now = DateTime.Now;
						Stopwatch stopwatch = new Stopwatch();
						stopwatch.Start();
						DateTime dt = DateTime.Now.AddDays(-3.0);
						object obj2 = DataBase.First(string.Format("select id, start from synchronize where node_id = {0} order by start desc", current[0]), "start");
						if (obj2 != null)
						{
							dt = DateTime.Parse(obj2.ToString());
						}
						list.Add("соединяюсь с " + current[1] + "...");
						string text = string.Format("http://{0}:{3}/get_new/{1}/{2}", new object[]
						{
							current[1],
							Utils.DateTimeToString(dt),
							current[0],
							current[2]
						});
						syncResult.IpAddress = current[1].ToString();
						Logger.Instance.WriteToLog(string.Format("соединение с {0}: {1}", current[1], text));
						string htmlContent = Utils.getHtmlContent(text);
						syncResult.Description = current[3].ToString();
						if (htmlContent.Length > 0)
						{
							syncResult.Status = SyncStatus.OK;
							list.Add("успех: " + htmlContent.Length + " байт");
							syncResult.Bytes = htmlContent.Length;
							JsonNewObject[] arg_212_0 = new JavaScriptSerializer
							{
								MaxJsonLength = 2147483647
							}.Deserialize<JsonNewObject[]>(htmlContent);
							bool flag2 = current[1].ToString() == DBDict.Settings["SERVER_ADDRESS"].ToString();
							JsonNewObject[] array = arg_212_0;
							for (int i = 0; i < array.Length; i++)
							{
								JsonNewObject jsonNewObject = array[i];
								list.Add(jsonNewObject.objectname);
								if (flag2 || !(jsonNewObject.objectname != "event"))
								{
									int[] array2 = Synchronizer.SyncObject(jsonNewObject);
									num2 += array2[0];
									num += array2[1];
									string text2 = string.Format("{2} удалено {0}, изменено {1}", array2[0], array2[1], jsonNewObject.objectname);
									list.Add(text2);
									Logger.Instance.WriteToLog(text2);
								}
							}
							list.Add(string.Format("Всего: удалено {0}, изменено {1}", num2, num));
							syncResult.Reserved = num;
							syncResult.Deleted = num2;
						}
						else
						{
							syncResult.Status = SyncStatus.EMPTY;
							list.Add("получен пустой ответ");
						}
						stopwatch.Stop();
						DateTime dateTime = now + stopwatch.Elapsed;
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
				Synchronizer.status = Synchronizer.Status_Sync.SUSPEND;
				if (Synchronizer.SyncStop != null)
				{
					try
					{
						Synchronizer.SyncStop();
					}
					catch (Exception ex2)
					{
						Logger.Instance.WriteToLog("Событие SyncStop вызвало исключение: " + ex2.Message);
					}
				}
			}
			return list;
		}

		static Synchronizer()
		{
			// Note: this type is marked as 'beforefieldinit'.
			Synchronizer.SyncStart = null;
			Synchronizer.SyncStop = null;
			Synchronizer.status = Synchronizer.Status_Sync.SUSPEND;
			Synchronizer.SYNC_NEW = new Entity[]
			{
				new EventSync(),
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
				new SyncZone()
			};
			Synchronizer.backgroundThread = null;
			Synchronizer.NeedUpdate = false;
		}
	}
}
