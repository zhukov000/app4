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

namespace App2
{
    public partial class Form1 : Form
    {
        private const string CONNECTION_STRING = "PG:host=localhost dbname=openstreetmap user=postgres password=postgres";

        private OgrDatasource DS = new OgrDatasource();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var layer = new OgrLayer();
            
            if (layer.OpenFromDatabase(CONNECTION_STRING, "planet_osm_roads"))
            {
                axMap1.RemoveAllLayers();
                int layerHandle = axMap1.AddLayer(layer, true);
                Shapefile sf = axMap1.get_Shapefile(layerHandle);
                sf.Labels.Generate("[Type]", tkLabelPositioning.lpCenter, false);
                sf.Labels.FrameVisible = true;
                sf.Labels.FrameType = tkLabelFrameType.lfRectangle;

            }
            else
            {
                MessageBox.Show("Failed to open: " + layer.get_ErrorMsg(layer.LastErrorCode));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            axMap1.RemoveAllLayers();
            if (!DS.Open(CONNECTION_STRING))
            {
                MessageBox.Show("Failed to establish connection: " + DS.GdalLastErrorMsg);
            }
            else
            {
                int count = DS.LayerCount;
                checkedListBox1.Items.Clear();
                axMap1.RemoveAllLayers();
                for (int i = 0; i < count; i++)
                {
                    var lyr = DS.GetLayer(i);
                    checkedListBox1.Items.Add(lyr.Name);
                    axMap1.AddLayer(lyr, true);
                    //lyr.Close();
                }
                DS.Close();
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           /* if (!DS.Open(CONNECTION_STRING))
            {
                MessageBox.Show("Failed to establish connection: " + DS.GdalLastErrorMsg);
            }
            else
            {
                checkedListBox1.Enabled = false;
                axMap1.RemoveAllLayers();
                foreach (int indexChecked in checkedListBox1.CheckedIndices)
                {
                    axMap1.AddLayer(DS.GetLayer(indexChecked), true);
                }
                checkedListBox1.Enabled = true;
            }
            DS.Close();*/
        }
    }
}
