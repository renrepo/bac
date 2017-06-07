using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using ZedGraph;
using System.Globalization;

namespace PE
{
    public partial class Form1 : Form
    {


        List<List<string>> row = new List<List<string>>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        Dictionary<string, string> fab = new Dictionary<string, string>();
        string font = "Arial";
        int fontsize_activated = 12;
        int fontsize_deactivated = 11;
        int bordersize_activated = 2;
        int bordersize_deactivated = 1;
        string not_pressed = "DimGray";
        string pressed = "black";

        GraphPane myPane;


        List<string> list_gauss = new List<string>();
        public int num_gauss = 0;



        List<string> fuerlabels = new List<string>();


        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};


        TextObj b = new TextObj();
        YAxis ya = new YAxis();




        private void Form1_Load(object sender, EventArgs e)
        {

            string filePath = System.IO.Path.GetFullPath("Bindungsenergiencsv.csv");
            string filePath2 = System.IO.Path.GetFullPath("colors2.csv");
            // StreamReader sr = new StreamReader(filePath);
            row = File.ReadAllLines(filePath).Select(l => l.Split(',').ToList()).ToList();
            fab = File.ReadLines(filePath2).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);
            //MessageBox.Show(row[6][3]);
            var num = row.Count;
  
            for (int i = 0; i < num; i++)
            {
                dictionary.Add(row[i][1],row[i][0]);
            }




            //https://stackoverflow.com/questions/11239904/zedgraph-decrease-dist-between-label-and-axis-labels






           // label.FontSpec.Size = 10f;
           //label.FontSpec.FontColor = Color.DimGray;
           //label.FontSpec.Border.IsVisible = false;
           //https://stackoverflow.com/questions/32715379/add-padding-to-a-textobj-item-in-a-zedgraph-chart
           //label.FontSpec.Fill.Color = Color.Gray;
           //label.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
           //label.Location.AlignH = AlignH.Left;
           //https://stackoverflow.com/questions/11960531/positioning-an-imageobj-in-zedgraph
           //https://stackoverflow.com/questions/3808792/zedgraph-axis-labels
           //https://stackoverflow.com/questions/12248141/how-to-position-text-label-in-the-x-axis-using-zedgraph-api

            //zedGraphControl1.Refresh();

