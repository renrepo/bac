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
using ArduinoDriver;
using ArduinoUploader;


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

        HV_device DPS = new HV_device();
        HV_device H150666 = new HV_device();


        // IP adress Iseg-devices (ethernet connection)
        string ip_dps;
        string ip_xray;

        // General settings
        double V_photon;

        double E_HeI;               // Energy HeI-line
        double E_Al_Ka ;            // Energy Al K_alpha photon
        double E_Mg_Ka ;            // Energy Mg K_alpha photon
        double ri;                    // Radius inner hemisphere in mm
        //double ri = 92.075;
        double ra;                    // Radius outer hemisphere in mm
        //double ra = 111.125;
        double deviation = 0.001;            // maximim voltage deviation (in V) at the beginng of the voltage ramp
        string pin_pressure_ak = string.Empty;       // Analog Input Pin of Ionivac
        string pin_pressure_pk = string.Empty;
        string pin_pressure_sk = string.Empty;
        string pin_flow = string.Empty;
        string pin_hemo = string.Empty;
        string pin_hemi = string.Empty;
        string pin_analyser = string.Empty;
        double vchanneltron;          // voltage drop over channeltron
        double vor = 16;
        double nach = 5;
        double slope_korr = 0.613;      // ergibt sich aus Plot vhemi gegen vlens bei maxmalen zählraten
        double offset = - 48.85;
        double workfunction = 0;
        double v_stabi_volt;                     // to always habe approx. 1500V voltage drop over Z-diode circuitry
                                                 //double offset = 20;


        // Labjack stuff
        int handle_pressure_ak = 0;            // Labjack threads
        int handle_pressure_pk = 0; 
        int handle_pressure_sk = 0;            
        int handle_flow = 0;
        int handle_hemi = 0;
        int handle_hemo = 0;
        int handle_analyser = 0;
        int handle_count = 0;
        int handle_DAC = 0;
        int handle_DAC2 = 0;
        int handle_adc1 = 0;
        int handle_adc2 = 0;
        double ionivac_v_out = 0;           // Voltage of Ionivac output measured with Labjack device
        double cnt_before = 0;              // coutner reading befor and after delay
        double cnt_after = 0;
        double cnt_flow_before = 0;              // coutner reading befor and after delay
        double cnt_flow_after = 0;
        double intcounter = 0;
        double LJ_flow = 0;
        double LJ_hemi = 0;
        double LJ_hemo = 0;
        double LJ_lens = 0;
        double LJ_analyser2 = 0;            // voltages to be displayed
        double LJ_analyser = 0;
        double LJ_hemi2 = 0;
        double LJ_hemo2 = 0;
        double LJ_lens2 = 0;
        double adc1 = 0;
        double adc2 = 0;

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
        double v_stabi_min;                     // to always habe approx. 1500V voltage drop over Z-diode circuitry
        double v_stabi_max;
        double v_channeltron_out_max;
        double v_hemo_max;
        double v_hemi_max;
        double v_pass_meas = 0;


        // Emission current regulation


        string path_logfile;
        string path_binding_energies = Path.GetFullPath("Bindungsenergien.csv");
        string path_colors = Path.GetFullPath("colors2.csv");
        string path_electronconfig = Path.GetFullPath("electronconfiguration.csv");
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string curr_time;
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};       // name of energy-lines corresponding to electron binding energies in "Bindungsenergien.csv"
        List<List<string>> table_binding_energies = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
        List<string> display_labels = new List<string>();
        PointPairList values_to_plot = new PointPairList();
        Dictionary<string, string> binding_energies_dict = new Dictionary<string, string>();
        Dictionary<string, string> color_dict = new Dictionary<string, string>();
        Dictionary<string, string> dic = new Dictionary<string, string>();
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
        TextBox[] vm;
        System.Windows.Forms.Label[] lb_list_binding_energies;
        System.Windows.Forms.Label[] lb_list_orbital_structure;
        ManualResetEvent _suspend_background_measurement = new ManualResetEvent(true);
        private CancellationTokenSource _cts_pressure_labjack;           // Cancellation of Labjack pressure background measurement 
        private CancellationTokenSource _cts_flow_labjack;           // Cancellation of Labjack pressure background measurement 
        private CancellationTokenSource _cts_volt_dps;           // Cancellation of Iseg DPS voltage background measurement 
        private CancellationTokenSource _cts_counter_labjack;           // Cancellation of Labjack Counter background measurement 
        private CancellationTokenSource _cts_UPS;           // Cancellation of UPS spectra
        private CancellationTokenSource _cts_voltagemonitor;
        //private IMessageBasedSession DPS_HV;           // Iseg-HV session 6 Chanel HV
        //private IMessageBasedSession Xray_HV;        // Iseg X-Ray HV session


        bool labjack_connected = false;
        bool take_UPS_spec = false;     // true if UPS spectra is taken


        public XPS()
        {
            // dot instead of comma (very important for voltage input values!)
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            //try
            // {
            var dic = File.ReadAllLines("settings.txt")
              .Select(l => l.Split(new[] { '=' }))
              .ToDictionary(s => s[0].Trim(), s => s[1].Trim());
            //}
            //catch (Exception)
            //{
              //  AutoClosingMessageBox.Show("Can't read settings.txt", "Info", 2000);
                //XPS_FormClosing();
            //}


            Double.TryParse(dic["E_Photon_Aluminium_Ka_emission_line_[eV]"], out E_Al_Ka);
            Double.TryParse(dic["E_Photon_Magnesium_Ka_emission_line_[eV]"], out E_Mg_Ka);
            Double.TryParse(dic["E_Photon_HeI_emission_line_[eV]"], out E_HeI);
            Double.TryParse(dic["r_inner_hemisphere_[mm]"], out ri);
            Double.TryParse(dic["r_outer_hemisphere_[mm]"], out ra);
            Double.TryParse(dic["V_channeltron_voltagedrop_[V]"], out vchanneltron);
            Double.TryParse(dic["V_across_z-Diode_circuitry_[V]"], out v_stabi_volt);
            pin_pressure_ak = dic["LJ_T7_input_ADC_pressure_analyser_chamber"];
            pin_pressure_pk = dic["LJ_T7_input_ADC_pressure_analyser_chamber"];
            pin_pressure_sk = dic["LJ_T7_input_ADC_pressure_analyser_chamber"];
            pin_flow = dic["LJ_T7_input_ADC_flow_control"];
            pin_hemo = dic["LJ_T7_input_ADC_Hemo"];
            pin_hemi = dic["LJ_T7_input_ADC_Hemi"];
            pin_analyser = dic["LJ_T7_input_ADC_Analyser"];
            ip_dps = dic["IP_Iseg_DPS"];
            ip_xray = dic["IP_Iseg_H150666"];

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
            DPS.Is_session_open = false;
            H150666.Is_session_open = false;
            groupBox3.Enabled = false;
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
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_count);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_flow);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_hemi);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_hemo);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_v_analyser);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_v_lens);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_pressure_ak);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_DAC);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_DAC2);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_adc1);
                LJM.OpenS("T7", "Ethernet", "ANY", ref handle_adc2);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_schwelle);
                //LJM.OpenS("T7", "Ethernet", "ANY", ref handle_V_minus);

                // backgroundtask for pressure measurement at Ionivac-device mounted on analyser chamber.
                // pressure value updates every second
                background_meas_pressure_labjack();
                // cooling flow measurement, update every second
                background_meas_flow_labjack();
                //labjack_connected = true;
               //  LJM.eWriteName(handle_schwelle, "TDAC2", schwelle);
               // LJM.eWriteName(handle_DAC2, "TDAC3", V_minus);
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

            vm = new TextBox[] { vm1, vm2, vm3, vm4, vm5 };


            // Try to Open DPS and H150666 HV devices while startup of software
            // IMPORTANT: textboxes have to be created before éxecuting the code below!!!
            Iseg_DPS_session.Checked = !Iseg_DPS_session.Checked;
            Iseg_Xray_session.Checked = !Iseg_Xray_session.Checked;


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
                await DPS.channel_on(chanel);
                c.Text = "On";
                c.BackColor = Color.LimeGreen;
            }

            else
            {
                await DPS.channel_off(chanel);
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
            bool Vset = Double.TryParse(vset[chanel].Text.Replace(",","."), out double vset_in);
            vset[chanel].Text = vset_in.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture); //necessary because otherwise "vset_in" would bei "20\r\n40" if first 20 and then 40 typed in
            if (Vset)
            {
                try
                {
                    await DPS.set_voltage(vset_in,chanel);
                }
                catch (Exception)
                {
                }
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
            await DPS.channel_off(chanel);
            await DPS.reset_channels();
            vset[chanel].Text = string.Empty;
            vmeas[chanel].Text = string.Empty;
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
                        ya.Title.Text = string.Empty;
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
            if (lb_element_name.Text == string.Empty)
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
                    label.Text = string.Empty;
                }

                foreach (var label in lb_list_orbital_structure)
                {
                    label.Text = "--";
                }

                lb_element_name.Text = string.Empty;
                lb_atomic_number.Text = string.Empty;
            }
        }


        private async void Iseg_Xray_session_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            Cursor.Current = Cursors.WaitCursor;
            if (H150666.Is_session_open == false)
            {
                try
                {
                    await H150666.open_session("132.195.109.241");
                    c.Text = "Iseg Xray connected";
                    c.BackColor = Color.LightGreen;

                    if ((cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1) && DPS.Is_session_open == true)
                    {
                        btn_start.Enabled = true;
                    }

                    else
                    {
                        btn_start.Enabled = false;
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    H150666.Is_session_open = false;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }

            }

            else
            {
                try
                {
                    await H150666.reset_channels();
                    await H150666.clear();
                    await H150666.dispose();
                    H150666.Is_session_open = false;
                    c.Text = "Iseg Xray disconnected";
                    c.BackColor = Color.Silver;
                    btn_start.Enabled = false;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    H150666.Is_session_open = true;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }


        private async void Iseg_DPS_session_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            Cursor.Current = Cursors.WaitCursor;
            //if (c.Text == "Iseg DPS disconnected")
            if (DPS.Is_session_open == false)
                {

                try
                {
                    await DPS.open_session("132.195.109.144");
                    await DPS.check_dps();
                    await DPS.clear_emergency();
                    await DPS.voltage_ramp(4.0);

                    for (int i = 0; i < 6; i++)
                    {
                        reset[i].Enabled = true;
                        reload[i].Enabled = true;
                        stat[i].Enabled = true;
                    }
                    groupBox3.Enabled = true;
                    rs_all.Enabled = true;
                    queryButton.Enabled = true;
                    writeButton.Enabled = true;
                    clearButton.Enabled = true;
                    btn_emcy.Enabled = true;
                    background_meas_volt_DPS();
                    c.Text = "Iseg DPS connected";
                    c.BackColor = Color.LightGreen;

                    if ((cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1) && H150666.Is_session_open == true)
                    {
                        btn_start.Enabled = true;
                    }

                    else
                    {
                        btn_start.Enabled = false;
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    DPS.Is_session_open = false;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }

            else
            {
                try
                {
                    if (_cts_volt_dps != null)
                    {
                        _cts_volt_dps.Cancel();
                    }
                    _suspend_background_measurement.Set();
                    await DPS.reset_channels();
                    //DPS.clear();
                    await DPS.dispose();
                    DPS.Is_session_open = false;
                    btn_start.Enabled = false;
                    c.Text = "Iseg DPS disconnected";
                    c.BackColor = Color.Silver;
                }
                catch (Exception exp)
                {
                    MessageBox.Show(exp.Message);
                    DPS.Is_session_open = true;
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }


        private async void write_Click(object sender, EventArgs e)
        {
            _suspend_background_measurement.Reset();
            Cursor.Current = Cursors.WaitCursor;
            try
            {
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
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //readTextBox.Text = await read_from_Iseg(writeTextBox.Text + '\n', "DPS");
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



        double LJ_hemi_corr;
        double LJ_hemo_corr;
        double ticks;

        double v_hemi = 0;
        double v_hemo = 0;
        double v_analyser = 0;

        double k;
        double E_pass =15;
        double E_kin;
        double sc = 0;    

        private async void take_UPS_spectra()
        {
            _cts_UPS = new CancellationTokenSource();
            var token = _cts_UPS.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_counter.Text = value;
                vm1.Text = v_hemi.ToString("0.000");
                vm2.Text = v_hemo.ToString("0.000");
                //vm3.Text = (LJ_lens2 / 3).ToString("0.000");
                vm4.Text = v_analyser.ToString("0.000");
                vm5.Text = v_channeltron_out_min.ToString("0.000");
                values_to_plot.Add(E_kin, Convert.ToDouble((v_hemi - v_hemo) / k));
                myCurve.AddPoint(E_kin, Convert.ToDouble((v_hemi - v_hemo) / k));
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
            });
            var progress = progressHandler as IProgress<string>;


            // read in desired values for Passenergy, voltage bias, stepsize, time per step and lens voltage
            vpass = Convert.ToDouble(cb_pass.SelectedItem);
            vbias = Convert.ToDouble(cb_bias.SelectedItem);
            vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
            tcount = Convert.ToDouble(cb_counttime.SelectedItem);
            vLens = Convert.ToDouble(cb_v_lens.SelectedItem);

            btn_can.Enabled = true;

            myCurve = myPane.AddCurve("", values_to_plot, Color.Black, SymbolType.None);
            curr_time = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
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
                file.WriteLine("#E_k \t cps \t Ana \t Hemi \t Hemo \t EP \t Mt \t 0.4 \t corr");
                file.WriteLine("" + Environment.NewLine);
            }

            // E_Analyser = E_pass - E_Photon = (U_Analyser - U_bias)*e; Electrons with E=E_photon barely can reach the chaneltron
            // neglected the work function of the electron (which would add +V_work to v_analyser_min)
            v_analyser_min = (vpass - E_HeI + vbias);
            // corresponding minimum voltage of the outer/inner hemisphere; here "k" is estimated and yet not known exactly!
            v_hemo_min = (v_analyser_min - (vpass * k * 0.4));  
            v_hemi_min = (v_hemo_min + vpass * k );

            // Needed lens voltage unknown
            //vLens_min = .0;

            // even the slowest electron should now reach the chaneltron (E_Analyser = E_pass + (E_Spec - E_Probe) = (U_Analyser - U_bias)*e)
            v_analyser_max = vpass + vbias + 5;     // "5" takes (unknown) E_Spec - E_Probe into account
            v_hemo_max = (v_analyser_max - (vpass * k * 0.4)); 
            v_hemi_max = (v_analyser_max + vpass * k);

            // voltage drop over channeltron
            v_channeltron_out_min = v_analyser_min + vchanneltron;
            v_channeltron_out_max = v_analyser_max + vchanneltron;

            token.ThrowIfCancellationRequested();


            // set initial voltages (roughly)
            LJM.eWriteName(handle_DAC, "TDAC0", v_hemi_min/5.0);
            LJM.eWriteName(handle_DAC2, "TDAC1", v_hemo_min / 5.09577);
            //_suspend_background_measurement.Reset();
            //await write_to_Iseg(String.Format(":VOLT {0},(@4)\n", v_channeltron_out_min.ToString("0.000")), "DPS");
            //await write_to_Iseg(String.Format(":VOLT ON,(@4)\n"), "DPS");
            //_suspend_background_measurement.Set();
            Thread.Sleep(30);
            LJM.eReadName(handle_DAC, "AIN1", ref LJ_hemi);
            double dev_hemi = LJ_hemi - v_hemi_min / 5.03054;
            LJ_hemi_corr = LJ_hemi;

            while (Math.Abs(dev_hemi) > deviation)
            {
                LJ_hemi_corr = LJ_hemi_corr - dev_hemi;
                LJM.eWriteName(handle_DAC, "TDAC0", LJ_hemi_corr);
                await Task.Delay(10);
                LJM.eReadName(handle_hemi, pin_hemi, ref LJ_hemi);
                dev_hemi = LJ_hemi - v_hemi_min / 5.03054;
            }

            LJM.eReadName(handle_DAC2, "AIN2", ref LJ_hemo);
            double dev_hemo = LJ_hemo - v_hemo_min / 5.03318;
            LJ_hemo_corr = LJ_hemo;

            while (Math.Abs(dev_hemo) > deviation)
            {
                LJ_hemo_corr = LJ_hemo_corr - dev_hemo;
                LJM.eWriteName(handle_DAC2, "TDAC1", LJ_hemo_corr);
                await Task.Delay(10);
                LJM.eReadName(handle_hemo, pin_hemo, ref LJ_hemo);
                dev_hemo = LJ_hemo - v_hemo_min / 5.03318;
            }

            token.ThrowIfCancellationRequested();

            btn_start.Enabled = false;
            btn_can.Enabled = true;
            tb_show.Enabled = true;
            tb_safe.Enabled = false;

            Stopwatch sw = new Stopwatch();
            LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
            LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);

            double integrated_LJ_hemi = 0;
            double integrated_LJ_hemo = 0;
            double integrated_LJ_analyser = 0;

            double elapsed_seconds = 0;
            double counts = 0;

            int inc = 1;

            double corr = 0;

            token.ThrowIfCancellationRequested();

            try
            {
                await Task.Run(() =>
                {
                    while (v_hemi < v_hemi_max)
                    {
                        integrated_LJ_hemi = 0;
                        integrated_LJ_hemo = 0;
                        integrated_LJ_analyser = 0;
                        int num_meas = 0;
                        token.ThrowIfCancellationRequested();

                        while (num_meas <= 4)
                        {
                            LJM.eReadName(handle_hemi, pin_hemi, ref LJ_hemi);
                            integrated_LJ_hemi += LJ_hemi * 5.03054;
                            LJM.eReadName(handle_hemo, pin_hemo, ref LJ_hemo);
                            integrated_LJ_hemo += LJ_hemo * 5.03318;
                            LJM.eReadName(handle_analyser, pin_analyser, ref LJ_analyser);
                            integrated_LJ_analyser += LJ_analyser * 5.03182;
                            ++num_meas;
                        }

                        sw.Start();
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                        sc = intcounter;

                        v_hemi = integrated_LJ_hemi / num_meas;
                        v_hemo = integrated_LJ_hemo / num_meas;
                        v_analyser = integrated_LJ_analyser / num_meas;
                        E_pass = (v_hemi - v_hemo) / k;
                        corr += E_pass - vpass;

                        while (sw.ElapsedMilliseconds < tcount)
                        {
                            Thread.Sleep(1);
                        }
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                        ticks = sw.ElapsedTicks;
                        sw.Reset();

                        //##########################################################################################################################################################
                        //handle DAC und DAC2 als einen??
                        elapsed_seconds = ticks / Stopwatch.Frequency;
                        LJM.eWriteName(handle_DAC, "TDAC0", LJ_hemi_corr + vstepsize * inc / 5.09577 / 1000 - corr * k / 5.09577 / 2.0);
                        LJM.eWriteName(handle_DAC2, "TDAC1", LJ_hemo_corr + vstepsize * inc / 5.09577 / 1000 + corr * k / 5.09577 / 2.0);
                        ++inc;
                        //counts = (intcounter - sc) / elapsed_seconds;
                        counts = elapsed_seconds*1000;
                        // because (V_analyser - V_bias)*e + E_kin - workfunction = E_pass
                        E_kin = E_pass - v_analyser + vbias + workfunction;

                        using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                        {
                            file.WriteLine(
                                E_kin.ToString("0.000") + "\t" +
                                counts.ToString("000000") + "\t" +
                                v_analyser.ToString("0.000") + "\t" +
                                v_hemi.ToString("0.000") + "\t" +
                                v_hemo.ToString("0.000") + "\t" +
                                E_pass.ToString("0.000") + "\t" +
                                (elapsed_seconds * 1000).ToString("000") + "\t" +
                                ((v_hemi-v_analyser)/(v_hemi - v_hemo)).ToString("0.000") + "\t" +
                                corr.ToString("0.00")
                                );
                        }
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
                        progress.Report(counts.ToString("000000"));
                        // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                        //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                    }
                });
                LJM.eWriteName(handle_DAC, "TDAC0", 0.0);
                LJM.eWriteName(handle_DAC2, "TDAC1", 0.0);
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                showdata.Enabled = true;
                _suspend_background_measurement.Reset();
                await DPS.channel_off(4);
                await DPS.reset_channels();

                _suspend_background_measurement.Set();
            }
            catch (OperationCanceledException)
            {
                _suspend_background_measurement.Reset();
                await DPS.channel_off(4);
                await DPS.reset_channels();
                _suspend_background_measurement.Set();
                LJM.eWriteName(handle_DAC, "TDAC0", 0.0);
                LJM.eWriteName(handle_DAC2, "TDAC1", 0.0);
                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
                showdata.Enabled = true;
                fig_name.Enabled = true;
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                //AutoClosingMessageBox.Show("Problem with background measurement of Iseg DPS voltages", "Info", 500);
            }
        }


        private void btn_can_Click(object sender, EventArgs e)
        {
            if (_cts_UPS != null)
            {
                _cts_UPS.Cancel();
            }
        }


        private void btn_start_Click(object sender, EventArgs e)
        {
            if ((cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1) && DPS.Is_session_open == true && H150666.Is_session_open == true)
            {
                take_XPS_spectra();
            }

            if (cb_select.SelectedIndex == 2)
            {
                take_UPS_spectra();
            }
        }


        private void btn_clear_Click(object sender, EventArgs e)
        {
        take_UPS_spec = false;
        foreach (var item in vm)
        {
            item.Text = String.Empty;
        }
        tb_show.Text = string.Empty;
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
            if (fig_name.Text == string.Empty)
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
                bool Vset = Double.TryParse(vset[i].Text.Replace(',', '.'), out double vset_in);
                vset[i].Text = vset_in.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture);
                if (Vset)
                {    
                    await DPS.set_voltage(vset_in,i);
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
            await DPS.reset_channels();
            for (int i = 0; i <= 5; i++)
            {
                vset[i].Text = string.Empty;
                vmeas[i].Text = string.Empty;
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
                await DPS.channel_off(i);
                stat[i].Text = "Off";
                stat[i].BackColor = SystemColors.ControlLightLight;
            }
            _suspend_background_measurement.Set();
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

            if (_cts_UPS != null)
            {
                _cts_UPS.Cancel();
            }

            if (DPS.Is_session_open == true)
            {
                await DPS.reset_channels();
                await DPS.dispose();
            }

            if (H150666.Is_session_open == true)
            {
                await H150666.reset_channels();
                await H150666.dispose();
            }

            if (labjack_connected)
            {
                LJM.CloseAll();
            }
            Thread.Sleep(1000);
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
            await H150666.emergency_off(1);
            await H150666.emergency_off(2);

            for (int i = 0; i < 6; i++)
            {
                await DPS.emergency_off(i);
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
                background_counter_labjack();
            }

            else
            {
                if (_cts_counter_labjack != null)
                {
                    _cts_counter_labjack.Cancel();
                }
                c.Text = "Off";
                c.BackColor = SystemColors.ControlLightLight;
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
                int ct = int.Parse(tb_counter_ms.Text);
                double erg = 0;
                Stopwatch sw = new Stopwatch();
                cb_counter.Text = "On";
                cb_counter.BackColor = Color.LightGreen;
                tb_counter_ms.ReadOnly = true;
                await Task.Run(() =>
                {
                    while (true)
                    {
                        LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
                        LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
                        sw.Start();
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref cnt_before);
                        Thread.Sleep(ct);
                        sw.Stop();
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                        erg = (cnt_after - cnt_before) / sw.Elapsed.TotalSeconds;
                        sw.Reset();
                        if (progress != null)
                        {
                            progress.Report(erg.ToString("N0"));    //no decimal placed
                            token.ThrowIfCancellationRequested();
                        }
                    }
                });
                //MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                //AutoClosingMessageBox.Show("Switched off counter!", "Info", 500);
                tb_counter.Text = String.Empty;
                tb_counter_ms.ReadOnly = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Type in Integer");
            }
        }



        //#########################################################################################################################
        // TEST PANEL
        private void btn_dac_Click(object sender, EventArgs e)
        {
            double value = Convert.ToDouble(tb_dac.Text.Replace(",","."));
           // LJM.eWriteName(handle_DAC, "TDAC0", value);
            LJM.eWriteName(handle_DAC, "DAC0", value);
        }

        private void btn_ref_Click(object sender, EventArgs e)
        {
            while (true)
            {
                double value2 = Convert.ToDouble(tb_ref.Text.Replace(",", "."));
                LJM.eWriteName(handle_DAC2, "TDAC1", value2);
                //LJM.eWriteName(handle_schwelle, "TDAC2", 0.0);
                //LJM.eWriteName(handle_schwelle, "TDAC2", 0.3);
            }

        }


        private void btn_stop_adc_Click(object sender, EventArgs e)
        {
            if (_cts_voltagemonitor != null)
            {
                _cts_voltagemonitor.Cancel();
            }
        }


        private async void btn_read_adc2_Click(object sender, EventArgs e)
        {
            LJM.eWriteName(handle_adc1, "AIN2_RESOLUTION_INDEX",8);
            LJM.eWriteName(handle_adc2, "AIN0_RESOLUTION_INDEX", 8);
            Thread.Sleep(10);
            double adc_cum_1 = 0;
            double adc_cum_2 = 0;
            double time = 0;
            double erg1 = 0; // Hemo
            double erg2 = 0; // Hemi
            int j = 0;

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + "ADC" + "\\"));
            string path_logfile = dl.FullName;

            using (var file = new StreamWriter(path_logfile + name + ".txt", true))
            {
                file.WriteLine(
                    "#Hemo" + "\t" + "Hemi" + "\t" + "Diff" + "\t" + "time[ms]" + "\n"
                    );
            }

            _cts_voltagemonitor = new CancellationTokenSource();
            var token = _cts_voltagemonitor.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_adc2.Text = value;
            });
            var progress = progressHandler as IProgress<string>;
            try
            {
                int ct = int.Parse(tb_counter_ms.Text);
                Stopwatch sw = new Stopwatch();
                cb_counter.Text = "On";
                cb_counter.BackColor = Color.LightGreen;
                tb_counter_ms.ReadOnly = true;
                await Task.Run(() =>
                {
                    while (true)
                    {
                        adc_cum_1 = 0;
                        adc_cum_2 = 0;
                        sw.Start();
                        while(j < 4)
                        {
                            LJM.eReadName(handle_adc1, "AIN2", ref adc1);
                            LJM.eReadName(handle_adc2, "AIN0", ref adc2);
                            adc_cum_1 += adc1;
                            adc_cum_2 += adc2;
                            j += 1;
                        }
                        j = 0;
                        //tb_adc2.Text = (adc_cum * 56.55 / 4).ToString("0.000");
                        //sw.Stop();
                        time = sw.Elapsed.TotalSeconds;
                        //time = sw.Elapsed.Milliseconds;
                        //sw.Reset();
                        //erg = adc_cum * 152.4 / 4;
                        erg1 = adc_cum_1 * 153.05 / 4;
                        erg2 = adc_cum_2 * 153.11 / 4;
                        using (var file = new StreamWriter(path_logfile + name + ".txt", true))
                        {
                            file.WriteLine(
                                erg1.ToString("0000.00") + "\t" +
                                erg2.ToString("0000.00") + "\t" +
                                (erg2-erg1).ToString("00.000") + "\t" +
                                time.ToString("0000.0000") + "\n"
                                );
                        }
                        if (progress != null)
                        {
                            //progress.Report(erg.ToString("0000.00"));
                            progress.Report(erg1.ToString("0000.0"));
                            token.ThrowIfCancellationRequested();
                        }
                        sw.Reset();
                    }
                });
                //MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                //AutoClosingMessageBox.Show("Switched off counter!", "Info", 500);
                tb_adc2.Text = String.Empty;
                sw.Stop();
                sw.Reset();
            }
            catch (Exception)
            {
                MessageBox.Show("Type in Integer");
            }
        }


        private async void btn_rampe_Click(object sender, EventArgs e)
        {
            LJM.eWriteName(handle_DAC2, "TDAC1", 0.02);

            for (int i = 0; i < 8000; i++)
            {
                LJM.eWriteName(handle_DAC, "TDAC0", i * 1.0 / 1000.0);
                LJM.eWriteName(handle_DAC, "TDAC1", 1.0);
                tb_rampe.Text = (i * 1.0 / 1000.0).ToString("0.000");
                await Task.Delay(500);
                LJM.eReadName(handle_hemi, pin_hemi, ref LJ_hemi);
                LJM.eReadName(handle_hemo, pin_hemo, ref LJ_hemo);
                tb_hem_in.Text = (LJ_hemi*5.03318).ToString("0.000");
                tb_hem_out.Text = (LJ_hemo*5.03054).ToString("0.000");
                await Task.Delay(500);
            }
        }


        Stopwatch sw = new Stopwatch();
        string name = "test";
        int sleeptime = 200;

        private void btn_st_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + "2MHz" + "\\"));
            string path_logfile = dl.FullName;

            using (var file = new StreamWriter(path_logfile + name + ".txt", true))
            {
                file.WriteLine(
                    "#counts" + "\n"
                    );
            }

            double erg = 0;
            double time = 0;

            for (int i = 0; i < 10000; i++)
            {
                LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
                LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);
                sw.Start();
                LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref cnt_before);
                Thread.Sleep(sleeptime);
                sw.Stop();
                LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                time = sw.Elapsed.TotalSeconds;
                erg = (cnt_after - cnt_before) / time;
                //erg = i;
                sw.Reset();
                using (var file = new StreamWriter(path_logfile + name + ".txt", true))
                {
                    file.WriteLine(
                        erg.ToString("0000000") + "\t" +
                        time.ToString("0.000000") + "\n"
                        );
                }
            }
        }



        // X P S #################################### X P S ##################################### X P S #################################### X P S ##############


        private void cb_select_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cb_select.SelectedIndex == 0)
            {
                cb_select.BackColor = System.Drawing.Color.MediumSpringGreen;
                V_photon = E_Al_Ka;

                if (DPS.Is_session_open == true & H150666.Is_session_open == true)
                {
                    btn_start.Enabled = true;
                }

                else
                {
                    btn_start.Enabled = false;
                }
            }

            if (cb_select.SelectedIndex == 1)
            {
                cb_select.BackColor = System.Drawing.Color.MediumSpringGreen;
                V_photon = E_Mg_Ka;

                if (DPS.Is_session_open == true & H150666.Is_session_open == true)
                {
                    btn_start.Enabled = true;
                }

                else
                {
                    btn_start.Enabled = false;
                }
            }

            if (cb_select.SelectedIndex == 2)
            {
                cb_select.BackColor = System.Drawing.Color.MediumSpringGreen;
                V_photon = E_HeI;

                if (DPS.Is_session_open)
                {
                    btn_start.Enabled = true;
                }

                else
                {
                    btn_start.Enabled = false;
                }
            }
        }

        private async void take_XPS_spectra()
        {
            _cts_UPS = new CancellationTokenSource();
            var token = _cts_UPS.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_counter.Text = value;
                vm1.Text = v_hemi.ToString("0.0");
                vm2.Text = v_hemo.ToString("0.0");
                //vm3.Text = (LJ_lens2 / 3).ToString("0.000");
                vm4.Text = v_analyser.ToString("0.0");
                //vm5.Text = v_channeltron_out_min.ToString("0.000");
                values_to_plot.Add(E_kin, Convert.ToDouble(E_pass));
                myCurve.AddPoint(E_kin, Convert.ToDouble(E_pass));
                //values_to_plot.Add(ctner, Convert.ToDouble(value));
                //myCurve.AddPoint(ctner, Convert.ToDouble(value));
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
            });
            var progress = progressHandler as IProgress<string>;


            // read in desired values for Passenergy, voltage bias, stepsize, time per step and lens voltage
            vpass = Convert.ToDouble(cb_pass.SelectedItem);
            vbias = Convert.ToDouble(cb_bias.SelectedItem);
            //vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
            tcount = Convert.ToDouble(cb_counttime.SelectedItem);
            vLens = Convert.ToDouble(cb_v_lens.SelectedItem);

            btn_can.Enabled = true;

            myCurve = myPane.AddCurve("", values_to_plot, Color.Black, SymbolType.None);
            curr_time = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
            string u = tb_safe.Text + curr_time;
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + curr_time + "_" + tb_safe.Text + "\\"));
            path_logfile = dl.FullName;
            using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
            {
                file.WriteLine("#XPS-spectrum" + Environment.NewLine);
                file.WriteLine("#Date/time: \t{0}", DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss"));
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("#AK pressure: \t{0} \t{1}", tb_pressure.Text, "mbar");
                file.WriteLine("#Pass energy: \t{0} \t{1}", vpass.ToString("0.0"), "eV");
                file.WriteLine("#Volt. bias: \t{0} \t{1}", vbias.ToString("0.0"), "V");
                file.WriteLine("#Volt. lens: \t{0} \t{1}", vLens.ToString("0.0"), "V");
                //file.WriteLine("#Step width: \t{0} \t{1}", vstepsize.ToString("0.0"), "meV");
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
                file.WriteLine("#E_k \t cps \t Analyser \t V_hemi \t V_hemo \t E_Pass \t t_meas");
                file.WriteLine("" + Environment.NewLine);
            }

            // E_Analyser = E_pass - E_Photon = (U_Analyser - U_bias)*e; Electrons with E=E_photon barely can reach the chaneltron
            // neglected the work function of the electron (which would add +V_work to v_analyser_min)
            v_analyser_min = (vpass - V_photon + vbias) -50 ; //-50V extra
            // corresponding minimum voltage of the outer/inner hemisphere; here "k" is estimated and yet not known exactly!
            v_hemo_min = (v_analyser_min - (vpass * k * 0.4));
            v_stabi_min = v_hemo_min + v_stabi_volt;
            //v_hemi_min = (v_hemo_min + vpass * k);

            // Needed lens voltage unknown
            //vLens_min = .0;

            // even the slowest electron should now reach the chaneltron (E_Analyser = E_pass + (E_Spec - E_Probe) = (U_Analyser - U_bias)*e)
            v_analyser_max = vpass + vbias + 50;     // "5" takes (unknown) E_Spec - E_Probe into account
            v_hemo_max = (v_analyser_max - (vpass * k * 0.4));
            v_stabi_max = v_hemo_max + v_stabi_volt;
            //v_hemi_max = (v_analyser_max + vpass * k);

            // voltage drop over channeltron
            v_channeltron_out_min = v_analyser_min + vchanneltron;
            v_channeltron_out_max = v_analyser_max + vchanneltron;

            token.ThrowIfCancellationRequested();

            await DPS.voltage_ramp(5);
            await DPS.set_voltage(v_channeltron_out_min,4);
            await DPS.set_voltage(v_hemo_min,1);
            await DPS.set_voltage(v_stabi_min,5);
            await DPS.channel_on(4);
            await DPS.channel_on(1);
            await DPS.channel_on(5);

            await Task.Delay(30000);

            await DPS.voltage_ramp(0.1);




            token.ThrowIfCancellationRequested();

            btn_start.Enabled = false;
            btn_can.Enabled = true;
            tb_show.Enabled = true;
            tb_safe.Enabled = false;

            Stopwatch sw = new Stopwatch();
            LJM.eWriteName(handle_count, "DIO18_EF_INDEX", 7);
            LJM.eWriteName(handle_count, "DIO18_EF_ENABLE", 1);

            double integrated_LJ_hemo = 0;
            double integrated_LJ_hemi = 0;

            double elapsed_seconds = 0;
            double counts = 0;
            double LJ_old_hemo = 0;
            double LJ_old_hemi = 0;

            token.ThrowIfCancellationRequested();

            try
            {
                await DPS.set_voltage(v_hemo_max,1);
                await DPS.set_voltage(v_stabi_max, 5);
                await DPS.set_voltage(v_channeltron_out_max, 4);

                LJM.eReadName(handle_hemo, pin_hemo, ref LJ_hemo);
                
                v_hemo = LJ_hemo * 153.05;
                

                await Task.Run(() =>
                {
                    while (v_hemo < v_hemo_max)
                    {                                           
                        integrated_LJ_hemo = 0;
                        integrated_LJ_hemi = 0;
                        int num_meas = 0;
                        token.ThrowIfCancellationRequested();

                        
                        while (num_meas < 4)
                        {
                            LJM.eReadName(handle_hemo, pin_hemo, ref LJ_hemo);
                            integrated_LJ_hemo += LJ_hemo * 153.05;
                            LJM.eReadName(handle_hemi, pin_hemi, ref LJ_hemi);
                            integrated_LJ_hemi += LJ_hemi * 153.11;
                            ++num_meas;
                        }

                        LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                        
                        sw.Start(); // start/stop stopwatch before or after Labjack-readout!
                        sc = intcounter;
                        v_hemo = (integrated_LJ_hemo + LJ_old_hemo)/ (2*num_meas);
                        v_hemi = (integrated_LJ_hemi + LJ_old_hemi)/ (2 * num_meas);
                        E_pass = (v_hemi - v_hemo) / k;
                        v_pass_meas = (v_hemi - v_hemo);
                        v_analyser = v_hemo + v_pass_meas * 0.4;
                        LJ_old_hemo = integrated_LJ_hemo;
                        LJ_old_hemi = integrated_LJ_hemi;

                        // because (V_analyser - V_bias)*e + E_kin - workfunction = E_pass
                        E_kin = E_pass - v_analyser + vbias + workfunction;

                        while (sw.ElapsedMilliseconds < tcount)
                        {
                            Thread.Sleep(1);
                        }
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A", ref intcounter);
                        ticks = sw.ElapsedTicks;
                        sw.Reset();

                        //##########################################################################################################################################################
                        //handle DAC und DAC2 als einen??
                        elapsed_seconds = ticks / Stopwatch.Frequency;

                        counts = (intcounter - sc) / elapsed_seconds;
                        //counts = elapsed_seconds * 1000;

                        using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                        {
                            file.WriteLine(
                                E_kin.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                counts.ToString("000000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                v_analyser.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                v_hemi.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                v_hemo.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                E_pass.ToString("000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                v_pass_meas.ToString("000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                (elapsed_seconds * 1000).ToString("000", System.Globalization.CultureInfo.InvariantCulture)
                                );
                        }
                        LJM.eReadName(handle_count, "DIO18_EF_READ_A_AND_RESET", ref intcounter);
                        progress.Report(counts.ToString("000000"));
                        //progress.Report((elapsed_seconds * 10000).ToString("000000"));
                        // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                        //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                    }
                });
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                showdata.Enabled = true;
                await DPS.channel_off(4);
                await DPS.reset_channels();
            }
            catch (OperationCanceledException)
            {
                await DPS.voltage_ramp(5);
                await DPS.channel_off(4);
                await DPS.channel_off(1);
                await DPS.channel_off(5);

                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
                showdata.Enabled = true;
                fig_name.Enabled = true;
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                //AutoClosingMessageBox.Show("Problem with background measurement of Iseg DPS voltages", "Info", 500);
            }
        }

        private async void btn_hv_Click(object sender, EventArgs e)
        {
            await H150666.set_current(0.5,1);
            await H150666.channel_on(1);
        }

        private async void btn_hv_2_Click(object sender, EventArgs e)
        {
            //await write_to_Iseg(String.Format(":VOLT 600.89, (@0)\n"), "XRAY");
            await H150666.channel_off(1);
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
--- read_raw_sync zweifach
 ***/