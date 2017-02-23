using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Class
{
    /// <summary>
    /// Работа с адресной системой
    /// </summary>
    public class Address
    {
        enum ADDRESS_LEVEL {
            REGION = 1,
            DISTRICT = 2,
            LOCALITY = 3,
            STREET = 4
        }

        #region Скрытые поля
        private int id = 0;
        /// <summary>
        /// Названия региона, например "Ростовская область"
        /// </summary>
        private string region = "";
        /// <summary>
        /// Код региона в системе КЛАДР
        /// </summary>
        private string rCode = "";
        /// <summary>
        /// Название района, например "Целинский район", возможно городской округ "город Шахты"
        /// </summary>
        // private string district = ""; 
        private int districtId = -1;
        /// <summary>
        /// Код района в системе КЛАДР
        /// </summary>
        private string dCode = "";
        /// <summary>
        /// Название населенного пункта (для городского округа совпадает)
        /// </summary>
        private string locality = "";
        /// <summary>
        /// Код населенного пункта в системе КЛАДР
        /// </summary>
        private string lCode = "";
        /// <summary>
        /// Название улицы
        /// </summary>
        private string street = "";
        /// <summary>
        /// Код улицы в системе КЛАДР
        /// </summary>
        private string sCode = "";
        /// <summary>
        /// Номер дома
        /// </summary>
        private string house = "";
        /// <summary>
        /// Код дома в системе КЛАДР
        /// </summary>
        private string hCode = "";
        /// <summary>
        /// Строка адреса
        /// </summary>
        private string address = "";
        private string code = "";

        private string latitude = "";
        private string longitude = "";
        #endregion

        #region Открытые свойства
        /// <summary>
        /// Регион (ростовская область)
        /// </summary>
        public string Region
        {
            get { return region; }
            set
            {
                address = ""; code = "";
                region = value;
                rCode = DataBase.First(
                    string.Format("SELECT DISTINCT code FROM kladr.regions WHERE name || ' ' || socrname = '{0}'", region),
                    "code"
                ).ToString();

            }
        }

        public int Id
        {
            get { return id;  }
        }

        public int DistrictId
        {
            get 
            {
                return districtId;
            }
        }

        public string District
        {
            get
            {
                return DataBase.First("select name from regions2map where num = " + districtId, "name").ToString();
            }
        }

        /// <summary>
        /// Населенный пункт
        /// </summary>
        public string Locality
        {
            get { return locality; }
            set
            {
                address = "";
                locality = value;
                if (value != "")
                {
                    code = "";
                    object o = DataBase.First(
                        string.Format("SELECT DISTINCT code FROM kladr.locality WHERE fullname = '{0}'", locality),
                        "code"
                    );
                    if (o!= null)
                    {
                        lCode = o.ToString();
                    }
                }
            }
        }
        /// <summary>
        /// Улица
        /// </summary>
        public string Street
        {
            get { return street; }
            set
            {
                address = ""; 
                street = value;
                if (value != "")
                {
                    code = "";
                    object o = DataBase.First(
                        string.Format("select distinct s.code from kladr.street s left join kladr.socrbase b on s.socr = b.scname WHERE lower(b.socrname) || ' ' || s.name = '{0}' and s.code like '{1}'", street, SqlLikeMask(lCode)),
                        "code"
                    );
                    if (o != null) 
                        sCode = o.ToString();
                }
            }
        }
        /// <summary>
        /// Дом
        /// </summary>
        public string House
        {
            get { return house; }
            set
            {
                address = "";
                house = value;
                if (value != "")
                {
                    code = "";
                    hCode = DataBase.First(
                        string.Format("select distinct code from kladr.house where fullname = '{0}'", house),
                        "code"
                    ).ToString();
                }
            }
        }
        /// <summary>
        /// Код КЛАДР для адреса
        /// </summary>
        public string Code
        {
            set
            {
                code = value;
                hCode = value;
                if (code != "")
                {
                    rCode = StrToMask(value, ADDRESS_LEVEL.REGION);
                    dCode = StrToMask(value, ADDRESS_LEVEL.DISTRICT);
                    lCode = StrToMask(value, ADDRESS_LEVEL.LOCALITY);
                    sCode = StrToMask(value, ADDRESS_LEVEL.STREET);
                }
            }
            get 
            {
                if (code == "")
                {
                    if (hCode != "")
                    {
                        code = hCode;
                    }
                    else if (sCode != "")
                    {
                        code = sCode;
                    }
                    else if (lCode != "")
                    {
                        code = lCode;
                    }
                    else if (dCode != "")
                    {
                        code = dCode;
                    }
                    code = rCode;
                }
                return code;
            }
        }
        /// <summary>
        /// Получение полного адреса
        /// </summary>
        public string FullAddress
        {
            get
            {
                if (address == "")
                { // строку полного адреса требуется обновить
                    string s = Region;
                    if (District != "") s = s + ", " + District;
                    if (Locality != "") s = s + ", " + Locality;
                    if (Street != "") s = s + ", " + Street;
                    if (House != "") s = s + ", " + House;
                    address = s;
                }
                return address;
            }
        }
        #endregion

        #region Конструкторы
        public Address()
        {
            // DO NOTHING
        }

        /// <summary>
        /// Создание адреса на основании информации в БД
        /// </summary>
        /// <param name="pIdAddress"></param>
        public Address(int pIdAddress)
        {
            Load4DB(pIdAddress);
            /*
            DataRow row = DataBase.FirstRow(
                string.Format("select * from oko.addresses where id = {0}", pIdAddress)
            );
            if (row != null)
            {
                id = pIdAddress;
                Region = row["region"].ToString();
                District = Address.RegionById(row["district"].ToInt());
                Locality = row["locality"].ToString();
                Street = row["street"].ToString();
                House = row["house"].ToString();
                address = row["address"].ToString();
                code = row["code"].ToString();
            }*/
        }

        /// <summary>
        /// Создание адреса по основным адресным элементам
        /// </summary>
        /// <param name="pRegion"></param>
        /// <param name="pDistrict"></param>
        /// <param name="pLocality"></param>
        /// <param name="pStreet"></param>
        /// <param name="pHouse"></param>
        public Address(string pRegion, string pDistrict, string pLocality, string pStreet, string pHouse)
        {
            Region = pRegion;
            districtId = DataBase.First("select num from regions2map where name = " + pDistrict, "num").ToInt();
            Locality = pLocality;
            Street = pStreet;
            House = pHouse;
        }

        public Address(string pRegion, int pDistrictId, string pLocality, string pStreet, string pHouse)
        {
            Region = pRegion;
            districtId = pDistrictId;
            Locality = pLocality;
            Street = pStreet;
            House = pHouse;
        }

        #endregion

        #region Методы

        public string GetStreet(string KladrCode)
        {
            object o = DataBase.First(string.Format("SELECT name FROM kladr.street WHERE CODE = '{0}'", KladrCode), "name");
            return o.ToString();
        }

        public string GetLocality(string KladrCode)
        {
            object o = DataBase.First(string.Format("SELECT fullname FROM kladr.locality WHERE CODE = '{0}'", StrToMask(KladrCode, ADDRESS_LEVEL.LOCALITY)), "fullname");
            return o.ToString();
        }

        public string GetDistrict(string KladrCode)
        {
            object o = DataBase.First(string.Format("SELECT fullname FROM kladr.districts WHERE CODE = '{0}'", StrToMask(KladrCode, ADDRESS_LEVEL.DISTRICT)), "fullname");
            return o.ToString();
        }

        public string GetRegion(string KladrCode)
        {
            object o = DataBase.First(string.Format("SELECT fullname FROM kladr.regions WHERE CODE = '{0}'", StrToMask(KladrCode, ADDRESS_LEVEL.REGION)), "fullname");
            return o.ToString();
        }

        #endregion

        #region Методы класса

        internal void Save2DB()
        {
            IDictionary<string, object> fields = new Dictionary<string, object>()
            {
                { "code", code.Q() },
                { "locality", locality.Q() },
                { "street", street.Q() },
                { "house", house.Q() },
                { "region", region.Q() },
                { "address", address.Q() },
                { "lat", latitude.Q() },
                { "lon", longitude.Q() },
                { "district", DistrictId }
            };
            if (id != 0)
            {
                DataBase.RunCommandUpdate("oko.addresses", fields, new Dictionary<string, object>() { { "id", id } });
            }
            else
            {
                object[] res;
                DataBase.RunCommandInsert("oko.addresses", fields, "id", out res);
                id = res[0].ToInt();
            }
        }

        private void Load4DB(DataRow row)
        {
            if (row != null)
            {
                Code = row["code"].ToString();
                region = row["region"].ToString();
                districtId = row["district"].ToInt();
                locality = row["locality"].ToString();
                street = row["street"].ToString();
                house = row["house"].ToString();
                address = row["address"].ToString();
                latitude = row["lat"].ToString();
                longitude = row["lon"].ToString();
            }
        }

        internal void Load4DB(int address_id)
        {
            id = address_id;
            DataRow row = DataBase.FirstRow(
                string.Format(
                    "SELECT * FROM oko.addresses WHERE id = {0}", id
                )
            );
            Load4DB(row);
        }

        /// <summary>
        /// !!! DEPRICATED !!!!
        /// Наименование района по его ID
        /// </summary>
        /// <param name="pIdRegion"></param>
        /// <returns></returns>
        public static string RegionById(int pIdRegion)
        {
            return DataBase.First(
                    "SELECT name, fullname FROM regions2map WHERE num = " + pIdRegion,
                    "fullname"
                ).ToString();
        }

        /// <summary>
        /// Название адрессного объекта по коду КЛАДР
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        public static string NameByCode(string Code)
        {
            return DataBase.First(string.Format("select distinct name from kladr.kladr where code = '{0}'", Code), "name").ToString();
        }

        #endregion
    
        #region Методы автодополнения

        public AutoCompleteStringCollection HouseCollection()
        {
            AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("select distinct fullname, name, code, korp from kladr.house WHERE code like '{0}'", SqlLikeMask(sCode)), ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(row["fullname"].ToString());
            }
            return ret;
        }

        public AutoCompleteStringCollection StreetCollection()
        {
            AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("select distinct lower(b.socrname) socrname, s.name, s.code from kladr.street s left join kladr.socrbase b on s.socr = b.scname WHERE code like '{0}'", SqlLikeMask(lCode)), ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(string.Format("{0} {1}", row["socrname"], row["name"]));
            }
            return ret;
        }

        public AutoCompleteStringCollection LocalityCollection()
        {
            AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("SELECT DISTINCT fullname, socrname, name, code FROM kladr.locality WHERE code like '{0}'", SqlLikeMask(dCode)), ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                // ret.Add(string.Format("{0} {1}", row["socrname"], row["name"]));
                ret.Add(row["fullname"].ToString());
            }
            return ret;
        }

        public AutoCompleteStringCollection DistrictCollection()
        {
            AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("SELECT fullname as name FROM kladr.districts"), ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(row["name"].ToString());
            }
            return ret;
        }

        #endregion

        #region Скрытые методы

        private static string SqlLikeMask(string Code)
        {
            return Code.TrimEnd('0') + '%';
        }

        private static string StrToMask(string Code, ADDRESS_LEVEL lvl)
        {
            int codeLen = 13;
            if (Code.Length > codeLen)
            {
                codeLen = Code.Length;
            }
            string ret = "0000000000000";
            switch(lvl)
            {
                case ADDRESS_LEVEL.REGION:
                    ret = Code.Substring(0, 2).PadRight(codeLen, '0');
                    break;
                case ADDRESS_LEVEL.DISTRICT:
                    string s = Code.Substring(2, 3);
                    if (s == "000")
                    {
                        ret = Code.Substring(0, 8);
                    }
                    else
                    {
                        ret = Code.Substring(0, 5);
                    }
                    ret = ret.PadRight(codeLen, '0');
                    break;
                case ADDRESS_LEVEL.LOCALITY:
                    ret = Code.Substring(0, 11).PadRight(codeLen, '0');
                    break;
                case ADDRESS_LEVEL.STREET:
                    ret = Code.Substring(0, 13);
                    break;
                default:
                    ret = Code;
                    break;
            }

            return ret;
        }

        #endregion


    }
}
