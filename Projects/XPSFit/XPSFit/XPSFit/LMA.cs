using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace XPSFit
{
    struct LMA
    {
        #region Fields

        const int NDONE = 8, ITMAX = 1000;
        int ndat, ma, mfit;
        private double[] x, y, sig;
        double tol;
        LMAFunction func;
        bool[] ia;
        double[] a;
        double[,] covar;
        double[,] alpha;
        double chisq;
        bool singular_matrix;

        #endregion //-----------------------------------------------


        #region Constructor

        public LMA(LMAFunction f, ref double[] xx, ref double[] yy, ref double[] ssig,
                ref double[] aa, double TOL = 0.001 )
        {
            this.ndat = xx.Count();
            this.ma = aa.Count();
            this.x = xx;
            this.y = yy;
            this.sig = ssig;
            this.tol = TOL;
            this.func = f;
            this.ia = new bool[ma];
            this.alpha = new double[ma, ma];
            this.a = aa;
            this.covar = new double[ma,ma];

            this.chisq = 0; //-------------------------------------------------------------------------- mandatory when using struct! (+ performance?)
            this.mfit = 0;
            this.singular_matrix = false;

            for (int i = 0; i < ma; i++) ia[i] = true;
        }

        #endregion //-----------------------------------------------





        #region Properties

        public double[] A
        {
            get { return a; }
            set { a = value; }
        }

        public double Chi2
        {
            get { return chisq / (ndat - ma); }
            set { chisq = value; }
        }

        #endregion //-----------------------------------------------







        #region Methods

        public void hold(int i, double val) { ia[i] = false; a[i] = val; }  
        public void free(int i) { ia[i] = true; }


        public void fit()
        {
            int j, k, l, iter, done = 0;
            double alambda = 0.001;
            //double alambda = 0.1;
            double ochisq;
            double[] atry = new double[ma];
            double[] beta = new double[ma];
            double[] da = new double[ma];
            mfit = 0;
            for (j = 0; j < ma; j++) if (ia[j]) mfit++;
            double[,] oneda = new double[mfit,1];
            double[,] temp = new double[mfit, mfit];
            mrqcof(ref a, ref alpha, ref beta);

            for (j = 0; j < ma; j++) atry[j] = a[j];
            ochisq = chisq;
            for (iter = 0; iter < ITMAX; iter++)
            {
                Console.WriteLine(iter);
                if (done == NDONE) alambda = 0.0;
                for (j = 0; j < mfit; j++)
                {
                    for (k = 0; k < mfit; k++) covar[j, k] = alpha[j, k];
                    covar[j, j] = alpha[j, j] * (1.0 + alambda);
                    for (k = 0; k < mfit; k++) temp[j, k] = covar[j, k];
                    oneda[j, 0] = beta[j];                                     
                }

                gaussj(ref temp, ref oneda); // Matrix solution.
                if (singular_matrix) return;
                
                for (j = 0; j < mfit; j++) 
                {
                    for (k = 0; k < mfit; k++) covar[j, k] = temp[j, k];
                    da[j] = oneda[j,0];
                }

                if (done == NDONE) // Converged. Clean up and return.
                {
                    covstr(ref covar);
                    covstr(ref alpha);
                    //A = a;
                    //Chi2 = chisq;
                    return;
                }

                for (j = 0, l = 0; l < ma; l++) // Did the trial succeed?                
                {
                    if (ia[l])
                    {
                        //atry[l] = a[l] + da[j++];
                        
                        if ((l == 2 && (atry[l] > 4.0 || atry[l] < 0.5)) || (l == 7 && (atry[l] > 4.0 || atry[l] < 0.5)))
                        {
                            atry[l] = 1.0;
                            //a[l] = 1.9;
                        }

                        else if ((l == 3 && (atry[l] > 4.0 || atry[l] < 0.5)) || (l == 8 && (atry[l] > 4.0 || atry[l] < 0.5)))
                        {
                            atry[l] = 1.0;
                            //a[l] = 1.9;
                        }

                        else if ((l == 4 && (atry[l] > 100.0 || atry[l] < 0.0)) || (l == 9 && (atry[l] > 100.0 || atry[l] < 0.0)))
                        {
                            //a[l] = a[l] > 50 ? 80 : 20;
                            atry[l] = atry[l] > 50.0 ? 80.0 : 20.0;
                            //a[l] = 50.0;
                        }
                        else
                        {
                            atry[l] = a[l] + da[j++];
                        }
                        
                    }//-------------------------------------------------------- ADD CONSTRAINTS HERE?!
                }
                mrqcof(ref atry, ref covar, ref da);
                if (Math.Abs(chisq - ochisq) < Math.Max(tol, tol * chisq)) done++;
                if (chisq < ochisq) // success, accept new solution
                {
                    alambda *= 0.05;
                    ochisq = chisq;
                    for (j = 0; j < mfit; j++)
                    {
                        for (k = 0; k < mfit; k++)
                        {
                            alpha[j, k] = covar[j, k];
                            beta[j] = da[j];
                        }
                    }
                    for (l = 0; l < ma; l++) a[l] = atry[l];
                }
                else     // Failure, increase alambda.
                {
                    alambda *= 5;
                    chisq = ochisq;
                }
            }
            MessageBox.Show("Fitmrq too many iterations");        
        }


        private void mrqcof(ref double[] a, ref double[,] alpha, ref double[] beta)
        {
            int i, j, k, l, m = 0;
            double ymod = 0.0; //----------------------------------------------------------------------------OK????
            double wt, sig2i, dy;
            double[] dyda = new double[ma];

            for (j = 0; j < mfit; j++)  // Initialize (symmetric) alpha, beta
            {
                for (k = 0; k <= j; k++) alpha[j, k] = 0.0;
                beta[j] = 0.0;
            }
            chisq = 0.0;
            for (i = 0; i < ndat; i++)
            {
                func.GetY(x[i], ref a, ref ymod, ref dyda);
                sig2i = 1.0 / (sig[i] * sig[i]);
                dy = y[i] - ymod;
                for (j = 0, l = 0; l < ma; l++)
                {
                    if (ia[l])
                    {
                        wt = dyda[l] * sig2i;
                        for (k = 0, m = 0; m < l + 1; m++)
                        {
                            if (ia[m]) alpha[j,k++] += wt * dyda[m];
                        }
                        beta[j++] += dy * wt;
                    }
                }
                chisq += dy * dy * sig2i;
            }
            for (j = 1; j < mfit; j++)
            {
                for (k = 0; k < j; k++) alpha[k, j] = alpha[j, k];
            }
        }

        private void gaussj(ref double[,] a, ref double[,] b)
        {
            int i, icol, irow, j, k, l, ll, n = a.GetLength(0), m = b.GetLength(1);
            icol = irow = 0;
            double big, dum, pivinv;
            int[] indxc = new int[n];
            int[] indxr = new int[n];
            int[] ipiv = new int[n];
            for (j = 0; j < n; j++) ipiv[j] = 0;
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                {
                    if (ipiv[j] != 1)
                    {
                        for (k = 0; k < n; k++)
                        {
                            if (ipiv[k] == 0)
                            {
                                if (Math.Abs(a[j, k]) >= big)
                                {
                                    big = Math.Abs(a[j, k]);
                                    irow = j;
                                    icol = k;
                                }
                            }
                        }
                    }
                }
                ++(ipiv[icol]);
                if (irow != icol)
                {
                    for (l = 0; l < n; l++) SWAP(ref a[irow, l], ref a[icol, l]);
                    for (l = 0; l < m; l++) SWAP(ref b[irow, l], ref b[icol, l]);
                }
                indxr[i] = irow;
                indxc[i] = icol;
                if (a[icol, icol] == 0.0)
                {
                    MessageBox.Show("gaussj: Singular Matrix");
                    singular_matrix = true;
                    break;
                }
                pivinv = 1.0 / a[icol, icol];
                a[icol, icol] = 1.0;
                for (l = 0; l < n; l++) a[icol, l] *= pivinv;
                for (l = 0; l < m; l++) b[icol, l] *= pivinv;
                for (ll = 0; ll < n; ll++)
                {
                    if (ll != icol)
                    {
                        dum = a[ll,icol];
                        a[ll,icol] = 0.0;
                        for (l = 0; l < n; l++) a[ll,l] -= a[icol,l] * dum;
                        for (l = 0; l < m; l++) b[ll,l] -= b[icol,l] * dum;
                    }
                }
            }

            for (l = n - 1; l >= 0; l--)
            {
                if (indxr[l] != indxc[l])
                {
                    for (k = 0; k < n; k++) SWAP(ref a[k,indxr[l]], ref a[k,indxc[l]]);
                }
            }
        }


        private void covstr(ref double[,] covar)
        {
            int i, j, k;
            for (i = mfit; i < ma; i++)
            {
                for (j = 0; j < i + 1; j++) covar[i, j] = covar[j, i] = 0.0;
            }
            k = mfit - 1;
            for (j = ma - 1; j >= 0; j--)
            {
                if (ia[j])
                {
                    for (i = 0; i < ma; i++) SWAP(ref covar[i,k], ref covar[i,j]);
                    for (i = 0; i < ma; i++) SWAP(ref covar[k,i], ref covar[j,i]);
                    k--;
                }
            }
        }

        private void SWAP(ref double a, ref double b)
        {
            var temp = a;
            a = b;
            b = temp;
        }


        public double SIGN(double a, double b)
        {
            return (b >= 0 ? (a >= 0 ? a : -a) : (a >= 0 ? -a : a));
        }


        public double result(double[] a, double x)
        {
            double G, L, m;
            double f = 0.0;

            for (int i = 0; i < a.Length - 1; i += 5)
            {
                G = Math.Exp(-4.0 * Math.Log(2) * Math.Pow(x / a[i + 3], 2));
                L = 1.0 / (1.0 + 4.0 * Math.Pow(x / a[i + 2], 2));
                m = a[i + 4] * 0.01;

                f += (m * L + (1.0 - m) * G);
            }
            f -= 0.5;
            return f;
        }


        public double GetHWHM(double[] a, double x1, double x2)
        {
            double fl = result(a, x1);
            double fh = result(a, x2);
            double xacc = 0.00001;
            const int MAXIT = 60;
            if ((fl > 0.0 && fh < 0.0) || (fl < 0.0 && fh > 0.0))
            {
                double xl = x1;
                double xh = x2;
                double ans = -9.99e99;
                for (int j = 0; j < MAXIT; j++)
                {
                    double xm = 0.5 * (xl + xh);
                    //Console.WriteLine("FWHM Iteration {0} at value {1}", j, xm);
                    double fm = result(a, xm);
                    double s = Math.Sqrt(fm * fm - fl * fh);
                    if (s == 0.0) return ans;
                    double xnew = xm + (xm - xl) * ((fl >= fh ? 1.0 : -1.0) * fm / s);
                    if (Math.Abs(xnew - ans) <= xacc) return ans;
                    ans = xnew;
                    double fnew = result(a, ans);
                    if (fnew == 0.0) return ans;
                    if (SIGN(fm, fnew) != fm)
                    {
                        xl = xm;
                        fl = fm;
                        xh = ans;
                        fh = fnew;
                    }
                    else if (SIGN(fl, fnew) != fl)
                    {
                        xh = ans;
                        fh = fnew;
                    }
                    else if (SIGN(fh, fnew) != fh)
                    {
                        xl = ans;
                        fl = fnew;
                    }
                    else MessageBox.Show("never get here.");
                    if (Math.Abs(xh - xl) <= xacc) return ans;
                    
                }
                MessageBox.Show("zriddr exceed maximum iterations");
            }
            else
            {
                if (fl == 0.0) return x1;
                if (fh == 0.0) return x2;
                MessageBox.Show("root must be bracketed in zriddr.");
            }
            return 0;
        }




        public double GetHWHM_WDB(double[] aa, double x1, double x2)
        {
            const int ITMAX = 100;
            double EPS = Math.Pow(2, -52);
            double Tol = 0.00001;
            
            double a = x1, b = x2, c = x2, fa = result(aa, a), fb = result(aa, b), fc, p, q, r, s, tol1, xm;
            double e = 0.0;
            double d = 0.0;
            if ((fa > 0.0 && fb > 0.0) || (fa < 0.0 && fb < 0.0))
                MessageBox.Show("Root must be bracketed in zbrent");
            fc = fb;
            for (int iter = 0; iter < ITMAX; iter++)
            {
                if ((fb > 0.0 && fc > 0.0) || (fb < 0.0 && fc < 0.0))
                {
                    c = a;
                    fc = fa;
                    e = d = b - a;
                }
                if (Math.Abs(fc) < Math.Abs(fb))
                {
                    a = b;
                    b = c;
                    c = a;
                    fa = fb;
                    fb = fc;
                    fc = fa;
                }
                tol1 = 2.0 * EPS * Math.Abs(b) + 0.5 * Tol;
                xm = 0.5 * (c - b);
                Console.WriteLine("FWHM Iteration {0} at value {1}", iter, b);
                if (Math.Abs(xm) <= tol1 || fb == 0.0) return b;
                if (Math.Abs(e) >= tol1 && Math.Abs(fa) > Math.Abs(fb))
                {
                    s = fb / fa;
                    if (a == c)
                    {
                        p = 2.0 * xm * s;
                        q = 1.0 - s;
                    }
                    else
                    {
                        q = fa / fc;
                        r = fb / fc;
                        p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
                        q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                    }
                    if (p > 0.0) q = -q;
                    p = Math.Abs(p);
                    double min1 = 3.0 * xm * q - Math.Abs(tol1 * q);
                    double min2 = Math.Abs(e * q);
                    if (2.0 * p < (min1 < min2 ? min1 : min2))
                    {
                        e = d; 
                        d = p / q;
                    }
                    else
                    {
                        d = xm;
                        e = d;
                    }
                }
                else
                {                   
                    d = xm;
                    e = d;
                }
                a = b;
                fa = fb;
                if (Math.Abs(d) > tol1) b += d;
                else
                {
                    b += SIGN(tol1, xm);
                    fb = result(aa, b);
                }

            }
           MessageBox.Show("Maximum number of iterations exceeded in zbrent");
            return 0;
        }

        #endregion //-----------------------------------------------
    }
}
