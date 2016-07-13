using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;

namespace WindowsFormsApplication1
{
   
    public partial class Form1 : Form
    {
        private StudentContext db = new StudentContext();

        public Form1()
        {
            InitializeComponent();
        }

        private void UpdateData()
        {
            try
            {
                var query = from b in db.Persons
                            join s in db.Students on b.person_id equals s.person_id
                            orderby b.name
                            select new { surname = b.surname, name = b.name, gr = s.gr };
                listBox1.Items.Clear();
                foreach (var p in query)
                {
                    listBox1.Items.Add(p.surname + " " + p.name + " " + p.gr);
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                var p = db.Persons.Add(new Person { surname = surnameBox.Text, name = nameBox.Text });
                db.Students.Add(new Student { person_id = p.person_id, gr = grBox.Text });
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UpdateData();
        }
    }

    public class StudentContext : DbContext
    {
        public StudentContext() : base("PostgresDotNet") { }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
            modelBuilder.Entity<Person>().ToTable("person");
            modelBuilder.Entity<Student>().ToTable("student");
            base.OnModelCreating(modelBuilder);
        }
    }

}
