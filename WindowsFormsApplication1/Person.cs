using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Person
    {
        [Key]
        public int person_id { get; set; }
        public string surname { get; set; }
        public string name { get; set; }

        public virtual List<Student> Students { get; set; }
    }
}
