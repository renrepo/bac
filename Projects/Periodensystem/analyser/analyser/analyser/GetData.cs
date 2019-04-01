using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace analyser
{
    class GetData
    {

        #region Fields
        //--- Fields -----------------------		
        //public list = new List;
        //-------------------------------------
        #endregion

        #region Methods
        public Tuple<double[],double[],string> get_values_to_plot()
        {
            // var list = new List<List<double>>();
            var list_cps = new List<double>();
            var list_energy = new List<double>();
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
                    string[] lin ;
                    foreach (var line in lines)
                    {
                        lin = line.Split('\t');
                        list_energy.Add(Convert.ToDouble(lin[0]));
                        list_cps.Add(Convert.ToDouble(lin[1]));
                        //ppl.Add(Convert.ToDouble(lin[0]), Convert.ToDouble(lin[1]));
                    }
                    var energy = list_energy.ToArray();
                    var cps = list_cps.ToArray();
                    var file_name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    return Tuple.Create(energy, cps, file_name);             
                }
            }
            return null;
        }
        # endregion

    }
}
