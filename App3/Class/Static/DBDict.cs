using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MessageGroupId = App3.Class.Utils.MessageGroupId;

namespace App3.Class.Static
{
    class TMessageDict
    {
        private IDictionary<int, IDictionary<int, IDictionary<int, Tuple<MessageGroupId, string, string>>>> data;

        public TMessageDict()
        {
            data = new Dictionary<int, IDictionary<int, IDictionary<int, Tuple<MessageGroupId, string, string>>>>();
            List<object[]> l = DataBase.RowSelect("select \"OKO\", class, code, mgroup_id, message, notes from oko.message_text");
            foreach(object[] row in l)
            {
                int Oko = row[0].ToInt();
                int Class = row[1].ToInt();
                int Code = row[2].ToInt();
                MessageGroupId MGrId = (MessageGroupId)row[3].ToInt();
                string Message = row[4].ToString();
                string Notes = row[5].ToString();

                if (!data.ContainsKey(Oko))
                    data[Oko] = new Dictionary<int, IDictionary<int, Tuple<MessageGroupId, string, string>>>();
                if (!data[Oko].ContainsKey(Class))
                    data[Oko][Class] = new Dictionary<int, Tuple<MessageGroupId, string, string>>();
                if (!data[Oko][Class].ContainsKey(Code))
                    data[Oko][Class][Code] = new Tuple<MessageGroupId, string, string>(MGrId, Message, Notes);
            }
        }

        public Tuple<MessageGroupId, string, string> this[int Oko, int Class, int Code]
        {
            get
            {
                Tuple<MessageGroupId, string, string> r = new Tuple<MessageGroupId, string, string> (MessageGroupId.UNDEFINED, "", "");
                do
                {
                    if (!data.ContainsKey(Oko)) break;
                    if (!data[Oko].ContainsKey(Class)) break;
                    if (!data[Oko][Class].ContainsKey(Code))
                    {
                        if (data[Oko][Class].ContainsKey(0)) r = data[Oko][Class][0];
                        break;
                    }
                    r = data[Oko][Class][Code];
                } while (false);
                return r;
            }
        }
    }

    static class DBDict
    {
        public static IDictionary<int, string> TContact;
        public static IDictionary<int, string> TNumber;
        public static IDictionary<int, string> TStatus;
        public static IDictionary<int, ObjectState> TState;
        // public static IDictionary<int, Tuple<string, int>> TClass;

        public static IDictionary<int, Tuple<string, string, string>> TRegion;
        public static IDictionary<string, Tuple<int, string, string>> TDistrict;

        public static IDictionary<string, int> IPAddress;
        public static IDictionary<string, string> Settings;
        // public static IDictionary<int, Tuple<string, string, string>> TMinistry;
        public static IDictionary<int, Tuple<string, int>> TCompany;
        public static NTree<ClassifierObject> TClassifier;
        public static IDictionary<int, Tuple<int,int,string,string>> TClassify;
        public static IDictionary<int, string> TRealObject;
        public static IDictionary<int, string> TCustomer;
        public static IDictionary<int, Tuple<string, bool>> TMessages;
        public static TMessageDict TMessage;

