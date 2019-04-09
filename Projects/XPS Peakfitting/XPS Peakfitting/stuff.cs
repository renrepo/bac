using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Threading;

namespace XPS_Peakfitting
{
    class stuff
    {
        #region Fields


        #endregion //--------------------------------------------------------------------------------------






        #region Properties


        #endregion //--------------------------------------------------------------------------------------






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
                    List<double> l1 = new List<double>();
                    List<double> l2 = new List<double>();
                    try
                    {
                        foreach (var line in lines)
                        {
                            lin = line.Split('\t');
                            l1.Add(Convert.ToDouble(lin[0],System.Globalization.CultureInfo.InvariantCulture));
                            l2.Add(Convert.ToDouble(lin[1],System.Globalization.CultureInfo.InvariantCulture));
                            
                            //list_values.Add(Convert.ToDouble(lin[0]));
                            //list_values.Add(Convert.ToDouble(lin[1]));
                            //list_energy.Add(Convert.ToDouble(lin[0]));
                            //list_cps.Add(Convert.ToDouble(lin[1]));
                        }
                        list_values.Add(l1);
                        list_values.Add(l2);
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





        public double[] integral(double[] x_data, double[] y_data, int iterations)
        {
            int data_length = x_data.Length;
            double I_max = y_data[data_length - 1];
            double I_min = y_data[1];
            double[] B_n = new double[data_length];
            double[] B_n_old = new double[data_length];
            double fak = 1.0;

            for (int k = 0; k < data_length; k++)
            {
                B_n_old[k] = 0.0;
                B_n[k] = 0.0;
            }

            //number of iterations
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 1; i < data_length; i++)
                {
                    // Integral from E to E_max
                    for (int j = i; j < data_length; j++)
                    {
                        B_n[i] += ((x_data[j] - x_data[j - 1])) * (0.5 * (y_data[j] + y_data[j - 1]) - I_max - B_n_old[j - 1]);
                    }
                    if (i == 1)
                    {
                        fak = 0.0;
                        for (int l = 1; l < data_length; l++)
                        {
                            fak += ((x_data[l] - x_data[l - 1])) * (0.5 * (y_data[l] + y_data[l - 1]) - I_max - B_n_old[l - 1]);
                        }
                    }
                    B_n[i] *= (I_min - I_max) / fak;
                }
                for (int r = 0; r < data_length; r++)
                {
                    B_n_old[r] = B_n[r];
                    B_n[r] = 0.0;
                }
            }
            for (int i = 0; i < data_length; i++)
            {
                B_n_old[i] += I_max;
            }
            return B_n_old;
        }

        #endregion //--------------------------------------------------------------------------------------



        #region Events
        

        

        #endregion //--------------------------------------------------------------------------------------
    }
}
