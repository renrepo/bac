﻿using System;
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
using System.Globalization;

namespace XPS
{
    public partial class XPS : Form
    {

        string filePath = Path.GetFullPath("Bindungsenergien.csv");
        string filePath2 = Path.GetFullPath("colors2.csv");
        List<List<string>> row = new List<List<string>>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        Dictionary<string, string> fab = new Dictionary<string, string>(); 
        GraphPane myPane;
        LineItem myCurve;
        List<string> values = new List<string>();
        public int num_gauss = 20;
        PointPairList list1 = new PointPairList();
        List<string> fuerlabels = new List<string>();
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};
        TextObj pane_labs;
        YAxis yaxis = new YAxis();


        int i;
        int end = 0;
        // bw-DoWork

        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string now = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string curr_time;



        public XPS()
        {
            InitializeComponent();
            myPane = zedGraphControl1.GraphPane;
        }


        private void create_graph (GraphPane myPane)
        {
            myPane.Title.Text = "XPS spectra";
            myPane.Title.FontSpec.Size = 13;
            myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 11;
            myPane.YAxis.Title.Text = "counts";
            myPane.YAxis.Title.FontSpec.Size = 11;
            myPane.Fill.Color = Color.LightGray;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            // StreamReader sr = new StreamReader(filePath);
            row = File.ReadAllLines(filePath).Select(l => l.Split(',').ToList()).ToList();
            fab = File.ReadLines(filePath2).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);
            //MessageBox.Show(row[6][3]);
            var num = row.Count;
            
            for (int i = 0; i < num; i++)
            {
                dictionary.Add(row[i][1],row[i][0]);
            }
            create_graph(myPane);

