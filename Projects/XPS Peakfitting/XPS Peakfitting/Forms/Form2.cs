using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace XPS_Peakfitting.Forms
{
    public partial class Form2 : Form
    {



        #region Fields
        Form1 f1;
        private ZedGraphControl _zg_control;
        private List<List<double>> _raw_bg_data_item;
        private List<List<double>> _models_data_item;
        private List<List<List<double>>> _raw_bg_data = new List<List<List<double>>>();
        private List<List<List<double>>> _models_data = new List<List<List<double>>>();

        #endregion //--------------------------------------------------------------------------------------





        #region Properties
        public ZedGraphControl Zg_control
        {
            get { return _zg_control; }
            set { _zg_control = value; }
        }

        public List<List<double>> Raw_bg_data_item
        {
            get { return Raw_bg_data_item; }
            set
            {   _raw_bg_data_item = value;
                _raw_bg_data.Add(value);
            }
        }

        public List<List<double>> Models_data_item
        {
            get { return Models_data_item; }
            set
            {   _models_data_item = value;
                _models_data.Add(value);
            }
        }

        #endregion //--------------------------------------------------------------------------------------




        #region Constructor
        public Form2(Form callingForm)
        {
            InitializeComponent();
            f1 = callingForm as Form1;          
            this.TopMost = true;
        }
        #endregion //--------------------------------------------------------------------------------------





        #region Events
        private void btn_form2_test_Click(object sender, EventArgs e)
        {
            f1.tester();
            //Console.WriteLine(_raw_bg_data[0][0].ToString());
        }
        #endregion //--------------------------------------------------------------------------------------

    }
}
