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
        private bool isInit = false;
        private Thread listnerThread = null;

        private void ServerInit()
        {
            try
            {
                _server = new TcpListener(IPAddress.Any, _port);
                _server.Start();
                listnerThread = new Thread(new ThreadStart(ClientAccept));
                isInit = true;
                listnerThread.Start();
            }
            catch (ThreadAbortException ex)
            {
                // TODO
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        public void Stop()
        {
            _server?.Stop();
            listnerThread?.Abort();
        }

        private void ClientAccept()
        {
            try
            {
                Logger.Instance.WriteToLog("Start listner at port: " + _port.ToString());
                while (true)
                {
                    TcpClient client = _server.AcceptTcpClient();
                    // Logger.Instance.WriteToLog("SocketSync: Client Accept");
                    ClientObject clientObject = new ClientObject(client);
                    clientObject.onProcess += new ClientObject.ProcessDelegate(onProcess);
                    // создаем новый поток для обслуживания нового клиента
                    Thread clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (SocketException e)
            {
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", MethodBase.GetCurrentMethod().DeclaringType.Name, MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        public PortListner(int port)
        {
            _port = port;
            ServerInit();
        }

    }
}
