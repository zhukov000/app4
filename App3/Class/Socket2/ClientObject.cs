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
            NetworkStream stream = null;
            try
            {
                // Logger.Instance.WriteToLog("SocketSync: Client process");
                stream = client.GetStream();
                byte[] datapart = new byte[64]; // буфер для получаемых данных
                while (true)
                {
                    List<byte> data = new List<byte>();
                    // получаем сообщение
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(datapart, 0, datapart.Length);
                        for(int i=0; i < bytes; ++i ) data.Add(datapart[i]);
                    }
                    while (stream.DataAvailable);
                    if (data.Count > 0)
                    {
                        // десереализация объекта
                        SendObject obj = (SendObject)SocketUtils.ByteArrayToObject(data.ToArray());
                        if (obj.Message == null) obj.Message = "";
                        if (obj.Message == "CLOSE") break;
                        // вызов делегата (если он есть)
                        if (obj.Message != "TEST") 
                            onProcess?.Invoke(obj);
                        // отправляем обратно сообщение об успешном получении
                        datapart = Encoding.Unicode.GetBytes("ACCEPTED");
                        stream.Write(datapart, 0, datapart.Length);
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            finally
            {
                if (stream != null)
                    stream.Close();
                if (client != null)
                    client.Close();
            }
        }
    }
}
