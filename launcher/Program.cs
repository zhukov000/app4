using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace launcher
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            using (var mutex = new Mutex(false, "Spolox Launcher Application"))
            {
                if (mutex.WaitOne(TimeSpan.FromSeconds(5)))
                {
                    Application.Run(new Form1());
                }
                else
                {
                    string mess = "Сбой запуска: другая копия процесса уже запущена (иконка в трее)";
                    MessageBox.Show(mess);
                }
            }
        }
    }
}
