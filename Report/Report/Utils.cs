using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report
{
    static class Utils
    {
        static public int ToInt(this object toInt)
        {
            int i = 0;
            if (toInt != null)
                int.TryParse(toInt.ToString(), out i);
            return i;
        }
    }
}
