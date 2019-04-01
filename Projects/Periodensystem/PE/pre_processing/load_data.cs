using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.IO;

namespace XPS.pre_processing
{
    class load_data
    {
        PointPairList vals_to_plot = new PointPairList();

        public load_data()
        {
            PointPairList vals_to_plot = new PointPairList();
        }

        public PointPairList get_values_to_plot()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    string[] fileContent = File.ReadAllLines(openFileDialog.FileName);
                    //Read the contents of the file into a stream
                    double energy, cps;
                    string[] vals = new string[2];
                    foreach (var line in fileContent)
                    {
                        vals = line.Split('\t');
                        energy = Convert.ToDouble(vals[0]);
                        cps = Convert.ToDouble(vals[1]);
                        vals_to_plot.Add(energy, cps);
                        //Console.WriteLine();
                    }
                }
            }

            return vals_to_plot;
        }
    }
}
