using App3.Class.Singleton;
using App3.Class.Static;
using App3.Forms;
using App3.Forms.Dialog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;


namespace App3.Class
{
    public static class Utils
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);

        public enum MessageGroupId {
            UNDEFINED = 0,
            NORMA,
            NEISPRAVNO,
            NET_NA_SVY,
            TREV_POGAR,
            TREV_OHRAN,
            SERVIS,
            CH_S
        };

        public class DecryptException : Exception
        {
        }

        public static string AppDirectory()
        {
            string s = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            return s;
        }

        public static int ComboboxVal(ref ComboBox combo)
        {
            int ret = 0;
            if (combo.SelectedIndex > -1)
            {
                ComboboxItem selItem = (ComboboxItem)combo.Items[combo.SelectedIndex];
                ret = selItem.Value.ToInt();
            }
            return ret;
        }
        
        // обновление статусов регионов
        static public void UpdateDistrictStatuses(int HomeDistrict)
        {

            Logger.Instance.WriteToLog("Update district status started");
            if (HomeDistrict == -1)
            {
                DataBase.RunCommand("select oko.update_district_statuses()");
            }
            else
            {
                DataBase.RunCommand(string.Format("select oko.update_district_status({0})", HomeDistrict));
            }
            Logger.Instance.WriteToLog("Update district status finished");

        }

        public static void ArhiveEvents()
        {
            object o = DataBase.First("select now()::date - max(start)::date as dt from archive.journal", "dt");
            if (o != null && o.ToInt() > 1)
            {
                Logger.Instance.WriteToLog("Archive event");
                DataBase.RunCommand("select * from archive.arh_events()");
            }
        }

        public static void FreezeObject(Int64 pObjectId, DateTime pFreezeOff)
        {
            object o = DataBase.First("select * from oko.freeze where osm_id = " + pObjectId, "thaw");
            var data = new Dictionary<string,object>()
            { 
                {"osm_id", pObjectId },
                {"thaw", pFreezeOff } 
            };
            if (o == null)
            {
                DataBase.RunCommandInsert("oko.freeze", data);
            }
            else
            {
                DataBase.RunCommandUpdate("oko.freeze", data, new Dictionary<string, object>() { { "osm_id", pObjectId } });
            }
        }

        public static Color FontColor(Color BackColor)
        {
            if ((BackColor.R + BackColor.G + BackColor.B).ToDouble() / 3 > 85)
                return Color.Black;
            else
                return Color.White;
        }

        public static void GetObjectInfo(int Number)
        {

        }

        public static bool ListenIP(string address)
        {
            bool res = false;
            int nothing = 0;
            if (DBDict.IPAddress.TryGetValue(address, out nothing)) res = true;
            return res;
        }

        public static int RegionByIP(string address)
        {
            return DataBase.First(
                    string.Format(
                        "SELECT * FROM oko.ipaddresses WHERE ipaddress = '{0}'",
                        address
                    ), 
                    "id_region"
                ).ToInt() ;
        }

        public static int MessageGroup(int Class, int Code, int Oko)
        {
            object o = DataBase.First(String.Format("SELECT * FROM oko.message_text WHERE class = {0} and code = {1} and \"OKO\" = {2}", Class, Code, Oko), "mgroup_id");
            return o.ToInt();
        }

        public static List<object[]> RegionStatus()
        {
            return DataBase.RowSelect("SELECT color, min_norma, max_norma FROM oko.region_status ORDER BY min_norma DESC");
        }

        public static List<object[]>[] CommonStatistic()
        {
            List<object[]>[] res = new List<object[]>[2];
            res[0] = DataBase.RowSelect(
                @"select state, color, count(osm_id) as cnt, state_id 
                from oko.object_status and region_id in (select id_region from oko.ipaddresses where listen)
                where instat and dogovor
                group by state, color, state_id");
            res[1] = DataBase.RowSelect(
                @"select state, color, count(osm_id) as cnt, state_id 
                from oko.object_status and region_id in (select id_region from oko.ipaddresses where listen)
                where instat and not dogovor
                group by state, color, state_id");
            return res;
        }

        public static void UpdateState(int ObjNumber, int RegionId)
        {
            string sql = String.Format("with sub as ( select objectnumber, region_id, mt.mgroup_id from oko.event e inner join oko.message_text mt on mt.class = e.class and mt.\"OKO\" = e.oko_version and e.code = mt.code " +
                            @"where objectnumber = {0} and region_id = {1}
                            order by datetime desc
                            limit 1
                            )
                            update oko.object obj
                            set tstate_id = sub.mgroup_id
                            from sub
                            where sub.objectnumber = obj.number and sub.region_id = obj.region_id", ObjNumber, RegionId);
            DataBase.RunCommand(sql);
        }

        public static List<object []>[] GetStatistic(int RegionId)
        {
            List<object[]>[] res = new List<object[]>[2] { new List<object[]>(), new List<object[]>() };
            List<object[]> rows = new List<object[]>();
            rows = DataBase.RowSelect(
                    string.Format(
                        @"select state, color, count(os.osm_id) as cnt, state_id, o.dogovor 
                            from oko.object_status os 
                            inner join oko.object o on o.osm_id = os.osm_id and region_id in (select id_region from oko.ipaddresses where listen)
                            where instat and (o.region_id = {0} or 0 = {0})
                            group by os.rank % 100, o.dogovor, state, color, state_id
                            order by o.dogovor desc, os.rank % 100,  state, color, state_id",
                        RegionId
                    )
                );
            int i = 0;
            // с договором
            for (; i < rows.Count; i++ )
            {
                if (!rows[i][4].ToBool())
                    break;
                res[0].Add(rows[i]);
            }
            // без договора
            for (; i < rows.Count; i++)
            {
                res[1].Add(rows[i]);
            }
            return res;
        }

        public static List<object[]>[] RegionStatistic(int RegionId)
        {
            List<object[]>[] res = new List<object[]>[2];
            res[0] = DataBase.RowSelect(
                    string.Format(
                        @"select state, color, count(os.osm_id) as cnt, state_id 
                            from oko.object_status os 
                            inner join oko.object o on o.osm_id = os.osm_id 
                            where instat and o.dogovor and o.region_id = {0} group by state, color, state_id",
                        RegionId
                    )
                );
            res[1] = DataBase.RowSelect(
                    string.Format(
                        @"select state, color, count(os.osm_id) as cnt, state_id 
                            from oko.object_status os 
                            inner join oko.object o on o.osm_id = os.osm_id 
                            where instat and not o.dogovor and o.region_id = {0} group by state, color, state_id",
                        RegionId
                    )
                );
            return res;
        }

        public static int GetNextNumber(int number, int region_id)
        {
            object o = DataBase.First(
                    string.Format(
                        @"(select osm_id, number
                            from oko.object
                            where number > {0} and region_id = {1}
                            order by number
                            limit 1 )
                            union 
                            ( select osm_id, number
                            from oko.object
                            where region_id = {1}
                            order by number 
                            limit 1)
                            order by number desc
                            limit 1",
                        number, region_id
                    ),
                    "osm_id"
                );
            return o.ToInt();
        }

        public static int GetPrevNumber(int number, int region_id)
        {
            object o = DataBase.First(
                    string.Format(
                        @"( select osm_id, number
                            from oko.object
                            where number < {0} and region_id = {1}
                            order by number desc 
                            limit 1)
                            union 
                            ( select osm_id, number
                            from oko.object
                            where region_id = {1}
                            order by number desc
                            limit 1)
                            order by number
                            limit 1",
                        number, region_id
                    ),
                    "osm_id"
                );
            return o.ToInt();
        }

        public static void UpdateRegionStatus()
        {
            DataBase.RowSelect("select oko.update_district_status(num) from regions2map");
        }

        public static object[] RegionStatus(int RegionId)
        {
            return DataBase.FirstRow(
                    string.Format(
                        "select * from oko.district_status({0})",
                        RegionId
                    )
                ,0);
        }

        /// <summary>
        /// Получает статистику по числу объектов по региону
        /// </summary>
        /// <param name="RegionId"></param>
        /// <returns></returns>
        /*public static List<object[]> StatObjects(int RegionNum = 0)
        {
            string s = "select status_id, count(objectnumber) from oko.object_status where status_id in (1,2,3)";
            if (RegionNum != 0)
            {
                s += string.Format(" and ST_covers((SELECT distinct way FROM regions2map WHERE num = {0}), way)", RegionNum); 
            }
            s += " group by status_id";
            return DataBase.RowSelect(
                    string.Format(@"select id, count, status, color from ({0}) stat
                        join
                        (
                            select mg.name, mg.status, mg.color, mg.id
                            from oko.message_group mg
                        ) colors on colors.id = stat.status_id
                        order by status_id", s
                    )
                );
        }*/

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        public static string GetaAllMessages(this Exception exception)
        {
            var messages = exception.FromHierarchy(ex => ex.InnerException)
                .Select(ex => ex.Message);
            return String.Join(Environment.NewLine, string.Format("{1} : {0}", messages, exception.TargetSite.Name));
        }

        public static List<object[]> GetListenIp()
        {
            return DataBase.RowSelect("select distinct rm.fullname, rm.num, rm.color, rm.name \r\n                    from public.regions2map rm\r\n                      inner join oko.ipaddresses ip on rm.num = ip.id_region\r\n                    where ip.listen\r\n                    order by name");
        }

        public static string GetMessageText(int Class, int Code)
        {
            string s = "Неизвестное сообщение";
            object[] o = DataBase.FirstRow(
                string.Format("SELECT message, notes, mgroup_id FROM oko.message_text WHERE code = {0} and class = {1}", Code, Class), 
                0
            );
            if (o != null && o.Count() > 0)
            {
                s = string.Format("{0}({1})", o[0], o[1]);
            }
            return s;
        }

        public static object[] GetMessageGroup(int GroupId)
        {
            return DataBase.FirstRow("SELECT * FROM oko.tstate WHERE id = " + GroupId, 0);
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static string CutPrefixRec(string Text)
        {
            int i = Text.IndexOf("REC:");
            if (i < 0)
            {
                i = -1;
            }
            else
            {
                i = i + 4;
            }
            return Text.Substring(i + 1);
        }

        public static void restartApp()
        {
            ProcessStartInfo Info = new ProcessStartInfo();
            Info.Arguments = "/C ping 127.0.0.1 -n 2 && \"" + Application.ExecutablePath + "\"";
            Info.WindowStyle = ProcessWindowStyle.Hidden;
            Info.CreateNoWindow = true;
            Info.FileName = "cmd.exe";
            Process.Start(Info);
            Application.Exit();
        }


        public static string CutPrefix(string Text)
        {
            int i = Text.IndexOf("SEND:");
            if (i<0)
            {
                return CutPrefixRec(Text);
            }
            else
            {
                i = i + 5;
            }
            return Text.Substring(i + 1);
        }

        public static void MergeCellsInRow(DataGridView dataGridView1, int row, int col1, int col2)
        {
            Graphics g = dataGridView1.CreateGraphics();
            Pen p = new Pen(dataGridView1.GridColor);
            Rectangle r1 = dataGridView1.GetCellDisplayRectangle(col1, row, true);
            Rectangle r2 = dataGridView1.GetCellDisplayRectangle(col2, row, true);

            int recWidth = 0;
            string recValue = string.Empty;
            for (int i = col1; i <= col2; i++)
            {
                recWidth += dataGridView1.GetCellDisplayRectangle(i, row, true).Width;
                if (dataGridView1[i, row].Value != null)
                    recValue += dataGridView1[i, row].Value.ToString() + " ";
            }
            Rectangle newCell = new Rectangle(r1.X, r1.Y, recWidth, r1.Height);
            g.FillRectangle(new SolidBrush(dataGridView1.Rows[row].DefaultCellStyle.BackColor), newCell);
            g.DrawRectangle(p, newCell);

            g.DrawString(recValue, dataGridView1.Rows[row].DefaultCellStyle.Font, new SolidBrush(Color.WhiteSmoke), newCell.X + 10, newCell.Y + 3);
        }

        public static void MergeCellsInColumn(DataGridView dataGridView1, int col, int row1, int row2)
        {
            Graphics g = dataGridView1.CreateGraphics();
            Pen p = new Pen(dataGridView1.GridColor);
            Rectangle r1 = dataGridView1.GetCellDisplayRectangle(col, row1, true);
            Rectangle r2 = dataGridView1.GetCellDisplayRectangle(col, row2, true);

            int recHeight = 0;
            string recValue = string.Empty;
            for (int i = row1; i <= row2; i++)
            {
                recHeight += dataGridView1.GetCellDisplayRectangle(col, i, true).Height;
                if (dataGridView1[col, i].Value != null)
                    recValue += dataGridView1[col, i].Value.ToString() + " ";
            }
            Rectangle newCell = new Rectangle(r1.X, r1.Y, r1.Width, recHeight);
            g.FillRectangle(new SolidBrush(dataGridView1.DefaultCellStyle.BackColor), newCell);
            g.DrawRectangle(p, newCell);
            g.DrawString(recValue, dataGridView1.DefaultCellStyle.Font, new SolidBrush(dataGridView1.DefaultCellStyle.ForeColor), newCell.X + 3, newCell.Y + 3);
        }

        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        #region Конвертирование строки <=> массива байт

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        #endregion

        #region Окошко ожидания в отдельном потоке

        public static WaitDialog CreateWaitThread(Form parent, int Monitor)
        {
            WaitDialog wd = new WaitDialog(parent);
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ShowOnMonitor(wd, Monitor);
                    wd.ShowDialog();
                }
            ));
            backgroundThread.Start();
            return wd;
        }

        public static void DestroyWaitThread(WaitDialog wd)
        {
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    Thread.Sleep(1000);
                    wd.Invoke(new Action(() => { wd.Close(); }));
                }
            ));
            backgroundThread.Start();
        }

        public static string DateTimeToString(DateTime dt)
        {
            return string.Format("{0}{1}{2}{3}{4}{5}", dt.Year, dt.Month.ToString("D2"), dt.Day.ToString("D2"), dt.Hour.ToString("D2"), dt.Minute.ToString("D2"), dt.Second.ToString("D2"));
        }

        public static DateTime StringToDateTime(String str)
        {
            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
        }

        public static StartForm CreateLoadThread(int Monitor)
        {
            StartForm wd = new StartForm();
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    ShowOnMonitor(wd, Monitor);
                    wd.ShowDialog();
                }
            ));
            backgroundThread.Start();
            return wd;
        }

        public static void DestroyStartThread(StartForm wd)
        {
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    Thread.Sleep(1000);
                    wd.Invoke(new Action(() => { wd.Close(); }));
                }
            ));
            backgroundThread.Start();
        }

        /// <summary>
        /// Проверяет является ли адрес локальным
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static bool IsLocalIpAddress(string host)
        {
            try
            { 
                IPAddress[] hostIPs = Dns.GetHostAddresses(host);
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress hostIP in hostIPs)
                {
                    if (IPAddress.IsLoopback(hostIP)) return true;
                    foreach (IPAddress localIP in localIPs)
                    {
                        if (hostIP.Equals(localIP)) return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return false;
        }

        public static string getHtmlContent(string urlAddress)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(urlAddress);
                response = (HttpWebResponse)request.GetResponse();
            }
            catch(Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                return "";
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null || response.CharacterSet == "")
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }

                string data = readStream.ReadToEnd();

                response.Close();
                readStream.Close();

                return data;
            }
            return "";
        }

        public static int CompareVersions(string x, string y)
        {
            double d = double.Parse(x.Replace('.', ',')) - double.Parse(y.Replace('.', ','));
            if (d < 0)
            {
                return -1;
            }
            else if (d == 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }

        }

        #endregion

        #region Расширение стандартных функций

        /// <summary>
        /// Проверка строки по маске - оператор Like
        /// </summary>
        /// <param name="toSearch"></param>
        /// <param name="toFind"></param>
        /// <returns></returns>
        public static bool Like(this string toSearch, string toFind)
        {
            return new System.Text.RegularExpressions.Regex(@"\A" + new System.Text.RegularExpressions.Regex(@"\.|\$|\^|\{|\[|\(|\||\)|\*|\+|\?|\\").Replace(toFind, ch => @"\" + ch).Replace('_', '.').Replace("%", ".*") + @"\z", System.Text.RegularExpressions.RegexOptions.Singleline).IsMatch(toSearch);
        }

        /// <summary>
        /// Добавляет слева и справа одинарную кавычку и приводит к строке
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        public static string Q(this object S)
        {
            string str = S.ToString().Trim('\'');
            return string.Format("'{0}'", str);
        }

        public static Int32 ToInt(this object S)
        {
            int i = 0;
            if (S != null)
                int.TryParse(S.ToString(), out i);
            return i;
        }

        public static Double ToDouble(this object S)
        {
            Double i = 0;
            if (S != null)
                Double.TryParse(S.ToString(), out i);
            return i;
        }

        public static Int64 ToInt64(this object S)
        {
            Int64 i = 0;
            if (S != null)
                Int64.TryParse(S.ToString(), out i);
            return i;
        }

        public static bool ToBool(this object S)
        {
            bool b = false;
            if (S != null)
                bool.TryParse(S.ToString(), out b);
            return b;
        }

        public static byte ToByte(this object B)
        {
            byte result = 0;
            if (B != null)
            {
                byte.TryParse(B.ToString(), out result);
            }
            return result;
        }

        public static ushort ToUShort(this object B)
        {
            ushort result = 0;
            if (B != null)
            {
                ushort.TryParse(B.ToString(), out result);
            }
            return result;
        }
        
        public static string Crypt(this string text)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(text), Encoding.Unicode.GetBytes(Config.Get("Entropy")), DataProtectionScope.LocalMachine));
        }

        public static string Derypt(this string text)
        {
            string result = "--- ошибочное значение ---";
            try
            {
                result = Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(text), Encoding.Unicode.GetBytes(Config.Get("Entropy")), DataProtectionScope.LocalMachine));
            }
            catch
            {
                throw new Utils.DecryptException();
            }
            return result;
        }

        #endregion


        public static void ShowOnMonitor(Form frm, int numberMonitor)
        {
            // const short SWP_NOMOVE = 0X2;
            // const short SWP_NOSIZE = 1;
            const short SWP_NOZORDER = 0X4;
            // const int SWP_SHOWWINDOW = 0x0040;

            Screen[] sc;
            sc = Screen.AllScreens;
            Logger.Instance.WriteToLog("Monitor: " + numberMonitor.ToString());
            /*Logger.Instance.WriteToLog("AllScreen: " + sc.Length);
            for(int i=0; i<sc.Length; i++)
            {
                Logger.Instance.WriteToLog(string.Format("Index: {2}, Left: {0}, Top: {1}, Width: {3}, Height: {4}",
                    sc[i].Bounds.Left,
                    sc[i].Bounds.Top, 
                    i,
                    sc[i].Bounds.Width,
                    sc[i].Bounds.Height)
                );
            }
            Logger.Instance.WriteToLog(string.Format("FORM Left: {0}, Top: {1}", frm.Left, frm.Top));*/

            // Rectangle monitor = Screen.AllScreens[numberMonitor - 1].WorkingArea;

            IntPtr hWnd = FindWindow((string)null, GetMainTitle());
            if (hWnd != IntPtr.Zero)
            {
                if (numberMonitor < 1 || numberMonitor > sc.Count())
                {
                    numberMonitor = 1;
                }
                SetWindowPos(hWnd, 0,
                        sc[numberMonitor - 1].Bounds.Left,
                        sc[numberMonitor - 1].Bounds.Top,
                        sc[numberMonitor - 1].Bounds.Width,
                        sc[numberMonitor - 1].Bounds.Height,
                        SWP_NOZORDER);
            }
            else
            {
                Logger.Instance.WriteToLog("Окно не найдено");
            }
        }

        public static string GetMainTitle()
        {
            return "ГМК Сполох версия " + Config.APPVERSION;
        }

        public static List<IDictionary<string, object>> GetObjects(int region_id = -1)
        {
            List<IDictionary<string, object>> result = new List<IDictionary<string, object>>();
            string sql = @"select ST_AsText(ST_Transform(way, 4326)) as point, 
	                        obj.number, obj.name, obj.osm_id, obj.tstate_id, obj.tstatus_id,
                            obj.region_id, obj.makedatetime, obj.dogovor, obj.dt, obj.description,
                            adr.code, adr.locality, adr.street, adr.house, adr.region,
                            adr.address, adr.lat, adr.lon
                        from oko.object obj
                        left join oko.addresses adr on obj.address_id = adr.id ";
            if (region_id != -1)
            {
                sql += @" where region_id = {0} ";
            }
            sql += "order by obj.number";

            foreach (object[] row in DataBase.RowSelect(string.Format(sql, region_id)))
            {
                result.Add(new Dictionary<string, object>
                {
                    { "point", row[0] },
                    { "number", row[1] },
                    { "name", row[2] },
                    { "osm_id", row[3] },
                    { "tstate_id", row[4] },
                    { "tstatus_id", row[5] },
                    { "region_id", row[6] },
                    { "makedatetime", row[7] },
                    { "dogovor", row[8] },
                    { "datetime", row[9].Q() },
                    { "description", row[10] },
                    { "code", row[11] },
                    { "locality", row[12] },
                    { "street", row[13] },
                    { "house", row[14] },
                    { "region", row[15] },
                    { "address", row[16] },
                    { "lat", row[17] },
                    { "lon", row[18] }
                });
            }

            return result;
        }

        public static List<IDictionary<string, object>> GetObjectsStatuses(int region_id = -1)
        {
            List<IDictionary<string, object>> result = new List<IDictionary<string, object>>();
            string sql = @"SELECT DISTINCT ON (objectnumber) objectnumber,
                                alarmgroupid, code, channelnumber, partnumber,
                                zoneusernumber, typenumber, class, address, 
                                datetime, region_id, oko_version, retrnumber, 
                                isrepeat, siglevel, id
                           FROM   oko.event ";
            if (region_id != -1)
            {
                sql += @" WHERE region_id = {0} ";
            }
            sql += " ORDER BY objectnumber, datetime DESC";
            foreach (object[] row in DataBase.RowSelect(string.Format(sql, region_id)))
            {
                result.Add( new Dictionary<string, object>
                {
                    { "objectnumber", row[0] },
                    { "alarmgroupid", row[1] },
                    { "code", row[2] },
                    { "channelnumber", row[3] },
                    { "partnumber", row[4] },
                    { "zoneusernumber", row[5] },
                    { "typenumber", row[6] },
                    { "class", row[7] },
                    { "address", row[8] },
                    { "datetime", row[9].Q() },
                    { "region_id", row[10] },
                    { "oko_version", row[11] },
                    { "retrnumber", row[12] },
                    { "isrepeat", row[13] },
                    { "siglevel", row[14] },
                    { "id", row[15] }
                });
            }
            return result;
        }

    }
}
