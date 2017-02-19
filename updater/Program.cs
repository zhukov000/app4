using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Configuration;
using System.Collections.Generic;
using System.Net;
using FtpLib;
using Microsoft.Owin.Hosting;
using System.Runtime.InteropServices;

namespace RESTService
{
    class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;


        const string VERSION_DIR = "versions";

        static void Main(string[] args)
        {
            if (args.Length > 0 && Array.Exists(args, element => element == "install"))
            {
                // запустить обновление
                string version = getMaxVersion();
                string[] ver = args.Where(element => element.StartsWith("version=")).ToArray();
                if (ver.Length > 0)
                {
                    if (CompareVersions(ver[0].Substring(8), version) <= 0)
                        version = ver[0].Substring(8);
                    else
                        Console.WriteLine("Запрошенная версия не найдена");
                }

                string myVer = getSpoloxVersion();
                if (CompareVersions(version, myVer) > 0)
                {
                    string dir = getTargetDirectory() + "\\" + version;
                    string[] dirs = args.Where(element => element.StartsWith("path=")).ToArray();
                    if (dirs.Length > 0)
                    {
                        dir = dirs[0].Substring(5);
                    }
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir);
                    else
                    { // чистим директорию
                        System.IO.DirectoryInfo di = new DirectoryInfo(dir);
                        foreach (FileInfo f in di.GetFiles())
                        {
                            f.Delete();
                        }
                        foreach (DirectoryInfo d in di.GetDirectories())
                        {
                            d.Delete(true);
                        }
                    }

                    Console.WriteLine("Устанавливаю версию " + version);
                    Thread.Sleep(1000);
                    // соединиться с сервером для получения файла
                    string server = ConfigurationManager.AppSettings["Host"];

                    Console.WriteLine("Качаю файлы");
                    // using (Ftp ftp = new Ftp())
                    try
                    {
                        using (FtpConnection ftp = new FtpConnection(server, "anonymous", ""))
                        {

                            ftp.Open();
                            ftp.Login();

                            if (ftp.DirectoryExists("/Spolox"))
                                ftp.SetCurrentDirectory("/Spolox");

                            string dirDistr = "/Spolox/" + version;
                            foreach (var FtpFile in ftp.GetFiles("/Spolox/" + version))
                            {
                                Console.WriteLine(FtpFile.Name + " => " + dir + @"\" + FtpFile.Name);
                                ftp.GetFile(dirDistr + "/" + FtpFile.Name, dir + @"\" + FtpFile.Name, false);
                                Console.WriteLine("Скачан файл: " + FtpFile.Name + ", размер - " + ftp.GetFileSize(dirDistr + "/" + FtpFile.Name) + " байт" + FtpFile.LastAccessTime.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("При подключении к серверу обновлений произошли ошибки: " + ex.Message);
                    }
                    Console.WriteLine("Установка");
                }
            }
            else
            {
                // запустить сервис автоматического обновления на заданном порту
                var url = "http://+:" + ConfigurationManager.AppSettings["uPort"];

                try
                {
                    using (WebApp.Start<Startup>(url))
                    {
                        var handle = GetConsoleWindow();
                        ShowWindow(handle, SW_HIDE);

                        Console.WriteLine("Running on {0}", url);
                        Console.WriteLine("Press enter to exit");
                        Console.ReadLine();
                    }
                }
                catch
                {
                    return;
                }
            }
            // Console.ReadLine();
        }

        static string getSpoloxVersion()
        {
            string s = "1.0";
            if (File.Exists("version"))
            {
                using (StreamReader file = new StreamReader("version"))
                {
                    s = file.ReadLine();
                }
            }
            else
            {
                using (StreamWriter file = new StreamWriter("version", true))
                {
                    file.WriteLine(s);
                }
            }
            return s;
        }

        static string getTargetDirectory()
        {
            return Directory.GetCurrentDirectory() + "\\" + VERSION_DIR;
        }

        static string getMaxVersion()
        {
            string targetDirectory = getTargetDirectory();
            string s = "1.0";
            if (Directory.Exists(targetDirectory))
            {
                string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory).Select(x => Path.GetFileName(x)).ToArray();
                Array.Sort(subdirectoryEntries, CompareVersions);
                if (subdirectoryEntries.Length > 0)
                    s = subdirectoryEntries[subdirectoryEntries.Length-1];
            }
            return s;
        }

        public static int CompareVersions(string x, string y)
        {
            double d = double.Parse(x.Replace('.', ',')) - double.Parse(y.Replace('.', ','));
            if ( d < 0 )
            {
                return -1;
            } else if (d == 0)
            {
                return 0;
            } else
            {
                return 1;
            }

        }
    }
}
