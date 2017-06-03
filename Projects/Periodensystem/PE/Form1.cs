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

namespace PE
{
    public partial class Form1 : Form
    {


        List<List<string>> row = new List<List<string>>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string font = "Arial";
        int fontsize_activated = 12;
        int fontsize_deactivated = 11;
        int bordersize_activated = 2;
        int bordersize_deactivated = 1;
        string not_pressed = "DimGray";
        string pressed = "black";




        public Form1()
        {
            InitializeComponent();
        }


        

        private void Form1_Load(object sender, EventArgs e)
        {

            string filePath = System.IO.Path.GetFullPath("Bindungsenergiencsv.csv");
           // StreamReader sr = new StreamReader(filePath);
            row = File.ReadAllLines(filePath).Select(l => l.Split(',').ToList()).ToList();
            //MessageBox.Show(row[6][3]);
            var num = row.Count;
  
            for (int i = 0; i < num; i++)
            {
                dictionary.Add(row[i][1],row[i][0]);
            }


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



        public void colorchanger(object sender)
        {
            Button btn = (Button)sender;

            if (btn.ForeColor == Color.FromName(not_pressed))
            {
                btn.Font = new Font(font, fontsize_activated, FontStyle.Bold);
                btn.ForeColor = Color.FromName(pressed);
                btn.FlatAppearance.BorderColor = Color.FromName(pressed);
                btn.FlatAppearance.BorderSize = bordersize_activated;

            }

            else
            {
                btn.ForeColor = Color.FromName(not_pressed);
                btn.Font = new Font(font, fontsize_deactivated, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = bordersize_deactivated;
                btn.FlatAppearance.BorderColor = Color.FromName(not_pressed);
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








        private void H_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
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
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }




        private void H_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }



        private void He_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }



        private void Li_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }



        private void Be_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void B_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void C_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void N_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void O_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void F_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }


        private void Ne_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Na_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Al_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Si_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void P_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void S_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ar_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void K_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ca_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sc_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ti_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void V_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Fe_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Co_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ni_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cu_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Zn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ga_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ge_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void As_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Se_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Br_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Kr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            };
        }

        private void Rb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Y_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Zr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Nb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Mo_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tc_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ru_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Rh_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ag_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void In_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Te_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void I_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Xe_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Cs_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ba_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void La_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Hf_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ta_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void W_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Re_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Os_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ir_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pt_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Au_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Hg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Bi_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Po_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void At_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Rn_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Fr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ra_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ac_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ce_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pr_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Nd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Sm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Eu_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Gd_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Dy_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Ho_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Er_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Tm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Yb_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Lu_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Th_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void Pa_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

        private void U_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender);
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

            // Make up some data arrays based on the Sine function
            double x, y1, y2;
            PointPairList list1 = new PointPairList();
            PointPairList list2 = new PointPairList();
            for (int i = 0; i < 36; i++)
            {
                x = (double)i + 5;
                y1 = 1.5 + Math.Sin((double)i * 0.2);
                y2 = 3.0 * (1.5 + Math.Sin((double)i * 0.2));
                list1.Add(x, y1);
                list2.Add(x, y2);
            }

            // Generate a red curve with diamond
            // symbols, and "Porsche" in the legend
            LineItem myCurve = myPane.AddCurve("Porsche",
                  list1, Color.Red, SymbolType.Diamond);

            // Generate a blue curve with circle
            // symbols, and "Piper" in the legend
            LineItem myCurve2 = myPane.AddCurve("Piper",
                  list2, Color.Blue, SymbolType.Circle);

            // Tell ZedGraph to refigure the
            // axes since the data have changed
            zgc.AxisChange();
        }

    }
}