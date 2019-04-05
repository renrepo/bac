using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XPS_Peakfitting
{
    class stuff
    {


        #region Methods
        public Tuple<List<List<double>>, string> get_values_to_plot()
        {
            var list_values = new List<List<double>>();
            //var list_cps = new List<double>();
            //var list_energy = new List<double>();
            //PointPairList ppl = new PointPairList();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var lines = File.ReadLines(openFileDialog.FileName);
                    string[] lin;
                    List<double> lin2 = new List<double>();
                    try
                    {
                        foreach (var line in lines)
                        {
                            lin = line.Split('\t');
                            lin2.Add(Convert.ToDouble(lin[0]));
                            lin2.Add(Convert.ToDouble(lin[1]));
                            list_values.Add(lin2);
                            //list_values.Add(Convert.ToDouble(lin[0]));
                            //list_values.Add(Convert.ToDouble(lin[1]));
                            //list_energy.Add(Convert.ToDouble(lin[0]));
                            //list_cps.Add(Convert.ToDouble(lin[1]));
                        }
                        //var energy = list_energy.ToArray();
                        //var cps = list_cps.ToArray();
                        //List<List<double>> vals = 
                        //double[][] vals = list_values.Select(a => a.ToArray()).ToArray();
                        var file_name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        return Tuple.Create(list_values, file_name);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return null;
                    }                   
                    
                }
            }
            return null;
        }



        #endregion //--------------------------------------------------------------------------------------
    }
}
