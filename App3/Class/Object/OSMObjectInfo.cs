using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    public class OSMObjectInfo
    {
        public Int64 id;
        public string name;
        public object way;

        public OSMObjectInfo(string table, Int64 pid)
        {
            DataRow row = DataBase.FirstRow(string.Format("SELECT * FROM {0} WHERE osm_id = {1}", table, pid));
            if (row != null)
            {
                name = row["name"].ToString();
                way = row["way"];
            }
        }

        public OSMObjectInfo(Int64 pid, string pname, object pway)
        {
            id = pid;
            name = pname;
            way = pway;
        }

        static public double[] GetObjectCoordinate(Int64 pid, string table)
        {
            double[] r = new double[2];
            object o = DataBase.First(String.Format("SELECT ST_AsText(way) as pnt FROM {0} WHERE osm_id = {1}", table, pid), "pnt");
            if (o != null)
            {
                r = o.ToString().Substring(6).Trim('(', ')', ' ').Split(' ').Select(x => double.Parse(x, CultureInfo.InvariantCulture)).ToArray();
            }
            return r;
        }
    }
}
