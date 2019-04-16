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
    class stuff : methods
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
        //public Dictionary<string, double> Bg_Bounds = new Dictionary<string, double>();
        public List<double> Bg_Bounds = new List<double>();
        public List<double[]> Bg_Sub = new List<double[]>();

        bool bMouseDown = false;
        bool MouseEventExist = false;


        #endregion //-------------------------------------------------------------------------------------





        #region Properties

        public string Data_name { get; set; }
        public List<double> x { get; set; }
        public List<double> y { get; set; }
        public TabControl tc_zgc { get; set; }
        public int Bg_tag_num { get; set; }
        public string Bg_tag_type { get; set; }
        public double x_bg_left { get; set; }
        public double x_bg_right { get; set; }

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
            myPane_plots.XAxis.Scale.Min = x[0];
            myPane_plots.XAxis.Scale.Max = x[x.Count - 1];
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
            LineItem LI;
            if (List_LineItem.Contains(List_LineItem.Find(a => a.Tag.ToString() == tag)))
            {
                LI = List_LineItem.Find(a => a.Tag.ToString() == tag);
                var LI_tag = LI.Tag;
                var index = List_LineItem.FindIndex(a => a == LI);
                List_LineItem.Remove(LI);
                myPane_plots.CurveList.Remove(LI);
                var LI_new = myPane_plots.AddCurve("", x_values.ToArray(), y_values.ToArray(), Color.Green, SymbolType.None);
                LI_new.Tag = LI_tag;
                List_LineItem.Insert(index, LI_new);
            }
            else
            {
                LI = myPane_plots.AddCurve("", x_values.ToArray(), y_values.ToArray(), Color.Green, SymbolType.None);
                LI.Tag = tag ?? Data_name;
                List_LineItem.Add(LI);
            }
            LI.IsSelectable = true;
            zgc_plots.AxisChange();
            zgc_plots.Invalidate();

        }


        public void Draw_Polyobj()
        {
            double X_left;
            double X_right;
            var PO = List_PolyObj.Find(a => Convert.ToInt16(a.Tag) == Bg_tag_num);
   
            double Y_max = myPane_plots.YAxis.Scale.Max;
            double Y_min = myPane_plots.YAxis.Scale.Min;

            if (PO != null)
            {
                X_left = Bg_Bounds[Bg_tag_num * 2];
                X_right = Bg_Bounds[Bg_tag_num * 2 + 1];

                PO.Points = new[]
                    {
                    new ZedGraph.PointD(X_left, Y_max),
                    new ZedGraph.PointD(X_left, Y_min),
                    new ZedGraph.PointD(X_right, Y_min),
                    new ZedGraph.PointD(X_right, Y_max),
                    new ZedGraph.PointD(X_left, Y_max)
            };
                PO.IsVisible = true;
            }

            else
            {
                var max = x[y.IndexOf(y.Max())];
                Bg_Bounds.Add(max - 1.0);
                Bg_Bounds.Add(max + 1.0);
                X_left = max - 1.0;
                X_right = max + 1.0;

                var PO_new = new ZedGraph.PolyObj
                {
                    Points = new[]
                    {
                    new ZedGraph.PointD(X_left, Y_max),
                    new ZedGraph.PointD(X_left, Y_min),
                    new ZedGraph.PointD(X_right, Y_min),
                    new ZedGraph.PointD(X_right, Y_max),
                    new ZedGraph.PointD(X_left, Y_max)
                },
                    Fill = new ZedGraph.Fill(Color.FromArgb(240, 255, 240)),
                    ZOrder = ZedGraph.ZOrder.E_BehindCurves
                };
                PO_new.Border.Color = Color.FromArgb(140, 255, 140);
                PO_new.Tag = Bg_tag_num;
                List_PolyObj.Add(PO_new);
                myPane_plots.GraphObjList.Add(PO_new); // really necessary !!
            }
            zgc_plots.AxisChange();
            zgc_plots.Invalidate();           
        }



        public void Remove_PolyObj()
        {
            var GO = List_PolyObj.Find(a => Convert.ToInt16(a.Tag) == Bg_tag_num);
            if (GO != null)
            {
                myPane_plots.GraphObjList.Remove(GO);
                List_PolyObj.Remove(GO);
                zgc_plots.AxisChange();
                zgc_plots.Invalidate();
            }
        }

        public void Hide_PolyObj(int tag)
        {
            var GO = List_PolyObj.Find(a => Convert.ToInt16(a.Tag) == tag);
            if (GO != null)
            {
                GO.IsVisible = false;
                zgc_plots.AxisChange();
                zgc_plots.Invalidate();
            }
        }


        public void Remove_Line(string tag)
        {
            var name = tag ?? Data_name;
            var LI = List_LineItem.Find(a => a.Tag.ToString() == tag.ToString());
            if (LI != null)
            {
                myPane_plots.CurveList.Remove(LI);
                List_LineItem.Remove(LI);
                LI.Clear();
                zgc_plots.Refresh();
            }          
        }

        public void Hide_Line(string tag)
        {
            var name = tag ?? Data_name;
            var LI = List_LineItem.Find(a => a.Tag.ToString() == tag.ToString());
            if (LI != null)
            {
                LI.IsVisible = false;
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
            var x_left = Bg_Bounds[Bg_tag_num * 2];
            var x_right = Bg_Bounds[Bg_tag_num * 2 + 1];
            if (bMouseDown)
            {
                myPane_plots.ReverseTransform(e.Location, out double x_cond, out double y_cond);
                if (0.5 * (x_left + x_right) < x_cond) x_right = x_cond;
                else x_left = x_cond;
                Draw_Polyobj();
                Bg_Bounds[Bg_tag_num * 2] = x_left;
                Bg_Bounds[Bg_tag_num * 2 + 1] = x_right;
            }
            return false;
        }


        public bool zgc_plots_MouseUpEvent(object sender, MouseEventArgs e)
        {
            bMouseDown = false;

            List<double> x_vals_crop = new List<double>();
            List<double> y_vals_crop = new List<double>();
            List<double> erg = new List<double>();
            int counter = 0;

            foreach (var item in x)
            {
                if (item > Bg_Bounds[Bg_tag_num * 2] && item < Bg_Bounds[Bg_tag_num * 2 + 1])
                {
                    x_vals_crop.Add(item);
                    y_vals_crop.Add(y[x.IndexOf(item)]);
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            switch (Bg_tag_type)
            {
                case "Shirley":
                    erg = Shirley(x_vals_crop.ToArray(), y_vals_crop.ToArray(), 10);
                    Draw_Line(x_vals_crop, erg, Bg_tag_num.ToString());
                    break;
                case "Linear":
                    erg = Linear(x_vals_crop.ToArray(), y_vals_crop.ToArray());
                    Draw_Line(x_vals_crop, erg, Bg_tag_num.ToString());
                    break;
            }

            if (Bg_tag_num < Bg_Sub.Count)
            {
                var bg = Bg_Sub[Bg_tag_num];
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] > Bg_Bounds[Bg_tag_num * 2] && x[i] < Bg_Bounds[Bg_tag_num * 2 + 1]) { bg[i] = erg[counter]; counter++; }
                    else bg[i] = 0;
                }
            }
            else
            {
                double[] bg = new double[x.Count];
                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] > Bg_Bounds[Bg_tag_num * 2] && x[i] < Bg_Bounds[Bg_tag_num * 2 + 1]) { bg[i] = erg[counter]; counter++; }
                    else bg[i] = 0;
                }
                Bg_Sub.Add(bg);
            }

            Cursor.Current = Cursors.Default;

            return false;
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Events



        #endregion //-------------------------------------------------------------------------------------
    }
}
