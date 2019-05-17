using System;
using System.Linq;

namespace XPSFit
{
    /***
    interface ILMAFunction
    {
        void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r);
    }
    ***/

    public abstract class LMAFunction //: ILMAFunction
    {
        static public double ln = Math.Log(2.0);
        static public double sqln = Math.Sqrt(ln);
        static public double pi = Math.PI;
        static public double sqpi = Math.Sqrt(pi);
        static public double ln2 = 4.0 * ln;

        public string[] Models { get; set; }

        public abstract void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r);
    }

    

    public class GaussianFunction : LMAFunction
    {
        int i, na;
        double fac, ex, arg;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            y = 0.0;
            for (i = 0; i < na - 1; i += 4)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                ex = Math.Exp(- arg * arg * ln);
                fac = a[i] * ex * 2.0 * arg;
                y += a[i] * ex;
                dyda[i] = ex;
                dyda[i + 1] = fac / a[i + 2];
                dyda[i + 2] = fac * arg / a[i + 2];
            }
        }
    }


    public class LorentzianFunction : LMAFunction
    {
        int i, na;
        double arg, L, c, s, o, u;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            y = 0.0;
            for (i = 0; i < na - 1; i += 4)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                L = 1.0 / (1.0 + 4.0 * arg * arg);

                c = 2.0 * a[i] * arg / a[i + 2];
                s = L * L;

                u = a[i + 1] - 2.0;
                o = a[i + 1] + 2.0;

                dyda[i] = L - r / (a[i] * a[i]);
                dyda[i + 1] = c * s - r * (o - u) * (o + u - 2.0 * a[i + 1]) / ((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                dyda[i + 2] = c * arg * s - r / (a[i + 2] * a[i + 2]);

                y += a[i] * L + r * (1.0 / a[i] + 1.0 / a[i + 2] + (u - o) / ((a[i + 1] - u) * (a[i + 1] - o)));
            }
        }
    }


    public class GLP : LMAFunction
    {
        int i, na;
        double L, m, G, arg, c, sg, sl, o, u;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            y = 0.0;         
            for (i = 0; i < na - 1; i += 4)
            {
                m = a[i + 3];
                arg = (x - a[i + 1]) / a[i + 2];
                G = Math.Exp(- ln2 * (1.0 - m) * arg * arg);
                L = 1.0 / (1.0 + 4.0 * m * arg * arg);
                c = 2.0 * a[i] * arg / a[i + 2];
                sg =  G * (1.0 - m) * ln2;
                sl = 4.0 * m * L * L;

                u = a[i + 1] - 2.0;
                o = a[i + 1] + 2.0;

                // derivatives OK -- checked with wolframalpha 14.05.2019
                dyda[i] = L * G - r / (a[i] * a[i]);
                dyda[i + 1] = c * sg * L + c * sl * G - r * (o - u) * (o + u - 2.0 * a[i + 1]) / ((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                dyda[i + 2] = c * arg * sl * G + c * arg * sg * L - r / (a[i + 2] * a[i + 2]);
                dyda[i + 3] = -4.0 * a[i] * arg * arg * L * L * G + a[i] * ln2 * arg * arg * G * L;// + r * (2.0 * m - 1.0) / (m * m * (1.0 - m) * (1.0 - m));

                //y += a[i] * (m * L + (1.0 - m) * G) / a[i + 2];// + (100.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[i + 1]-o)) + 1.0 / a[i+2]);
                y += a[i] * L * G + r * (1.0 / a[i] + 1.0 / a[i + 2] + (u - o) / ((a[i + 1] - u) * (a[i + 1] - o)));// + (1.0 / (m * (1.0 - m))));       
            }
        }
    }


    public class GLS : LMAFunction
    {
        int i, na;
        double L, m, G, arg, c, s, u, o;
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            y = 0.0;
            for (i = 0; i < na - 1; i += 4)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                //G = Math.Exp(-ln * arg * arg) * sqln / sqpi;
                //L = 1.0 / (1.0 + arg * arg) / pi;
                G = Math.Exp(-ln2 * arg * arg);
                L = 1.0 / (1.0 + 4.0 * arg * arg);
                m = a[i + 3];
                c = 2.0 * a[i] * arg / a[i + 2];
                s = sqln * G * (1.0 - m) + m * pi * L * L;

                u = a[i + 1] - 2.0;
                o = a[i + 1] + 2.0;

                dyda[i] = (m * L + (1.0 - m) * G) / a[i + 2] - r / (a[i] * a[i]);
                //dyda[i] = (m * L + (1.0 - m) * G) / a[i + 2] - r / (a[i] * a[i]); //old version with pi and so on
                //dyda[i + 1] = c * s / a[i + 2] - r * (o-u)*(o+u-2.0*a[i+1])/((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                dyda[i + 1] = c * s - r * (o - u) * (o + u - 2.0 * a[i + 1]) / ((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                //dyda[i + 2] = - 1.0 / a[i + 2] / a[i + 2] * a[i] * (m * L + (1.0 - m) * G) + c * arg * s - r / (a[i + 2] * a[i + 2]); //old version with pi and so on
                dyda[i + 2] = c * arg * s - r / (a[i + 2] * a[i + 2]);
                dyda[i + 3] = a[i] * (L - G) / a[i + 2];// + r * Math.Pow((2.0 * m - 1.0) / (m * m * (1.0 - m) * (1.0 - m)), 9);

                //y += a[i] * (m * L + (1.0 - m) * G) / a[i + 2] + (1.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[i + 1]-o)) + 1.0 / a[i+2]); //old version with pi and so on
                y += a[i] * (m * L + (1.0 - m) * G) + r * (1.0 / a[i] + 1.0 / a[i + 2] + (u - o) / ((a[i + 1] - u) * (a[i + 1] - o))) ;// + (1.0 / (m * (1.0 - m))));       
            }
        }
    }


    public class custom : LMAFunction
    {
        GLS GLS = new GLS();
        GLP GLP = new GLP();
        GaussianFunction G_raw = new GaussianFunction();
        LorentzianFunction L_raw = new LorentzianFunction();
        int i, na;
        double y_old;
        double[] a_part, dyda_part;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            y = y_old = 0.0;
            for (i = 0; i < na - 1; i += 4)
            {
                a_part = new double[] { a[i], a[i + 1], a[i + 2], a[i + 3] };
                dyda_part = new double[] { dyda[i], dyda[i + 1], dyda[i + 2], dyda[i + 3] };

                switch (Models[i / 4])
                {
                    case "GLS":
                        GLS.GetY(x, ref a_part, ref y, ref dyda_part, r);
                        break;
                    case "GLP":
                        GLP.GetY(x, ref a_part, ref y, ref dyda_part, r);
                        break;
                    case "G":
                        G_raw.GetY(x, ref a_part, ref y, ref dyda_part, r);
                        break;
                    case "L":
                        L_raw.GetY(x, ref a_part, ref y, ref dyda_part, r);
                        break;                   
                }
                y_old += y;
                dyda[i] = dyda_part[0];
                dyda[i + 1] = dyda_part[1];
                dyda[i + 2] = dyda_part[2];
                dyda[i + 3] = dyda_part[3];
            }
            y = y_old;
        }
    }
}



/***
 * Ableitung mit Originalpunkt und dminus (statt dminus und dplus)
 ***/

