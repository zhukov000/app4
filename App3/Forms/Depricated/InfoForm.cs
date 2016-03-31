using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class InfoForm : Form
    {
        public InfoForm(Form Parent)
        {
            InitializeComponent();
            MdiParent = Parent;
            FillDistrictsCombo();
        }

        private void InfoForm_Load(object sender, EventArgs e)
        {
            // this.Width = (Parent.Width * 0.25);
            // this.Height = Parent.Height - 5;
        }

        public void SetText(string Txt)
        {
            label1.Text = Txt;
        }

        public void SetAlarm(string Txt)
        {
            if (label2.InvokeRequired)
                label2.Invoke(new Action(() => label2.Text = Txt));
            else
                label2.Text = Txt;
            
            if (groupBox4.InvokeRequired)
                groupBox4.Invoke(new Action(() => groupBox4.Visible = true));
            else
                groupBox4.Visible = true;
        }

        private void FillDistrictsCombo()
        {
            DataSet ds=new DataSet();
            DataBase.RowSelect("select distinct fullname, num, name from public.regions order by name", ds);
            if(ds.Tables[0].Rows.Count > 0)
            {
                comboBox1.Items.Clear();
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    comboBox1.Items.Add(string.Format("{0} (:{1})", row["fullname"], row["num"]));
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            
            ((MainForm)MdiParent).ShowDistrict(
                new string(s.Where(c => char.IsLetter(c) || c == ' ' || c == '-').ToArray()).Trim()
            );
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            // ((MainForm)MdiParent).ChangeDistrictScale(Convert.ToDouble(comboBox2.Text));
        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {
            
        }

        private void label2_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }
    }
}
