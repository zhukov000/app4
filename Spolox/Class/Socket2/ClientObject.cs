using App3.Class.Singleton;
using App3.Class.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace App3.Class.Socket2
{
    /// <summary>
    /// Класс представляющий отдельное подключение
    /// </summary>
    class ClientObject
    {
        public delegate void ProcessDelegate(SendObject data);
        public ProcessDelegate onProcess = null;

        public TcpClient client;
        public ClientObject(TcpClient tcpClient)
        {
            client = tcpClient;
        }

        /// <summary>
        /// Обработка данных соединения
        /// </summary>
        public void Process()
        {
            try
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] datapart = new byte[64]; // буфер для получаемых данных
                    do
                    {
                        // подождем данных
                        System.Threading.Thread.Sleep(2000);
                        List<byte> data = new List<byte>();
                        // получаем сообщение
                        int bytes = 0;
                        do
                        {
                            bytes = stream.Read(datapart, 0, datapart.Length);
                            for (int i = 0; i < bytes; ++i) data.Add(datapart[i]);
                        }
                        while (stream.DataAvailable);
                        if (data.Count > 0)
                        {
                            Logger.Instance.WriteToLog(string.Format("Socket sync: {0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Получено байт " + data.Count), Logger.LogLevel.DEBUG);
                            // десереализация объекта
                            SendObject obj = (SendObject)SocketUtils.ByteArrayToObject(data.ToArray());
                            if (obj.Message == null) obj.Message = "";
                            // вызов делегата (если он есть)
                            onProcess?.Invoke(obj);
                            if (Config.Get("RedirectAllIncommingServer") != "")
                            { // пересылка
                                SocketClient.SendObjectFromSocket2(obj, Config.Get("RedirectAllIncommingServer"), Config.Get("RedirectAllIncommingPort").ToInt());
                            }
                            // отправляем обратно сообщение об успешном получении
                            datapart = Encoding.Unicode.GetBytes("ACCEPTED");
                            stream.Write(datapart, 0, datapart.Length);
                            break;
                        }
                    } while (false);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            finally
            {
                if (client != null)
                    client.Close();
            }
        }
    }
}
