using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace filewriter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void btn1_Click(object sender, EventArgs e)
        {
            //string path = Path.GetTempFileName();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            List<string> liste = new List<string>();



            //   File.WriteAllLines(path + @"\test.txt", liste);
            string now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
            for (int i = 0; i <= 1000; i++)
            using (var tw = new StreamWriter(path +  @"\test_" + now + ".txt", true))
            {
                {
                        //DateTime now = DateTime.Now;
                        now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");
                        string var = "Beitrag Nummer" + i.ToString("000");
                        string var2 = "zusätzlich" + (2 * i).ToString("000");
                        tw.WriteLine(var +"\t" + var2 + "\t" + now);
                        liste.Add(var + var2);
                        tw.Close();
                        Thread.Sleep(1);

                }
            }
        }
    }
}
