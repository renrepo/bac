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
using System.Diagnostics;
using System.Threading;

namespace analyser
{    
    class zgc_class
    {
        bg_processing sh = new bg_processing();
        
        #region Fields

        public string class_name;
        public double[] x_vals;
        public double[] y_vals;
        List<LineItem> line_items = new List<LineItem>();
        List<PolyObj> polyobj_item = new List<PolyObj>();

        public ZedGraphControl zgc { get; set; }
        public ZedGraph.GraphPane myPane;
        public bool bMouseDown = false;
        public bool start = true;
        double xVal_left;
        double xVal_right;

        #endregion



        #region Constructor
        public zgc_class(string name, TabControl tc)
        {
            zgc = new ZedGraphControl();
            zgc.AccessibleName = name;
            this.class_name = name;
            myPane = zgc.GraphPane;
            zgc.Dock = DockStyle.Fill;

            TabPage tp = new TabPage(name);
            tc.TabPages.Add(tp);
            tp.Controls.Add(zgc);
            tp.Name = class_name;
            tc.SelectedTab = tp;
            int red = 200;
            int green = 200;
            int blue = 200;
            myPane.Title.Text = "";
            //myPane.Title.Text = "";
            myPane.Title.FontSpec.Size = 8;
            //myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "Binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 8;
            myPane.XAxis.Scale.FontSpec.Size = 8;
            //myPane.XAxis.Scale.IsReverse = true;
            myPane.YAxis.Title.Text = "cps";
            myPane.YAxis.Title.FontSpec.Size = 8;
            myPane.YAxis.Scale.FontSpec.Size = 8;
            myPane.XAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
            myPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
            myPane.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            //myPane.YAxis.Color = Color.FromArgb(90, 30, 0);
            //myPane.XAxis.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
        }
        #endregion


        #region Methods
        public void plot_data(double[] x, double[] y, string curve_name)
        {
            line_items.Add(zgc.GraphPane.AddCurve("", x, y, Color.FromArgb(21, 172, 61), SymbolType.None));
            myPane.CurveList[myPane.CurveList.Count - 1].Tag = curve_name;
            line_items[line_items.Count - 1].Tag = curve_name;
            line_items[line_items.Count - 1].Line.Width = 1;
            zgc.AxisChange();
            zgc.Invalidate();
            x_vals = x;
            y_vals = y;
        }
        

        public Tuple<double,double> select_background()
        {
            return Tuple.Create(1.0,1.0);
        }

        public void tester(string tb)
        {
            myPane.Chart.Fill = new Fill(Color.FromArgb(35, 35, 35));
            zgc.AxisChange();
            zgc.Invalidate();
            Console.WriteLine(tb);
        }

        public void disable_zoom()
        {
            zgc.IsEnableHZoom = false;
            zgc.IsEnableVZoom = false;
        }

        public bool zgc_MouseUpEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (bMouseDown)
            {
                Console.WriteLine(myPane.CurveList.Count);

                //line_items[line_items.Count - 1].Clear();
                //zgc.Refresh();
                //line_items[line_items.Count - 1].
                List<double> x_vals_crop = new List<double>();
                List<double> y_vals_crop = new List<double>();
                foreach (var item in x_vals)
                {
                    if (item > xVal_left && item < xVal_right)
                    {
                        x_vals_crop.Add(item);
                        y_vals_crop.Add(y_vals[Array.IndexOf(x_vals, item)]);
                    }
                }
                double[] erg = sh.integral(x_vals_crop.ToArray(), y_vals_crop.ToArray(),5);
                line_items.Add(myPane.AddCurve(" ", x_vals_crop.ToArray(), erg, Color.Red, SymbolType.None));
                //line_items[line_items.Count - 1].;
                zgc.Invalidate();
                //myPane.CurveList[myPane.CurveList.Count - 1].Clear();
                bMouseDown = false;
                
            }
            return false;
        }

        public bool zgc_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (start)
            {
                xVal_left = x_vals[Array.IndexOf(y_vals, y_vals.Max())] - 1000;
                xVal_right = x_vals[Array.IndexOf(y_vals, y_vals.Max())] + 1000;
                var poly = new ZedGraph.PolyObj
                {
                    Points = new[]
                    {
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Min),
                    new ZedGraph.PointD(xVal_right, myPane.YAxis.Scale.Max),
                    new ZedGraph.PointD(xVal_left, myPane.YAxis.Scale.Max)
                },
                    Fill = new ZedGraph.Fill(Color.FromArgb(204, 255, 204)),
                    ZOrder = ZedGraph.ZOrder.E_BehindCurves,
                };
                poly.Border.Color = Color.FromArgb(153, 255, 153);
                polyobj_item.Add(poly);
                polyobj_item[polyobj_item.Count - 1].Tag = "tag";
                myPane.GraphObjList.Add(poly);
                start = false;
                zgc.Refresh();
                return true;
            }
            bMouseDown = true;
            var result = DrawLine(sender, e, xVal_left, xVal_right);
            xVal_left = result.Item1;
            xVal_right = result.Item2;
            return false;
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

        public Tuple<double,double> DrawLine(ZedGraphControl sender, MouseEventArgs e, double xVal_left, double xVal_right)
        {
            double yVal_left;
            double yVal_right;
            myPane.Legend.IsVisible = false; 
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




        public void testt()
        {
            double[,] xy = new double[,] { { 1, 2 }, { 1, 2 } };
            double[,] xyz = new double[,] { { 1, 2, 4 }, { 1, 2, 8 } };
            List<double[,]> lll = new List<double[,]>();
            lll.Add(xy);
            lll.Add(xyz);
        }


        #endregion




    }
}