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
        Fitmodels fm;
        GetData GD;
        bg_processing sh;
        List<zgc_class> zgc_list = new List<zgc_class>();

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
            sh = new bg_processing();
            fm = new Fitmodels();
        }
        #endregion

//#################################################################################################################################################################################
        #region Methods
        private void btn_load_data_Click(object sender, EventArgs e)
        {    
            var data = GD.get_values_to_plot();

            //if (zgc_class.ContainsKey(data.Item3))
            if (zgc_list.Contains(zgc_list.Find(x => x.class_name == data.Item3)))
            {
                tc_plots.SelectTab(data.Item3);
            }

            else
            {
                zgc_class zw = new zgc_class(data.Item3, tc_plots);
                zgc_list.Add(zw);
                //zgc_class.Add(data.Item3, zw);
                //zw.new_zgc(tc_plots);
                zw.plot_data(data.Item1, data.Item2, data.Item3 + "_data");
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
                //zgc_class.Remove(tc_plots.SelectedTab.Name);
                zgc_list.RemoveAt(zgc_list.FindIndex(x => x.class_name == tc_plots.SelectedTab.Name));
                tc_plots.TabPages.Remove(tc_plots.SelectedTab);
                foreach (var item in zgc_list)
                {
                    Console.WriteLine(item.class_name);
                }
            }
                             
        }

        #endregion

        private void btn_tester_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {
                //zgc_class[tc_plots.SelectedTab.Name].tester(tc_plots.SelectedTab.Name);
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
                var zc = zgc_list.Find(x => x.class_name == tc_plots.SelectedTab.Name);
                zc.disable_zoom();
                //zgc_class[tc_plots.SelectedTab.Name].disable_zoom();
                //Console.WriteLine(zgc_class[tc_plots.SelectedTab.Name].class_name);
                zc.zgc.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseDownEvent);
                zc.zgc.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseUpEvent);
                zc.zgc.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseMoveEvent);
                //zgc_class[tc_plots.SelectedTab.Name].DrawLine(ZedGraphControl sender, MouseEventArgs e, 360, 370);
                //double[] x = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //double[] y = new double[] { 1.0, 2.0, 3.0, 4.0, 5.0 };
                //zgc_class[tc_plots.SelectedTab.Name].plot_data(x,y);

            }
        }

        private void btn_bg_add_Click(object sender, EventArgs e)
        {
            // zgc_class[tc_plots.SelectedTab.Name].start = true;
            //var name = zgc_class[tc_plots.SelectedTab.Name].zgc.AccessibleName;

            List<ZedGraphControl> zgc_list = new List<ZedGraphControl>();
            //ZedGraphControl z1 = new ZedGraphControl();
            zgc_list.Add(new ZedGraphControl() { AccessibleName = "z1" });
            zgc_list[zgc_list.Count - 1].AccessibleName = "z1";
            zgc_list.Add(new ZedGraphControl());
            zgc_list[zgc_list.Count - 1].AccessibleName = "z2";
            zgc_list.Add(new ZedGraphControl());
            zgc_list[zgc_list.Count - 1].AccessibleName = "z3";
            zgc_list.RemoveAt(zgc_list.FindIndex(x => x.AccessibleName == "z1"));
            zgc_list.Add(new ZedGraphControl());
            zgc_list[zgc_list.Count - 1].AccessibleName = "z4";
            zgc_list.Add(new ZedGraphControl());
            zgc_list[zgc_list.Count - 1].AccessibleName = "z5";

            foreach (var item in zgc_list)
            {
                Console.WriteLine(item.AccessibleName);
            }


        }

        private void tc_plots_SelectedIndexChanged(object sender, EventArgs e)
        {
            fm.tb_changed(tc_plots.SelectedTab.Name);
        }
    }

}
