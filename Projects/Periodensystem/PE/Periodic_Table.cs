using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using LabJack;
using System.Drawing;
using System.Diagnostics;
using ZedGraph;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Collections.Generic; // needed for lists




namespace XPS
{
    public partial class XPS : Form
    {

        List<List<string>> table_binding_energies = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
        List<string> display_labels = new List<string>();
        PointPairList values_to_plot = new PointPairList();
        PointPairList values_to_plot_svg = new PointPairList();
        PointPairList values_to_plot_svg_deriv = new PointPairList();
        PointPairList errorlist = new PointPairList();
        Dictionary<string, string> binding_energies_dict = new Dictionary<string, string>();
        Dictionary<string, string> color_dict = new Dictionary<string, string>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
        GraphPane myPane;
        LineItem myCurve;
        LineItem myCurve_svg;
        LineItem myCurve_svg_deriv;
        ErrorBarItem errorCurve;
        TextObj pane_labs;
        YAxis yaxis = new YAxis();

        string path_binding_energies = Path.GetFullPath("Bindungsenergien.csv");
        string path_colors = Path.GetFullPath("colors2.csv");
        string path_electronconfig = Path.GetFullPath("electronconfiguration.csv");

        System.Windows.Forms.Label[] lb_list_binding_energies;
        //System.Windows.Forms.Label[] lb_list_orbital_structure;

        // default settings
        int red = 255;
        int green = 251;
        int blue = 230;


