using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor
{
    public partial class SpoloxNode : UserControl
    {
        public bool IsOn
        {
            get
            {
                return panelOn.Visible;
            }
            set
            {
                panelOn.Visible = value;
                panelOff.Visible = !value;
            }
        }

        public string NodeName
        {
            get
            {
                return label1.Text;
            }
        }

        public string Ip
        {
            get
            {
                return node.Ip;
            }
        }

        public int Port
        {
            get
            {
                return node.Port;
            }
        }

        private CompNode node;

        private void SetNode(int id, string ip, int port, string rn)
        {
            node = new CompNode(id, ip, port);
            label1.Text = rn;
            toolTip1.ToolTipTitle = rn;
        }

        public SpoloxNode()
        {
            InitializeComponent();
        }

        public SpoloxNode(int id, string ip, int port, string rn, bool status = false)
        {
            InitializeComponent();
            SetNode(id, ip, port, rn);            
            IsOn = status;
        }
    }

    public class CompNode
    {
        private int id;
        private string ip;
        private int port;

        public int Id
        {
            get { return id; }
        }
        public string Ip
        {
            get { return ip; }
        }
        public int Port
        {
            get { return port; }
        }
        public CompNode(int pid, string pip, int pport)
        {
            id = pid;
            ip = pip;
            port = pport;
        }
    }
}