            try
            {
                if (!Directory.Exists(path + @"\Logfiles_XPS"))
                {
                    Directory.CreateDirectory(path + @"\Logfiles_XPS");
                }
            }
            catch
            {
                MessageBox.Show("Can't create Folder 'Logfile' on Desktop");
            }
        }



        private void elementnames_Popup(object sender, PopupEventArgs e)
        {
        }




        private void tableLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            tableLayoutPanel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }
        //string line = File.ReadLines(path + @"\Bindungsenergien.csv").Skip(14).Take(1).First()


        public void colorchanger(object sender, YAxis ya)
        {
            Button btn = (Button)sender;
            var panel = sender as Control;
            var thePanelName = panel.Name;
            string col = fab[thePanelName];
            int zeile = Convert.ToInt32(dictionary[thePanelName]) - 1;
            float value;

            if (btn.ForeColor == Color.DimGray)
            {
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;

                for (int i = 2; i <= 25; i++)
                {
                    bool result = float.TryParse(row[zeile][i], out value);

                    if (result)
                    {
                        pane_labs = new TextObj((row[zeile][i] + "\n" + row[zeile][1] + " " + scores[i]), float.Parse(row[zeile][i], CultureInfo.InvariantCulture), -0.05,
                            CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        pane_labs.FontSpec.Size = 10f;
                        pane_labs.FontSpec.Angle = 40;
                        pane_labs.FontSpec.Fill.Color = Color.Transparent;
                        pane_labs.FontSpec.FontColor = Color.DimGray;
                        pane_labs.FontSpec.Border.IsVisible = false;
                        pane_labs.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(pane_labs);
                        fuerlabels.Add(thePanelName);

                        ya = new YAxis();                        
                        ya.Scale.IsVisible = false;
                        ya.Scale.LabelGap = 0f;
                        ya.Title.Gap = 0f;
                        ya.Title.Text = "";
                        ya.Color = Color.FromName(col);
                        ya.AxisGap = 0f;
                        ya.Scale.Format = "#";
                        ya.Scale.Min = 0;
                        ya.Scale.Mag = 0;
                        ya.MajorTic.IsAllTics = false;
                        ya.MinorTic.IsAllTics = false;                      
                        ya.Cross = Double.Parse(row[zeile][i], CultureInfo.InvariantCulture);
                        ya.IsVisible = true;
                        ya.MajorGrid.IsZeroLine = false;
                        // hides xaxis
                        myPane.YAxisList.Add(ya);
                    }
                }
                zedGraphControl1.Refresh();
            }

            else
            {
                btn.ForeColor = Color.DimGray;
                btn.Font = new Font("Arial", 11, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.DimGray;

                int laenge = fuerlabels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (fuerlabels[y] == thePanelName)
                    {
                        fuerlabels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y+1);
                    }
                }
                zedGraphControl1.Refresh();
            }
        }



        private void set_element(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender, yaxis);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }
        string path2;


        private void btn_start_Click(object sender, EventArgs e)
        {
            if ((!bW_data.IsBusy))
            {
                end = 0;
                myCurve = myPane.AddCurve("",
                list1, Color.Black, SymbolType.None);
                curr_time = now;
                string u = tb_safe.Text + curr_time;
                DirectoryInfo dl =  Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_XPS\", " " + tb_safe.Text + "_" + curr_time + "\\"));
                path2 = dl.FullName;
                using (var file = new StreamWriter(path2 + "data.txt", true))
                {
                    file.WriteLine("#XPS-spectrum" + Environment.NewLine);
                    file.WriteLine("#Date/time: \t{0}", now);
                    file.WriteLine("#X-ray source :\t" + Environment.NewLine);
                    file.WriteLine("#E_b \t counts");                   
                }

                bW_data.RunWorkerAsync(); //run bW if it is not still running
                btn_start.Enabled = false;
                btn_can.Enabled = true;
                tb_show.Enabled = true;
            }

            else
	        {
                    MessageBox.Show("Backgroundworker is busy!");
            }                      
        }



        private void bW_data_DoWork(object sender, DoWorkEventArgs e)
        {
            for (i = 0; i <= num_gauss; i++) 
            {
                end += i;
                values.Add(end + "\t" + 2 * end);
                bW_data.ReportProgress(100 * i / num_gauss, end);
                //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
                using (var file = new StreamWriter(path2 + "data.txt", true))
                {
                    file.WriteLine(i.ToString("000") + "\t" + end.ToString("00000"));
                }
                Thread.Sleep(500);

                if (bW_data.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_data.ReportProgress(0);
                    break; //warum? ist wichtig!
                }
            }
            //safer.safe(path,list_gauss);
            e.Result = end; //stores the results of what has been done in bW
        }

        private void bW_data_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
            tb_show.Text = Convert.ToString(e.UserState);
            // x = e.ProgressPercentage
            //list1.Add(i, Convert.ToDouble(e.UserState));
            list1.Add(i, end);
            myCurve.AddPoint(i, end);

            zedGraphControl1.Invalidate();
            zedGraphControl1.AxisChange();
        }

        private void bW_data_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path2 + "data.txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
                browse.Enabled = true;
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_show.Text = e.Error.Message;
            }

            else
            {
                //tb_show.Text = Convert.ToString(e.UserState);
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                browse.Enabled = true;
                zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot.png"));
            }
           
        }



        private void btn_can_Click(object sender, EventArgs e)
        {
            if (bW_data.IsBusy) // .IsBusy is true, if bW is running, otherwise false
            {
                bW_data.CancelAsync(); //cancels the background operation and sets CancellationPendiung to true!
                btn_clear.Enabled = true;
                btn_can.Enabled = false;
            }
        }



        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (bW_data.IsBusy)
            {
                MessageBox.Show("Backgroundworker is still busy!");
            }

            else
            {
                tb_show.Text = "";
                lb_perc_gauss.Text = "%";
                btn_start.Enabled = true;
                btn_clear.Enabled = false;
                browse.Enabled = false;
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                list1.Clear();
                progressBar1.Value = 0;
                create_graph(myPane);
                zedGraphControl1.Refresh();
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
           set_element(sender, e);
        }

        private void He_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Li_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Be_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void B_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void C_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void N_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void O_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void F_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ne_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Na_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Mg_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Al_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Si_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void P_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void S_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Cl_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ar_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void K_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ca_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Sc_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ti_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void V_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Cr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Mn_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Fe_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Co_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ni_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Cu_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Zn_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ga_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ge_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void As_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Se_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Br_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Kr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Rb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Sr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Y_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Zr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Nb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Mo_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Tc_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ru_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Rh_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pd_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ag_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Cd_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void In_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Sn_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Sb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Te_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void I_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Xe_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Cs_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ba_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void La_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Hf_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ta_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void W_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Re_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Os_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ir_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pt_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Au_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Hg_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Tl_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Bi_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Po_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void At_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Rn_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Fr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ra_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ac_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ce_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pr_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Nd_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pm_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Sm_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Eu_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Gd_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Tb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Dy_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Ho_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Er_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Tm_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Yb_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Lu_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Th_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void Pa_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        private void U_MouseDown(object sender, MouseEventArgs e)
        {
            set_element(sender, e);
        }

        int counter = 2;

        private void browse_Click(object sender, EventArgs e)
        {
            zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot" + counter + ".png"));
            tb_safe.Text = "Fig. saved";
            tb_safe.BackColor = Color.LimeGreen;
            counter += 1;

            //SaveFileDialog sv = new SaveFileDialog();
            //sv.Title = "Save data";
            //sv.ShowDialog();
        }
    }
}