using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace launcher
{
    public partial class Form1 : Form
    {
        static string FILENAME = ConfigurationManager.AppSettings["AppPath"];
        static Thread backgroundThread = null;
        static int pause = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultWait"]);
        static bool wait = false;
        static bool isAlive = false;

        public Form1()
        {
            InitializeComponent();
            Logger.Instance.WriteToLog("Старт приложения");
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // if (FormWindowState.Minimized == WindowState)
            Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            Hide();
            Start();
        }

        static public void Start()
        {
            
                backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {
                        Logger.Instance.WriteToLog("Старт фонового потока");
                        isAlive = true;
                        string updPath = ConfigurationManager.AppSettings["UpdBatPath"];
                        Process firstProc = new Process();
                        firstProc.StartInfo.FileName = FILENAME;
                        // RunOption
                        firstProc.StartInfo.Arguments = "run";
                        try
                        {
                            firstProc.StartInfo.Arguments = ConfigurationManager.AppSettings["RunOption"]; 
                        }
                        catch (Exception ex)
                        {
                            Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                        }
                        firstProc.EnableRaisingEvents = true;
                        Logger.Instance.WriteToLog("Запуск процесса: " + firstProc.StartInfo.FileName);
                        while (isAlive)
                        {
                            bool answer = true;
                            // запуск приложения Сполох
                            try
                            {
                                firstProc.Start();
                            }
                            catch(Exception ex)
                            {
                                Logger.Instance.WriteToLog("Стек вызова: " + ex.StackTrace);
                                Logger.Instance.WriteToLog("Не удалось запустить приложение Сполох: " + ex.Message);
                                Thread.Sleep(pause);
                                for (int i = 0; i < pause / 1000; i++)
                                {
                                    Thread.Sleep(1000);
                                    if (!isAlive) break;
                                }
                                continue;
                            }
                            firstProc.WaitForExit();

                            try
                            {
                                // запуск обновления
                                // ExecuteCommand(updPath);
                                if (ConfigurationManager.AppSettings["EnableUpdate"] == "1")
                                {
                                    RunCmd(updPath, "");
                                }
                            } catch (Exception ex)
                            {
                                Logger.Instance.WriteToLog("Невозможно запустить команду обновления приложения: " + ex.Message);
                            }
                            
                            do
                            {
                                do
                                {
                                    for (int i = 0 ; i < pause / 1000 ; i ++)
                                    {
                                        Thread.Sleep(1000);
                                        if (!isAlive) break;
                                    }
                                }
                                while (wait);
                                if (isAlive)
                                {
                                    if (ConfigurationManager.AppSettings["AskForRun"] == "1")
                                    {
                                        answer = MessageBox.Show("Приложение СПОЛОХ неактивно. Запустить?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes;
                                    }
                                    else if (ConfigurationManager.AppSettings["AskForRun"] == "2")
                                    {
                                        answer = true; // безусловный незапуск
                                    }
                                    else
                                    {
                                        answer = false; // безусловный запуск
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            } while (answer);
                        }
                        Logger.Instance.WriteToLog("Окончание фонового потока");
                        Logger.Instance.FlushLog();
                    }
                ));
                backgroundThread.Start();
            
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ( MessageBox.Show("Вы действительно хотите завершить все процессы системы СПОЛОХ?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes )
            {
                Logger.Instance.WriteToLog("Плановое завершение");
                Close();
            }
        }

        private void Stop()
        {
            if (backgroundThread != null) backgroundThread.Abort();
            isAlive = false;
            Logger.Instance.WriteToLog("Прерывание фонового потока");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Stop();
        }

        public static void RunCmd(string exefile, string strCmdText, bool wait = true)
        {
            Process process = new Process { StartInfo = new ProcessStartInfo(exefile, strCmdText) };
            process.Start();
            if (wait)
            {
                process.WaitForExit();
            }
        }

        static void ExecuteCommand(string command)
        {
            Logger.Instance.WriteToLog("Запуск команды: " + command);
            int exitCode;
            ProcessStartInfo processInfo;
            Process process;

            processInfo = new ProcessStartInfo("cmd.exe", "/c \"" + command + "\"");
            processInfo.CreateNoWindow = true;
            processInfo.UseShellExecute = false;
            // *** Redirect the output ***
            processInfo.RedirectStandardError = true;
            processInfo.RedirectStandardOutput = true;

            process = Process.Start(processInfo);
            process.WaitForExit();

            // *** Read the streams ***
            // Warning: This approach can lead to deadlocks, see Edit #2
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            exitCode = process.ExitCode;

            Console.WriteLine("output>>" + (String.IsNullOrEmpty(output) ? "(none)" : output));
            Console.WriteLine("error>>" + (String.IsNullOrEmpty(error) ? "(none)" : error));
            Console.WriteLine("ExitCode: " + exitCode.ToString(), "ExecuteCommand");
            process.Close();
        }

        private void интервалОпросаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            wait = true;
            Ask frm = new Ask();
            frm.ShowDialog();
            pause = frm.AskInt;
            Logger.Instance.WriteToLog("Интервал опроса изменен: " + pause);
            wait = false;
        }

        private void Restart()
        {
            Stop();
            Start();
        }

        private void сполохToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Restart();
        }
    }
}
