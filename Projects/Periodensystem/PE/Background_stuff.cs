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
                        LJM.eReadName(handle_pressure_ak, pin_pressure_ak, ref ionivac_v_out);
                        pressure = Math.Pow(10, ((Convert.ToDouble(ionivac_v_out) - 7.75)) / 0.75);
                        Thread.Sleep(2000);
                        if (progress != null)
                        {
                            progress.Report(pressure.ToString("0.00E0"));
                            token.ThrowIfCancellationRequested();
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
                        token.ThrowIfCancellationRequested();
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
                    while (true)
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


        private async void background_counter_labjack()
        {
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
                            //progress.Report(erg.ToString("N0"));    //no decimal placed
                            progress.Report(erg.ToString("0000000"));
                            token.ThrowIfCancellationRequested();
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


        private double scan(int[] aScanList, int samples_per_second, int samples_for_mean, int scansPerRead)
        {
            int DeviceScanBacklog = 0;
            int LJMScanBacklog = 0;
            int inc = 0;
            int l = 0;
            double ctn = 0;
            double ctn_old = 0;
            double ctn_now = 0;
            double t_now;
            double t_old = 0;
            double mean_volt_hemo = 0;
            int numAddresses = aScanList.Length;
            int data_length = numAddresses * scansPerRead;
            double[] aData = new double[data_length];
            double voltramp = 0.125 * 1 / Convert.ToDouble(cb_samp_ev.SelectedItem);
            double hemo_check = 0;

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
                //file.WriteLine("#E_b \t counts");    
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("" + Environment.NewLine);
                file.WriteLine("#E_bind \t cps \t E_kin \t V_hemo");
                file.WriteLine("" + Environment.NewLine);
            }


            LJM.eStreamRead(handle_stream, aData, ref DeviceScanBacklog, ref LJMScanBacklog);


            for (int i = 1; i <= samples_per_second; i++)
            {
                inc = (i * samples_for_mean - 1) * numAddresses;
                t_now = aData[inc + 3] + aData[inc + 4] * 65536;
                ctn_now = aData[inc + 1] + aData[inc + 2] * 65536;
                if (t_now < t_old)
                {
                    ctn = (ctn_now - ctn_old) / (t_now - t_old + 4294967295) * 40000000;
                }
                else
                {
                    ctn = (ctn_now - ctn_old) / (t_now - t_old) * 40000000;
                }
                ctn_old = ctn_now;
                t_old = t_now;

                while (l <= i * samples_for_mean - 1)
                {
                    mean_volt_hemo += aData[l * numAddresses + 0];
                    l++;
                }
                l--;
                mean_volt_hemo = mean_volt_hemo / (samples_for_mean + 1) * voltage_divider;
                //STIMMT DAS???????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????????
                // wie lange ist die scan-time? 220ms? hängt das von den samples_for_mean ab?

                //oldtime += 1;

                //E_pass = (v_hemi - v_hemo) / k;
                E_pass = vpass / k;
                v_analyser = mean_volt_hemo + vpass * 0.4;
                // because (V_analyser - V_bias)*e + E_kin - workfunction = E_pass NO?!
                // hv = E_B + E_Pass + V_bias - V_Aanaly + W_S
                E_kin = E_pass - v_analyser + vbias;
                E_B = V_photon - E_kin - workfunction - correction_offset;

                values_to_plot.Add(E_B, ctn);
                myCurve.AddPoint(E_B, ctn);


                using (var file = new StreamWriter(path_logfile + "data" + ".txt", true))
                {
                    file.WriteLine(
                        E_B.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                        ctn.ToString("0", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                        E_kin.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                        //v_analyser.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                        //v_hemi.ToString("0000.00", System.Globalization.CultureInfo.InvariantCulture) + "\t" +
                        mean_volt_hemo.ToString("0000.000", System.Globalization.CultureInfo.InvariantCulture)
                        //v_pass_meas.ToString("000.00", System.Globalization.CultureInfo.InvariantCulture)
                        //(elapsed_seconds * 1000).ToString("000", System.Globalization.CultureInfo.InvariantCulture)
                        );
                }
                hemo_check = mean_volt_hemo / (samples_for_mean + 1) * voltage_divider;
                //progress.Report(ctn.ToString("000000"));
                mean_volt_hemo = 0;
                //token.ThrowIfCancellationRequested();
            }

            l = 0;
            mean_volt_hemo = aData[data_length - numAddresses];

            return 1;
        }
    }

}