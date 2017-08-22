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
using ZedGraph;
using System.Globalization;
using System.Diagnostics;
using NationalInstruments.Visa;
using LabJack;

namespace XPS
{
    public partial class XPS : Form
    {

        double V_photon = 21.21;
        double W_aus = 4.5;
        double delta_v_channeltron = 2800;
        double ri = 106;
        double ra = 112;
        double v_ana_min;
        double v_ana_max;
        double v_ana_bind;
        double v_cem;
        double delta_v;
        double v_hem_in;
        double v_hem_out;

        string filePath = Path.GetFullPath("Bindungsenergien.csv");
        string filePath2 = Path.GetFullPath("colors2.csv");
        string filePath3 = Path.GetFullPath("electronconfiguration.csv");
        List<List<string>> row = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
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
        int ch_num;
        double max_curr = 0.003;
        int vnom = 4000;
        int sleep_vset = 8000;
        double perc_ramp = 40.000;


        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string now = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string curr_time;

        private MessageBasedSession iseg;
        private string lastResourceString = null;

        Dictionary<string, int> ch = new Dictionary<string, int>();
        Dictionary<string, int> th = new Dictionary<string, int>();
        Dictionary<string, int> rh = new Dictionary<string, int>();
        Dictionary<string, int> ph = new Dictionary<string, int>();
        Dictionary<string, string> nocheins = new Dictionary<string, string>();

        TextBox[] vset;
        TextBox[] vmin;
        TextBox[] vmax;
        TextBox[] vramp;
        TextBox[] vstep;
        TextBox[] vmeas;
        TextBox[] vmeas2;
        Button[] reload;
        Button[] reset;
        CheckBox[] stat;


        string AIN0 = "AIN0";
        string AIN2 = "AIN2";
        double v_labjack = 0;
        double value = 0;
        double value2 = 0;
        double value3 = 0;
        double vlens;
        int handle3 = 0;
        int handle2 = 0;
        int handle = 0;
        int handle4 = 0;

        ManualResetEvent _suspendEvent = new ManualResetEvent(true);

        string garbage;

        public XPS()
        {
            InitializeComponent();
            myPane = zedGraphControl1.GraphPane;
            Rf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Db.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Sg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Bh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Hs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Mt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Ds.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Rg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Cn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Uuh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            Uup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uuq.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uuo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            Np.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Pu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Am.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Cm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Bk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Cf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Es.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Fm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Md.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            No.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Lr.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;

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
            elec_bind = File.ReadAllLines(filePath3).Select(l => l.Split(',').ToList()).ToList();
            fab = File.ReadLines(filePath2).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);
            //MessageBox.Show(row[6][3]);
            var num = row.Count;

            for (int i = 0; i < num; i++)
            {
                dictionary.Add(row[i][1], row[i][0]);
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


            Button [] but = {H,He, Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar, K, Ca, Sc,
                                                 Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr, Rb, Sr, Y, Zr,
                                                 Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe, Cs, Ba, La, Hf, Ta,
                                                 W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn, Fr, Ra, Ac, Ce, Pr, Nd,
                                                 Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Th, Pa, U, Rf, Np, Pu, Am, Cm,
                                                 Bk, Cf, Es, Fm, Md, No, Lr};
            foreach (var item in but)
            {
                item.MouseDown += Global_Button_Click;
            }

            vset = new TextBox [] { ch1_v, ch2_v, ch3_v, ch4_v, ch5_v, ch6_v };
            vmin = new TextBox[]  { ch1_vmin, ch2_vmin, ch3_vmin, ch4_vmin, ch5_vmin, ch6_vmin };
            vmax = new TextBox[] { ch1_vmax, ch2_vmax, ch3_vmax, ch4_vmax, ch5_vmax, ch6_vmax };
            vstep = new TextBox[] { ch1_step, ch2_step, ch3_step, ch4_step, ch5_step, ch6_step };
            vramp = new TextBox[] { ch1_ramp, ch2_ramp, ch3_ramp, ch4_ramp, ch5_ramp, ch6_ramp };
            vmeas = new TextBox[] { ch1_meas, ch2_meas, ch3_meas, ch4_meas, ch5_meas, ch6_meas };
            vmeas2 = new TextBox[] { vm1, vm2, vm3, vm4, vm5, vm6 };
            reload = new Button[] { btn_reload1, btn_reload2, btn_reload3, btn_reload4, btn_reload5, btn_reload6 };
            reset = new Button[] { rs1,rs2,rs3,rs4,rs5,rs6 };
            stat = new CheckBox [] { stat1, stat2, stat3, stat4, stat5, stat6 };

            ch.Add("btn_reload1", 0);
            ch.Add("btn_reload2", 1);
            ch.Add("btn_reload3", 2);
            ch.Add("btn_reload4", 3);
            ch.Add("btn_reload5", 4);
            ch.Add("btn_reload6", 5);

            nocheins.Add("ch1_v","btn_reload1");
            nocheins.Add("ch2_v","btn_reload2");
            nocheins.Add("ch3_v","btn_reload3");
            nocheins.Add("ch4_v","btn_reload4");
            nocheins.Add("ch5_v","btn_reload5");
            nocheins.Add("ch6_v","btn_reload6");

            th.Add("stat1", 0);
            th.Add("stat2", 1);
            th.Add("stat3", 2);
            th.Add("stat4", 3);
            th.Add("stat5", 4);
            th.Add("stat6", 5);

            rh.Add("rs1", 0);
            rh.Add("rs2", 1);
            rh.Add("rs3", 2);
            rh.Add("rs4", 3);
            rh.Add("rs5", 4);
            rh.Add("rs6", 5);

            ph.Add("p1", 0);
            ph.Add("p2", 1);
            ph.Add("p3", 2);
            ph.Add("p4", 3);
            ph.Add("p5", 4);
            ph.Add("p6", 5);

            tb_pressure.ReadOnly = true;
            tb_counter.ReadOnly = true;
            for (int i = 0; i < 6; i++)
            {
                vmeas[i].ReadOnly = true;
                vmeas2[i].ReadOnly = true;
            }

            foreach (var item in reload)
            {
                item.MouseDown += Global_iseg_reload;

            }

            //foreach (var item in vset)
            //{
            //    item.KeyDown += Global_iseg_enter;
            //}

            foreach (var item in stat)
            {
                item.MouseDown += Global_iseg_terminal;
            }

            foreach (var item in reset)
            {
                item.MouseDown += Global_iseg_reset;
            }

            try
            {
                LJM.OpenS("ANY", "ANY", "ANY", ref handle3);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle2);
               // LJM.eWriteName(handle2, "DIO18", 0);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle);
                // LJM.eWriteName(handle2, "DIO18", 1);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle4);

                if (!bw_pressure.IsBusy)
                {
                    bw_pressure.RunWorkerAsync();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Can't open Labjack T7 device!");
            }  
        }

