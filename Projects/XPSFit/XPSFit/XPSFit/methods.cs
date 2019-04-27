using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace XPSFit
{
    class methods
    {
        #region Fields



        #endregion //-------------------------------------------------------------------------------------





        #region Properties



        #endregion //-------------------------------------------------------------------------------------





        #region Constructor



        #endregion //-------------------------------------------------------------------------------------





        #region Methods

        public Tuple<List<double>, List<double>, string> get_values_to_plot()
        {
            try
            {
                // var list = new List<List<double>>();
                var list_cps = new List<double>();
                var list_energy = new List<double>();
                string file_name = string.Empty;
                //PointPairList ppl = new PointPairList();
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var lines = File.ReadLines(openFileDialog.FileName);
                        string[] lin;
                        foreach (var line in lines)
                        {
                            lin = line.Split('\t');
                            list_energy.Add(Convert.ToDouble(lin[0].Replace(',','.'), System.Globalization.CultureInfo.InvariantCulture));
                            list_cps.Add(Convert.ToDouble(lin[1].Replace(',', '.'), System.Globalization.CultureInfo.InvariantCulture));
                        }
                        file_name = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                    }
                    return Tuple.Create(list_energy, list_cps, file_name);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

        }


        public List<double> Shirley(double[] x_data, double[] y_data, int iterations)
        {
            int data_length = x_data.Length;
            double I_max = y_data[data_length - 1];
            double I_min = y_data[0];
            double[] B_n = new double[data_length];
            double[] B_n_old = new double[data_length];
            double fak = 1.0;
            int tester = 0;

            for (int k = 0; k < data_length; k++)
            {
                B_n_old[k] = 0.0;
                B_n[k] = 0.0;
            }

            //number of iterations
            for (int iter = 0; iter < iterations; iter++)
            {
                for (int i = 1; i < data_length; i++)
                {
                    // Integral from E to E_max
                    for (int j = i; j < data_length; j++)
                    {
                        B_n[i] += ((x_data[j] - x_data[j - 1])) * (0.5 * (y_data[j] + y_data[j - 1]) - I_max - B_n_old[j - 1]);
                    }
                    if (i == 1)
                    {
                        fak = 0.0;
                        for (int l = 1; l < data_length; l++)
                        {
                            fak += ((x_data[l] - x_data[l - 1])) * (0.5 * (y_data[l] + y_data[l - 1]) - I_max - B_n_old[l - 1]);
                        }
                    }
                    B_n[i] *= (I_min - I_max) / fak;
                }

                for (int t = 0; t < data_length; t++)
                {
                    if (Math.Abs(B_n_old[t] - B_n[t]) < 0.0001) tester += 1;
                    B_n_old[t] = B_n[t];
                    B_n[t] = 0.0;
                }
                if (tester > data_length - 5) break;
            }
            for (int i = 0; i < data_length; i++) { B_n_old[i] += I_max; }

            if (x_data.Count() > 50)
            {
                for (int i = 6; i < 15; i++)
                {
                    B_n_old[i] = (B_n_old[i - 5] + B_n_old[i - 4] + B_n_old[i - 3] + B_n_old[i - 2] + B_n_old[i - 1] + B_n_old[i] + B_n_old[i + 1] + B_n_old[i + 2] + B_n_old[i + 3] + B_n_old[i + 4] + B_n_old[i + 5]) / 11.0;
                    B_n_old[data_length - i] = (B_n_old[data_length - i + 1] + B_n_old[data_length - i + 2] + B_n_old[data_length - i + 3] + B_n_old[data_length - i + 4] + B_n_old[data_length - i + 5] + B_n_old[data_length - i] +
                        B_n_old[data_length - i - 1] + B_n_old[data_length - i - 2] + B_n_old[data_length - i - 3] + B_n_old[data_length - i - 4] + B_n_old[data_length - i - 5]) / 11.0;

                }
            }
            B_n_old[0] = B_n_old[1];

            //smooth_start /= 50;
            //smooth_end /= 50;
            //B_n_old[0] = smooth_start;
            //B_n_old[data_length - 1] = smooth_end;
            //smooth_start = smooth_end = 0;

            return B_n_old.ToList();
        }


        public List<double> Linear(double[] x_data, double[] y_data)
        {
            double smooth_start = 0;
            double smooth_end = 0;
            int data_length = x_data.Length;
            double[] result = new double[data_length];

            double m = (y_data[data_length - 1] - y_data[0]) / (x_data[data_length - 1] - x_data[0]);
            double n = y_data[0] - m * x_data[0];

            for (int i = 0; i < data_length; i++)
            {
                result[i] = m * x_data[i] + n;
            }

            for (int i = 1; i < 11; i++)
            {
                smooth_start += result[i];
                smooth_end += result[data_length - i];
            }
            smooth_start /= 10;
            smooth_end /= 10;
            result[0] = smooth_start;
            result[data_length - 1] = smooth_end;
            smooth_start = smooth_end = 0;
            return result.ToList();
        }

        public Tuple<List<double>, List<double>> discreter(List<double> x, List<double> y, int binsize)
        {
            List<double> x_bin = new List<double>();

            int tester;
            for (int i = 0; i < x.Count; i++)
            {
                tester = Convert.ToInt16((x[i] * 1000) / binsize);
                x_bin.Add(((x[i] * 1000) % binsize < binsize/2 ? tester: tester + 1) * binsize / 1000.0);
            }
            var a1 = x_bin.ToArray();
            var a2 = y.ToArray();
            Array.Sort(a1, a2);
            List<double> y_sort = new List<double>(a2);
            List<double> x_sort = new List<double>(a1);

            List<double> x_res = new List<double>();
            List<double> y_res = new List<double>();
            double y_ = y_sort[0]; ;
            int ctn = 1;

            for (int i = 0; i < x_sort.Count - 1; i++)
            {
                if (x_sort[i + 1] == x_sort[i])
                {
                    ctn++;
                    y_ += y_sort[i + 1];
                }
                else
                {
                    x_res.Add(x_sort[i]);
                    y_res.Add(y_ / ctn);
                    ctn = 1;
                    y_ = y_sort[i + 1];
                }
            }
            return Tuple.Create(x_res, y_res);
        }



        public void SavGol(List<double> x, List<double> y, double[] c, int np, int nl, int nr, int ld, int m)
        {
            int j, k, imj, ipj, kk, mm;
            double fac, sum;
            if (np < nl + nr + 1 || nl < 0 || nr < 0 || ld > m || nl + nr < m)
                MessageBox.Show("bad args in savgol");
            int[] indx = new int[m + 1];
            double[,] a = new double[m + 1,m + 1];
            double[] b = new double[m + 1];

            for (ipj = 0; ipj <= (m << 1); ipj++)
            {

                sum = ipj > 0 ? 0.0 : 1.0;
                for (k = 1; k <= nr; k++) sum += Math.Pow((double)k, (double)ipj);
                for (k = 1; k <= nl; k++) sum += Math.Pow((double)-k, (double)ipj);
                mm = Math.Min(ipj, 2 * m - ipj);
                for (imj = -mm; imj <= mm; imj += 2) a[(ipj + imj) / 2, (ipj - imj) / 2] = sum;
            }

            LUdcmp alud = new LUdcmp(ref a);

            for (j = 0; j < m + 1; j++) b[j] = 0.0;
            b[ld] = 1.0;

            alud.solve(b, out b);
            for (kk = 0; kk < np; kk++) c[kk] = 0.0;
            for (k = -nl; k <= nr; k++)
            {
                sum = b[0];
                fac = 1.0;
                for (mm = 1; mm <= m; mm++) sum += b[mm] * (fac *= k);
                kk = (np - k) % np;
                c[kk] = sum;
            }
            
        }



        public double GetArea(List<double> xvals, List<double> yvals)
        {
            if (xvals.Count != yvals.Count)
            {
                MessageBox.Show("x and y must have the same length");
                return 0;
            }
            double Area = 0.0;
            double ym = 0.0;
            double yp = 0.0;
            double xm = 0.0;
            double xp = 0.0;
            double nom = 0.0;
            for (int i = 0; i < xvals.Count - 2; i++)
            {
                ym = yvals[i];
                yp = yvals[i + 1];
                xm = xvals[i];
                xp = xvals[i + 1];
                nom = (((yp > 0 ? yp : -yp) + (ym > 0 ? ym : -ym))) / 2.0;
                Area += (nom > 0 ? nom : -nom) * (xp > xm ? (xp - xm) : (xm - xp));
            }
            return Area;
        }




        #endregion //-------------------------------------------------------------------------------------





        #region Events



        #endregion //-------------------------------------------------------------------------------------




        


    }










    public class ZScoreOutput
    {
        public List<double> input;
        public List<int> signals;
        public List<double> avgFilter;
        public List<double> filtered_stddev;
    }

    public static class ZScore
    {
        public static ZScoreOutput StartAlgo(List<double> input, int lag, double threshold, double influence)
        {
            // init variables!
            int[] signals = new int[input.Count];
            double[] filteredY = new List<double>(input).ToArray();
            double[] avgFilter = new double[input.Count];
            double[] stdFilter = new double[input.Count];

            var initialWindow = new List<double>(filteredY).Skip(0).Take(lag).ToList();

            avgFilter[lag - 1] = Mean(initialWindow);
            stdFilter[lag - 1] = StdDev(initialWindow);

            for (int i = lag; i < input.Count; i++)
            {
                if (Math.Abs(input[i] - avgFilter[i - 1]) > threshold * stdFilter[i - 1])
                {
                    signals[i] = (input[i] > avgFilter[i - 1]) ? 10000 : -1;
                    filteredY[i] = influence * input[i] + (1 - influence) * filteredY[i - 1];
                }
                else
                {
                    signals[i] = 0;
                    filteredY[i] = input[i];
                }

                // Update rolling average and deviation
                var slidingWindow = new List<double>(filteredY).Skip(i - lag).Take(lag + 1).ToList();

                var tmpMean = Mean(slidingWindow);
                var tmpStdDev = StdDev(slidingWindow);

                avgFilter[i] = Mean(slidingWindow);
                stdFilter[i] = StdDev(slidingWindow);
            }

            // Copy to convenience class 
            var result = new ZScoreOutput();
            result.input = input;
            result.avgFilter = new List<double>(avgFilter);
            result.signals = new List<int>(signals);
            result.filtered_stddev = new List<double>(stdFilter);

            return result;
        }

        private static double Mean(List<double> list)
        {
            // Simple helper function! 
            return list.Average();
        }

        private static double StdDev(List<double> values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                double avg = values.Average();
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                ret = Math.Sqrt((sum) / (values.Count() - 1));
            }
            return ret;
        }
    }



}


