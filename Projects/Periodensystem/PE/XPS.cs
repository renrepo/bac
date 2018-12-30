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
//using System.ComponentModel;
using System.Data;
using System.Linq;
//using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using ZedGraph;
//using System.Globalization;
using System.Diagnostics;
//using NationalInstruments.Visa;
using LabJack;
//using Ivi.Visa;
//using ArduinoDriver;
//using ArduinoUploader;



namespace XPS
{
    public partial class XPS : Form
    {

        HV_device DPS = new HV_device();
        HV_device H150666 = new HV_device();


        // IP adress Iseg-devices (ethernet connection)
        string ip_dps;
        string ip_xray;

        string LJM_connection_type = "ANY";

        // General settings
        double V_photon;
        double E_HeI;               // Energy HeI-line
        double E_Al_Ka;            // Energy Al K_alpha photon
        double E_Mg_Ka;            // Energy Mg K_alpha photon
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
        double offset = -48.85;
        //double workfunction = 4.8;
        double workfunction = 4.701;
        double v_stabi_volt;                     // to always habe approx. 1500V voltage drop over Z-diode circuitry
                                                 //double offset = 20;
        double correction_offset = 0;        // correction factor vor pass energy

        //[ 0.8544416  14.47170123  4.70083337  0.99564488]
        double voltage_divider = 153.612 * 0.99564488;
        double k = 0.85444;

        int handle_stream = 0;
        // Labjack stuff           // Labjack threads
        //int handle_pressure_pk = 0;
        //int handle_pressure_sk = 0;
        int handle_analyser = 0;
        int handle_DAC = 0;
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
        double E_B;


        // Emission current regulation


        string path_logfile;

        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string curr_time;
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};       // name of energy-lines corresponding to electron binding energies in "Bindungsenergien.csv"

        TextBox[] vset;
        TextBox[] vmeas;
        TextBox[] vmeas2;
        Button[] reload;
        Button[] reset;
        CheckBox[] stat;
        TextBox[] vm;
        TextBox[] pwm_tb;

