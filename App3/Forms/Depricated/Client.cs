using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App3.Class;

namespace App3.Forms
{
    public partial class Client : Form
    {
        OKOGate.Module oModule = new OKOGate.Module();

        public Client()
        {
            InitializeComponent();
            oModule.LogLevel = OKOGate.Tracer.eLogLevel.DEBUG;
            oModule.Protocol = OKOGate.Module.PROTOCOL.XML_GUARD;

            oModule.RemotePort = Config.Get("ModuleLocalServerPort").ToInt();
            oModule.LocalServerIP = Config.Get("ModuleLocalServerIP");
            oModule.LocalServerPort = Config.Get("ModuleLocalServerPort").ToInt();
            oModule.LocalGUID = Config.Get("ModuleLocalGUID");
            oModule.ModuleId = Config.Get("ModuleModuleId").ToInt() + 1;

            oModule.StartModule();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OKOGate.Message msg = new OKOGate.Message(textBox1.Text);
            msg.Address = "89.16.96.72";
            XMLReader.MessagePacker.Pack(msg);
            oModule.SendMessage(msg);
        }

        private void Client_FormClosing(object sender, FormClosingEventArgs e)
        {
            oModule.StopModule();
        }
    }
}
