﻿/***
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
        //double V_photon;
        double E_HeI;               // Energy HeI-line
        double E_Al_Ka;            // Energy Al K_alpha photon
        double E_Mg_Ka;            // Energy Mg K_alpha photon
        double ri;                    // Radius inner hemisphere in mm
        //double ri = 92.075;
        double ra;                    // Radius outer hemisphere in mm
        //double ra = 111.125;
        string pin_pressure_ak = string.Empty;       // Analog Input Pin of Ionivac
        string pin_pressure_pk = string.Empty;
        string pin_pressure_sk = string.Empty;
        string pin_flow = string.Empty;
        string pin_hemo = string.Empty;
        string pin_hemi = string.Empty;
        string pin_analyser = string.Empty;
        double vchanneltron;          // voltage drop over channeltron
        //double workfunction = 4.8;
        double workfunction = 4.746;
        double v_stabi_volt;                     // to always habe approx. 1500V voltage drop over Z-diode circuitry
                                                 //double offset = 20;
        double correction_offset = 0;        // correction factor vor pass energy

        //[ 0.8544416  14.47170123  4.70083337  0.99564488]
        double voltage_divider = 153.612 * 0.99569;
        double k = 0.859;


        // Labjack stuff           // Labjack threads
        double ionivac_v_out = 0;           // Voltage of Ionivac output measured with Labjack device
        double cnt_before = 0;              // coutner reading befor and after delay
        double cnt_after = 0;
        double cnt_flow_before = 0;              // coutner reading befor and after delay
        double cnt_flow_after = 0;

        // Voltage setting stuff
        double vpass;
        double vbias;                    // multiples of 4 mV/s


        // Emission current regulation


        string path_logfile;

        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string curr_time;
        string[] scores = { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
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
        private CancellationTokenSource _cts_XPS;
        //private IMessageBasedSession DPS_HV;           // Iseg-HV session 6 Chanel HV
        //private IMessageBasedSession Xray_HV;        // Iseg X-Ray HV session





        public XPS()
        {
            // dot instead of comma (very important for voltage input values!)
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            //try
            // {
            var dic = File.ReadAllLines("settings.txt").Select(l => l.Split(new[] { '=' })).ToDictionary(s => s[0].Trim(), s => s[1].Trim());
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
            for (var i = 0; i < table_binding_energies.Count; i++)
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


            background_meas_pressure_labjack();
            // cooling flow measurement, update every second
            background_meas_flow_labjack();




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
            cb_scanrange.SelectedIndex = 0;
            cb_samp_ev.SelectedIndex = 2;
            cb_DAC.SelectedIndex = 0;

            // proportionality between the voltage applied to the hemispheres and the pass energy

            pwm_tb = new TextBox[] {tb_roll, tb_divisor};
            foreach (var item in pwm_tb)
            {
                //item.KeyDown += global_pwm_down;
                item.KeyDown += new KeyEventHandler(global_pwm_down);
            }

            //this.TopMost = true;
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;

            Iseg_DPS_session.Checked = !Iseg_DPS_session.Checked;
            Iseg_Xray_session.Checked = !Iseg_Xray_session.Checked;
        }

        //##################################################################################################################################################################



        private void Init_DPS_control_panel()
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



        private void btn_can_Click(object sender, EventArgs e)
        {
            if (_cts_XPS != null)
            {
                _cts_XPS.Cancel();
            }
            DPS_reset();
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
            progressBar1.Value = 0;
            lb_progress.Text = string.Empty;
            //if (Mg_anode.Checked) {Mg_anode.Enabled = true;}
            //    else { Al_anode.Enabled = true;}
            fig_name.Clear();
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            values_to_plot.Clear();
            values_to_plot_svg.Clear();
            values_to_plot_svg_deriv.Clear();
            errorlist.Clear();
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
            safe_fig.Enabled = (fig_name.Text == string.Empty) ? false : true;
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


        private void rs_all_Click(object sender, EventArgs e)
        {
            DPS_reset();
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
            if (_cts_XPS != null)
            {
                _cts_XPS.Cancel();
                _cts_XPS.Token.WaitHandle.WaitOne();
                //LJM.eStreamStop(handle_stream);
                Thread.Sleep(1000);
                //LJM.Close(handle_stream);
            }

            if (_cts_pressure_labjack != null)
            {
                _cts_pressure_labjack.Cancel();
                _cts_pressure_labjack.Token.WaitHandle.WaitOne();
            }

            if (_cts_volt_dps != null)
            {
                _cts_volt_dps.Cancel();
                _cts_volt_dps.Token.WaitHandle.WaitOne();
            }

            if (_cts_counter_labjack != null)
            {
                _cts_counter_labjack.Cancel();
                _cts_counter_labjack.Token.WaitHandle.WaitOne();
            }

            if (_cts_flow_labjack != null)
            {
                _cts_flow_labjack.Cancel();
                _cts_flow_labjack.Token.WaitHandle.WaitOne();
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
            //await Task.Delay(500);
            //Thread.Sleep(500);

            try
            {
                values_to_plot.Clear();
                values_to_plot_svg.Clear();
                errorlist.Clear();
                values_to_plot_svg_deriv.Clear();
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't clear Pointpairlists", "ZedGraph error", 1000);
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

            DPS_reset();
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

        // X P S #################################### X P S ##################################### X P S #################################### X P S ##############


        private void cb_select_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_start.Enabled = (DPS.Is_session_open == true & H150666.Is_session_open == true) ? true : false;
            cb_select.BackColor = System.Drawing.Color.MediumSpringGreen;
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


        private async void btn_emi_off_Click(object sender, EventArgs e)
        {
            await H150666.current_ramp(3);
            await H150666.channel_off(0);
            await H150666.set_current(0, 1);
            await H150666.voltage_ramp(20);
            await H150666.channel_off(0);
            await H150666.set_voltage(0, 0);
            await H150666.reset_channels();
            //await H150666.set_current(0,2);
            //await H150666.channel_off(1);
        }

        private async void set_all_control_voltages(double E_bind, double ramp, int sleeptime)
        {
            try
            {
                double vpass = Convert.ToDouble(cb_pass.SelectedItem);
                double vbias = Convert.ToDouble(cb_bias.SelectedItem);
                double V_photon = (cb_select.SelectedIndex == 0) ? E_Al_Ka : (cb_select.SelectedIndex == 1) ? E_Mg_Ka : E_HeI;
                double set_voltage_hemo = -V_photon + vbias + vpass / k + workfunction + E_bind - vpass * 0.4;
                double set_voltage_channeltron = set_voltage_hemo + vpass * 0.4 + vchanneltron;
                double set_voltage_Stabi = set_voltage_hemo + v_stabi_volt;

                await DPS.voltage_ramp(ramp);
                await DPS.set_voltage(set_voltage_hemo, 0);
                await DPS.set_voltage(Convert.ToDouble(tb_lens.Text), 2);
                await DPS.set_voltage(set_voltage_channeltron, 4);
                await DPS.set_voltage(set_voltage_Stabi, 5);

                await DPS.channel_on(0);
                await DPS.channel_on(2);
                await DPS.channel_on(4);
                await DPS.channel_on(5);

                await Task.Delay(sleeptime);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Unable to set voltages","Voltage setting error",2000);
            }
        }


        private void btn_clear_fig_Click(object sender, EventArgs e)
        {
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

        //double timee;


        private void btn_test_Click(object sender, EventArgs e)
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

        int handle_PWM = 6;
        public void PWM(object sender, int roll, int divisor)
        {          
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_PWM);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_PWM' session!", "Info", 500);
            }
            
            //TextBox tb = (TextBox)sender;
            LJM.eWriteName(handle_PWM, "DIO_EF_CLOCK1_ENABLE", 0);  // Disable clock source                                                                    // Set Clock0's divisor and roll value to configure frequency: 80MHz/1/80000 = 1kHz
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
            if (e.KeyCode == Keys.Enter && Double.TryParse(tb_dac.Text.Replace(",", "."), out double v_dac))
            {
                int handle_DAC = 5;
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_DAC);
                try
                {
                    LJM.eWriteName(handle_DAC, cb_DAC.SelectedItem.ToString(), v_dac);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Input Error","LJM handle error",1000);
                    LJM.Close(handle_DAC);
                    return;
                }
                tb_dac.Text = String.Empty;
                tb_dac.Text = v_dac.ToString();
                //LJM.Close(handle_DAC);
            }
        }

        private void tb_set_E_B_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && Double.TryParse(tb_set_E_B.Text.Replace(",", "."), out double U_E_binding))
            {
                set_all_control_voltages(U_E_binding,15,100);
                tb_set_E_B.Text = String.Empty;
                tb_set_E_B.Text = tb_set_E_B.Text.ToString();
                //await Task.Delay(10000);
            }
        }

        private async void btn_hv_off_Click(object sender, EventArgs e)
        {
            try
            {
                await H150666.voltage_ramp(20);
                await H150666.channel_off(0);
                await H150666.set_voltage(0, 0);
                await H150666.channel_off(1);
                await H150666.set_voltage(0, 1);
            }
            catch (Exception)
            {
                return;
            }
        }

        private async void DPS_reset()
        {
            try
            {
                await DPS.voltage_ramp(20);
                await DPS.reset_channels();
                for (int i = 0; i <= 5; i++)
                {
                    vset[i].Text = string.Empty;
                    vmeas[i].Text = string.Empty;
                    stat[i].Text = "Off";
                    stat[i].BackColor = SystemColors.ControlLightLight;
                }
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't reset DPS device","DPS Error",1000);
                return;
            }
        }

        private void btn_DPS_off_Click(object sender, EventArgs e)
        {
            DPS_reset();
            //_suspend_background_measurement.Set();
            //rs_all.PerformClick();
            //_suspend_background_measurement.Reset();
        }

        private void tb_Imin_TextChanged(object sender, EventArgs e)
        {

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