        Dictionary<string, double[]> sav_gol_coeff = new Dictionary<string, double[]>
        {
            {"smooth_deg_4_num_points_5", new [] { 0.0, 0.0, 1.0, 0.0, 0.0} },
            {"deriv_deg_4_num_points_5", new [] { 0.08333333, -0.66666667, 0.0 , 0.66666667, -0.08333333 } },

            {"smooth_deg_4_num_points_11", new [] {0.04195804, -0.1048951 , -0.02331002,  0.13986014,  0.27972028,
                0.33333333,  0.27972028,  0.13986014, -0.02331002, -0.1048951 ,
                0.04195804} },
            {"deriv_deg_4_num_points_11", new [] {0.058275, -0.057110, -0.103341, -0.097708, -0.057498, 0.000000,
                0.057498, 0.097708, 0.103341, 0.057110, -0.058275} },

            {"smooth_deg_4_num_points_21", new [] {0.044720, -0.024845, -0.050016, -0.043151, -0.015153, 0.024529,
                0.067900, 0.108417, 0.140992, 0.161991, 0.169233, 0.161991,
                0.140992, 0.108417, 0.067900, 0.024529, -0.015153, -0.043151,
                -0.050016, -0.024845, 0.044720} },
            {"deriv_deg_4_num_points_21", new [] {0.023135, 0.002761, -0.011911, -0.021512, -0.026677, -0.028040,
                -0.026234, -0.021894, -0.015652, -0.008143, 0.000000, 0.008143,
                0.015652, 0.021894, 0.026234, 0.028040, 0.026677, 0.021512,
                0.011911, -0.002761, -0.023135} },

            {"smooth_deg_4_num_points_33", new [] {0.03685504,  0.002457  , -0.01902195, -0.0297218 , -0.03163493,
                -0.02660614, -0.01633264, -0.00236408,  0.01389751,  0.03119765,
                 0.04842946,  0.06463365,  0.07899851,  0.0908599 ,  0.09970128,
                 0.10515369,  0.10699576,  0.10515369,  0.09970128,  0.0908599 ,
                 0.07899851,  0.06463365,  0.04842946,  0.03119765,  0.01389751,
                -0.00236408, -0.01633264, -0.02660614, -0.03163493, -0.0297218 ,
                -0.01902195,  0.002457  ,  0.03685504} },
            {"deriv_deg_4_num_points_33", new [] {0.01079422,  0.00507526,  0.00033263, -0.00349878, -0.00648404,
                -0.00868824, -0.01017648, -0.01101384, -0.01126541, -0.01099627,
                -0.01027152, -0.00915624, -0.00771552, -0.00601445, -0.00411811,
                -0.0020916 ,  0.0       ,  0.0020916 ,  0.00411811,  0.00601445,
                 0.00771552,  0.00915624,  0.01027152,  0.01099627,  0.01126541,
                 0.01101384,  0.01017648,  0.00868824,  0.00648404,  0.00349878,
                -0.00033263, -0.00507526, -0.01079422} },

            {"smooth_deg_4_num_points_51", new [] {0.027848, 0.011603, -0.000865, -0.009946, -0.016012, -0.019419,
                -0.020508, -0.019600, -0.017002, -0.013005, -0.007880, -0.001886,
                 0.004738, 0.011769, 0.018999, 0.026238, 0.033312, 0.040063,
                 0.046352, 0.052053, 0.057059, 0.061279, 0.064639, 0.067080,
                 0.068562, 0.069058, 0.068562, 0.067080, 0.064639, 0.061279,
                 0.057059, 0.052053, 0.046352, 0.040063, 0.033312, 0.026238,
                 0.018999, 0.011769, 0.004738, -0.001886, -0.007880, -0.013005,
                 -0.017002, -0.019600, -0.020508, -0.019419, -0.016012, -0.009946,
                -0.000865, 0.011603, 0.027848} },
            {"deriv_deg_4_num_points_51", new [] {0.004928, 0.003292, 0.001833, 0.000543, -0.000586, -0.001561,
                -0.002389, -0.003077, -0.003634, -0.004066, -0.004380, -0.004585,
                -0.004686, -0.004693, -0.004611, -0.004449, -0.004213, -0.003911,
                -0.003551, -0.003139, -0.002683, -0.002190, -0.001668, -0.001124,
                 -0.000566, -0.000000, 0.000566, 0.001124, 0.001668, 0.002190,
                 0.002683, 0.003139, 0.003551, 0.003911, 0.004213, 0.004449,
                0.004611, 0.004693, 0.004686, 0.004585, 0.004380, 0.004066,
                 0.003634, 0.003077, 0.002389, 0.001561, 0.000586, -0.000543,
                 -0.001833, -0.003292, -0.004928} },
        };
        private void create_graph(GraphPane myPane)
        {          
            myPane.Title.Text = "UPS/XPS Spectra";
            //myPane.Title.Text = "";
            myPane.Title.FontSpec.Size = 12;
            myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "Binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 11;
            //myPane.XAxis.Scale.IsReverse = true;
            myPane.YAxis.Title.Text = "cps";
            myPane.YAxis.Title.FontSpec.Size = 11;
            //myPane.Fill.Color = Color.LightGray;
            // This will do the area outside of the graphing area
            myPane.Fill = new Fill(Color.FromArgb(45, 45, 45));
            // This will do the area inside the graphing area
            myPane.Chart.Fill = new Fill(Color.FromArgb(35, 35, 35));
            myPane.Chart.Border.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.Scale.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.XAxis.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.YAxis.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.XAxis.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.YAxis.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
            myPane.YAxis.Color = Color.FromArgb(90, 30, 0);
            myPane.XAxis.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
            myPane.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);

            //myPane.YAxis.Scale.MajorStepAuto = false;
            //myPane.YAxis.MajorGrid.IsZeroLine = false;
            //myPane.YAxis.MajorGrid.Color = Color.FromArgb(255, 248, 245);
            //myPane.YAxis.MajorGrid.IsVisible = true;
            //myPane.XAxis.Scale.MajorStep = 100;


            myPane.Margin.All = 3;
            myPane.TitleGap = 2;
            myPane.Legend.IsVisible = false;
            myPane.Legend.Gap = 2;



            //myPane.YAxis.Type = AxisType.Linear;
            myPane.YAxis.Scale.MinAuto = true;
            //myPane.YAxis.MajorGrid.Color = Color.FromArgb(255, 248, 245);
            //myPane.YAxis.MajorGrid.IsVisible = true;

