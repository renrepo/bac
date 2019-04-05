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
using System.Diagnostics;


namespace analyser
{
    public partial class Form_Fitmodels : Form
    {
        zgc_class z;
        #region Properties
        public string filename { get; set; }
        #endregion


        #region Fields
        private Form_Main mainForm = null;
        #endregion


        #region Constructor
        public Form_Fitmodels(Form callingForm)
        {
            //https://stackoverflow.com/questions/1665533/communicate-between-two-windows-forms-in-c-sharp
            mainForm = callingForm as Form_Main;
            InitializeComponent();
            //keep Form always on top
            this.TopMost = true;


            dgv_background.CellValueChanged += new DataGridViewCellEventHandler(dgv_background_CellValueChanged);
            dgv_background.CurrentCellDirtyStateChanged += new EventHandler(dgv_background_CurrentCellDirtyStateChanged);

        }
        #endregion


        private void Fitmodels_Load(object sender, EventArgs e)
        {

        }


        public void tb_changed(string filename)
        {
            //this.z = filename;
            //lb_filename.Refresh();
        }

        void dgv_background_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_background.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dgv_background.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dgv_background_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // My combobox column is the second one so I hard coded a 1, flavor to taste
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_background.Rows[e.RowIndex].Cells[0];
            if (cb.Value != null)
            {
                // do stuff
                mainForm.get_coordinates();
                lb_filename.Text = cb.Value.ToString();
                dgv_background.Invalidate();
            }
        }


    }
}
