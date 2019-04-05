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
        stuff s = new stuff();
        private ZedGraphControl _zg_control;
        private List<double> _raw_bg_data_item_single;
        private List<double> _models_data_item_single;
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
            get { return _raw_bg_data_item; }
            set
            {   _raw_bg_data_item = value;
                _raw_bg_data.Add(value);
            }
        }

        public List<List<double>> Models_data_item
        {
            get { return _models_data_item; }
            set
            {   _models_data_item = value;
                _models_data.Add(value);
            }
        }

        public List<double> Raw_bg_data_item_single
        {
            get { return _raw_bg_data_item_single; }
            set
            {
                _raw_bg_data_item[_models_data.Count] = value;
                _raw_bg_data[_models_data.Count].Add(value);
            }
        }

        public List<double> Models_data_item_single
        {
            get { return _models_data_item_single; }
            set
            {
                _models_data_item[_models_data.Count] = value;
                _models_data[_models_data.Count].Add(value);
            }
        }

        #endregion //--------------------------------------------------------------------------------------





        #region Constructor
        public Form2(Form callingForm)
        {
            InitializeComponent();
            f1 = callingForm as Form1;      // Interaction with Form 1   
            this.TopMost = true;            // Analyser-table always on top
            dgv_bg.CellValueChanged += new DataGridViewCellEventHandler(dgv_bg_CellValueChanged);
            dgv_bg.CurrentCellDirtyStateChanged += new EventHandler(dgv_bg_CurrentCellDirtyStateChanged);
        }
        #endregion //--------------------------------------------------------------------------------------





        #region Events
        private void btn_form2_test_Click(object sender, EventArgs e)
        {
            f1.tester();
        }


        private void dgv_bg_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_bg.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dgv_bg.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgv_bg_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // My combobox column is the second one so I hard coded a 1, flavor to taste
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_bg.Rows[e.RowIndex].Cells[0];
            if (cb.Value != null)
            {
                if (cb.Value.ToString() == "Shirley")
                {
                    s.X = _raw_bg_data[f1.Current_tp_index][e.RowIndex];
                    s.Y = _raw_bg_data[f1.Current_tp_index][e.RowIndex + 1];
                    var zgc = f1.ZGC;
                    zgc.IsEnableHZoom = false;
                    zgc.IsEnableVZoom = false;
                    zgc.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(s.zgc_MouseDownEvent);
                    zgc.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(s.zgc_MouseUpEvent);
                    zgc.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(s.zgc_MouseMoveEvent);

                    Console.WriteLine("OK");
                }

                // do stuff
                //mainForm.get_coordinates();
                //lb_filename.Text = cb.Value.ToString();
                dgv_bg.Invalidate();
            }
        }
        #endregion //--------------------------------------------------------------------------------------

    }
}
