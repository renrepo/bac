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
using XPS_Peakfitting.Forms;
using System.Threading;

namespace XPS_Peakfitting
{

    public partial class Form1 : Form
    {


        #region Fields

        private ZedGraphControl zgc;
        private int current_tp_index;
        private string current_tp_name;
        private Form2 f2;
        stuff stuff = new stuff();
        private List<ZedGraphControl> _list_zgc = new List<ZedGraphControl>();
        private List<LineItem> _list_lineitem_raw = new List<LineItem>();

        public bool bMouseDown = false;
        public bool start = true;
        double xVal_left;
        double xVal_right;
        List<PolyObj> polyobj_item = new List<PolyObj>();
        public double[] x;
        public double[] y;
        private double[] x_bg;
        private double[] y_bg;

        #endregion //--------------------------------------------------------------------------------------





        #region Properties

        public ZedGraphControl ZGC
        {
            get { return _list_zgc[_list_zgc.FindIndex(x => x.AccessibleName == (tc_plots.SelectedTab.Name + "_plots"))]; }
            set { zgc = value; }
        }

        public int Current_tp_index
        {
            get { return tc_plots.SelectedIndex; }
            set { current_tp_index = value; }
        }

        public string Current_tp_name
        {
            get { return tc_plots.SelectedTab.Name; }
            set { current_tp_name = value; }
        }

        #endregion //--------------------------------------------------------------------------------------





        #region Constructor
        public Form1()
        {
            f2 = new Form2(this); // access Form2 via properties
            InitializeComponent();
        }
        #endregion //--------------------------------------------------------------------------------------






        #region Methods

        public void Tester()
        {
            Console.WriteLine("test");
        }

        #endregion //--------------------------------------------------------------------------------------





        #region Events
        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = stuff.get_values_to_plot();

            if (data == null)
            {
                return;
            }

            if (tc_plots.Contains(_list_zgc.Find(x => x.AccessibleName == (data.Item2 + "_plots"))))
            {
                tc_plots.SelectTab(data.Item2);
            }

            else
            {
                
                ZedGraphControl zgc_plots = new ZedGraphControl() { AccessibleName = (data.Item2 + "_plots") };
                _list_zgc.Add(zgc_plots);
                ZedGraphControl zgc_residuals = new ZedGraphControl() { AccessibleName = (data.Item2 + "_residuals") };
                _list_zgc.Add(zgc_residuals);

                GraphPane myPane_plots = zgc_plots.GraphPane;
                GraphPane myPane_residuals = zgc_residuals.GraphPane;
                TabPage tp = new TabPage();
                TableLayoutPanel tlp = new TableLayoutPanel( );
                tlp.RowCount = 2;
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 500));
                tlp.ColumnCount = 1;
                tc_plots.TabPages.Add(tp);
                tp.Controls.Add(tlp);
                tlp.Controls.Add(zgc_residuals);
                tlp.Controls.Add(zgc_plots);
                tlp.Dock = DockStyle.Fill;
                zgc_residuals.Dock = DockStyle.Fill;
                zgc_plots.Dock = DockStyle.Fill;

                tp.Name = tp.Text = data.Item2;
                tc_plots.SelectedTab = tp;

