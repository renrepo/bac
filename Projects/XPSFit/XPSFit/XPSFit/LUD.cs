using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPSFit
{
    class LUdcmp
    {
        //--- variables -----------------
        private Int32 n;
        private double[,] lu;
        private Int32[] indx;
        private double d;
        //-------------------------------

        //--- Constructor -----------------------
        public LUdcmp(ref double[,] ca)
        {
            n = ca.GetLength(0);
            indx = new Int32[n];
            lu = ca;

            Decompose();
        }
        //---------------------------------------

        //--- Methods----------------------------------------
        private void Decompose()
        {
            // Given a matrix a[0..n-1][0..n-1], this routine replaces it by the LU decomposition of a
            // rowwise permutation of itself. a is input. On output, it is arranged as in equation(2.3.14)
            // above; indx[0..n-1] us an output vector hat records the row permutation effected by the
            // partial pivoting; d is output as +-1 depending on whether the number of row interchanges
            // was even or odd, respectively. This routine is used in combination with 'solve' to solve linear
            // equations or invert a matrix

            // O.Müller 18-05-2014
            // Numerical Recipes rd Edition 

            double TINY = 1.0e-40;  // A small number
            Int32 i, imax, j, k;
            double big, temp;
            double[] vv = new double[n];    // vv stores the implicit scaling of each row

            d = 1.0;    // no row interchanges yet

            // Loop over rows to get the implicit scaling information
            for (i = 0; i < n; i++)
            {
                big = 0.0;
                for (j = 0; j < n; j++)
                {
                    if ((temp = Math.Abs(lu[i,j])) > big)
                    {
                        big = temp;
                    }
                }

                if (big == 0.0) { System.Windows.Forms.MessageBox.Show("Singular matrix in LUdcmp"); }
                // No nonzero largest element

                vv[i] = 1.0 / big;  // save the scaling
            }


            // This is the outermost kij loop.
            for (k = 0; k < n; k++)
            {
                big = 0.0;  // Initialize for the search for largest pivot element.
                imax = k;

                for (i = k; i < n; i++)
                {
                    temp = vv[i] * Math.Abs(lu[i,k]);

                    if (temp > big)
                    {
                        big = temp;
                        imax = i;
                    }
                }

                if (k != imax)              // Is the figure of merit for the pivot better than the best so far?
                {
                    for (j = 0; j < n; j++) // Yes, do so ...
                    {
                        temp = lu[imax,j];
                        lu[imax,j] = lu[k,j];
                        lu[k,j] = temp;
                    }

                    d = -d;                 // ... and change the parity of d.
                    vv[imax] = vv[k];       // Also interchange the scale factor.
                }

                indx[k] = imax;
                if (lu[k,k] == 0.0) { lu[k,k] = TINY; }
                // If the pivot element is zero, the matrix is singular (at least to the precision of the
                // algorithm). For some applications on singular matrices, it is desirable ti substitute
                // TINY for zero.


                for (i = k + 1; i < n; i++)
                {
                    temp = lu[i,k] /= lu[k,k];    // Divide the pivot element.

                    // Innermost loop: reduce remaining submatrix.
                    for (j = k + 1; j < n; j++)
                    {
                        lu[i,j] -= temp * lu[k,j];
                    }
                }
            }

        }

        public void solve(double[] b, out double[] x)
        {
            Int32 i, ii = 0, ip, j;
            Double sum;
            x = new double[n];

            if (b.Length != n)
            {
                System.Windows.Forms.MessageBox.Show("LUdcmp::solve bad sizes");
            }


            for (i = 0; i < n; i++)
            {
                x[i] = b[i];
            }

            for (i = 0; i < n; i++)
            {
                ip = indx[i];
                sum = x[ip];
                x[ip] = x[i];

                if (ii != 0)
                {
                    for (j = ii - 1; j < i; j++)
                    {
                        sum -= lu[i,j] * x[j];
                    }
                }
                else if (sum != 0.0)
                {
                    ii = i + 1;
                }

                x[i] = sum;
            }

            for (i = n - 1; i >= 0; i--)
            {
                sum = x[i];
                for (j = i + 1; j < n; j++)
                {
                    sum -= lu[i,j] * x[j];
                }

                x[i] = sum / lu[i,i];
            }
        }
        public void solve(ref double[][] b, out double[][] x)
        {
            Int32 i, j, m = b[0].Length;

            x = new double[n][];
            for (Int32 ss = 0; ss < x.Length; ss++)
            {
                x[ss] = new double[b[0].Length];
            }


            if (b.Length != n)
            {
                System.Windows.Forms.MessageBox.Show("LUdcmp::solve bad sizes");
            }

            double[] xx = new double[n];

            for (j = 0; j < m; j++)
            {
                for (i = 0; i < n; i++)
                {
                    xx[i] = b[i][j];
                }

                solve(xx, out xx); // hier könnte es ernste probleme geben. out initialisier xx neu!

                for (i = 0; i < n; i++)
                {
                    x[i][j] = xx[i];
                }
            }

        }
        //---------------------------------------------------
    }
}
