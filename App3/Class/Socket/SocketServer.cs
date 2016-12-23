using App3.Class.Singleton;
using System;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace App3.Class.Socket
{
    /**
     * Сервер - получает сообщения о сработках от клиентов
     * Сообщения приходят в формате SendObject
     */
    class SocketServer
    {
        private static Thread back = null;
        public delegate void GetObjectDelegate(SendObject data);
        public static GetObjectDelegate onGetObjectDelegate = null;

        private static string _server = "";
        private static int _port = 0;

        static public void StopListen()
        {
            if (back != null)
            {
                back.Abort();
            }
            /*
            IPHostEntry ipHost = Dns.GetHostEntry(_server);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, _port);
            System.Net.Sockets.Socket sender = new System.Net.Sockets.Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sender.Connect(ipEndPoint);
            SendObject data = new SendObject("END");
            int bytesSent = sender.Send(SocketUtils.ObjectToByteArray(data));
            */
        }

        static public void StartListen(string server, int port)
        {
            _server = server;
            _port = port;
            back = new Thread(
                new ThreadStart(() =>
                {
                    try
                    {
                        IPHostEntry ipHost = Dns.GetHostEntry(server);
                        IPAddress ipAddr = ipHost.AddressList[0];
                        IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                        // Создаем сокет Tcp/Ip
                        System.Net.Sockets.Socket sListener = new System.Net.Sockets.Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    
                        sListener.Bind(ipEndPoint);
                        sListener.Listen(10);
                        while (true)
                        {
                            // Программа приостанавливается, ожидая входящее соединение
                            System.Net.Sockets.Socket handler = sListener.Accept();
                            SendObject data = null;

                            // Мы дождались клиента, пытающегося с нами соединиться
                            byte[] bytes = new byte[Config.Get("SocketPackageSize").ToInt()];
                            int bytesRec = handler.Receive(bytes);
                            byte[] byteData = new byte[bytesRec];
                            Array.Copy(bytes, byteData, bytesRec);

                            data = (SendObject)SocketUtils.ByteArrayToObject(byteData);
                            // получили данные
                            if (data.Message == "END")
                            {
                                handler.Send(SocketUtils.ObjectToByteArray(data));
                                handler.Shutdown(SocketShutdown.Both);
                                handler.Close();
                                break;
                            }
                            // вызов метода
                            if (onGetObjectDelegate != null)
                            {
                                onGetObjectDelegate(data);
                            }
                            else
                            {
                                Logger.Instance.WriteToLog("SOCKET Recived: " + data.Message);
                            }
                            // посылаем сообщение о том, что данные получены
                            data = new SendObject("ACCEPTED");
                            handler.Send(SocketUtils.ObjectToByteArray(data));
                            handler.Shutdown(SocketShutdown.Both);
                            handler.Close();
                        }
                    }
                    catch (ThreadAbortException ex)
                    {
                        
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
                    }
                })
            );
            back.IsBackground = true;
            back.Start();
        }
    }
}
