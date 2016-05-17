using App3.Class;
using App3.Class.Static;
using App3.Controls;
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

namespace App3.Forms
{
    public partial class SearchObject : Form
    {
        public delegate void SelectObjectHandler(Int64 object_id);
        public SelectObjectHandler OnSelectObject = null;

        public string Number
        {
            get { return numberBox.Text; }
        }

        public SearchObject()
        {
            InitializeComponent();
        }

        private void SearchObject_Load(object sender, EventArgs e)
        {
            CancelButton = button2;
            DBDict.Load2Combobox(ref comboBox1,
                DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                null
            );
        }

        private void button1_Click(object sender, EventArgs e)
        {
            exListBox1.Items.Clear();
            int i = comboBox1.SelectedIndex;
            
            string reg = "0";
            int num = 0;
            if (i >= 0)
            {
                reg = ((ComboboxItem)comboBox1.Items[i]).Value.ToString();
            }
            if (!int.TryParse(numberBox.Text, out num))
            {
                num = 0;
            }
            List<object[]> rows = DataBase.RowSelect(
                        string.Format(@"SELECT * FROM (
                                    SELECT obj.osm_id, obj.number, obj.name, obj.region_id, obj.address_id,
                                            addr.address, 
                                            3*(obj.number = {0})::int + (obj.region_id = {1})::int + (obj.name like '%{2}%')::int as rate
                                    FROM oko.object obj 
                                        INNER JOIN oko.addresses addr ON obj.address_id = addr.id
                                        INNER JOIN regions2map reg ON reg.num = obj.region_id) t WHERE t.rate > 0 ORDER BY t.rate DESC LIMIT 30",
                        num,
                        reg,
                        nameBox.Text
                    )
                );
            foreach(object[] row in rows)
            {
                exListBox1.Items.Add(
                    new exListBoxItem(
                        row[0].ToInt(), 
                        string.Format("№{0} ({1})", row[1], row[2]),
                        string.Format("Адресс объекта: {0} ", row[5]),
                        App3.Properties.Resources.nophoto
                    )
                );
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void exListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            exListBoxItem selectedItem = null;
            int i = exListBox1.SelectedIndex;
            if (i >= 0)
            {
                selectedItem = (exListBoxItem)exListBox1.Items[i];
                if (OnSelectObject != null)
                {
                    Thread backgroundThread = new Thread(
                        new ThreadStart(() =>
                        {
                            OnSelectObject(selectedItem.Id);
                        }
                    ));
                    backgroundThread.IsBackground = true;
                    backgroundThread.Start();
                    Close();
                }
            }
        }

        private void numberBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1.PerformClick();
            }
        }

        private void nameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1.PerformClick();
            }
        }

    }
}