        public static void UpdateDictionary(string name)
        {
            switch(name)
            {
                case "TRealObject":
                    TRealObject = DataBase.RowSelect("select id, name from oko.real_object")
                        .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());
                    break;
                case "TCustomer":
                    TCustomer = DataBase.RowSelect("select id, name from oko.customer")
                        .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());
                    break;
                case "TCompany":
                    TCompany = DataBase.RowSelect("select id_company, title, type from oko.company").
                        ToDictionary(
                            x => x[0].ToInt(),
                            x => new Tuple<string, int>(x[1].ToString(), x[2].ToInt())
                        );
                    break;
                case "TMessage":
                    TMessage = new TMessageDict();
                    break;
            }
        }

        public static void Update()
        {
            TContact = DataBase.RowSelect("select id, name from oko.tcontact")
                .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());

            TRealObject = DataBase.RowSelect("select id, name from oko.real_object")
                .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());

            TCustomer = DataBase.RowSelect("select id, name from oko.customer")
                        .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());

            TNumber = DataBase.RowSelect("select id, name from oko.tnumber")
                .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());

            TStatus = DataBase.RowSelect("select id, status from oko.tstatus")
                .ToDictionary(x => x[0].ToInt(), x => x[1].ToString());

            TState = DataBase.RowSelect("select id, name, status, color, instat, inprocess, warn, music from oko.tstate")
                .ToDictionary(
                    x => x[0].ToInt(),
                    x => new ObjectState(x)
                );

            List<object[]> tmpRegions = DataBase.RowSelect("select num, fullname, name, color from regions2map order by fullname");

            TRegion = tmpRegions
                .ToDictionary(
                    x => x[0].ToInt(),
                    x => new Tuple<string, string, string>(x[1].ToString(), x[2].ToString(), x[3].ToString())
                );

            TDistrict = tmpRegions
                .ToDictionary(
                    x => x[1].ToString(),
                    x => new Tuple<int, string, string>(x[0].ToInt(), x[2].ToString(), x[3].ToString())
                );

            try
            {
                IPAddress = DataBase.RowSelect("select id_region, ipaddress from oko.ipaddresses where listen")
                    .ToDictionary(x => x[1].ToString(), x => x[0].ToInt());
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog("Ошибка в справочнике районов:  " + ex.Message); Logger.Instance.FlushLog();

                IPAddress = DataBase.RowSelect("select id_region, max(ipaddress) as ipaddress from oko.ipaddresses where listen group by id_region")
                    .ToDictionary(x => x[1].ToString(), x => x[0].ToInt());
            }

            Settings = DataBase.RowSelect("select name, value from settings")
                .ToDictionary(x => x[0].ToString(), x => x[1].ToString());

            /*TMinistry = DataBase.RowSelect("select id, name, head, phone from oko.ministry")
               .ToDictionary(
                   x => x[0].ToInt(),
                   x => new Tuple<string, string, string>(x[1].ToString(), x[2].ToString(), x[3].ToString())
               ); */
            TCompany = DataBase.RowSelect("select id, title, type from oko.company").
                ToDictionary(
                    x => x[0].ToInt(),
                    x => new Tuple<string, int>(x[1].ToString(), x[2].ToInt())
                );

            TClassify = DataBase.RowSelect("select cl.id, cl.pid, cl.value, cl.rid, clp.value from oko.classifier cl inner join oko.classifier clp on cl.pid = clp.id and clp.pid = 0").
                ToDictionary(
                    x => x[3].ToInt(),
                    x => new Tuple<int, int, string, string>(x[0].ToInt(), x[1].ToInt(), x[2].ToString(), x[4].ToString())
                );

            TMessages = DataBase.RowSelect("select id, constant_name, show from oko.tmessages").
                ToDictionary(
                    x => x[0].ToInt(),
                    x => new Tuple<string, bool>(x[1].ToString(), x[2].ToBool())
                );

            ClassifierObject obj = new ClassifierObject(0, 0, 0, "", false);
            
            TClassifier = new NTree<ClassifierObject>(obj);
            
            foreach (object[] r in DataBase.RowSelect("select id, pid, value, rid from oko.classifier order by coalesce(pid,0), id"))
            {
                bool l = false;
                if (r[1].ToInt() != 0)
                {
                    l = true;
                }
                obj = new ClassifierObject(r[3].ToInt(), r[0].ToInt(), r[1].ToInt(), r[2].ToString(), l);
                
                if (!l)
                {
                    TClassifier.AddChild(obj);
                }
                else
                {
                    List<ClassifierObject> lstObj = new List<ClassifierObject>();
                    ClassifierObject o = new ClassifierObject(0, r[1].ToInt(), 0, "", l);
                    try
                    {
                        NTree<ClassifierObject> node = TClassifier.RSearch(o, ref lstObj);
                        node.AddChild(obj);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка в классификаторе");
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                    }
                }
            }
            TMessage = new TMessageDict();
        }

        static public void Load2Combobox(ref ComboBox combo, List<ComboboxItem> items, object match)
        {
            combo.Items.Clear();
            int i = 0;
            foreach (ComboboxItem item in items)
            {
                combo.Items.Add(item);
                if (item.Value.Equals(match))
                {
                    combo.SelectedIndex = i;
                }
                i++;
            }
            if (combo.SelectedIndex < 0 && match != null && combo.Items.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
        }

        #region Depricated Работа со связанными справочниками
        /*public static List<KeyValuePair<int,string>> DictValues(string Table, string FieldK, string FieldV)
        {
            List<KeyValuePair<int, string>> r = new List<KeyValuePair<int, string>>();
            DataSet ds = new DataSet();
            DataBase.RowSelect("SELECT * FROM " + Table, ds);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                r.Add(new KeyValuePair<int, string>(
                    (dr[FieldK]),
                    dr[FieldV].ToString()
                ));
            }
            return r;
        }

        public static List<KeyValuePair<int, string>> Classes()
        {
            return DictValues("oko.klass", "id", "name");
        }

        public static List<KeyValuePair<int, string>> Statuses()
        {
            return DictValues("oko.tstatus", "id", "status"); ;
        } */
        #endregion
    }
}