        private void elementnames_Popup(object sender, PopupEventArgs e){}

        private void tableLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void tableLayoutPanel3_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }

        private void tableLayoutPanel4_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }



        public void colorchanger(object sender, YAxis ya)
        {
            Button btn = (Button)sender;
            // var panel = sender as Control;
            //var thePanelName = btn.Name;
            string col = fab[btn.Name];
            int zeile = Convert.ToInt32(dictionary[btn.Name]) - 1;
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
                        fuerlabels.Add(btn.Name);

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
                btn.Font = new Font("Arial", 12, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.DimGray;

                int laenge = fuerlabels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (fuerlabels[y] == btn.Name)
                    {
                        fuerlabels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y+1);
                    }
                }
                zedGraphControl1.Refresh();
            }
        }


        string path2;

        int data_coutner = 0;

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (!bW_data.IsBusy)
            {
                end = 0;
                string source;
                if (Al_anode.Checked){source = "Aluminium";}
                else {source = "Magnesium";}
                myCurve = myPane.AddCurve("",
                list1, Color.Black, SymbolType.None);
                curr_time = now;
                string u = tb_safe.Text + curr_time;
                DirectoryInfo dl =  Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_XPS\", " " + tb_safe.Text + "_" + curr_time + "\\"));
                path2 = dl.FullName;
                using (var file = new StreamWriter(path2  +"data" + data_coutner + ".txt", true))
                {
                    file.WriteLine("#XPS-spectrum" + Environment.NewLine);
                    file.WriteLine("#Date/time: \t{0}", DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss"));
                    file.WriteLine("#X-ray source: \t{0}", source + Environment.NewLine);
                    //file.WriteLine("#E_b \t counts");     
                    file.WriteLine("#V_iseg \t V_Labjack \t iseg-labjack");
                }

                bW_data.RunWorkerAsync(); //run bW if it is not still running
                btn_start.Enabled = false;
                btn_can.Enabled = true;
                tb_show.Enabled = true;
                tb_safe.Enabled = false;
                Mg_anode.Enabled = false;
                Al_anode.Enabled = false;
            }

            else
	        {
                    MessageBox.Show("BW busy!");
            }                      
        }

        double spannung;
        double spannung2;
        double j = 0;
        double sc = 0;
        double diff = 0;

        private void bW_data_DoWork(object sender, DoWorkEventArgs e)
        {
            for (i = 0; i <= 1000; i++) 
            {

                LJM.eWriteName(handle2, "DIO18_EF_INDEX", 7);
                LJM.eWriteName(handle2, "DIO18_EF_ENABLE", 1);
                //LJM.eWriteName()
                LJM.eReadName(handle4, "AIN2", ref vlens);
                LJM.eReadName(handle2, "DIO18_EF_READ_A", ref value2);
                Thread.Sleep(250);
                sc = vlens;
                LJM.eReadName(handle4, "AIN2", ref vlens);
                sc += vlens;
                Thread.Sleep(250);
                LJM.eReadName(handle4, "AIN2", ref vlens);
                sc += vlens;
                Thread.Sleep(250);
                LJM.eReadName(handle4, "AIN2", ref vlens);
                sc += vlens;
                Thread.Sleep(250);
                LJM.eReadName(handle4, "AIN2", ref vlens);
                sc += vlens;
                LJM.eReadName(handle2, "DIO18_EF_READ_A_AND_RESET", ref value3);
                //  LJM.eWriteName(handle2, "DIO18_EF_READ", ref value2);
                double erg = value3 - value2;
                spannung2 = erg;
                spannung = sc/(0.1962*5);
                bW_data.ReportProgress(i / 900, spannung2.ToString("00000"));
                values.Add(spannung + "\t" + spannung2);
                //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
                using (var file = new StreamWriter(path2 + "data" + data_coutner + ".txt", true))
                {
                    file.WriteLine(spannung.ToString("0.000") + "\t" + spannung2.ToString("00000") + "\t");
                }
                Thread.Sleep(10);
                sc = 0;
                if (bW_data.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_data.ReportProgress(0);
                    break; //warum? ist wichtig! vllt um aus for-loop zu kommen
                }
            }
            //safer.safe(path,list_gauss);
            e.Result = spannung2; //stores the results of what has been done in bW
        }

        private void bW_data_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
            tb_counter.Text = Convert.ToString(e.UserState);
            // x = e.ProgressPercentage
            //list1.Add(i, Convert.ToDouble(e.UserState));
            list1.Add(spannung, spannung2);
            myCurve.AddPoint(spannung, spannung2);

            zedGraphControl1.Invalidate();
            zedGraphControl1.AxisChange();
        }

        private void bW_data_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path2 + "data" + data_coutner + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
              //  zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot" + data_coutner + ".png"));
               // safe_fig.Enabled = true;
                showdata.Enabled = true;
                fig_name.Enabled = true;
                data_coutner += 1;
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
                fig_name.Enabled = true;
                // safe_fig.Enabled = true;
                showdata.Enabled = true;
               // zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot" + data_coutner + ".png"));
                data_coutner += 1;
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
                showdata.Enabled = false;
                safe_fig.Enabled = false;
                tb_safe.Enabled = true;
                fig_name.Enabled = false;
                if (Mg_anode.Checked) {Mg_anode.Enabled = true;}
                    else { Al_anode.Enabled = true;}
                fig_name.Clear();
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                list1.Clear();
                fuerlabels.Clear();
                myPane.YAxisList.Clear();
                myPane.AddYAxis("counts");
                progressBar1.Value = 0;
                create_graph(myPane);
                                                      zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
        }


        public void labelchanger(object sender)
        {
            var panel = sender as Control;
            //var thePanelName = panel.Name;
            //https://stackoverflow.com/questions/8000957/mouseenter-mouseleave-objectname
            int zeile = Convert.ToInt32(dictionary[panel.Name]) - 1;

            System.Windows.Forms.Label[] li = {label4,label6,label8,label10,label12,label14,label16,label18,label20,label22,
                                                   label24,label26,label28,label30,label32,label34,label36,label38,label40,label42,
                                                    label44,label46,label48,label50};
            System.Windows.Forms.Label[] eb = {s1,s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16,s17,s18,s19,s20,s21,s22,
                                                   s23,s24,s25,s26,s27,s28,s29,s30,s31,s32,s33,s34,s35,s36,s37,s38,s39,s40,s41,s42,
                                                   s43,s44,s45,s46,s47,s48,s49,s50,s51,s52,s53,s54,s55,s56,s57,s58,s59};


            if (label51.Text == elementnames.GetToolTip(panel))
            {
                foreach (System.Windows.Forms.Label label in li)
                {
                    label.Text = "";
                }
                label51.Text = "";
                label52.Text = "";

                foreach (System.Windows.Forms.Label label in eb)
                {
                    label.Text = "--";
                }
            }

            else
            {
                int count = 2;
                foreach (System.Windows.Forms.Label label in li)
                {
                    label.Text = row[zeile][count];
                    count += 1;
                }
                label51.Text = elementnames.GetToolTip(panel);
                label52.Text = dictionary[panel.Name];

                count = 1;
                foreach (System.Windows.Forms.Label label in eb)
                {
                    label.Text = elec_bind[zeile][count];
                    count += 1;
                }
            }
        }

              

        private void Global_Button_Click(object sender, MouseEventArgs e)
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



        private async void safe_fig_Click(object sender, EventArgs e)
        {
            zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, fig_name.Text + ".png"));
            safe_fig.Text = "Fig. saved";
            safe_fig.BackColor = Color.LimeGreen;
            await Task.Delay(800);
            safe_fig.Text = "Save fig.";
            safe_fig.BackColor = Color.Transparent;
        }



        private void showdata_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", path2 + "data" + (data_coutner - 1) + ".txt");
        }



        private void tb_safe_TextChanged(object sender, EventArgs e)
        {
            tb_safe.BackColor = Color.LightGray;
            enable_start();
        }



        private void fig_name_TextChanged(object sender, EventArgs e)
        {
            if (fig_name.Text == "")
            {
                safe_fig.Enabled = false;
            }
            else
            {
                safe_fig.Enabled = true;
            }
        }



        private void Mg_anode_CheckedChanged(object sender, EventArgs e)
        {
            Mg_anode.BackColor = SystemColors.Control;
            Al_anode.BackColor = SystemColors.Control;
            if (Mg_anode.Checked)
            {
                Al_anode.Enabled = false;
            }

            else
            {
                Al_anode.Enabled = true;
            }
            enable_start();
        }



        private void Al_anode_CheckedChanged(object sender, EventArgs e)
        {
            Al_anode.BackColor = SystemColors.Control;
            Mg_anode.BackColor = SystemColors.Control;
            if (Al_anode.Checked)
            {
                Mg_anode.Enabled = false;
            }

            else
            {
                Mg_anode.Enabled = true;
            }
            enable_start();
        }

        private void enable_start ()
        {
            btn_start.Enabled = tb_safe.Text != string.Empty && (Al_anode.Checked || Mg_anode.Checked);
        }



