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
            int handle_pressure_ak = 1;
            double ionivac_v_out = 0;

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
                        try
                        {
                            LJM.eReadName(handle_pressure_ak, pin_pressure_ak, ref ionivac_v_out);
                            pressure = Math.Pow(10, ((Convert.ToDouble(ionivac_v_out) - 7.75)) / 0.75);
                            Thread.Sleep(2000);
                            if (progress != null)
                            {
                                progress.Report(pressure.ToString("0.00E0") + " mbar");
                            }
                        }
                        catch (Exception)
                        {
                            AutoClosingMessageBox.Show("Can'r read pressure pin","LJM Error",1000);
                            return;
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
            //_cts_volt_dps = new CancellationTokenSource();
            //var token = _cts_volt_dps.Token;
            string[] arr_voltages = new string[6];
            string[] arr_voltages_H150666 = new string[6];
            double volt, curr = 0;
            var progressHandler2 = new Progress<string>(value =>
            {
                vmeas2[0].Text = arr_voltages[0];
                vmeas2[1].Text = arr_voltages[5];

                for (int i = 0; i < 3; i++)
                {
                    meas_H150666[i].Text = i == 0 ? arr_voltages_H150666[i] + " V" : arr_voltages_H150666[i] + " mA";
                }

                for (int j = 0; j < 6; j++)
                {
                    vmeas[j].Text = arr_voltages[j];         
                }

                double.TryParse(arr_voltages_H150666[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out volt);
                double.TryParse(arr_voltages_H150666[2], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out curr);
                tb_power.Text = ((volt * curr / 1.0e6) > 2.0 ? (volt * curr / 1.0e6).ToString("0.0") : "0.0") + " W";
                //meas_H150666[0].BackColor = (arr_voltages_H150666[0] != String.Empty && Math.Abs(Convert.ToDouble(arr_voltages_H150666[0])) > 1000) ? Color.Khaki : SystemColors.Control;
            });
            var progress = progressHandler2 as IProgress<string>;
            string readback = string.Empty;
            try
            {
                await Task.Run(() =>
                {
                    while (true)
                    {
                        for (int s = 0; s < 6 ; s++)
                        {
                            //_cts_volt_dps.Token.ThrowIfCancellationRequested();
                            //arr_voltages[s] = (s == 1) ? arr_voltages[0] - Convert.ToDouble(cb_pass.SelectedItem) : 
                            arr_voltages[s] = DPS.raw_read_syn(s, "U") + " V";
                        }             
                        arr_voltages_H150666[0] = H150666.raw_read_syn(0, "U");
                        arr_voltages_H150666[1] = H150666.raw_read_syn(1, "I");
                        arr_voltages_H150666[2] = H150666.raw_read_syn(2, "I");

                        progress.Report(readback);
                        Thread.Sleep(250);

                        _suspend_background_measurement.WaitOne(Timeout.Infinite);
                    }
                });
            }
            catch (OperationCanceledException ex)
            {
                //await DPS.reset_channels();
                //DPS.Is_session_open = btn_start.Enabled = false;
                //await DPS.dispose();   
                AutoClosingMessageBox.Show(ex.Message,"ERROR",2000);
            }

        }


        private async void background_meas_flow_labjack()
        {
            int handle_flow = 2;
            double cnt_flow_before = 0;
            double cnt_flow_after = 0;
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
                    LJM.eWriteName(handle_flow, "DIO17_EF_INDEX", 7);
                    LJM.eWriteName(handle_flow, "DIO17_EF_ENABLE", 1);

                    while (!_cts_flow_labjack.IsCancellationRequested)
                    {
                        try
                        {
                            LJM.eReadName(handle_flow, "DIO17_EF_READ_A", ref cnt_flow_before);
                            Thread.Sleep(2000);
                            LJM.eReadName(handle_flow, "DIO17_EF_READ_A_AND_RESET", ref cnt_flow_after);
                            //erg2 = (cnt_flow_after - cnt_flow_before) / 750 / 2 * 60;
                            erg2 = (cnt_flow_after - cnt_flow_before);
                            if (progress != null)
                            {
                                progress.Report(erg2.ToString("0.0"));    //no decimal placed
                            }
                        }
                        catch (Exception)
                        {
                            AutoClosingMessageBox.Show("Can'r read flow pin", "LJM Error", 1000);
                            return;
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
            int handle_background_counter = 3;
            double cnt_before = 0;
            double cnt_after = 0;
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
                        LJM.eWriteName(handle_background_counter, "DIO17_EF_INDEX", 7);
                        LJM.eWriteName(handle_background_counter, "DIO17_EF_ENABLE", 1);
                        sw.Start();
                        LJM.eReadName(handle_background_counter, "DIO17_EF_READ_A", ref cnt_before);
                        Thread.Sleep(ct);
                        sw.Stop();
                        LJM.eReadName(handle_background_counter, "DIO17_EF_READ_A_AND_RESET", ref cnt_after);
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
            return median;
        }


        private async void take_XPS_spectra()
        {
            if (_cts_pressure_labjack != null)
            {
                _cts_pressure_labjack.Cancel();
            }

            if (_cts_flow_labjack != null)
            {
                _cts_flow_labjack.Cancel();
            }

            Queue<double> filt_values = new Queue<double>();
            //string dict_entry_smooth = "smooth_deg_4_num_points_" + (Convert.ToInt32(cb_samp_ev.SelectedItem) * 2 + 1).ToString();
            //string dict_entry_deriv = "deriv_deg_4_num_points_" + (Convert.ToInt32(cb_samp_ev.SelectedItem) * 2 + 1).ToString();
            //double[] coeff = sav_gol_coeff[dict_entry_smooth];
            //double[] coeff_deriv = sav_gol_coeff[dict_entry_deriv];
            double timee = 0;
            double p_ak = 0;
            double flow_rate = 0;
            //int handle_DAC2 = 4;
            //int handle_stream = 0;
            int handle_tdac = 14;
            int num_scans = 0;

            int DeviceScanBacklog = 0;
            int LJMScanBacklog = 0;

            bool Is_spot = false;



            double curr_E_B = 0;
            //double E_B_end = 0;
            double E_B_starting = 0;
            int.TryParse(tb_samples_per_second.Text, out int samples_per_second);
            int.TryParse(tb_samples_for_mean.Text, out int samples_for_mean);
            int.TryParse(tb_num_spectra.Text, out int num_spectra);
            Double.TryParse(tb_detailscan_start.Text, out double E_B_start);
            Double.TryParse(tb_detailscan_stop.Text, out double E_B_end);

            double V_photon = (cb_select.SelectedIndex == 0) ? E_Al_Ka : (cb_select.SelectedIndex == 1) ? E_Mg_Ka : E_HeI;

            for (int i = 1; i <= num_spectra; i++)
            {
                lb_num_spectra.Text = (i + "/" + num_spectra).ToString();
                _cts_XPS = new CancellationTokenSource();
                
                btn_start.Enabled = tb_safe.Enabled = false;
                btn_can.Enabled = tb_show.Enabled = true;
                //Iseg_DPS_session.Enabled = Iseg_Xray_session.Enabled = false;

                try
                {
                    LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_stream);
                    LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_DAC2);
                }
                catch (Exception)
                {
                    AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_stream' or 'handle_DAC2' session!", "Info", 500);
                    btn_start.Enabled = tb_safe.Enabled = true;
                    btn_can.Enabled = tb_show.Enabled = false;
                    return;
                }

                LJM.eWriteName(handle_DAC2, "DAC0", Convert.ToDouble(tb_dac.Text));

                //double mean_volt_hemo = 0;
               
                var token = _cts_XPS.Token;
                var progressHandler = new Progress<string>(value =>
                {
                    tb_cps.Text = value;
                    tb_E_B.Text = curr_E_B.ToString("0.00") + " eV";
                    progressBar1.Value = Math.Min(Math.Max(100 - Convert.ToInt32(((E_B_end - curr_E_B) / (E_B_end - E_B_starting + 0.01)) * 100), 0), 100); 
                    lb_progress.Text = progressBar1.Value.ToString() + "%";
                    //lb_progress.Text = Convert.ToInt32(timee).ToString("0") + "ms";
                    tb_pressure.Text = Math.Pow(10, ((Convert.ToDouble(p_ak) - 7.75)) / 0.75).ToString("0.00E0") + " mbar";
                    tb_flow.Text = flow_rate.ToString("0.0") + " l/min";
                });
                var progress = progressHandler as IProgress<string>;

                int counter_LSB = 3036;
                int MSB = 4899;
                int Core_Timer = 61520;
                int AIN0_hemo = 0;
                int counter_flow_LSB = 3034;
                int AIN1_PAK = 2;
                double scanRate = samples_per_second * samples_for_mean;
                int scansPerRead = Convert.ToInt32(scanRate);

                int[] aScanList = new int[] {
                AIN0_hemo,
                counter_LSB, MSB,
                Core_Timer, MSB,
                AIN1_PAK,
                counter_flow_LSB};
                int numAddresses = aScanList.Length;
                int data_length = numAddresses * scansPerRead;
                double[] aData = (cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1 || cb_select.SelectedIndex == 2) ? new double[data_length] : new double[samples_for_mean * numAddresses];
                double[] v_hemo_arr = new double[data_length];
                //double v_ctn_cum = 0;

                // read in desired values for Passenergy, voltage bias, stepsize, time per step and lens voltage
                double vpass = Convert.ToDouble(cb_pass.SelectedItem);
                //if (cb_pass.SelectedIndex == 2)
                //{
                //    offset = 0.9;
                //}
                //double vbias = Convert.ToDouble(cb_bias.SelectedItem);
                //double vbias = 0.0;
                double vbias = LJM_ADC(pin_bias_voltage, 16);
                //vstepsize = Convert.ToDouble(cb_stepwidth.SelectedItem);
                // 0.025 = 100/4000 (100 wg. prozentualer angabe), insg. faktor, mit dem die V/s multipliziert werden müssen
                double voltramp = 0.025 * samples_per_second / Convert.ToDouble(cb_samp_ev.SelectedItem);

                

                if (cb_select.SelectedIndex != 2 || cb_select.SelectedIndex == 2)
                {
                    try
                    {
                        switch (cb_scanrange.SelectedItem.ToString())
                        {
                            case ("survey"):
                                //E_B_end = V_photon;
                                //E_B_end = V_photon > 1200 ? 1200 : V_photon;
                                E_B_end = 1400;
                                set_all_control_voltages(-2, 15, 100, vbias, 0, "XPS");
                                pb_hv_icon.Visible = true;
                                E_B_starting = 0;
                                break;
                            case ("detail"):
                                //Double.TryParse(tb_detailscan_start.Text, out double E_B_start);
                                //Double.TryParse(tb_detailscan_stop.Text, out double E_B_tb_end);
                                if ((E_B_start >= 0) && (E_B_start <= V_photon) && (E_B_end >= 0) && (E_B_end <= V_photon))
                                {
                                    set_all_control_voltages(E_B_start, 15, 100, vbias, 0, "XPS");
                                    //E_B_end = E_B_tb_end;
                                    E_B_starting = E_B_start;
                                    pb_hv_icon.Visible = true;
                                }

                                else
                                {
                                    AutoClosingMessageBox.Show("E_B in range 0 to E_Photon", "Input Error", 2000);
                                    Clear();
                                    return;
                                }
                                break;
                            case ("spot"):
                                
                                if (Double.TryParse(tb_set_E_B.Text.Replace(",", "."), out double U_E_binding))
                                {
                                    set_all_control_voltages(U_E_binding, 15, 100, vbias, 0, "XPS");
                                    pb_hv_icon.Visible = true;
                                    btn_can.Enabled = true;
                                    tb_show.Enabled = true;
                                    tb_safe.Enabled = false;
                                    tb_set_E_B.Text = String.Empty;
                                    tb_set_E_B.Text = tb_set_E_B.Text.ToString();
                                    Is_spot = true;
                                }
                                else
                                {
                                    AutoClosingMessageBox.Show("E_B in range 0 to E_Photon", "Input Error", 2000);
                                    Clear();
                                    return;
                                }
                                break;
                            default:
                                break;
                        }
                        await Task.Delay(8000, token);
                        //await Task.Delay(8000);
                        curr_E_B = E_B_starting;
                    }

                    catch (Exception ex)
                    {
                        DPS_reset();
                        Clear();
                        if (ex is System.Exception)
                        {
                        }
                        else
                        {
                            MessageBox.Show(ex.Message);
                        }                   
                        return;
                    }
                }


                else if(cb_select.SelectedIndex == 2000000000)
                {
                    try
                    {
                        LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_tdac);
                        set_all_control_voltages(0, 15.0, 100, vbias, handle_tdac, "XPS");
                        pb_hv_icon.Visible = true;
                        E_B_end = V_photon + 6.0;
                        await Task.Delay(8000, token);
                    }
                    catch (Exception ex)
                    {
                        Clear();
                        MessageBox.Show(ex.Message);
                        return;
                    }
                }

                else
                {
                    MessageBox.Show("Start-/End-E_B not in a valid range");
                }

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
                //curr_time = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
                curr_time = DateTime.Now.ToString("yyyy-MM-dd__");
                string u = tb_safe.Text + curr_time;
                //DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES_tin\", " " + curr_time + "_" +
                //tb_safe.Text + "_" + cb_pass.SelectedItem + "_" + tb_slit.Text + "\\"));
                DirectoryInfo dl = Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_PES\", " " + curr_time + "XPS" + "\\"));
                path_logfile = dl.FullName;
                string name = tb_safe.Text + "_" + cb_pass.SelectedItem;
                fig_name.Text = name;
                using (var file = new StreamWriter(path_logfile + name + "_header.txt", true))
                {
                    file.WriteLine("XPS-spectrum" + Environment.NewLine);
                    file.WriteLine("Date/time: \t{0}", DateTime.Now.ToString("\t yyyy-MM-dd__HH-mm-ss"));
                    file.WriteLine("" + Environment.NewLine);
                    file.WriteLine("AK pressure: \t{0} \t{1}", tb_pressure.Text, " mbar");
                    file.WriteLine("Pass energy: \t{0} \t{1}", vpass.ToString("0.0"), "\t eV");
                    file.WriteLine("Volt. bias: \t{0} \t{1}", vbias.ToString("0.000"), "\t V");
                    file.WriteLine("E_Photon: \t{0} \t{1}", V_photon.ToString("0.0"), "\t V");
                    file.WriteLine("I_Emission: \t{0} \t{1}", tb_emi.Text, "\t mA");
                    file.WriteLine("V_Anode: \t{0} \t{1}", tb_anode_voltage.Text, "\t V");
                    file.WriteLine("Workfunction: \t{0} \t{1}", workfunction, "\t eV");
                    file.WriteLine("Samples/eV: \t{0} \t{1}", Convert.ToDouble(cb_samp_ev.SelectedItem), "\t 1/s");
                    file.WriteLine("TDAC V_Ref: \t{0} \t{1}", tb_dac.Text.ToString(), "\t V");
                    //file.WriteLine("#V_Lens: \t{0} \t{1}", tb_lens.Text.ToString(), "\t V");
                    file.WriteLine("Factor k: \t{0} \t{1}", k_fac.ToString(), "\t ");
                    file.WriteLine("Slope: \t{0} \t{1}", (voltramp * 4000 / 100).ToString("0.0000"), "\t V/s");
                    file.WriteLine("V_Channelt.: \t{0} \t{1}", vchanneltron, "\t V");
                    file.WriteLine("Flow cooling: \t{0} \t{1}", tb_flow.Text, "\t l/min");
                    file.WriteLine("Ana. Slit: \t{0} \t{1}", tb_slit.Text, "");
                    file.WriteLine("ADC_factor: \t{0} \t{1}", voltage_divider.ToString(), "");
                    file.WriteLine("Samp_mean: \t{0} \t{1}", samples_for_mean.ToString(), "");
                    file.WriteLine("Samp/sec: \t{0} \t{1}", samples_per_second.ToString(), "");
                    //file.WriteLine("#Sav_Gol deg: \t{0}", "4", "");
                    //file.WriteLine("#Sav_Gol Samp: \t{0} ", (samples_per_second * 2).ToString(), "");
                    file.WriteLine("" + Environment.NewLine);
                    //file.WriteLine("" + Environment.NewLine);
                    //file.WriteLine("#E_bind \t cps \t E_kin \t V_hemo");
                    //file.WriteLine("" + Environment.NewLine);
                }
                LJM.eWriteName(handle_stream, "DIO18_EF_INDEX", 7);
                LJM.eWriteName(handle_stream, "DIO18_EF_ENABLE", 1);
                LJM.eWriteName(handle_stream, "DIO17_EF_INDEX", 7);
                LJM.eWriteName(handle_stream, "DIO17_EF_ENABLE", 1);
                LJM.eWriteName(handle_stream, "STREAM_RESOLUTION_INDEX", 6); //Resolution Index  8 still ok?

                LJM.LJMERROR LJMError;
                if (cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1 || cb_select.SelectedIndex == 2)
                {
                    LJMError = LJM.eStreamStart(handle_stream, scansPerRead, numAddresses, aScanList, ref scanRate);
                    Thread.Sleep(20);
                }

                if (_cts_pressure_labjack != null)
                {
                    _cts_pressure_labjack.Cancel();
                }

                if (_cts_flow_labjack != null)
                {
                    _cts_flow_labjack.Cancel();
                }

                string figurename = fig_name.Text;

                try
                {
                    double oldtime = 1;
                    double mean_volt_hemo, ctn_old, t_old;
                    mean_volt_hemo = ctn_old = t_old = 0;
                    double samp_ev = Convert.ToDouble(cb_samp_ev.SelectedItem);
                    double cps_old = 1;
                    Stopwatch sw = new Stopwatch();

                    num_scans = (cb_select.SelectedIndex == 0 || cb_select.SelectedIndex == 1 || cb_select.SelectedIndex == 2) ? scansPerRead : samples_for_mean;

                    if (!Is_spot)
                    //if (cb_select.SelectedIndex != 2 && !Is_spot)
                    {
                        set_all_control_voltages(E_B_end + 10, voltramp, 100, vbias, 0, "XPS");
                    }

                    await Task.Run(() =>
                    {                      
                        //XPS
                        if (num_scans == scansPerRead)
                        {
                            LJMError = LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);

                            if (LJMError == 0)
                            {
                                ctn_old = aData[data_length - 6] + aData[data_length - 5] * 65536;
                                t_old = aData[data_length - 4] + aData[data_length - 3] * 65536 + 1;
                                mean_volt_hemo = aData[data_length - numAddresses];
                            }
                            else
                            {
                                LJM.eStreamStop(handle_stream);
                                _cts_XPS.Cancel();
                                AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode", "LJM Error", 2000);
                            }
                        }


                        //while (hemo_check < 5.0 && !_cts_XPS.IsCancellationRequested)
                        while (true)
                        {
                            //if (oldtime > 20)
                            if (E_B_end < curr_E_B && !Is_spot)
                            {
                                break; //jumps out of await Task.Run();
                            }
                             sw.Reset();

                            //XPS
                            if (num_scans == scansPerRead)
                            {
                                LJMError = LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);
                                if (LJMError == 0)
                                {
                                    sw.Start();
                                    var old_values = data_processing(aData, numAddresses, filt_values, samples_per_second, samples_for_mean, samp_ev,
                                        ctn_old, t_old, mean_volt_hemo, oldtime, cps_old, V_photon, vbias, vpass, k_fac, name);
                                    ctn_old = old_values.Item1;
                                    t_old = old_values.Item2;
                                    //oldtime = old_values.Item3;
                                    flow_rate = old_values.Item3;
                                    curr_E_B = old_values.Item4;
                                    mean_volt_hemo = old_values.Item5;
                                    oldtime = old_values.Item6;
                                    cps_old = old_values.Item7;
                                    p_ak = old_values.Rest.Item1;
                                    //p_ak = old_values.Item8;
                                    progress.Report(cps_old.ToString("000000"));
                                    // don't place _suspend_.. above "result = i" (avoids false allocation of readback and chanel number)
                                    //_suspend_background_measurement.WaitOne(Timeout.Infinite);
                                    timee = sw.Elapsed.TotalMilliseconds;
                                    sw.Stop();
                                    _cts_XPS.Token.ThrowIfCancellationRequested();
                                }
                                else
                                {
                                    LJM.eStreamStop(handle_stream);
                                    _cts_XPS.Cancel();
                                    AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode in while Loop", "LJM Error", 2000);
                                    break;
                                }
                            }

                            //UPS
                            /***
                            if (num_scans == samples_for_mean)
                            {
                                LJMError = LJM.StreamBurst(handle_stream, numAddresses, aScanList, ref scanRate, num_scans, aData);
                                if (LJMError == 0)
                                {
                                    var old_values = data_processing_UPS(aData, numAddresses, num_scans, V_photon, vbias, vpass, oldtime, name);
                                    curr_E_B = old_values.Item1;
                                    oldtime = old_values.Item3;
                                    timee = sw.Elapsed.TotalMilliseconds;
                                    sw.Stop();
                                    _cts_XPS.Token.ThrowIfCancellationRequested();
                                    ups_volt += (ups_step / fac_amp);
                                    LJM.eWriteName(handle_tdac, "TDAC0", ups_volt);
                                    LJM.eWriteName(handle_tdac, "TDAC1", ups_volt + UPS_delta / fac_amp);
                                }

                                else
                                {
                                    _cts_XPS.Cancel();
                                    AutoClosingMessageBox.Show("Unable to read Labjack Stream Mode in while Loop", "LJM Error", 2000);
                                    break;
                                }
                            }
                            ***/
                        }

                        try
                        {
                            zedGraphControl1.Validate();
                            //zedGraphControl1.MasterPane.GetImage().Save(path, System.Drawing.Imaging.ImageFormat.Png);
                            using (MemoryStream memory = new MemoryStream())
                            {
                                using (FileStream fs = new FileStream(path_logfile + "_" + figurename + ".png", FileMode.Create, FileAccess.ReadWrite))
                                {
                                    zedGraphControl1.MasterPane.GetImage().Save(memory, System.Drawing.Imaging.ImageFormat.Png);
                                    byte[] bytes = memory.ToArray();
                                    fs.Write(bytes, 0, bytes.Length);
                                }
                            }
                            //safe_spectra_fig(path_logfile + "_" + figurename + ".png");
                        }
                        catch (Exception ex)
                        {
                            AutoClosingMessageBox.Show(ex.Message, "Zedgraph-Error", 2000);
                        }
                    });
                    
                    btn_can.Enabled = false;
                    btn_clear.Enabled = fig_name.Enabled = true;
                    DPS_reset();
                    if (num_scans == scansPerRead)
                    {
                        LJM.eStreamStop(handle_stream);
                    }
                    if (num_scans == samples_for_mean)
                    {
                        LJM.eWriteName(handle_tdac, "TDAC0", ups_volt);
                        LJM.eWriteName(handle_tdac, "TDAC1", ups_volt + UPS_delta / fac_amp);
                    }
                    tb_cps.Text = String.Empty;
                    progressBar1.Value = 0;
                    lb_progress.Text = String.Empty;

                    using (var file = new StreamWriter(path_logfile + name + "_header.txt", true))
                    {
                        file.WriteLine(Environment.NewLine + "#S C A N  E N D" + Environment.NewLine);
                    }
                    pb_hv_icon.Visible = false;
                    //LJM.CloseAll();
                }

                catch (OperationCanceledException)
                {
                    zedGraphControl1.MasterPane.GetImage().Save(path_logfile + figurename + "_can.png", System.Drawing.Imaging.ImageFormat.Png);
                    btn_can.Enabled = false;
                    btn_clear.Enabled = fig_name.Enabled = true;
                    progressBar1.Value = 0;
                    lb_progress.Text = String.Empty;
                    if (num_scans == scansPerRead)
                    {
                        try
                        {
                            LJM.eStreamStop(handle_stream);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (num_scans == samples_for_mean)
                    {
                        //LJM.eWriteName(handle_tdac, "TDAC0", ups_volt);
                        //LJM.eWriteName(handle_tdac, "TDAC1", ups_volt + UPS_delta / fac_amp);
                    }
                    DPS_reset();
                    tb_cps.Text = String.Empty;
                    using (var file = new StreamWriter(path_logfile + name + "_header.txt", true))
                    {
                        file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D" + Environment.NewLine);
                    }
                    //LJM.CloseAll();
                    pb_hv_icon.Visible = false;
                    return; // jumps into "finally"                
                }

                finally
                {
                    DPS_reset();
                    Iseg_DPS_session.Enabled = Iseg_Xray_session.Enabled = true;
                    Thread.Sleep(20);
                    LJM.CloseAll();
                }

                if (i != num_spectra)
                {
                    Clear();
                }
            }
        }


        public Tuple<double, double, double, double, double, double, double, Tuple<double>> data_processing(
            double[] aData, int numAddresses, Queue<double> filt_values,
            int samples_per_second,
            int samples_for_mean, double samp_ev, double ctn_old, double t_old, 
            double mean_volt_hemo, double oldtime, double cps_old, double V_photon,
            double vbias, double vpass, double k_fac, string name)

        {
            double ctn, ctn_now, t_now, error, E_bind, result, result_deriv, flow_beg, flow_end, flow_rate, test;
            ctn = ctn_now = t_now = error = E_bind = result = result_deriv = flow_beg = flow_end = flow_rate = test = 0;
            double p_ak = 0;
            var inc = 0;
            int l = 0;
            double mean_volt_hemo_old = mean_volt_hemo;

            double[] arr_E_B = new double[samples_per_second];
            double[] arr_cps = new double[samples_per_second];
            double[] arr_mean_volt_hemo = new double[samples_per_second];
            double[] arr_result = new double[samples_per_second];
            double[] arr_time = new double[samples_per_second];
            double[] arr_timer = new double[samples_per_second];

            double[] arr_median = new double[samples_for_mean+1];

            for (int i = 1; i <= samples_per_second; i++)
            {
                inc = (i * samples_for_mean - 1) * numAddresses;

                t_now = aData[inc + 3] + aData[inc + 4] * 65536;
                ctn_now = aData[inc + 1] + aData[inc + 2] * 65536;
                p_ak = aData[inc + 5];

                ctn = ((t_now < t_old) ? (ctn_now - ctn_old) / (t_now - t_old + 4294967295) : (ctn_now - ctn_old) / (t_now - t_old)) * 40000000 + 1;
                ctn = ctn < 0 ? 0 : ctn;

                int p = 0;
                while (l <= i * samples_for_mean - 1)
                {
                    mean_volt_hemo += aData[l * numAddresses + 0];
                    arr_median[p] = aData[l * numAddresses + 0];
                    l++;
                    p++;
                }
                l--;
                p--;
                /***
                using (var file = new StreamWriter(path_logfile + "debug" + ".txt", true))
                {
                    for (int k = 0; k < samples_for_mean; k++)
                    {
                        file.WriteLine(
                        arr_median[k].ToString("0.000000", System.Globalization.CultureInfo.InvariantCulture)
                        );
                    }
                    file.WriteLine("" + Environment.NewLine);
                }
                ***/
                test = median(arr_median);
                mean_volt_hemo = mean_volt_hemo / (samples_for_mean + 1);
                //mean_volt_hemo = (mean_volt_hemo * ctn + mean_volt_hemo_old * cps_old) / (ctn + cps_old);

                ////E_bind = V_photon - workfunction - vbias + mean_volt_hemo * voltage_divider - vpass / k_fac + vpass * 0.4;
                E_bind = V_photon + mean_volt_hemo * voltage_divider - vbias - workfunction - vpass / k_fac + vpass * 0.4 + 0.77 * vpass + offset;
                //E_bind = V_photon + mean_volt_hemo * voltage_divider - vbias - workfunction - vpass;

                // because ctn is measured only for 1/samples_per_second time intervall, and so total poisson-error is samples_per_sec * ctn_in_meas_interval
                error = Math.Sqrt(samples_per_second) * Math.Sqrt(ctn);
                //values_to_plot.Add(oldtime, E_bind);
                values_to_plot.Add(E_bind, ctn);
                values_to_plot_mean.Add(V_photon + test * voltage_divider - vbias - workfunction - vpass / k_fac + vpass * 0.4 + 0.77 * vpass + offset, ctn);


                //myCurve.AddPoint(oldtime, oldtime + 1000);
                errorlist.Add(E_bind, ctn - error, ctn + error);
                //errorlist.Add(oldtime, E_bind - error, E_bind + error);
                //errorCurve.Add(oldtime, ctn - error, ctn + error);

                /***
                //Savitzky Golay filtering
                filt_values.Enqueue(ctn);
                //double result = Sav_Gol(filt_values, coeff, coeff_deriv, 10).Item1;
                //double result_deriv = Sav_Gol(filt_values, coeff, coeff_deriv, 10).Item2;   
                //var results = Sav_Gol(filt_values, coeff, coeff_deriv);

                if (filt_values.Count() == coeff.Length)
                {
                    result = result_deriv = 0;
                    var k = 0;
                    foreach (var item in filt_values)
                    {
                        result += coeff[k] * item;
                        result_deriv += coeff_deriv[k] * item;
                        k++;
                    }
                    filt_values.Dequeue();
                    //values_to_plot_svg.Add(E_bind - samples_per_second / samp_ev, result);
                    //values_to_plot_svg.Add(E_bind, result);
                    //myCurve_svg.AddPoint(oldtime, result);
                    //values_to_plot_svg_deriv.Add(E_bind - samples_per_second / samp_ev, result_deriv);
                   // values_to_plot_svg_deriv.Add(oldtime, result_deriv);
                    //myCurve_svg_deriv.AddPoint(oldtime, result_deriv);
                }

                ***/

                arr_time[i - 1] = ((t_now > t_old) ? (t_now - t_old)  : (t_now - t_old + 4294967295)) / 40000;
                arr_E_B[i - 1] = E_bind;
                arr_cps[i - 1] = ctn;
                arr_mean_volt_hemo[i - 1] = mean_volt_hemo;
                arr_result[i - 1] = result;
                arr_timer[i - 1] = t_now;
                
                //myPane.GraphObjList.Clear();
                //TextObj label = new TextObj(ctn.ToString("0.0"), 1, ctn);
                //label.Location.CoordinateFrame = CoordType.XChartFractionY2Scale;
                //label.Location.AlignH = AlignH.Right;
                //myPane.GraphObjList.Add(label);
                ctn_old = ctn_now;
                t_old = t_now;
                mean_volt_hemo_old = mean_volt_hemo;
                mean_volt_hemo = 0;

                oldtime++;
            }

            // X-Ray HV device cooling
            flow_beg = aData[6];
            flow_end = aData[aData.Length - 1];
            flow_rate = ((flow_end > flow_beg) ? (flow_end - flow_beg) : (flow_end - flow_beg + 65536)) / 750 * 60;

            //mean_volt_hemo = aData[aData.Length - numAddresses];
            //progress.Report(v_ctn_cum.ToString("000000"));
            using (var file = new StreamWriter(path_logfile + name + "_data.txt", true))
            {
                for (int i = 0; i <= samples_per_second-1; i++)
                {
                    file.WriteLine(
                    arr_E_B[i].ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    arr_cps[i].ToString("000000", System.Globalization.CultureInfo.InvariantCulture)
                    //arr_timer[i].ToString("0000000000", System.Globalization.CultureInfo.InvariantCulture)
                    //arr_mean_volt_hemo[i].ToString("0000.000000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    //arr_result[i].ToString("000000.0", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                    //arr_time[i].ToString("000.0000", System.Globalization.CultureInfo.InvariantCulture)
                    //string.Join("\n", aData) + "\t" +
                    //"######################################################################################"
                    );
                }
            }


            try
            {
                if (!_cts_XPS.Token.IsCancellationRequested)
                {
                    zedGraphControl1.Invalidate();
                    zedGraphControl1.AxisChange();
                }
            }
            catch (Exception)
            {
            }

            //return Tuple.Create(ctn_old, t_old, samp_ev, E_bind, mean_volt_hemo_old, oldtime, ctn, p_ak);
            return Tuple.Create(ctn_old, t_old, samp_ev, E_bind, mean_volt_hemo_old, flow_rate, ctn, p_ak);
        }


    public Tuple<double,double,double> data_processing_UPS(double[] aData, int numAddresses, int num_scans, double V_photon, double vbias, double vpass, double oldtime, string name)
    {
        double ctn, ctn_now, t_now, error, E_bind, result, result_deriv, t_old, ctn_old;
        ctn = ctn_now = t_now = error = E_bind = result = result_deriv = t_old = ctn_old = 0.0;
        int l = 0;
        double mean_volt_hemo = 0;
        double volt_div_ups = 5.0;

        t_now = aData[aData.Length - numAddresses + 3] + aData[aData.Length - numAddresses + 4] * 65536;
        t_old = aData[3] + aData[4] * 65536;
        ctn_now = aData[aData.Length - numAddresses + 1] + aData[aData.Length - numAddresses + 2] * 65536;
        ctn_old = aData[1] + aData[2] * 65536;

        ctn = (t_now < t_old) ? (ctn_now - ctn_old) / (t_now - t_old + 4294967295) : (ctn_now - ctn_old) / (t_now - t_old);
        ctn = ctn * 40000000 + 1;

        while (l <= num_scans - 1)
        {
            mean_volt_hemo += aData[l * numAddresses + 0];
            l++;
        }
        l--;
        mean_volt_hemo = mean_volt_hemo / (num_scans);
        //E_bind = mean_volt_hemo * volt_div_ups + V_photon - vbias - vpass / k_fac - workfunction + vpass * 0.4;
        error = Math.Sqrt(40000000/ (t_now - t_old)) * Math.Sqrt(ctn);
        values_to_plot.Add(E_bind, ctn);
        errorlist.Add(E_bind, ctn - error, ctn + error);

        using (var file = new StreamWriter(path_logfile + name + ".txt", true))
        {
            file.WriteLine(
            E_bind.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
            ctn.ToString("000000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
            mean_volt_hemo.ToString("0000.000000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
            //arr_result[i].ToString("000000.0", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
            (40000000 / (t_now - t_old)).ToString("000.0000", System.Globalization.CultureInfo.InvariantCulture)
            );
        }
        oldtime++;

        try
        {
            if (!_cts_XPS.Token.IsCancellationRequested)
            {
                zedGraphControl1.Invalidate();
                zedGraphControl1.AxisChange();
            }
        }
        catch (Exception)
        {
        }

        return Tuple.Create(E_bind, ctn, oldtime);
    }


    private double LJM_ADC(string chanel, int num_read)
        {
            int handle_adc = 10;
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_adc);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_adc' session!", "Info", 500);
                return 0;
            }
            LJM.eWriteName(handle_adc, "AIN" + chanel + "_RESOLUTION_INDEX", 8);
            double voltage = 0;
            double volt = 0;
            for (int i = 0; i < num_read; i++)
            {
                LJM.eReadName(handle_adc, "AIN" + chanel, ref voltage);
                volt += voltage;
                Thread.Sleep(10);
            }
            volt = volt / num_read;
            return volt;
        }

        bool test_abort = false;

        private async void btn_UPS_Click(object sender, EventArgs e)
        {
            int handle_dac = 11;
            try
            {
                LJM.OpenS("T7", LJM_connection_type, "ANY", ref handle_dac);
            }
            catch (Exception)
            {
                AutoClosingMessageBox.Show("Can't open Labjack T7 'handle_adc' session!", "Info", 500);;
            }

            LJM.eWriteName(handle_dac, "TDAC1" , -8.0);

            for (int i = 0; i < 1000; i++)
            {
                double volt = i / 100.0;
                LJM.eWriteName(handle_dac, "TDAC0", volt);
                await Task.Delay(200);

                if (test_abort)
                {
                    break;
                }
            }
            test_abort = false;
        }

        private void btn_stop_UPS_test_Click(object sender, EventArgs e)
        {
            test_abort = true;
        }
    }

    }




/***
 * 
 * https://stackoverflow.com/questions/23037936/read-a-file-and-sort-it-alphabetically-by-certain-rules
 *  var lines = File.ReadLines("inputFilePath")
            .Select(x => x.Split(',').Reverse().ToArray())
            .OrderBy(x => x[0])
            .ThenBy(x => x[1])
            .Select(x => string.Join(" ", x));

    File.WriteAllLines("outputFilePath", lines);

 ***/
