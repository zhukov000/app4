using App3.Class.Static;
using App3.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class.Synchronization
{
    class Entity : IEntitySynchronization
    {

        virtual public List<KeyValuePair<string, object>[]> getReversed(DateTime ActualyPoint)
        {
            return Synchronizer.getReversed(getTableName(), ActualyPoint);
        }

        virtual public KeyValuePair<string, object>[] getDeleted(DateTime ActualyPoint)
        {
            List<object[]> l = Synchronizer.getDeleted(getName(), ActualyPoint);
            KeyValuePair<string, object>[] ret = l.Select(x => new KeyValuePair<string, object>(x[3].ToString(), x[2].ToInt())).ToArray();
            return ret;
        }

        virtual public string getName()
        {
            throw new NotImplementedException();
        }

        virtual public string getTableName()
        {
            return "oko." + getName();
        }

        override public string ToString()
        {
            return getName();
        }

    }
}
