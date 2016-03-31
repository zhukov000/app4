using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;


using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace AddrNorm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int i = 0;
            int.TryParse(label1.Text, out i);
            string s0 = "text_to_process=" + textBox1.Text;
            string ss = getResponse("http://ahunter.ru/site/search", s0);
            HtmlDocument doc = new HtmlDocument();
            using(Stream s = GenerateStreamFromString(ss))
            {
                doc.Load(s);
            }
            HtmlNode link = doc.GetElementbyId("FoundAddresses");
            if (link != null)
            {
                foreach (var row in link.SelectNodes(".//tr[@class=\"field\"]"))
                {
                    HtmlNode nd0 = row.SelectSingleNode("td[1]");
                    HtmlNode nd1 = row.SelectSingleNode("td[2]");
                    string t = Encode(nd0.InnerText).ToLower();
                    string title = nd1.GetAttributeValue("title", t);
                    switch (Encode(title))
                    {
                        case "Region":
                            regBox.Text = t + ". " + Encode(nd1.InnerText);
                            break;
                        case "District":
                            disBox.Text = t + ". " + Encode(nd1.InnerText);
                            break;
                        case "Place":
                        case "City":
                            locBox.Text = t + ". " + Encode(nd1.InnerText);
                            break;
                        case "Street":
                            streetBox.Text = t + ". " + Encode(nd1.InnerText);
                            break;
                        case "House":
                            houseBox.Text = t + " " + Encode(nd1.InnerText);
                            break;
                        case "Код КЛАДР до улицы включительно":
                            kladrBox.Text = Encode(nd1.InnerText);
                            break;
                    }
                }
                i++;
                label1.Text = i.ToString();
            }
            else
            {
                MessageBox.Show("Error!!");
            }
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
            byte [] bytes = Encoding.Default.GetBytes(s);
            return Encoding.UTF8.GetString(bytes);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Form2()).ShowDialog();
        }
    }
}
