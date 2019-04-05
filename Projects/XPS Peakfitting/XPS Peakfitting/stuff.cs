using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Threading;

namespace XPS_Peakfitting
{
    class stuff
    {

        #region Fields

        private double[] x;
        private double[] y;
        public bool bMouseDown = false;
        public bool start = true;
        double xVal_left;
        double xVal_right;
        List<PolyObj> polyobj_item = new List<PolyObj>();

        #endregion


        #region Properties

        public List<double> X
        {
            get { return null; }
            set { x = value.ToArray(); }
        }
        public List<double> Y
        {
            get { return null; }
            set { y = value.ToArray(); }
        }

        #endregion


        #region Methods
        public Tuple<List<List<double>>, string> get_values_to_plot()
        {
            var list_values = new List<List<double>>();
            //var list_cps = new List<double>();
            //var list_energy = new List<double>();
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
                    List<double> l1 = new List<double>();
                    List<double> l2 = new List<double>();
                    try
                    {
                        foreach (var line in lines)
                        {
                            lin = line.Split('\t');
                            l1.Add(Convert.ToDouble(lin[0],System.Globalization.CultureInfo.InvariantCulture));
                            l2.Add(Convert.ToDouble(lin[1],System.Globalization.CultureInfo.InvariantCulture));
                            
                            //list_values.Add(Convert.ToDouble(lin[0]));
                            //list_values.Add(Convert.ToDouble(lin[1]));
                            //list_energy.Add(Convert.ToDouble(lin[0]));
                            //list_cps.Add(Convert.ToDouble(lin[1]));
                        }
                        list_values.Add(l1);
                        list_values.Add(l2);
                        //var energy = list_energy.ToArray();
                        //var cps = list_cps.ToArray();
                        //List<List<double>> vals = 
                        //double[][] vals = list_values.Select(a => a.ToArray()).ToArray();
                        var file_name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                        return Tuple.Create(list_values, file_name);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        return null;
                    }                   
                    
                }
            }
            return null;
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


        public double[] integral(double[] x_data, double[] y_data, int iterations)
        {
            int data_length = x_data.Length;
            double I_max = y_data[data_length - 1];
            double I_min = y_data[1];
            double[] B_n = new double[data_length];
            double[] B_n_old = new double[data_length];
            double fak = 1.0;

            for (int k = 0; k < data_length; k++)
            {
                B_n_old[k] = 0.0;
                B_n[k] = 0.0;
            }

            //number of iterations
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 1; i < data_length; i++)
                {
                    // Integral from E to E_max
                    for (int j = i; j < data_length; j++)
                    {
                        B_n[i] += ((x_data[j] - x_data[j - 1])) * (0.5 * (y_data[j] + y_data[j - 1]) - I_max - B_n_old[j - 1]);
                    }
                    if (i == 1)
                    {
                        fak = 0.0;
                        for (int l = 1; l < data_length; l++)
                        {
                            fak += ((x_data[l] - x_data[l - 1])) * (0.5 * (y_data[l] + y_data[l - 1]) - I_max - B_n_old[l - 1]);
                        }
                    }
                    B_n[i] *= (I_min - I_max) / fak;
                }
                for (int r = 0; r < data_length; r++)
                {
                    B_n_old[r] = B_n[r];
                    B_n[r] = 0.0;
                }
            }
            for (int i = 0; i < data_length; i++)
            {
                B_n_old[i] += I_max;
            }
            return B_n_old;
        }

        #endregion //--------------------------------------------------------------------------------------



        #region Events
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
                double[] erg = integral(x_vals_crop.ToArray(), y_vals_crop.ToArray(), 5);
                bMouseDown = false;
                List<List<double>> result = new List<List<double>>();
                result.Add(x_vals_crop);
                result.Add(erg.ToList());
                
            }
            return false;
        }

        public bool zgc_MouseDownEvent(ZedGraphControl sender, MouseEventArgs e)
        {
            ZedGraphControl zgc = sender as ZedGraphControl;
            GraphPane myPane = zgc.GraphPane;
            
            if (start)
            {
                xVal_left = x[Array.IndexOf(y, y.Max())] - 1;
                xVal_right = x[Array.IndexOf(y, y.Max())] + 1;
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

        public Tuple<double, double> DrawLine(ZedGraphControl sender, MouseEventArgs e, double xVal_left, double xVal_right)
        {
            ZedGraphControl zgc = sender as ZedGraphControl;
            GraphPane myPane = zgc.GraphPane;
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

        #endregion
    }
}
