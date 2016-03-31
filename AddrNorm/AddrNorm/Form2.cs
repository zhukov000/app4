using App3;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AddrNorm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Расшифровка короткого названия
        /// </summary>
        /// <param name="shortstr"></param>
        /// <returns></returns>
        private string unshortstr(string shortstr, int level)
        {
            string str = shortstr;
            if (DataBase.IsOpen)
            {
                object row = DataBase.First(
                    string.Format(
                        "SELECT * FROM kladr.socrbase WHERE level = '{0}' AND scname = '{1}'",
                        level, shortstr
                    ),
                    "socrname"
                );
                if (row != null)
                {
                    str = row.ToString();
                }
            }
            return str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1 Соединение с БД
            DataBase.OpenConnection(string.Format(
                        "Server={0};Port={1};User Id={2};Password={3};Database={4};",
                        "localhost",
                        "5432",
                        "postgres",
                        "postgres",
                        "gis"
                    ));
            List<object[]> rows = DataBase.RowSelect(@"SELECT address, district, id, code, locality, street, house, region FROM oko.addresses 
                                                       WHERE code is null and address not in (select address from oko.geo_address)");
            int ii = 0;
            foreach(object[] row0 in rows)
            {
                HtmlNode link = null;
                ii++;
                int maxcnt = 15;
                do
                {
                    string s0 = "text_to_process=" + row0[0].ToString();
                    string ss = "";
                    try
                    {
                        ss = getResponse("http://ahunter.ru/site/search", s0);
                    }
                    catch
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    HtmlDocument doc = new HtmlDocument();
                    using (Stream s = GenerateStreamFromString(ss))
                    {
                        doc.Load(s);
                    }
                    link = doc.GetElementbyId("FoundAddresses");
                    if (link != null)
                    {
                        IDictionary<string, object> rowdata = new Dictionary<string, object>();
                        rowdata.Add("address", "'" + row0[0] + "'");
                        rowdata.Add("district_id", Convert.ToInt32(row0[1]));
                        if (link.SelectNodes(".//tr[@class=\"field\"]") == null)
                        {
                            continue;
                        }

                        foreach (var row in link.SelectNodes(".//tr[@class=\"field\"]"))
                        {
                            HtmlNode nd0 = row.SelectSingleNode("td[1]");
                            HtmlNode nd1 = row.SelectSingleNode("td[2]");
                            string t = Encode(nd0.InnerText).ToLower();
                            string title = nd1.GetAttributeValue("title", t);
                            switch (Encode(title))
                            {
                                case "Region":
                                    rowdata.Add("region", "'" + unshortstr(t, 1) + " " + Encode(nd1.InnerText) + "'");
                                    break;
                                case "District":
                                    rowdata.Add("district", "'" + unshortstr(t, 2) + " " + Encode(nd1.InnerText) + "'");
                                    break;
                                case "Place":
                                case "City":
                                    string t2 = unshortstr(t, 3);
                                    if (t2 == t)
                                        t2 = unshortstr(t, 4);
                                    /*if (t == "мкр" 
                                        && rowdata.ContainsKey("locality"))
                                    {
                                        if (!rowdata.ContainsKey("street"))
                                        {
                                            rowdata.Add("street", "'" + t2 + " " + Encode(nd1.InnerText) + "'");
                                        }
                                        continue;
                                    }*/
                                    if (rowdata.ContainsKey("locality"))
                                    {
                                        rowdata["locality"] = "'" + t2 + " " + Encode(nd1.InnerText) + "'";
                                    }
                                    else
                                    {
                                        rowdata.Add("locality", "'" + t2 + " " + Encode(nd1.InnerText) + "'");
                                    }
                                    break;
                                case "Street":
                                    if (!rowdata.ContainsKey("street"))
                                    {
                                        rowdata.Add("street", "'" + unshortstr(t, 5) + " " + Encode(nd1.InnerText) + "'");
                                    }
                                    else
                                    {
                                        rowdata["street"] = "'" + unshortstr(t, 5) + " " + Encode(nd1.InnerText) + "'";
                                    }
                                    break;
                                case "House":
                                    rowdata.Add("house", "'" + Encode(nd1.InnerText) + "'");
                                    break;
                                case "Географическая широта адресного объекта":
                                    rowdata.Add("lat", "'" + nd1.InnerText + "'");
                                    break;
                                case "Географическая долгота адресного объекта":
                                    rowdata.Add("lon", "'" + nd1.InnerText + "'");
                                    break;
                                case "Код КЛАДР до улицы включительно":
                                    rowdata.Add("code", "'" + nd1.InnerText + "'");
                                    break;
                                default:
                                    break;
                            }
                        }
                        DataBase.RunCommandInsert("oko.geo_address", rowdata);
                    }
                   Thread.Sleep(1000);
                }
                while (link == null && maxcnt-- > 0);
            }
            DataBase.CloseConnection();
        }

        static string getResponse(string uri, string postData)
        {
            System.Net.ServicePointManager.Expect100Continue = false;

            StringBuilder sb = new StringBuilder();
            byte[] buf = new byte[8192];
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "POST";
            request.Headers.Add("Cache-Control", "max-age=0");
            byte[] byteArray = Encoding.Default.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            request.Proxy = new WebProxy("127.0.0.1:8118");
            request.KeepAlive = false;
            request.Timeout = 100 * 60 * 10;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            int count = 0;
            do
            {
                count = resStream.Read(buf, 0, buf.Length);
                if (count != 0)
                {
                    sb.Append(Encoding.Default.GetString(buf, 0, count));
                }
            }
            while (count > 0);
            return sb.ToString();
        }

        static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static private string Encode(string s)
        {
            byte[] bytes = Encoding.Default.GetBytes(s);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
