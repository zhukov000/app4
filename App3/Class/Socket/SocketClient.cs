using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using App3.Class.Singleton;
using System.Reflection;

namespace App3.Class.Socket
{
    class SocketClient
    {
        static public string SendObjectFromSocket(SendObject data, string server, int port)
        {
            string res = "";
            try
            {
                for (int itry = 0; itry < 3; itry++)
                {
                    // Буфер для входящих данных
                    byte[] bytes = new byte[1024];
                    // Соединяемся с удаленным устройством
                    // Устанавливаем удаленную точку для сокета
                    IPHostEntry ipHost = Dns.GetHostEntry(server);
                    Logger.Instance.WriteToLog("SOCKET ADDRESS CNT: " + ipHost.AddressList.Count().ToString() + ", SERVER = " + server);
                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                    System.Net.Sockets.Socket sender = new System.Net.Sockets.Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    // Соединяем сокет с удаленной точкой
                    sender.Connect(ipEndPoint);
                    // Отправляем данные через сокет
                    int bytesSent = sender.Send(SocketUtils.ObjectToByteArray(data));
                    // Получаем ответ от сервера
                    int bytesRec = sender.Receive(bytes);
                    byte[] byteData = new byte[bytesRec];
                    Array.Copy(bytes, byteData, bytesRec);
                    data = (SendObject)SocketUtils.ByteArrayToObject(byteData);
                    res = data.Message;
                    if (data.Message == "ACCEPTED") break;

                    // Освобождаем сокет
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return res;
        }
    }
}
