using System;
using System.Linq;
using System.Windows.Forms;
using App3.Forms;
using System.Threading;
using App3.Class.Singleton;
using System.IO;
using App3.Class;
using App3.Class.Static;

namespace App3
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string tstart = "";
            if (args.Count() == 0)
            {
                StartupForm dialog = new StartupForm();
                dialog.ShowDialog();
                tstart = dialog.TStartup;
            }
            else
            {
                tstart = args[0];
            }
            //
            if (tstart == StartupType.Run)
            {
                using (var mutex = new Mutex(false, "Spolox Application"))
                {
                    bool f = true;
                    // обновление БД
                    if (File.Exists("update.sql"))
                    {
                        f = false;
                        try
                        {
                            DataBase.OpenConnection(
                                string.Format(
                                    "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;Timeout=250;CommandTimeout=0;",
                                    Config.Get("DBServerHost"),
                                    Config.Get("DBServerPort"),
                                    Config.Get("DBUser"),
                                    Config.Get("DBPassword"),
                                    Config.Get("DBName")
                                ));

                            StreamReader file = new StreamReader("update.sql");
                            string line = "";
                            while((line = file.ReadLine()) != null)
                            {
                                DataBase.RunCommand(line);
                            }
                            file.Close();                            

                            f = true;
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog(string.Format("При открытии соединения с базой произошла ошибка: {0}", ex.Message));
                        }
                        
                        if (DataBase.IsOpen())
                        {
                            DataBase.CloseConnection();
                        }
                    
                        if (!f)
                        {
                            string mess = "Ошибка обновления";
                            Logger.Instance.WriteToLog(mess);
                            // MessageBox.Show(mess);
                        }
                        else
                        {
                            FileInfo fi = new FileInfo("update.sql");
                            string str = fi.FullName.Substring(0, fi.FullName.Length - 3) + Utils.DateTimeToString(DateTime.Now) + ".sql";
                            File.Move(fi.FullName, str);
                        }
                    }
                    // если обновление БД упешно пройдено или не нужно
                    if (mutex.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        try
                        {
                            Application.Run(new App3.Forms.MainForm());
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog("Необработанное исключение: " + ex.GetaAllMessages());
                            Logger.Instance.FlushLog();
                        }
                    }
                    else
                    {
                        string mess = "Сбой запуска: другая копия процесса уже запущена";
                        Logger.Instance.WriteToLog(mess);
                        MessageBox.Show(mess);
                    }                    
                }
            }
            else if (tstart == StartupType.Log)
            {
                try
                {
                    LogForm expr_21F = new LogForm();
                    ((LogForm)expr_21F).UpdateView();
                    Application.Run(expr_21F);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog("Необработанное исключение: " + ex.GetaAllMessages());
                    Logger.Instance.FlushLog();
                }
            }
            else if (tstart == StartupType.Socket)
            {
                try
                {
                    SocketTest expr_21F = new SocketTest();
                    Application.Run(expr_21F);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog("Необработанное исключение: " + ex.GetaAllMessages());
                    Logger.Instance.FlushLog();
                }
            }
            else if (tstart == StartupType.Monitor)
            {
                DataBase.OpenConnection(
                                string.Format(
                                    "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                                    Config.Get("DBServerHost"),
                                    Config.Get("DBServerPort"),
                                    Config.Get("DBUser"),
                                    Config.Get("DBPassword"),
                                    Config.Get("DBName")
                                ));

                try
                {
                    MonitorForm expr_21F = new MonitorForm();
                    Application.Run(expr_21F);
                }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog("Необработанное исключение: " + ex.GetaAllMessages());
                    Logger.Instance.FlushLog();
                }
            }
            else if (tstart == StartupType.Server)
            {
                using (var mutex = new Mutex(false, "Spolox Server"))
                {
                    if (mutex.WaitOne(TimeSpan.FromSeconds(2)))
                    {
                        try
                        {
                            DBDict.IsServer = true;
                            Application.Run(new App3.Forms.ServerForm());
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog("Необработанное исключение: " + ex.GetaAllMessages());
                            Logger.Instance.FlushLog();
                        }
                    }
                }
            }
            Environment.Exit(0);
        }
    }
}
