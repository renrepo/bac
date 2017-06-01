using System;
using System.Drawing;
using System.Windows.Forms;

namespace PE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

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














        private void button159_Click(object sender, EventArgs e)
        {

        }


        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button139_Click(object sender, EventArgs e)
        {

        }

        private void button134_Click(object sender, EventArgs e)
        {

        }
    }
}
