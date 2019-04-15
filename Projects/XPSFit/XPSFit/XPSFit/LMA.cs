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

        const int NDONE = 4, ITMAX = 1000;
        int ndat, ma, mfit;
        private double[] x, y, sig;
        double tol;
        LMAFunction func;
        bool[] ia;
        double[] a;
        double[,] covar;
        double[,] alpha;
        double chisq;

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

            for (int i = 0; i < ma; i++) ia[i] = true;
        }

        #endregion //-----------------------------------------------





        #region Properties

        public double[] A
        {
            get { return a; }
            set { a = value; }

        }

        #endregion //-----------------------------------------------






    
        #region Methods

        public void hold(int i, double val) { ia[i] = false; a[i] = val; }  
        public void free(int i) { ia[i] = true; }


        public void fit()
        {
            int j, k, l, iter, done = 0;
            double alambda = 0.001;
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
                
                for (j = 0; j < mfit; j++) 
                {
                    for (k = 0; k < mfit; k++) covar[j, k] = temp[j, k];
                    da[j] = oneda[j,0];
                }

                if (done == NDONE) // Converged. Clean up and return.
                {
                    covstr(ref covar);
                    covstr(ref alpha);
                    A = a;
                    return;
                }

                for (j = 0, l = 0; l < ma; l++) // Did the trial succeed?                
                {
                    if (ia[l]) atry[l] = a[l] + da[j++];
                }
                mrqcof(ref atry, ref covar, ref da);
                if (Math.Abs(chisq - ochisq) < Math.Max(tol, tol * chisq)) done++;
                if (chisq < ochisq) // success, accept new solution
                {
                    alambda *= 0.1;
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
                    alambda *= 10;
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
                for (k = 0; k <= j; k++) alpha[j, k] = 0.0; beta[j] = 0.0;
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
                if (a[icol, icol] == 0.0) MessageBox.Show("gaussj: Singular Matrix");
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

        #endregion //-----------------------------------------------
    }
}
