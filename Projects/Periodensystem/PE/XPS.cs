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
        // General settings
        double V_photon = 21.21;            // Energiey HeI-line
        double W_aus = 4.5;                 // workfunction spectrometer
        double ri = 106;                    // Radius inner hemisphere in mm
        double ra = 112;                    // Radius outer hemisphere in mm
        double delta_channeltron = 3000;    // voltage drop over channeltron in V
        double deviation = 0.08;            // maximim voltage deviation (in V) at the beginng of the voltage ramp
        double perc_ramp = 40.000;          // voltage ramp in percent of 4000 V/s (4000 = Vnominal)
        string pressure_pin = "AIN0";       // Analog Input Pin of Ionivac


        // Labjack stuff
        int handle_pressure = 0;            // Labjack threads
        int handle_v_hemi = 0;
        int handle_v_hemo = 0;
        int handle_v_analyser = 0;
        int handle_v_lens = 0;
        int handle_count = 0;
        double ionivac_v_out = 0;           // Voltage of Ionivac output measured with Labjack device
        double cnt_before = 0;              // coutner reading befor and after delay
        double cnt_after = 0;
        double intcounter = 0;
        double LJ_analyser = 0;             // Labjack input (voltage devider)
        double LJ_hemi = 0;
        double LJ_hemo = 0;
        double LJ_lens = 0;
        double LJ_analyser2 = 0;            // voltages to be displayed
        double LJ_hemi2 = 0;
        double LJ_hemo2 = 0;
        double LJ_lens2 = 0;

        // Voltage setting stuff
        double vpass;
        double vbias;
        double vstepsize;
        double vLens;
        double tcount;                      // counting-time
        double slope;                       // voltage slope (multiples of 4 mV/s)
        double slop;                        // multiples of 4 mV/s
        double v_analyser_min;              // initial and final voltages
        double v_channeltron_out_min;
        double v_hemo_min;
        double v_hemi_min;
        double v_analyser_max;
        double v_channeltron_out_max;
        double v_hemo_max;
        double v_hemi_max;
        double v_hemi_min_korr;
        double v_hemo_min_korr;

        string garbage;                     // need to read back each comment send to Iseg device
        string path_logfile;
        string path_bindungenergies = Path.GetFullPath("Bindungsenergien.csv");
        string path_colors = Path.GetFullPath("colors2.csv");
        string path_electronconfig = Path.GetFullPath("electronconfiguration.csv");
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string now = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string curr_time;
        private string lastResourceString = null;
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};
        List<List<string>> row = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
        List<string> display_labels = new List<string>();
        PointPairList values_to_plot = new PointPairList();
        Dictionary<string, string> binding_energies_dict = new Dictionary<string, string>();
        Dictionary<string, string> color_dict = new Dictionary<string, string>();
        Dictionary<string, int> ch = new Dictionary<string, int>();
        GraphPane myPane;
        LineItem myCurve;
        TextObj pane_labs;
        YAxis yaxis = new YAxis();
        TextBox[] vset;
        TextBox[] vmeas;
        TextBox[] vmeas2;
        Button[] reload;
        Button[] reset;
        CheckBox[] stat;
        ManualResetEvent _suspend_background_measurement = new ManualResetEvent(true);
        private MessageBasedSession iseg;           // Iseg-HV session
        bool start_ok = false;





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


        private void Form1_Load(object sender, EventArgs e)
        {
            // read files with bindung energies, colors and electron configuration
            row = File.ReadAllLines(path_bindungenergies).Select(l => l.Split(',').ToList()).ToList();
            elec_bind = File.ReadAllLines(path_electronconfig).Select(l => l.Split(',').ToList()).ToList();
            color_dict = File.ReadLines(path_colors).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);

            for (int i = 0; i < row.Count; i++)
            {
                binding_energies_dict.Add(row[i][1], row[i][0]);
            }
            create_graph(myPane);

            try
            {
                if (!Directory.Exists(path + @"\Logfiles_PES"))
                {
                    Directory.CreateDirectory(path + @"\Logfiles_PES");
                }
            }
            catch
            {
                MessageBox.Show("Can't create Folder 'Logfile_PES' on Desktop");
            }

            try
            {   // Open Labjack sessions
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_count);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_hemi);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_hemo);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_analyser);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_lens);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_pressure);

                if (!bw_pressure.IsBusy)
                {
                    bw_pressure.RunWorkerAsync();
                }
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 device!", "Info", 500);
            }

            Button [] but = {H ,He, Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar, K, Ca, Sc,
                             Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr, Rb, Sr, Y, Zr,
                             Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe, Cs, Ba, La, Hf, Ta,
                             W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn, Fr, Ra, Ac, Ce, Pr, Nd,
                             Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Th, Pa, U, Rf, Np, Pu, Am, Cm,
                             Bk, Cf, Es, Fm, Md, No, Lr};

            vset = new TextBox[] { ch1_v, ch2_v, ch3_v, ch4_v, ch5_v, ch6_v };
            vmeas = new TextBox[] { ch1_meas, ch2_meas, ch3_meas, ch4_meas, ch5_meas, ch6_meas };
            vmeas2 = new TextBox[] { vm1, vm2, vm3, vm4, vm5 };
            reload = new Button[] { btn_reload1, btn_reload2, btn_reload3, btn_reload4, btn_reload5, btn_reload6 };
            reset = new Button[] { rs1, rs2, rs3, rs4, rs5, rs6 };
            stat = new CheckBox[] { stat1, stat2, stat3, stat4, stat5, stat6 };

            foreach (var item in but)
            {
                item.MouseDown += Global_Button_Click;
            }
            foreach (var item in stat)
            {
                item.MouseDown += Global_iseg_terminal;
            }

            foreach (var item in reset)
            {
                item.MouseDown += Global_iseg_reset;
            }

            foreach (var item in reload)
            {
                item.MouseDown += Global_iseg_reload;

            }


            ch.Add("btn_reload1", 0);
            ch.Add("btn_reload2", 1);
            ch.Add("btn_reload3", 2);
            ch.Add("btn_reload4", 3);
            ch.Add("btn_reload5", 4);
            ch.Add("btn_reload6", 5);
            ch.Add("stat1", 0);
            ch.Add("stat2", 1);
            ch.Add("stat3", 2);
            ch.Add("stat4", 3);
            ch.Add("stat5", 4);
            ch.Add("stat6", 5);
            ch.Add("rs1", 0);
            ch.Add("rs2", 1);
            ch.Add("rs3", 2);
            ch.Add("rs4", 3);
            ch.Add("rs5", 4);
            ch.Add("rs6", 5);

            cb_pass.SelectedIndex = 4;
            cb_bias.SelectedIndex = 4;
            cb_counttime.SelectedIndex = 0;
            cb_stepwidth.SelectedIndex = 3;
            cb_v_lens.SelectedIndex = 2;

            k = ra / ri - ri / ra;
        }


        private void create_graph(GraphPane myPane)
        {
            myPane.Title.Text = "UPS/XPS";
            myPane.Title.FontSpec.Size = 13;
            myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "Kinetic energy (+ offset) [eV]";
            myPane.XAxis.Title.FontSpec.Size = 11;
            myPane.YAxis.Title.Text = "counts";
            myPane.YAxis.Title.FontSpec.Size = 11;
            myPane.Fill.Color = Color.LightGray;
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


        public void colorchanger(object sender, YAxis ya)       // Function for interactive periodic table
        {
            Button btn = (Button)sender;
            // var panel = sender as Control;
            //var thePanelName = btn.Name;
            string col = color_dict[btn.Name];
            int current_line = Convert.ToInt32(binding_energies_dict[btn.Name]) - 1;
            float value;

            if (btn.ForeColor == Color.DimGray)
            {
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;

                for (int i = 2; i <= 25; i++)
                {
                    bool result = float.TryParse(row[current_line][i], out value);

                    if (result)
                    {
                        pane_labs = new TextObj((row[current_line][i] + "\n" + row[current_line][1] + " " + scores[i]), float.Parse(row[current_line][i], CultureInfo.InvariantCulture), -0.05,
                            CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        pane_labs.FontSpec.Size = 10f;
                        pane_labs.FontSpec.Angle = 40;
                        pane_labs.FontSpec.Fill.Color = Color.Transparent;
                        pane_labs.FontSpec.FontColor = Color.DimGray;
                        pane_labs.FontSpec.Border.IsVisible = false;
                        pane_labs.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(pane_labs);
                        display_labels.Add(btn.Name);

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
                        ya.Cross = Double.Parse(row[current_line][i], CultureInfo.InvariantCulture);
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

                int laenge = display_labels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (display_labels[y] == btn.Name)
                    {
                        display_labels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y+1);
                    }
                }
                zedGraphControl1.Refresh();
            }
        }

        public void labelchanger(object sender)         // Function for electronic configuration plot
        {
            var panel = sender as Control;
            //https://stackoverflow.com/questions/8000957/mouseenter-mouseleave-objectname
            int current_line = Convert.ToInt32(binding_energies_dict[panel.Name]) - 1;

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
                    label.Text = row[current_line][count];
                    count += 1;
                }
                label51.Text = elementnames.GetToolTip(panel);
                label52.Text = binding_energies_dict[panel.Name];

                count = 1;
                foreach (System.Windows.Forms.Label label in eb)
                {
                    label.Text = elec_bind[current_line][count];
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


        private async void await_time(int delay)
        {
            await Task.Delay(delay);
            garbage = iseg.RawIO.ReadString();
            await Task.Delay(delay);
        }

        private void sleep_time(int delay)
        {
            Thread.Sleep(delay);
            garbage = iseg.RawIO.ReadString();
            Thread.Sleep(delay);
        }


        //################################################################################################################################################################
        // Open Iseg Device and Iseg Terminal


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
                            start_ok = true;
                            iseg = (MessageBasedSession)rmSession.Open(sr.ResourceName);
                            iseg.RawIO.Write("CONF:HVMICC HV_OK\n");
                            await_time(20);
                            iseg.RawIO.Write(":VOLT EMCY CLR,(@0-5)\n");
                            await_time(20);
                            iseg.RawIO.Write("*RST\n");
                            await_time(20);
                            iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp));
                            await_time(20);
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
                                btn_emcy.Enabled = true;
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
            try
            {
                iseg.RawIO.Write("*RST\n");
                iseg.Dispose();
            }
            catch (Exception)
            {
                MessageBox.Show("Can not close Iseg-Hv");
            }
        }

        private async void query_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                string textToWrite = writeTextBox.Text + '\n';
                //string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                iseg.RawIO.Write(textToWrite);
                await Task.Delay(20);
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
                await Task.Delay(20);
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
            _suspend_background_measurement.Set();
        }

        private void write_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                iseg.RawIO.Write(textToWrite);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            _suspend_background_measurement.Set();
        }

        private void read_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
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
            _suspend_background_measurement.Set();
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


        //###########################################################################################################################################
        // Backgroundworker for taking XPS/UPS spectra


        private async void btn_start_Click(object sender, EventArgs e)
        {
            if (!bW_data.IsBusy)
            {
                //if (Al_anode.Checked){source = "Aluminium";}
                //else {source = "Magnesium";}
                myCurve = myPane.AddCurve("",
                values_to_plot, Color.Black, SymbolType.None);
                curr_time = now;
                string u = tb_safe.Text + curr_time;
                DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + curr_time + "_" + tb_safe.Text + "\\"));
                path_logfile = dl.FullName;
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine("#UPS-spectrum" + Environment.NewLine);
                    file.WriteLine("#Date/time: \t{0}", DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss"));
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("#AK pressure: \t{0} \t{1}", tb_pressure.Text, "mbar");
                    file.WriteLine("#Pass energy: \t{0} \t{1}", vpass.ToString("0.0"), "eV");
                    file.WriteLine("#Voltage bias: \t{0} \t{1}", vbias.ToString("0.0"), "V");
                    file.WriteLine("#Voltage lenses: \t{0} \t{1}", vLens.ToString("0.0"), "V");
                    file.WriteLine("#Step width: \t{0} \t{1}", vstepsize.ToString("0.0"), "meV");
                    file.WriteLine("#Counttime: \t{0} \t{1}", tcount.ToString("0.0"), "ms");
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("#Slope: \t{0} \t{1}", (slope*1000).ToString("0.0"), "mV/s");
                    file.WriteLine("#Counttime: \t{0} \t{1}", tcount.ToString("0.0"), "ms");
                    //file.WriteLine("#X-ray source: \t{0}", source + Environment.NewLine);
                    //file.WriteLine("#E_b \t counts");    
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("#E_kin \t Counts \t V_ana. \t V_hemi \t V_hemo \t V_lens");
                    file.WriteLine("" + Environment.NewLine);
                }

                v_analyser_min = vpass - V_photon + vbias - 15;          //hier sollte kein elektron mehr im channeltron ankommen
                //v_analyser_min = vpass - V_photon + vbias - 5;
                v_hemo_min = v_analyser_min - (vpass * k * 0.4);  //äußere hemispährenspannung aus passenergie nach spannugnsteiler (3.885M, 5.58M,269.5k))
                v_hemi_min = v_hemo_min + vpass * k;               //liegt entsprechung die Spannungdifferenz drüber

                // v_analyser_max = vpass +vbias;          //hier sollte auch das langsamste Elektron ankommen
                v_analyser_max = v_analyser_min + V_photon + 5;
                v_hemo_max = v_analyser_max - (vpass * k * 0.4);  //äußere hemispährenspannung aus passenergie nach spannugnsteiler (3.885M, 5.58M,269.5k))
                v_hemi_max = v_analyser_max + vpass * k;

                v_channeltron_out_min = v_analyser_min + 3000;
                v_channeltron_out_max = v_analyser_max + 3000;


                _suspend_background_measurement.Reset();
                iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT 10.000%/s\n"));     // Rampe auf 400 V/s
                await_time(20);
                iseg.RawIO.Write(String.Format(":VOLT {0},(@0)\n", v_hemi_min.ToString("0.000")));
                await_time(20);
                iseg.RawIO.Write(String.Format(":VOLT {0},(@1)\n", v_hemo_min.ToString("0.000")));
                await_time(20);
                iseg.RawIO.Write(String.Format(":VOLT {0},(@2)\n", vLens.ToString("0.000")));
                //iseg.RawIO.Write(String.Format(":VOLT {0},(@2)\n", (v_hemi_min+5).ToString("0.000")));
                await_time(20);
                iseg.RawIO.Write(String.Format(":VOLT {0},(@4)\n", v_channeltron_out_min.ToString("0.000")));
                await_time(20);
                iseg.RawIO.Write(String.Format(":VOLT ON,(@0-5)\n"));
                await_time(20);


                for (int i = 0; i < 12; i++)             //warten bis sich Spannungen gesetzt haben
                {
                    LJM.eReadName(handle_v_analyser, "AIN3", ref LJ_analyser);
                    LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                    LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);
                    LJM.eReadName(handle_v_lens, "AIN6", ref LJ_lens);
                    //LJ_analyser2 = LJ_analyser / 0.1956;
                    //LJ_analyser2 = LJ_analyser / 0.204;
                    LJ_hemi2 = LJ_hemi / 0.1962;
                    LJ_hemo2 = LJ_hemo / 0.1960;
                    LJ_analyser2 = LJ_hemo2 + (vpass * k * 0.3991);
                    LJ_lens2 = LJ_lens / 0.1962;
                    vm1.Text = LJ_hemi2.ToString("0.000");
                    vm2.Text = LJ_hemo2.ToString("0.000");
                    vm3.Text = LJ_lens2.ToString("0.000");
                    vm4.Text = LJ_analyser2.ToString("0.000");
                    vm5.Text = v_channeltron_out_max.ToString("0.000");

                    await Task.Delay(1000);

                }

                while (Math.Abs(LJ_hemi2 - v_hemi_min) > deviation || Math.Abs(LJ_hemo2 - v_hemo_min) > deviation)
                {
                    if (Math.Abs(LJ_hemi2 - v_hemi_min) > deviation)
                    {
                        v_hemi_min_korr = v_hemi_min - (LJ_hemi2 - v_hemi_min);
                        iseg.RawIO.Write(String.Format(":VOLT {0},(@0)\n", v_hemi_min_korr.ToString("0.000")));
                        await Task.Delay(10);
                        garbage = iseg.RawIO.ReadString();
                        await Task.Delay(10);
                    }

                    if (Math.Abs(LJ_hemo2 - v_hemo_min) > deviation)
                    {
                        v_hemo_min_korr = v_hemo_min - (LJ_hemo2 - v_hemo_min);
                        iseg.RawIO.Write(String.Format(":VOLT {0},(@1)\n", v_hemo_min_korr.ToString("0.000")));
                        await Task.Delay(10);
                        garbage = iseg.RawIO.ReadString();
                        await Task.Delay(10);
                    }


                    await Task.Delay(8000);

                    LJM.eReadName(handle_v_analyser, "AIN3", ref LJ_analyser);
                    LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                    LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);

                    LJ_hemi2 = LJ_hemi / 0.1962;
                    LJ_hemo2 = LJ_hemo / 0.1960;
                    LJ_analyser2 = LJ_hemo2 + (vpass * k * 0.3991);
                    vm1.Text = LJ_hemi2.ToString("0.000");
                    vm2.Text = LJ_hemo2.ToString("0.000");
                    vm3.Text = LJ_lens2.ToString("0.000");
                    vm4.Text = LJ_analyser2.ToString("0.000");
                    //_suspend_background_measurement.Set();
                }

                bW_data.RunWorkerAsync(); //run bW if it is not still running
                btn_start.Enabled = false;
                btn_can.Enabled = true;
                tb_show.Enabled = true;
                tb_safe.Enabled = false;
                cb_cnt_inf.Enabled = false;
                btn_start_counter.Enabled = false;
            }

            else
            {
                MessageBox.Show("BW busy!");
            }
        }

        double sc = 0;
        double oldcounter = 0;
        double k;
        double E_pass;
        double E_kin;
        double cnt;

        private void bW_data_DoWork(object sender, DoWorkEventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
            iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", slop * 0.0001));
            //iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", 0.0004));
            sleep_time(20);
            iseg.RawIO.Write(String.Format(":VOLT {0},(@0)\n", v_hemi_max.ToString("0.000")));
            sleep_time(20);
            iseg.RawIO.Write(String.Format(":VOLT {0},(@1)\n", v_hemo_max.ToString("0.000")));
            sleep_time(20);
            //iseg.RawIO.Write(String.Format(":VOLT {0},(@5)\n", v_analyser_max2.ToString("0.000")));
            //Thread.Sleep(20);
            //garbage = iseg.RawIO.ReadString();
            //Thread.Sleep(20);
            iseg.RawIO.Write(String.Format(":VOLT {0},(@4)\n", v_channeltron_out_max.ToString("0.000")));
            sleep_time(20);

            LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
            while (true)
            {
                sw.Start();
                LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                LJM.eReadName(handle_v_analyser, "AIN3", ref LJ_analyser);
                LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);
                LJM.eReadName(handle_v_lens, "AIN6", ref LJ_lens);
                sc = (intcounter - oldcounter) * 1000 / tcount;       //auf sekunde normieren
                                                                      //LJ_analyser2 = LJ_analyser / 0.1956;
                LJ_hemi2 = LJ_hemi / 0.1962;
                LJ_hemo2 = LJ_hemo / 0.1960;
                LJ_analyser2 = LJ_hemo2 + (vpass * k * 0.4);
                LJ_lens2 = LJ_lens / 0.1962;

                E_pass = (LJ_hemi2 - LJ_hemo2) / k;
                //E_kin = E_pass - LJ_analyser2 - vbias;          // denn für detektierte e- gilt: 0 = Vbias + Ekin^0 + V_ana^0 - V_pass (^0: bezogen auf 0V)
                E_kin = vbias - LJ_analyser2 + E_pass;          // ohne berücksichtigung de raustrittsarbeit
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(E_kin.ToString("0.000") + "\t" + sc.ToString("00000") + "\t" + LJ_analyser2.ToString("0.000") + "\t"
                        + LJ_hemi2.ToString("0.000") + "\t" + LJ_hemo2.ToString("0.000") + "\t" + LJ_lens2.ToString("0.000")  +"\t" + (LJ_hemo2 - LJ_hemi2).ToString("0.000") + "\t"
                        + ((LJ_hemo2 - LJ_analyser2) / (LJ_hemi2 - LJ_hemo2)).ToString("0.000") + "\t");
                }


                bW_data.ReportProgress(0, sc.ToString("00000"));


                oldcounter = intcounter;

                if (bW_data.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_data.ReportProgress(0);
                    LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
                    break; //warum? ist wichtig! vllt um aus for-loop zu kommen
                }

                while (sw.ElapsedMilliseconds < Convert.ToInt16(tcount))
                {
                    Thread.Sleep(1);
                }
                sw.Reset();
            }
            e.Result = sc; //stores the results of what has been done in bW
            LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
        }

        private void bW_data_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            vm1.Text = LJ_hemi2.ToString("0.000");
            vm2.Text = LJ_hemo2.ToString("0.000");
            vm3.Text = LJ_lens2.ToString("0.000");
            vm4.Text = LJ_analyser2.ToString("0.000");
            vm5.Text = v_channeltron_out_max.ToString("0.000");
            tb_counter.Text = Convert.ToString(e.UserState);
            values_to_plot.Add(E_kin, sc);
            myCurve.AddPoint(E_kin, sc);

            zedGraphControl1.Invalidate();
            zedGraphControl1.AxisChange();
        }

        private void bW_data_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT 10.000%/s\n"));
                sleep_time(20);
                iseg.RawIO.Write(String.Format(":VOLT OFF,(@0-5)\n"));
                sleep_time(20);

                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path_logfile + "data"  + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
                //  zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, "plot" + data_coutner + ".png"));
                // safe_fig.Enabled = true;
                showdata.Enabled = true;
                fig_name.Enabled = true;
                sc = 0;
                intcounter = 0;
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
                // zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, "plot" + data_coutner + ".png"));
            }
            cb_cnt_inf.Enabled = true;
            btn_start_counter.Enabled = true;
            _suspend_background_measurement.Set();
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
                //if (Mg_anode.Checked) {Mg_anode.Enabled = true;}
                //    else { Al_anode.Enabled = true;}
                fig_name.Clear();
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                values_to_plot.Clear();
                display_labels.Clear();
                myPane.YAxisList.Clear();
                myPane.AddYAxis("counts");
                progressBar1.Value = 0;
                create_graph(myPane);
                zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
        }        


        private async void safe_fig_Click(object sender, EventArgs e)
        {
            zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, fig_name.Text + ".png"));
            safe_fig.Text = "Fig. saved";
            safe_fig.BackColor = Color.LimeGreen;
            await Task.Delay(800);
            safe_fig.Text = "Save fig.";
            safe_fig.BackColor = Color.Transparent;
        }



        private void showdata_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", path_logfile + "data.txt");
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

        //####################################################################################################################################### 


        private async void Global_iseg_terminal(object sender, MouseEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            if (c.Text == "Off")
            {
                _suspend_background_measurement.Reset();
                await Task.Delay(10);
                iseg.RawIO.Write(String.Format(":VOLT ON,(@{0})\n", ch[c.Name]));
                await_time(20);
                _suspend_background_measurement.Set();
                c.Text = "On";
                c.BackColor = Color.LimeGreen;
            }
            else
            {
                _suspend_background_measurement.Reset();
                await Task.Delay(10);
                iseg.RawIO.Write(String.Format(":VOLT OFF,(@{0})\n", ch[c.Name]));
                await_time(20);
                _suspend_background_measurement.Set();
                c.Text = "Off";
                c.BackColor = SystemColors.ControlLightLight;
            }
        }


        private async void Global_iseg_reload(object sender,EventArgs e)
        {
            Button b = sender as Button;
            bool Vset = Decimal.TryParse(vset[ch[b.Name]].Text.Replace(',', '.'), out decimal vset_in);
            vset[ch[b.Name]].Text = vset_in.ToString("0.000");
            if (Vset)
            {
                _suspend_background_measurement.Reset();
                iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", vset_in.ToString("0.000"), ch[b.Name])); // 3 decimal places
                await_time(20);
                _suspend_background_measurement.Set();
            }

            else
            {
                MessageBox.Show("Type in Vset (float)");
            }
        }


        private async void Global_iseg_reset(object sender, MouseEventArgs e)
        {
            Button r = sender as Button; // 3 decimal places
            _suspend_background_measurement.Reset();
            iseg.RawIO.Write(String.Format(":VOLT OFF,(@{0})\n", ch[r.Name]));
            await_time(20);
            iseg.RawIO.Write(String.Format(":VOLT 0.000,(@{0})\n", ch[r.Name]));
            await_time(20);
            _suspend_background_measurement.Set();
            vset[ch[r.Name]].Text = "";
            vmeas[ch[r.Name]].Text = "";
            stat[ch[r.Name]].Text = "Off";
            stat[ch[r.Name]].BackColor = SystemColors.ControlLightLight;
        }


        private async void btn_reload_all_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            for (int i = 0; i <= 5; i++)
            {
                bool Vset = Decimal.TryParse(vset[i].Text.Replace(',', '.'), out decimal vset_in);
                vset[i].Text = vset_in.ToString("0.000");
                if (Vset)
                {
                    iseg.RawIO.Write(String.Format(":VOLT {0},(@{1})\n", vset_in.ToString("0.000"), i)); // 3 decimal places
                    await_time(20);                   
                }

                else
                {
                    MessageBox.Show("Type in Vset (float)");
                    break;
                }
            }
            _suspend_background_measurement.Set();
        }


        private async void rs_all_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            iseg.RawIO.Write(String.Format("*RST\n"));
            await_time(20);
            _suspend_background_measurement.Set();
            for (int i = 0; i <= 5; i++)
            {
                vset[i].Text = "";
                vmeas[i].Text = "";
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
        }


        private async void stat_all_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            for (int i = 0; i <= 5; i++)
            {
                iseg.RawIO.Write(String.Format(":VOLT OFF,(@{0})\n", i));
                await_time(20);
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
            _suspend_background_measurement.Set();
        }


        private void bw_pressure_DoWork(object sender, DoWorkEventArgs e)
        {
            double pressure;
            while (!bw_pressure.CancellationPending)
            {
                LJM.eReadName(handle_pressure, pressure_pin, ref ionivac_v_out);
                Thread.Sleep(20);
                pressure = Math.Pow(10,((Convert.ToDouble(ionivac_v_out) -7.75))/0.75);
                bw_pressure.ReportProgress(0, pressure.ToString("0.00E0"));
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
                if (bw_iseg_volts.CancellationPending)        
                {
                    e.Cancel = true;
                    break;
                }

                try
                {
                    iseg.RawIO.Write(String.Format(":MEAS:VOLT? (@{0})\n", counter));
                    Thread.Sleep(20);
                    spannungen = iseg.RawIO.ReadString();
                    Thread.Sleep(10);
                    spannungen = iseg.RawIO.ReadString();
                    Thread.Sleep(10);
                    _suspend_background_measurement.WaitOne(Timeout.Infinite);
                    s = Double.Parse(spannungen.Replace("V\r\n", ""), System.Globalization.NumberStyles.Float);
                }
                catch (Exception)
                {
                    s = 0;
                }
                bw_iseg_volts.ReportProgress(counter, s.ToString("0.000"));
                Thread.Sleep(2);
                //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                Thread.Sleep(2);
                Thread.Sleep(2);
                _suspend_background_measurement.WaitOne(Timeout.Infinite);
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
            //vmeas2[e.ProgressPercentage].Text = Convert.ToString(e.UserState);
            int percentage = e.ProgressPercentage;
        }

        private void bw_iseg_volts_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {}


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


        int ct;
        private async void btn_start_counter_Click(object sender, EventArgs e)
        {
            try
            {
                cb_cnt_inf.Enabled = false;
                ct = int.Parse(tb_counter_ms.Text);
                tb_counter_ms.Text = ct.ToString();
                LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
                //LJM.eWriteName(handle_count, "DIO18_EF_OPTIONS", 1);
                //LJM.eWriteName(handle_count, "DIO18_EF_CONFIG_A", 2);
                //LJM.eWriteName(handle_count, "DIO18_EF_CONFIG_B", 100);
                //LJM.eWriteName(handle_count, "FIO7", 0);
                LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
                //LJM.eWriteName()
                LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref cnt_before);
                await Task.Delay(ct);
                LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                //  LJM.eWriteName(handle_count, "DIO18_EF_READ", ref cnt_before);
                double erg = cnt_after - cnt_before;
                tb_counter.Text = erg.ToString();
                //LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 0);
                cb_cnt_inf.Enabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Type in Integer!");
                cb_cnt_inf.Enabled = true;
            }
        }


        private async void XPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _suspend_background_measurement.Reset();
                iseg.RawIO.Write(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp));
                await_time(20);
                iseg.RawIO.Write("*RST\n");
                await_time(20);
                bw_iseg_volts.CancelAsync();
                iseg.Dispose();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Problems with closing Iseg device!", "Info", 500);
            }

            try
            {
                bw_pressure.CancelAsync();
                LJM.CloseAll();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Problems with closing Labjack device!", "Info", 500);
            }

        }
       


        private void enable_start ()
        {
            //btn_start.Enabled = (tb_pass.Text != string.Empty && tb_bias.Text != string.Empty && tb_stepwidth.Text != string.Empty && tb_counttime.Text != string.Empty);
            //if (tb_pass.Text != string.Empty && tb_bias.Text != string.Empty && tb_stepwidth.Text != string.Empty && tb_counttime.Text != string.Empty)
            vpass = Convert.ToDouble(cb_pass.SelectedItem);
            vbias = Convert.ToDouble(cb_bias.SelectedItem);
            vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
            tcount = Convert.ToDouble(cb_counttime.SelectedItem);
            vLens = Convert.ToDouble(cb_v_lens.SelectedItem);

            slop = Math.Truncate(vstepsize * 1000 / (tcount*4)); // multiples of 4 mV/s
            slope = (slop * 4)/1000;
            tb_slope.Text = (slope*1000).ToString();
            if (slop > 0 & start_ok)
            {
                btn_start.Enabled = true;
            }
        }

        private void cb_counttime_SelectedValueChanged(object sender, EventArgs e)
        {
            enable_start();
        }

        private void cb_stepwidth_SelectedValueChanged(object sender, EventArgs e)
        {
            enable_start();
        }


        public class AutoClosingMessageBox
        {
            //https://stackoverflow.com/questions/14522540/close-a-messagebox-after-several-seconds
            System.Threading.Timer _timeoutTimer;
            string _caption;
            AutoClosingMessageBox(string text, string caption, int timeout)
            {
                _caption = caption;
                _timeoutTimer = new System.Threading.Timer(OnTimerElapsed,
                    null, timeout, System.Threading.Timeout.Infinite);
                using (_timeoutTimer)
                    MessageBox.Show(text, caption);
            }
            public static void Show(string text, string caption, int timeout)
            {
                new AutoClosingMessageBox(text, caption, timeout);
            }
            void OnTimerElapsed(object state)
            {
                IntPtr mbWnd = FindWindow("#32770", _caption); // lpClassName is #32770 for MessageBox
                if (mbWnd != IntPtr.Zero)
                    SendMessage(mbWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                _timeoutTimer.Dispose();
            }
            const int WM_CLOSE = 0x0010;
            [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
            static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
            static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        }

        private async void btn_emcy_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            await Task.Delay(10);
            iseg.RawIO.Write(":VOLT EMCY OFF, (@0-5)\n");
            await_time(20);
            _suspend_background_measurement.Set();

            for (int i = 0; i < 6; i++)
            {
                stat[i].Text = "Off";
                stat[i].Enabled = false;
                reload[i].Enabled = false;
                reset[i].Enabled = false;
                stat[i].BackColor = SystemColors.ControlLightLight;
            }

            btn_start.Enabled = false;
        }

        private void btn_scpi_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:\Users\Test\Desktop\bac\Projects\Periodensystem\PE\SCPI.pdf");
            }
            catch (Exception)
            {
                MessageBox.Show("Cant find path to PDF!");
            }
        }


        private void bw_counter_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!bw_counter.CancellationPending)
            {
                try
                {
                    ct = int.Parse(tb_counter_ms.Text);
                    tb_counter_ms.Text = ct.ToString();
                    LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
                    //LJM.eWriteName(handle_count, "DIO18_EF_OPTIONS", 1);
                    //LJM.eWriteName(handle_count, "DIO18_EF_CONFIG_A", 2);
                    //LJM.eWriteName(handle_count, "DIO18_EF_CONFIG_B", 100);
                    //LJM.eWriteName(handle_count, "FIO7", 0);
                    LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
                    //LJM.eWriteName()
                    LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref cnt_before);
                    Thread.Sleep(ct);
                    LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                    //  LJM.eWriteName(handle_count, "DIO18_EF_READ", ref cnt_before);
                    double erg = cnt_after - cnt_before;
                    tb_counter.Text = erg.ToString();
                    //LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 0);
                }
                catch (Exception)
                {
                    MessageBox.Show("Type in Integer!");
                }
            }
        }

        private void bw_counter_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tb_counter.Text = Convert.ToString(e.UserState);
            //tb_counter.Text = e.ProgressPercentage.ToString();
        }

        private void bw_counter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                tb_counter.Text = "Stop!";
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_counter.Text = e.Error.Message;
            }

            else
            {

            }
            btn_start_counter.Enabled = true;
            cb_cnt_inf.Text = "Inf";
        }

        private void cb_cnt_inf_CheckStateChanged(object sender, EventArgs e)
        {
            if (cb_cnt_inf.Checked)
            {
                bw_counter.RunWorkerAsync();
                btn_start_counter.Enabled = false;
                cb_cnt_inf.Text = "end";
            }

            if (!cb_cnt_inf.Checked)
            {
                bw_counter.CancelAsync();
            }
        }
    }
}




//bugs:
// - nach clear führt das abwählen von elementen zzu einem error (da ebtl. noch in liste gespeichert)
// - close iseg HV
