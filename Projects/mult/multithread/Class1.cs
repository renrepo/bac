using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace multithread
{

    public class Constants
    {
        public int num_gauss = 0;
        public int num_fib = 0;
    }

    public class safer
    {
        static string now = DateTime.Now.ToString("dd_MM_yyyy_hh_mm");

        public static void safe(string path, List<string> liste)
        {
            using (var tw = new StreamWriter(path + now + ".txt", true))
            {
                //tw.WriteLine(var + "\t" + var2 + "\t" + now);
                foreach (var item in liste)
                {
                    tw.Write(item + Environment.NewLine);
                }
                tw.Close();
            }
        }

        public static void safe_line(string path, string line)
        {
            using (var tw = new StreamWriter(path + now + ".txt", true))
            {
                tw.Write(line + Environment.NewLine);
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
