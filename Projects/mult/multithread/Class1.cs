using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace multithread
{

    public static class Constants
    {
        public const int fav = 3;
    }

    public class safer
    {
       static string now = DateTime.Now.ToString("dd_MM_yyyy");

        public static void safe(string path, List<string> liste)
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
        string n = DateTime.Now.ToString("dd_MM_yyyy");

        public saver(string path, List<string> liste)
        {
            p = path;
            l = liste;
        }

        public void save()
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
