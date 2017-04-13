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

namespace multithread
{
    public partial class Form1 : Form
    {

        int num_gauss;
        int num_fib;


        public Form1()
        {
            InitializeComponent();
            tb_gauss_startvalue.BackColor = Color.Red;
            tb_fib_startvalue.BackColor = Color.Red;
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
                bW_gauss.ReportProgress(100 * i /num_gauss);
                Thread.Sleep(5);


                if (bW_gauss.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_gauss.ReportProgress(0);
                    return; //warum? ist wichtig!
                }

            }

            e.Result = end; //stores the results of what has been done in bW
        }



        private void bW_gauss_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {   //this event is raised, when the ReportProgress-Method is called (in DoWork!)
            progressBar1.Value = e.ProgressPercentage;
            lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
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















        private void btn_fib_Click(object sender, EventArgs e)
        {
            if (!bW_fib.IsBusy)
            {
                bW_fib.RunWorkerAsync();
                tb_fib_startvalue.Enabled = false;
            }
        }

        private void btn_fib_can_Click(object sender, EventArgs e)
        {
            if (bW_fib.IsBusy)
            {
                bW_fib.CancelAsync();
                btn_clear.Enabled = true;
            }
        }


        private void bW_fib_DoWork(object sender, DoWorkEventArgs e)
        {
            int i = 0;
            double end = 0;
            double[] fib = new double[num_fib];
            fib[0] = 1;
            fib[1] = 1;
            for (i = 2; i <= (num_fib - 1); i++)
            {
                fib[i] = fib[(i-1)] + fib[(i-2)];
                bW_fib.ReportProgress(100 * i / (num_fib - 1));
                Thread.Sleep(500);
                end = fib[i];



                if (bW_fib.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_fib.ReportProgress(0);
                    return;
                }

            }
            e.Result = end;
        }


        private void bW_fib_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar2.Value = e.ProgressPercentage;
            lb_perc_fib.Text = e.ProgressPercentage.ToString() + " %";
        }

        private void bW_fib_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                tb_fib.Text = "Cancelled!";
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_fib.Text = e.Error.Message;
            }

            else
            {
                tb_fib.Text = e.Result.ToString();
            }
        }











        private void btn_tester_Click(object sender, EventArgs e)
        {
            Random rnd = new Random();
            Color randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            btn_tester.BackColor = randomColor;
        }



        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (bW_fib.IsBusy)
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

            else
            {
                tb_fib.Text = "";
                tb_fib_startvalue.Text = "";
                lb_perc_fib.Text = "";
                tb_fib_startvalue.BackColor = Color.Red;
                tb_fib_startvalue.Enabled = true;
                progressBar2.Value = 0;

            }
        }


        private void tb_gauss_startvalue_TextChanged_1(object sender, EventArgs e)
        {
        
            string eingabe = tb_gauss_startvalue.Text;
            if (int.TryParse(eingabe,out num_gauss))
            {
                btn_gauss.Enabled = true;
                tb_gauss_startvalue.BackColor = Color.White;
                btn_gauss.Enabled = true;
            }
        }

        private void tb_fib_startvalue_TextChanged_1(object sender, EventArgs e)
        {
            string eingabe = tb_fib_startvalue.Text;
            if (int.TryParse(eingabe, out num_fib))
            {
                btn_fib.Enabled = true;
                tb_fib_startvalue.BackColor = Color.White;
                btn_fib.Enabled = true;
            }
        }
    }
}
