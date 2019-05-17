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
        int na;
        double fac, ex, arg;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            arg = (x - a[1]) / a[2];
            ex = Math.Exp(- arg * arg * ln);
            fac = a[0] * ex * 2.0 * arg;
            y = a[0] * ex;
            dyda[0] = ex;
            dyda[1] = fac / a[2];
            dyda[2] = fac * arg / a[2];            
        }
    }


    public class LorentzianFunction : LMAFunction
    {
        int na;
        double arg, L, c, s, o, u;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            arg = (x - a[1]) / a[2];
            L = 1.0 / (1.0 + 4.0 * arg * arg);

            c = 2.0 * a[0] * arg / a[2];
            s = L * L;

            u = a[1] - 2.0;
            o = a[1] + 2.0;

            dyda[0] = L - r / (a[0] * a[0]);
            dyda[1] = c * s - r * (o - u) * (o + u - 2.0 * a[1]) / ((a[1] - o) * (a[1] - o) * (u - a[1]) * (u - a[1]));
            dyda[2] = c * arg * s - r / (a[2] * a[2]);

            y = a[0] * L + r * (1.0 / a[0] + 1.0 / a[2] + (u - o) / ((a[1] - u) * (a[1] - o)));         
        }
    }


    public class GLP : LMAFunction
    {
        int na;
        double L, m, G, arg, c, sg, sl, o, u;

        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            m = a[3];
            arg = (x - a[1]) / a[2];
            G = Math.Exp(- ln2 * (1.0 - m) * arg * arg);
            L = 1.0 / (1.0 + 4.0 * m * arg * arg);
            c = 2.0 * a[0] * arg / a[2];
            sg =  G * (1.0 - m) * ln2;
            sl = 4.0 * m * L * L;

            u = a[1] - 2.0;
            o = a[1] + 2.0;

            // derivatives OK -- checked with wolframalpha 14.05.2019
            dyda[0] = L * G - r / (a[0] * a[0]);
            dyda[1] = c * sg * L + c * sl * G - r * (o - u) * (o + u - 2.0 * a[1]) / ((a[1] - o) * (a[1] - o) * (u - a[1]) * (u - a[1]));
            dyda[2] = c * arg * sl * G + c * arg * sg * L - r / (a[2] * a[2]);
            dyda[3] = -4.0 * a[0] * arg * arg * L * L * G + a[0] * ln2 * arg * arg * G * L;// + r * (2.0 * m - 1.0) / (m * m * (1.0 - m) * (1.0 - m));

            //y += a[0] * (m * L + (1.0 - m) * G) / a[2];// + (100.0/(m * (1.0 -m )) + 1.0/a[0] + (u - o)/((a[i+1]-u)*(a[1]-o)) + 1.0 / a[i+2]);
            y = a[0] * L * G + r * (1.0 / a[0] + 1.0 / a[2] + (u - o) / ((a[1] - u) * (a[1] - o)));// + (1.0 / (m * (1.0 - m))));                  
        }
    }


    public class GLS : LMAFunction
    {
        int na;
        double L, m, G, arg, c, s, u, o;
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            na = a.Count();
            arg = (x - a[1]) / a[2];
            //G = Math.Exp(-ln * arg * arg) * sqln / sqpi;
            //L = 1.0 / (1.0 + arg * arg) / pi;
            G = Math.Exp(-ln2 * arg * arg);
            L = 1.0 / (1.0 + 4.0 * arg * arg);
            m = a[3];
            c = 2.0 * a[0] * arg / a[2];
            s = sqln * G * (1.0 - m) + m * pi * L * L;

            u = a[1] - 2.0;
            o = a[1] + 2.0;

            dyda[0] = (m * L + (1.0 - m) * G) / a[2] - r / (a[0] * a[0]);
            //dyda[i] = (m * L + (1.0 - m) * G) / a[2] - r / (a[i] * a[i]); //old version with pi and so on
            //dyda[1] = c * s / a[2] - r * (o-u)*(o+u-2.0*a[i+1])/((a[1] - o) * (a[1] - o) * (u - a[1]) * (u - a[1]));
            dyda[1] = c * s - r * (o - u) * (o + u - 2.0 * a[1]) / ((a[1] - o) * (a[1] - o) * (u - a[1]) * (u - a[1]));
            //dyda[2] = - 1.0 / a[2] / a[2] * a[i] * (m * L + (1.0 - m) * G) + c * arg * s - r / (a[2] * a[2]); //old version with pi and so on
            dyda[2] = c * arg * s - r / (a[2] * a[2]);
            dyda[3] = a[0] * (L - G) / a[2];// + r * Math.Pow((2.0 * m - 1.0) / (m * m * (1.0 - m) * (1.0 - m)), 9);

            //y += a[i] * (m * L + (1.0 - m) * G) / a[2] + (1.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[1]-o)) + 1.0 / a[i+2]); //old version with pi and so on
            y = a[0] * (m * L + (1.0 - m) * G) + r * (1.0 / a[0] + 1.0 / a[2] + (u - o) / ((a[1] - u) * (a[1] - o))) ;// + (1.0 / (m * (1.0 - m))));       
        }
    }


    public class custom : LMAFunction
    {
        LMAFunction f;
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
                        f = GLS;
                        break;
                    case "GLP":
                        f = GLP;
                        break;
                    case "G":
                        f = G_raw;
                        break;
                    case "L":
                        f = L_raw;
                        break;                   
                }
                f.GetY(x, ref a_part, ref y, ref dyda_part, r);
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