            int svg_red = 100;
            int svg_green = 255;
            int svg_blue = 255;
            //int error_red = Convert.ToInt16(Math.Floor((255 - svg_red) * dimm));
            //int error_green = Convert.ToInt16(Math.Floor((255 - svg_green) * dimm));
            //int error_blue = Convert.ToInt16(Math.Floor((255 - svg_blue) * dimm));
            //int error_red = 128;
            //int error_green = 21;
            //int error_blue = 0;
            myCurve_svg = myPane.AddCurve("", values_to_plot_svg, Color.FromArgb(svg_red, svg_green, svg_blue), SymbolType.None);
            myCurve_svg.Line.Width = 1;
            myCurve_svg.Tag = 1;
            //myCurve_svg.YAxisIndex = 1;

            myCurve_svg_deriv = myPane.AddCurve("", values_to_plot_svg_deriv, Color.FromArgb(21, 172, 61), SymbolType.None);
            myCurve_svg_deriv.Line.Width = 1;
            myCurve_svg_deriv.Tag = 2;
            //myCurve_svg_deriv.YAxisIndex = 2;

            errorCurve = myPane.AddErrorBar("Error", errorlist, Color.FromArgb(90, 15, 0));
            errorCurve.Bar.Symbol.Type = SymbolType.Circle;
            errorCurve.Bar.Symbol.Size = 0;
            errorCurve.Tag = 3;
            //errorCurve.YAxisIndex = 3;

            myCurve = myPane.AddCurve("", values_to_plot, Color.FromArgb(red, green, blue), SymbolType.Circle);
            //myCurve = myPane.AddCurve("", values_to_plot, Color.FromArgb(210, 104, 87), SymbolType.Circle);
            myCurve.Symbol.Size = 1;
            //myCurve.Line.Color = Color.FromArgb(90, 15, 0);
            //myCurve.Line.Color = Color.FromArgb(90, 15, 0);
            myCurve.Line.Color = Color.FromArgb(230, 225, 215);
            myCurve.Tag = 4;


            //myCurve.YAxisIndex = 1;
            //myPane.XAxis.Tag = 5;
            //myPane.YAxis.Tag = 6;
            myPane.XAxis.Scale.MaxAuto = myPane.YAxis.Scale.MaxAuto = myPane.XAxis.Scale.MinAuto = myPane.YAxis.Scale.MinAuto = true;

            myPane.AxisChange();

        }