            // Setup the graph
            CreateGraph(zedGraphControl1);
            // Size the control to fill the form with a margin
            //https://www.codeproject.com/Articles/5431/A-flexible-charting-library-for-NET



        }


        private void elementnames_Popup(object sender, PopupEventArgs e)
        {

        }

        private void tableLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        public void labelchanger(object sender)
        {
            var panel = sender as Control;
            var thePanelName = panel.Name;
            //https://stackoverflow.com/questions/8000957/mouseenter-mouseleave-objectname
            int zeile = Convert.ToInt32(dictionary[thePanelName]) - 1;


            if (label51.Text == thePanelName)
            {
                label4.Text = "";
                label6.Text = "";
                label8.Text = "";
                label10.Text = "";
                label12.Text = "";
                label14.Text = "";
                label16.Text = "";
                label18.Text = "";
                label20.Text = "";
                label22.Text = "";
                label24.Text = "";
                label26.Text = "";
                label28.Text = "";
                label30.Text = "";
                label32.Text = "";
                label34.Text = "";
                label36.Text = "";
                label38.Text = "";
                label40.Text = "";
                label42.Text = "";
                label44.Text = "";
                label46.Text = "";
                label48.Text = "";
                label50.Text = "";
                label51.Text = "";
                label52.Text = "";
            }

            else
            {
                label51.Text = thePanelName;
                label52.Text = dictionary[thePanelName];
                label4.Text = row[zeile][2];
                label6.Text = row[zeile][3];
                label8.Text = row[zeile][4];
                label10.Text = row[zeile][5];
                label12.Text = row[zeile][6];
                label14.Text = row[zeile][7];
                label16.Text = row[zeile][8];
                label18.Text = row[zeile][9];
                label20.Text = row[zeile][10];
                label22.Text = row[zeile][11];
                label24.Text = row[zeile][12];
                label26.Text = row[zeile][13];
                label28.Text = row[zeile][14];
                label30.Text = row[zeile][15];
                label32.Text = row[zeile][16];
                label34.Text = row[zeile][17];
                label36.Text = row[zeile][18];
                label38.Text = row[zeile][19];
                label40.Text = row[zeile][20];
                label42.Text = row[zeile][21];
                label44.Text = row[zeile][22];
                label46.Text = row[zeile][23];
                label48.Text = row[zeile][24];
                label50.Text = row[zeile][25];
            }

        }




        //string line = File.ReadLines(path + @"\Bindungsenergien.csv").Skip(14).Take(1).First()


        public void colorchanger(object sender, TextObj phi, YAxis ya)
        {
            Button btn = (Button)sender;
            var panel = sender as Control;
            var thePanelName = panel.Name;
            string col = fab[thePanelName];


            int zeile = Convert.ToInt32(dictionary[thePanelName]) - 1;


            double value;
            int j = 0;
            int safelastj = 0;
            int k = 0;
            for (int l = 0; l <= zeile; l++)
            {
                for (int i = 2; i <= 25; i++)
                {
                    bool result = double.TryParse(row[l][i], out value);

                    if (result)
                    {
                        j += 1;
                    }
                }
                k += j;
                safelastj = j;
                j = 0;
            }

            

            if (btn.ForeColor == Color.FromName(not_pressed))
            {

                btn.Font = new Font(font, fontsize_activated, FontStyle.Bold);
                btn.ForeColor = Color.FromName(pressed);
                btn.FlatAppearance.BorderColor = Color.FromName(pressed);
                btn.FlatAppearance.BorderSize = bordersize_activated;

               // drawlines(k, safelastj, zeile, thePanelName);



                for (int i = 2; i <= 25; i++)
                {
                    bool result = double.TryParse(row[zeile][i], out value);

                    if (result)
                    {
                        //b.Text = (row[zeile][1] + " " + scores[i]);
                        //b.Location.X = float.Parse(row[zeile][i], CultureInfo.InvariantCulture);
                        //b.Location.Y = -0.02;
                        b = new TextObj((row[zeile][1] + " " + scores[i] + "\n" + row[zeile][i]), float.Parse(row[zeile][i], CultureInfo.InvariantCulture), 0.01,
                            CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        b.FontSpec.Size = 10f;
                        b.FontSpec.Fill.Color = Color.LightGray;
                        b.FontSpec.FontColor = Color.DimGray;
                        b.FontSpec.Border.IsVisible = false;
                        //b.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                        //b.Location.AlignH = AlignH.Center;
                        b.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(b);
                        fuerlabels.Add(thePanelName);



                        ya = new YAxis();                        
                        ya.Scale.IsVisible = false;
                        ya.Scale.LabelGap = 0f;
                        ya.Title.Gap = 0f;
                        ya.Title.Text = "";
                        //myPane.YAxisList.Add(y).Title.Text = thePanelName ;
                        //myPane.YAxisList.Add(y).Title.IsVisible = true;
                        ya.Color = Color.FromName(col);
                        ya.AxisGap = 0f;
                        ya.Scale.Format = "#";
                        ya.Scale.Min = 0;
                        ya.Scale.Mag = 0;
                        //ya.Title.IsVisible = false;
                        ya.MajorTic.IsAllTics = false;
                        ya.MinorTic.IsAllTics = false;                      
                        ya.Cross = Double.Parse(row[zeile][i], CultureInfo.InvariantCulture);
                        ya.IsVisible = true;
                        ya.MajorGrid.IsZeroLine = false;
                        // hides xaxis
                        myPane.YAxisList.Add(ya);

                        zedGraphControl1.Invalidate();
                    }

                }



                zedGraphControl1.Refresh();

            }

            else
            {
                btn.ForeColor = Color.FromName(not_pressed);
                btn.Font = new Font(font, fontsize_deactivated, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = bordersize_deactivated;
                btn.FlatAppearance.BorderColor = Color.FromName(not_pressed);



                int laenge = fuerlabels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (fuerlabels[y] == thePanelName)
                    {
                        fuerlabels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y+1);
                        zedGraphControl1.Invalidate();
                    }
                }


               



               // removelines(k,safelastj,zeile);

                //myPane.GraphObjList.Remove(phi);
                zedGraphControl1.Refresh();
            }
        }





        private void H_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {              
                labelchanger(sender);
            }
        }



        private void He_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {            
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }



        private void Li_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }



        private void Be_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void B_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void C_MouseDown(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void N_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void O_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void F_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void Ne_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Na_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Al_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Si_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void P_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void S_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ar_MouseDown(object sender, MouseEventArgs e)
        {
            var panel = sender as Control;
            var thePanelName = panel.Name;

            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void K_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ca_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ti_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void V_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Fe_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Co_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ni_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Zn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ga_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ge_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void As_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Se_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Br_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Kr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            };
        }

        private void Rb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Y_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Zr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Nb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tc_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ru_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Rh_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void In_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Te_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void I_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Xe_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cs_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ba_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void La_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Hf_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ta_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void W_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Re_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Os_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ir_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pt_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Au_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Hg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Bi_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Po_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void At_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Rn_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Fr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ra_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ac_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ce_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pr_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Nd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Eu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Gd_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Dy_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ho_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Er_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Yb_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Lu_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Th_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pa_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void U_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender,b ,ya);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }






        // SetSize() is separate from Resize() so we can 
        // call it independently from the Form1_Load() method
        // This leaves a 10 px margin around the outside of the control
        // Customize this to fit your needs

        

        private void CreateGraph(ZedGraphControl zgc)
        {
            // get a reference to the GraphPane
            GraphPane myPane = zgc.GraphPane;


            //myPane.Border.IsVisible = false;

            // Set the Titles
            myPane.Title.Text = "XPS spectra";
            myPane.Title.FontSpec.Size = 14;
            myPane.XAxis.Title.Text = "binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 11;
            myPane.YAxis.Title.Text = "counts";
            myPane.YAxis.Title.FontSpec.Size = 11;
            //myPane.X2Axis.Title.Text = "lines";
            myPane.Fill.Color = Color.LightGray;

            




            // Make up some data arrays based on the Sine function
            // double x, y1, y2;
            // PointPairList list1 = new PointPairList();
            // PointPairList list2 = new PointPairList();
            // for (int i = 0; i < 36; i++)
            //  {
            //    x = (double)i + 5;
            //     y1 = 1.5 + Math.Sin((double)i * 0.2);
            //     y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
            //     list1.Add(x, y1);
            //     list2.Add(x, y2);
            // }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            //  LineItem myCurve = myPane.AddCurve("Porsche",
            //      list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            //  LineItem myCurve2 = myPane.AddCurve("Piper",
            //      list2, Color.Blue, SymbolType.Circle);



            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }





        private void btn_gauss_Click(object sender, EventArgs e)
        {
            if ((!bW_gauss.IsBusy))
            {
                bW_gauss.RunWorkerAsync(); //run bW if it is not still running
                tb_gauss_startvalue.Enabled = false;
            }
        }

        private void btn_gauss_can_Click(object sender, EventArgs e)
        {
            if (bW_gauss.IsBusy) // .IsBusy is true, if bW is running, otherwise false
            {
                bW_gauss.CancelAsync(); //cancels the background operation and sets CancellationPendiung to true!
                btn_clear.Enabled = true;
            }
        }


        private void bW_gauss_DoWork(object sender, DoWorkEventArgs e)
        {
            int i;
            double end = 0;


            for (i = 0; i <= num_gauss; i++)
            {
                end += i;
                list_gauss.Add(end + "\t" + 2 * end);
                bW_gauss.ReportProgress(100 * i / num_gauss, end.ToString());
                //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
                Thread.Sleep(500);


                if (bW_gauss.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_gauss.ReportProgress(0);
                    break; //warum? ist wichtig!
                }

            }
            //safer.safe(path,list_gauss);
            e.Result = end; //stores the results of what has been done in bW
        }



        private void bW_gauss_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {   //this event is raised, when the ReportProgress-Method is called (in DoWork!)
            progressBar1.Value = e.ProgressPercentage;
            lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
            tb_gauss.Text = e.UserState as String;




            // get a reference to the GraphPane
           


            //myPane.Border.IsVisible = false;

            // Set the Titles
            //myPane.Title.Text = "XPS spectra";
            //myPane.Title.FontSpec.Size = 14;
            //myPane.XAxis.Title.Text = "binding energy [eV]";
            //myPane.XAxis.Title.FontSpec.Size = 11;
            //myPane.YAxis.Title.Text = "counts";
           // myPane.YAxis.Title.FontSpec.Size = 11;

            // Make up some data arrays based on the Sine function
            double x, y1;
            PointPairList list1 = new PointPairList();
            x = e.ProgressPercentage;
            y1 = Convert.ToDouble(e.UserState);
            list1.Add(x, y1);
            

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("",
                  list1, Color.Black, SymbolType.Circle);

            zedGraphControl1.Refresh();

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zedGraphControl1.AxisChange();



        }


        private void bW_gauss_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                tb_gauss.Text = "Cancelled!";
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_gauss.Text = e.Error.Message;
            }

            else
            {
                tb_gauss.Text = e.Result.ToString();
            }
        }

        private void btn_clear_Click(object sender, EventArgs e)
        {

                if (bW_gauss.IsBusy)
                {
                    btn_clear.Enabled = false;
                }

                else
                {
                    tb_gauss.Text = "";
                    tb_gauss_startvalue.Text = "";
                    lb_perc_gauss.Text = "";
                    tb_gauss_startvalue.BackColor = Color.Red;
                    tb_gauss_startvalue.Enabled = true;
                    progressBar1.Value = 0;
                }
        }

        private void tb_gauss_startvalue_TextChanged(object sender, EventArgs e)
        {
            string eingabe = tb_gauss_startvalue.Text;
            if (int.TryParse(eingabe, out num_gauss))
            {
                btn_gauss.Enabled = true;
                tb_gauss_startvalue.BackColor = Color.White;
                btn_gauss.Enabled = true;
            }
        }


































        public Form1()
        {
            InitializeComponent();
            myPane = zedGraphControl1.GraphPane;
        }
    }
}