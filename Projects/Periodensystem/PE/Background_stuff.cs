using System;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Threading;
using LabJack;
using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
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
    }

}