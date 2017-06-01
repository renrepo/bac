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
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string var;
        string var2;
        List<string> liste = new List<string>();
        string now = DateTime.Now.ToString("yyyy-MM-dd-HH-mm");

        public Form1()
        {
            InitializeComponent();
        }


        private void btn1_Click(object sender, EventArgs e)
        {
            //string path = Path.GetTempFileName();

            //   File.WriteAllLines(path + @"\test.txt", liste);
            for (int i = 0; i <= 5000; i++)
            {
                //DateTime now = DateTime.Now;
                var = "Beitrag Nummer" + i.ToString("000");
                var2 = "zusätzlich" + (2 * i).ToString("000");
                liste.Add(var + "\t" + var2);
                //Thread.Sleep(1);

            }
        }

 //       safer nr1 = new safer();

        private void btn_safe_Click(object sender, EventArgs e)
        {
            /*           using (var tw = new StreamWriter(path + @"\test_" + now + ".txt", true))
                       {
                           //tw.WriteLine(var + "\t" + var2 + "\t" + now);
                           foreach (var item in liste)
                           {
                               tw.Write(item + Environment.NewLine);
                           }
                           tw.Close();
                       }
                       */
            //            nr1.safe(path, liste, now);
            saver sv1 = new saver(path, liste, now);
            sv1.save();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }








    public class safer
      {

          public static void safe(string path, List<string> liste, string now)
          {
              using (var tw = new StreamWriter(path + @"\test_" + now + ".txt", true))
              {
                  //tw.WriteLine(var + "\t" + var2 + "\t" + now);
                  foreach (var item in liste)
                  {
                      tw.Write(item + Environment.NewLine);
                  }
                  tw.Close();
              }
          }
      }
 




    public class saver
    {
        public string p;
        List<string> l;
        string n;

        public saver(string path, List<string> liste, string now)
        {
            p = path;
            l = liste;
            n = now;
        }

        public void save ()
        {
            using (var tw = new StreamWriter(p + @"\test_" + n + ".txt", true))
            {
                //tw.WriteLine(var + "\t" + var2 + "\t" + now);
                foreach (var item in l)
                {
                    tw.Write(item + Environment.NewLine);
                }
                tw.Close();
            }
        }
    }
} 
