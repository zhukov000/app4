/**
 * 
 * http://www.cyberforum.ru/csharp-net/thread106428.html
 **/

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
 
namespace App3.Class.Static
{
    internal static class SingleInstance
    {
        private const int WM_COPYDATA = 0x4A;
        private static readonly bool isNew;
        private static readonly string guid;
        private static Mutex _mutex;
 
        static SingleInstance()
        {
            using (Process currentProcess = Process.GetCurrentProcess())
            {
                guid = string.Format("[{0}]", currentProcess.ProcessName);
            }
 
            if (_mutex == null)
                _mutex = new Mutex(true, guid, out isNew);
        }
 
        /// <summary>
        /// возвращает False если приложение уже запущено  
        /// </summary>
        public static bool IsFirstRun
        {
            get { return isNew; }
        }
 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommand nCmdShow);
 
        [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
 
        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
 
        /// <summary>
        /// Разворачивает и выводит на первый план окно c указанным заголовком
        /// </summary>
        /// <param name="windowName">Название окна</param>
        public static void ShowWindow(string windowName)
        {
            IntPtr handle = FindWindowByCaption(IntPtr.Zero, windowName);
            ShowWindow(handle);
        }
 
        /// <summary>
        /// Разворачивает и выводит на первый план окно c указанным дескриптором
        /// </summary>
        /// <param name="handleWindow">Дескриптор окна</param>
        public static void ShowWindow(IntPtr handleWindow)
        {
            ShowWindow(handleWindow, ShowWindowCommand.Restore);
            SetForegroundWindow(handleWindow);
        }
 
        /// <summary>
        /// Преобразовывает парамтер Message в строку
        /// </summary>
        /// <param name="m">Сообщение WM_COPYDATA</param>
        /// <returns>Строка аргументов разделенных пробелом переденных приложению</returns>
        public static string MessageToString(Message m)
        {
            var data = (COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof (COPYDATASTRUCT));
            return data.lpData;
        }
 
        /// <summary>
        /// Преобразовывает парамтер Message в массив строк
        /// </summary>
        /// <param name="m">Сообщение WM_COPYDATA</param>
        /// <returns>Массив аргументов переденных приложению</returns>
        public static string[] MessageToArray(Message m)
        {
            var data = (COPYDATASTRUCT) Marshal.PtrToStructure(m.LParam, typeof (COPYDATASTRUCT));
            return data.lpData.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
        }
 
        /// <summary>
        /// Отправляет указанному окну сообщение WM_COPYDATA с указаными аргументами
        /// </summary>
        /// <param name="windowName">Название окна</param>
        /// <param name="args">Отправляемые строки</param>
        public static void SendArgs(string windowName, string[] args)
        {
            IntPtr handle = FindWindowByCaption(IntPtr.Zero, windowName);
            SendArgs(handle, args);
        }
 
        /// <summary>
        /// Отправляет указанному окну сообщение WM_COPYDATA с указаными аргументами
        /// </summary>
        /// <param name="handle">Дескриптор окна</param>
        /// <param name="args">Отправляемые строки</param>
        public static void SendArgs(IntPtr handle, string[] args)
        {
            if (handle == IntPtr.Zero || args == null || args.Length <= 0)
                return;
 
            string cmd = "";
 
            for (int i = 0; i < args.Length; i++)
                cmd = String.Concat(cmd, " ", args[i]);
 
            var cds = new COPYDATASTRUCT {lpData = cmd, cbData = cmd.Length + 1};
 
            IntPtr buffer = Marshal.AllocHGlobal(Marshal.SizeOf(cds));
            Marshal.StructureToPtr(cds, buffer, false);
            SendMessage(handle, WM_COPYDATA, IntPtr.Zero, buffer);
            Marshal.FreeHGlobal(buffer);
        }
 
        #region Nested type: COPYDATASTRUCT
 
        [StructLayout(LayoutKind.Sequential)]
        private struct COPYDATASTRUCT
        {
            public int dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)] public string lpData;
        }
 
        #endregion
 
        #region Nested type: ShowWindowCommand
 
        private enum ShowWindowCommand
        {
            /// <summary>
            /// Hides the window and activates another window.
            /// </summary>
            Hide = 0,
            /// <summary>
            /// Activates and displays a window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position.
            /// An application should specify this flag when displaying the window 
            /// for the first time.
            /// </summary>
            Normal = 1,
            /// <summary>
            /// Activates the window and displays it as a minimized window.
            /// </summary>
            ShowMinimized = 2,
            /// <summary>
            /// Maximizes the specified window.
            /// </summary>
            Maximize = 3, // is this the right value?
            /// <summary>
            /// Activates the window and displays it as a maximized window.
            /// </summary>       
            ShowMaximized = 3,
            /// <summary>
            /// Displays a window in its most recent size and position. This value 
            /// is similar to <see cref="Win32.ShowWindowCommand.Normal"></see>
            ///   , except 
            /// the window is not actived.
            /// </summary>
            ShowNoActivate = 4,
            /// <summary>
            /// Activates the window and displays it in its current size and position. 
            /// </summary>
            Show = 5,
            /// <summary>
            /// Minimizes the specified window and activates the next top-level 
            /// window in the Z order.
            /// </summary>
            Minimize = 6,
            /// <summary>
            /// Displays the window as a minimized window. This value is similar to
            /// <see cref="Win32.ShowWindowCommand.ShowMinimized"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowMinNoActive = 7,
            /// <summary>
            /// Displays the window in its current size and position. This value is 
            /// similar to <see cref="Win32.ShowWindowCommand.Show"/>, except the 
            /// window is not activated.
            /// </summary>
            ShowNA = 8,
            /// <summary>
            /// Activates and displays the window. If the window is minimized or 
            /// maximized, the system restores it to its original size and position. 
            /// An application should specify this flag when restoring a minimized window.
            /// </summary>
            Restore = 9,
            /// <summary>
            /// Sets the show state based on the SW_* value specified in the 
            /// STARTUPINFO structure passed to the CreateProcess function by the 
            /// program that started the application.
            /// </summary>
            ShowDefault = 10,
            /// <summary>
            ///  <b>Windows 2000/XP:</b> Minimizes a window, even if the thread 
            /// that owns the window is not responding. This flag should only be 
            /// used when minimizing windows from a different thread.
            /// </summary>
            ForceMinimize = 11
        }
 
        #endregion
    }
}