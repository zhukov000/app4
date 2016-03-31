using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    [Serializable]
    public partial class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
         
        public ComboboxItem() 
        {
            Text = "";
            Value = null;
        }

        public ComboboxItem(string pText, object pValue)
        {
            Text = pText;
            Value = pValue;
        }

        public ComboboxItem(KeyValuePair<int, string> pair) 
        {
            Text = pair.Value;
            Value = pair.Key;
        }
    }
}
