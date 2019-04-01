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
using System.Threading;

namespace analyser
{
    

    class zedgraph
    {

        #region Fields

        public string class_name;
        public double[] x_vals;
        public double[] y_vals;
        public ZedGraphControl zgc { get; set; }
        GraphPane myPane;
        public bool bMouseDown = false;
        PointPairList userClickrList;
        LineItem userClickCurve;


        #endregion

        #region Constructor
        public zedgraph(string name)
        {
            zgc = new ZedGraphControl();
            this.class_name = name;
            
        }
        #endregion


        #region Methods
        public void plot_data(double[] x, double[] y)
        {

            LineItem curve = zgc.GraphPane.AddCurve("", x, y, Color.FromArgb(21, 172, 61), SymbolType.None);
            curve.Line.Width = 1;
            //curve.Tag = 2; ;
            zgc.AxisChange();
            zgc.Invalidate();
            x_vals = x;
            y_vals = y;
        }
        
        public void new_zgc(TabControl tc)
        {           
            zgc.Name = class_name;
            zgc.Anchor = AnchorStyles.Top;
            zgc.Anchor = AnchorStyles.Left;
            zgc.Anchor = AnchorStyles.Bottom;
            zgc.Anchor = AnchorStyles.Right;
            zgc.Dock = DockStyle.Fill;

            TabPage tp = new TabPage(class_name);
            tc.TabPages.Add(tp);
            tp.Controls.Add(zgc);
            tp.Name = class_name;
            tc.SelectedTab = tp;

            myPane = zgc.GraphPane;
            int red = 200;
            int green = 200;
            int blue = 200;
            myPane.Title.Text = "";
            //myPane.Title.Text = "";
            myPane.Title.FontSpec.Size = 8;
            //myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "Binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 8;
            myPane.XAxis.Scale.FontSpec.Size = 9;
            //myPane.XAxis.Scale.IsReverse = true;
            myPane.YAxis.Title.Text = "cps";
            myPane.YAxis.Title.FontSpec.Size = 8;
            myPane.YAxis.Scale.FontSpec.Size = 9;
            //myPane.Fill.Color = Color.LightGray;
            // This will do the area outside of the graphing area
            //myPane.Fill = new Fill(Color.FromArgb(45, 45, 45));
            // This will do the area inside the graphing area
            //myPane.Chart.Fill = new Fill(Color.FromArgb(35, 35, 35));
            //myPane.Chart.Border.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.Scale.FontSpec.FontColor = Color.FromArgb(20,20,20);
            myPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(20,20,20);
            myPane.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.YAxis.Color = Color.FromArgb(90, 30, 0);
            myPane.XAxis.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);

            userClickrList = new PointPairList();
            userClickCurve = new LineItem("userClickCurve");
            //return zgc;

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
            bMouseDown = false;
            return false;
        }

        public bool zgc_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            bMouseDown = true;
            DrawLine(sender, e);
            return false;
        }

        public bool zgc_MouseMoveEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            if (bMouseDown) DrawLine(sender, e);
            return false;
        }

        private void DrawLine(ZedGraphControl sender, MouseEventArgs e)
        {
           // GraphPane myPane = zedGraphControl1.GraphPane;

            // x & y variables to store the axis values
            double xVal;
            double yVal;

            xVal = x_vals[Array.IndexOf(y_vals,y_vals.Max())] ;
            //xVal_right = x_vals.Max() + 1.0;

            // Clear the previous values if any
            userClickrList.Clear();

            myPane.Legend.IsVisible = false;

            // Use the current mouse locations to get the corresponding 
            // X & Y CO-Ordinates         
            

            myPane.ReverseTransform(e.Location, out xVal, out yVal);

            // Create a list using the above x & y values
            // Add a curve
            userClickrList.Add(xVal, myPane.YAxis.Scale.Max);
            userClickrList.Add(xVal, myPane.YAxis.Scale.Min);

            userClickCurve = myPane.AddCurve(" ", userClickrList, Color.Red, SymbolType.None);
            userClickCurve = myPane.AddCurve(" ", userClickrList, Color.Red, SymbolType.None);

            zgc.Refresh();
            ;

        
        }


        #endregion
    }
}