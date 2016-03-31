using App3.Class.Singleton;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    public class AKObject
    {
        private const string OTable = "oko.object";

        #region Поля
        private Int64 id = 0;
        private int region_id = 0;
        private double latitude = 0;
        private double longitude = 0;
        
        // private int state = 1;
        // private int status = 1;
        // private int tministry = 0;
        // private int tclass_id = 0;
        private int tstate_id = 0;
        private int tstatus_id = 0;
        private bool dogovor = false;
        private int real_object = 0;
        private int customer_id = 0;

        private Address address = new Address();

        public int number = 0;        
        public string name = "";
        // public string policedepartment = "";
        // public string weakspots = "";
        // public int armstatenumber = 0;
        public string makedatetime = "";
        // public string activatedtatetime = "";
        public string devicetypename = "";
        public bool autocontrol = true;
        public int autointerval = 0;
        
        public List<int> ClassCodes = new List<int>();
        
        #endregion

        #region Свойства

        public int Customer
        {
            get { return customer_id; }
            set
            {
                customer_id = value;
            }
        }

        public string CustomerName
        {
            get 
            {
                string s = "";
                DBDict.TCustomer.TryGetValue(customer_id, out s);
                return s;
            }
        }

        public int RealObject
        {
            get { return real_object; }
            set
            {
                real_object = value;
            }
        }

        public string RealObjectName
        {
            get 
            {
                string s = "";
                DBDict.TRealObject.TryGetValue(real_object, out s);
                return s;
            }
        }

        public bool Dogovor
        {
            get { return dogovor; }
            set
            {
                dogovor = value;
            }
        }

        /*public int TClass
        {
            get
            {
                return tclass_id;
            }
            set
            {
                tclass_id = value;
            }
        }*/

        public int RegionId
        {
            get 
            {
/*                if (address.DistrictId > 0)
                    return address.DistrictId;
                else*/
                    return region_id; 
            }
        }

        public Color OColor
        {
            get
            {
                return System.Drawing.ColorTranslator.FromHtml(DBDict.TState[TState].Color);
            }
        }

        public int TState
        {
            get
            {
                return tstate_id;
            }
            set
            {
                tstate_id = value;
            }
        }

        public string Password
        {
            set
            {
                if (IsExists())
                {
                    DataBase.RunCommand(string.Format("UPDATE {0} SET password = '{1}' WHERE osm_id = {2}", OTable, value, id));
                }
                else
                {
                    throw new Exception("Нельзя сохранить пароль для объекта, который не был добавлен в БД. Сначала сохраните объект!");
                }
            }
        }

        public Int64 Id
        {
            get { return id; }
        }

        public string DistrictName
        {
            get
            {
                if (address != null)
                {
                    return address.District;
                }
                return null;
            }
        }

        public string AddressStr
        {
            get { return address.FullAddress; }
        }

        public string AddressCode
        {
            get { return address.Code; }
        }

        public int TStatus
        {
            get { return tstatus_id; }
            set
            {
                tstatus_id = value;
            }
        }
        
        #endregion

        #region Конструкторы

        public AKObject()
        {

        }

        public List<KeyValuePair<string, int>> Properties()
        {
            List<KeyValuePair<string, int>> ret = new List<KeyValuePair<string, int>>();
            foreach (int code in ClassCodes)
            {
                ret.Add(new KeyValuePair<string, int>(String.Format("({1}):{0}", DBDict.TClassify[code].Item3, DBDict.TClassify[code].Item4), code));
            }
            return ret;
        }

        public void ObjectProperties()
        {
            ClassCodes.Clear();
            foreach (object[] row in DataBase.RowSelect("SELECT property_id FROM oko.object_properties WHERE object_id = " + id))
            {
                ClassCodes.Add(row[0].ToInt());
            }
        }

        public void AddProperties(int property_id)
        {
            DataBase.RunCommand(String.Format("INSERT INTO oko.object_properties(object_id, property_id) VALUES({0},{1})", id, property_id));
            ClassCodes.Add(property_id);
        }

        public void DelProperties(int property_id)
        {
            DataBase.RunCommand(String.Format("DELETE FROM oko.object_properties WHERE object_id={0} and property_id={1}", id, property_id));
            ClassCodes.Remove(property_id);
        }

        /// <summary>
        /// Создать экземпляр для объекта по его ID
        /// </summary>
        /// <param name="pid"></param>
        public AKObject(Int64 pid)
        {
            id = pid;
            DataRow RowFromDB = SelectData("osm_id", pid.ToString());

            if (RowFromDB != null)
            {
                LoadFromDB(RowFromDB);
            }
        }
        /// <summary>
        /// Создать экземпляр для объекта по номеру и коду региона
        /// </summary>
        /// <param name="pnumber"></param>
        /// <param name="pregion"></param>
        public AKObject(int pnumber, int pregion)
        {
            DataRow RowFromDB = SelectData(
                new string[] {"number", "region_id"},
                new string[] { pnumber.ToString(), pregion.ToString() });

            if (RowFromDB != null)
            {
                try { id = Convert.ToInt64(RowFromDB["osm_id"]); }
                catch (Exception ex)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}, id = {3}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, id));
                    id = Convert.ToInt64(RowFromDB["id"]); 
                }
                LoadFromDB(RowFromDB);
            }
        }

        #endregion


        public bool IsExists()
        {
            if (id == 0)
            {
                return false;
            }
            return true;
        }

        public void SetPoint(double Lat, double Lon, bool only = false)
        {
            latitude = Lat;
            longitude = Lon;
            if (!only)
            {
                double [] coor = Geocoder.LL2UTM(new double[] { Lat, Lon });
                address = Geocoder.RGeocode(new double[] { Lat, Lon });
                // region_id = address.DistrictId;
            }
        }

        public List<string> GetContacts()
        {
            List<object[]> res = null;
            if (id != 0)
            {
                res = DataBase.RowSelect(
                        string.Format(
                            "SELECT cnt.value || ' (' || tcnt.shortname || ')' as value FROM oko.contact cnt JOIN oko.tcontact tcnt ON cnt.tcontact = tcnt.id WHERE object_id = {0}",
                            this.id
                        )
                    );
            }
            return res.Select(x => x[0].ToString()).ToList<string>();
        }

        public void AddContact(int tcontact, string value, string desc)
        {
            if (id != 0)
            { // добавить информацию о контакте
                DataBase.RunCommandInsert(
                        "oko.contact",
                        new Dictionary<string, object>()
                        {
                            {"tcontact", tcontact},
                            {"value", value.Q()},
                            {"\"desc\"", desc.Q()},
                            {"object_id", id}
                        }
                    );
            }
        }

        public void UpdContact(int tcontact, string value, string desc)
        {
            if (id != 0)
            { // добавить информацию о контакте
                DataBase.RunCommandUpdate(
                        "oko.contact",
                        new Dictionary<string, object>()
                        {
                            {"tcontact", tcontact},
                            {"value", value.Q()},
                            {"\"desc\"", desc.Q()}
                        },
                        new Dictionary<string, object>()
                        {
                            {"object_id", id}
                        }
                    );
            }
        }

        public void DelContact(int tcontact, string value, string desc)
        {
            if (id != 0)
            { // добавить информацию о контакте
                DataBase.RunCommandDelete(
                    "oko.contact",
                    new Dictionary<string, object>()
                    {
                        {"tcontact", tcontact},
                        {"value", value.Q()},
                        {"\"desc\"", desc.Q()},
                        {"object_id", id}
                    }
                );
            }
        }

        public void AddContract(int number, string date_create, string date_start, string date_finish, int idcompany)
        {
            if (id != 0)
            { // добавить информацию о договоре
                DataBase.RunCommandInsert(
                    "oko.contract",
                    new Dictionary<string, object>()
                    {
                        {"number", number.Q()},
                        {"date_create", date_create.Q()},
                        {"date_start", date_start.Q()},
                        {"date_finish", date_finish.Q()},
                        {"company_id", idcompany}
                    }
                );
            }
        }

        public void SetAddress(Address pAddress)
        {
            address = pAddress;
            double[] coor = Geocoder.Geocode(pAddress);
            latitude = coor[0];
            longitude = coor[1];
            // region_id = address.DistrictId;
        }

        /// <summary>
        /// Сохранение
        /// </summary>
        public void Save2DB()
        {
            // информация об объекте
            Dictionary<string, object> Data = new Dictionary<string, object>() 
            { 
                {"makedatetime",makedatetime.Q()},
                // {"tclass_id",TClass},
                {"tstatus_id",TStatus},
                {"name",name.Q()},
                {"number",number},
                {"region_id", RegionId},
                {"real_object", real_object}, 
                {"customer_id", customer_id}, 
                {"dogovor",dogovor},
                {"way",string.Format("ST_Transform(ST_GeomFromText('POINT({0} {1})', 4326), 900913)",latitude.C2S(), longitude.C2S())}
            };
            if (id != 0)
            { // обновить информацию об объекте
                DataBase.RunCommandUpdate(
                        OTable,
                        Data,
                        new Dictionary<string, object>()
                        {
                            {"osm_id",id}
                        }
                    );
            }
            else
            { // добавить
                object[] res;
                if (DataBase.RunCommandInsert(OTable, Data, "osm_id", out res) > 0)
                {
                    id = Convert.ToInt64(res[0]);
                }
            }
            address.Save2DB();
        }

        private DataRow SelectData(string kfield, string kval)
        {
            return DataBase.FirstRow(String.Format("SELECT *, ST_AsText(ST_Transform(way, 4326)) as coor_txt FROM {2} WHERE {0} = '{1}'", kfield, kval, OTable));
        }

        private DataRow SelectData(string[] kfields, string[] kvals)
        {
            string where = " WHERE " + string.Join(" and ", kfields.Zip(kvals, (a, b) => string.Format("{0} = '{1}'", a, b)).ToArray() );
            return DataBase.FirstRow(
                String.Format("SELECT *, ST_AsText(ST_Transform(way, 4326)) as coor_txt FROM {0} {1}", OTable, where));
        }

        public void LoadFromDB(DataRow RowFromDB)
        {
            if (RowFromDB == null)
            {
                RowFromDB = SelectData("osm_id", id.ToString());
            }
            if (RowFromDB != null)
            {
                id = RowFromDB["osm_id"].ToInt64();
                number = RowFromDB["number"].ToInt();
                name = RowFromDB["name"].ToString();
                makedatetime = RowFromDB["makedatetime"].ToString();
                // TClass = RowFromDB["tclass_id"].ToInt();
                string coor = RowFromDB["coor_txt"].ToString();
                double[] r = Geocoder.ExtractFromString(coor);
                latitude = r[0]; longitude = r[1];
                TState = RowFromDB["tstate_id"].ToInt();
                TStatus = RowFromDB["tstatus_id"].ToInt();
                region_id = RowFromDB["region_id"].ToInt();
                address.Load4DB(RowFromDB["address_id"].ToInt());
                dogovor = RowFromDB["dogovor"].ToBool();
                real_object = RowFromDB["real_object"].ToInt();
                customer_id = RowFromDB["customer_id"].ToInt();
                ObjectProperties();
            }
            UpdateStatus();
        }

        public void UpdateStatus()
        {
            if (number != 0)
            {
                DataRow row = DataBase.FirstRow(
                    string.Format("select * from oko.object_status where osm_id = {0}", id)
                );
                if (row != null)
                {
                    TStatus = row["status_id"].ToInt();
                    TState = row["state_id"].ToInt();
                }
            }
        }

        public AKObject(DataRow RowFromDB)
        {
            id = Convert.ToInt64(RowFromDB["osm_id"]);
            LoadFromDB(RowFromDB);
        }
        
        

        public static bool TryGetObjectByAddress(string Address, out AKObject Result)
        {
            bool r = false;
            Result = null;
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("SELECT * FROM {1} WHERE \"address\" = '{0}'", Address, OTable), ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                r = true;
                Result = new AKObject(ds.Tables[0].Rows[0]);
            }
            return r;
        }

        public static bool TryGetObjectByName(string Name, out AKObject Result)
        {
            bool r = false;
            Result = null;
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("SELECT * FROM {1} WHERE \"name\" = '{0}'", Name, OTable), ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                r = true;
                Result = new AKObject(ds.Tables[0].Rows[0]);
            }
            return r;
        }

        public static bool TryGetObjectByNumber(string Number, out AKObject Result)
        {
            bool r = false;
            Result = null;
            DataSet ds = new DataSet();
            DataBase.RowSelect(String.Format("SELECT * FROM {1} WHERE \"number\" = {0}", Number, OTable), ds);
            if (ds.Tables[0].Rows.Count == 1)
            {
                r = true;
                Result = new AKObject(ds.Tables[0].Rows[0]);
            }
            return r;
        }

        
    }
}
