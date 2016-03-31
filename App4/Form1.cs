using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App4
{
    
    public partial class Form1 : Form
    {
        private GuardAgent2.Module moduleGuard;
        // private MessageModuleEventHandler moduleEvents;

        public Form1()
        {
            InitializeComponent();
        }

        private void Log(string s)
        {
            listBox1.Items.Add(s);
        }

        private void ModuleEvents_GetModuleMessageEvent(GuardAgent2.Message msg)
        {
            Log(msg.Type);
        }

        public void asd(object arg)
        {
            Log("1");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            moduleGuard = new GuardAgent2.Module();
            moduleGuard.LocalAddress = "11";
            moduleGuard.ModuleId = 11;
            
            // moduleEvents = new MessageModuleEventHandler(); // new GuardAgent2.EventDelegate(asd);
            moduleGuard.GetModuleMessageEvent += new GuardAgent2.EventDelegate(GetModuleMessageEvent); //...guardTask.MessageReceived += new GUARDTASK.MessageReceivedDelegate(this.ReceiveMessage);

            uint ui = moduleGuard.Init("COM3", 19200);
            Log(ui.ToString());
            /*while (!moduleGuard.TestConnection())
            {
                Thread.Sleep(1000);
                if (ui == 4146) break;
            } */
            // uint ui = moduleGuard.Init("COM3", 9600);
            // moduleGuard.SendTestConnect();
            string s = moduleGuard.GetModemAddress();
            Log("Adress " + s);
            
            moduleGuard.ClearRetrAddrList(); //	очистка списка ретрансляции
            moduleGuard.SetRetrType(0); //,	где type – тип ретрансляции
            moduleGuard.AddRetrAddr(32); //	где addr1 – 1й адрес ретрансляции
            // moduleGuard.AddRetrAddr(addr2); //	где addr2 – 2й адрес ретрансляции
            moduleGuard.SetChannelsMask(255); //	где mask – маска каналов
            moduleGuard.SendAskForState(1, 0, 6910); //
            // moduleGuard.TestConnection();
            // moduleGuard.SendGuardTestConnect(32);
            

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduleGuard.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool b = moduleGuard.TestConnection();
            Log(b.ToString());
            // Log(moduleEvents.LastType());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /*bool false;
            moduleGuard.ClearRetrAddrList(); //	очистка списка ретрансляции
            moduleGuard.SetRetrType(0); //,	где type – тип ретрансляции
            moduleGuard.AddRetrAddr(0); //	где addr1 – 1й адрес ретрансляции
            // moduleGuard.AddRetrAddr(addr2); //	где addr2 – 2й адрес ретрансляции
            moduleGuard.SetChannelsMask(1); //	где mask – маска каналов
            moduleGuard.SendAskForState(0, 0, 32); //*/
        }

        public void GetModuleMessageEvent(object Message)
        {
            // throw new NotImplementedException();
            GuardAgent2.Message msg = (GuardAgent2.Message)Message;
            string Var1 = msg.Address;
            DateTime Var2 = msg.TimeStamp;
            object [] Var = new object[9];

            switch (msg.Type)
            {
                case "MESSAGE_OKO2_RM":
                    Var[0] = msg.Get("Attributes");
                    Var[1] = msg.Get("RadioRetr");
                    Var[2] = msg.Get("Object");
                    Var[3] = msg.Get("Class");
                    Var[4] = msg.Get("Code");
                    Var[5] = msg.Get("Part");
                    Var[6] = msg.Get("Zone");
                    Var[7] = msg.Get("Number");
                    Var[8] = msg.Get("ChannelsMask");
                    break;
                case "MESSAGE_OKO1_RM":
                    Var[0] = msg.Get("RetrNumber");
                    Var[1] = msg.Get("Code");
                    Var[2] = msg.Get("Object");
                    Var[3] = msg.Get("ChannelsMask");
                    break;
                case "MESSAGE_GUARD_RM":
                    Var[0] = msg.Get("Result");
                    Var[1] = msg.Get("Part");
                    Var[2] = msg.Get("Status");
                    Var[3] = msg.Get("Code");
                    Var[4] = msg.Get("User");
                    Var[5] = msg.Get("ChannelsMask");
                    break;
                case "MESSAGE_SYSTEM_RM":
                    Var[0] = msg.Get("Command");
                    switch((int)Var[0])
                    {
                        case 128:
                            Var[1] = msg.Get("Result");
                            break;
                        case 186:
                            Var[1] = msg.Get("Result");
                            Var[2] = msg.Get("Text");
                            break;
                    }
                    break;
            }
            string s = Var1 + ": =" + Var2 + "= : " + string.Join(",", Var);
            Logger.Instance.WriteToLog(s);
            Log(s);
        }
    }

    public class MessageModuleEventHandler : GuardAgent2.ModuleEvents
    {
        private int x = 0;
        private string lastStr = "";
        public MessageModuleEventHandler()
        {
            x = 1;
        }

        public void GetModuleMessageEvent(object Message)
        {
            // throw new NotImplementedException();
            x += 1;
            GuardAgent2.Message msg = (GuardAgent2.Message)Message;
            lastStr = msg.Address;
            Logger.Instance.WriteToLog(msg.Type + ": " + msg.Address + " " + msg.TimeStamp);
        }

        public string LastType()
        {
            return lastStr;
        }
    }

}