//################################################################################################################################################################
            // ISEG TERMINAL


        private async void openSessionButton_Click(object sender, EventArgs e)
        {
            using (SelectResource sr = new SelectResource())
            {
                if (lastResourceString != null)
                {
                    sr.ResourceName = lastResourceString;
                }
                DialogResult result = sr.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    lastResourceString = sr.ResourceName;
                    Cursor.Current = Cursors.WaitCursor;
                    using (var rmSession = new ResourceManager())
                    {
                        try
                        {
                            iseg = (MessageBasedSession)rmSession.Open(sr.ResourceName);
                            iseg.RawIO.Write("CONF:HVMICC HV_OK\n");
                            //iseg.RawIO.Write("CONF:HVMICC?\n");
                            await Task.Delay(20);
                            garbage = iseg.RawIO.ReadString();
                            await Task.Delay(20);
                            iseg.RawIO.Write(":VOLT EMCY CLR,(@0-5)\n");
                            //iseg.RawIO.Write(":READ:VOLT:EMCY? (@0-5)\n");
                            await Task.Delay(20);
                            garbage = iseg.RawIO.ReadString();
                            await Task.Delay(20);
                            iseg.RawIO.Write("*RST\n");
                            //iseg.RawIO.Write(":READ:VOLT:EMCY? (@0-5)\n");
                            await Task.Delay(20);
                            garbage = iseg.RawIO.ReadString();
                            await Task.Delay(20);
                            iseg.RawIO.Write(String.Format(":CURR {0},(@0-5)\n", max_curr)); // Strombegrenzung auf 0.1 mA
                            //iseg.RawIO.Write(":READ:CURR? (@0-5)\n");
                            await Task.Delay(20);
                            garbage = iseg.RawIO.ReadString();
                            await Task.Delay(20);
                            //iseg.RawIO.Write(String.Format(":SYS:USER:CONF 520048\n"));
                            //await Task.Delay(20);
                            //garbage = iseg.RawIO.ReadString();
                            //await Task.Delay(20);
                            //iseg.RawIO.Write(String.Format(":SYS:USER:WRITE:VNOM {0}, (@5)\n", vnom));
                            //await Task.Delay(20);
                            //garbage = iseg.RawIO.ReadString();
                            //await Task.Delay(20);
                            //iseg.RawIO.Write(String.Format(":READ:VOLT:NOM? (@{0})\n", ch_num)); 
                            iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp));
                            await Task.Delay(20);
                            garbage = iseg.RawIO.ReadString();
                            await Task.Delay(20);
                            //SetupControlState(true);
                            bw_iseg_volts.RunWorkerAsync();
                            for (int i = 0; i < 6; i++)
                            {
                                reset[i].Enabled = true;
                                reload[i].Enabled = true;
                                stat[i].Enabled = true;
                                rs_all.Enabled = true;
                                queryButton.Enabled = true;
                                readButton.Enabled = true;
                                writeButton.Enabled = true;
                                clearButton.Enabled = true;
                                //btn_emcy.Enabled = true;
                            }
                        }
                        catch (InvalidCastException)
                        {
                            MessageBox.Show("Resource selected must be a message-based session");
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
        }

        private void closeSession_Click(object sender, System.EventArgs e)
        {
            iseg.RawIO.Write("*RST\n");
            iseg.Dispose();
        }

        private void query_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                string textToWrite = writeTextBox.Text + '\n';
                //string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                iseg.RawIO.Write(textToWrite);
                Thread.Sleep(20);
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
                Thread.Sleep(20);
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
                // WARUM KLAPPT DAS NUR BEI ZUWEIMAL LESEN?
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void write_Click(object sender, EventArgs e)
        {
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                iseg.RawIO.Write(textToWrite);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void read_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }


        //##########################################################################################################################################

        private async void Global_iseg_terminal(object sender, MouseEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c.Text == "Off")
            {
                _suspendEvent.Reset();
                await Task.Delay(10);
                iseg.RawIO.Write(String.Format(":VOLT ON,(@{0})\n", th[c.Name]));
                await Task.Delay(10);
                readTextBox.Text = iseg.RawIO.ReadString();
                await Task.Delay(10);
                _suspendEvent.Set();
                c.Text = "On";
                c.BackColor = Color.LimeGreen;
            }
            else
            {
                _suspendEvent.Reset();
                await Task.Delay(10);
                iseg.RawIO.Write(String.Format(":VOLT OFF,(@{0})\n", th[c.Name]));
                await Task.Delay(10);
                readTextBox.Text = iseg.RawIO.ReadString();
                await Task.Delay(10);
                _suspendEvent.Set();
                c.Text = "Off";
                c.BackColor = SystemColors.ControlLightLight;
            }
        }

        double ramp_vmin;
        double ramp_vmax;
        double ramp_vstep;
        int woistdierampe;
        int ramp_vramp; //das ist eine Zeit, vllt mal besser benennen IN MS!!!!!!!

        private async void Global_iseg_reload(object sender,EventArgs e)
        {
            Button b = sender as Button;

            bool Vset = Decimal.TryParse(vset[ch[b.Name]].Text.Replace(',', '.'), out decimal vset_in);
            bool Vmin = Decimal.TryParse(vmin[ch[b.Name]].Text.Replace(',', '.'), out decimal vmin_in);
            bool Vmax = Decimal.TryParse(vmax[ch[b.Name]].Text.Replace(',', '.'), out decimal vmax_in);
            bool Vramp = Decimal.TryParse(vramp[ch[b.Name]].Text.Replace(',', '.'), out decimal vramp_in);
            bool Vstep = Decimal.TryParse(vstep[ch[b.Name]].Text.Replace(',', '.'), out decimal vstep_in);
            vset[ch[b.Name]].Text = vset_in.ToString("0.000");
            if (Vset &! Vmin &! Vmax &! Vramp &! Vstep)
            {
                _suspendEvent.Reset();
                iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", vset_in.ToString("0.000"), ch[b.Name])); // 3 decimal places
                await Task.Delay(10);
                garbage = iseg.RawIO.ReadString();
                await Task.Delay(10);
                _suspendEvent.Set();
                //iseg.RawIO.Write(String.Format(":SYS:USER:WRITE:VNOM 100,(@{0})\n", ch[b.Name]));
                //iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT 0.01%/s\n"));
            }

            else if (!Vset & Vmin & Vmax & Vramp & Vstep)
            {
                woistdierampe = ch[b.Name];
                if (bw_iseg.IsBusy) // .IsBusy is true, if bW is running, otherwise false
                {
                    MessageBox.Show("Backgroundworker still busy!");
                }
                else
                {
                    ch_num = ch[b.Name];
                    ramp_vmin = Convert.ToDouble(vmin_in);
                    ramp_vmax = Convert.ToDouble(vmax_in);
                    ramp_vstep = Convert.ToDouble(vstep_in);
                    ramp_vramp = Convert.ToInt32(vramp_in);
                    iseg.RawIO.Write(String.Format(":SYS:USER:WRITE:VNOM {0} (@{1})\n", vnom, ch_num));
                    //iseg.RawIO.Write(String.Format(":READ:VOLT:NOM? (@{0})\n", ch_num)); 
                    iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp));
                    //iseg.RawIO.Write(String.Format(":READ:RAMP:VOLT? (@{0})\n", ch_num));
                    vmeas[ch_num].Enabled = true;
                    bw_iseg.RunWorkerAsync();
                }
            }

            else
            {
                MessageBox.Show("Type in    Vset    or    Vmin + Vmax + Vramp + Vstep");
            }
        }



        //private void Global_iseg_enter(object sender, KeyEventArgs e)
        //{
        //    TextBox t = sender as TextBox;
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        Global_iseg_reload(nocheins[t.Name], new EventArgs());
        //    }
        //}



        private async void Global_iseg_reset(object sender, MouseEventArgs e)
        {
            Button r = sender as Button; // 3 decimal places
            _suspendEvent.Reset();
            if (bw_iseg.IsBusy & woistdierampe == rh[r.Name]) // .IsBusy is true, if bW is running, otherwise false
            {
                bw_iseg.CancelAsync(); //cancels the background operation and sets CancellationPendiung to true!
            }
            iseg.RawIO.Write(String.Format(":VOLT OFF,(@{0})\n", rh[r.Name]));
            await Task.Delay(10);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(10);
            iseg.RawIO.Write(String.Format(":VOLT 0.000,(@{0})\n", rh[r.Name]));
            await Task.Delay(10);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(10);
            _suspendEvent.Set();
            vset[rh[r.Name]].Text = "";
            vmin[rh[r.Name]].Text = "";
            vmax[rh[r.Name]].Text = "";
            vstep[rh[r.Name]].Text = "";
            vramp[rh[r.Name]].Text = "";
            vmeas[rh[r.Name]].Text = "";
            stat[rh[r.Name]].Text = "Off";
            stat[rh[r.Name]].BackColor = SystemColors.ControlLightLight;
        }

        private async void rs_all_Click(object sender, EventArgs e)
        {
            _suspendEvent.Reset();
            if (bw_iseg.IsBusy) // .IsBusy is true, if bW is running, otherwise false
            {
                bw_iseg.CancelAsync(); //cancels the background operation and sets CancellationPendiung to true!
            }
            iseg.RawIO.Write(String.Format("*RST\n"));
            await Task.Delay(10);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(10);
            _suspendEvent.Set();
            for (int i = 0; i <= 5; i++)
            {
                vset[i].Text = "";
                vmin[i].Text = "";
                vmax[i].Text = "";
                vstep[i].Text = "";
                vramp[i].Text = "";
                vmeas[i].Text = "";
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
        }


        private void bw_iseg_DoWork(object sender, DoWorkEventArgs e)
        {

            //bW_data.ReportProgress(100 * i / num_gauss, end);
            //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
            iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", ramp_vmin,ch_num));
            //iseg.RawIO.Write(String.Format(":MEAS:VOLT? (@{0})\n", ch_num));
            Thread.Sleep(sleep_vset);
            double v = ramp_vmin;
            //hier kommt noch erste messung mit hin
            while (v <= (ramp_vmax-ramp_vstep))
            {
                if (bw_iseg.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    break; //warum? ist wichtig! vllt um aus for-loop zu kommen
                }
                v += ramp_vstep;
                iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", v.ToString("0.000"), ch_num));
                //iseg.RawIO.Write(String.Format(":MEAS:VOLT? (@{0})\n", ch_num));
                //Thread.Sleep(100);
                //bw_iseg.ReportProgress(0, iseg.RawIO.ReadString());
                bw_iseg.ReportProgress(0, (v+ramp_vstep).ToString("0.000"));
                Thread.Sleep(ramp_vramp);
            }
            //evtl. am ende wieder auf ground ziehen!
            e.Result = end; //stores the results of what has been done in bW
        }


        private void bw_iseg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            //lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
            //vmeas[ch_num].Text = Convert.ToString(e.UserState);
        }


        private void bw_iseg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                vmeas[ch_num].Text = "";
                vmeas[ch_num].Enabled = false;
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                MessageBox.Show("Error during asynchronous operation");
            }

            else
            {
                MessageBox.Show("Done!");
            }
        }




        private void bw_pressure_DoWork(object sender, DoWorkEventArgs e)
        {
            double pressure;
            while (!bw_pressure.CancellationPending)
            {
                LJM.eReadName(handle, AIN0, ref value);
                Thread.Sleep(20);
                pressure = Math.Pow(10,((Convert.ToDouble(value)-7.75))/0.75);
                bw_pressure.ReportProgress(Convert.ToInt32(value2), pressure.ToString("0.00E0"));
                Thread.Sleep(1000);
            }
        }

        private void bw_pressure_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tb_pressure.Text = Convert.ToString(e.UserState);
            //tb_counter.Text = e.ProgressPercentage.ToString();
        }

        private void bw_pressure_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                tb_pressure.Text = "Stop!";
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_show.Text = e.Error.Message;
            }

            else
            {

            }
        }







       

        string spannungen;

        private void bw_iseg_volts_DoWork(object sender, DoWorkEventArgs e)
        {
            double s = 0;
            for (int counter = 0;  counter < 6; counter++)
            {
                if (bw_iseg_volts.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    break;
                }
                iseg.RawIO.Write(String.Format(":MEAS:VOLT? (@{0})\n", counter));
                Thread.Sleep(20);
                spannungen = iseg.RawIO.ReadString();
                Thread.Sleep(10);
                spannungen = iseg.RawIO.ReadString();
                Thread.Sleep(10);
                _suspendEvent.WaitOne(Timeout.Infinite);
                try
                {
                    s = Double.Parse(spannungen.Replace("V\r\n", ""), System.Globalization.NumberStyles.Float);
                }
                catch (Exception)
                {
                    s = 0;
                }
                bw_iseg_volts.ReportProgress(counter, s.ToString("0.000"));
                Thread.Sleep(2);
                //_suspendEvent.WaitOne(Timeout.Infinite);
                Thread.Sleep(2);
                Thread.Sleep(2);
                _suspendEvent.WaitOne(Timeout.Infinite);
                if (counter == 5)
                {
                    counter = -1;
                    Thread.Sleep(20);
                }
            }
        }

        private void bw_iseg_volts_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vmeas[e.ProgressPercentage].Text = Convert.ToString(e.UserState);
            vmeas2[e.ProgressPercentage].Text = Convert.ToString(e.UserState);
            int percentage = e.ProgressPercentage;
        }

        private void bw_iseg_volts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }















        private void cb_pressure_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_pressure.Checked)
            {
                bw_pressure.RunWorkerAsync();
                //cb_pressure.Text = "Hide";
                tb_pressure.Enabled = true;
            }

            else
            {
                bw_pressure.CancelAsync();
                //cb_pressure.Text = "Show";
                tb_pressure.Text = "";
                tb_pressure.Enabled = false;
            }
        }

        private async void btn_emcy_Click(object sender, EventArgs e)
        {
            _suspendEvent.Reset();
            await Task.Delay(10);
            iseg.RawIO.Write(":VOLT EMCY OFF, (@0-5)\n");
            await Task.Delay(10);
            garbage = iseg.RawIO.ReadString();
            bw_iseg.CancelAsync();
            _suspendEvent.Set();

            for (int i = 0; i <6 ; i++)
            {
                stat[i].Text = "Off";
                stat[i].Enabled = false;
                reload[i].Enabled = false;
                reset[i].Enabled = false;
                stat[i].BackColor = SystemColors.ControlLightLight;
            }

            (sender as Button).Enabled = false;
        }


        private void ch6_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload6, new EventArgs());
            }
        }
        private void ch5_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload5, new EventArgs());
            }
        }
        private void ch4_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload4, new EventArgs());
            }
        }
        private void ch3_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload3, new EventArgs());
            }
        }
        private void ch2_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload2, new EventArgs());
            }
        }
        private void ch1_v_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Global_iseg_reload(btn_reload1, new EventArgs());
            }
        }

        private void tb_counter_ms_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_start_counter_Click(sender, new EventArgs());
                tb_counter_ms.Text = ct.ToString();
            }
        }


        int ct = 2000;
        private async void btn_start_counter_Click(object sender, EventArgs e)
        {
            //while (true)
            //{
                try
                {
                    //ct = int.Parse(tb_counter_ms.Text);
                    tb_counter_ms.Text = ct.ToString();
                    LJM.eWriteName(handle2, "DIO18_EF_INDEX", 7);
                    //LJM.eWriteName(handle2, "DIO18_EF_OPTIONS", 1);
                    //LJM.eWriteName(handle2, "DIO18_EF_CONFIG_A", 2);
                    //LJM.eWriteName(handle2, "DIO18_EF_CONFIG_B", 100);
                    //LJM.eWriteName(handle2, "FIO7", 0);
                    LJM.eWriteName(handle2, "DIO18_EF_ENABLE", 1);
                    //LJM.eWriteName()
                    LJM.eReadName(handle2, "DIO18_EF_READ_A", ref value2);
                    await Task.Delay(ct);
                    LJM.eReadName(handle2, "DIO18_EF_READ_A_AND_RESET", ref value3);
                    //  LJM.eWriteName(handle2, "DIO18_EF_READ", ref value2);
                    double erg = value3 - value2;
                    tb_counter.Text = erg.ToString();
                    //LJM.eWriteName(handle2, "DIO18_EF_ENABLE", 0);
                }
                catch (Exception)
                {
                    MessageBox.Show("Type in Integer!");
                }
            //}
        }


        private async void XPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bw_iseg_volts.CancelAsync();

                if (bw_iseg.IsBusy)
                {
                    bw_iseg.CancelAsync();
                }
                try
                {
                    iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp));
                    await Task.Delay(20);
                    garbage = iseg.RawIO.ReadString();
                    await Task.Delay(20);
                    iseg.RawIO.Write("*RST\n");
                    garbage = iseg.RawIO.ReadString();
                    iseg.RawIO.Write(":VOLT EMCY OFF, (@0-5)\n");
                    garbage = iseg.RawIO.ReadString();
                    await Task.Delay(50);
                    iseg.Dispose();
                }
                catch (Exception)
                {
                }

                if (bw_pressure.IsBusy)
                {
                    bw_pressure.CancelAsync();
                }
                LJM.CloseAll();
            }
            catch (Exception)
            {
                MessageBox.Show("Problems with closing Iseg or Labjack T7 device!");
            }


        }
       
        private void btn_load_Click(object sender, EventArgs e)
        {
            double k = ra / ri - ri / ra;
            bool V_pass = double.TryParse(tb_v_pass.Text.Replace(',', '.'), out double v_pass);
            bool V_vor = double.TryParse(tb_v_vor.Text.Replace(',', '.'), out double v_vor);
            bool V_bind = double.TryParse(tb_v_bind.Text.Replace(',', '.'), out double v_bind);

            if (V_pass & V_vor & V_bind)
            {
                v_ana_min = v_vor - V_photon + W_aus + v_pass;
                v_ana_max = v_vor + W_aus + v_pass;
                v_ana_bind = v_ana_min + v_bind;
                v_cem = v_ana_bind + delta_v_channeltron;
                delta_v = v_pass * k;
                //v_hem_in = v_ana_bind + delta_v / 2; // 50:50 spannungsteiler
                //v_hem_out = v_ana_bind - delta_v / 2; // 50:50 spannungsteiler
                v_hem_in = v_ana_bind + delta_v * 0.6;
                v_hem_out = v_ana_bind - delta_v*0.4;
                tb_ana_min.Text = v_ana_min.ToString("0.000");
                tb_ana_max.Text = v_ana_max.ToString("0.000");
                ch4_v.Text = v_ana_bind.ToString("0.000");
                ch5_v.Text = v_cem.ToString("0.000");
                ch1_v.Text = v_hem_in.ToString("0.000");
                ch2_v.Text = v_hem_out.ToString("0.000");
            }
            else
            {
                MessageBox.Show("Type in V_pass, V_vor and E_bind");
            }
        }

        private async void bt_rampe_Click(object sender, EventArgs e)
        {
            //iseg.RawIO.Write(String.Format(":READ:VOLT:NOM? (@{0})\n", ch_num)); 
            string wert = tb_rampe.Text;
            string rampe_bis = tb_rampe_bis.Text;
            _suspendEvent.Reset();
            iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", wert));
            await Task.Delay(20);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(20);
            iseg.RawIO.Write(String.Format(":VOLT {0},(@2)\n", rampe_bis));
            await Task.Delay(20);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(20);
            _suspendEvent.Set();
        }

        private async void btn_rampe_Click(object sender, EventArgs e)
        {
            _suspendEvent.Reset();
            iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT 40.000%/s\n"));
            await Task.Delay(20);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(20);
            _suspendEvent.Set();
        }
    }
}




