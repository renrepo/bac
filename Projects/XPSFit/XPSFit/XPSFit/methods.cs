using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace XPSFit
{
    class methods
    {
        #region Fields



        #endregion //-------------------------------------------------------------------------------------





        #region Properties



        #endregion //-------------------------------------------------------------------------------------





        #region Constructor



        #endregion //-------------------------------------------------------------------------------------





        #region Methods

        public Tuple<List<double>, List<double>, string> get_values_to_plot()
        {
            try
            {
                // var list = new List<List<double>>();
                var list_cps = new List<double>();
                var list_energy = new List<double>();
                string file_name = string.Empty;
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
                        foreach (var line in lines)
                        {
                            lin = line.Split('\t');
                            list_energy.Add(Convert.ToDouble(lin[0], System.Globalization.CultureInfo.InvariantCulture));
                            list_cps.Add(Convert.ToDouble(lin[1], System.Globalization.CultureInfo.InvariantCulture));
                            //ppl.Add(Convert.ToDouble(lin[0]), Convert.ToDouble(lin[1]));
                        }
                        file_name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    }
                    return Tuple.Create(list_energy, list_cps, file_name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
           
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Events



        #endregion //-------------------------------------------------------------------------------------
    }
}
