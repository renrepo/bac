using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArduinoDriver;
using ArduinoUploader;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;
using System.IO;

namespace Ardutest
{
    public partial class Form1 : Form
    {
        public SerialPort myport;


        double read = .0;
        double read1 = .0;
        double read2 = .0;
        double read3 = .0;

        Stopwatch sw2 = new Stopwatch();
        Stopwatch sw = new Stopwatch();




        public Form1()
        {
            InitializeComponent();
            myport = new SerialPort();
            myport.BaudRate = 115200;
            myport.PortName = "COM3";


            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + "1024" + "\\"));
            string path_logfile = dl.FullName;


        }

        private void Form1_Load(object sender, EventArgs e)
        {          
            var uploader = new ArduinoSketchUploader(
            new ArduinoSketchUploaderOptions()
            {
                FileName = @"C:\Users\Rene\Desktop\testsketch.ino",
                PortName = "COM3",
                ArduinoModel = ArduinoUploader.Hardware.ArduinoModel.UnoR3
            });

        }



        private void btn_dac_Click(object sender, EventArgs e)
        {
            sw2.Start();
            myport.Open();
            myport.WriteLine("O");
            myport.Close();
            textBox6.Text = sw2.Elapsed.TotalMilliseconds.ToString();
            sw2.Reset();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            double n = 1024;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + "1024" + "\\"));
            string path_logfile = dl.FullName;

            using (var file = new StreamWriter(path_logfile + n + ".txt", true))
            {
                file.WriteLine(
                    "#Bin. ADC1" + "\t" + "Bin. ADC2" + "\t" + "Bin. ADC3" + "\t" + "Bin. ADC4" + "\t" + "Scantime" + "\n"
                    );
            }




            double time = 0;
            myport.Open();

            for (int i = 0; i < 10000; i++)
            {
                sw.Start();
                myport.WriteLine("a");
                read = Convert.ToDouble(myport.ReadLine());
                read1 = Convert.ToDouble(myport.ReadLine());
                read2 = Convert.ToDouble(myport.ReadLine());
                read3 = Convert.ToDouble(myport.ReadLine());
                time = sw.Elapsed.TotalMilliseconds;



                textBox2.Text = time.ToString();
                textBox1.Text = (read / n).ToString();
                textBox3.Text = (read1 / n).ToString();
                textBox4.Text = (read2 / n).ToString();
                textBox5.Text = (read3 / n).ToString();
                sw.Reset();
                using (var file = new StreamWriter(path_logfile + n + ".txt", true))
                {
                    file.WriteLine(
                        (read / n).ToString("00000.0000") + "\t" +
                        (read1 / n).ToString("00000.0000") + "\t" +
                        (read2 / n).ToString("00000.0000") + "\t" +
                        (read3 / n).ToString("00000.0000") + "\t" +
                        (time*1000).ToString("000000") + "\t"
                        );
                }
            }
            myport.Close();
        }
    }
}
