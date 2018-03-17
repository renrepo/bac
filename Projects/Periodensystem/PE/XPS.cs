/***
 * CONTROL SOFTWARE ESCA LAB 5
 * Author: Rene Wabnitz
 * Email: 1410255@uni-wuppertal.de
 * Bergische Universität Wuppertal
***/

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
using Ivi.Visa;


/***
 * General stuff:
 * #################################
 * prefix lb...label
 * prefix tb...textbox
 * prefix btn... button
 * 
 * 
 * 
 * 
 * 
 * 
***/

namespace XPS
{
    public partial class XPS : Form
    {
        // IP adress Iseg-devices (ethernet connection)
        string ip_dps = "132.195.109.144";
        string ip_xray = "132.195.109.241";

        // General settings
        double V_photon = 21.21;            // Energiey HeI-line
        double W_aus = 4.5;                 // workfunction spectrometer
        double ri = 106;                    // Radius inner hemisphere in mm
        double ra = 112;                    // Radius outer hemisphere in mm
        double delta_channeltron = 3000;    // voltage drop over channeltron in V
        double deviation = 0.2;            // maximim voltage deviation (in V) at the beginng of the voltage ramp
        double perc_ramp = 40.000;          // voltage ramp in percent of 4000 V/s (4000 = Vnominal)
        string pressure_pin = "AIN0";       // Analog Input Pin of Ionivac
        double spannungsteiler = 10.9404;
        double vor = 16;
        double nach = 5;
        double slope_korr = 0.613;      // ergibt sich aus Plot vhemi gegen vlens bei maxmalen zählraten
        //double slope_korr = 1;
        double offset = - 48.85;
        //double offset = 20;


        // Labjack stuff
        int handle_pressure = 0;            // Labjack threads
        int handle_v_hemi = 0;
        int handle_v_hemo = 0;
        int handle_v_analyser = 0;
        int handle_v_lens = 0;
        int handle_count = 0;
        int handle_DAC = 0;
        int handle_DAC2 = 0;
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
        double vLens_min_korr;

        string read_iseg;                     // need to read back each comment send to Iseg device
        string path_logfile;
        string path_binding_energies = Path.GetFullPath("Bindungsenergien.csv");
        string path_colors = Path.GetFullPath("colors2.csv");
        string path_electronconfig = Path.GetFullPath("electronconfiguration.csv");
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string now = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string curr_time;
        private string lastResourceString = null;
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};       // name of energy-lines corresponding to electron binding energies in "Bindungsenergien.csv"
        List<List<string>> table_binding_energies = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
        List<string> display_labels = new List<string>();
        PointPairList values_to_plot = new PointPairList();
        Dictionary<string, string> binding_energies_dict = new Dictionary<string, string>();
        Dictionary<string, string> color_dict = new Dictionary<string, string>();
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
        System.Windows.Forms.Label[] lb_list_binding_energies;
        System.Windows.Forms.Label[] lb_list_orbital_structure;
        ManualResetEvent _suspend_background_measurement = new ManualResetEvent(true);
        private CancellationTokenSource _cts_pressure_labjack;           // Cancellation of Labjack pressure background measurement 
        private CancellationTokenSource _cts_volt_dps;           // Cancellation of Iseg DPS voltage background measurement 
        private CancellationTokenSource _cts_counter_labjack;           // Cancellation of Labjack Counter background measurement 
        private IMessageBasedSession DPS_HV;           // Iseg-HV session 6 Chanel HV
        private IMessageBasedSession Xray_HV;        // Iseg X-Ray HV session

        bool start_ok = false;
        bool stop = false;      // Interrupt "btn_start_Click"-method
        bool DPS_HV_is_open = false;
        bool Xray_HV_is_open = false;





        public XPS()
        {
            InitializeComponent();


            // graph for showing XPS/UPS spectra (zedGraph)
            myPane = zedGraphControl1.GraphPane;

            // formatting stuff for some buttons
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

            // border for the periodic table
            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;


            // display bindung energies in "Bindung energies" block & show oribital structure of an element
            lb_list_binding_energies = new System.Windows.Forms.Label [] {label4,label6,label8,label10,label12,label14,label16,label18,label20,label22,
                                                   label24,label26,label28,label30,label32,label34,label36,label38,label40,label42,
                                                    label44,label46,label48,label50};
            lb_list_orbital_structure = new System.Windows.Forms.Label[] {s1,s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16,s17,s18,s19,s20,s21,s22,
                                                   s23,s24,s25,s26,s27,s28,s29,s30,s31,s32,s33,s34,s35,s36,s37,s38,s39,s40,s41,s42,
                                                   s43,s44,s45,s46,s47,s48,s49,s50,s51,s52,s53,s54,s55,s56,s57,s58,s59};
        }









        private void Form1_Load(object sender, EventArgs e)
        {
            // dot instead of comma (very important for voltage input values!)
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            // draw coordinate system
            create_graph(myPane);
            // table_binding_energies: line=all elements in periodic table; column: 1) atomic number 2) element name 3)-x)electron binding energy
            // source: http://xdb.lbl.gov/Section1/Table_1-1.pdf
            table_binding_energies = File.ReadAllLines(path_binding_energies).Select(l => l.Split(',').ToList()).ToList();
            elec_bind = File.ReadAllLines(path_electronconfig).Select(l => l.Split(',').ToList()).ToList();
            // different linecolors for different elements the plot 
            color_dict = File.ReadLines(path_colors).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);

