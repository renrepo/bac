using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XPSFit
{
    interface ILMAFunction
    {
        /// <summary>
        /// Returns the y value of the function for
        /// the given x and vector of parameters
        /// </summary>
        /// <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
        /// <param name="a">The fitting parameters. </param>
        /// <returns></returns>
        void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r);

        /// <summary>
        /// The method which gives the partial derivates used in the LMA fit.
        /// If you can't calculate the derivate, use a small <code>a</code>-step (e.g., <i>da</i> = 1e-20)
        /// and return <i>dy/da</i> at the given <i>x</i> for each fit parameter.
        /// </summary>
        /// <param name="x">The <i>x</i>-value for which the partial derivate is calculated.</param>
        /// <param name="a">The fitting parameters.</param>
        /// <param name="parameterIndex">The parameter index for which the partial derivate is calculated.</param>
        /// <returns>The partial derivate of the function with respect to parameter <code>parameterIndex</code> at <i>x</i>.</returns>
        //double GetPartialDerivative(double x, double[] a, int parameterIndex);

    }

    public abstract class LMAFunction : ILMAFunction
    {
        /// <summary>
        /// Returns the y value of the function for
        /// the given x and vector of parameters
        /// </summary>
        /// <param name="x">The <i>x</i>-value for which the <i>y</i>-value is calculated.</param>
        /// <param name="a">The fitting parameters. </param>
        /// <returns></returns>
        public abstract void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r);

        /***
        /// <summary>
        /// The method which gives the partial derivates used in the LMA fit.
        /// If you can't provide the functional derivative, use a small <code>a</code>-step (e.g., <i>da</i> = 1e-20)
        /// and return <i>dy/da</i> at the given <i>x</i> for each fit parameter.
        /// This is provided in the method below as a default implementation
        /// </summary>
        /// <param name="x">The <i>x</i>-value for which the partial derivate is calculated.</param>
        /// <param name="a">The fitting parameters.</param>
        /// <param name="parameterIndex">The parameter index for which the partial derivate is calculated.</param>
        /// <returns>The partial derivative of the function with respect to parameter <code>parameterIndex</code> at <i>x</i>.</returns>
        public virtual double GetPartialDerivative(double x, double[] a, int parameterIndex)
        {
            //kk 25 Jun 2010
            //this value has been changed to 1*10-9 from 1*10-14 after a hint by a user
            //who was having issues with convergence on some gaussian function

            double delta = 0.000000001;
            double[] newParam = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                newParam[i] = a[i];

            newParam[parameterIndex] = a[parameterIndex] + delta;
            double dplusResult = GetY(x, newParam);

            newParam[parameterIndex] = a[parameterIndex] - delta;
            double dminusResult = GetY(x, newParam);

            double result = (dplusResult - dminusResult) / (2 * delta);

            return result;
        }
        ***/

        /// <summary>
        /// Returns array of x,y values, given x and fitting parameters
        /// used by all tests to generate test data for exact fits
        /// </summary>
        /// <param name="xValues">x values</param>
        /// <param name="a">fitting parameters</param>
        /// <returns>point values</returns>
        public double[][] GenerateData(double[] a, double[] xValues)
        {
            double[] yValues = new double[xValues.Length];
            double[] dy = new double[a.Length];

            for (int i = 0; i < xValues.Length; i++)
            {
                GetY(xValues[i], ref a, ref yValues[i], ref dy, 0.0);
                //yValues[i] = GetY(xValues[i], a) + i / 500.0;
            }

            return new double[][] { xValues, yValues };
        }


    }


    public class GaussianFunction : LMAFunction
    {
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            double ln = 4.0 * Math.Log(2.0);
            int i, na = a.Count();
            double fac, ex, arg;
            y = 0.0;
            for (i = 0; i < na - 1; i += 3)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                ex = Math.Exp(-Math.Pow(arg, 2) * ln);
                if (a.Length % 3 != 0) MessageBox.Show("Invalid number of parameters for Gaussian");
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
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            int i, na = a.Count();
            double fac, ex, arg;
            y = 0.0;
            for (i = 0; i < na - 1; i += 3)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                ex = Math.Exp(-Math.Pow(arg, 2));
                //if (a.Length % 3 != 0) MessageBox.Show("Invalid number of parameters for Gaussian");
                fac = a[i] * ex * 2.0 * arg;
                y += a[i] * ex;
                dyda[i] = ex;
                dyda[i + 1] = fac / a[i + 2];
                dyda[i + 2] = fac * arg / a[i + 2];
            }

            /***
            double delta = 0.000000001;
            double[] newParam = new double[a.Length];
            for (int i = 0; i < a.Length; i++)
                newParam[i] = a[i];

            newParam[parameterIndex] = a[parameterIndex] + delta;
            double dplusResult = GetY(x, newParam);

            newParam[parameterIndex] = a[parameterIndex] - delta;
            double dminusResult = GetY(x, newParam);

            double result = (dplusResult - dminusResult) / (2 * delta);

            return result;
            ***/
        }
    }


    public class GLP : LMAFunction
    {
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            int i, na = a.Count();
            double L, m, G, arg, c, s, sg, sl;
            double u;
            double o;
            y = 0.0;
            double ln2 =  4.0 * Math.Log(2.0);

            
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
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda, double r)
        {
            int i, na = a.Count();
            double L, m, G, arg, c, s, sg, sl;
            double u;
            double o;
            y = 0.0;
            double ln = Math.Log(2.0);
            double sqln = Math.Sqrt(ln);
            double pi = Math.PI;
            double sqpi = Math.Sqrt(pi);


            for (i = 0; i < na - 1; i += 4)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                G = Math.Exp(-ln * arg * arg) * sqln / sqpi;
                L = 1.0 / (1.0 + arg * arg) / pi;
                m = a[i + 3];
                c = 2.0 * a[i] * arg / a[i + 2];
                s = sqln * G * (1.0 - m) + m * pi * L * L;

                u = a[i + 1] - 2.0;
                o = a[i + 1] + 2.0;

                dyda[i] = (m * L + (1.0 - m) * G) / a[i + 2] - r / (a[i] * a[i]);
                //dyda[i + 1] = c * s / a[i + 2] - r * (o-u)*(o+u-2.0*a[i+1])/((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                dyda[i + 1] = c * s - r * (o - u) * (o + u - 2.0 * a[i + 1]) / ((a[i + 1] - o) * (a[i + 1] - o) * (u - a[i + 1]) * (u - a[i + 1]));
                dyda[i + 2] = - 1.0 / a[i + 2] / a[i + 2] * a[i] * (m * L + (1.0 - m) * G) + c * arg * s - r / (a[i + 2] * a[i + 2]);
                dyda[i + 3] = a[i] * (L - G) / a[i + 2];// + r * Math.Pow((2.0 * m - 1.0) / (m * m * (1.0 - m) * (1.0 - m)), 9);

                //y += a[i] * (m * L + (1.0 - m) * G) / a[i + 2] + (1.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[i + 1]-o)) + 1.0 / a[i+2]);
                y += a[i] * (m * L + (1.0 - m) * G) / a[i + 2] + r * (1.0 / a[i] + 1.0 / a[i + 2] + (u - o) / ((a[i + 1] - u) * (a[i + 1] - o))) ;// + (1.0 / (m * (1.0 - m))));       
            }
        }
    }
}



/***
 * Ableitung mit Originalpunkt und dminus (statt dminus und dplus)
 ***/

