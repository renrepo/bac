using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.IO;

namespace XPS
{
    public partial class Analyser : Form
    {
        GraphPane gp_analyzer;
        Quantify Quantify_form = new Quantify();
        public Analyser()
        {
            InitializeComponent();
            
        }

        private void Analyser_Load(object sender, EventArgs e)
        {
            //Zgc_analyzer = new ZedGraph.ZedGraphControl();
            gp_analyzer = zgc_analyzer.GraphPane;
        }

        private void btn_ana_load_data_Click(object sender, EventArgs e)
        {
            //var fileContent = string.Empty;
            PointPairList vals_to_plot = new PointPairList();

            pre_processing.load_data n1 = new pre_processing.load_data();

            vals_to_plot = n1.get_values_to_plot();

            LineItem Curve = gp_analyzer.AddCurve("", vals_to_plot, Color.FromArgb(21, 172, 61), SymbolType.None);
            Curve.Line.Width = 1;
            Curve.Tag = 2; ;
            zgc_analyzer.AxisChange();
            zgc_analyzer.Invalidate();
            //MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            //System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        private void btn_quantiy_Click(object sender, EventArgs e)
        {
            if (Quantify_form.IsDisposed)
            {
                Quantify_form = new Quantify();
            }

            Quantify_form.Show();
        }

        private void Analyser_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!Quantify_form.IsDisposed)
            {
                Quantify_form.Close();
            }
        }
    }
}
