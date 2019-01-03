﻿using System;
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

            //myPane.YAxis.Scale.MajorStepAuto = false;
            //myPane.YAxis.MajorGrid.IsZeroLine = false;
            //myPane.YAxis.MajorGrid.Color = Color.FromArgb(255, 248, 245);
            //myPane.YAxis.MajorGrid.IsVisible = true;
            //myPane.XAxis.Scale.MajorStep = 100;
            

            myPane.Margin.All = 3;
            myPane.TitleGap = 2;
            myPane.Legend.IsVisible = false;
            myPane.Legend.Gap = 2;

            myPane.XAxis.Scale.MaxAuto = true;
            myPane.YAxis.Scale.MaxAuto = true;
            //myPane.YAxis.MajorGrid.Color = Color.FromArgb(255, 248, 245);
            //myPane.YAxis.MajorGrid.IsVisible = true;

            int svg_red = 100;
            int svg_green = 255;
            int svg_blue = 255;
            double dimm = 1.0;
            //int error_red = Convert.ToInt16(Math.Floor((255 - svg_red) * dimm));
            //int error_green = Convert.ToInt16(Math.Floor((255 - svg_green) * dimm));
            //int error_blue = Convert.ToInt16(Math.Floor((255 - svg_blue) * dimm));
            int error_red = 128;
            int error_green = 21;
            int error_blue = 0;
            myCurve_svg = myPane.AddCurve("", values_to_plot_svg, Color.FromArgb(svg_red, svg_green, svg_blue), SymbolType.None);
            myCurve_svg.Line.Width = 1;
            myCurve_svg.Tag = 1;
            //myCurve_svg.YAxisIndex = 1;

            myCurve_svg_deriv = myPane.AddCurve("", values_to_plot_svg_deriv, Color.FromArgb(21, 172, 61), SymbolType.None);
            myCurve_svg_deriv.Line.Width = 1;
            myCurve_svg_deriv.Tag = 2;
            //myCurve_svg_deriv.YAxisIndex = 2;

            errorCurve = myPane.AddErrorBar("Error", errorlist, Color.FromArgb(error_red, error_green, error_blue));
            errorCurve.Bar.Symbol.Type = SymbolType.Circle;
            errorCurve.Bar.Symbol.Size = 0;
            errorCurve.Tag = 3;
            //errorCurve.YAxisIndex = 3;

            myCurve = myPane.AddCurve("", values_to_plot, Color.FromArgb(210, 104, 87), SymbolType.Circle);
            myCurve.Symbol.Size = 1;
            //myCurve.Line.Color = Color.FromArgb(90, 15, 0);
            myCurve.Line.Color = Color.FromArgb(90, 15, 0);
            myCurve.Tag = 4;

            
            //myCurve.YAxisIndex = 1;
            //myPane.XAxis.Tag = 5;
            //myPane.YAxis.Tag = 6;

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
                    bool result = float.TryParse(table_binding_energies[current_line][i], out value);

                    // beginning at K1s shell (i=2), check weather there are more electron binding energies for the selected element (i>2)
                    if (result)
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