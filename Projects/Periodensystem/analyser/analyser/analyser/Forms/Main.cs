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
using analyser.Forms;
using System.Diagnostics;

namespace analyser
{
    public partial class Main : Form
    {
        Fitmodels fm = new Fitmodels();
        GetData GD;
        shirley sh;
        //Dictionary<string, ZedGraphControl> zgc_dic = new Dictionary<string, ZedGraphControl>();
        Dictionary<string, zedgraph> zgc_class = new Dictionary<string, zedgraph>();


        #region Constructor
        public Main()
        {
            InitializeComponent();
        }

        #endregion


        #region Load_Main
        private void Main_Load(object sender, EventArgs e)
        {
            GD = new GetData();
            sh = new shirley();
        }
        #endregion

//#################################################################################################################################################################################
        #region Methods
        private void btn_load_data_Click(object sender, EventArgs e)
        {    
            var data = GD.get_values_to_plot();

            if (zgc_class.ContainsKey(data.Item3))
            {
                tc_plots.SelectTab(data.Item3);
            }

            else
            {
                zedgraph zw = new zedgraph(data.Item3);
                zgc_class.Add(data.Item3, zw);
                zw.new_zgc(tc_plots);
                zw.plot_data(data.Item1, data.Item2);
            }                 
        }

        private void btn_processing_Click(object sender, EventArgs e)
        {
            double[] x = new double[650];
            double[] y = new double[650];
            for (int i = 0; i < 650; i++)
            {
                x[i] = 65.0 - i/10.0;
                y[i] = Math.Exp(-Math.Pow((x[i] - 40.0),2) / 10.0) + Math.Exp(-Math.Pow((x[i] - 60.0), 2) / 5.0) + Math.Pow(x[i],2) /10000.0;
            }
            //LineItem Curve = zgc.GraphPane.AddCurve("", x,y, Color.FromArgb(21, 172, 61), SymbolType.None);
            //Curve.Line.Width = 1;
            //Curve.Tag = 2; ;
            //zgc =  zg.new_zgc("shirley", tc_plots);
            //zg.new_zgc("shirley", tc_plots);
            //zg.plot_data(x,y);
            // double[] x = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0, 6.0, 7.0, 8.0, 9.0, 10.0, 11.0, 12.0, 13.0, 14.0, 15.0, 16.0, 17.0, 18.0, 19.0, 20.0,21.0,22.0,23.0,24.0,25.0 };
            //double[] y = new double[] { 0.0, 1.0, 2.0, 1.0, 3.0, 4.0, 6.0, 8.0, 12.0, 19.0, 30.0, 49.0, 38.0, 27.0, 20.0, 14.0, 10.0, 8.0, 5.0, 6.0,5.5,6.0,7.0,6.5,4.5 };
            
            Stopwatch sw = new Stopwatch();
            double[] B;
            sw.Start();
            B = sh.integral(x, y, 10);
            var erg = sw.Elapsed.TotalMilliseconds;
            sw.Stop();
            //zg.plot_data(x, B);
            //LineItem Curve2 = zgc.GraphPane.AddCurve("", x, B, Color.FromArgb(21, 172, 61), SymbolType.None);
            //zgc.AxisChange();
            //zgc.Invalidate();
            foreach (var item in B)
            {               
                Console.WriteLine(item);
            }
            Console.WriteLine(erg);
            sw.Reset();
            

            if (fm.IsDisposed)
            {
                fm = new Fitmodels();
            }
            fm.Show();
        }
        

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {
                zgc_class.Remove(tc_plots.SelectedTab.Name);
                tc_plots.TabPages.Remove(tc_plots.SelectedTab);
            }
                             
        }

        #endregion

        private void btn_tester_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {
                zgc_class[tc_plots.SelectedTab.Name].tester(tc_plots.SelectedTab.Name);
               //double[] x = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //double[] y = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //zgc_class[tc_plots.SelectedTab.Name].plot_data(x,y);

            }
                
            //ZedGraphControl z = zgc_dic[tc_plots.SelectedTab.Name];
            //zgc_class[tc_plots.SelectedTab.Name].GraphPane.Chart.Fill = new Fill(Color.FromArgb(35, 35, 35));
            //z.GraphPane.Chart.Fill = new Fill(Color.FromArgb(35, 35, 35));
            //zgc_class[tc_plots.SelectedTab.Name].AxisChange();
            //zgc_class[tc_plots.SelectedTab.Name].Invalidate();
        }

        private void btn_bg_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {

                zgc_class[tc_plots.SelectedTab.Name].disable_zoom();
                zgc_class[tc_plots.SelectedTab.Name].zgc.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_class[tc_plots.SelectedTab.Name].zgc_MouseDownEvent);
                zgc_class[tc_plots.SelectedTab.Name].zgc.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_class[tc_plots.SelectedTab.Name].zgc_MouseUpEvent);
                zgc_class[tc_plots.SelectedTab.Name].zgc.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_class[tc_plots.SelectedTab.Name].zgc_MouseMoveEvent);
                //double[] x = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //double[] y = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //zgc_class[tc_plots.SelectedTab.Name].plot_data(x,y);

            }
        }
    }

}
