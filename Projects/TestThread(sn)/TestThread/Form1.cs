using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TestThread
{
    public partial class Form1 : Form
    {

        Thread counterone;

        public Form1()
        {
            InitializeComponent();         
        }




        public void cn1()
        {
            int counter = 0;
            while (counter <=20)
            {
                label1.Text = counter.ToString();
                counter++;
                label1.Update();
                Thread.Sleep(400);
            }
        }

        public void cn2()
        {
            int counter = 0;
            while (counter <= 20)
            {
                label2.Text = counter.ToString();
                counter++;
                label2.Update();
                Thread.Sleep(400);
            }
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            counterone = new Thread(new ThreadStart(cn1));
            
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            cn2();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
