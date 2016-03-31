using App3.Class;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms.Dialog
{
    public partial class ContactDialog : Form
    {
        private int CType = 1;

        public string Value
        {
            get
            {
                return textBox1.Text;
            }
        }
        public int TypeContact
        {
            get
            {
                var item = (ComboboxItem)comboBox1.SelectedItem;
                object tcontact = item.Value;
                return tcontact.ToInt();
            }
        }
        public string Description
        {
            get
            {
                return textBox2.Text;
            }
        }

        public ContactDialog()
        {
            InitializeComponent();
        }

        public ContactDialog(int pCType, string pValue, string pDescription)
        {
            InitializeComponent();
            CType = pCType;
            textBox2.Text = pDescription;
            textBox1.Text = pValue;
        }

        private void ContactDialog_Load(object sender, EventArgs e)
        {
            DBDict.Load2Combobox(
                ref comboBox1,
                DBDict.TContact.Select(x => new ComboboxItem(x.Value, x.Key)).ToList(),
                CType
            );
        }
    }
}
