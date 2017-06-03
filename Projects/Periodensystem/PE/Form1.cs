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

namespace PE
{
    public partial class Form1 : Form
    {

        List<List<string>> row = new List<List<string>>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();


        public Form1()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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
        }


        private void elementnames_Popup(object sender, PopupEventArgs e)
        {

        }




        string font = "Arial";
        int fontsize_activated = 12;
        int fontsize_deactivated = 11;
        int bordersize_activated = 2;
        int bordersize_deactivated = 1;
        string not_pressed = "DimGray";
        string pressed = "black";



  
    
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

        public void clearlabel(object sender)
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





        private void H_Click(object sender, EventArgs e)
        {
            // Button btn = (Button)sender;
            colorchanger((Button)sender);
        }



        private void He_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }



        private void Li_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }



        private void Be_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }


        private void B_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void C_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }


        private void N_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }


        private void O_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void F_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }


        private void Ne_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Na_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Mg_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Al_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Si_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void P_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void S_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Cl_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ar_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void K_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ca_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Sc_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ti_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void V_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Cr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Mn_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Fe_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Co_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ni_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Cu_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Zn_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ga_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ge_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void As_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Se_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Br_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Kr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Rb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Sr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Y_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Zr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Nb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Mo_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Tc_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ru_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Rh_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pd_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ag_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Cd_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void In_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Sn_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Sb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Te_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void I_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Xe_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Cs_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ba_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void La_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Hf_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ta_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void W_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Re_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Os_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ir_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pt_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Au_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Hg_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Tl_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Bi_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Po_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void At_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Rn_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Fr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ra_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ac_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ce_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pr_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Nd_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pm_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Sm_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Eu_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Gd_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Tb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Dy_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Ho_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Er_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Tm_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Yb_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Lu_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Th_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void Pa_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }

        private void U_Click(object sender, EventArgs e)
        {
            colorchanger((Button)sender);
        }















        private void H_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }



        private void He_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }



        private void Li_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }



        private void Be_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }


        private void B_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void C_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }


        private void N_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }


        private void O_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void F_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }


        private void Ne_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Na_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Mg_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Al_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Si_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void P_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void S_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Cl_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ar_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void K_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ca_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Sc_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ti_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void V_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Cr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Mn_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Fe_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Co_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ni_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Cu_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Zn_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ga_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ge_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void As_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Se_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Br_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Kr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Rb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Sr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Y_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Zr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Nb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Mo_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Tc_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ru_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Rh_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pd_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ag_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Cd_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void In_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Sn_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Sb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Te_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void I_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Xe_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Cs_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ba_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void La_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Hf_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ta_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void W_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Re_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Os_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ir_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pt_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Au_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Hg_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Tl_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Bi_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Po_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void At_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Rn_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Fr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ra_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ac_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ce_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pr_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Nd_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pm_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Sm_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Eu_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Gd_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Tb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Dy_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Ho_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Er_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Tm_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Yb_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Lu_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Th_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void Pa_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }

        private void U_MouseEnter(object sender, EventArgs e)
        {
            labelchanger(sender);
        }
















        private void H_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }



        private void He_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }



        private void Li_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }



        private void Be_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }


        private void B_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void C_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }


        private void N_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }


        private void O_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void F_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }


        private void Ne_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Na_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Mg_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Al_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Si_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void P_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void S_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Cl_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ar_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void K_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ca_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Sc_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ti_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void V_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Cr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Mn_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Fe_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Co_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ni_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Cu_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Zn_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ga_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ge_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void As_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Se_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Br_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Kr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Rb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Sr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Y_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Zr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Nb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Mo_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Tc_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ru_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Rh_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pd_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ag_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Cd_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void In_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Sn_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Sb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Te_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void I_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Xe_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Cs_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ba_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void La_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Hf_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ta_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void W_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Re_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Os_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ir_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pt_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Au_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Hg_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Tl_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Bi_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Po_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void At_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Rn_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Fr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ra_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ac_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ce_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pr_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Nd_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pm_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Sm_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Eu_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Gd_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Tb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Dy_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Ho_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Er_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Tm_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Yb_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Lu_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Th_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void Pa_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }

        private void U_MouseLeave(object sender, EventArgs e)
        {
            clearlabel(sender);
        }
    }
}
