using App3.Class.Singleton;
// using Communication.Sockets.Core.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;

namespace App3.Class.Socket2
{
    /// <summary>
    /// Класс, обеспечивающий открытия прослушки на некотором порте
    /// </summary>
    class PortListner
    {
        /// <summary>
        /// Вызывается при получении объекта
        /// </summary>
        static public ClientObject.ProcessDelegate onProcess = null;

        private int _port = 0;
        private TcpListener _server = null;
        private Thread listnerThread = null;

        private void ServerInit()
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, _port);
                _server.Start();
                Logger.Log("Start listner at port: " + _port.ToString(), Logger.LogLevel.EVENTS);
                listnerThread = new Thread(new ThreadStart(ClientAccept));
                listnerThread.Start();
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
        }

        public void Stop()
        {
            _server?.Stop();
            listnerThread?.Abort();
        }

        /// <summary>
        /// Вызывается для обработки полученных соединений
        /// </summary>
        private void ClientAccept()
        {
            try
            {
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    ClientObject clientObject = new ClientObject(client);
                    // метод, который реализует дополнительную обработку полученных данных
                    clientObject.onProcess = new ClientObject.ProcessDelegate(onProcess);
                    // создаем новый поток для обслуживания нового клиента
                    new Thread(new ThreadStart(clientObject.Process)).Start();
                }
            }
            catch (SocketException e)
            {
                Logger.Log(string.Format("SOCKET {0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, e.Message), Logger.LogLevel.ERROR);
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            finally
            {
                _server?.Stop();
            }
        }

        public PortListner(int port)
        {
            _port = port;
            ServerInit();
        }

    }
}
