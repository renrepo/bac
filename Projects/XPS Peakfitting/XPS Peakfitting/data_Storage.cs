using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;
using System.Drawing;

namespace XPS_Peakfitting
{
    class data_storage
    {



        #region Fields
        public List<double> bg = new List<double>();


        #endregion //--------------------------------------------------------------------------------------






        #region Constructor
        public data_storage(string name, List<double> x_vals, List<double> y_vals)
        {
            Name = name;
            x_raw = x_vals;
            y_raw = y_vals;

        }


        #endregion //--------------------------------------------------------------------------------------






        #region Properties


        public string Name { get; set; }
        public List<double> x_raw { get; set; }
        public List<double> y_raw { get; set; }


        #endregion //--------------------------------------------------------------------------------------







        #region Methods

        public void draw_objectboxes(ZedGraphControl zgc)
        {
            foreach (var item in bg)
            {
                var poly = new ZedGraph.PolyObj
                {
                    Points = new[]
                {
                new ZedGraph.PointD(item, zgc.GraphPane.YAxis.Scale.Max),
                new ZedGraph.PointD(item, zgc.GraphPane.YAxis.Scale.Min),
                new ZedGraph.PointD(item + 1, zgc.GraphPane.YAxis.Scale.Min),
                new ZedGraph.PointD(item + 1, zgc.GraphPane.YAxis.Scale.Max),
                new ZedGraph.PointD(item + 1 , zgc.GraphPane.YAxis.Scale.Max)
                },
                    Fill = new ZedGraph.Fill(Color.FromArgb(204, 255, 204)),
                    ZOrder = ZedGraph.ZOrder.E_BehindCurves,
                };
                poly.Border.Color = Color.FromArgb(153, 255, 153);
                //polyobj_item.Add(poly);
                //polyobj_item[polyobj_item.Count - 1].Tag = zgc.AccessibleName;
                zgc.GraphPane.GraphObjList.Add(poly);
                zgc.Refresh();
            }
            
        }



        public void draw_initial_bg(ZedGraphControl zgc)
        {
            double xVal_left = x_raw[Array.IndexOf(y_raw.ToArray(), y_raw.Max())] - 1;
            double xVal_right = x_raw[Array.IndexOf(y_raw.ToArray(), y_raw.Max())] + 1;

            var poly = new ZedGraph.PolyObj
            {
                Points = new[]
                {
                new ZedGraph.PointD(xVal_left, zgc.GraphPane.YAxis.Scale.Max),
                new ZedGraph.PointD(xVal_left, zgc.GraphPane.YAxis.Scale.Min),
                new ZedGraph.PointD(xVal_right, zgc.GraphPane.YAxis.Scale.Min),
                new ZedGraph.PointD(xVal_right, zgc.GraphPane.YAxis.Scale.Max),
                new ZedGraph.PointD(xVal_right, zgc.GraphPane.YAxis.Scale.Max)
                },
                Fill = new ZedGraph.Fill(Color.FromArgb(204, 255, 204)),
                ZOrder = ZedGraph.ZOrder.E_BehindCurves,
            };
            poly.Border.Color = Color.FromArgb(153, 255, 153);
            zgc.GraphPane.GraphObjList.Add(poly);
            polyobj_item.Add(poly);
            polyobj_item[polyobj_item.Count - 1].Tag = zgc.AccessibleName;
            zgc.Refresh();
        }

                

        #endregion //--------------------------------------------------------------------------------------






    }
}
