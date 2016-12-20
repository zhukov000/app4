using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Class.Static
{
    internal class TMessageDict
    {
        private IDictionary<int, IDictionary<int, IDictionary<int, Tuple<Utils.MessageGroupId, string, string>>>> data;

        public Tuple<Utils.MessageGroupId, string, string> this[int Oko, int Class, int Code]
        {
            get
            {
                Tuple<Utils.MessageGroupId, string, string> result = new Tuple<Utils.MessageGroupId, string, string>(Utils.MessageGroupId.UNDEFINED, "", "");
                if (this.data.ContainsKey(Oko) && this.data[Oko].ContainsKey(Class))
                {
                    if (!this.data[Oko][Class].ContainsKey(Code))
                    {
                        if (this.data[Oko][Class].ContainsKey(0))
                        {
                            result = this.data[Oko][Class][0];
                        }
                    }
                    else
                    {
                        result = this.data[Oko][Class][Code];
                    }
                }
                return result;
            }
        }

        public TMessageDict()
        {
            this.data = new Dictionary<int, IDictionary<int, IDictionary<int, Tuple<Utils.MessageGroupId, string, string>>>>();
            foreach (object[] expr_2D in DataBase.RowSelect("select \"OKO\", class, code, mgroup_id, message, notes from oko.message_text"))
            {
                int key = expr_2D[0].ToInt();
                int key2 = expr_2D[1].ToInt();
                int key3 = expr_2D[2].ToInt();
                Utils.MessageGroupId item = (Utils.MessageGroupId)expr_2D[3].ToInt();
                string item2 = expr_2D[4].ToString();
                string item3 = expr_2D[5].ToString();
                if (!this.data.ContainsKey(key))
                {
                    this.data[key] = new Dictionary<int, IDictionary<int, Tuple<Utils.MessageGroupId, string, string>>>();
                }
                if (!this.data[key].ContainsKey(key2))
                {
                    this.data[key][key2] = new Dictionary<int, Tuple<Utils.MessageGroupId, string, string>>();
                }
                if (!this.data[key][key2].ContainsKey(key3))
                {
                    this.data[key][key2][key3] = new Tuple<Utils.MessageGroupId, string, string>(item, item2, item3);
                }
            }
        }
    }
}