        // right-click on a button in the periodic table: display binding energies and electronic configuration of the selected element
        // left-click: draw energy-lines of the selected elements into the plot
        private void global_element_click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draw_energy_lines((Button)sender, yaxis);
            }
            if (e.Button == MouseButtons.Right)
            {
                electron_configuration((Button)sender);
            }
        }


        // displays elementname and atomic number if cursor rollover buttons in periodic table
        private void elementnames_Popup(object sender, PopupEventArgs e) { }

        // this method changes the apparence/color of a button in the periodic table when pressed.
        // Additionally the corresponding energy-lines and some more infomation were plottet in the graph
        public void draw_energy_lines(object sender, YAxis ya)
        {
            Button btn = (Button)sender;
            // different colors for different elements
            string col = color_dict[btn.Name];
            // current_line is equal to the atomic number of the selected element
            int current_line = Convert.ToInt32(binding_energies_dict[btn.Name]) - 1;
            float value;

            // button not left-klicked
            if (btn.ForeColor == Color.DimGray)
            {
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;

                for (int i = 2; i <= 25; i++)
                {
                    // beginning at K1s shell (i=2), check weather there are more electron binding energies for the selected element (i>2)
                    if (float.TryParse(table_binding_energies[current_line][i], out value))
                    {
                        // print out information at the top of a line in the graph (element name, electron binding energy, K/L/M-line)
                        pane_labs = new TextObj((table_binding_energies[current_line][i] + "\n" + table_binding_energies[current_line][1] + " " + scores[i]),
                            float.Parse(table_binding_energies[current_line][i], CultureInfo.InvariantCulture), -0.05, CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        pane_labs.FontSpec.Size = 10f;
                        pane_labs.FontSpec.Angle = 40;
                        pane_labs.FontSpec.Fill.Color = Color.Transparent;
                        pane_labs.FontSpec.FontColor = Color.FromArgb(red, green, blue);
                        pane_labs.FontSpec.Border.IsVisible = false;
                        pane_labs.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(pane_labs);
                        display_labels.Add(btn.Name);

                        // formatting for the energy-lines shown in the graph when an element is selected
                        ya = new YAxis();
                        ya.Scale.IsVisible = false;
                        ya.Scale.LabelGap = 0f;
                        ya.Title.Gap = 0f;
                        ya.Title.Text = string.Empty;
                        ya.Color = Color.FromName(col);
                        ya.AxisGap = 0f;
                        ya.Scale.Format = "#";
                        ya.Scale.Min = 0;
                        ya.Scale.Mag = 0;
                        ya.MajorTic.IsAllTics = false;
                        ya.MinorTic.IsAllTics = false;
                        ya.Cross = Double.Parse(table_binding_energies[current_line][i], CultureInfo.InvariantCulture);
                        ya.IsVisible = true;
                        ya.MajorGrid.IsZeroLine = false;
                        // hides xaxis
                        myPane.YAxisList.Add(ya);
                    }
                }
                zedGraphControl1.Refresh();
            }

            // button already left-klicked
            else
            {
                btn.ForeColor = Color.DimGray;
                btn.Font = new Font("Arial", 12, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.DimGray;

                // erease energy-lines and displayed information
                int laenge = display_labels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (display_labels[y] == btn.Name)
                    {
                        display_labels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y + 1);
                    }
                }
                zedGraphControl1.Refresh();
            }
        }

        
        // Function for electronic configuration plot
        public void electron_configuration(object sender)
        {
            Button btn = (Button)sender;
            // current_line is equal to the atomic number of the selected element
            int current_line = Convert.ToInt32(binding_energies_dict[btn.Name]) - 1;

            // button not right-klicked
            if (lb_element_name.Text == string.Empty)
            {
                // display electron binding energies of an element in the "Binding Energies"-box
                // startvalue i=2 because binding energy starts at column 2 in "table_binding_energies"
                for (int i = 2; i <= lb_list_binding_energies.Count(); i++)
                {
                    lb_list_binding_energies[i - 2].Text = table_binding_energies[current_line][i];
                }

                /***
                // display orbital configuration of the element
                for (int i = 1; i <= lb_list_orbital_structure.Count(); i++)
                {
                    lb_list_orbital_structure[i - 1].Text = elec_bind[current_line][i];
                }
                ***/
                lb_element_name.Text = elementnames.GetToolTip(btn);
                lb_atomic_number.Text = binding_energies_dict[btn.Name];
            }

            // button already right-klicked: clear all labels
            else
            {
                foreach (var label in lb_list_binding_energies)
                {
                    label.Text = string.Empty;
                }
                /***
                foreach (var label in lb_list_orbital_structure)
                {
                    label.Text = "--";
                }
                ***/
                lb_element_name.Text = string.Empty;
                lb_atomic_number.Text = string.Empty;
            }
        }


        public Tuple<double, double> Sav_Gol(Queue<double> input_data, double[] coefficients, double[] coefficients_dev)
        {
            double smoothed_data = 0;
            double smoothed_data_first_dev = 0;

            //if (input_data.Count() == coefficients.Length)
            //{
                int i = 0;
                foreach (var item in input_data)
                {
                    smoothed_data += coefficients[i] * item;
                    smoothed_data_first_dev += coefficients_dev[i] * item;
                    i++;
                }
            //}
            return Tuple.Create(smoothed_data, smoothed_data_first_dev);
        }

    }

}