            //Dict with entries "element name" and "atomic number"
            for (int i = 0; i < table_binding_energies.Count; i++)
            {
                binding_energies_dict.Add(table_binding_energies[i][1], table_binding_energies[i][0]);
            }

            //create dictionary for logfiles
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
            {   // Open Labjack sessions for voltage/counts measurements and so on
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_count);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_hemi);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_hemo);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_analyser);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_v_lens);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_pressure);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_DAC);
                LJM.OpenS("ANY", "ANY", "ANY", ref handle_DAC2);

                // backgroundworker for pressure measurement at Ionivac-device mounted on analyser chamber.
                // pressure value updates every second

                /***
                if (!bw_pressure.IsBusy)
                {
                    bw_pressure.RunWorkerAsync();
                }
                ***/

                // NEEDS TO BE TESTED!
                background_meas_pressure_labjack();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 device!", "Info", 500);
            }


            //buttons for interactive periodic table
            Button [] but = {H ,He, Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar, K, Ca, Sc,
                             Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr, Rb, Sr, Y, Zr,
                             Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe, Cs, Ba, La, Hf, Ta,
                             W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn, Fr, Ra, Ac, Ce, Pr, Nd,
                             Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Th, Pa, U, Rf, Np, Pu, Am, Cm,
                             Bk, Cf, Es, Fm, Md, No, Lr};

            // textboxes and buttons for the "Iseg Control" panel (controlling of the 6-chanel-HV-device)
            vset = new TextBox[] { ch1_v, ch2_v, ch3_v, ch4_v, ch5_v, ch6_v };
            vmeas = new TextBox[] { ch1_meas, ch2_meas, ch3_meas, ch4_meas, ch5_meas, ch6_meas };
            vmeas2 = new TextBox[] { vm1, vm2, vm3, vm4, vm5 };
            reload = new Button[] { btn_reload1, btn_reload2, btn_reload3, btn_reload4, btn_reload5, btn_reload6 };
            reset = new Button[] { rs1, rs2, rs3, rs4, rs5, rs6 };
            stat = new CheckBox[] { stat1, stat2, stat3, stat4, stat5, stat6 };


            // click-event for buttons in the periodic table will call the "global_element_click"-method
            foreach (var item in but)
            {
                item.MouseDown += global_element_click;
            }

            //same as above but for the on/off switches in "Iseg ControL" tab
            foreach (var item in stat)
            {
                item.MouseDown += Global_DPS_on_off_switch;
            }

            // reset buttons (turn off HV-chanel and reset)
            foreach (var item in reset)
            {
                item.MouseDown += Global_DPS_reset;
            }

            // read in the new voltage-values but not turn on the HV-chanel
            foreach (var item in reload)
            {
                item.MouseDown += Global_DPS_reload_volt;
            }

            // raises event if key is pressed within vset textboxes in "iseg control"
            // (goal: if enter-key pressed --> call reload method)
            //https://stackoverflow.com/questions/3752451/enter-key-pressed-event-handler
            foreach (var item in vset)
            {
                item.KeyDown += new KeyEventHandler(tb_KeyDown);
            }


            // default values for pass-energy, bias-voltage,... shown in the "XPS and UPS settings"
            cb_pass.SelectedIndex = 3;
            cb_bias.SelectedIndex = 3;
            cb_counttime.SelectedIndex = 1;
            cb_stepwidth.SelectedIndex = 1;
            cb_v_lens.SelectedIndex = 2;

            // proportionality between the voltage applied to the hemispheres and the pass energy
            k = ra / ri - ri / ra;

            // WHY?????????????????????????????????????????????????????????
            enable_start();
        }











        //##################################################################################################################################################################


        // default settings
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


        // right-click on a button in the periodic table: display binding energies and electronic configuration of the selected element
        // left-click: draw energy-lines of the selected elements into the plot
        private void global_element_click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draw_energy_lines((Button)sender, yaxis);
            }
            if (e.Button == MouseButtons.Right)
            {
                electron_configuration((Button)sender);
            }
        }


        // displays elementname and atomic number if cursor rollover buttons in periodic table
        private void elementnames_Popup(object sender, PopupEventArgs e) { }


        // switch on/off one of the six Iseg DPS HV-modules in "Iseg Control" tab
        private async void Global_DPS_on_off_switch(object sender, MouseEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            int chanel = Convert.ToInt16(c.Tag);
            _suspend_background_measurement.Reset();
            //await Task.Delay(10);

            if (c.Text == "Off")
            {
                await write_to_Iseg(String.Format(":VOLT ON,(@{0})\n", chanel), "DPS");
                c.Text = "On";
                c.BackColor = Color.LimeGreen;
            }

            else
            {
                await write_to_Iseg(String.Format(":VOLT OFF,(@{0})\n", chanel), "DPS");
                c.Text = "Off";
                c.BackColor = SystemColors.ControlLightLight;
            }
            _suspend_background_measurement.Set();
        }


        // apply/reload voltage on an Iseg DPS HV-module in "Iseg Control" tab
        private async void Global_DPS_reload_volt(object sender, EventArgs e)
        {
            Button b = sender as Button;
            int chanel = Convert.ToInt16(b.Tag);
            bool Vset = Decimal.TryParse(vset[chanel].Text.Replace(",","."), out decimal vset_in);
            //vset[chanel].Text = vset_in.ToString("0.000");
            if (Vset)
            {
                _suspend_background_measurement.Reset();
                await write_to_Iseg(String.Format(":VOLT {0},(@{1})\n", vset_in.ToString("0.000"), chanel), "DPS"); // 3 decimal places
                _suspend_background_measurement.Set();
            }

            else
            {
                MessageBox.Show("Type in Vset (float)");
            }
        }


        // call "Global_DPS_reload_volt" if enter-key is pressed in vset-textbox in "iseg control"
        private void tb_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (e.KeyCode == Keys.Enter)
            {
                // first argument: button corresponding to textbox
                Global_DPS_reload_volt(reload[Convert.ToInt16(tb.Tag)], new EventArgs());
            }
        }


        // switch off voltage at Iseg DPS HV-chanel in "Iseg Control" tab
        private async void Global_DPS_reset(object sender, MouseEventArgs e)
        {
            Button r = sender as Button; // 3 decimal places
            int chanel = Convert.ToInt16(r.Tag);
            _suspend_background_measurement.Reset();
            await write_to_Iseg(String.Format(":VOLT OFF,(@{0})\n", chanel), "DPS");
            await write_to_Iseg(String.Format(":VOLT 0.000,(@{0})\n", chanel), "DPS");        
            _suspend_background_measurement.Set();
            vset[chanel].Text = "";
            vmeas[chanel].Text = "";
            stat[chanel].Text = "Off";
            stat[chanel].BackColor = SystemColors.ControlLightLight;
        }


        // this method changes the apparence/color of a button in the periodic table when pressed.
        // Additionally the corresponding energy-lines and some more infomation were plottet in the graph
        public void draw_energy_lines(object sender, YAxis ya)
        {
            Button btn = (Button)sender;
            // different colors for different elements
            string col = color_dict[btn.Name];
            // current_line is equal to the atomic number of the selected element
            int current_line = Convert.ToInt32(binding_energies_dict[btn.Name]) - 1;
            float value;

            // button not left-klicked
            if (btn.ForeColor == Color.DimGray)
            {
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;

                for (int i = 2; i <= 25; i++)
                {
                    bool result = float.TryParse(table_binding_energies[current_line][i], out value);

                    // beginning at K1s shell (i=2), check weather there are more electron binding energies for the selected element (i>2)
                    if (result)
                    {
                        // print out information at the top of a line in the graph (element name, electron binding energy, K/L/M-line)
                        pane_labs = new TextObj((table_binding_energies[current_line][i] + "\n" + table_binding_energies[current_line][1] + " " + scores[i]), 
                            float.Parse(table_binding_energies[current_line][i], CultureInfo.InvariantCulture), -0.05, CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        pane_labs.FontSpec.Size = 10f;
                        pane_labs.FontSpec.Angle = 40;
                        pane_labs.FontSpec.Fill.Color = Color.Transparent;
                        pane_labs.FontSpec.FontColor = Color.DimGray;
                        pane_labs.FontSpec.Border.IsVisible = false;
                        pane_labs.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(pane_labs);
                        display_labels.Add(btn.Name);

                        // formatting for the energy-lines shown in the graph when an element is selected
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
                        ya.Cross = Double.Parse(table_binding_energies[current_line][i], CultureInfo.InvariantCulture);
                        ya.IsVisible = true;
                        ya.MajorGrid.IsZeroLine = false;
                        // hides xaxis
                        myPane.YAxisList.Add(ya);
                    }
                }
                zedGraphControl1.Refresh();
            }

            // button already left-klicked
            else
            {
                btn.ForeColor = Color.DimGray;
                btn.Font = new Font("Arial", 12, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.DimGray;

                // erease energy-lines and displayed information
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


        // Function for electronic configuration plot
        public void electron_configuration(object sender)
        {
            Button btn = (Button)sender;
            // current_line is equal to the atomic number of the selected element
            int current_line = Convert.ToInt32(binding_energies_dict[btn.Name]) - 1;

            // button not right-klicked
            if (lb_element_name.Text == "")
            {
                // display electron binding energies of an element in the "Binding Energies"-box
                // startvalue i=2 because binding energy starts at column 2 in "table_binding_energies"
                for (int i = 2; i <= lb_list_binding_energies.Count(); i++)
                {
                    lb_list_binding_energies[i-2].Text = table_binding_energies[current_line][i];
                }

                // display orbital configuration of the element
                for (int i = 1; i <= lb_list_orbital_structure.Count(); i++)
                {
                    lb_list_orbital_structure[i-1].Text = elec_bind[current_line][i];
                }

                lb_element_name.Text = elementnames.GetToolTip(btn);
                lb_atomic_number.Text = binding_energies_dict[btn.Name];
            }

            // button already right-klicked: clear all labels
            else
            {
                foreach (var label in lb_list_binding_energies)
                {
                    label.Text = "";
                }

                foreach (var label in lb_list_orbital_structure)
                {
                    label.Text = "--";
                }

                lb_element_name.Text = "";
                lb_atomic_number.Text = "";
            }
        }


        // write SCPI-command to Iseg HV-device. wait 20ms after sending and receiving commands.
        // If there is no readback after a command was send to the device there may be future issues 
        // regarding readbacks.
        // "async"-methods prevent the user interface to freeze while "await wait" is called within the method.
        // further information re "async/await" see https://stackoverflow.com/questions/14455293/how-and-when-to-use-async-and-await
        private async Task<int> write_to_Iseg(string command, string device)
        {
            try
            {
                if (device == "DPS")
                {
                    DPS_HV.RawIO.Write(command);
                }

                if (device == "XRAY")
                {
                    Xray_HV.RawIO.Write(command);
                }

                await Task.Delay(20);
            }
            catch (Exception)
            {
                if (device == "DPS")
                {
                    MessageBox.Show("Can't write to Iseg DPS");
                }

                if (device == "XRAY")
                {
                    MessageBox.Show("Can't write to Iseg X-Ray Power Supply");
                }
            }

            return 1;
        }



        private async Task<string> read_from_Iseg(string command, string device)
        {
            read_iseg = "";

            try
            {
                if (device == "DPS")
                {
                    DPS_HV.RawIO.Write(command);
                    await Task.Delay(20);
                    read_iseg = DPS_HV.RawIO.ReadString();
                }

                if (device == "XRAY")
                {
                    Xray_HV.RawIO.Write(command);
                    await Task.Delay(20);
                    read_iseg = Xray_HV.RawIO.ReadString();
                }

                await Task.Delay(40);
            }
            catch (Exception)
            {
                if (device == "DPS")
                {
                    MessageBox.Show("Can't write/read to/from Iseg DPS");
                }

                if (device == "XRAY")
                {
                    MessageBox.Show("Can't write/read to/from Iseg X-Ray Power Supply");
                }
            }
            return read_iseg;
        }



        // same as above. should be replaced by async/await in the near futur (together with background worker)
        private string write_to_DPS_sync(string command)
        {
            try
            {
                DPS_HV.RawIO.Write(command);
                Thread.Sleep(20);
                read_iseg = DPS_HV.RawIO.ReadString();
                Thread.Sleep(40);
            }
            catch (Exception)
            {
                MessageBox.Show("Backgroundworker DPS voltage measurement failed");
            }

            return read_iseg;
        }



        //#############################################################################################################################
        // Open Iseg DPS 6-Chanel HV device & Iseg X-Ray Power Supply


        private async void openSessionButton_Click(object sender, EventArgs e)
        {

            using (ResourceManager rm = new ResourceManager())
            {
                try
                {
                    DPS_HV = (IMessageBasedSession)rm.Open("TCPIP0::" + ip_dps +"::10001::SOCKET");
                    DPS_HV_is_open = false;
                    // no timeout-error when reading back from Iseg-device after query (e.g. ":MEAS:VOLT? (@0)\n") was send 
                    //(if no query was send, a readback will take about 2000ms (default timeout) and give a "null"-result)
                    ((INativeVisaSession)DPS_HV).SetAttributeBoolean(NativeVisaAttribute.SuppressEndEnabled, false);
                    await write_to_Iseg("CONF:HVMICC HV_OK\n","DPS");
                    await write_to_Iseg(":VOLT EMCY CLR,(@0-5)\n","DPS");
                    await write_to_Iseg("*RST\n","DPS");
                    //await write_to_Iseg(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp),"DPS");
                    //bw_iseg_volts.RunWorkerAsync();
                    for (int i = 0; i < 6; i++)
                    {
                        reset[i].Enabled = true;
                        reload[i].Enabled = true;
                        stat[i].Enabled = true;
                    }
                    rs_all.Enabled = true;
                    queryButton.Enabled = true;
                    writeButton.Enabled = true;
                    clearButton.Enabled = true;
                    btn_emcy.Enabled = true;
                    start_ok = true;
                    enable_start();
                    background_meas_volt_DPS();
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


        private async void openSessionButton_Xray_Click(object sender, EventArgs e)
        {
            using (var rm = new ResourceManager())
            {
                try
                {
                    Xray_HV = (IMessageBasedSession)rm.Open("TCPIP0::"+ ip_xray + "::10001::SOCKET");
                    Xray_HV_is_open = true;
                    // no timeout-error when reading back from Iseg-device after query (e.g. ":MEAS:VOLT? (@0)\n") was send 
                    //(if no query was send, a readback will take about 2000ms (default timeout) and give a "null"-result)
                    ((INativeVisaSession)Xray_HV).SetAttributeBoolean(NativeVisaAttribute.SuppressEndEnabled, false);
                    await write_to_Iseg("*RST\n", "XRAY");
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


        private async void close_Xray_HV_session_Click(object sender, EventArgs e)
        {
            try
            {
                await write_to_Iseg("*RST\n", "XRAY");
                Xray_HV.Dispose();
                Xray_HV_is_open = false;
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can not close Iseg Xray HV", "Info", 500);
            }
        }


        private async void close_DPS_HV_session_Click(object sender, EventArgs e)
        {
            try
            {
                await write_to_Iseg("*RST\n", "DPS");
                DPS_HV.Dispose();
                DPS_HV_is_open = false;
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can not close Iseg DPS HV", "info", 500);
            }
        }


        private async void write_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                await write_to_Iseg(ReplaceCommonEscapeSequences(writeTextBox.Text + "\n"), "DPS");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                _suspend_background_measurement.Set();
            }
        }


        private async void query_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                readTextBox.Text = await read_from_Iseg(writeTextBox.Text + '\n', "DPS");
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                _suspend_background_measurement.Set();
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


































        //###########################################################################################################################################
        // Backgroundworker for taking XPS/UPS spectra
        double vLens_min = 0;
        double vLens_max = 0;

        private async void btn_start_Click(object sender, EventArgs e)
        {
            enable_start();
            btn_can.Enabled = true;
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
                    file.WriteLine("#Volt. bias: \t{0} \t{1}", vbias.ToString("0.0"), "V");
                    file.WriteLine("#Volt. lens: \t{0} \t{1}", vLens.ToString("0.0"), "V");
                    file.WriteLine("#Step width: \t{0} \t{1}", vstepsize.ToString("0.0"), "meV");
                    file.WriteLine("#Counttime: \t{0} \t{1}", tcount.ToString("0.0"), "ms");
                    file.WriteLine("#Vor: \t{0} \t{1}", vor.ToString("0.0"), "V");
                    file.WriteLine("#Nach: \t{0} \t{1}", nach.ToString("0.0"), "V");
                    file.WriteLine("#slope: \t{0} \t{1}", slope_korr.ToString("0.000"), "V/s");
                    file.WriteLine("#offset: \t{0} \t{1}", offset.ToString("0.000"), "V");
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("#Slope: \t{0} \t{1}", (slope).ToString("0.0"), "mV/s");
                    file.WriteLine("#Counttime: \t{0} \t{1}", tcount.ToString("0.0"), "ms");
                    //file.WriteLine("#X-ray source: \t{0}", source + Environment.NewLine);
                    //file.WriteLine("#E_b \t counts");    
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("#E_k \t cps \t Ana \t Hemi \t Hemo \t VP \t EP \t 0.4 \t Mt \t Lens");
                    file.WriteLine("" + Environment.NewLine);
                }

                v_analyser_min = (vpass - V_photon + vbias - vor)*spannungsteiler;          //hier sollte kein elektron mehr im channeltron ankommen
                //v_analyser_min = vpass - V_photon + vbias - 5;
                v_hemo_min = (v_analyser_min - (vpass * k * 0.4 * spannungsteiler));  //äußere hemispährenspannung aus passenergie nach spannugnsteiler (3.885M, 5.58M,269.5k))
                v_hemi_min = (v_hemo_min + vpass * k * spannungsteiler);               //liegt entsprechung die Spannungdifferenz drüber
                                                                                            // plus korrekturterme
                vLens_min = v_hemi_min + offset/slope_korr;                                 // empirisch bestimmt

                // v_analyser_max = vpass +vbias;          //hier sollte auch das langsamste Elektron ankommen
                v_analyser_max = (v_analyser_min + (V_photon + nach) * spannungsteiler);
                v_hemo_max = (v_analyser_max - (vpass * k * 0.4 * spannungsteiler));  //äußere hemispährenspannung aus passenergie nach spannugnsteiler (3.885M, 5.58M,269.5k))
                v_hemi_max = (v_analyser_max + vpass * k * spannungsteiler);

                v_channeltron_out_min = v_analyser_min/spannungsteiler + 3000;
                v_channeltron_out_max = v_analyser_max/spannungsteiler + 3000;

                vLens_max = v_hemi_max + offset/slope_korr;

                if (stop)
                {
                    stop = false;
                    return;
                }

                _suspend_background_measurement.Reset();
                await write_to_Iseg(String.Format(":CONF:RAMP:VOLT 10.000%/s\n"), "DPS");     // Rampe auf 400 V/s
                await write_to_Iseg(String.Format(":VOLT {0},(@0)\n", v_hemi_min.ToString("0.000")), "DPS");
                await write_to_Iseg(String.Format(":VOLT {0},(@1)\n", v_hemo_min.ToString("0.000")), "DPS");
                await write_to_Iseg(String.Format(":VOLT {0},(@2)\n", vLens_min.ToString("0.000")), "DPS");
                await write_to_Iseg(String.Format(":VOLT {0},(@4)\n", v_channeltron_out_min.ToString("0.000")), "DPS");
                await write_to_Iseg(String.Format(":VOLT ON,(@0-5)\n"), "DPS");

                int j = 0;

                if (stop)
                {
                    await write_to_Iseg(String.Format("*RST\n"), "DPS");
                    stop = false;
                    return;
                }

                for (int i = 0; i < 20; i++)             //warten bis sich Spannungen gesetzt haben
                {
                    LJ_hemi2 = 0;
                    LJ_hemo2 = 0;
                    LJ_lens2 = 0;
                    LJ_analyser2 = 0;

                    while (j<2)
                    {
                        LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                        Thread.Sleep(2);
                        LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);
                        Thread.Sleep(2);
                        LJM.eReadName(handle_v_lens, "AIN6", ref LJ_lens);
                        Thread.Sleep(2);
                        LJ_hemi2 += LJ_hemi / 0.1988;
                        LJ_hemo2 += LJ_hemo / 0.1988;
                        LJ_lens2 += LJ_lens / 0.1988;
                        LJ_analyser2 += LJ_hemo / 0.1988 + ((LJ_hemi / 0.1988 - LJ_hemo / 0.1988) * 0.4);
                        j += 1;
                    }
                    vm1.Text = (LJ_hemi2/2).ToString("0.000");
                    vm2.Text = (LJ_hemo2/2).ToString("0.000");
                    vm3.Text = (LJ_lens2/2).ToString("0.000");
                    vm4.Text = (LJ_analyser2/2).ToString("0.000");
                    vm5.Text = v_channeltron_out_max.ToString("0.000");

                    await Task.Delay(1000);

                    j = 0;
                    if (stop)
                    {
                        await write_to_Iseg(String.Format("*RST\n"), "DPS");
                        stop = false;
                        return;
                    }
                }
                 
                while (Math.Abs(LJ_hemi2 * spannungsteiler / 2 - v_hemi_min) > deviation || Math.Abs(LJ_hemo2 * spannungsteiler / 2 - v_hemo_min) > deviation)
                   // || Math.Abs(LJ_lens2 * spannungsteiler / 2 - vLens_min) > deviation)
                {
                    if (Math.Abs(LJ_hemi2 * spannungsteiler / 2 - v_hemi_min) > deviation)
                    {
                        v_hemi_min_korr = v_hemi_min - (LJ_hemi2*spannungsteiler/2 - v_hemi_min);
                        if (Math.Abs(v_hemi_min_korr) < 500)
                        {
                            await write_to_Iseg(String.Format(":VOLT {0},(@0)\n", v_hemi_min_korr.ToString("0.000")), "DPS");
                        }
                    }

                    if (Math.Abs(LJ_hemo2 * spannungsteiler / 2 - v_hemo_min) > deviation)
                    {
                        v_hemo_min_korr = v_hemo_min - (LJ_hemo2*spannungsteiler/2 - v_hemo_min);
                        if (Math.Abs(v_hemi_min_korr) < 500)
                        {
                            await write_to_Iseg(String.Format(":VOLT {0},(@1)\n", v_hemo_min_korr.ToString("0.000")), "DPS");
                        }
                    }

                    //if (Math.Abs(LJ_lens2 * spannungsteiler / 2 - vLens_min) > deviation)
                    //{
                    //    vLens_min_korr = vLens_min - (LJ_lens2 * spannungsteiler / 2 - vLens_min);
                    //    if (Math.Abs(vLens_min_korr) < 500)
                    //    {
                    //        await write_to_Iseg(String.Format(":VOLT {0},(@2)\n", vLens_min_korr.ToString("0.000")),"DPS");
                    //    }
                    //}



                    await Task.Delay(10000);

                    LJ_hemi2 = 0;
                    LJ_hemo2 = 0;
                    LJ_analyser2 = 0;
                    LJ_lens2 = 0;

                    while (j < 2)
                    {
                        LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                        Thread.Sleep(2);
                        LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);
                        Thread.Sleep(2);
                        LJM.eReadName(handle_v_lens, "AIN6", ref LJ_lens);
                        Thread.Sleep(2);
                        LJ_hemi2 += LJ_hemi / 0.1988;
                        LJ_hemo2 += LJ_hemo / 0.1988;
                        LJ_lens2 += LJ_lens / 0.1988;
                        LJ_analyser2 += LJ_hemo / 0.1988 + ((LJ_hemi / 0.1988 - LJ_hemo / 0.1988) * 0.4);
                        j += 1;
                    }
                    vm1.Text = (LJ_hemi2 / 2).ToString("0.000");
                    vm2.Text = (LJ_hemo2 / 2).ToString("0.000");
                    vm3.Text = (LJ_lens / 2).ToString("0.000");
                    vm4.Text = (LJ_analyser2 / 2).ToString("0.000");
                    vm5.Text = v_channeltron_out_max.ToString("0.000");
                    //_suspend_background_measurement.Set();

                    if (stop)
                    {
                        await write_to_Iseg(String.Format("*RST\n"), "DPS");
                        stop = false;
                        return;
                    }
                }

                bW_data.RunWorkerAsync(); //run bW if it is not still running
                btn_start.Enabled = false;
                btn_can.Enabled = true;
                tb_show.Enabled = true;
                tb_safe.Enabled = false;
            }

            else
            {
                MessageBox.Show("BW busy!");
            }
        }

        double sc = 0;
        double k;
        double E_pass;
        double E_kin;
        long ms;
        long ticks;

        private void bW_data_DoWork(object sender, DoWorkEventArgs e)
        {
            LJ_hemi2 = 0;
            LJ_hemo2 = 0;
            LJ_analyser2 = 0;
            LJ_lens2 = 0;
            Stopwatch sw = new Stopwatch();
            LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
            write_to_DPS_sync(String.Format(":CONF:RAMP:VOLT {0}%/s\n", slop * 0.01));
            //await write_to_DPS_sync(String.Format(":CONF:RAMP:VOLT {0}%/s\n", 0.0004));
            write_to_DPS_sync(String.Format(":VOLT {0},(@0)\n", v_hemi_max.ToString("0.000")));
            write_to_DPS_sync(String.Format(":VOLT {0},(@1)\n", v_hemo_max.ToString("0.000")));
            write_to_DPS_sync(String.Format(":VOLT {0},(@2)\n", vLens_max.ToString("0.000")));
            write_to_DPS_sync(String.Format(":VOLT {0},(@4)\n", v_channeltron_out_max.ToString("0.000")));
            //bw_lens.RunWorkerAsync();
            LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
            while (true)
            {
                int i = 0;
                int count2 = Convert.ToInt16(tcount);
                sw.Start();
                LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                sc = intcounter;
                while (sw.ElapsedMilliseconds < count2)
                {
                    LJM.eReadName(handle_v_hemi, "AIN2", ref LJ_hemi);
                    Thread.Sleep(2);
                    LJM.eReadName(handle_v_hemo, "AIN1", ref LJ_hemo);
                    Thread.Sleep(2);
                    LJM.eReadName(handle_v_lens, "AIN6", ref LJ_lens);
                    Thread.Sleep(2);
                    LJ_hemi2 += LJ_hemi / 0.1988;
                    LJ_hemo2 += LJ_hemo / 0.1988;
                    LJ_lens2 += LJ_lens / 0.1988;
                    LJ_analyser2 += LJ_hemo / 0.1988 + ((LJ_hemi / 0.1988-LJ_hemo / 0.1988) * 0.4);

                    if (i==2)
                    {
                        break;
                    }

                    while (sw.ElapsedMilliseconds < (count2-20) / (2-i))
                    {
                        Thread.Sleep(1);
                    }
                    i += 1;
                }


                while (sw.ElapsedMilliseconds < count2 )
                {
                    Thread.Sleep(1);
                }

                LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                ticks = sw.ElapsedTicks;
                ms = ticks*1000 / Stopwatch.Frequency;
                E_pass = (LJ_hemi2 - LJ_hemo2) / (3*k);
                //E_kin = E_pass - LJ_analyser2 - vbias;          // denn für detektierte e- gilt: 0 = Vbias + Ekin^0 + V_ana^0 - V_pass (^0: bezogen auf 0V)
                E_kin = vbias - LJ_analyser2/3 + E_pass;          // ohne berücksichtigung de raustrittsarbeit
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(E_kin.ToString("0.000") + "\t" + ((intcounter-sc)*1000/ms).ToString("00000") + "\t" + (LJ_analyser2/3).ToString("0.000") + "\t"
                        + (LJ_hemi2/3).ToString("0.000") + "\t" + (LJ_hemo2/3).ToString("0.000") + "\t" + ((LJ_hemo2 - LJ_hemi2)/3).ToString("0.000") + "\t"
                        + E_pass.ToString("0.000") + "\t" +((LJ_hemo2 - LJ_analyser2) / (LJ_hemi2 - LJ_hemo2)).ToString("0.000") + "\t" + ms.ToString("0.000") + "\t" + (LJ_lens2 / 3).ToString("0.000") + "\t"
                        + ((LJ_hemi2*0.612/3 - 4.017)-LJ_lens2 / 3).ToString("0.000"));
                }
                //await write_to_DPS_sync(String.Format(":VOLT {0},(@2)\n", ((LJ_hemi2 / 3) * 6.707 - 42.134).ToString("0.000")));
                bW_data.ReportProgress(0, ((intcounter - sc) * 1000 / ms).ToString("00000"));


                if (bW_data.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_data.ReportProgress(0);
                    LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
                    break; //warum? ist wichtig! vllt um aus for-loop zu kommen
                }

                sw.Reset();

            }
            e.Result = ((intcounter - sc) * 1000 / ms); //stores the results of what has been done in bW
            LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
        }

        private void bW_data_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            vm1.Text = (LJ_hemi2/3).ToString("0.000");
            vm2.Text = (LJ_hemo2/3).ToString("0.000");
            vm3.Text = (LJ_lens2 / 3).ToString("0.000");
            vm4.Text = (LJ_analyser2/3).ToString("0.000");
            vm5.Text = v_channeltron_out_max.ToString("0.000");
            tb_counter.Text = Convert.ToString(e.UserState);
            values_to_plot.Add(E_kin, Convert.ToDouble(e.UserState));
            myCurve.AddPoint(E_kin, Convert.ToDouble(e.UserState));
            LJ_hemi2 = 0;
            LJ_hemo2 = 0;
            LJ_analyser2 = 0;
            LJ_lens2 = 0;
            ms = 0;
            zedGraphControl1.Invalidate();
            zedGraphControl1.AxisChange();
        }

        private void bW_data_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                write_to_DPS_sync(String.Format(":CONF:RAMP:VOLT 10.000%/s\n"));
                write_to_DPS_sync(String.Format(":VOLT OFF,(@0-5)\n"));

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
                //bw_lens.CancelAsync();
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
            stop = true;
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

        // 16.03.18 tested --OK (kommentare einfügen und ggf. kürzen)
        private async void btn_reload_all_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            for (int i = 0; i <= 5; i++)
            {
                bool Vset = Decimal.TryParse(vset[i].Text.Replace(',', '.'), out decimal vset_in);
                vset[i].Text = vset_in.ToString("0.000");
                if (Vset)
                {
                    await write_to_Iseg(String.Format(":VOLT {0},(@{1})\n", vset_in.ToString("0.000"), i), "DPS"); // 3 decimal places              
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
            await write_to_Iseg(String.Format("*RST\n"), "DPS");
            _suspend_background_measurement.Set();
            for (int i = 0; i <= 5; i++)
            {
                vset[i].Text = "";
                vmeas[i].Text = "";
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
        }

        // 16.03.18 tested --OK (kommentare einfügen und ggf. kürzen)
        private async void stat_all_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            for (int i = 0; i <= 5; i++)
            {
                await write_to_Iseg(String.Format(":VOLT OFF,(@{0})\n", i), "DPS");
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
            _suspend_background_measurement.Set();
        }








        // tested 16.03 -- OK!
        private async void background_meas_pressure_labjack()
        {
            double pressure;
            _cts_pressure_labjack = new CancellationTokenSource();
            var token = _cts_pressure_labjack.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_pressure.Text = value;
            });
            var progress = progressHandler as IProgress<string>;
            try
            {
                await Task.Run(() =>
                {
                    
                    while (true)
                    {
                        LJM.eReadName(handle_pressure, pressure_pin, ref ionivac_v_out);
                        pressure = Math.Pow(10, ((Convert.ToDouble(ionivac_v_out) - 7.75)) / 0.75);
                        if (progress != null)
                        {
                            progress.Report(pressure.ToString("0.00E0"));
                            token.ThrowIfCancellationRequested();
                        }
                        Thread.Sleep(2000);
                    }
                });
                MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                tb_pressure.Text = "can";
            }
        }


        private void btn_cancel_ak_pressure_Click(object sender, EventArgs e)
        {
            if (_cts_pressure_labjack != null)
            {
                _cts_pressure_labjack.Cancel();
            }

            if (_cts_volt_dps != null)
            {
                _cts_volt_dps.Cancel();
            }
        }


        private void btn_unpause_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Set();
        }

        private void btn_pause_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
        }






        //tested 16.03.18 --OK
        private async void background_meas_volt_DPS()
        {
            _cts_volt_dps = new CancellationTokenSource();
            var token = _cts_volt_dps.Token;
            int result = 0;
            int i = -1;
            var progressHandler2 = new Progress<string>(value =>
            {
                vmeas[result].Text = value;
            });
            var progress = progressHandler2 as IProgress<string>;
            string readback = "";
            try
            {

                await Task.Run(() =>
                {
                    while (true)
                    {
                        ++i;
                        if (i == 6)
                        {
                            i = 0;
                        }
                        // call of read_from_iseg not possible because this part has to run synchronously
                        DPS_HV.RawIO.Write(String.Format(":MEAS:VOLT? (@{0})\n", i));
                        Thread.Sleep(50);
                        readback = DPS_HV.RawIO.ReadString();
                        progress.Report(readback.Replace("V\r\n", "").ToString());
                        //progress.Report(i.ToString());
                        result = i;
                        // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                        _suspend_background_measurement.WaitOne(Timeout.Infinite);
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(50);
                    }
                });
                //MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                MessageBox.Show("Problem with background measurement of Iseg DPS voltages");
            }
        }














     








        private async void XPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cts_pressure_labjack != null)
            {
                _cts_pressure_labjack.Cancel();
            }

            if (_cts_volt_dps != null)
            {
                _cts_volt_dps.Cancel();
            }

            if (_cts_counter_labjack != null)
            {
                _cts_counter_labjack.Cancel();
            }

            if (DPS_HV_is_open)
            {
                _suspend_background_measurement.Reset();
                //bw_iseg_volts.CancelAsync();
                await write_to_Iseg(String.Format(":CONF:RAMP:VOLT {0}%/s\n", perc_ramp), "DPS");
                await write_to_Iseg("*RST\n", "DPS");
                DPS_HV.Dispose();
            }

            if (Xray_HV_is_open)
            {
                await write_to_Iseg("*RST\n", "XRAY");
                Xray_HV.Dispose();
            }

            try
            {
                LJM.CloseAll();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Problems with closing Labjack device!", "Info", 500);
            }

            start_ok = false;
            Thread.Sleep(1000);

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

            slop = Math.Truncate(vstepsize * 1000 * 11 / (tcount*400)); // multiples of 400 mV/s. 1:11 Voltage devider --> min.slope = 36.4 mV/s
            slope = (slop * 400)/11;
            tb_slope.Text = slope.ToString("0.0");
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
            await write_to_Iseg(":VOLT EMCY OFF, (@0-5)\n", "DPS");
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




        private void cb_counter_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;

            if (c.Text == "Off")
            {
                c.Text = "On";
                c.BackColor = Color.LimeGreen;
                background_counter_labjack();
            }

            else
            {
                c.Text = "Off";
                c.BackColor = SystemColors.ControlLightLight;
                if (_cts_counter_labjack != null)
                {
                    _cts_counter_labjack.Cancel();
                }
            }
        }


        private async void background_counter_labjack()
        {
            _cts_counter_labjack = new CancellationTokenSource();
            var token = _cts_counter_labjack.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_counter.Text = value;
            });
            var progress = progressHandler as IProgress<string>;
            try
            {
                tb_counter_ms.ReadOnly = true;
                int ct = int.Parse(tb_counter_ms.Text);
                double erg = 0;
                Stopwatch sw = new Stopwatch();

                await Task.Run(() =>
                {

                    while (true)
                    {
                        //LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
                        //LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
                        sw.Start();
                        //LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref cnt_before);
                        Thread.Sleep(ct);
                        sw.Stop();
                        //LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                        //erg = (cnt_after - cnt_before) / sw.Elapsed.TotalSeconds;
                        erg = sw.Elapsed.TotalMilliseconds;
                        sw.Reset();
                        if (progress != null)
                        {
                            progress.Report(erg.ToString("N3"));    //no decimal placed
                            token.ThrowIfCancellationRequested();
                        }
                    }
                });
                //MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                //MessageBox.Show("Can't start background-counter");
                tb_counter.Text = String.Empty;
                tb_counter_ms.ReadOnly = false;
            }
        }
      

        double value = 0;
        double value2 = 0;

        private void btn_dac_Click(object sender, EventArgs e)
        {
            value = Convert.ToDouble(tb_dac.Text.Replace(",","."));
            LJM.eWriteName(handle_DAC, "TDAC0", value);
        }


        private void btn_ref_Click(object sender, EventArgs e)
        {
            value2 = Convert.ToDouble(tb_ref.Text.Replace(",", "."));
            LJM.eWriteName(handle_DAC2, "TDAC1", value2);
        }

    }
}


//bugs:
// - nach clear führt das abwählen von elementen zzu einem error (da ebtl. noch in liste gespeichert)
// - close iseg HV


//links:
// https://stackoverflow.com/questions/8000957/mouseenter-mouseleave-objectname
// https://stackoverflow.com/questions/14455293/how-and-when-to-use-async-and-await


// TODO:
/*** 
 * replace bachgroundworker with Task/async
 * replace/remove write_to_dps_async
 * key down kram
 * iseg terminal
 * open iseg devices (ip as string)
 * background counter catch (was wenn parse nicht geht? was wenn abbruch?)
 * counter labjack
 ***/
