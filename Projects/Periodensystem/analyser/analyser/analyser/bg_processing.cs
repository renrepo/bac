using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace analyser
{
    class bg_processing
    {
        #region Fields
        private double[] x_data;
        private double[] y_data;
        private int iterations;

        //private double IntMax;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public bg_processing()
        {

        }
        #endregion


        #region Methods

        public double[] integral(double[] x_data, double[] y_data, int iterations)
        {
            int data_length = x_data.Length;
            double I_max = y_data[data_length - 1];
            double I_min = y_data[1];
            double[] B_n = new double[data_length];
            double[] B_n_old = new double[data_length];
            double fak = 1.0;

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
                            fak += ((x_data[l] - x_data[l - 1])) * (0.5 * (y_data[l] + y_data[l - 1])  - I_max - B_n_old[l - 1]);
                        }
                    }
                    B_n[i] *= (I_min - I_max) / fak;
                }
                for (int r = 0; r < data_length; r++)
                {
                    B_n_old[r] = B_n[r];
                    B_n[r] = 0.0;
                }      
            }
            for (int i = 0; i < data_length; i++)
            {
                B_n_old[i] += I_max;
            }
            return B_n_old;
        }

        #endregion

    }
}