//bugs:
// - nach clear führt das abwählen von elementen zzu einem error (da ebtl. noch in liste gespeichert)
// - close iseg HV


    // TESTEN OB ISEG MANUELL NE RAMPE FAHREN KANN
//j = i;
//spannung = -45.000 + j;
////end += i;
//_suspendEvent.Reset();
//iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", spannung.ToString("0.000"), 5)); // 3 decimal places
//Thread.Sleep(10000);
//garbage = iseg.RawIO.ReadString();
//Thread.Sleep(50);
//_suspendEvent.Set();
//LJM.eReadName(handle3, "AIN9", ref v_labjack);
//Thread.Sleep(50);
//sc += v_labjack;
//LJM.eReadName(handle3, "AIN9", ref v_labjack);
//Thread.Sleep(50);
//sc += v_labjack;
//LJM.eReadName(handle3, "AIN9", ref v_labjack);
//Thread.Sleep(50);
//sc += v_labjack;
//spannung2 = sc / (3*0.2040);
//bW_data.ReportProgress(i/900, spannung2.ToString("0.000"));
//Thread.Sleep(10);
//sc = 0;
//diff = spannung - spannung2;
//values.Add(spannung + "\t" + spannung2);
////safer.safe_line(path + @"\gauss", end.ToString("000000000"));
//using (var file = new StreamWriter(path2 +  "data" + data_coutner + ".txt", true))
//{
//    file.WriteLine(spannung.ToString("0.000") + "\t" + spannung2.ToString("0.000") + "\t" + diff.ToString("0.000"));
//}
//Thread.Sleep(10);
//diff = 0;


    // COUNTER IN DAUERSCHLEIFE
