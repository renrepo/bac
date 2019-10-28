using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace double_pendulum
{
    class Program
    {
        

        static void Main(string[] args)
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            const double pi = Math.PI;
            Stopwatch sw = new Stopwatch();
            double time = 0;
            //double dt = 0.00001;
            //double end = 25;

            double m1 = 0.1;
            double m2 = 0.1;
            double m = m1 + m2;
            double l1 = 0.1;
            double l2 = 0.1;

            double g = 9.81;
            //int N = Convert.ToInt32(end / dt);
            double ti = 0.0;
            double k11, k12, k13, k14 = 0.0;
            double k21, k22, k23, k24 = 0.0;
            double k31, k32, k33, k34 = 0.0;
            double k41, k42, k43, k44 = 0.0;
            double dk11, dk21, dk31, dk41 = 0.0;
            double dk12, dk22, dk32, dk42 = 0.0;
            double dk13, dk23, dk33, dk43 = 0.0;
            double q;

            //double[] test = new double[N];

            double DGL_q1(double q1, double q2, double p1, double p2, double f)
            {
                return (l2 * p1 - l1 * p2 * Math.Cos(q1 - q2)) / (l1 * f);
            }

            double DGL_q2(double q1, double q2, double p1, double p2, double f)
            {
                return (l1 * m * p2 - l2 * m2 * p1 * Math.Cos(q1 - q2)) / (l2 * m2 * f);
            }

            double DGL_p1(double q1, double q2, double p1, double p2, double C)
            {
                return -m * g * l1 * Math.Sin(q1) - C;
            }

            double DGL_p2(double q1, double q2, double p1, double p2, double C)
            {
                return -m2 * g * l2 * Math.Sin(q2) + C;
            }


            double RK4(double y, double k1, double k2, double k3, double k4, double dt)
            {
                return y + dt * (k1 + 2 * k2 + 2 * k3 + k4) / 6;
            }

            /***
            double H(double q1, double q2, double p1, double p2)
            {
                return 0.5 * (m1 + m2) * l1 * l1 * q1 * q1 + 0.5 * m2 * l2 * l2 * p2 * p2 + m2 * l1 * l2 * p1 * p2 * Math.Cos(q1 - q2) - (m1 + m2) * g * l1 * Math.Cos(q1) - m2 * g * l2 * Math.Cos(q2);
            }
            ***/
            
            double First_Rollover(int num, double q1, double q2, double p1, double p2, double dt)
            {
                if (Math.Cos(q2) + 2.0 * Math.Cos(q1) > 1.0)
                {
                    return num;
                }
               
                else
                {
                    for (int i = 0; i < num; i++)
                    {
                        
                        ti = i * dt;
                        double s = Math.Sin(q1 - q2);
                        double f = l1 * l2 * (m1 + m2 * s * s);
                        double con = l2 * l2 * m2 * p1 * p1 + l1 * l1 * m * p2 * p2;
                        double C1 = p1 * p2 * s / f;
                        double C2 = (con - 2 * l1 * l2 * m2 * p1 * p2 * Math.Cos(q1 - q2)) * Math.Sin(2 * (q1 - q2)) / (2 * f * f);
                        double C = C1 - C2;

                        k11 = DGL_q1(q1, q2, p1, p2, f);
                        k21 = DGL_q2(q1, q2, p1, p2, f);
                        k31 = DGL_p1(q1, q2, p1, p2, C);
                        k41 = DGL_p2(q1, q2, p1, p2, C);

                        dk11 = q1 + k11 * dt / 2;
                        dk21 = q2 + k21 * dt / 2;
                        dk31 = p1 + k31 * dt / 2;
                        dk41 = p2 + k41 * dt / 2;



                        s = Math.Sin(dk11 - dk21);
                        f = l1 * l2 * (m1 + m2 * s * s);
                        C1 = p1 * p2 * s / f;
                        C2 = (con - 2 * l1 * l2 * m2 * p1 * p2 * Math.Cos(dk11 - dk21)) * Math.Sin(2 * (dk11 - dk21)) / (2 * f * f);
                        C = C1 - C2;

                        k12 = DGL_q1(dk11, dk21, dk31, dk41, f);
                        k22 = DGL_q2(dk11, dk21, dk31, dk41, f);
                        k32 = DGL_p1(dk11, dk21, dk31, dk41, C);
                        k42 = DGL_p2(dk11, dk21, dk31, dk41, C);

                        dk12 = q1 + k12 * dt / 2;
                        dk22 = q2 + k22 * dt / 2;
                        dk32 = p1 + k32 * dt / 2;
                        dk42 = p2 + k42 * dt / 2;



                        s = Math.Sin(dk12 - dk22);
                        f = l1 * l2 * (m1 + m2 * s * s);
                        C1 = p1 * p2 * s / f;
                        C2 = (con - 2 * l1 * l2 * m2 * p1 * p2 * Math.Cos(dk12 - dk22)) * Math.Sin(2 * (dk12 - dk22)) / (2 * f * f);
                        C = C1 - C2;

                        k13 = DGL_q1(dk12, dk22, dk32, dk42, f);
                        k23 = DGL_q2(dk12, dk22, dk32, dk42, f);
                        k33 = DGL_p1(dk12, dk22, dk32, dk42, C);
                        k43 = DGL_p2(dk12, dk22, dk32, dk42, C);

                        dk13 = q1 + k13 * dt;
                        dk23 = q2 + k23 * dt;
                        dk33 = p1 + k33 * dt;
                        dk43 = p2 + k43 * dt;



                        s = Math.Sin(dk13 - dk23);
                        f = l1 * l2 * (m1 + m2 * s * s);
                        C1 = p1 * p2 * s / f;
                        C2 = (con - 2 * l1 * l2 * m2 * p1 * p2 * Math.Cos(dk13 - dk23)) * Math.Sin(2 * (dk13 - dk23)) / (2 * f * f);
                        C = C1 - C2;

                        k14 = DGL_q1(dk13, dk23, dk33, dk43, f);
                        k24 = DGL_q2(dk13, dk23, dk33, dk43, f);
                        k34 = DGL_p1(dk13, dk23, dk33, dk43, C);
                        k44 = DGL_p2(dk13, dk23, dk33, dk43, C);

                        q1 = RK4(q1, k11, k12, k13, k14, dt);
                        q2 = RK4(q2, k21, k22, k23, k24, dt);
                        p1 = RK4(p1, k31, k32, k33, k34, dt);
                        p2 = RK4(p2, k41, k42, k43, k44, dt);

                       
                        if (q2 > Math.PI || q2 < -Math.PI)
                        { 
                            return i;
                        }
                    }
                    return num;
                }

            }


            int len = 4;
            int N;
            double[] erg;
            int[] Size = new int[len];
            double[] Dt = new double[len];
            double[] End = new double[len];

            /***
             * 720_0.01_25 etwa 10 Minuten
             * 
             * ***/
            Size = new int[] { 720, 720, 720, 720, 720, 720};
            Dt = new double[] { 0.1, 0.01, 0.001, 0.0001, 0.001, 0.001};
            End = new double[] { 20, 20, 20, 20, 40, 10};

            int ctn;
            

            for (int k = 0; k < len; k++)
            {
                erg = new double[Size[k] * Size[k] * 2];
                N = Convert.ToInt32(End[k]/Dt[k]);
                ctn = 0;
                double dt = Dt[k];
                int size = Size[k];
                double frac = Math.PI / size;

                for (int i = 0; i < size; i++)
                {
                    for (int j = -size; j < size; j++)
                    {
                        erg[ctn] = First_Rollover(N, j * frac, i * frac, 0, 0, dt);
                        ctn++;
                    }
                    Console.WriteLine(time + "size = " + Size[k] + "   dt = " + Dt[k] +  + k + "   end = " + End[k] + "     i = " + i + "  time = " + erg[ctn - 1]);
                }
                
                using (StreamWriter writetext = new StreamWriter(String.Format("erg_{0}_{1}_{2}.txt", Size[k], Dt[k], End[k])))
                {
                    for (int i = 0; i < erg.Length; i++) { writetext.WriteLine(erg[i]); }
                }
            }
            




            /***
            double x1 = 0.0;
            double x2 = 1.45;
            Console.WriteLine(x1 / 2 / Math.PI * 360);
            Console.WriteLine(x2 / 2 / Math.PI * 360);
            Console.WriteLine(First_Rollover(N, x1, x2, 0, 0));
            //Console.WriteLine(2*Math.Cos(x1) + Math.Cos(x2));
            Console.ReadKey();
            
            using (StreamWriter writetext = new StreamWriter("test.txt"))
            {
                for (int i = 0; i < test.Length; i++)
                {
                    writetext.WriteLine(test[i]);
                }
            }
            ***/
        }
    }
}
