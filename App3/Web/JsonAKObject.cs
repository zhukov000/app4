using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace App3.Web
{
    public class JsonAKObject
    {
        public string point { get; set; }
        public string number { get; set; } 
	    public string name { get; set; } 
	    public Int64 osm_id { get; set; }
        public int tstate_id { get; set; }
        public int tstatus_id { get; set; }
        public int region_id { get; set; }
        public string makedatetime { get; set; }
        public bool dogovor { get; set; }
        public string datetime { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public string locality { get; set; }
        public string street { get; set; }
        public string house { get; set; }
        public string region { get; set; }
        public string address { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
        public int address_id { get; set; }
    }
}