//                {
//                spannung = i*10;
//                //end += i;
//                //ct = int.Parse(tb_counter_ms.Text);
//                //tb_counter_ms.Text = ct.ToString();
//                LJM.eWriteName(handle2, "DIO18_EF_INDEX", 7);
//                //LJM.eWriteName(handle2, "DIO18_EF_OPTIONS", 1);
//                //LJM.eWriteName(handle2, "DIO18_EF_CONFIG_A", 2);
//                //LJM.eWriteName(handle2, "DIO18_EF_CONFIG_B", 100);
//                //LJM.eWriteName(handle2, "FIO7", 0);
//                LJM.eWriteName(handle2, "DIO18_EF_ENABLE", 1);
//                //LJM.eWriteName()
//                LJM.eReadName(handle2, "DIO18_EF_READ_A", ref value2);
//                Thread.Sleep(1000);
//                LJM.eReadName(handle2, "DIO18_EF_READ_A_AND_RESET", ref value3);
//                //  LJM.eWriteName(handle2, "DIO18_EF_READ", ref value2);
//                double erg = value3 - value2;
//spannung2 = erg;
//                bW_data.ReportProgress(i / 900, spannung2.ToString("00000"));
//                values.Add(spannung + "\t" + spannung2);
//                //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
//                using (var file = new StreamWriter(path2 + "data" + data_coutner + ".txt", true))