                int red = 200;
                int green = 200;
                int blue = 200;
                myPane_plots.Title.Text = "";
                myPane_residuals.Title.Text = "";
                myPane_plots.Title.IsVisible = false;
                myPane_residuals.Title.IsVisible = false;
                //myPane.Title.Text = "";
                //myPane_plots.Title.FontSpec.Size = 8;
                //myPane.TitleGap = 1.6f;
                myPane_plots.XAxis.Title.Text = "Binding energy [eV]";
                myPane_plots.XAxis.Title.FontSpec.Size = 8;
                myPane_plots.XAxis.Scale.FontSpec.Size = 8;
                myPane_residuals.XAxis.IsVisible = false;
                //myPane.XAxis.Scale.IsReverse = true;
                myPane_plots.YAxis.Title.Text = "cps";
                //myPane_residuals.YAxis.IsVisible = false;
                myPane_plots.YAxis.Title.FontSpec.Size = 8;
                myPane_plots.YAxis.Scale.FontSpec.Size = 8;
                myPane_plots.XAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
                myPane_plots.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
                myPane_plots.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
                //myPane.YAxis.Color = Color.FromArgb(90, 30, 0);
                //myPane.XAxis.Color = Color.FromArgb(red, green, blue);
                myPane_plots.YAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
                myPane_plots.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
                myPane_plots.XAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
                myPane_plots.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);

                /***
                foreach (var item in _list_zgc)
                {
                    Console.WriteLine(item.AccessibleName);
                }
                ***/
                f2.Raw_bg_data_item = data.Item1;               
                Draw_line(zgc_plots, data.Item1);
            }

        }

        private void btn_analyse_Click(object sender, EventArgs e)
        {
            if (f2.IsDisposed)
            {
                f2 = new Form2(this);
            }
            f2.Show(); 
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //f2 = new Form2(this);
        }

        private void btn_close_tab_Click(object sender, EventArgs e)
        {
            
            if (tc_plots.SelectedTab !=null)
            {
                var index = _list_zgc.FindIndex(x => x.AccessibleName == tc_plots.SelectedTab.Name + "_plots");
                _list_zgc.RemoveRange(index, 2);
                f2.remove_at_index = index / 2;
                tc_plots.SelectedTab.Dispose();
            }
        }


       

        public bool zgc_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            //ZedGraphControl zgc = sender as ZedGraphControl;
            //GraphPane myPane = zgc.GraphPane;

            bMouseDown = true;
            //var result = DrawLine(sender, e , xVal_left, xVal_right);
            //xVal_left = result.Item1;
            //xVal_right = result.Item2;
            return true;
        }

        public bool zgc_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (bMouseDown)
            {
                var result = DrawLine(sender, e, xVal_left, xVal_right);
                xVal_left = result.Item1;
                xVal_right = result.Item2;
            }
            return false;
        }


        public bool zgc_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (bMouseDown)
            {
                List<double> x_vals_crop = new List<double>();
                List<double> y_vals_crop = new List<double>();
                foreach (var item in x)
                {
                    if (item > xVal_left && item < xVal_right)
                    {
                        x_vals_crop.Add(item);
                        y_vals_crop.Add(y[Array.IndexOf(x, item)]);
                    }
                }
                double[] erg = stuff.integral(x_vals_crop.ToArray(), y_vals_crop.ToArray(), 5);
                bMouseDown = false;
                List<List<double>> result = new List<List<double>>();
                result.Add(x_vals_crop);
                result.Add(erg.ToList());
                x_bg = x_vals_crop.ToArray();
                y_bg = y_vals_crop.ToArray();
                f2.Bg_values = xVal_left;
                f2.Bg_values = xVal_right;

                //f2.Bg_start_stop = xVal_right;
                //f2.Bg_start_stop = xVal_left;

            }
            return false;
        }



        public Tuple<double, double> DrawLine(ZedGraphControl sender, MouseEventArgs e, double xVal_left, double xVal_right)
        {
            ZedGraphControl zgc = sender as ZedGraphControl;
            GraphPane myPane = zgc.GraphPane;
            double yVal_left;
            double yVal_right;
            myPane.Legend.IsVisible = false;
            try
            {
                myPane.ReverseTransform(e.Location, out double x_cond, out double y_cond);
                if (0.5 * (xVal_right + xVal_left) < x_cond)
                {
                    myPane.ReverseTransform(e.Location, out xVal_right, out yVal_right);
                    polyobj_item[polyobj_item.Count - 1].Points = new[]
                        {
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max)
                };
                }

                else
                {
                    myPane.ReverseTransform(e.Location, out xVal_left, out yVal_left);
                    polyobj_item[polyobj_item.Count - 1].Points = new[]
                        {
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max)
                };
                }

                zgc.Refresh();
                Thread.Sleep(20);
                return Tuple.Create(xVal_left, xVal_right);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null ;
            }


        }


        public void Draw_line(ZedGraphControl zgc, List<List<double>> values)
        {
            double[][] vals = values.Select(a => a.ToArray()).ToArray();
            zgc.GraphPane.AddCurve("", vals[0], vals[1], Color.FromArgb(21, 172, 61), SymbolType.None);
            //myPane.CurveList[myPane.CurveList.Count - 1].Tag = curve_name;
            //line_items[line_items.Count - 1].Tag = curve_name;
            //line_items[line_items.Count - 1].Line.Width = 1;
            zgc.AxisChange();
            zgc.Invalidate();
            //x_vals = x;
            //y_vals = y;
        }

        #endregion //--------------------------------------------------------------------------------------


    }
}
