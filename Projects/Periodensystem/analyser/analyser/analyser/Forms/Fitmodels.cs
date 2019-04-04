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
using analyser.Forms;
using System.Diagnostics;

namespace analyser.Forms
{
    public partial class Fitmodels : Form
    {
        public string filename { get; set; }
        #region Constructor
        public Fitmodels()
        {
            InitializeComponent();
            //keep Form always on top
            this.TopMost = true;
            dgv_background.CellValueChanged +=
             new DataGridViewCellEventHandler(dataGridView1_CellValueChanged);
            dgv_background.CurrentCellDirtyStateChanged +=
                 new EventHandler(dataGridView1_CurrentCellDirtyStateChanged);

        }
        #endregion

        private void btn_shirley_Click(object sender, EventArgs e)
        {

        }

        private void Fitmodels_Load(object sender, EventArgs e)
        {

        }

        private void tb_select_bg_Click(object sender, EventArgs e)
        {
            //ZedGraphControl z =
        }

        public void tb_changed(string filename)
        {
            
          // lb_filename.Refresh();
        }

        void dataGridView1_CurrentCellDirtyStateChanged(object sender,
        EventArgs e)
        {
            if (dgv_background.IsCurrentCellDirty)
            {
                // This fires the cell value changed handler below
                dgv_background.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // My combobox column is the second one so I hard coded a 1, flavor to taste
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_background.Rows[e.RowIndex].Cells[0];
            if (cb.Value != null)
            {
                // do stuff
                lb_filename.Text = cb.Value.ToString();
                dgv_background.Invalidate();
            }
        }


    }
}
