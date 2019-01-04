using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using LabJack;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using ZedGraph;




namespace XPS
{
    public partial class XPS : Form
    {


        private async void background_meas_pressure_labjack()
        {
            int handle_pressure_ak = 0;

            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_pressure_ak);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_pressure_ak' session!", "Info", 500);
                return; //jump out of method
            }
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
                    while (!_cts_pressure_labjack.IsCancellationRequested)
                        //while (true)
                    {
                        /***
                        if (_cts_pressure_labjack.IsCancellationRequested)
                        {
                            return;
                        }
                        ***/
                        LJM.eReadName(handle_pressure_ak, pin_pressure_ak, ref ionivac_v_out);
                        pressure = Math.Pow(10, ((Convert.ToDouble(ionivac_v_out) - 7.75)) / 0.75);
                        Thread.Sleep(2000);
                        if (progress != null)
                        {
                            progress.Report(pressure.ToString("0.00E0"));
                        }
                    }
                });
                //MessageBox.Show("Completed!");
            }

            catch (OperationCanceledException)
            {
                tb_pressure.Text = "can";
            }
        }


        private async void background_meas_volt_DPS()
        {
            _cts_volt_dps = new CancellationTokenSource();
            var token = _cts_volt_dps.Token;
            int result = 0;
            int i = -1;
            var progressHandler2 = new Progress<string>(value =>
            {
                //vmeas[result].Text = value.Substring(0,7);
                vmeas[result].Text = String.Format("{0:.##}", value);

                if (take_UPS_spec && result == 4)
                {
                    vm5.Text = value;
                }
            });
            var progress = progressHandler2 as IProgress<string>;
            string readback = string.Empty;
            //double readback = 0;
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
                        DPS.raw_read_syn(i);
                        Thread.Sleep(50);
                        //otherwise problems when closing DPS-session
                        try
                        {
                            readback = (DPS.raw_read_syn().Replace("V\r\n", ""));
                            readback = readback.Replace(".", ",");
                            readback = ((Double.Parse(readback) * 1).ToString("0.00"));
                            if (readback.Substring(readback.Length - 2, 2) == "E3")
                            {
                                readback = readback.Replace("E3", "");
                                readback = ((Double.Parse(readback) * 1000.0).ToString("0.00"));
                            }
                        }
                        catch (Exception) { }
                        progress.Report(readback);
                        //progress.Report(i.ToString());
                        result = i;
                        // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                        _suspend_background_measurement.WaitOne(Timeout.Infinite);
                        Thread.Sleep(50);
                    }
                });
                //MessageBox.Show("Completed!");
            }
            catch (OperationCanceledException)
            {
                //AutoClosingMessageBox.Show("Problem with background measurement of Iseg DPS voltages", "Info", 500);
            }
        }


        private async void background_meas_flow_labjack()
        {
            int handle_flow = 0;
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_flow);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_flow' session!", "Info", 500);
                return;
            }
            _cts_flow_labjack = new CancellationTokenSource();
            var token = _cts_flow_labjack.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_flow.Text = value;
            });
            var progress = progressHandler as IProgress<string>;
            try
            {
                double erg2 = 0;
                //Stopwatch sw = new Stopwatch();
                await Task.Run(() =>
                {
                    while (!_cts_flow_labjack.IsCancellationRequested)
                    {
                        LJM.eWriteName(handle_flow, "DIO17_EF_INDEX", 7);
                        LJM.eWriteName(handle_flow, "DIO17_EF_ENABLE", 1);
                        //sw.Start();
                        LJM.eReadName(handle_flow, "DIO17_EF_READ_A", ref cnt_flow_before);
                        Thread.Sleep(2000);
                        //sw.Stop();
                        LJM.eReadName(handle_flow, "DIO17_EF_READ_A_AND_RESET", ref cnt_flow_after);
                        erg2 = (cnt_flow_after - cnt_flow_before) / 750 / 2 * 60;
                        //erg2 = (cnt_flow_after - cnt_flow_before);
                        //sw.Reset();
                        if (progress != null)
                        {
                            progress.Report(erg2.ToString("0.0"));    //no decimal placed
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


        private async void background_counter_labjack()
        {
            int handle_background_counter = 0;
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_background_counter);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_background_counter' session!", "Info", 500);
            }
            myCurve = myPane.AddCurve("", values_to_plot, Color.Black, SymbolType.None);
            int number = 0;
            _cts_counter_labjack = new CancellationTokenSource();
            var token = _cts_counter_labjack.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_counter.Text = value;
                values_to_plot.Add(number, Double.Parse(value));
                myCurve.AddPoint(number, Double.Parse(value));
                //values_to_plot.Add(ctner, Convert.ToDouble(value));
                //myCurve.AddPoint(ctner, Convert.ToDouble(value));
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
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
                    while (!_cts_counter_labjack.IsCancellationRequested)
                    {
                        LJM.eWriteName(handle_background_counter, "DIO18_EF_INDEX", 7);
                        LJM.eWriteName(handle_background_counter, "DIO18_EF_ENABLE", 1);
                        sw.Start();
                        LJM.eReadName(handle_background_counter, "DIO18_EF_READ_A", ref cnt_before);
                        Thread.Sleep(ct);
                        sw.Stop();
                        LJM.eReadName(handle_background_counter, "DIO18_EF_READ_A_AND_RESET", ref cnt_after);
                        erg = (cnt_after - cnt_before) / sw.Elapsed.TotalSeconds;
                        sw.Reset();
                        if (progress != null)
                        {
                            //progress.Report(erg.ToString("N0"));    //no decimal placed
                            progress.Report(erg.ToString("0000000"));
                        }
                        number += 1;
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


        private double median(double[] numbers)
        {
            int numberCount = numbers.Count();
            int halfIndex = numbers.Count() / 2;
            var sortedNumbers = numbers.OrderBy(n => n);
            double median;
            if ((numberCount % 2) == 0)
            {
                median = ((sortedNumbers.ElementAt(halfIndex) + sortedNumbers.ElementAt((halfIndex - 1))) / 2);
            }
            else
            {
                median = sortedNumbers.ElementAt(halfIndex);
            }
            return 1;
        }


        private async void take_XPS_spectra()
        {
            Queue<double> filt_values = new Queue<double>();
            string dict_entry_smooth = "smooth_deg_4_num_points_" + (Convert.ToInt32(cb_samp_ev.SelectedItem) * 2 + 1).ToString();
            string dict_entry_deriv = "deriv_deg_4_num_points_" + (Convert.ToInt32(cb_samp_ev.SelectedItem) * 2 + 1).ToString();
            double[] coeff = sav_gol_coeff[dict_entry_smooth];
            double[] coeff_deriv = sav_gol_coeff[dict_entry_deriv];

            int handle_stream = 0;


            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_stream);
                //LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_DAC2);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_stream' or 'handle_DAC2' session!", "Info", 500);
                return;
            }

            //LJM.eWriteName(handle_DAC2, "DAC1", 4.0000);

            //double mean_volt_hemo = 0;
            _cts_XPS = new CancellationTokenSource();
            var token = _cts_XPS.Token;
            var progressHandler = new Progress<string>(value =>
            {
                tb_cps.Text = value;
                try
                {
                    //progressBar1.Value = Convert.ToInt32(timee/2);
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
            double scanRate = samples_per_second * samples_for_mean;
            int scansPerRead = Convert.ToInt32(scanRate);

            int[] aScanList = new int[] {
                AIN0_hemo,
                counter_LSB, MSB,
                Core_Timer, MSB,
                AIN5_PAK,
                counter_flow_LSB};
            int numAddresses = aScanList.Length;
            int DeviceScanBacklog = 0;
            int LJMScanBacklog = 0;
            int data_length = numAddresses * scansPerRead;
            double[] aData = new double[data_length];
            double[] v_hemo_arr = new double[data_length];
            //double v_ctn_cum = 0;

            // read in desired values for Passenergy, voltage bias, stepsize, time per step and lens voltage
            vpass = Convert.ToDouble(cb_pass.SelectedItem);
            vbias = Convert.ToDouble(cb_bias.SelectedItem);
            //vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
            double voltramp = 0.125 * 1 / Convert.ToDouble(cb_samp_ev.SelectedItem);

            btn_can.Enabled = true;
            

            //yaxis.Tag = 5;
            //Y2Axis axis2 = new Y2Axis("first deriv");
            //var axis2 = myPane.AddYAxis("Secnd");
            //axis2.Scale.IsVisible = true;
            //myCurve_svg_deriv.IsY2Axis = true;
            //myCurve_svg_deriv.YAxisIndex = axis2;
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
                file.WriteLine("#Sav_Gol deg: \t 4");
                file.WriteLine("#Sav_Gold Samp: \t{0} ", (samples_per_second*2).ToString(), "");
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("#E_bind \t cps \t E_kin \t V_hemo");
                file.WriteLine("" + Environment.NewLine);
            }

            double E_B_end = 0;
            if (cb_scanrange.SelectedItem.ToString() == "Full")
            {
                set_all_control_voltages(0, 12, 100);
                E_B_end = V_photon;
            }

            else if (cb_scanrange.SelectedItem.ToString() == "Detail")
            {
                try
                {
                    double E_B_start = Convert.ToDouble(tb_detailscan_start.Text);
                    E_B_end = Convert.ToDouble(tb_detailscan_stop.Text);
                    if ((E_B_start >= 0) && (E_B_start <= V_photon) && (E_B_end >= 0) && (E_B_end <= V_photon))
                    {
                        set_all_control_voltages(E_B_start, 12, 100);
                    }
                    else
                    {
                        AutoClosingMessageBox.Show("E_B in range 0 to E_Photon", "Input Error", 2000);
                        return;
                    }
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Type in a number in a valid range","Input Error",2000);
                    return;
                }
            }

            else if (cb_scanrange.SelectedItem.ToString() == "Spot")
            {
                try
                {
                    double E_B_spot = Convert.ToDouble(tb_set_E_B.Text);
                    if ((E_B_spot >= 0) && (E_B_spot <= V_photon))
                    {
                        E_B_end = Convert.ToDouble(tb_set_E_B.Text);
                        set_all_control_voltages(E_B_spot, 12, 100);
                    }
                    else
                    {
                        AutoClosingMessageBox.Show("E_B in range 0 to E_Photon", "Input Error", 2000);
                        return;
                    }
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Type in a number", "Input Error", 2000);
                    return;
                }
            }

            await Task.Delay(10000);


            /***
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

            try
            {
                double oldtime = 1;
                double cps = 0;
                //double ctn = 0;
                double ctn_old = 0;
                //double ctn_now = 0;
                //double t_now;
                double t_old = 0;
                double curr_E_B = 0;
                double samp_ev = Convert.ToDouble(cb_samp_ev.SelectedItem);
                //double error = 0;
                //v_ctn_cum = 0;
                //Stopwatch sw = new Stopwatch();

                set_all_control_voltages(E_B_end,voltramp,100);
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
                        //hemo_check = aData[data_length - numAddresses] * voltage_divider;
                    }

                    else
                    {
                        LJM.eStreamStop(handle_stream);
                        _cts_XPS.Cancel();
                        AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode", "LJM Error", 2000);
                    }
                    //while (hemo_check < 5.0 && !_cts_XPS.IsCancellationRequested)
                    while (true)
                    {
                        if ((E_B_end < (curr_E_B + 1.0)) || _cts_XPS.IsCancellationRequested)
                        {
                            return;
                        }
                        //sw.Reset();
                        _cts_XPS.Token.ThrowIfCancellationRequested();
                        LJMError = LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);

                        if (LJMError == 0)
                        {
                            //sw.Start();
                            var old_values = data_processing(aData, numAddresses, filt_values, coeff, coeff_deriv, samples_per_second, samples_for_mean, samp_ev,
                                ctn_old, t_old);
                            ctn_old = old_values.Item1;
                            t_old = old_values.Item2;
                            oldtime = old_values.Item3;
                            curr_E_B = old_values.Item4;
                            progress.Report(cps.ToString("000000"));
                            //progress.Report(v_ctn_cum.ToString("N"));
                            //progress.Report(ctn.ToString("N"));
                            //progress.Report((elapsed_seconds * 10000).ToString("000000"));
                            // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                            //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                            //timee = sw.Elapsed.TotalMilliseconds;
                            //sw.Stop();
                        }

                        else
                        {
                            LJM.eStreamStop(handle_stream);
                            _cts_XPS.Cancel();
                            AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode in while Loop", "LJM Error", 2000);
                            return;
                        }
                    }
                });
                zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, fig_name.Text + ".png"));
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                showdata.Enabled = true;
                DPS_reset();
                LJM.eStreamStop(handle_stream);
                tb_cps.Text = "end";

                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  E N D");
                }
            }

            catch (OperationCanceledException)
            {
                zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path_logfile, fig_name.Text + ".png"));
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                showdata.Enabled = true;
                DPS_reset();
                //await DPS.channel_off(4);
                //await DPS.reset_channels();
                LJM.eStreamStop(handle_stream);
                tb_cps.Text = "Stop!";
                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
            }
        }


        Dictionary<string, double[]> sav_gol_coeff = new Dictionary<string, double[]>
        {
            {"smooth_deg_4_num_points_5", new [] { 0.0, 0.0, 1.0, 0.0, 0.0} },
            {"deriv_deg_4_num_points_5", new [] { 0.08333333, -0.66666667, 0.0 , 0.66666667, -0.08333333 } },

            {"smooth_deg_4_num_points_11", new [] {0.04195804, -0.1048951 , -0.02331002,  0.13986014,  0.27972028,
                0.33333333,  0.27972028,  0.13986014, -0.02331002, -0.1048951 ,
                0.04195804} },
            {"deriv_deg_4_num_points_11", new [] {0.058275, -0.057110, -0.103341, -0.097708, -0.057498, 0.000000,
                0.057498, 0.097708, 0.103341, 0.057110, -0.058275} },

            {"smooth_deg_4_num_points_21", new [] {0.044720, -0.024845, -0.050016, -0.043151, -0.015153, 0.024529,
                0.067900, 0.108417, 0.140992, 0.161991, 0.169233, 0.161991,
                0.140992, 0.108417, 0.067900, 0.024529, -0.015153, -0.043151,
                -0.050016, -0.024845, 0.044720} },
            {"deriv_deg_4_num_points_21", new [] {0.023135, 0.002761, -0.011911, -0.021512, -0.026677, -0.028040,
                -0.026234, -0.021894, -0.015652, -0.008143, 0.000000, 0.008143,
                0.015652, 0.021894, 0.026234, 0.028040, 0.026677, 0.021512,
                0.011911, -0.002761, -0.023135} },

            {"smooth_deg_4_num_points_33", new [] {0.03685504,  0.002457  , -0.01902195, -0.0297218 , -0.03163493,
                -0.02660614, -0.01633264, -0.00236408,  0.01389751,  0.03119765,
                 0.04842946,  0.06463365,  0.07899851,  0.0908599 ,  0.09970128,
                 0.10515369,  0.10699576,  0.10515369,  0.09970128,  0.0908599 ,
                 0.07899851,  0.06463365,  0.04842946,  0.03119765,  0.01389751,
                -0.00236408, -0.01633264, -0.02660614, -0.03163493, -0.0297218 ,
                -0.01902195,  0.002457  ,  0.03685504} },
            {"deriv_deg_4_num_points_33", new [] {0.01079422,  0.00507526,  0.00033263, -0.00349878, -0.00648404,
                -0.00868824, -0.01017648, -0.01101384, -0.01126541, -0.01099627,
                -0.01027152, -0.00915624, -0.00771552, -0.00601445, -0.00411811,
                -0.0020916 ,  0.0       ,  0.0020916 ,  0.00411811,  0.00601445,
                 0.00771552,  0.00915624,  0.01027152,  0.01099627,  0.01126541,
                 0.01101384,  0.01017648,  0.00868824,  0.00648404,  0.00349878,
                -0.00033263, -0.00507526, -0.01079422} },

            {"smooth_deg_4_num_points_51", new [] {0.027848, 0.011603, -0.000865, -0.009946, -0.016012, -0.019419,
                -0.020508, -0.019600, -0.017002, -0.013005, -0.007880, -0.001886,
                 0.004738, 0.011769, 0.018999, 0.026238, 0.033312, 0.040063,
                 0.046352, 0.052053, 0.057059, 0.061279, 0.064639, 0.067080,
                 0.068562, 0.069058, 0.068562, 0.067080, 0.064639, 0.061279,
                 0.057059, 0.052053, 0.046352, 0.040063, 0.033312, 0.026238,
                 0.018999, 0.011769, 0.004738, -0.001886, -0.007880, -0.013005,
                 -0.017002, -0.019600, -0.020508, -0.019419, -0.016012, -0.009946,
                -0.000865, 0.011603, 0.027848} },
            {"deriv_deg_4_num_points_51", new [] {0.004928, 0.003292, 0.001833, 0.000543, -0.000586, -0.001561,
                -0.002389, -0.003077, -0.003634, -0.004066, -0.004380, -0.004585,
                -0.004686, -0.004693, -0.004611, -0.004449, -0.004213, -0.003911,
                -0.003551, -0.003139, -0.002683, -0.002190, -0.001668, -0.001124,
                 -0.000566, -0.000000, 0.000566, 0.001124, 0.001668, 0.002190,
                 0.002683, 0.003139, 0.003551, 0.003911, 0.004213, 0.004449,
                0.004611, 0.004693, 0.004686, 0.004585, 0.004380, 0.004066,
                 0.003634, 0.003077, 0.002389, 0.001561, 0.000586, -0.000543,
                 -0.001833, -0.003292, -0.004928} },
        };


        public Tuple<double, double, double, double> data_processing(double[] aData, int numAddresses, Queue<double> filt_values,
            double[] coeff, double[] coeff_deriv, int samples_per_second, int samples_for_mean, double samp_ev,
            double ctn_old, double t_old)
        {
            
            //double oldtime = 1;
            double cps = 0;
            double ctn = 0;
            //double ctn_old = 0;
            double ctn_now = 0;
            double t_now;
            //double t_old = 0;
            double error = 0;
            double v_ctn_cum = 0;
            double mean_volt_hemo = 0;
            var inc = 0;
            double E_bind = 0;
            double result = 0;
            double result_deriv = 0;

            for (var j = 0; j < samples_per_second * samples_for_mean; j++)
            {
                inc = j * numAddresses;
                ctn_now = aData[inc + 1] + aData[inc + 2] * 65536;
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
                    (aData[inc + 3] + aData[inc + 4] * 65536).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    ctn.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    (aData[inc + 2]).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    (aData[inc + 1]).ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    ctn_now.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    ctn_old.ToString("N", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    v_ctn_cum.ToString("N", System.Globalization.CultureInfo.InvariantCulture)
                    );
                }
                ***/
                ctn_old = ctn_now;
                mean_volt_hemo += aData[inc + 0] * (ctn + 1);
                v_ctn_cum += (ctn + 1);

                if ((j + 1) % samples_for_mean == 0)
                {
                    t_now = aData[inc + 3] + aData[inc + 4] * 65536;
                    mean_volt_hemo = mean_volt_hemo / v_ctn_cum * voltage_divider;
                    v_ctn_cum = v_ctn_cum - samples_for_mean;

                    if (t_now < t_old)
                    {
                        v_ctn_cum = v_ctn_cum / (t_now - t_old + Math.Pow(2,32)) * 40000000;
                    }
                    else
                    {
                        v_ctn_cum = v_ctn_cum / (t_now - t_old) * 40000000;
                        //v_ctn_cum = v_ctn_cum * samples_per_second;
                    }

                    E_bind = mean_volt_hemo + V_photon - vbias - vpass / k - workfunction + vpass * 0.4;

                    error = Math.Sqrt(v_ctn_cum) / 2;
                    values_to_plot.Add(E_bind, v_ctn_cum);
                    //myCurve.AddPoint((oldtime) / samples_for_mean, v_ctn_cum);
                    errorlist.Add(E_bind, v_ctn_cum - error, v_ctn_cum + error);

                    
                    //t_old = t_now;
                    //mean_volt_hemo = 0;

                    //Savitzky Golay filtering
                    filt_values.Enqueue(v_ctn_cum);
                    //double result = Sav_Gol(filt_values, coeff, coeff_deriv, 10).Item1;
                    //double result_deriv = Sav_Gol(filt_values, coeff, coeff_deriv, 10).Item2;   
                    //var results = Sav_Gol(filt_values, coeff, coeff_deriv);

                    if (filt_values.Count() == coeff.Length)
                    {
                        result = 0;
                        result_deriv = 0;

                        var i = 0;
                        foreach (var item in filt_values)
                        {
                            result += coeff[i] * item;
                            result_deriv += coeff_deriv[i] * item;
                            i++;
                        }
                        filt_values.Dequeue();
                        values_to_plot_svg.Add(E_bind - samples_per_second / samp_ev, result);
                        //myCurve_svg.AddPoint((oldtime) / samples_for_mean - (coeff.Length - 1) / 2, result);
                        //values_to_plot_svg_deriv.Add(E_bind - samples_per_second / samp_ev, result_deriv);
                        //myCurve_svg_deriv.AddPoint((oldtime) / samples_for_mean - (coeff_deriv.Length - 1) / 2 - 1, result_deriv);
                    }

                    using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                    {
                        file.WriteLine(
                            E_bind.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                            v_ctn_cum.ToString("000000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                            mean_volt_hemo.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                            result.ToString("000000.0", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                            result_deriv.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture)
                            );
                    }

                    //myPane.GraphObjList.Clear();
                    TextObj label = new TextObj(v_ctn_cum.ToString("0.0"), 1, v_ctn_cum);
                    label.Location.CoordinateFrame = CoordType.XChartFractionY2Scale;
                    label.Location.AlignH = AlignH.Right;
                    myPane.GraphObjList.Add(label);

                    t_old = t_now;
                    mean_volt_hemo = 0;
                    cps = v_ctn_cum;
                    v_ctn_cum = 0;
                }
                //oldtime++;
                //progress.Report(v_ctn_cum.ToString("000000"));
            }
            try
            {
                zedGraphControl1.AxisChange();
                zedGraphControl1.Invalidate();
            }
            catch (Exception)
            {

            }
            
            return Tuple.Create(ctn_old, t_old, samp_ev, E_bind);
        }

        

    }
}