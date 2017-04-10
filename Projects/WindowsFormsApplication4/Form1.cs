using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WindowsFormsApplication4
{
    public partial class Form1 : Form
    {
        Thread conversion;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conversion = new Thread(new ThreadStart(t_Conversion));
        }



        private void btn_start_Click(object sender, EventArgs e)
        {
            if (tB_start.Text == "start")
            {
                tB_start.Text = "stop";
            }
            else
            {
                tB_start.Text = "start";
            }

            //tB_start.BackColor = Color.LawnGreen;
            //Thread.Sleep(10000);

            conversion.Start();
        }

        private void btn_LoadFile_Click(object sender, EventArgs e)
        {
            double[,] messdaten = new double[5,2];
            string temp;
            StreamReader file;
            StreamWriter file_out;
            OpenFileDialog openFileDialog1;


            openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                file = new StreamReader(openFileDialog1.FileName);
                

                for (int i = 0; i < 5; i++)
                {
                    temp = file.ReadLine();
                    messdaten[i, 1] = Convert.ToDouble(temp.Substring(2));
                    temp = temp.Remove(1);
                    messdaten[i, 0] = Convert.ToDouble(temp.Substring(0));
                }
            }

            for(int i=0; i<5; i++)
            {
                messdaten[i, 0] = messdaten[i, 0] * 2;
                messdaten[i, 1] = messdaten[i, 1] * 2;
            }

            file_out = new StreamWriter(openFileDialog1.FileName + "2");

            for (int i = 0; i < 5; i++)
            {
                file_out.WriteLine(Convert.ToString(messdaten[i,0]) + "\t" + Convert.ToString(messdaten[i, 1]));
            }
            file_out.Close();
            btn_LoadFile.Text = "fertig";
        }


        public void t_Conversion()
        {
            //tB_start.BackColor = Color.Red;
            Thread.Sleep(10000);
        }
    }
}
