using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OKOGate;

namespace App0
{
    class Program
    {
        static void Main(string[] args)
        {
            Module oModule = new Module();
            oModule.LogLevel = Tracer.eLogLevel.DEBUG;
            oModule.Protocol = Module.PROTOCOL.XML_GUARD;
            oModule.RemotePort = 30003;
            oModule.LocalServerIP = "89.16.96.72";
            oModule.LocalServerPort = 30002;
            oModule.LocalGUID = "54665703-E19F-424C-B4CF-F7A81EFB0E13";
            oModule.ModuleId = 15;
            oModule.GetModuleMessageEvent += ReciveMessage;

            oModule.StartModule();
            oModule.StartReceive();
            
            Console.Title = "Press any key for CLOSE";
            Console.ReadKey();

            oModule.StopReceive();
            oModule.StopModule();
        }

        // public delegate bool SendMessageDelegate(Message msg);

        static void ReciveMessage(object arg)
        {
            Message msg = (Message)arg;
            if (msg.Type == "MESSAGE_PULT_OKOGATE")
            {
                Console.WriteLine("Message Recived: Address = {0} Content = {1} Id = {2} Text = {3} Type = {4}", msg.Address, msg.Content, msg.Id, msg.Text, msg.Type);
            }
        }
    }
}
