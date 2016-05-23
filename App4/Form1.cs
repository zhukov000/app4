using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
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
            // listBox1.Items.Add(s);
            // .Invoke(new Action(() => { oObjectList.SetFilter(Filter); }));
            listBox1.BeginInvoke(new Action(() => { listBox1.Items.Add(s); }));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            moduleGuard = new GuardAgent2.Module();
            moduleGuard.LocalAddress = "11";
            moduleGuard.ModuleId = 11;

            string [] l = SerialPort.GetPortNames();
            foreach(string s in l)
            {
                comboBox1.Items.Add(s);
            }
            if (comboBox1.Items.Count > 1)
                comboBox1.SelectedIndex = 1;
            else if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            moduleGuard.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool b = moduleGuard.TestConnection();
            Log(b.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            moduleGuard.Close();

            moduleGuard.GetModuleMessageEvent += new GuardAgent2.EventDelegate(GetModuleMessageEvent); 

            string pname = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            Log("Connect to " + pname);
            uint ui = moduleGuard.Init(pname, 19200);
            Log(OKO_Messages.Message(ui));
            string s = moduleGuard.GetModemAddress();
            Log("Adress " + s);

            moduleGuard.ClearRetrAddrList(); //	очистка списка ретрансляции
            moduleGuard.SetRetrType(0); //,	где type – тип ретрансляции
            moduleGuard.AddRetrAddr(32); //	где addr1 – 1й адрес ретрансляции
            moduleGuard.SetChannelsMask(255); //	где mask – маска каналов
            moduleGuard.SendAskForState(1, 0, 6910); //
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
            Log( msg.Type + ": " + s);
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
