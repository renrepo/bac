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
        void GetY(double x, ref double[] a, ref double y, ref double[] dyda);

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
        public abstract void GetY(double x, ref double[] a, ref double y, ref double[] dyda);

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

            for (int i = 0; i < xValues.Length; i++)
            {
                //yValues[i] = GetY(xValues[i], a);
                //yValues[i] = GetY(xValues[i], a) + i / 500.0;
            }

            return new double[][] { xValues, yValues };
        }


    }


    public class GaussianFunction : LMAFunction
    {
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda)
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
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda)
        {
            int i, na = a.Count();
            double fac, ex, arg;
            y = 0.0;
            for (i = 0; i < na - 1; i += 3)
            {
                arg = (x - a[i + 1]) / a[i + 2];
                ex = Math.Exp(-Math.Pow(arg, 2));
                if (a.Length % 3 != 0) MessageBox.Show("Invalid number of parameters for Gaussian");
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


    public class CustomFunction : LMAFunction
    {
        public override void GetY(double x, ref double[] a, ref double y, ref double[] dyda)
        {
            double ln = - 4.0 * Math.Log(2.0);
            double delta = 0.000001;
            int i, na = a.Count();
            double fac, ex, arg, G, L, V, argG, argL1, argL2, m, L1, L2;
            double T = 1.0;
            double dplus, dminus;
            y = 0.0;

            double[] erg = new double[1 + dyda.Length];
            double[] newParam = new double[a.Length];
            for (int l = 0; l < a.Length; l++)
                newParam[l] = a[l];


            
            for (i = 0; i < na - 1; i += 5)
            {
                argL1 = (x - a[i + 1]) / a[i + 2];
                argL2 = (x - a[i + 1] - 0.416) / a[i + 2];
                argG = (x - a[i + 1]) / a[i + 3];            
                m = a[i + 4] * 0.01;

                G = Math.Exp(ln * Math.Pow(argG, 2));
                L1 = 1.0 / (1.0 + 4.0 * Math.Pow(argL1, 2));
                L2 = 1.0 / (1.0 + 4.0 * Math.Pow(argL2, 2)) / 2.0;
                L = L1 + L2;

                dyda[i] = m * L + (1.0 - m) * G;
                dyda[i + 1] = a[i] * (m * 8.0 * argL1 * Math.Pow(L1, 2) / a[i + 2] + m * 16.0 * argL2 * Math.Pow(L2, 2) / a[i + 2] - (1.0 - m) * 2.0 * argG * ln * G / a[i + 3]);
                dyda[i + 2] = a[i] * (m * 8.0 * Math.Pow(argL1, 2) * Math.Pow(L1,2) / a[i + 2] + m * 16.0 * Math.Pow(argL2, 2) * Math.Pow(L2, 2) / a[i + 2]);
                dyda[i + 3] = - a[i] * ((1.0 - m) * 2.0 * Math.Pow(argG, 2) * ln * G / a[i + 3]);
                dyda[i + 4] = a[i] * (L - G);

                if (x > 368.4 && x < 368.6)
                {
                    for (int h = 0; h < 5; h++)
                    {
                        Console.WriteLine(a[h]);
                    }
                    Console.WriteLine("");
                    Console.WriteLine(dyda[i + 3]);
                    Console.WriteLine(dyda[i + 2]);
                    Console.WriteLine("");

                }

                V = (m * L + (1.0 - m) * G) * a[i];

                //if (a[i + 5] != 0) T = x < a[i + 1] ? Math.Exp(-a[i + 5] * argL1) : 1.0;

                //y = a[i] *  (V + (1.0 - V) * T);
                y = V;
            }

            
            /***
            
            y = 0.0;
            for (i = 0; i < na - 1; i += 5)
            {
                argG = (x - a[i + 1]) / a[i + 3];
                argL1 = (x - a[i + 1]) / a[i + 2];
                argL2 = (x - a[i + 1] - 0.416) / a[i + 2];

                G = Math.Exp(ln * Math.Pow(argG, 2) * (1 - a[i + 4] * 0.01));
                L = 1.0 / (1.0 + a[i + 4] * 0.04 * Math.Pow(argL1, 2)) + 1.0 / (1.0 + a[i + 4] * 0.04 * Math.Pow(argL2, 2)) / 2.0;

                V = L * G;

                //if (a[i + 5] != 0) T = x < a[i + 1] ? Math.Exp(-a[i + 5] * argL1) : 1.0;

                //y = a[i] *  (V + (1.0 - V) * T);
                y += a[i] * V;
            }

            for (int s = 0; s < na; s++)
            {
                dplus = 0.0;
                dminus = 0.0;
                newParam[s] += delta;
                for (i = 0; i < na - 1; i += 5)
                {
                    argG = (x - newParam[i + 1]) / newParam[i + 3];
                    argL1 = (x - newParam[i + 1]) / newParam[i + 2];
                    argL2 = (x - newParam[i + 1] - 0.416) / newParam[i + 2];

                    G = Math.Exp(ln * Math.Pow(argG, 2) * (1 - newParam[i + 4] * 0.01));
                    L = 1.0 / (1.0 + newParam[i + 4] * 0.04 * Math.Pow(argL1, 2)) + 1.0 / (1.0 + newParam[i + 4] * 0.04 * Math.Pow(argL2, 2)) / 2.0;

                    V = L * G;

                    //if (newParam[i + 5] != 0) T = x < newParam[i + 1] ? Math.Exp(-newParam[i + 5] * argL1) : 1.0;

                    //dplus = newParam[i] * (V + (1.0 - V) * T);
                    dplus += newParam[i] * V;
                }
                newParam[s] -= 2.0 * delta;
                for (i = 0; i < na - 1; i += 5)
                { 
                    argG = (x - newParam[i + 1]) / newParam[i + 3];
                    argL1 = (x - newParam[i + 1]) / newParam[i + 2];
                    argL2 = (x - newParam[i + 1] - 0.416) / newParam[i + 2];

                    G = Math.Exp(ln * Math.Pow(argG, 2) * (1.0 - newParam[i + 4] * 0.01));
                    L = 1.0 / (1.0 + newParam[i + 4] * 0.04 * Math.Pow(argL1, 2)) + 1.0 / (1.0 + newParam[i + 4] * 0.04 * Math.Pow(argL2, 2)) / 2.0;

                    V = L * G;

                    //if (newParam[i + 5] != 0) T = x < newParam[i + 1] ? Math.Exp(-newParam[i + 5] * argL1) : 1.0;

                    //dminus = newParam[i] * (V + (1.0 - V) * T);
                    dminus += newParam[i] * V;                  
                }               
                newParam[s] += delta;
                dyda[s] = (dplus - dminus) / (2.0 * delta);
            }
             
            ***/



        }
    }


}


/***
 * Ableitung mit Originalpunkt und dminus (statt dminus und dplus)
 ***/

