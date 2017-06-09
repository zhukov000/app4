using App3.Class;
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
    public partial class ComboDialog : Form
    {
        public ComboboxItem SelectedItem
        {
            get 
            {
                return (ComboboxItem)comboBox1.SelectedItem;
            }
        }

        public ComboDialog()
        {
            InitializeComponent();
        }

        public ComboDialog(string pTitle, string pText, List<ComboboxItem> pList, int pSelectedIdx = 0)
        {
            InitializeComponent();
            Text = pTitle;
            label1.Text = pText;
            foreach (ComboboxItem item in pList)
            {
                comboBox1.Items.Add(item);
            }
            comboBox1.SelectedItem = comboBox1.Items[pSelectedIdx];
        }
    }
}
