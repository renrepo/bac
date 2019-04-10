using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace XPSFit
{
    class stuff
    {
        #region Fields

        ZedGraphControl zgc_plots;
        ZedGraphControl zgc_residuals;
        GraphPane myPane_plots;
        GraphPane myPane_residuals;
        TabPage tp;
        TableLayoutPanel tlp;

        public List<LineItem> List_LineItem = new List<LineItem>();
        public List<PolyObj> List_PolyObj = new List<PolyObj>();

        bool bMouseDown = false;
        double x_left;
        double x_right;

        #endregion //-------------------------------------------------------------------------------------





        #region Properties

        public string Data_name { get; set; }
        public List<double> x { get; set; }
        public List<double> y { get; set; }
        public TabControl tc_zgc { get; set; }

        #endregion //-------------------------------------------------------------------------------------





        #region Constructor

        public stuff(List<double> X_values, List<double> Y_values, string Name, TabControl Tc_zgc)
        {
            x = X_values;
            y = Y_values;
            Data_name = Name;
            tc_zgc = Tc_zgc;
            initial_zgc();
            Draw_Line(x,y,Data_name);
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Methods

        public void initial_zgc()
        {
            zgc_plots = new ZedGraphControl() { AccessibleName = (Data_name + "_plots") };
            zgc_residuals = new ZedGraphControl() { AccessibleName = (Data_name + "_residuals") };
            myPane_plots = zgc_plots.GraphPane;
            myPane_residuals = zgc_residuals.GraphPane;
            tp = new TabPage();
            tlp = new TableLayoutPanel();
            tlp.RowCount = 2;
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 500));
            tlp.ColumnCount = 1;
            tc_zgc.TabPages.Add(tp);
            tp.Controls.Add(tlp);
            tlp.Controls.Add(zgc_residuals);
            tlp.Controls.Add(zgc_plots);
            tlp.Dock = zgc_residuals.Dock = zgc_plots.Dock = DockStyle.Fill;

            tp.Name = tp.Text = Data_name;
            tc_zgc.SelectedTab = tp;

            int red = 200;
            int green = 200;
            int blue = 200;
            myPane_plots.Title.Text = "";
            myPane_plots.Title.IsVisible = false;
            myPane_residuals.Title.IsVisible = myPane_residuals.XAxis.IsVisible = false;
            myPane_plots.XAxis.Title.Text = "Binding energy [eV]";
            myPane_plots.XAxis.Title.FontSpec.Size = myPane_plots.XAxis.Scale.FontSpec.Size = 8;
            myPane_residuals.YAxis.Scale.FontSpec.Size = 16;
            myPane_plots.YAxis.Title.Text = "cps";
            myPane_plots.YAxis.Title.FontSpec.Size = myPane_plots.YAxis.Scale.FontSpec.Size = 8;
            myPane_plots.XAxis.Scale.FontSpec.FontColor = myPane_plots.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
            myPane_plots.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane_plots.YAxis.MajorTic.Color = myPane_plots.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
            myPane_plots.XAxis.MajorTic.Color = myPane_plots.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
            myPane_residuals.TitleGap = 0;           
        }


        public void Draw_Line(List<double> x_values, List<double> y_values, string tag)
        {
            var LI =  myPane_plots.AddCurve("", x_values.ToArray(), y_values.ToArray(), Color.Green, SymbolType.None);
            LI.Tag = tag ?? Data_name;
            LI.IsSelectable = true;
            List_LineItem.Add(LI);
            zgc_plots.AxisChange();
            zgc_plots.Invalidate();
        }


        public void Draw_Polyobj(double x_left, double x_right, string tag)
        {
            var PO = List_PolyObj.Find(a => a.Tag == tag);
            if (PO != null)
            {
                PO.Points = new[]
                    {
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Max),
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Min),
                    new ZedGraph.PointD(x_right, myPane_plots.YAxis.Scale.Min),
                    new ZedGraph.PointD(x_right, myPane_plots.YAxis.Scale.Max),
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Max)
                };
            }

            else
            {
                var PO_new = new ZedGraph.PolyObj
                {
                    Points = new[]
                    {
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Max),
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Min),
                    new ZedGraph.PointD(x_right, myPane_plots.YAxis.Scale.Min),
                    new ZedGraph.PointD(x_right, myPane_plots.YAxis.Scale.Max),
                    new ZedGraph.PointD(x_left, myPane_plots.YAxis.Scale.Max)
                    },
                    Fill = new ZedGraph.Fill(Color.FromArgb(230, 255, 225)),
                    ZOrder = ZedGraph.ZOrder.E_BehindCurves
                };
                PO_new.Border.Color = Color.FromArgb(140, 255, 140);
                PO_new.Tag = tag;
                List_PolyObj.Add(PO_new);
                myPane_plots.GraphObjList.Add(PO_new); // really necessary??
            }
            zgc_plots.AxisChange();
            zgc_plots.Invalidate();           
        }


        public void Draw_Polyobj_initial(string tag)
        {
            var x_max = x[y.IndexOf(y.Max())];
            x_left = x_max - 1.0;
            x_right = x_max + 1.0;
            Draw_Polyobj(x_left, x_right, tag);
        }


        public void Update_Line(List<double> x_values, List<double> y_values, LineItem LI)
        {
            // better modifie CurveObj-List!
            var LI_tag = LI.Tag;
            var index = List_LineItem.FindIndex(a => a == LI);
            if (List_LineItem.Contains(LI))
            {
                List_LineItem.Remove(LI);
                var LI_new = myPane_plots.AddCurve("", x_values.ToArray(), y_values.ToArray(), Color.Green, SymbolType.None);
                LI_new.Tag = LI_tag;
                LI.IsSelectable = true;
                List_LineItem.Insert(index, LI);
                zgc_plots.AxisChange();
                zgc_plots.Invalidate();
            }
        }


        public void Remove_Line(string tag)
        {
            var name = tag ?? Data_name;
            var LI = List_LineItem.Find(a => a.Tag.ToString() == name);
            if (LI != null)
            {
                List_LineItem.Remove(LI);
                LI.Clear();
                zgc_plots.Refresh();
            }          
        }


        public void Add_Mouse_Events()
        {
            zgc_plots.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseDownEvent);
            zgc_plots.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseMoveEvent);
            zgc_plots.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseUpEvent);
            zgc_plots.IsEnableHZoom = zgc_plots.IsEnableVZoom = false;
        }


        public void Remove_Mouse_Events()
        {
            zgc_plots.MouseDownEvent -= new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseDownEvent);
            zgc_plots.MouseMoveEvent -= new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseMoveEvent);
            zgc_plots.MouseUpEvent -= new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zgc_plots_MouseUpEvent);
            zgc_plots.IsEnableHZoom = zgc_plots.IsEnableVZoom = true;
        }


        public bool zgc_plots_MouseDownEvent(object sender, MouseEventArgs e)
        {
            bMouseDown = true;
            return false;
        }


        public bool zgc_plots_MouseMoveEvent(object sender, MouseEventArgs e)
        {
            if (bMouseDown)
            {
                myPane_plots.ReverseTransform(e.Location, out double x_cond, out double y_cond);
                if (0.5 * (x_left + x_right) < x_cond)
                {
                    Draw_Polyobj(x_left, x_cond, "test");
                    x_right = x_cond;
                }
                else
                {
                    Draw_Polyobj(x_cond, x_right, "test");
                    x_left = x_cond;
                }
            }
            return false;
        }


        public bool zgc_plots_MouseUpEvent(object sender, MouseEventArgs e)
        {
            bMouseDown = false;
            return false;
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Events



        #endregion //-------------------------------------------------------------------------------------
    }
}
