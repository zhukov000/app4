using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Web
{
    public class Deleted
    {
        public string id { get; set; }
    }

    public class Attr
    {
        public string key { get; set; }
        public string val { get; set; }
    }

    class JsonNewObject
    {
        public string objectname { get; set; }
        public List<List<Attr>> reversed { get; set; }
        public List<Deleted> deleted { get; set; }
    }
}
