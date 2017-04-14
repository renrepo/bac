using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace multithread
{
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
