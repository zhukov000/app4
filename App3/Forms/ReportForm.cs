using App3.Class;
using App3.Class.Static;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class ReportForm : Form
    {
        bool reportVisible = false;
        public ReportForm()
        {
            InitializeComponent();
        }

        /*
        private AutoCompleteStringCollection DataObjects(int region_id = 0)
        {
            AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            DataSet ds = new DataSet();
            if (region_id != 0)
            {
                DataBase.RowSelect(String.Format("select distinct osm_id, number, name from oko.object where region_id = {0} order by name, number", region_id), ds);
            }
            else
            {
                DataBase.RowSelect(String.Format("select distinct osm_id, number, name from oko.object order by name, number"), ds);
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(string.Format("{0} {1} ID={2}", row["number"], row["name"], row["osm_id"]));
            }
            return ret;
        }
         * */
        private List<ComboboxItem> DataObjects(int region_id = 0)
        {
            // AutoCompleteStringCollection ret = new AutoCompleteStringCollection();
            List<ComboboxItem> ret = new List<ComboboxItem>();
            DataSet ds = new DataSet();
            if (region_id != 0)
            {
                DataBase.RowSelect(String.Format("select distinct osm_id, number, name from oko.object where region_id = {0} order by name, number", region_id), ds);
            }
            else
            {
                DataBase.RowSelect(String.Format("select distinct osm_id, number, name from oko.object order by name, number"), ds);
            }
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ret.Add(new ComboboxItem(
                    string.Format("{0} {1}", row["number"], row["name"]), 
                    row["osm_id"].ToInt())
                );
            }
            return ret;
        }

        private TreeNode GetCurrentTree()
        {
            return treeView1.SelectedNode;
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
            
            autoCompleteTextbox6.AutoCompleteMode = AutoCompleteMode.Suggest;
            autoCompleteTextbox6.AutoCompleteSource = AutoCompleteSource.CustomSource;
            autoCompleteTextbox6.AutoCompleteList = DataObjects();
            if (autoCompleteTextbox6.AutoCompleteCustomSource.Count == 1)
            {
                autoCompleteTextbox6.Text = autoCompleteTextbox6.AutoCompleteCustomSource[0].ToString();
            }
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadData();
                DBDict.Load2Combobox(ref districtBox,
                    DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    null
                );
                DBDict.Load2Combobox(
                    ref MonCompBox,
                    DBDict.TCompany.Where(x => x.Value.Item2 == 1).Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    1
                );
                DBDict.Load2Combobox(
                    ref serviceCompBox,
                    DBDict.TCompany.Where(x => x.Value.Item2 == 2).Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(),
                    null
                );
            }
            catch (Exception ex)
            {
                Class.Singleton.Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            dtStart.Value = DateTime.Now.AddMonths(-1);
            treeView1.Nodes[0].Checked = true;
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            CheckTreeViewNode(e.Node, e.Node.Checked);
        }

        private void CheckTreeViewNode(TreeNode node, Boolean isChecked)
        {
            foreach (TreeNode item in node.Nodes)
            {
                item.Checked = isChecked;

                if (item.Nodes.Count > 0)
                {
                    this.CheckTreeViewNode(item, isChecked);
                }
            }
        }

        private int getRegionId()
        {
            ComboboxItem i = (ComboboxItem)districtBox.SelectedItem;
            if (i != null)
                return i.Value.ToInt();
            else 
                return -1;
        }

        private int getObjectId()
        {
            ComboboxItem i = autoCompleteTextbox6.SelectedItem;
            if (i != null)
                return i.Value.ToInt();
            else 
                return -1;
        }

        private List<int> getClassifierCodes(TreeNodeCollection pNodes)
        {
            List<int> ret = new List<int>();
            foreach (TreeNode aNode in pNodes)
            {
                if (aNode.Checked)
                {
                    ret.Add(aNode.Name.ToInt());
                }
                if (aNode.Nodes.Count > 0)
                {
                    List<int> ll = getClassifierCodes(aNode.Nodes);
                    ret = ret.Union(ll).ToList();
                }
            }
            return ret;
        }

        private void districtBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem i = (ComboboxItem)districtBox.SelectedItem;
            autoCompleteTextbox6.AutoCompleteList = DataObjects(i.Value.ToInt());
        }
        
        private void ShowReport(string sql, string report)
        {
            Class.Report r = new Class.Report(report, sql);
            r.showReport(reportViewer1);
            /*
            DataSet myDS = new DataSet();
            DataBase.RowSelect(sql, myDS);
            DataTable dt = myDS.Tables[0];
            ReportDataSource DSReport = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(DSReport);
            reportViewer1.LocalReport.ReportEmbeddedResource = report;
            reportViewer1.RefreshReport(); 
            */
        }

        private void ShowDynamicReport(string sql, string report)
        {
            DataSet myDS = new DataSet();
            DataBase.RowSelect(sql, myDS);
            DataTable dt = myDS.Tables[0];
            // обработка записей в таблице с данными
            DateTime start = dtStart.Value;
            DateTime finish = dtStart.Value;
            int timeout_interval = DBDict.Settings["TIMEOUT_MESSAGE_INTERVAL"].ToInt();
            int not_connected_interval = DBDict.Settings["NOT_CONNECTED_INTERVAL"].ToInt();
            // список последних обращений
            Dictionary<long, int> links = new Dictionary<long,int>();
            // список сообщений, в которых дата/время меньше start
            // Dictionary<long, int> links_del = new Dictionary<long, int>();
            // obj.osm_id, ev.datetime as start, mt.mgroup_id as state_id, null as finish, ts.status
/*            for (int i = 0; i < dt.Rows.Count-1; i++)
            {
                DataRow r = dt.Rows[i];
                DateTime dtSt = new DateTime();
                if (DateTime.TryParse(r["start"].ToString(), out dtSt))
                {
                    if (start.Subtract(dtSt).TotalSeconds < 0)
                    {
                        int j = i-2;
                        while (j >= 0)
                        {
                            dt.Rows.RemoveAt(j);
                            j--;
                        }
                        break;
                    }
                }

            }*/

            // List<int> rows4delete = new List<int>();

            for(int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow r = dt.Rows[i];
                long l = r["osm_id"].ToInt64();

                if ( !links.ContainsKey(l) )
                { // не было такого объекта
                    links.Add(l, i);
                }
                else 
                { // уже был такой объект 
                    DataRow r2 = dt.Rows[links[l]];
                    DateTime dtSt = new DateTime();
                    DateTime dtFn = new DateTime();
                    
                    if (DateTime.TryParse(r2["start"].ToString(), out dtSt) && DateTime.TryParse(r["start"].ToString(), out dtFn))
                    {
                        TimeSpan ts0 = dtSt.Subtract(start);
                        DateTime dtSt2 = dtSt;
                        if (dtSt < start)
                        {

                            /*if (links_del.ContainsKey(l))
                            { // для этого объекта уже было сообщение с датой меньше начала
                                rows4delete.Add(links_del[l]);
                                links_del[l] = links[l];
                            }
                            else
                            {
                                links_del.Add(l, links[l]);
                            }*/
                            // MessageBox.Show(dtSt.ToString());
                            dtSt2 = start;
                            dt.Rows[links[l]]["start"] = start;
                        }
                        TimeSpan ts = dtFn.Subtract(dtSt);
                        // если следующее событие пришло позже, чем not_connected_interval
                        if (ts.TotalSeconds > not_connected_interval)
                        {
                            // DateTime dtSt2 = new DateTime();
                            // if (DateTime.TryParse(dt.Rows[links[l]]["start"].ToString(), out dtSt2) )
                            // {
                                // dtSt2 = dtSt2.AddSeconds(not_connected_interval);
                            dt.Rows[links[l]]["finish"] = dtSt.AddSeconds(not_connected_interval);
                            // }
                            dt.Rows[links[l]]["val"] = dtSt.AddSeconds(not_connected_interval).Subtract(dtSt2).TotalSeconds;// dtFn.Subtract();
                        }
                        else
                        {
                            dt.Rows[links[l]]["finish"] = r["start"];
                            dt.Rows[links[l]]["val"] = ts.TotalSeconds;
                            if (dtFn.Subtract(start).TotalSeconds > 0)
                            {
                                if (ts0.TotalSeconds < 0)
                                {
                                    dt.Rows[links[l]]["val"] = ts.TotalSeconds + ts0.TotalSeconds;
                                }
                            }
                            else
                            {
                                dt.Rows[links[l]]["val"] = 0;
                            }
                            
                        }
                        // dt.Rows[links[l]]["value"] = ts.ToString(@"dd\.hh\:mm\:ss").Replace(".","д. ");
                    }
                }
                links[l] = i;
            }
            /* for (int i = rows4delete.Count-1; i >= 0; i-- )
            {
                dt.Rows.RemoveAt(rows4delete[i]);
            } */
            // вывод отчета
            ReportDataSource DSReport = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(DSReport);
            reportViewer1.LocalReport.ReportEmbeddedResource = report;
            reportViewer1.RefreshReport();
        }

        private Pair<string,string> Request()
        {
            Pair<string, string> req = new Pair<string, string>("", "");
            string where = " WHERE TRUE ";
            List<int> codes = getClassifierCodes(treeView1.Nodes);
            /// Выбор отчета
            if (totalReport.Checked)
            { // общий отчет
                req.First = @"SELECT DISTINCT count(distinct obj.osm_id) as cnt, ost.state, ost.color, ost.state_id
                            FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            JOIN oko.object_status ost ON ost.osm_id = obj.osm_id 
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id {0}
                            GROUP BY ost.state, ost.color, ost.state_id
                            ORDER BY ost.state";
                req.Second = "App3.Reports.StatAllObj.rdlc";
            } else if (statusReport.Checked)
            { // отчет по статусам
                req.First = @"SELECT DISTINCT count(distinct obj.osm_id) as cnt, ost.status as state
                            FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            JOIN oko.object_status ost ON ost.osm_id = obj.osm_id 
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id {0}
                            GROUP BY ost.status
                            ORDER BY ost.status";
                req.Second = "App3.Reports.StatStatus.rdlc";
            } else if (classifytyReport.Checked)
            {
                req.First = @"SELECT DISTINCT count(distinct obj.osm_id) as cnt, Coalesce(clr.value, 'не использует классификатор') as name
                            FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            JOIN oko.object_status ost ON ost.osm_id = obj.osm_id 
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id
                            LEFT JOIN oko.classifier clr on clr.rid = opr.property_id {0}
                            GROUP BY clr.value";
                req.Second = "App3.Reports.StatClassifier.rdlc";
            } else if (dynamicReport.Checked)
            {
                req.First = String.Format(@"SELECT DISTINCT obj.osm_id, ev.datetime as start, mt.mgroup_id as state_id, 
                            LEAST (ev.datetime + '{2} second'::interval , '{1}'::timestamp) as finish, ts.status, obj.number, 
                            obj.number || ' ' || Coalesce(obj.name, '') as name, 
                            round(EXTRACT(EPOCH FROM  (LEAST (ev.datetime + '{2} second'::interval , '{1}'::timestamp) - GREATEST(ev.datetime, '{0}'::timestamp))) ) as val, 
                            extract('epoch' from '{1}'::timestamp - '{0}'::timestamp) as value
                            FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            JOIN oko.object_status ost ON ost.osm_id = obj.osm_id 
                            JOIN oko.all_event ev ON ev.region_id = obj.region_id and ev.objectnumber = obj.number ", dtStart.Value.ToString(), dtFinish.Value.ToString(), DBDict.Settings["NOT_CONNECTED_INTERVAL"].ToInt()) +
                            "JOIN oko.message_text mt ON mt.\"OKO\" = ev.oko_version and mt.class = ev.class and mt.code = ev.code " +
                            @"JOIN oko.tstate ts ON ts.id = mt.mgroup_id AND ts.id in (1,2,3)
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id
                            LEFT JOIN oko.classifier clr on clr.rid = opr.property_id  {0}
                            ORDER BY ev.datetime, obj.osm_id";
                where = string.Format(" WHERE ev.datetime BETWEEN ('{0}'::timestamp - '{2} second'::interval) and '{1}' ", dtStart.Value.ToString(), dtFinish.Value.ToString(), DBDict.Settings["NOT_CONNECTED_INTERVAL"].ToInt());
                req.Second = "App3.Reports.StatEvents.rdlc";
            } else if (ProcessingList.Checked)
            {
                req.First = "select obj.number, obj.name, mt.message, count(distinct ev.id) as cntev " +
                            @"FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id " +
                            "inner join oko.all_event ev on ev.objectnumber = obj.number and ev.region_id = obj.region_id " +
                            "inner join oko.message_text mt on ev.class = mt.class and ev.code = mt.code and ev.oko_version = mt.\"OKO\" {0} " +
                            "group by obj.number, obj.name, mt.message";
                where = string.Format(" WHERE mt.\"OKO\" = 10 AND ev.datetime BETWEEN ('{0}'::timestamp - '{2} second'::interval) and '{1}' ", dtStart.Value.ToString(), dtFinish.Value.ToString(), DBDict.Settings["NOT_CONNECTED_INTERVAL"].ToInt());
                req.Second = "App3.Reports.ListProcessing.rdlc";
            } else if (ProcessingStat.Checked)
            {
                req.First = "select mt.message, count(distinct ev.id) as cntev " +
                            @"FROM regions2map reg
                            JOIN oko.object obj ON obj.region_id = reg.num
                            LEFT JOIN oko.object_properties opr ON opr.object_id = obj.osm_id " +
                            "inner join oko.all_event ev on ev.objectnumber = obj.number and ev.region_id = obj.region_id " +
                            "inner join oko.message_text mt on ev.class = mt.class and ev.code = mt.code and ev.oko_version = mt.\"OKO\" {0} " +
                            "group by mt.message";
                where = string.Format(" WHERE mt.\"OKO\" = 10 AND ev.datetime BETWEEN ('{0}'::timestamp - '{2} second'::interval) and '{1}' ", dtStart.Value.ToString(), dtFinish.Value.ToString(), DBDict.Settings["NOT_CONNECTED_INTERVAL"].ToInt());
                req.Second = "App3.Reports.StatProcessing.rdlc";
            }
            else
            { // ошибка
                return req;
            }
            /// фильтры
            if (checkBox4.Checked)
            { // фильтр по объекту
                int object_id = getObjectId();
                if (object_id != -1)
                {
                    where += " AND obj.osm_id = " + object_id.ToString();
                }
            }
            if (checkBox5.Checked)
            { // фильтр по району
                int region_id = getRegionId();
                if (region_id != -1)
                {
                    where += " AND obj.region_id = " + region_id.ToString();
                }
            }
            if (checkBox6.Checked)
            {
                int id_comp = Utils.ComboboxVal(ref MonCompBox);
                if (id_comp != -1)
                {
                    where += String.Format(@" AND obj.osm_id IN (select oin.object_id
                        from oko.contract con
                         inner join oko.object_in_contract oin on oin.contract_id = con.id_contract
                        where con.company_id = {0})", id_comp);
                }
            }
            if (codes.Count > 0 && classifityBox.Checked)
            { // фильтр по классификатору
                string s = string.Join(",", codes.ToArray());
                where += string.Format(" AND (opr.property_id IN ({0}))", s); ;
            }
            List<int> status = new List<int>();
            if (checkBox1.Checked) status.Add(1);
            if (checkBox2.Checked) status.Add(2);
            if (checkBox3.Checked) status.Add(0);
            if (status.Count > 0)
            {
                string s = String.Join(",", status.Select(x => x.ToString()).ToArray());
                where += string.Format(" AND obj.tstatus_id IN ({0})", s);
            }
            req.First = String.Format(req.First, where);
            return req;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Pair<string,string> r = Request();
            if (r.First != "" && r.Second != "")
            {
                splitContainer1.SplitterDistance = 150;
                if (dynamicReport.Checked)
                {
                    ShowDynamicReport(r.First, r.Second);
                }
                else 
                { 
                    ShowReport(r.First, r.Second);
                }
            }
            else
            {
                MessageBox.Show("Невозможно построить отчет для выбранных параметров");
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            autoCompleteTextbox6.Enabled = checkBox4.Checked;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            districtBox.Enabled = checkBox5.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void splitContainer1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            splitContainer1.SplitterDistance = 650;
        }

        private void objectBox_CheckedChanged(object sender, EventArgs e)
        {
            autoCompleteTextbox1.Enabled = objectBox.Checked;
        }

        private void customerBox_CheckedChanged(object sender, EventArgs e)
        {
            autoCompleteTextbox2.Enabled = customerBox.Checked;
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            serviceCompBox.Enabled = checkBox7.Checked;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            MonCompBox.Enabled = checkBox6.Checked;
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            splitContainer1.SplitterDistance = 650;
        }

        private void reportViewer1_Drillthrough(object sender, DrillthroughEventArgs e)
        {
            List<ReportParameterInfo> param = e.Report.GetParameters().ToList();
            switch (reportViewer1.LocalReport.ReportEmbeddedResource)
            {
                case "App3.Reports.StatAllObj.rdlc":
                    // show ListObjectByState
                    Class.Report rep = new Class.Report("App3.Reports.ListObjectByState.rdlc",
                        string.Format(@"SELECT obj.number, obj.name, obj.tstate_id, obj.region_id,
                            reg.name, case when obj.dogovor then 'есть' else 'нет' end as dogovor, 
                            obj.dt, obj.description, adr.address
                          FROM oko.object obj
                            left join oko.addresses adr on adr.id = obj.address_id
                            inner join regions2map reg on reg.num = obj.region_id
                          WHERE tstate_id = {0};", param[0].Values[0]));
                    LocalReport localRep = (LocalReport)e.Report;
                    localRep.DataSources.Add(
                        new ReportDataSource(
                            "DataSet1",
                            rep.getDetailReport(localRep)
                        )
                    );
                    break;
            }
            /*
            List<ReportParameterInfo> param = e.Report.GetParameters().ToList();

            string reportName = e.ReportPath + ".rdlc";
            string sqlStr = "SELECT * FROM task_and_answers " +
                            "WHERE task = '" + param[0].Values[0] + "'";
            myReport myRep = new myReport(reportName);

            LocalReport localRep = (LocalReport)e.Report;
            localRep.DataSources.Add(new ReportDataSource("DataSet1",
                myRep.getDetailReport(conStr, sqlStr, localRep)));
                */
        }
    }
}
