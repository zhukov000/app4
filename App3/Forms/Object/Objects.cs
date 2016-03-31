using App3.Class;
using App3.Class.Static;
using App3.Forms.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms.Object
{
    public partial class Objects : Form
    {
        private Controls.DBTable objectTable;

        private void UpdStatus()
        {
            toolStripStatusLabel1.Text = string.Format("Строк выбрано: {0}", objectTable.RowCount);
        }

        private void InitTable()
        {
            this.objectTable = new App3.Controls.DBTable();
            this.panel2.Controls.Add(this.objectTable);
            // 
            // objectTable
            // 
            this.objectTable.CanAdd = false;
            this.objectTable.CanDel = false;
            this.objectTable.CanEdit = false;
            this.objectTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.objectTable.Filter = "";
            this.objectTable.Location = new System.Drawing.Point(0, 0);
            this.objectTable.Name = "objectTable";
            this.objectTable.Size = new System.Drawing.Size(823, 226);
            this.objectTable.TabIndex = 0;
            this.objectTable.TableName = "oko.object";
            this.objectTable.AddHidden2("way");
            this.objectTable.AddHidden2("address_id");
            this.objectTable.AddHidden2("region_id");
            this.objectTable.AddHidden2("tstate_id");
            this.objectTable.AddHidden2("osm_id");
            this.objectTable.AddHidden2("autocontrol");
            this.objectTable.AddHidden2("autointerval");
            this.objectTable.AddHidden2("real_object");
            this.objectTable.AddHidden2("customer_id");
            // внешние ключи
            this.objectTable.AddForeign("tstatus_id", "oko.tstatus", "id", "status");
            //
            this.objectTable.SetColumnSize(new int[] { 100, 100, 350, 100, 100, 100 });
            this.objectTable.Refresh();

            this.objectTable.TableDoubleClick += this.dbTable1_DoubleClick;
            UpdStatus();
        }

        private void dbTable1_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (objectTable.ColumnName(e.ColumnIndex) == "number")
            {
                long id = this.objectTable.Value(e.RowIndex, "osm_id").ToInt64();
                Handling.onObjectCardOpen(id);
            }
        }

        public Objects(Form pParent)
        {
            InitializeComponent();
            InitTable();
            this.MdiParent = pParent;
        }

        private void Objects_Load(object sender, EventArgs e)
        {
            DBDict.Load2Combobox(ref comboBox1,
                DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                null
            );
        }

        private void Objects_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        public void SetFilter(string Filter)
        {
            int i = Filter.IndexOf("region_id");
            if(i >= 0)
            {
                i = Filter.IndexOf("=", i + "region_id".Length) + 1;
                string s = "";
                while ((Filter[i] == ' ') || (Filter[i] >= 0 && Filter[i] <= 9))
                {
                    if (Filter[i] != ' ')
                        s += Filter[i];
                    i++;
                }
                i = s.ToInt();
                if (i > 0)
                {
                    DBDict.Load2Combobox(ref comboBox1,
                        DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                        i
                    );
                }
                
            }
            ///
            objectTable.Filter = Filter;
            this.objectTable.Sorting("number");
            objectTable.Refresh();
            UpdStatus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ComboboxItem item = (ComboboxItem)comboBox1.SelectedItem;
            if (item != null)
            {
                this.SetFilter( String.Format("region_id = {0}", item.Value) );
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            objectTable.UpdateData();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var wd = Utils.CreateWaitThread(this);
            ReportDialog frm = new ReportDialog();
            string sql = @"select number, name, makedatetime, 
                                case when dogovor then 'Есть' else 'Нет' end as dogovor, 
                                tstatus.status, rm.region, lm.message, lm.datetime as messagetime
                            from oko.object
                            inner join oko.tstatus on tstatus.id = object.tstatus_id
                            inner join (select num as id_region, fullname as region from regions2map) rm on rm.id_region = object.region_id
                            left join (
	                            select a.objectnumber, a.datetime, a.message, a.region_id as reid
	                            from oko.event_messages as a 
	                            LEFT JOIN oko.event_messages as b
		                            ON a.objectnumber = b.objectnumber AND a.region_id = b.region_id AND a.datetime < b.datetime
	                            WHERE b.objectnumber IS NULL
                            ) lm on lm.objectnumber = object.number and lm.reid = region_id ";
            if (objectTable.Filter != "")
            {
                sql += " WHERE " + objectTable.Filter;
            }
            frm.ShowReport(sql, "App3.Reports.ListObject.rdlc");
            Utils.DestroyWaitThread(wd);
            frm.ShowDialog();
        }

        
    }
}
