using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace poe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        bool v1 = false;
        bool v2 = false;
        public void tb1_TextChanged(object sender, EventArgs e)

        {
            bool v1 = true;
        }


        public void tb2_TextChanged(object sender, EventArgs e)
        {
            bool v2 = true;
        }

        private void btn3_sub_Click(object sender, EventArgs e)
        {
            if (v1 == true && v2 == true)
            {
                btn3_sub.Enabled = false;
                btn3_sub.Update();
            }
            tb_erg.Text = calc.sub(Convert.ToInt32(tb1.Text), Convert.ToInt32(tb2.Text)).ToString();
        }

        private void btn4_add_Click(object sender, EventArgs e)
        {
            tb_erg.Text = calc.Add(Convert.ToInt32(tb1.Text), Convert.ToInt32(tb2.Text)).ToString();
        }

        private void tb_erg_TextChanged(object sender, EventArgs e)
        {

        }

        private void btn2_mult_Click(object sender, EventArgs e)
        {
            tb_erg.Text = calc.mult(Convert.ToInt32(tb1.Text), Convert.ToInt32(tb2.Text)).ToString();
        }

        private void btn1_div_Click(object sender, EventArgs e)
        {
            tb_erg.Text = calc.div(Convert.ToInt32(tb1.Text), Convert.ToInt32(tb2.Text)).ToString();
        }
    }


    public class calc
    {
        public static double mult(double a,double b)
        {
            return a * b;   
        }

        public static double Add(double a, double b)
        {
            return a + b;
        }

        public static double sub(double a, double b)

        {
            return a - b;       
        }

        public static double div(double a, double b)
        {
            if (b == 0)
            {
                MessageBox.Show("Division durch 0!");
                return 0;
            }
            else
            {
                return a / b;
            }
        }


    }

}
