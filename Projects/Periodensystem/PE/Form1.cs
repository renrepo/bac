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
        TextObj b1 = new TextObj();
        TextObj b2 = new TextObj();


        List<string> list_gauss = new List<string>();
        public int num_gauss = 0;



        


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
            for (int i = 0; i < 1700; i++)
            {
                myPane.YAxisList[i+1].IsVisible = false;
            }






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




  
    
        //string line = File.ReadLines(path + @"\Bindungsenergien.csv").Skip(14).Take(1).First();

        public void drawlines(int k, int safelastj, int zeile, string thePanelName)
        {
            int s = 2;
            string col = fab[thePanelName];
            for (int i = (k - safelastj); i < k; i++)
            {
                myPane.YAxisList[i + 1].Scale.IsVisible = false;
                myPane.YAxisList[i + 1].Scale.LabelGap = 0f;
                //myPane.YAxisList[i + 1].Title.Text = thePanelName ;
                //myPane.YAxisList[i + 1].Title.IsVisible = true;
                myPane.YAxisList[i + 1].Color = Color.FromName(col);
                myPane.YAxisList[i + 1].AxisGap = 0f;
                myPane.YAxisList[i + 1].Scale.Format = "#";
                myPane.YAxisList[i + 1].Scale.Mag = 0;
                myPane.YAxisList[i + 1].MajorTic.IsAllTics = false;
                myPane.YAxisList[i + 1].MinorTic.IsAllTics = false;
                myPane.YAxisList[i + 1].Cross = Double.Parse(row[zeile][s], CultureInfo.InvariantCulture);
                myPane.YAxisList[i + 1].IsVisible = true;

                //myPane.GraphObjList.Add(label);

                s += 1;
            }
            myPane.XAxis.Color = Color.Black;
            s = 2;
           // zedGraphControl1.Refresh();
        }


        public void removelines(int k, int safelastj, int zeile)
        {
            for (int i = (k - safelastj); i < k; i++)
            {
                myPane.YAxisList[i + 1].IsVisible = false;
            }
           // zedGraphControl1.Refresh();
        }


        public void colorchanger(object sender, TextObj phi)
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

                drawlines(k, safelastj, zeile, thePanelName);

                myPane.GraphObjList.Add(phi);
                zedGraphControl1.Refresh();

            }

            else
            {
                btn.ForeColor = Color.FromName(not_pressed);
                btn.Font = new Font(font, fontsize_deactivated, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = bordersize_deactivated;
                btn.FlatAppearance.BorderColor = Color.FromName(not_pressed);


                removelines(k,safelastj,zeile);
                myPane.GraphObjList.Remove(phi);
                zedGraphControl1.Refresh();
            }
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




        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};



        private void H_MouseDown(object sender, MouseEventArgs e)
        {
            var panel = sender as Control;
            var thePanelName = panel.Name;
            int zeile = Convert.ToInt32(dictionary[thePanelName]) - 1;
            double value;
            for (int i = 2; i <= 25; i++)
            {
                bool result = double.TryParse(row[zeile][i], out value);

                if (result)
                {
                    b1 = new TextObj("    " + row[zeile][1] + " " + scores[i] + "    ", float.Parse(row[zeile][i], CultureInfo.InvariantCulture), -0.02);
                    b1.FontSpec.Size = 10f;
                    b1.FontSpec.FontColor = Color.DimGray;
                    b1.FontSpec.Border.IsVisible = false;
                    b1.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                    b1.Location.AlignH = AlignH.Left;
                    b1.ZOrder = ZOrder.E_BehindCurves;
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender, b1);
            }
            if (e.Button == MouseButtons.Right)
            {              
                labelchanger(sender);
            }
        }



        private void He_MouseDown(object sender, MouseEventArgs e)
        {
            var panel = sender as Control;
            var thePanelName = panel.Name;
            int zeile = Convert.ToInt32(dictionary[thePanelName]) - 1;
            double value;
            for (int i = 2; i <= 25; i++)
            {
                bool result = double.TryParse(row[zeile][i], out value);

                if (result)
                {
                    b2 = new TextObj("    " + row[zeile][1] + " " + scores[i] + "    ", float.Parse(row[zeile][i], CultureInfo.InvariantCulture), -0.02);
                    b2.FontSpec.Size = 10f;
                    b2.FontSpec.FontColor = Color.DimGray;
                    b2.FontSpec.Border.IsVisible = false;
                    b2.Location.CoordinateFrame = CoordType.XScaleYChartFraction;
                    b2.Location.AlignH = AlignH.Left;
                    b2.ZOrder = ZOrder.E_BehindCurves;
                    //myPane.GraphObjList.Add(b2);
                    //zedGraphControl1.Refresh();
                }
            }

            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);
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
                colorchanger((Button)sender, b2);

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


            var y0 = myPane.AddYAxis("");
            var y1 = myPane.AddYAxis("");
            var y2 = myPane.AddYAxis("");
            var y3 = myPane.AddYAxis("");
            var y4 = myPane.AddYAxis("");
            var y5 = myPane.AddYAxis("");
            var y6 = myPane.AddYAxis("");
            var y7 = myPane.AddYAxis("");
            var y8 = myPane.AddYAxis("");
            var y9 = myPane.AddYAxis("");
            var y10 = myPane.AddYAxis("");
            var y11 = myPane.AddYAxis("");
            var y12 = myPane.AddYAxis("");
            var y13 = myPane.AddYAxis("");
            var y14 = myPane.AddYAxis("");
            var y15 = myPane.AddYAxis("");
            var y16 = myPane.AddYAxis("");
            var y17 = myPane.AddYAxis("");
            var y18 = myPane.AddYAxis("");
            var y19 = myPane.AddYAxis("");
            var y20 = myPane.AddYAxis("");
            var y21 = myPane.AddYAxis("");
            var y22 = myPane.AddYAxis("");
            var y23 = myPane.AddYAxis("");
            var y24 = myPane.AddYAxis("");
            var y25 = myPane.AddYAxis("");
            var y26 = myPane.AddYAxis("");
            var y27 = myPane.AddYAxis("");
            var y28 = myPane.AddYAxis("");
            var y29 = myPane.AddYAxis("");
            var y30 = myPane.AddYAxis("");
            var y31 = myPane.AddYAxis("");
            var y32 = myPane.AddYAxis("");
            var y33 = myPane.AddYAxis("");
            var y34 = myPane.AddYAxis("");
            var y35 = myPane.AddYAxis("");
            var y36 = myPane.AddYAxis("");
            var y37 = myPane.AddYAxis("");
            var y38 = myPane.AddYAxis("");
            var y39 = myPane.AddYAxis("");
            var y40 = myPane.AddYAxis("");
            var y41 = myPane.AddYAxis("");
            var y42 = myPane.AddYAxis("");
            var y43 = myPane.AddYAxis("");
            var y44 = myPane.AddYAxis("");
            var y45 = myPane.AddYAxis("");
            var y46 = myPane.AddYAxis("");
            var y47 = myPane.AddYAxis("");
            var y48 = myPane.AddYAxis("");
            var y49 = myPane.AddYAxis("");
            var y50 = myPane.AddYAxis("");
            var y51 = myPane.AddYAxis("");
            var y52 = myPane.AddYAxis("");
            var y53 = myPane.AddYAxis("");
            var y54 = myPane.AddYAxis("");
            var y55 = myPane.AddYAxis("");
            var y56 = myPane.AddYAxis("");
            var y57 = myPane.AddYAxis("");
            var y58 = myPane.AddYAxis("");
            var y59 = myPane.AddYAxis("");
            var y60 = myPane.AddYAxis("");
            var y61 = myPane.AddYAxis("");
            var y62 = myPane.AddYAxis("");
            var y63 = myPane.AddYAxis("");
            var y64 = myPane.AddYAxis("");
            var y65 = myPane.AddYAxis("");
            var y66 = myPane.AddYAxis("");
            var y67 = myPane.AddYAxis("");
            var y68 = myPane.AddYAxis("");
            var y69 = myPane.AddYAxis("");
            var y70 = myPane.AddYAxis("");
            var y71 = myPane.AddYAxis("");
            var y72 = myPane.AddYAxis("");
            var y73 = myPane.AddYAxis("");
            var y74 = myPane.AddYAxis("");
            var y75 = myPane.AddYAxis("");
            var y76 = myPane.AddYAxis("");
            var y77 = myPane.AddYAxis("");
            var y78 = myPane.AddYAxis("");
            var y79 = myPane.AddYAxis("");
            var y80 = myPane.AddYAxis("");
            var y81 = myPane.AddYAxis("");
            var y82 = myPane.AddYAxis("");
            var y83 = myPane.AddYAxis("");
            var y84 = myPane.AddYAxis("");
            var y85 = myPane.AddYAxis("");
            var y86 = myPane.AddYAxis("");
            var y87 = myPane.AddYAxis("");
            var y88 = myPane.AddYAxis("");
            var y89 = myPane.AddYAxis("");
            var y90 = myPane.AddYAxis("");
            var y91 = myPane.AddYAxis("");
            var y92 = myPane.AddYAxis("");
            var y93 = myPane.AddYAxis("");
            var y94 = myPane.AddYAxis("");
            var y95 = myPane.AddYAxis("");
            var y96 = myPane.AddYAxis("");
            var y97 = myPane.AddYAxis("");
            var y98 = myPane.AddYAxis("");
            var y99 = myPane.AddYAxis("");
            var y100 = myPane.AddYAxis("");
            var y101 = myPane.AddYAxis("");
            var y102 = myPane.AddYAxis("");
            var y103 = myPane.AddYAxis("");
            var y104 = myPane.AddYAxis("");
            var y105 = myPane.AddYAxis("");
            var y106 = myPane.AddYAxis("");
            var y107 = myPane.AddYAxis("");
            var y108 = myPane.AddYAxis("");
            var y109 = myPane.AddYAxis("");
            var y110 = myPane.AddYAxis("");
            var y111 = myPane.AddYAxis("");
            var y112 = myPane.AddYAxis("");
            var y113 = myPane.AddYAxis("");
            var y114 = myPane.AddYAxis("");
            var y115 = myPane.AddYAxis("");
            var y116 = myPane.AddYAxis("");
            var y117 = myPane.AddYAxis("");
            var y118 = myPane.AddYAxis("");
            var y119 = myPane.AddYAxis("");
            var y120 = myPane.AddYAxis("");
            var y121 = myPane.AddYAxis("");
            var y122 = myPane.AddYAxis("");
            var y123 = myPane.AddYAxis("");
            var y124 = myPane.AddYAxis("");
            var y125 = myPane.AddYAxis("");
            var y126 = myPane.AddYAxis("");
            var y127 = myPane.AddYAxis("");
            var y128 = myPane.AddYAxis("");
            var y129 = myPane.AddYAxis("");
            var y130 = myPane.AddYAxis("");
            var y131 = myPane.AddYAxis("");
            var y132 = myPane.AddYAxis("");
            var y133 = myPane.AddYAxis("");
            var y134 = myPane.AddYAxis("");
            var y135 = myPane.AddYAxis("");
            var y136 = myPane.AddYAxis("");
            var y137 = myPane.AddYAxis("");
            var y138 = myPane.AddYAxis("");
            var y139 = myPane.AddYAxis("");
            var y140 = myPane.AddYAxis("");
            var y141 = myPane.AddYAxis("");
            var y142 = myPane.AddYAxis("");
            var y143 = myPane.AddYAxis("");
            var y144 = myPane.AddYAxis("");
            var y145 = myPane.AddYAxis("");
            var y146 = myPane.AddYAxis("");
            var y147 = myPane.AddYAxis("");
            var y148 = myPane.AddYAxis("");
            var y149 = myPane.AddYAxis("");
            var y150 = myPane.AddYAxis("");
            var y151 = myPane.AddYAxis("");
            var y152 = myPane.AddYAxis("");
            var y153 = myPane.AddYAxis("");
            var y154 = myPane.AddYAxis("");
            var y155 = myPane.AddYAxis("");
            var y156 = myPane.AddYAxis("");
            var y157 = myPane.AddYAxis("");
            var y158 = myPane.AddYAxis("");
            var y159 = myPane.AddYAxis("");
            var y160 = myPane.AddYAxis("");
            var y161 = myPane.AddYAxis("");
            var y162 = myPane.AddYAxis("");
            var y163 = myPane.AddYAxis("");
            var y164 = myPane.AddYAxis("");
            var y165 = myPane.AddYAxis("");
            var y166 = myPane.AddYAxis("");
            var y167 = myPane.AddYAxis("");
            var y168 = myPane.AddYAxis("");
            var y169 = myPane.AddYAxis("");
            var y170 = myPane.AddYAxis("");
            var y171 = myPane.AddYAxis("");
            var y172 = myPane.AddYAxis("");
            var y173 = myPane.AddYAxis("");
            var y174 = myPane.AddYAxis("");
            var y175 = myPane.AddYAxis("");
            var y176 = myPane.AddYAxis("");
            var y177 = myPane.AddYAxis("");
            var y178 = myPane.AddYAxis("");
            var y179 = myPane.AddYAxis("");
            var y180 = myPane.AddYAxis("");
            var y181 = myPane.AddYAxis("");
            var y182 = myPane.AddYAxis("");
            var y183 = myPane.AddYAxis("");
            var y184 = myPane.AddYAxis("");
            var y185 = myPane.AddYAxis("");
            var y186 = myPane.AddYAxis("");
            var y187 = myPane.AddYAxis("");
            var y188 = myPane.AddYAxis("");
            var y189 = myPane.AddYAxis("");
            var y190 = myPane.AddYAxis("");
            var y191 = myPane.AddYAxis("");
            var y192 = myPane.AddYAxis("");
            var y193 = myPane.AddYAxis("");
            var y194 = myPane.AddYAxis("");
            var y195 = myPane.AddYAxis("");
            var y196 = myPane.AddYAxis("");
            var y197 = myPane.AddYAxis("");
            var y198 = myPane.AddYAxis("");
            var y199 = myPane.AddYAxis("");
            var y200 = myPane.AddYAxis("");
            var y201 = myPane.AddYAxis("");
            var y202 = myPane.AddYAxis("");
            var y203 = myPane.AddYAxis("");
            var y204 = myPane.AddYAxis("");
            var y205 = myPane.AddYAxis("");
            var y206 = myPane.AddYAxis("");
            var y207 = myPane.AddYAxis("");
            var y208 = myPane.AddYAxis("");
            var y209 = myPane.AddYAxis("");
            var y210 = myPane.AddYAxis("");
            var y211 = myPane.AddYAxis("");
            var y212 = myPane.AddYAxis("");
            var y213 = myPane.AddYAxis("");
            var y214 = myPane.AddYAxis("");
            var y215 = myPane.AddYAxis("");
            var y216 = myPane.AddYAxis("");
            var y217 = myPane.AddYAxis("");
            var y218 = myPane.AddYAxis("");
            var y219 = myPane.AddYAxis("");
            var y220 = myPane.AddYAxis("");
            var y221 = myPane.AddYAxis("");
            var y222 = myPane.AddYAxis("");
            var y223 = myPane.AddYAxis("");
            var y224 = myPane.AddYAxis("");
            var y225 = myPane.AddYAxis("");
            var y226 = myPane.AddYAxis("");
            var y227 = myPane.AddYAxis("");
            var y228 = myPane.AddYAxis("");
            var y229 = myPane.AddYAxis("");
            var y230 = myPane.AddYAxis("");
            var y231 = myPane.AddYAxis("");
            var y232 = myPane.AddYAxis("");
            var y233 = myPane.AddYAxis("");
            var y234 = myPane.AddYAxis("");
            var y235 = myPane.AddYAxis("");
            var y236 = myPane.AddYAxis("");
            var y237 = myPane.AddYAxis("");
            var y238 = myPane.AddYAxis("");
            var y239 = myPane.AddYAxis("");
            var y240 = myPane.AddYAxis("");
            var y241 = myPane.AddYAxis("");
            var y242 = myPane.AddYAxis("");
            var y243 = myPane.AddYAxis("");
            var y244 = myPane.AddYAxis("");
            var y245 = myPane.AddYAxis("");
            var y246 = myPane.AddYAxis("");
            var y247 = myPane.AddYAxis("");
            var y248 = myPane.AddYAxis("");
            var y249 = myPane.AddYAxis("");
            var y250 = myPane.AddYAxis("");
            var y251 = myPane.AddYAxis("");
            var y252 = myPane.AddYAxis("");
            var y253 = myPane.AddYAxis("");
            var y254 = myPane.AddYAxis("");
            var y255 = myPane.AddYAxis("");
            var y256 = myPane.AddYAxis("");
            var y257 = myPane.AddYAxis("");
            var y258 = myPane.AddYAxis("");
            var y259 = myPane.AddYAxis("");
            var y260 = myPane.AddYAxis("");
            var y261 = myPane.AddYAxis("");
            var y262 = myPane.AddYAxis("");
            var y263 = myPane.AddYAxis("");
            var y264 = myPane.AddYAxis("");
            var y265 = myPane.AddYAxis("");
            var y266 = myPane.AddYAxis("");
            var y267 = myPane.AddYAxis("");
            var y268 = myPane.AddYAxis("");
            var y269 = myPane.AddYAxis("");
            var y270 = myPane.AddYAxis("");
            var y271 = myPane.AddYAxis("");
            var y272 = myPane.AddYAxis("");
            var y273 = myPane.AddYAxis("");
            var y274 = myPane.AddYAxis("");
            var y275 = myPane.AddYAxis("");
            var y276 = myPane.AddYAxis("");
            var y277 = myPane.AddYAxis("");
            var y278 = myPane.AddYAxis("");
            var y279 = myPane.AddYAxis("");
            var y280 = myPane.AddYAxis("");
            var y281 = myPane.AddYAxis("");
            var y282 = myPane.AddYAxis("");
            var y283 = myPane.AddYAxis("");
            var y284 = myPane.AddYAxis("");
            var y285 = myPane.AddYAxis("");
            var y286 = myPane.AddYAxis("");
            var y287 = myPane.AddYAxis("");
            var y288 = myPane.AddYAxis("");
            var y289 = myPane.AddYAxis("");
            var y290 = myPane.AddYAxis("");
            var y291 = myPane.AddYAxis("");
            var y292 = myPane.AddYAxis("");
            var y293 = myPane.AddYAxis("");
            var y294 = myPane.AddYAxis("");
            var y295 = myPane.AddYAxis("");
            var y296 = myPane.AddYAxis("");
            var y297 = myPane.AddYAxis("");
            var y298 = myPane.AddYAxis("");
            var y299 = myPane.AddYAxis("");
            var y300 = myPane.AddYAxis("");
            var y301 = myPane.AddYAxis("");
            var y302 = myPane.AddYAxis("");
            var y303 = myPane.AddYAxis("");
            var y304 = myPane.AddYAxis("");
            var y305 = myPane.AddYAxis("");
            var y306 = myPane.AddYAxis("");
            var y307 = myPane.AddYAxis("");
            var y308 = myPane.AddYAxis("");
            var y309 = myPane.AddYAxis("");
            var y310 = myPane.AddYAxis("");
            var y311 = myPane.AddYAxis("");
            var y312 = myPane.AddYAxis("");
            var y313 = myPane.AddYAxis("");
            var y314 = myPane.AddYAxis("");
            var y315 = myPane.AddYAxis("");
            var y316 = myPane.AddYAxis("");
            var y317 = myPane.AddYAxis("");
            var y318 = myPane.AddYAxis("");
            var y319 = myPane.AddYAxis("");
            var y320 = myPane.AddYAxis("");
            var y321 = myPane.AddYAxis("");
            var y322 = myPane.AddYAxis("");
            var y323 = myPane.AddYAxis("");
            var y324 = myPane.AddYAxis("");
            var y325 = myPane.AddYAxis("");
            var y326 = myPane.AddYAxis("");
            var y327 = myPane.AddYAxis("");
            var y328 = myPane.AddYAxis("");
            var y329 = myPane.AddYAxis("");
            var y330 = myPane.AddYAxis("");
            var y331 = myPane.AddYAxis("");
            var y332 = myPane.AddYAxis("");
            var y333 = myPane.AddYAxis("");
            var y334 = myPane.AddYAxis("");
            var y335 = myPane.AddYAxis("");
            var y336 = myPane.AddYAxis("");
            var y337 = myPane.AddYAxis("");
            var y338 = myPane.AddYAxis("");
            var y339 = myPane.AddYAxis("");
            var y340 = myPane.AddYAxis("");
            var y341 = myPane.AddYAxis("");
            var y342 = myPane.AddYAxis("");
            var y343 = myPane.AddYAxis("");
            var y344 = myPane.AddYAxis("");
            var y345 = myPane.AddYAxis("");
            var y346 = myPane.AddYAxis("");
            var y347 = myPane.AddYAxis("");
            var y348 = myPane.AddYAxis("");
            var y349 = myPane.AddYAxis("");
            var y350 = myPane.AddYAxis("");
            var y351 = myPane.AddYAxis("");
            var y352 = myPane.AddYAxis("");
            var y353 = myPane.AddYAxis("");
            var y354 = myPane.AddYAxis("");
            var y355 = myPane.AddYAxis("");
            var y356 = myPane.AddYAxis("");
            var y357 = myPane.AddYAxis("");
            var y358 = myPane.AddYAxis("");
            var y359 = myPane.AddYAxis("");
            var y360 = myPane.AddYAxis("");
            var y361 = myPane.AddYAxis("");
            var y362 = myPane.AddYAxis("");
            var y363 = myPane.AddYAxis("");
            var y364 = myPane.AddYAxis("");
            var y365 = myPane.AddYAxis("");
            var y366 = myPane.AddYAxis("");
            var y367 = myPane.AddYAxis("");
            var y368 = myPane.AddYAxis("");
            var y369 = myPane.AddYAxis("");
            var y370 = myPane.AddYAxis("");
            var y371 = myPane.AddYAxis("");
            var y372 = myPane.AddYAxis("");
            var y373 = myPane.AddYAxis("");
            var y374 = myPane.AddYAxis("");
            var y375 = myPane.AddYAxis("");
            var y376 = myPane.AddYAxis("");
            var y377 = myPane.AddYAxis("");
            var y378 = myPane.AddYAxis("");
            var y379 = myPane.AddYAxis("");
            var y380 = myPane.AddYAxis("");
            var y381 = myPane.AddYAxis("");
            var y382 = myPane.AddYAxis("");
            var y383 = myPane.AddYAxis("");
            var y384 = myPane.AddYAxis("");
            var y385 = myPane.AddYAxis("");
            var y386 = myPane.AddYAxis("");
            var y387 = myPane.AddYAxis("");
            var y388 = myPane.AddYAxis("");
            var y389 = myPane.AddYAxis("");
            var y390 = myPane.AddYAxis("");
            var y391 = myPane.AddYAxis("");
            var y392 = myPane.AddYAxis("");
            var y393 = myPane.AddYAxis("");
            var y394 = myPane.AddYAxis("");
            var y395 = myPane.AddYAxis("");
            var y396 = myPane.AddYAxis("");
            var y397 = myPane.AddYAxis("");
            var y398 = myPane.AddYAxis("");
            var y399 = myPane.AddYAxis("");
            var y400 = myPane.AddYAxis("");
            var y401 = myPane.AddYAxis("");
            var y402 = myPane.AddYAxis("");
            var y403 = myPane.AddYAxis("");
            var y404 = myPane.AddYAxis("");
            var y405 = myPane.AddYAxis("");
            var y406 = myPane.AddYAxis("");
            var y407 = myPane.AddYAxis("");
            var y408 = myPane.AddYAxis("");
            var y409 = myPane.AddYAxis("");
            var y410 = myPane.AddYAxis("");
            var y411 = myPane.AddYAxis("");
            var y412 = myPane.AddYAxis("");
            var y413 = myPane.AddYAxis("");
            var y414 = myPane.AddYAxis("");
            var y415 = myPane.AddYAxis("");
            var y416 = myPane.AddYAxis("");
            var y417 = myPane.AddYAxis("");
            var y418 = myPane.AddYAxis("");
            var y419 = myPane.AddYAxis("");
            var y420 = myPane.AddYAxis("");
            var y421 = myPane.AddYAxis("");
            var y422 = myPane.AddYAxis("");
            var y423 = myPane.AddYAxis("");
            var y424 = myPane.AddYAxis("");
            var y425 = myPane.AddYAxis("");
            var y426 = myPane.AddYAxis("");
            var y427 = myPane.AddYAxis("");
            var y428 = myPane.AddYAxis("");
            var y429 = myPane.AddYAxis("");
            var y430 = myPane.AddYAxis("");
            var y431 = myPane.AddYAxis("");
            var y432 = myPane.AddYAxis("");
            var y433 = myPane.AddYAxis("");
            var y434 = myPane.AddYAxis("");
            var y435 = myPane.AddYAxis("");
            var y436 = myPane.AddYAxis("");
            var y437 = myPane.AddYAxis("");
            var y438 = myPane.AddYAxis("");
            var y439 = myPane.AddYAxis("");
            var y440 = myPane.AddYAxis("");
            var y441 = myPane.AddYAxis("");
            var y442 = myPane.AddYAxis("");
            var y443 = myPane.AddYAxis("");
            var y444 = myPane.AddYAxis("");
            var y445 = myPane.AddYAxis("");
            var y446 = myPane.AddYAxis("");
            var y447 = myPane.AddYAxis("");
            var y448 = myPane.AddYAxis("");
            var y449 = myPane.AddYAxis("");
            var y450 = myPane.AddYAxis("");
            var y451 = myPane.AddYAxis("");
            var y452 = myPane.AddYAxis("");
            var y453 = myPane.AddYAxis("");
            var y454 = myPane.AddYAxis("");
            var y455 = myPane.AddYAxis("");
            var y456 = myPane.AddYAxis("");
            var y457 = myPane.AddYAxis("");
            var y458 = myPane.AddYAxis("");
            var y459 = myPane.AddYAxis("");
            var y460 = myPane.AddYAxis("");
            var y461 = myPane.AddYAxis("");
            var y462 = myPane.AddYAxis("");
            var y463 = myPane.AddYAxis("");
            var y464 = myPane.AddYAxis("");
            var y465 = myPane.AddYAxis("");
            var y466 = myPane.AddYAxis("");
            var y467 = myPane.AddYAxis("");
            var y468 = myPane.AddYAxis("");
            var y469 = myPane.AddYAxis("");
            var y470 = myPane.AddYAxis("");
            var y471 = myPane.AddYAxis("");
            var y472 = myPane.AddYAxis("");
            var y473 = myPane.AddYAxis("");
            var y474 = myPane.AddYAxis("");
            var y475 = myPane.AddYAxis("");
            var y476 = myPane.AddYAxis("");
            var y477 = myPane.AddYAxis("");
            var y478 = myPane.AddYAxis("");
            var y479 = myPane.AddYAxis("");
            var y480 = myPane.AddYAxis("");
            var y481 = myPane.AddYAxis("");
            var y482 = myPane.AddYAxis("");
            var y483 = myPane.AddYAxis("");
            var y484 = myPane.AddYAxis("");
            var y485 = myPane.AddYAxis("");
            var y486 = myPane.AddYAxis("");
            var y487 = myPane.AddYAxis("");
            var y488 = myPane.AddYAxis("");
            var y489 = myPane.AddYAxis("");
            var y490 = myPane.AddYAxis("");
            var y491 = myPane.AddYAxis("");
            var y492 = myPane.AddYAxis("");
            var y493 = myPane.AddYAxis("");
            var y494 = myPane.AddYAxis("");
            var y495 = myPane.AddYAxis("");
            var y496 = myPane.AddYAxis("");
            var y497 = myPane.AddYAxis("");
            var y498 = myPane.AddYAxis("");
            var y499 = myPane.AddYAxis("");
            var y500 = myPane.AddYAxis("");
            var y501 = myPane.AddYAxis("");
            var y502 = myPane.AddYAxis("");
            var y503 = myPane.AddYAxis("");
            var y504 = myPane.AddYAxis("");
            var y505 = myPane.AddYAxis("");
            var y506 = myPane.AddYAxis("");
            var y507 = myPane.AddYAxis("");
            var y508 = myPane.AddYAxis("");
            var y509 = myPane.AddYAxis("");
            var y510 = myPane.AddYAxis("");
            var y511 = myPane.AddYAxis("");
            var y512 = myPane.AddYAxis("");
            var y513 = myPane.AddYAxis("");
            var y514 = myPane.AddYAxis("");
            var y515 = myPane.AddYAxis("");
            var y516 = myPane.AddYAxis("");
            var y517 = myPane.AddYAxis("");
            var y518 = myPane.AddYAxis("");
            var y519 = myPane.AddYAxis("");
            var y520 = myPane.AddYAxis("");
            var y521 = myPane.AddYAxis("");
            var y522 = myPane.AddYAxis("");
            var y523 = myPane.AddYAxis("");
            var y524 = myPane.AddYAxis("");
            var y525 = myPane.AddYAxis("");
            var y526 = myPane.AddYAxis("");
            var y527 = myPane.AddYAxis("");
            var y528 = myPane.AddYAxis("");
            var y529 = myPane.AddYAxis("");
            var y530 = myPane.AddYAxis("");
            var y531 = myPane.AddYAxis("");
            var y532 = myPane.AddYAxis("");
            var y533 = myPane.AddYAxis("");
            var y534 = myPane.AddYAxis("");
            var y535 = myPane.AddYAxis("");
            var y536 = myPane.AddYAxis("");
            var y537 = myPane.AddYAxis("");
            var y538 = myPane.AddYAxis("");
            var y539 = myPane.AddYAxis("");
            var y540 = myPane.AddYAxis("");
            var y541 = myPane.AddYAxis("");
            var y542 = myPane.AddYAxis("");
            var y543 = myPane.AddYAxis("");
            var y544 = myPane.AddYAxis("");
            var y545 = myPane.AddYAxis("");
            var y546 = myPane.AddYAxis("");
            var y547 = myPane.AddYAxis("");
            var y548 = myPane.AddYAxis("");
            var y549 = myPane.AddYAxis("");
            var y550 = myPane.AddYAxis("");
            var y551 = myPane.AddYAxis("");
            var y552 = myPane.AddYAxis("");
            var y553 = myPane.AddYAxis("");
            var y554 = myPane.AddYAxis("");
            var y555 = myPane.AddYAxis("");
            var y556 = myPane.AddYAxis("");
            var y557 = myPane.AddYAxis("");
            var y558 = myPane.AddYAxis("");
            var y559 = myPane.AddYAxis("");
            var y560 = myPane.AddYAxis("");
            var y561 = myPane.AddYAxis("");
            var y562 = myPane.AddYAxis("");
            var y563 = myPane.AddYAxis("");
            var y564 = myPane.AddYAxis("");
            var y565 = myPane.AddYAxis("");
            var y566 = myPane.AddYAxis("");
            var y567 = myPane.AddYAxis("");
            var y568 = myPane.AddYAxis("");
            var y569 = myPane.AddYAxis("");
            var y570 = myPane.AddYAxis("");
            var y571 = myPane.AddYAxis("");
            var y572 = myPane.AddYAxis("");
            var y573 = myPane.AddYAxis("");
            var y574 = myPane.AddYAxis("");
            var y575 = myPane.AddYAxis("");
            var y576 = myPane.AddYAxis("");
            var y577 = myPane.AddYAxis("");
            var y578 = myPane.AddYAxis("");
            var y579 = myPane.AddYAxis("");
            var y580 = myPane.AddYAxis("");
            var y581 = myPane.AddYAxis("");
            var y582 = myPane.AddYAxis("");
            var y583 = myPane.AddYAxis("");
            var y584 = myPane.AddYAxis("");
            var y585 = myPane.AddYAxis("");
            var y586 = myPane.AddYAxis("");
            var y587 = myPane.AddYAxis("");
            var y588 = myPane.AddYAxis("");
            var y589 = myPane.AddYAxis("");
            var y590 = myPane.AddYAxis("");
            var y591 = myPane.AddYAxis("");
            var y592 = myPane.AddYAxis("");
            var y593 = myPane.AddYAxis("");
            var y594 = myPane.AddYAxis("");
            var y595 = myPane.AddYAxis("");
            var y596 = myPane.AddYAxis("");
            var y597 = myPane.AddYAxis("");
            var y598 = myPane.AddYAxis("");
            var y599 = myPane.AddYAxis("");
            var y600 = myPane.AddYAxis("");
            var y601 = myPane.AddYAxis("");
            var y602 = myPane.AddYAxis("");
            var y603 = myPane.AddYAxis("");
            var y604 = myPane.AddYAxis("");
            var y605 = myPane.AddYAxis("");
            var y606 = myPane.AddYAxis("");
            var y607 = myPane.AddYAxis("");
            var y608 = myPane.AddYAxis("");
            var y609 = myPane.AddYAxis("");
            var y610 = myPane.AddYAxis("");
            var y611 = myPane.AddYAxis("");
            var y612 = myPane.AddYAxis("");
            var y613 = myPane.AddYAxis("");
            var y614 = myPane.AddYAxis("");
            var y615 = myPane.AddYAxis("");
            var y616 = myPane.AddYAxis("");
            var y617 = myPane.AddYAxis("");
            var y618 = myPane.AddYAxis("");
            var y619 = myPane.AddYAxis("");
            var y620 = myPane.AddYAxis("");
            var y621 = myPane.AddYAxis("");
            var y622 = myPane.AddYAxis("");
            var y623 = myPane.AddYAxis("");
            var y624 = myPane.AddYAxis("");
            var y625 = myPane.AddYAxis("");
            var y626 = myPane.AddYAxis("");
            var y627 = myPane.AddYAxis("");
            var y628 = myPane.AddYAxis("");
            var y629 = myPane.AddYAxis("");
            var y630 = myPane.AddYAxis("");
            var y631 = myPane.AddYAxis("");
            var y632 = myPane.AddYAxis("");
            var y633 = myPane.AddYAxis("");
            var y634 = myPane.AddYAxis("");
            var y635 = myPane.AddYAxis("");
            var y636 = myPane.AddYAxis("");
            var y637 = myPane.AddYAxis("");
            var y638 = myPane.AddYAxis("");
            var y639 = myPane.AddYAxis("");
            var y640 = myPane.AddYAxis("");
            var y641 = myPane.AddYAxis("");
            var y642 = myPane.AddYAxis("");
            var y643 = myPane.AddYAxis("");
            var y644 = myPane.AddYAxis("");
            var y645 = myPane.AddYAxis("");
            var y646 = myPane.AddYAxis("");
            var y647 = myPane.AddYAxis("");
            var y648 = myPane.AddYAxis("");
            var y649 = myPane.AddYAxis("");
            var y650 = myPane.AddYAxis("");
            var y651 = myPane.AddYAxis("");
            var y652 = myPane.AddYAxis("");
            var y653 = myPane.AddYAxis("");
            var y654 = myPane.AddYAxis("");
            var y655 = myPane.AddYAxis("");
            var y656 = myPane.AddYAxis("");
            var y657 = myPane.AddYAxis("");
            var y658 = myPane.AddYAxis("");
            var y659 = myPane.AddYAxis("");
            var y660 = myPane.AddYAxis("");
            var y661 = myPane.AddYAxis("");
            var y662 = myPane.AddYAxis("");
            var y663 = myPane.AddYAxis("");
            var y664 = myPane.AddYAxis("");
            var y665 = myPane.AddYAxis("");
            var y666 = myPane.AddYAxis("");
            var y667 = myPane.AddYAxis("");
            var y668 = myPane.AddYAxis("");
            var y669 = myPane.AddYAxis("");
            var y670 = myPane.AddYAxis("");
            var y671 = myPane.AddYAxis("");
            var y672 = myPane.AddYAxis("");
            var y673 = myPane.AddYAxis("");
            var y674 = myPane.AddYAxis("");
            var y675 = myPane.AddYAxis("");
            var y676 = myPane.AddYAxis("");
            var y677 = myPane.AddYAxis("");
            var y678 = myPane.AddYAxis("");
            var y679 = myPane.AddYAxis("");
            var y680 = myPane.AddYAxis("");
            var y681 = myPane.AddYAxis("");
            var y682 = myPane.AddYAxis("");
            var y683 = myPane.AddYAxis("");
            var y684 = myPane.AddYAxis("");
            var y685 = myPane.AddYAxis("");
            var y686 = myPane.AddYAxis("");
            var y687 = myPane.AddYAxis("");
            var y688 = myPane.AddYAxis("");
            var y689 = myPane.AddYAxis("");
            var y690 = myPane.AddYAxis("");
            var y691 = myPane.AddYAxis("");
            var y692 = myPane.AddYAxis("");
            var y693 = myPane.AddYAxis("");
            var y694 = myPane.AddYAxis("");
            var y695 = myPane.AddYAxis("");
            var y696 = myPane.AddYAxis("");
            var y697 = myPane.AddYAxis("");
            var y698 = myPane.AddYAxis("");
            var y699 = myPane.AddYAxis("");
            var y700 = myPane.AddYAxis("");
            var y701 = myPane.AddYAxis("");
            var y702 = myPane.AddYAxis("");
            var y703 = myPane.AddYAxis("");
            var y704 = myPane.AddYAxis("");
            var y705 = myPane.AddYAxis("");
            var y706 = myPane.AddYAxis("");
            var y707 = myPane.AddYAxis("");
            var y708 = myPane.AddYAxis("");
            var y709 = myPane.AddYAxis("");
            var y710 = myPane.AddYAxis("");
            var y711 = myPane.AddYAxis("");
            var y712 = myPane.AddYAxis("");
            var y713 = myPane.AddYAxis("");
            var y714 = myPane.AddYAxis("");
            var y715 = myPane.AddYAxis("");
            var y716 = myPane.AddYAxis("");
            var y717 = myPane.AddYAxis("");
            var y718 = myPane.AddYAxis("");
            var y719 = myPane.AddYAxis("");
            var y720 = myPane.AddYAxis("");
            var y721 = myPane.AddYAxis("");
            var y722 = myPane.AddYAxis("");
            var y723 = myPane.AddYAxis("");
            var y724 = myPane.AddYAxis("");
            var y725 = myPane.AddYAxis("");
            var y726 = myPane.AddYAxis("");
            var y727 = myPane.AddYAxis("");
            var y728 = myPane.AddYAxis("");
            var y729 = myPane.AddYAxis("");
            var y730 = myPane.AddYAxis("");
            var y731 = myPane.AddYAxis("");
            var y732 = myPane.AddYAxis("");
            var y733 = myPane.AddYAxis("");
            var y734 = myPane.AddYAxis("");
            var y735 = myPane.AddYAxis("");
            var y736 = myPane.AddYAxis("");
            var y737 = myPane.AddYAxis("");
            var y738 = myPane.AddYAxis("");
            var y739 = myPane.AddYAxis("");
            var y740 = myPane.AddYAxis("");
            var y741 = myPane.AddYAxis("");
            var y742 = myPane.AddYAxis("");
            var y743 = myPane.AddYAxis("");
            var y744 = myPane.AddYAxis("");
            var y745 = myPane.AddYAxis("");
            var y746 = myPane.AddYAxis("");
            var y747 = myPane.AddYAxis("");
            var y748 = myPane.AddYAxis("");
            var y749 = myPane.AddYAxis("");
            var y750 = myPane.AddYAxis("");
            var y751 = myPane.AddYAxis("");
            var y752 = myPane.AddYAxis("");
            var y753 = myPane.AddYAxis("");
            var y754 = myPane.AddYAxis("");
            var y755 = myPane.AddYAxis("");
            var y756 = myPane.AddYAxis("");
            var y757 = myPane.AddYAxis("");
            var y758 = myPane.AddYAxis("");
            var y759 = myPane.AddYAxis("");
            var y760 = myPane.AddYAxis("");
            var y761 = myPane.AddYAxis("");
            var y762 = myPane.AddYAxis("");
            var y763 = myPane.AddYAxis("");
            var y764 = myPane.AddYAxis("");
            var y765 = myPane.AddYAxis("");
            var y766 = myPane.AddYAxis("");
            var y767 = myPane.AddYAxis("");
            var y768 = myPane.AddYAxis("");
            var y769 = myPane.AddYAxis("");
            var y770 = myPane.AddYAxis("");
            var y771 = myPane.AddYAxis("");
            var y772 = myPane.AddYAxis("");
            var y773 = myPane.AddYAxis("");
            var y774 = myPane.AddYAxis("");
            var y775 = myPane.AddYAxis("");
            var y776 = myPane.AddYAxis("");
            var y777 = myPane.AddYAxis("");
            var y778 = myPane.AddYAxis("");
            var y779 = myPane.AddYAxis("");
            var y780 = myPane.AddYAxis("");
            var y781 = myPane.AddYAxis("");
            var y782 = myPane.AddYAxis("");
            var y783 = myPane.AddYAxis("");
            var y784 = myPane.AddYAxis("");
            var y785 = myPane.AddYAxis("");
            var y786 = myPane.AddYAxis("");
            var y787 = myPane.AddYAxis("");
            var y788 = myPane.AddYAxis("");
            var y789 = myPane.AddYAxis("");
            var y790 = myPane.AddYAxis("");
            var y791 = myPane.AddYAxis("");
            var y792 = myPane.AddYAxis("");
            var y793 = myPane.AddYAxis("");
            var y794 = myPane.AddYAxis("");
            var y795 = myPane.AddYAxis("");
            var y796 = myPane.AddYAxis("");
            var y797 = myPane.AddYAxis("");
            var y798 = myPane.AddYAxis("");
            var y799 = myPane.AddYAxis("");
            var y800 = myPane.AddYAxis("");
            var y801 = myPane.AddYAxis("");
            var y802 = myPane.AddYAxis("");
            var y803 = myPane.AddYAxis("");
            var y804 = myPane.AddYAxis("");
            var y805 = myPane.AddYAxis("");
            var y806 = myPane.AddYAxis("");
            var y807 = myPane.AddYAxis("");
            var y808 = myPane.AddYAxis("");
            var y809 = myPane.AddYAxis("");
            var y810 = myPane.AddYAxis("");
            var y811 = myPane.AddYAxis("");
            var y812 = myPane.AddYAxis("");
            var y813 = myPane.AddYAxis("");
            var y814 = myPane.AddYAxis("");
            var y815 = myPane.AddYAxis("");
            var y816 = myPane.AddYAxis("");
            var y817 = myPane.AddYAxis("");
            var y818 = myPane.AddYAxis("");
            var y819 = myPane.AddYAxis("");
            var y820 = myPane.AddYAxis("");
            var y821 = myPane.AddYAxis("");
            var y822 = myPane.AddYAxis("");
            var y823 = myPane.AddYAxis("");
            var y824 = myPane.AddYAxis("");
            var y825 = myPane.AddYAxis("");
            var y826 = myPane.AddYAxis("");
            var y827 = myPane.AddYAxis("");
            var y828 = myPane.AddYAxis("");
            var y829 = myPane.AddYAxis("");
            var y830 = myPane.AddYAxis("");
            var y831 = myPane.AddYAxis("");
            var y832 = myPane.AddYAxis("");
            var y833 = myPane.AddYAxis("");
            var y834 = myPane.AddYAxis("");
            var y835 = myPane.AddYAxis("");
            var y836 = myPane.AddYAxis("");
            var y837 = myPane.AddYAxis("");
            var y838 = myPane.AddYAxis("");
            var y839 = myPane.AddYAxis("");
            var y840 = myPane.AddYAxis("");
            var y841 = myPane.AddYAxis("");
            var y842 = myPane.AddYAxis("");
            var y843 = myPane.AddYAxis("");
            var y844 = myPane.AddYAxis("");
            var y845 = myPane.AddYAxis("");
            var y846 = myPane.AddYAxis("");
            var y847 = myPane.AddYAxis("");
            var y848 = myPane.AddYAxis("");
            var y849 = myPane.AddYAxis("");
            var y850 = myPane.AddYAxis("");
            var y851 = myPane.AddYAxis("");
            var y852 = myPane.AddYAxis("");
            var y853 = myPane.AddYAxis("");
            var y854 = myPane.AddYAxis("");
            var y855 = myPane.AddYAxis("");
            var y856 = myPane.AddYAxis("");
            var y857 = myPane.AddYAxis("");
            var y858 = myPane.AddYAxis("");
            var y859 = myPane.AddYAxis("");
            var y860 = myPane.AddYAxis("");
            var y861 = myPane.AddYAxis("");
            var y862 = myPane.AddYAxis("");
            var y863 = myPane.AddYAxis("");
            var y864 = myPane.AddYAxis("");
            var y865 = myPane.AddYAxis("");
            var y866 = myPane.AddYAxis("");
            var y867 = myPane.AddYAxis("");
            var y868 = myPane.AddYAxis("");
            var y869 = myPane.AddYAxis("");
            var y870 = myPane.AddYAxis("");
            var y871 = myPane.AddYAxis("");
            var y872 = myPane.AddYAxis("");
            var y873 = myPane.AddYAxis("");
            var y874 = myPane.AddYAxis("");
            var y875 = myPane.AddYAxis("");
            var y876 = myPane.AddYAxis("");
            var y877 = myPane.AddYAxis("");
            var y878 = myPane.AddYAxis("");
            var y879 = myPane.AddYAxis("");
            var y880 = myPane.AddYAxis("");
            var y881 = myPane.AddYAxis("");
            var y882 = myPane.AddYAxis("");
            var y883 = myPane.AddYAxis("");
            var y884 = myPane.AddYAxis("");
            var y885 = myPane.AddYAxis("");
            var y886 = myPane.AddYAxis("");
            var y887 = myPane.AddYAxis("");
            var y888 = myPane.AddYAxis("");
            var y889 = myPane.AddYAxis("");
            var y890 = myPane.AddYAxis("");
            var y891 = myPane.AddYAxis("");
            var y892 = myPane.AddYAxis("");
            var y893 = myPane.AddYAxis("");
            var y894 = myPane.AddYAxis("");
            var y895 = myPane.AddYAxis("");
            var y896 = myPane.AddYAxis("");
            var y897 = myPane.AddYAxis("");
            var y898 = myPane.AddYAxis("");
            var y899 = myPane.AddYAxis("");
            var y900 = myPane.AddYAxis("");
            var y901 = myPane.AddYAxis("");
            var y902 = myPane.AddYAxis("");
            var y903 = myPane.AddYAxis("");
            var y904 = myPane.AddYAxis("");
            var y905 = myPane.AddYAxis("");
            var y906 = myPane.AddYAxis("");
            var y907 = myPane.AddYAxis("");
            var y908 = myPane.AddYAxis("");
            var y909 = myPane.AddYAxis("");
            var y910 = myPane.AddYAxis("");
            var y911 = myPane.AddYAxis("");
            var y912 = myPane.AddYAxis("");
            var y913 = myPane.AddYAxis("");
            var y914 = myPane.AddYAxis("");
            var y915 = myPane.AddYAxis("");
            var y916 = myPane.AddYAxis("");
            var y917 = myPane.AddYAxis("");
            var y918 = myPane.AddYAxis("");
            var y919 = myPane.AddYAxis("");
            var y920 = myPane.AddYAxis("");
            var y921 = myPane.AddYAxis("");
            var y922 = myPane.AddYAxis("");
            var y923 = myPane.AddYAxis("");
            var y924 = myPane.AddYAxis("");
            var y925 = myPane.AddYAxis("");
            var y926 = myPane.AddYAxis("");
            var y927 = myPane.AddYAxis("");
            var y928 = myPane.AddYAxis("");
            var y929 = myPane.AddYAxis("");
            var y930 = myPane.AddYAxis("");
            var y931 = myPane.AddYAxis("");
            var y932 = myPane.AddYAxis("");
            var y933 = myPane.AddYAxis("");
            var y934 = myPane.AddYAxis("");
            var y935 = myPane.AddYAxis("");
            var y936 = myPane.AddYAxis("");
            var y937 = myPane.AddYAxis("");
            var y938 = myPane.AddYAxis("");
            var y939 = myPane.AddYAxis("");
            var y940 = myPane.AddYAxis("");
            var y941 = myPane.AddYAxis("");
            var y942 = myPane.AddYAxis("");
            var y943 = myPane.AddYAxis("");
            var y944 = myPane.AddYAxis("");
            var y945 = myPane.AddYAxis("");
            var y946 = myPane.AddYAxis("");
            var y947 = myPane.AddYAxis("");
            var y948 = myPane.AddYAxis("");
            var y949 = myPane.AddYAxis("");
            var y950 = myPane.AddYAxis("");
            var y951 = myPane.AddYAxis("");
            var y952 = myPane.AddYAxis("");
            var y953 = myPane.AddYAxis("");
            var y954 = myPane.AddYAxis("");
            var y955 = myPane.AddYAxis("");
            var y956 = myPane.AddYAxis("");
            var y957 = myPane.AddYAxis("");
            var y958 = myPane.AddYAxis("");
            var y959 = myPane.AddYAxis("");
            var y960 = myPane.AddYAxis("");
            var y961 = myPane.AddYAxis("");
            var y962 = myPane.AddYAxis("");
            var y963 = myPane.AddYAxis("");
            var y964 = myPane.AddYAxis("");
            var y965 = myPane.AddYAxis("");
            var y966 = myPane.AddYAxis("");
            var y967 = myPane.AddYAxis("");
            var y968 = myPane.AddYAxis("");
            var y969 = myPane.AddYAxis("");
            var y970 = myPane.AddYAxis("");
            var y971 = myPane.AddYAxis("");
            var y972 = myPane.AddYAxis("");
            var y973 = myPane.AddYAxis("");
            var y974 = myPane.AddYAxis("");
            var y975 = myPane.AddYAxis("");
            var y976 = myPane.AddYAxis("");
            var y977 = myPane.AddYAxis("");
            var y978 = myPane.AddYAxis("");
            var y979 = myPane.AddYAxis("");
            var y980 = myPane.AddYAxis("");
            var y981 = myPane.AddYAxis("");
            var y982 = myPane.AddYAxis("");
            var y983 = myPane.AddYAxis("");
            var y984 = myPane.AddYAxis("");
            var y985 = myPane.AddYAxis("");
            var y986 = myPane.AddYAxis("");
            var y987 = myPane.AddYAxis("");
            var y988 = myPane.AddYAxis("");
            var y989 = myPane.AddYAxis("");
            var y990 = myPane.AddYAxis("");
            var y991 = myPane.AddYAxis("");
            var y992 = myPane.AddYAxis("");
            var y993 = myPane.AddYAxis("");
            var y994 = myPane.AddYAxis("");
            var y995 = myPane.AddYAxis("");
            var y996 = myPane.AddYAxis("");
            var y997 = myPane.AddYAxis("");
            var y998 = myPane.AddYAxis("");
            var y999 = myPane.AddYAxis("");
            var y1000 = myPane.AddYAxis("");
            var y1001 = myPane.AddYAxis("");
            var y1002 = myPane.AddYAxis("");
            var y1003 = myPane.AddYAxis("");
            var y1004 = myPane.AddYAxis("");
            var y1005 = myPane.AddYAxis("");
            var y1006 = myPane.AddYAxis("");
            var y1007 = myPane.AddYAxis("");
            var y1008 = myPane.AddYAxis("");
            var y1009 = myPane.AddYAxis("");
            var y1010 = myPane.AddYAxis("");
            var y1011 = myPane.AddYAxis("");
            var y1012 = myPane.AddYAxis("");
            var y1013 = myPane.AddYAxis("");
            var y1014 = myPane.AddYAxis("");
            var y1015 = myPane.AddYAxis("");
            var y1016 = myPane.AddYAxis("");
            var y1017 = myPane.AddYAxis("");
            var y1018 = myPane.AddYAxis("");
            var y1019 = myPane.AddYAxis("");
            var y1020 = myPane.AddYAxis("");
            var y1021 = myPane.AddYAxis("");
            var y1022 = myPane.AddYAxis("");
            var y1023 = myPane.AddYAxis("");
            var y1024 = myPane.AddYAxis("");
            var y1025 = myPane.AddYAxis("");
            var y1026 = myPane.AddYAxis("");
            var y1027 = myPane.AddYAxis("");
            var y1028 = myPane.AddYAxis("");
            var y1029 = myPane.AddYAxis("");
            var y1030 = myPane.AddYAxis("");
            var y1031 = myPane.AddYAxis("");
            var y1032 = myPane.AddYAxis("");
            var y1033 = myPane.AddYAxis("");
            var y1034 = myPane.AddYAxis("");
            var y1035 = myPane.AddYAxis("");
            var y1036 = myPane.AddYAxis("");
            var y1037 = myPane.AddYAxis("");
            var y1038 = myPane.AddYAxis("");
            var y1039 = myPane.AddYAxis("");
            var y1040 = myPane.AddYAxis("");
            var y1041 = myPane.AddYAxis("");
            var y1042 = myPane.AddYAxis("");
            var y1043 = myPane.AddYAxis("");
            var y1044 = myPane.AddYAxis("");
            var y1045 = myPane.AddYAxis("");
            var y1046 = myPane.AddYAxis("");
            var y1047 = myPane.AddYAxis("");
            var y1048 = myPane.AddYAxis("");
            var y1049 = myPane.AddYAxis("");
            var y1050 = myPane.AddYAxis("");
            var y1051 = myPane.AddYAxis("");
            var y1052 = myPane.AddYAxis("");
            var y1053 = myPane.AddYAxis("");
            var y1054 = myPane.AddYAxis("");
            var y1055 = myPane.AddYAxis("");
            var y1056 = myPane.AddYAxis("");
            var y1057 = myPane.AddYAxis("");
            var y1058 = myPane.AddYAxis("");
            var y1059 = myPane.AddYAxis("");
            var y1060 = myPane.AddYAxis("");
            var y1061 = myPane.AddYAxis("");
            var y1062 = myPane.AddYAxis("");
            var y1063 = myPane.AddYAxis("");
            var y1064 = myPane.AddYAxis("");
            var y1065 = myPane.AddYAxis("");
            var y1066 = myPane.AddYAxis("");
            var y1067 = myPane.AddYAxis("");
            var y1068 = myPane.AddYAxis("");
            var y1069 = myPane.AddYAxis("");
            var y1070 = myPane.AddYAxis("");
            var y1071 = myPane.AddYAxis("");
            var y1072 = myPane.AddYAxis("");
            var y1073 = myPane.AddYAxis("");
            var y1074 = myPane.AddYAxis("");
            var y1075 = myPane.AddYAxis("");
            var y1076 = myPane.AddYAxis("");
            var y1077 = myPane.AddYAxis("");
            var y1078 = myPane.AddYAxis("");
            var y1079 = myPane.AddYAxis("");
            var y1080 = myPane.AddYAxis("");
            var y1081 = myPane.AddYAxis("");
            var y1082 = myPane.AddYAxis("");
            var y1083 = myPane.AddYAxis("");
            var y1084 = myPane.AddYAxis("");
            var y1085 = myPane.AddYAxis("");
            var y1086 = myPane.AddYAxis("");
            var y1087 = myPane.AddYAxis("");
            var y1088 = myPane.AddYAxis("");
            var y1089 = myPane.AddYAxis("");
            var y1090 = myPane.AddYAxis("");
            var y1091 = myPane.AddYAxis("");
            var y1092 = myPane.AddYAxis("");
            var y1093 = myPane.AddYAxis("");
            var y1094 = myPane.AddYAxis("");
            var y1095 = myPane.AddYAxis("");
            var y1096 = myPane.AddYAxis("");
            var y1097 = myPane.AddYAxis("");
            var y1098 = myPane.AddYAxis("");
            var y1099 = myPane.AddYAxis("");
            var y1100 = myPane.AddYAxis("");
            var y1101 = myPane.AddYAxis("");
            var y1102 = myPane.AddYAxis("");
            var y1103 = myPane.AddYAxis("");
            var y1104 = myPane.AddYAxis("");
            var y1105 = myPane.AddYAxis("");
            var y1106 = myPane.AddYAxis("");
            var y1107 = myPane.AddYAxis("");
            var y1108 = myPane.AddYAxis("");
            var y1109 = myPane.AddYAxis("");
            var y1110 = myPane.AddYAxis("");
            var y1111 = myPane.AddYAxis("");
            var y1112 = myPane.AddYAxis("");
            var y1113 = myPane.AddYAxis("");
            var y1114 = myPane.AddYAxis("");
            var y1115 = myPane.AddYAxis("");
            var y1116 = myPane.AddYAxis("");
            var y1117 = myPane.AddYAxis("");
            var y1118 = myPane.AddYAxis("");
            var y1119 = myPane.AddYAxis("");
            var y1120 = myPane.AddYAxis("");
            var y1121 = myPane.AddYAxis("");
            var y1122 = myPane.AddYAxis("");
            var y1123 = myPane.AddYAxis("");
            var y1124 = myPane.AddYAxis("");
            var y1125 = myPane.AddYAxis("");
            var y1126 = myPane.AddYAxis("");
            var y1127 = myPane.AddYAxis("");
            var y1128 = myPane.AddYAxis("");
            var y1129 = myPane.AddYAxis("");
            var y1130 = myPane.AddYAxis("");
            var y1131 = myPane.AddYAxis("");
            var y1132 = myPane.AddYAxis("");
            var y1133 = myPane.AddYAxis("");
            var y1134 = myPane.AddYAxis("");
            var y1135 = myPane.AddYAxis("");
            var y1136 = myPane.AddYAxis("");
            var y1137 = myPane.AddYAxis("");
            var y1138 = myPane.AddYAxis("");
            var y1139 = myPane.AddYAxis("");
            var y1140 = myPane.AddYAxis("");
            var y1141 = myPane.AddYAxis("");
            var y1142 = myPane.AddYAxis("");
            var y1143 = myPane.AddYAxis("");
            var y1144 = myPane.AddYAxis("");
            var y1145 = myPane.AddYAxis("");
            var y1146 = myPane.AddYAxis("");
            var y1147 = myPane.AddYAxis("");
            var y1148 = myPane.AddYAxis("");
            var y1149 = myPane.AddYAxis("");
            var y1150 = myPane.AddYAxis("");
            var y1151 = myPane.AddYAxis("");
            var y1152 = myPane.AddYAxis("");
            var y1153 = myPane.AddYAxis("");
            var y1154 = myPane.AddYAxis("");
            var y1155 = myPane.AddYAxis("");
            var y1156 = myPane.AddYAxis("");
            var y1157 = myPane.AddYAxis("");
            var y1158 = myPane.AddYAxis("");
            var y1159 = myPane.AddYAxis("");
            var y1160 = myPane.AddYAxis("");
            var y1161 = myPane.AddYAxis("");
            var y1162 = myPane.AddYAxis("");
            var y1163 = myPane.AddYAxis("");
            var y1164 = myPane.AddYAxis("");
            var y1165 = myPane.AddYAxis("");
            var y1166 = myPane.AddYAxis("");
            var y1167 = myPane.AddYAxis("");
            var y1168 = myPane.AddYAxis("");
            var y1169 = myPane.AddYAxis("");
            var y1170 = myPane.AddYAxis("");
            var y1171 = myPane.AddYAxis("");
            var y1172 = myPane.AddYAxis("");
            var y1173 = myPane.AddYAxis("");
            var y1174 = myPane.AddYAxis("");
            var y1175 = myPane.AddYAxis("");
            var y1176 = myPane.AddYAxis("");
            var y1177 = myPane.AddYAxis("");
            var y1178 = myPane.AddYAxis("");
            var y1179 = myPane.AddYAxis("");
            var y1180 = myPane.AddYAxis("");
            var y1181 = myPane.AddYAxis("");
            var y1182 = myPane.AddYAxis("");
            var y1183 = myPane.AddYAxis("");
            var y1184 = myPane.AddYAxis("");
            var y1185 = myPane.AddYAxis("");
            var y1186 = myPane.AddYAxis("");
            var y1187 = myPane.AddYAxis("");
            var y1188 = myPane.AddYAxis("");
            var y1189 = myPane.AddYAxis("");
            var y1190 = myPane.AddYAxis("");
            var y1191 = myPane.AddYAxis("");
            var y1192 = myPane.AddYAxis("");
            var y1193 = myPane.AddYAxis("");
            var y1194 = myPane.AddYAxis("");
            var y1195 = myPane.AddYAxis("");
            var y1196 = myPane.AddYAxis("");
            var y1197 = myPane.AddYAxis("");
            var y1198 = myPane.AddYAxis("");
            var y1199 = myPane.AddYAxis("");
            var y1200 = myPane.AddYAxis("");
            var y1201 = myPane.AddYAxis("");
            var y1202 = myPane.AddYAxis("");
            var y1203 = myPane.AddYAxis("");
            var y1204 = myPane.AddYAxis("");
            var y1205 = myPane.AddYAxis("");
            var y1206 = myPane.AddYAxis("");
            var y1207 = myPane.AddYAxis("");
            var y1208 = myPane.AddYAxis("");
            var y1209 = myPane.AddYAxis("");
            var y1210 = myPane.AddYAxis("");
            var y1211 = myPane.AddYAxis("");
            var y1212 = myPane.AddYAxis("");
            var y1213 = myPane.AddYAxis("");
            var y1214 = myPane.AddYAxis("");
            var y1215 = myPane.AddYAxis("");
            var y1216 = myPane.AddYAxis("");
            var y1217 = myPane.AddYAxis("");
            var y1218 = myPane.AddYAxis("");
            var y1219 = myPane.AddYAxis("");
            var y1220 = myPane.AddYAxis("");
            var y1221 = myPane.AddYAxis("");
            var y1222 = myPane.AddYAxis("");
            var y1223 = myPane.AddYAxis("");
            var y1224 = myPane.AddYAxis("");
            var y1225 = myPane.AddYAxis("");
            var y1226 = myPane.AddYAxis("");
            var y1227 = myPane.AddYAxis("");
            var y1228 = myPane.AddYAxis("");
            var y1229 = myPane.AddYAxis("");
            var y1230 = myPane.AddYAxis("");
            var y1231 = myPane.AddYAxis("");
            var y1232 = myPane.AddYAxis("");
            var y1233 = myPane.AddYAxis("");
            var y1234 = myPane.AddYAxis("");
            var y1235 = myPane.AddYAxis("");
            var y1236 = myPane.AddYAxis("");
            var y1237 = myPane.AddYAxis("");
            var y1238 = myPane.AddYAxis("");
            var y1239 = myPane.AddYAxis("");
            var y1240 = myPane.AddYAxis("");
            var y1241 = myPane.AddYAxis("");
            var y1242 = myPane.AddYAxis("");
            var y1243 = myPane.AddYAxis("");
            var y1244 = myPane.AddYAxis("");
            var y1245 = myPane.AddYAxis("");
            var y1246 = myPane.AddYAxis("");
            var y1247 = myPane.AddYAxis("");
            var y1248 = myPane.AddYAxis("");
            var y1249 = myPane.AddYAxis("");
            var y1250 = myPane.AddYAxis("");
            var y1251 = myPane.AddYAxis("");
            var y1252 = myPane.AddYAxis("");
            var y1253 = myPane.AddYAxis("");
            var y1254 = myPane.AddYAxis("");
            var y1255 = myPane.AddYAxis("");
            var y1256 = myPane.AddYAxis("");
            var y1257 = myPane.AddYAxis("");
            var y1258 = myPane.AddYAxis("");
            var y1259 = myPane.AddYAxis("");
            var y1260 = myPane.AddYAxis("");
            var y1261 = myPane.AddYAxis("");
            var y1262 = myPane.AddYAxis("");
            var y1263 = myPane.AddYAxis("");
            var y1264 = myPane.AddYAxis("");
            var y1265 = myPane.AddYAxis("");
            var y1266 = myPane.AddYAxis("");
            var y1267 = myPane.AddYAxis("");
            var y1268 = myPane.AddYAxis("");
            var y1269 = myPane.AddYAxis("");
            var y1270 = myPane.AddYAxis("");
            var y1271 = myPane.AddYAxis("");
            var y1272 = myPane.AddYAxis("");
            var y1273 = myPane.AddYAxis("");
            var y1274 = myPane.AddYAxis("");
            var y1275 = myPane.AddYAxis("");
            var y1276 = myPane.AddYAxis("");
            var y1277 = myPane.AddYAxis("");
            var y1278 = myPane.AddYAxis("");
            var y1279 = myPane.AddYAxis("");
            var y1280 = myPane.AddYAxis("");
            var y1281 = myPane.AddYAxis("");
            var y1282 = myPane.AddYAxis("");
            var y1283 = myPane.AddYAxis("");
            var y1284 = myPane.AddYAxis("");
            var y1285 = myPane.AddYAxis("");
            var y1286 = myPane.AddYAxis("");
            var y1287 = myPane.AddYAxis("");
            var y1288 = myPane.AddYAxis("");
            var y1289 = myPane.AddYAxis("");
            var y1290 = myPane.AddYAxis("");
            var y1291 = myPane.AddYAxis("");
            var y1292 = myPane.AddYAxis("");
            var y1293 = myPane.AddYAxis("");
            var y1294 = myPane.AddYAxis("");
            var y1295 = myPane.AddYAxis("");
            var y1296 = myPane.AddYAxis("");
            var y1297 = myPane.AddYAxis("");
            var y1298 = myPane.AddYAxis("");
            var y1299 = myPane.AddYAxis("");
            var y1300 = myPane.AddYAxis("");
            var y1301 = myPane.AddYAxis("");
            var y1302 = myPane.AddYAxis("");
            var y1303 = myPane.AddYAxis("");
            var y1304 = myPane.AddYAxis("");
            var y1305 = myPane.AddYAxis("");
            var y1306 = myPane.AddYAxis("");
            var y1307 = myPane.AddYAxis("");
            var y1308 = myPane.AddYAxis("");
            var y1309 = myPane.AddYAxis("");
            var y1310 = myPane.AddYAxis("");
            var y1311 = myPane.AddYAxis("");
            var y1312 = myPane.AddYAxis("");
            var y1313 = myPane.AddYAxis("");
            var y1314 = myPane.AddYAxis("");
            var y1315 = myPane.AddYAxis("");
            var y1316 = myPane.AddYAxis("");
            var y1317 = myPane.AddYAxis("");
            var y1318 = myPane.AddYAxis("");
            var y1319 = myPane.AddYAxis("");
            var y1320 = myPane.AddYAxis("");
            var y1321 = myPane.AddYAxis("");
            var y1322 = myPane.AddYAxis("");
            var y1323 = myPane.AddYAxis("");
            var y1324 = myPane.AddYAxis("");
            var y1325 = myPane.AddYAxis("");
            var y1326 = myPane.AddYAxis("");
            var y1327 = myPane.AddYAxis("");
            var y1328 = myPane.AddYAxis("");
            var y1329 = myPane.AddYAxis("");
            var y1330 = myPane.AddYAxis("");
            var y1331 = myPane.AddYAxis("");
            var y1332 = myPane.AddYAxis("");
            var y1333 = myPane.AddYAxis("");
            var y1334 = myPane.AddYAxis("");
            var y1335 = myPane.AddYAxis("");
            var y1336 = myPane.AddYAxis("");
            var y1337 = myPane.AddYAxis("");
            var y1338 = myPane.AddYAxis("");
            var y1339 = myPane.AddYAxis("");
            var y1340 = myPane.AddYAxis("");
            var y1341 = myPane.AddYAxis("");
            var y1342 = myPane.AddYAxis("");
            var y1343 = myPane.AddYAxis("");
            var y1344 = myPane.AddYAxis("");
            var y1345 = myPane.AddYAxis("");
            var y1346 = myPane.AddYAxis("");
            var y1347 = myPane.AddYAxis("");
            var y1348 = myPane.AddYAxis("");
            var y1349 = myPane.AddYAxis("");
            var y1350 = myPane.AddYAxis("");
            var y1351 = myPane.AddYAxis("");
            var y1352 = myPane.AddYAxis("");
            var y1353 = myPane.AddYAxis("");
            var y1354 = myPane.AddYAxis("");
            var y1355 = myPane.AddYAxis("");
            var y1356 = myPane.AddYAxis("");
            var y1357 = myPane.AddYAxis("");
            var y1358 = myPane.AddYAxis("");
            var y1359 = myPane.AddYAxis("");
            var y1360 = myPane.AddYAxis("");
            var y1361 = myPane.AddYAxis("");
            var y1362 = myPane.AddYAxis("");
            var y1363 = myPane.AddYAxis("");
            var y1364 = myPane.AddYAxis("");
            var y1365 = myPane.AddYAxis("");
            var y1366 = myPane.AddYAxis("");
            var y1367 = myPane.AddYAxis("");
            var y1368 = myPane.AddYAxis("");
            var y1369 = myPane.AddYAxis("");
            var y1370 = myPane.AddYAxis("");
            var y1371 = myPane.AddYAxis("");
            var y1372 = myPane.AddYAxis("");
            var y1373 = myPane.AddYAxis("");
            var y1374 = myPane.AddYAxis("");
            var y1375 = myPane.AddYAxis("");
            var y1376 = myPane.AddYAxis("");
            var y1377 = myPane.AddYAxis("");
            var y1378 = myPane.AddYAxis("");
            var y1379 = myPane.AddYAxis("");
            var y1380 = myPane.AddYAxis("");
            var y1381 = myPane.AddYAxis("");
            var y1382 = myPane.AddYAxis("");
            var y1383 = myPane.AddYAxis("");
            var y1384 = myPane.AddYAxis("");
            var y1385 = myPane.AddYAxis("");
            var y1386 = myPane.AddYAxis("");
            var y1387 = myPane.AddYAxis("");
            var y1388 = myPane.AddYAxis("");
            var y1389 = myPane.AddYAxis("");
            var y1390 = myPane.AddYAxis("");
            var y1391 = myPane.AddYAxis("");
            var y1392 = myPane.AddYAxis("");
            var y1393 = myPane.AddYAxis("");
            var y1394 = myPane.AddYAxis("");
            var y1395 = myPane.AddYAxis("");
            var y1396 = myPane.AddYAxis("");
            var y1397 = myPane.AddYAxis("");
            var y1398 = myPane.AddYAxis("");
            var y1399 = myPane.AddYAxis("");
            var y1400 = myPane.AddYAxis("");
            var y1401 = myPane.AddYAxis("");
            var y1402 = myPane.AddYAxis("");
            var y1403 = myPane.AddYAxis("");
            var y1404 = myPane.AddYAxis("");
            var y1405 = myPane.AddYAxis("");
            var y1406 = myPane.AddYAxis("");
            var y1407 = myPane.AddYAxis("");
            var y1408 = myPane.AddYAxis("");
            var y1409 = myPane.AddYAxis("");
            var y1410 = myPane.AddYAxis("");
            var y1411 = myPane.AddYAxis("");
            var y1412 = myPane.AddYAxis("");
            var y1413 = myPane.AddYAxis("");
            var y1414 = myPane.AddYAxis("");
            var y1415 = myPane.AddYAxis("");
            var y1416 = myPane.AddYAxis("");
            var y1417 = myPane.AddYAxis("");
            var y1418 = myPane.AddYAxis("");
            var y1419 = myPane.AddYAxis("");
            var y1420 = myPane.AddYAxis("");
            var y1421 = myPane.AddYAxis("");
            var y1422 = myPane.AddYAxis("");
            var y1423 = myPane.AddYAxis("");
            var y1424 = myPane.AddYAxis("");
            var y1425 = myPane.AddYAxis("");
            var y1426 = myPane.AddYAxis("");
            var y1427 = myPane.AddYAxis("");
            var y1428 = myPane.AddYAxis("");
            var y1429 = myPane.AddYAxis("");
            var y1430 = myPane.AddYAxis("");
            var y1431 = myPane.AddYAxis("");
            var y1432 = myPane.AddYAxis("");
            var y1433 = myPane.AddYAxis("");
            var y1434 = myPane.AddYAxis("");
            var y1435 = myPane.AddYAxis("");
            var y1436 = myPane.AddYAxis("");
            var y1437 = myPane.AddYAxis("");
            var y1438 = myPane.AddYAxis("");
            var y1439 = myPane.AddYAxis("");
            var y1440 = myPane.AddYAxis("");
            var y1441 = myPane.AddYAxis("");
            var y1442 = myPane.AddYAxis("");
            var y1443 = myPane.AddYAxis("");
            var y1444 = myPane.AddYAxis("");
            var y1445 = myPane.AddYAxis("");
            var y1446 = myPane.AddYAxis("");
            var y1447 = myPane.AddYAxis("");
            var y1448 = myPane.AddYAxis("");
            var y1449 = myPane.AddYAxis("");
            var y1450 = myPane.AddYAxis("");
            var y1451 = myPane.AddYAxis("");
            var y1452 = myPane.AddYAxis("");
            var y1453 = myPane.AddYAxis("");
            var y1454 = myPane.AddYAxis("");
            var y1455 = myPane.AddYAxis("");
            var y1456 = myPane.AddYAxis("");
            var y1457 = myPane.AddYAxis("");
            var y1458 = myPane.AddYAxis("");
            var y1459 = myPane.AddYAxis("");
            var y1460 = myPane.AddYAxis("");
            var y1461 = myPane.AddYAxis("");
            var y1462 = myPane.AddYAxis("");
            var y1463 = myPane.AddYAxis("");
            var y1464 = myPane.AddYAxis("");
            var y1465 = myPane.AddYAxis("");
            var y1466 = myPane.AddYAxis("");
            var y1467 = myPane.AddYAxis("");
            var y1468 = myPane.AddYAxis("");
            var y1469 = myPane.AddYAxis("");
            var y1470 = myPane.AddYAxis("");
            var y1471 = myPane.AddYAxis("");
            var y1472 = myPane.AddYAxis("");
            var y1473 = myPane.AddYAxis("");
            var y1474 = myPane.AddYAxis("");
            var y1475 = myPane.AddYAxis("");
            var y1476 = myPane.AddYAxis("");
            var y1477 = myPane.AddYAxis("");
            var y1478 = myPane.AddYAxis("");
            var y1479 = myPane.AddYAxis("");
            var y1480 = myPane.AddYAxis("");
            var y1481 = myPane.AddYAxis("");
            var y1482 = myPane.AddYAxis("");
            var y1483 = myPane.AddYAxis("");
            var y1484 = myPane.AddYAxis("");
            var y1485 = myPane.AddYAxis("");
            var y1486 = myPane.AddYAxis("");
            var y1487 = myPane.AddYAxis("");
            var y1488 = myPane.AddYAxis("");
            var y1489 = myPane.AddYAxis("");
            var y1490 = myPane.AddYAxis("");
            var y1491 = myPane.AddYAxis("");
            var y1492 = myPane.AddYAxis("");
            var y1493 = myPane.AddYAxis("");
            var y1494 = myPane.AddYAxis("");
            var y1495 = myPane.AddYAxis("");
            var y1496 = myPane.AddYAxis("");
            var y1497 = myPane.AddYAxis("");
            var y1498 = myPane.AddYAxis("");
            var y1499 = myPane.AddYAxis("");
            var y1500 = myPane.AddYAxis("");
            var y1501 = myPane.AddYAxis("");
            var y1502 = myPane.AddYAxis("");
            var y1503 = myPane.AddYAxis("");
            var y1504 = myPane.AddYAxis("");
            var y1505 = myPane.AddYAxis("");
            var y1506 = myPane.AddYAxis("");
            var y1507 = myPane.AddYAxis("");
            var y1508 = myPane.AddYAxis("");
            var y1509 = myPane.AddYAxis("");
            var y1510 = myPane.AddYAxis("");
            var y1511 = myPane.AddYAxis("");
            var y1512 = myPane.AddYAxis("");
            var y1513 = myPane.AddYAxis("");
            var y1514 = myPane.AddYAxis("");
            var y1515 = myPane.AddYAxis("");
            var y1516 = myPane.AddYAxis("");
            var y1517 = myPane.AddYAxis("");
            var y1518 = myPane.AddYAxis("");
            var y1519 = myPane.AddYAxis("");
            var y1520 = myPane.AddYAxis("");
            var y1521 = myPane.AddYAxis("");
            var y1522 = myPane.AddYAxis("");
            var y1523 = myPane.AddYAxis("");
            var y1524 = myPane.AddYAxis("");
            var y1525 = myPane.AddYAxis("");
            var y1526 = myPane.AddYAxis("");
            var y1527 = myPane.AddYAxis("");
            var y1528 = myPane.AddYAxis("");
            var y1529 = myPane.AddYAxis("");
            var y1530 = myPane.AddYAxis("");
            var y1531 = myPane.AddYAxis("");
            var y1532 = myPane.AddYAxis("");
            var y1533 = myPane.AddYAxis("");
            var y1534 = myPane.AddYAxis("");
            var y1535 = myPane.AddYAxis("");
            var y1536 = myPane.AddYAxis("");
            var y1537 = myPane.AddYAxis("");
            var y1538 = myPane.AddYAxis("");
            var y1539 = myPane.AddYAxis("");
            var y1540 = myPane.AddYAxis("");
            var y1541 = myPane.AddYAxis("");
            var y1542 = myPane.AddYAxis("");
            var y1543 = myPane.AddYAxis("");
            var y1544 = myPane.AddYAxis("");
            var y1545 = myPane.AddYAxis("");
            var y1546 = myPane.AddYAxis("");
            var y1547 = myPane.AddYAxis("");
            var y1548 = myPane.AddYAxis("");
            var y1549 = myPane.AddYAxis("");
            var y1550 = myPane.AddYAxis("");
            var y1551 = myPane.AddYAxis("");
            var y1552 = myPane.AddYAxis("");
            var y1553 = myPane.AddYAxis("");
            var y1554 = myPane.AddYAxis("");
            var y1555 = myPane.AddYAxis("");
            var y1556 = myPane.AddYAxis("");
            var y1557 = myPane.AddYAxis("");
            var y1558 = myPane.AddYAxis("");
            var y1559 = myPane.AddYAxis("");
            var y1560 = myPane.AddYAxis("");
            var y1561 = myPane.AddYAxis("");
            var y1562 = myPane.AddYAxis("");
            var y1563 = myPane.AddYAxis("");
            var y1564 = myPane.AddYAxis("");
            var y1565 = myPane.AddYAxis("");
            var y1566 = myPane.AddYAxis("");
            var y1567 = myPane.AddYAxis("");
            var y1568 = myPane.AddYAxis("");
            var y1569 = myPane.AddYAxis("");
            var y1570 = myPane.AddYAxis("");
            var y1571 = myPane.AddYAxis("");
            var y1572 = myPane.AddYAxis("");
            var y1573 = myPane.AddYAxis("");
            var y1574 = myPane.AddYAxis("");
            var y1575 = myPane.AddYAxis("");
            var y1576 = myPane.AddYAxis("");
            var y1577 = myPane.AddYAxis("");
            var y1578 = myPane.AddYAxis("");
            var y1579 = myPane.AddYAxis("");
            var y1580 = myPane.AddYAxis("");
            var y1581 = myPane.AddYAxis("");
            var y1582 = myPane.AddYAxis("");
            var y1583 = myPane.AddYAxis("");
            var y1584 = myPane.AddYAxis("");
            var y1585 = myPane.AddYAxis("");
            var y1586 = myPane.AddYAxis("");
            var y1587 = myPane.AddYAxis("");
            var y1588 = myPane.AddYAxis("");
            var y1589 = myPane.AddYAxis("");
            var y1590 = myPane.AddYAxis("");
            var y1591 = myPane.AddYAxis("");
            var y1592 = myPane.AddYAxis("");
            var y1593 = myPane.AddYAxis("");
            var y1594 = myPane.AddYAxis("");
            var y1595 = myPane.AddYAxis("");
            var y1596 = myPane.AddYAxis("");
            var y1597 = myPane.AddYAxis("");
            var y1598 = myPane.AddYAxis("");
            var y1599 = myPane.AddYAxis("");
            var y1600 = myPane.AddYAxis("");
            var y1601 = myPane.AddYAxis("");
            var y1602 = myPane.AddYAxis("");
            var y1603 = myPane.AddYAxis("");
            var y1604 = myPane.AddYAxis("");
            var y1605 = myPane.AddYAxis("");
            var y1606 = myPane.AddYAxis("");
            var y1607 = myPane.AddYAxis("");
            var y1608 = myPane.AddYAxis("");
            var y1609 = myPane.AddYAxis("");
            var y1610 = myPane.AddYAxis("");
            var y1611 = myPane.AddYAxis("");
            var y1612 = myPane.AddYAxis("");
            var y1613 = myPane.AddYAxis("");
            var y1614 = myPane.AddYAxis("");
            var y1615 = myPane.AddYAxis("");
            var y1616 = myPane.AddYAxis("");
            var y1617 = myPane.AddYAxis("");
            var y1618 = myPane.AddYAxis("");
            var y1619 = myPane.AddYAxis("");
            var y1620 = myPane.AddYAxis("");
            var y1621 = myPane.AddYAxis("");
            var y1622 = myPane.AddYAxis("");
            var y1623 = myPane.AddYAxis("");
            var y1624 = myPane.AddYAxis("");
            var y1625 = myPane.AddYAxis("");
            var y1626 = myPane.AddYAxis("");
            var y1627 = myPane.AddYAxis("");
            var y1628 = myPane.AddYAxis("");
            var y1629 = myPane.AddYAxis("");
            var y1630 = myPane.AddYAxis("");
            var y1631 = myPane.AddYAxis("");
            var y1632 = myPane.AddYAxis("");
            var y1633 = myPane.AddYAxis("");
            var y1634 = myPane.AddYAxis("");
            var y1635 = myPane.AddYAxis("");
            var y1636 = myPane.AddYAxis("");
            var y1637 = myPane.AddYAxis("");
            var y1638 = myPane.AddYAxis("");
            var y1639 = myPane.AddYAxis("");
            var y1640 = myPane.AddYAxis("");
            var y1641 = myPane.AddYAxis("");
            var y1642 = myPane.AddYAxis("");
            var y1643 = myPane.AddYAxis("");
            var y1644 = myPane.AddYAxis("");
            var y1645 = myPane.AddYAxis("");
            var y1646 = myPane.AddYAxis("");
            var y1647 = myPane.AddYAxis("");
            var y1648 = myPane.AddYAxis("");
            var y1649 = myPane.AddYAxis("");
            var y1650 = myPane.AddYAxis("");
            var y1651 = myPane.AddYAxis("");
            var y1652 = myPane.AddYAxis("");
            var y1653 = myPane.AddYAxis("");
            var y1654 = myPane.AddYAxis("");
            var y1655 = myPane.AddYAxis("");
            var y1656 = myPane.AddYAxis("");
            var y1657 = myPane.AddYAxis("");
            var y1658 = myPane.AddYAxis("");
            var y1659 = myPane.AddYAxis("");
            var y1660 = myPane.AddYAxis("");
            var y1661 = myPane.AddYAxis("");
            var y1662 = myPane.AddYAxis("");
            var y1663 = myPane.AddYAxis("");
            var y1664 = myPane.AddYAxis("");
            var y1665 = myPane.AddYAxis("");
            var y1666 = myPane.AddYAxis("");
            var y1667 = myPane.AddYAxis("");
            var y1668 = myPane.AddYAxis("");
            var y1669 = myPane.AddYAxis("");
            var y1670 = myPane.AddYAxis("");
            var y1671 = myPane.AddYAxis("");
            var y1672 = myPane.AddYAxis("");
            var y1673 = myPane.AddYAxis("");
            var y1674 = myPane.AddYAxis("");
            var y1675 = myPane.AddYAxis("");
            var y1676 = myPane.AddYAxis("");
            var y1677 = myPane.AddYAxis("");
            var y1678 = myPane.AddYAxis("");
            var y1679 = myPane.AddYAxis("");
            var y1680 = myPane.AddYAxis("");
            var y1681 = myPane.AddYAxis("");
            var y1682 = myPane.AddYAxis("");
            var y1683 = myPane.AddYAxis("");
            var y1684 = myPane.AddYAxis("");
            var y1685 = myPane.AddYAxis("");
            var y1686 = myPane.AddYAxis("");
            var y1687 = myPane.AddYAxis("");
            var y1688 = myPane.AddYAxis("");
            var y1689 = myPane.AddYAxis("");
            var y1690 = myPane.AddYAxis("");
            var y1691 = myPane.AddYAxis("");
            var y1692 = myPane.AddYAxis("");
            var y1693 = myPane.AddYAxis("");
            var y1694 = myPane.AddYAxis("");
            var y1695 = myPane.AddYAxis("");
            var y1696 = myPane.AddYAxis("");
            var y1697 = myPane.AddYAxis("");
            var y1698 = myPane.AddYAxis("");
            var y1699 = myPane.AddYAxis("");


        }
    }
}