        ManualResetEvent _suspend_background_measurement = new ManualResetEvent(true);
        private CancellationTokenSource _cts_pressure_labjack;           // Cancellation of Labjack pressure background measurement 
        private CancellationTokenSource _cts_flow_labjack;           // Cancellation of Labjack pressure background measurement 
        private CancellationTokenSource _cts_volt_dps;           // Cancellation of Iseg DPS voltage background measurement 
        private CancellationTokenSource _cts_counter_labjack;           // Cancellation of Labjack Counter background measurement 
        private CancellationTokenSource _cts_UPS;           // Cancellation of UPS spectra
        private CancellationTokenSource _cts_XPS;
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
            lb_list_binding_energies = new System.Windows.Forms.Label[] {label4,label6,label8,label10,label12,label14,label16,label18,label20,label22,
                                                   label24,label26,label28,label30,label32,label34,label36,label38,label40,label42,
                                                    label44,label46,label48,label50};
            /***
            lb_list_orbital_structure = new System.Windows.Forms.Label[] {s1,s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16,s17,s18,s19,s20,s21,s22,
                                                   s23,s24,s25,s26,s27,s28,s29,s30,s31,s32,s33,s34,s35,s36,s37,s38,s39,s40,s41,s42,
                                                   s43,s44,s45,s46,s47,s48,s49,s50,s51,s52,s53,s54,s55,s56,s57,s58,s59};
            ***/


        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //LJM.eStreamStop(handle_stream);
                LJM.CloseAll();
            }
            catch (Exception)
            {
            }

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
            //elec_bind = File.ReadAllLines(path_electronconfig).Select(l => l.Split(',').ToList()).ToList();
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
                //LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_count);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_flow);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_hemi);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_hemo);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_pressure_ak);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_stream);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_DAC2);
                //LJM.OpenS("T7", "ANY", "ANY", ref handle_PWM);

                // backgroundtask for pressure measurement at Ionivac-device mounted on analyser chamber.
                // pressure value updates every second
                background_meas_pressure_labjack();
                // cooling flow measurement, update every second
                background_meas_flow_labjack();
            }

            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't start background measurements device!", "Info", 500);
            }



            //buttons for interactive periodic table
            Button[] but = {H ,He, Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar, K, Ca, Sc,
                             Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr, Rb, Sr, Y, Zr,
                             Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe, Cs, Ba, La, Hf, Ta,
                             W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn, Fr, Ra, Ac, Ce, Pr, Nd,
                             Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Th, Pa, U, Rf, Np, Pu, Am, Cm,
                             Bk, Cf, Es, Fm, Md, No, Lr};



            // click-event for buttons in the periodic table will call the "global_element_click"-method
            foreach (var item in but)
            {
                item.MouseDown += global_element_click;
            }



            // default values for pass-energy, bias-voltage,... shown in the "XPS and UPS settings"
            cb_pass.SelectedIndex = 3;
            cb_bias.SelectedIndex = 0;
            cb_select.SelectedIndex = 0;
            //cb_counttime.SelectedIndex = 1;
            cb_samp_ev.SelectedIndex = 4;
            cb_DAC.SelectedIndex = 0;

            // proportionality between the voltage applied to the hemispheres and the pass energy
            //k = ra / ri - ri / ra;
            //k = 0.673;
            //k = 0.6;
            //k = 0.8;
            pwm_tb = new TextBox[] {tb_roll, tb_divisor};
            foreach (var item in pwm_tb)
            {
                //item.KeyDown += global_pwm_down;
                item.KeyDown += new KeyEventHandler(global_pwm_down);
            }
        }

        //##################################################################################################################################################################



        private async void Init_DPS_control_panel()
        {
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
            //Iseg_DPS_session.Checked = !Iseg_DPS_session.Checked;
            //Iseg_Xray_session.Checked = !Iseg_Xray_session.Checked;

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
        }



        // switch on/off one of the six Iseg DPS HV-modules in "Iseg Control" tab
        private async void Global_DPS_on_off_switch(object sender, MouseEventArgs e)
        {
            CheckBox c = sender as CheckBox;
            int chanel = Convert.ToInt16(c.Tag);
            _suspend_background_measurement.Reset();

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
            bool Vset = Double.TryParse(vset[chanel].Text.Replace(",", "."), out double vset_in);
            vset[chanel].Text = vset_in.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture); //necessary because otherwise "vset_in" would bei "20\r\n40" if first 20 and then 40 typed in
            if (Vset)
            {
                try
                {
                    await DPS.set_voltage(vset_in, chanel);
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





        private async void Iseg_Xray_session_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox c = sender as CheckBox;
            Cursor.Current = Cursors.WaitCursor;
            if (H150666.Is_session_open == false)
            {
                try
                {
                    await H150666.open_session("132.195.109.241");
                    await H150666.set_current(0, 1);
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
                Init_DPS_control_panel();

                try
                {
                    await DPS.open_session("132.195.109.144");
                    await DPS.check_dps();
                    await DPS.clear_emergency();
                    //await DPS.set_vnom(1000,1);
                    await DPS.voltage_ramp(4.0);

                    for (int i = 0; i < 6; i++)
                    {
                        reset[i].Enabled = true;
                        reload[i].Enabled = true;
                        stat[i].Enabled = true;
                    }
                    groupBox3.Enabled = true;
                    rs_all.Enabled = true;
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




        //###########################################################################################################################################



        private async void btn_can_Click(object sender, EventArgs e)
        {
            if (_cts_UPS != null)
            {
                _cts_UPS.Cancel();
            }
            await DPS.reset_channels();
        }


        private void btn_start_Click(object sender, EventArgs e)
        {
            if ((cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1) && DPS.Is_session_open == true && H150666.Is_session_open == true)
            {
                take_XPS_spectra();
            }

            if (cb_select.SelectedIndex == 2)
            {
                //take_UPS_spectra();
            }
        }


        private void btn_clear_Click(object sender, EventArgs e)
        {
            //take_UPS_spec = false;
            foreach (var item in vm)
            {
                item.Text = String.Empty;
            }
            tb_show.Text = string.Empty;
            //lb_perc_gauss.Text = "%";
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
            values_to_plot_svg.Clear();
            values_to_plot_svg_deriv.Clear();
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
                    await DPS.set_voltage(vset_in, i);
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

            if (_cts_XPS != null)
            {
                _cts_XPS.Cancel();
                //LJM.eStreamStop(handle_stream);
                //LJM.Close(handle_stream);
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
        
            try
            {
                LJM.CloseAll();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't close Labjack device","LJM Closing Error",1000);
            }

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




        //#########################################################################################################################
        // TEST PANEL
        private void btn_dac_Click(object sender, EventArgs e)
        {
            try
            {
                //LJM.eWriteName(handle_stream, "DAC0", 1.0020000);
                //LJMError = LJM.eStreamStop(handle_stream);

            }
            catch (Exception)
            {
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
            Queue<double> filt_values = new Queue<double>();

            double[] coeff = {0.0447205 , -0.02484472, -0.05001635, -0.04315136, -0.01515297,
        0.02452935,  0.06789993,  0.10841682,  0.14099187,  0.16199065,
        0.16923254,  0.16199065,  0.14099187,  0.10841682,  0.06789993,
        0.02452935, -0.01515297, -0.04315136, -0.05001635, -0.02484472,
        0.0447205};
            double[] coeff_deriv1 = {2.31350775e-02,  2.76052450e-03, -1.19105405e-02, -2.15118384e-02,
       -2.66770902e-02, -2.80400166e-02, -2.62343386e-02, -2.18937770e-02,
       -1.56520528e-02, -8.14288686e-03,  7.87341175e-18,  8.14288686e-03,
        1.56520528e-02,  2.18937770e-02,  2.62343386e-02,  2.80400166e-02,
        2.66770902e-02,  2.15118384e-02,  1.19105405e-02, -2.76052450e-03,
       -2.31350775e-02};

            double E_pass = 0;
            double v_analyser = 0;
            double E_kin = 0;
            int handle_stream = 0;
            int handle_DAC2 = 0;

            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_stream);
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_DAC2);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_stream' or 'handle_DAC2' session!", "Info", 500);
            }

            LJM.eWriteName(handle_DAC2, "DAC1", 4.0000);

            double mean_volt_hemo = 0;
            _cts_XPS = new CancellationTokenSource();
            var token = _cts_XPS.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_cps.Text = value;
                try
                {
                    progressBar1.Value = Convert.ToInt32(timee);
                    lb_progress.Text = progressBar1.Value.ToString("0") + '%';
                }
                catch (Exception)
                {
                }
                //vm1.Text = ch1_meas.Text;
                //vm2.Text = (Convert.ToDouble(ch1_meas.Text) + vpass).ToString("0.0");
                //vm3.Text = ch3_meas.Text;
                //vm4.Text = ch6_meas.Text;
                //vm5.Text = ch5_meas.Text;
                //values_to_plot.Add(E_kin, Convert.ToDouble(E_pass));
                //myCurve.AddPoint(E_kin, Convert.ToDouble(E_pass));
                //values_to_plot.Add(ctner, Convert.ToDouble(value));
                //myCurve.AddPoint(ctner, Convert.ToDouble(value));
                //zedGraphControl1.Invalidate();
                //zedGraphControl1.AxisChange();
            });
            var progress = progressHandler as IProgress<string>;

            int samples_per_second = 5;
            int samples_for_mean = 32;

            int counter_LSB = 3036;
            int MSB = 4899;
            int Core_Timer = 61520;
            int AIN0_hemo = 0;
            int counter_flow_LSB = 3034;
            int AIN5_PAK = 4;
            int numAddresses = 7;
            double scanRate = samples_per_second * samples_for_mean;
            int scansPerRead = Convert.ToInt32(scanRate);

            int[] aScanList = new int[] {
                AIN0_hemo,
                counter_LSB, MSB,
                Core_Timer, MSB,
                AIN5_PAK,
                counter_flow_LSB};
            int DeviceScanBacklog = 0;
            int LJMScanBacklog = 0;
            int data_length = numAddresses * scansPerRead;
            double[] aData = new double[data_length];
            double[] v_hemo_arr = new double[data_length];
            double v_ctn_cum = 0;

            // read in desired values for Passenergy, voltage bias, stepsize, time per step and lens voltage
            vpass = Convert.ToDouble(cb_pass.SelectedItem);
            vbias = Convert.ToDouble(cb_bias.SelectedItem);
            //vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
            double voltramp = 0.125 * 1 / Convert.ToDouble(cb_samp_ev.SelectedItem);

            btn_can.Enabled = true;

            myCurve_svg = myPane.AddCurve("", values_to_plot_svg, Color.FromArgb(0, 230, 230), SymbolType.None);
            myCurve_svg.Line.Width = 2;
            myCurve_svg.Tag = 1;

            myCurve_svg_deriv = myPane.AddCurve("", values_to_plot_svg_deriv, Color.FromArgb(21, 172, 61), SymbolType.None);
            myCurve_svg_deriv.Line.Width = 1;
            myCurve_svg_deriv.Tag = 2;

            myCurve = myPane.AddCurve("", values_to_plot, Color.FromArgb(220, 32, 0), SymbolType.VDash);
            myCurve.Line.Color = Color.FromArgb(90, 15, 0);
            myCurve.Tag = 3;

            
            //Y2Axis axis2 = new Y2Axis("first deriv");
            var axis2 = myPane.AddYAxis("Secnd");
            //axis2.Scale.IsVisible = true;
            myCurve_svg_deriv.IsY2Axis = true;
            myCurve_svg_deriv.YAxisIndex = axis2;
            //myCurve.Line.IsVisible = false;
            curr_time = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
            string u = tb_safe.Text + curr_time;
            DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES_test\", " " + curr_time + "_" +
            tb_safe.Text + "_" + cb_pass.SelectedItem + "_" + tb_slit.Text + "_" + cb_bias.SelectedItem + "_" + tb_lens.Text + "\\"));
            path_logfile = dl.FullName;
            using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
            {
                file.WriteLine("#XPS-spectrum" + Environment.NewLine);
                file.WriteLine("#Date/time: \t{0}", DateTime.Now.ToString("\t yyyy-MM-dd__HH-mm-ss"));
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("#AK pressure: \t{0} \t{1}", tb_pressure.Text, " mbar");
                file.WriteLine("#Pass energy: \t{0} \t{1}", vpass.ToString("0.0"), "\t eV");
                file.WriteLine("#Volt. bias: \t{0} \t{1}", vbias.ToString("0.0"), "\t V");
                file.WriteLine("#E_Photon: \t{0} \t{1}", V_photon.ToString("0.0"), "\t V");
                file.WriteLine("#I_Emission: \t{0} \t{1}", tb_emi.Text, "\t mA");
                file.WriteLine("#V_Anode: \t{0} \t{1}", tb_anode_voltage.Text, "\t V");
                file.WriteLine("#Prevoltage: \t{0} \t{1}", tb_prevolt.Text, "\t V");
                file.WriteLine("#Workfunction: \t{0} \t{1}", workfunction, "\t eV");
                file.WriteLine("#Samples/eV: \t{0} \t{1}", Convert.ToDouble(cb_samp_ev.SelectedItem), "\t 1/s");
                file.WriteLine("#TDAC: \t{0} \t{1}", tb_dac, "\t V");
                file.WriteLine("#V_Lens: \t{0} \t{1}", tb_lens, "\t V");
                file.WriteLine("#Factor k: \t{0} \t{1}", k.ToString(), "\t ");
                file.WriteLine("#Slope: \t{0} \t{1}", voltramp.ToString("0.0000"), "\t Samples/eV");
                file.WriteLine("#V_Channelt.: \t{0} \t{1}", vchanneltron, "\t V");
                file.WriteLine("#Flow cooling: \t{0} \t{1}", tb_flow.Text, "\t l/min");
                file.WriteLine("#Slit: \t{0} \t{1}", tb_slit.Text, "");
                file.WriteLine("#Corr offset: \t{0} \t{1}", correction_offset.ToString(), "");
                file.WriteLine("#ADC_factor: \t{0} \t{1}", voltage_divider.ToString(), "");
                file.WriteLine("#Samp_mean: \t{0} \t{1}", samples_for_mean.ToString(), "");
                file.WriteLine("#Samp/sec: \t{0} \t{1}", samples_per_second.ToString(), "");
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("#E_bind \t cps \t E_kin \t V_hemo");
                file.WriteLine("" + Environment.NewLine);
            }
            _cts_XPS.Token.ThrowIfCancellationRequested();
            // E_Analyser = E_pass - E_Photon = (U_Analyser - U_bias)*e; Electrons with E=E_photon barely can reach the chaneltron
            // neglected the work function of the electron (which would add +V_work to v_analyser_min)
            v_analyser_min = (vpass / k - V_photon + vbias) + Convert.ToDouble(tb_prevolt.Text) + workfunction; //-50V extra
            // corresponding minimum voltage of the outer/inner hemisphere; here "k" is estimated and yet not known exactly!
            //v_hemo_min = (v_analyser_min - (vpass * k * 0.4));
            v_hemo_min = (v_analyser_min - (vpass * 0.4));
            v_stabi_min = v_hemo_min + v_stabi_volt;
            //v_hemi_min = (v_hemo_min + vpass * k);

            // Needed lens voltage unknown
            //vLens_min = .0;

            // even the slowest electron should now reach the chaneltron (E_Analyser = E_pass + (E_Spec - E_Probe) = (U_Analyser - U_bias)*e)
            //v_analyser_max = vpass + vbias + 50;     // "5" takes (unknown) E_Spec - E_Probe into account
            v_analyser_max = vpass / k + vbias + 50;     // "5" takes (unknown) E_Spec - E_Probe into account
            v_hemo_max = (v_analyser_max - (vpass * 0.4));
            v_stabi_max = v_hemo_max + v_stabi_volt;
            //v_hemi_max = (v_analyser_max + vpass * k);

            // voltage drop over channeltron .
            v_channeltron_out_min = v_analyser_min + vchanneltron;
            v_channeltron_out_max = v_analyser_max + vchanneltron;

            _cts_XPS.Token.ThrowIfCancellationRequested();

            /***
            await DPS.voltage_ramp(15);
            await DPS.set_voltage(v_channeltron_out_min, 4);
            await DPS.set_voltage(v_hemo_min, 0);
            await DPS.set_voltage(Convert.ToDouble(tb_lens.Text), 2);
            await DPS.set_voltage(v_stabi_min, 5);
            await DPS.channel_on(4);
            await DPS.channel_on(0);
            await DPS.channel_on(5);
            await DPS.channel_on(2);

            await Task.Delay(8000);

            await DPS.voltage_ramp(voltramp);

            _cts_XPS.Token.ThrowIfCancellationRequested();
            ***/

            btn_start.Enabled = false;
            btn_can.Enabled = true;
            tb_show.Enabled = true;
            tb_safe.Enabled = false;

            LJM.eWriteName(handle_stream, "DIO18_EF_INDEX", 7);
            LJM.eWriteName(handle_stream, "DIO18_EF_ENABLE", 1);
            LJM.eWriteName(handle_stream, "STREAM_RESOLUTION_INDEX", 8);

            LJM.LJMERROR LJMError;

            LJMError = LJM.eStreamStart(handle_stream, scansPerRead, numAddresses, aScanList, ref scanRate);

            if (_cts_pressure_labjack != null)
            {
                _cts_pressure_labjack.Cancel();
            }

            if (_cts_flow_labjack != null)
            {
                _cts_flow_labjack.Cancel();
            }

            _cts_XPS.Token.ThrowIfCancellationRequested();
            try
            {
                double oldtime = 1;
                double ctn = 0;
                double ctn_old = 0;
                double ctn_now = 0;
                double t_now;
                double t_old = 0;
                double hemo_check = 0;
                v_ctn_cum = 0;
                Stopwatch sw = new Stopwatch();
                /***
                await DPS.set_voltage(v_hemo_max, 0);
                await DPS.set_voltage(v_stabi_max, 5);
                await DPS.set_voltage(v_channeltron_out_max, 4);
                await DPS.set_voltage(Convert.ToDouble(tb_lens.Text) - 1600, 2);
                ***/
                await Task.Run(() =>
                {
                    LJMError = LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);

                    if (LJMError == 0)
                    {
                        ctn_old = aData[data_length - 6] + aData[data_length - 5] * 65536;
                        t_old = aData[data_length - 4] + aData[data_length - 3] * 65536;
                        hemo_check = aData[data_length - numAddresses];
                        _cts_XPS.Token.ThrowIfCancellationRequested();
                    }

                    else
                    {
                        LJM.eStreamStop(handle_stream);
                        _cts_UPS.Cancel();
                        AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode", "LJM Error", 2000);
                    }

                    while (hemo_check < v_hemo_max && !token.IsCancellationRequested)
                    {
                        _cts_XPS.Token.ThrowIfCancellationRequested();
                        sw.Reset();

                        LJMError = LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);

                        if (LJMError == 0)
                        {
                            sw.Start();
                            for (int j = 0; j < samples_per_second * samples_for_mean; j++)
                            {
                                ctn_now = aData[j * numAddresses + 1] + aData[j * numAddresses + 2] * 65536;
                                if (ctn_now < ctn_old)
                                {
                                    ctn = ctn_now - ctn_old + 65536;
                                    // warum auch immer wird manchmal ein MSB übertrag nicht mitgenommen?!
                                }
                                else
                                {
                                    ctn = (ctn_now - ctn_old);
                                }
                                /***
                                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                                {
                                    file.WriteLine(
                                    oldtime.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    (aData[j * numAddresses + 3] + aData[j * numAddresses + 4] * 65536).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    ctn.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    (aData[j * numAddresses + 2]).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    (aData[j * numAddresses + 1]).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    ctn_now.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    ctn_old.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                    v_ctn_cum.ToString("N", System.Globalization.CultureInfo.InvariantCulture)
                                    );
                                }
                                ***/
                                ctn_old = ctn_now;
                                mean_volt_hemo += aData[j * numAddresses + 0] * ctn;
                                v_ctn_cum += ctn;

                                if ((j + 1) % samples_for_mean == 0)
                                {
                                    _cts_XPS.Token.ThrowIfCancellationRequested();
                                    t_now = aData[j * numAddresses + 3] + aData[j * numAddresses + 4] * 65536;
                                    mean_volt_hemo = mean_volt_hemo / v_ctn_cum;

                                    if (t_now < t_old)
                                    {
                                        v_ctn_cum = v_ctn_cum / (t_now - t_old + 4294967295) * 40000000;
                                    }
                                    else
                                    {
                                        v_ctn_cum = v_ctn_cum / (t_now - t_old) * 40000000;
                                        //v_ctn_cum = v_ctn_cum * samples_per_second;
                                    }
                                    values_to_plot.Add((oldtime) / samples_for_mean, v_ctn_cum);
                                    myCurve.AddPoint((oldtime) / samples_for_mean, v_ctn_cum);

                                    using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                                    {
                                        file.WriteLine(
                                            (oldtime / samples_for_mean).ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                            mean_volt_hemo.ToString("0.00000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                            v_ctn_cum.ToString("0.0", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                                            ((t_now - t_old) / 40000000).ToString("0.000", System.Globalization.CultureInfo.InvariantCulture)
                                            );
                                    }
                                    t_old = t_now;
                                    mean_volt_hemo = 0;

                                    filt_values.Enqueue(v_ctn_cum);
                                    if (filt_values.Count() == coeff.Length)
                                    {
                                        int i = 0;
                                        double result = 0;
                                        double result_deriv = 0;
                                        foreach (var item in filt_values)
                                        {
                                            result += coeff[i] * item;
                                            result_deriv += coeff_deriv1[i] * item;
                                            i++;
                                        }
                                        filt_values.Dequeue();
                                        values_to_plot_svg.Add((oldtime) / samples_for_mean - (coeff.Length - 1) / 2, result);
                                        myCurve_svg.AddPoint((oldtime) / samples_for_mean - (coeff.Length - 1) / 2, result);
                                        values_to_plot_svg_deriv.Add((oldtime) / samples_for_mean - (coeff_deriv1.Length - 1) / 2, result_deriv);
                                        myCurve_svg_deriv.AddPoint((oldtime) / samples_for_mean - (coeff_deriv1.Length - 1) / 2, result_deriv);
                                    }

                                    progress.Report(v_ctn_cum.ToString("000000"));
                                    v_ctn_cum = 0;
                                }
                                oldtime++;
                                _cts_XPS.Token.ThrowIfCancellationRequested();
                            }
                            zedGraphControl1.Invalidate();
                            zedGraphControl1.AxisChange();
                            //progress.Report(v_ctn_cum.ToString("N"));
                            _cts_XPS.Token.ThrowIfCancellationRequested();
                            //progress.Report(ctn.ToString("N"));
                            //progress.Report((elapsed_seconds * 10000).ToString("000000"));
                            // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                            //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                            timee = sw.Elapsed.TotalMilliseconds;
                            sw.Stop();
                        }

                        else
                        {
                            LJM.eStreamStop(handle_stream);
                            _cts_UPS.Cancel();
                            AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode in while Loop", "LJM Error", 2000);
                        }
                    }
                });
                zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, fig_name.Text + ".png"));
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                showdata.Enabled = true;
                /***
                await DPS.channel_off(4);
                await DPS.reset_channels();
                ***/
                LJM.eStreamStop(handle_stream);
            }

            catch (OperationCanceledException)
            {
                /***
                await DPS.voltage_ramp(5);
                await DPS.channel_off(4);
                await DPS.channel_off(0);
                await DPS.channel_off(5);
                await DPS.channel_off(2);
                ***/

                AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode in while Loop", "LJM Error", 2000);

                try
                {
                    zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, fig_name.Text + ".png"));
                }
                catch (Exception)
                {
                }

                try
                {
                    LJM.eStreamStop(handle_stream);
                }
                catch (Exception)
                {
                }

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





        private async void btn_reload_fil_curr_Click(object sender, EventArgs e)
        {
            await H150666.set_current(Double.Parse(tb_fil_curr.Text), 1);
        }

        private async void btn_hv_Click(object sender, EventArgs e)
        {
            await H150666.current_ramp(3);
            await H150666.select_cathode(0);
            await H150666.channel_on(1);
        }

        private async void btn_hv_2_Click(object sender, EventArgs e)
        {
            //await write_to_Iseg(String.Format(":VOLT 600.89, (@0)\n"), "XRAY");
            await H150666.current_ramp(3);
            await H150666.channel_off(1);
            await H150666.set_current(0, 1);
            fila_is_on = false;
        }


        private async void btn_hv_off_Click(object sender, EventArgs e)
        {
            await H150666.voltage_ramp(20);
            await H150666.channel_off(0);
            await H150666.set_voltage(0, 0);
            hv_is_on = false;
        }

        private async void btn_hv_on_Click(object sender, EventArgs e)
        {
            //await H150666.limit_current(100);
            await H150666.channel_on(0);
        }

        private async void btn_hv_reload_Click(object sender, EventArgs e)
        {
            await H150666.set_voltage(Double.Parse(tb_hv.Text), 0);
        }


        bool fila_is_on = true;
        bool hv_is_on = true;

        private async void btn_emi_Click(object sender, EventArgs e)
        {
            int anode_voltage = int.Parse(tb_anode_voltage.Text);
            double emission_current = Double.Parse(tb_emi.Text) / 1000.0;
            double K_P = Double.Parse(tb_KP.Text);
            double K_I = Double.Parse(tb_KI.Text);
            double K_D = Double.Parse(tb_KD.Text);
            double I_min = Double.Parse(tb_Imin.Text);
            double I_max = Double.Parse(tb_Imax.Text);
            int curr_ramp = int.Parse(tb_curr_ramp.Text);
            int volt_ramp = int.Parse(tb_volt_ramp.Text);

            await H150666.set_current(0, 1);
            await H150666.set_voltage(0, 0);
            await H150666.current_ramp(curr_ramp);
            await H150666.voltage_ramp(volt_ramp);
            await H150666.filament_current_min(I_min);
            await H150666.filament_current_max(I_max);
            await H150666.set_K_P(K_P);
            await H150666.set_K_I(K_I);
            await H150666.set_K_D(K_D);
            await H150666.set_voltage(anode_voltage, 0);
            await H150666.channel_on(0);
            await H150666.set_current(I_min, 1);
            await H150666.channel_on(1);
            await H150666.set_current(emission_current, 2);
            await H150666.channel_on(2);
        }


        private async void btn_hv_ramp_Click(object sender, EventArgs e)
        {
            await H150666.voltage_ramp(10);
            await H150666.set_voltage(12000, 0);
            await H150666.channel_on(0);
        }

        private void btn_hv_reset_Click(object sender, EventArgs e)
        {
            fila_is_on = true;
            hv_is_on = true;
        }

        private async void btn_emi_off_Click(object sender, EventArgs e)
        {
            await H150666.current_ramp(3);
            await H150666.channel_off(0);
            await H150666.set_current(0, 1);
            await H150666.voltage_ramp(20);
            await H150666.channel_off(0);
            await H150666.set_voltage(0, 0);
            //await H150666.set_current(0,2);
            //await H150666.channel_off(1);
        }

        private void tb_Imin_TextChanged(object sender, EventArgs e)
        {

        }

        private async void set_all_control_voltages(double E_bind)
        {
            double vpass = Convert.ToDouble(cb_pass.SelectedItem);
            double vbias = Convert.ToDouble(cb_bias.SelectedItem);
            double set_voltage_hemo = -V_photon + vbias + vpass / k + workfunction + E_bind - vpass * 0.4;
            double set_voltage_channeltron = set_voltage_hemo + vpass * 0.4 + vchanneltron;
            double set_voltage_Stabi = set_voltage_hemo + v_stabi_volt;

            await DPS.voltage_ramp(10);
            await DPS.set_voltage(set_voltage_hemo, 0);
            await DPS.set_voltage(Convert.ToDouble(tb_lens.Text), 2);
            await DPS.set_voltage(set_voltage_channeltron, 4);
            await DPS.set_voltage(set_voltage_Stabi, 5);
        }


        private void btn_clear_fig_Click(object sender, EventArgs e)
        {
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

        double timee;


        private async void btn_test_Click(object sender, EventArgs e)
        {
            take_XPS_spectra();
        }


        public void global_pwm_down(object sender, KeyEventArgs e)
        {
            TextBox tb = sender as TextBox;
            int roll = 0;
            int divisor = 0;
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    roll = Convert.ToInt16(tb_roll.Text);
                    divisor = Convert.ToInt32(tb_divisor.Text);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Bla", "Roll/Divisor muss int sein", 500);
                }            
                tb_roll.Text = String.Empty;
                tb_roll.Text = roll.ToString();
                tb_divisor.Text = String.Empty;
                tb_divisor.Text = divisor.ToString();
                PWM(sender, roll, divisor);
            }
            
        }


        public void PWM(object sender, int roll, int divisor)
        {
            int handle_PWM = 0;
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_PWM);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_PWM' session!", "Info", 500);
            }
            //TextBox tb = (TextBox)sender;
            LJM.eWriteName(handle_PWM, "DIO_EF_CLOCK1_ENABLE", 0);  // Disable clock source
                                                                    // Set Clock0's divisor and roll value to configure frequency: 80MHz/1/80000 = 1kHz
            LJM.eWriteName(handle_PWM, "DIO_EF_CLOCK1_DIVISOR", divisor);     // Configure Clock0's divisor
            LJM.eWriteName(handle_PWM, "DIO_EF_CLOCK1_ROLL_VALUE", roll);  // Configure Clock0's roll value
            LJM.eWriteName(handle_PWM, "DIO_EF_CLOCK1_ENABLE", 1);  // Enable the clock source

            // Configure EF Channel Registers:
            LJM.eWriteName(handle_PWM, "DIO0_EF_ENABLE", 0);    // Disable the EF system for initial configuration
            LJM.eWriteName(handle_PWM, "DIO0_EF_INDEX", 0);     // Configure EF system for PWM
            LJM.eWriteName(handle_PWM, "DIO0_EF_OPTIONS", 1);   // Configure what clock source to use: Clock0
            LJM.eWriteName(handle_PWM, "DIO0_EF_CONFIG_A", roll/2);  // Configure duty cycle to be: 50%
            LJM.eWriteName(handle_PWM, "DIO0_EF_ENABLE", 1); 	// Enable the EF system, PWM wave is now being outputted
        }

        private void tb_dac_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Double.TryParse(tb_dac.Text.Replace(",", "."), out double v_dac))
                {
                    int handle_DAC = 0;
                    LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_DAC);
                    LJM.eWriteName(handle_DAC, cb_DAC.SelectedItem.ToString(), v_dac);
                    tb_dac.Text = String.Empty;
                    tb_dac.Text = v_dac.ToString();
                    //LJM.Close(handle_DAC);
                }
            }
        }

        private async void tb_set_E_B_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (Double.TryParse(tb_set_E_B.Text.Replace(",", "."), out double U_E_binding))
                {
                    set_all_control_voltages(U_E_binding);
                    await DPS.channel_on(0);
                    await DPS.channel_on(2);
                    await DPS.channel_on(4);
                    await DPS.channel_on(5);
                    tb_set_E_B.Text = String.Empty;
                    tb_set_E_B.Text = tb_set_E_B.ToString();
                }
            }
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
--- resolution index adc? nur 16 bit?
--- overflow counter mit if verhindern
 ***/