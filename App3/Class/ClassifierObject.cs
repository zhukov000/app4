using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    class ClassifierObject : System.Collections.IComparer
    {
        int _rid;
        int _id;
        int _pid;
        string _value;
        bool _leaf = false;

        #region

        public int Rid
        {
            get
            {
                return _rid;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public int Pid
        {
            get
            {
                return _pid;
            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        public bool Leaf
        {
            get
            {
                return _leaf;
            }
        }

        #endregion

        public ClassifierObject(int prid, int pid, int ppid, string pvalue, bool pleaf)
        {
            _rid = prid;
            _id = pid;
            _pid = ppid;
            _value = pvalue;
            _leaf = pleaf;
        }

        static int CustomCompsarer<T>(T a, T b) where T : IComparable
        {
            return a.CompareTo(b);
        }
        
        public int Compare(object x, object y)
        {
            if (!(x is ClassifierObject))
                return -1000;
            if (!(y is ClassifierObject))
                return 1000;
            
            return ((ClassifierObject)x).Rid.CompareTo(((ClassifierObject)y).Rid);
        }

        public override bool Equals(System.Object obj)
        {
            if (obj == null)
                return false;
            if (!(obj is ClassifierObject))
                return false;

            if (this._id == ((ClassifierObject)obj)._id &&
                this._pid == ((ClassifierObject)obj)._pid)
                return true;

            return false;
        }
    }
}
