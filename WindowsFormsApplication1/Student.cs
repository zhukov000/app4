using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    public class Student
    {
        [Key]
        public int student_id { get; set; }
        public string gr { get; set; }

        public int person_id { get; set; }
        public virtual Person Person { get; set; }
    }
}
