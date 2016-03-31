using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App3.Class
{
    class ObjectState
    {
        private int id;
        private string name;
        private string status;
        private string color;
        private bool instat, inprocess, warn, music;

        public int Id { get { return id; } }
        public string Name { get { return name; } }
        public string Status { get { return status; } }
        public string Color { get { return color; } }
        public bool Instat { get { return instat; } }
        public bool Inprocess { get { return inprocess; } }
        public bool Warn { get { return warn; } }
        public bool Music { get { return music; } }

        public ObjectState(object [] Data)
        {
              id = Data[0].ToInt();
              name = Data[1].ToString();
              status = Data[2].ToString();
              color = Data[3].ToString();
              instat = Data[4].ToBool();
              inprocess = Data[5].ToBool();
              warn = Data[6].ToBool();
              music = Data[7].ToBool();
        }
    }
}
