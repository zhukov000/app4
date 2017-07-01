using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using App3.Class.Singleton;

namespace App3.Class.Socket
{
    class SocketClient
    {
        static public string SendObjectFromSocket2(SendObject data, string server, int port)
        {
            TcpClient client = null;
            try
            {
                client = new TcpClient(server, port);
                Logger.Log("Socket sync: send object " + data.ObjectNum, Logger.LogLevel.DEBUG);

                using (NetworkStream stream = client.GetStream())
                {
                    // преобразуем сообщение в массив байтов
                    byte[] data_arr = SocketUtils.ObjectToByteArray(data);
                    // отправка сообщения
                    stream.Write(data_arr, 0, data_arr.Length);

                    Logger.Log(string.Format("Socket sync: {0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name,
                            string.Format("Отправлено байт: {0} на сервер {1}, по порту {2}", data_arr.Length, server, port)), Logger.LogLevel.DEBUG);
                    // получаем ответ в виде слова
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data_arr, 0, data_arr.Length);
                        builder.Append(Encoding.Unicode.GetString(data_arr, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    // отправка команды на закрытие канала
                    /*try
                    {
                        byte[] datapart = SocketUtils.ObjectToByteArray(new SendObject("CLOSE"));
                        stream.Write(datapart, 0, datapart.Length);
                    }
                    catch(Exception ex)
                    {
                        Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    }*/
                    Logger.Log(string.Format("Socket sync: Передача завершена"), Logger.LogLevel.DEBUG);
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            finally
            {
                client?.Close();
            }
            return "";
        }

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
                    Logger.Log("SOCKET ADDRESS CNT: " + ipHost.AddressList.Count().ToString() + ", SERVER = " + server, Logger.LogLevel.EVENTS);
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
                Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            return res;
        }
    }
}
