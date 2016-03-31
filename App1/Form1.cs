using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapWinGIS;
using System.Diagnostics;

namespace App1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string connectionString = "PG:host=localhost dbname=openstreetmap user=postgres password=postgres";
            var layer = new OgrLayer();
            if (layer.OpenFromDatabase(connectionString, "buildings"))
            {
                int layerHandle = axMap1.AddLayer(layer, true);
            }
            else
            {
                MessageBox.Show("Failed to open: " + layer.get_ErrorMsg(layer.LastErrorCode));
            }
        }
    }
}
