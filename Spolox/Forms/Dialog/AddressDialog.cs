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

namespace App3.Dialog
{
    public partial class AddressDialog : Form
    {
        public Address oAddress = new Address();

        public string GetAddress
        {
            get { return oAddress.FullAddress; }
        }

        public string GetCode
        {
            get { return oAddress.Code; }
        }

        public AddressDialog()
        {
            InitializeComponent();
        }

        private void RegionSelected()
        {
            regionBox.Enabled = false;
            districtBox.Enabled = true;

            oAddress.Region = regionBox.Items[regionBox.SelectedIndex].ToString();
            distBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            distBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            distBox.AutoCompleteCustomSource = oAddress.DistrictCollection();
            if (distBox.AutoCompleteCustomSource.Count == 1)
            {
                distBox.Text = distBox.AutoCompleteCustomSource[0];
                DistrictSelected();
            }
        }

        private void DistrictSelected()
        {
            distBox.Enabled = false;
            localityBox.Enabled = true;

            oAddress.District = distBox.Text;
            locBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            locBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            locBox.AutoCompleteCustomSource = oAddress.LocalityCollection();
            if (locBox.AutoCompleteCustomSource.Count == 1)
            {
                locBox.Text = locBox.AutoCompleteCustomSource[0];
                LocalitySelected();
            }
            if (locBox.AutoCompleteCustomSource.Count == 0)
            {
                locBox.Text = "В БД нет информации о населенных пунктах";
                LocalitySelected();
            }
        }

        private void LocalitySelected()
        {
            locBox.Enabled = false;
            streetBox.Enabled = true;

            oAddress.Locality = locBox.Text;
            strBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            strBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            strBox.AutoCompleteCustomSource = oAddress.StreetCollection();
            if (strBox.AutoCompleteCustomSource.Count == 1)
            {
                strBox.Text = strBox.AutoCompleteCustomSource[0];
                StreetSelected();
            }
            if (strBox.AutoCompleteCustomSource.Count == 0)
            {
                strBox.Text = "В БД нет информации об улицах";
                StreetSelected();
            }
        }

        private void StreetSelected()
        {
            strBox.Enabled = false;
            houseBox.Enabled = true;

            oAddress.Street = strBox.Text;
            hosBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            hosBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            hosBox.AutoCompleteCustomSource = oAddress.HouseCollection();
            if (hosBox.AutoCompleteCustomSource.Count == 1)
            {
                hosBox.Text = hosBox.AutoCompleteCustomSource[0];
                HouseSelected();
            }
            if (hosBox.AutoCompleteCustomSource.Count == 0)
            {
                hosBox.Text = "В БД нет информации о домах";
                HouseSelected();
            }
        }

        private void HouseSelected()
        {
            hosBox.Enabled = false;
            oAddress.House = hosBox.Text;
        }

        private void AddressForm_Load(object sender, EventArgs e)
        {
            regionBox.Items.Clear();
            DataSet dt = new DataSet();
            DataBase.RowSelect("select * from kladr.regions", dt);
            foreach(DataRow row in dt.Tables[0].Rows)
            {
                regionBox.Items.Add(string.Format("{0} {1}", row["name"], row["socrname"]));
            }
            
            if (regionBox.Items.Count == 1)
            {
                regionBox.SelectedIndex = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (houseBox.Text != "")
            {
                HouseSelected();
            }
            Close();
        }

        private void regionBox_SelectedValueChanged(object sender, EventArgs e)
        {
            RegionSelected();
        }

        private void distBox_Enter(object sender, EventArgs e)
        {
            
        }

        private void distBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                DistrictSelected();
            }
        }

        private void locBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                LocalitySelected();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void strBox_Enter(object sender, EventArgs e)
        {

        }

        private void strBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                StreetSelected();
            }
        }

        private void hosBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                HouseSelected();
            }
        }
    }
}
