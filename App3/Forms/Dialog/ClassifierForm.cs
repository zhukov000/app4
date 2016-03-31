using App3.Class;
using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms.Dialog
{
    public partial class ClassifierForm : Form
    {
        public ClassifierForm(bool HideSelect = false)
        {
            InitializeComponent();
            if (HideSelect)
            {
                button1.Visible = false;
            }
        }

        private void InitTreeView(NTree<ClassifierObject> nodeSrc, ref TreeNode node)
        {
            foreach (NTree<ClassifierObject> child in nodeSrc.Children)
            {
                TreeNode node1 = node.Nodes.Add(child.Data.Rid.ToString(), child.Data.Value);
                InitTreeView(child, ref node1);
            }
        }

        private void LoadData()
        {
            treeView1.Nodes.Clear();
            TreeNode node = treeView1.Nodes.Add("0", "корень");
            InitTreeView(DBDict.TClassifier, ref node);
            node.Expand();
        }

        private void ClassifierForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private int result = 0;

        public int Result
        {
            get 
            {
                return result;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private TreeNode GetCurrentTree()
        {
            return treeView1.SelectedNode;
        }

        private void добавитьУзелToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // добавить
            TreeNode node = GetCurrentTree(); // родитель
            if ( node == null )
            {
                MessageBox.Show("Выберите узел, в который необходимо добавить");
                return;
            }
            if (MessageBox.Show("Вы действительно хотите добавить узел?", "Вопрос", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string s = "";
                if (Utils.InputBox("Введите название","Введите название категории", ref s) == DialogResult.OK)
                {
                    // информация о родителе
                    object[] row = DataBase.FirstRow("SELECT id, pid FROM oko.classifier WHERE rid = " + node.Name, 0);
                    int idparent = 0;
                    // добавление узла
                    if (row.Count() > 0)
                    {
                        idparent = row[0].ToInt();
                    }
                    object o = DataBase.First(String.Format("select max(id) as mid from oko.classifier where pid = {0}", idparent), "mid");
                    int i = 1;
                    if (o != null)
                    {
                        i = o.ToInt() + 1;
                    }
                    // добавление узла
                    DataBase.RunCommandInsert
                    (
                        "oko.classifier",
                        new Dictionary<string, object>
                        {
                            {"id", i},
                            {"pid", idparent},
                            {"value", s.Q()}
                        }
                    );
                    //
                    DBDict.Update();
                    LoadData();
                }                
            }  
        }

        private void удалитьУзелToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // удалить
            TreeNode node = GetCurrentTree();
            if (MessageBox.Show("Вы действительно хотите удалить узел конфигурации: \"" + node.Text + "\"", "Вопрос", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                // информация об удаляемом узле
                object[] row = DataBase.FirstRow("SELECT id, pid FROM oko.classifier WHERE rid = " + node.Name, 0);
                if ( row[1].Equals(0) )
                {
                    // удалить все ветви
                    DataBase.RunCommand(String.Format("DELETE FROM oko.classifier WHERE pid = {0}", row[0]));
                }
                // удалить сам узел
                DataBase.RunCommand(String.Format("DELETE FROM oko.classifier WHERE rid = {0}", node.Name.ToInt()));
                DBDict.Update();
                LoadData();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // выбрать
            TreeNode node = GetCurrentTree();
            if (node.Nodes.Count > 0)
            {
                MessageBox.Show("Нельзя выбрать справочник, выберите какое-то из его свойств","Предупреждение");
            }
            else
            {
                result = node.Name.ToInt();
                Close();
            }
        }
    }
}
