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
using System.IO;

namespace poe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        public void tb1_TextChanged(object sender, EventArgs e)
        {
            
        }


        public void tb2_TextChanged(object sender, EventArgs e)
        {
            btn1_div.Enabled = true;
            btn2_mult.Enabled = true;
            btn3_sub.Enabled = true;
            btn4_add.Enabled = true;
        }

        

        private void btn3_sub_Click(object sender, EventArgs e)
        {
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_clear_Click(object sender, EventArgs e)
        {
            tb_erg.Text = "";
            tb1.Text = "";
            tb2.Text = "";

        }

        private void btn_fib_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void btn_end_fib_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            double[] fib = new double[50];
            double sum = 0;
            fib[0] = 1;
            fib[1] = 1;
            for (int i = 2; i < 50; i++)
            {
                fib[i] = fib[i - 1] + fib[i - 2];
               // tb_erg.Text = fib[i].ToString();
               // tb_erg.Update();
                Thread.Sleep(100);
                backgroundWorker1.ReportProgress((i+1)*2);
                sum = fib[i];



                if (backgroundWorker1.CancellationPending)

                {
                    e.Cancel = true;
                    backgroundWorker1.ReportProgress(0);
                    return;
                }
            }

            e.Result = sum;
           


        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label4.Text = e.ProgressPercentage.ToString() + " %";
            tb_erg.Text = "calculating";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                label4.Text = "Process cancelled";
            }

            else if (e.Error != null)
            {
                label4.Text = e.Error.Message;
            }

            else
            {
                label4.Text = e.Result.ToString();
                tb_erg.Text = e.Result.ToString();
                
            }

        }

        private void label4_Click(object sender, EventArgs e)
        {
         
        }
    }








    public class calc
    {
        public static double mult(double a, double b)
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






/*
 string path = @"c:\temp\MyTest.txt";

        // This text is added only once to the file.
        if (!File.Exists(path))
        {
            // Create a file to write to.
            string[] createText = { "Hello", "And", "Welcome" };
            File.WriteAllLines(path, createText, Encoding.UTF8);
        }

        // This text is always added, making the file longer over time
        // if it is not deleted.
        string appendText = "This is extra text" + Environment.NewLine;
        File.AppendAllText(path, appendText, Encoding.UTF8);

        // Open the file to read from.
        string[] readText = File.ReadAllLines(path, Encoding.UTF8);
        foreach (string s in readText)
        {
            Console.WriteLine(s);
        }*/
