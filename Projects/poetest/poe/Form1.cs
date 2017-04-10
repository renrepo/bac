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


        private void btn1_Click(object sender, EventArgs e)
        {
            string erg = Testclass.Add(Convert.ToInt16(tb1.Text), Convert.ToInt16(tb2.Text)).ToString();
            btn1.Text = "Ergebnis " + erg;
        }


        public void btn2_Click(object sender, EventArgs e)
        {
            string erg = Testclass.mult(Convert.ToInt16(tb1.Text), Convert.ToInt16(tb2.Text)).ToString();
            btn2.Text = "Ergebnis " + erg;
        }


        public void tb1_TextChanged(object sender, EventArgs e)

        {

        }


        public void tb2_TextChanged(object sender, EventArgs e)
        {

        }
    }



    public class Testclass
    {
        public static double mult(double a,double b)
        {
            return a * b;   
        }

        public static double Add(double number1, double number2)
        {
            return number1 + number2;
        }
    }



}
