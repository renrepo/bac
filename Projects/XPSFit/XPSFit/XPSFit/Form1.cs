using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XPSFit
{
    public partial class Form1 : Form
    {

        #region Fields

        methods m = new methods();
        stuff Curr_S;
        List<stuff> list_stuff = new List<stuff>();

        #endregion //-------------------------------------------------------------------------------------





        #region Properties



        #endregion //-------------------------------------------------------------------------------------





        #region Constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Methods



        #endregion //-------------------------------------------------------------------------------------





        #region Events

        private void Form1_Load(object sender, EventArgs e)
        {
            tc_zgc.Selected += new TabControlEventHandler(Tc_zgc_SelectedIndexChanged);
            dgv_bg.CurrentCellDirtyStateChanged += new EventHandler(dgv_bg_CurrentCellDirtyStateChanged);
            dgv_bg.CellValueChanged += new DataGridViewCellEventHandler(dgv_bg_CellValueChanged);
            dgv_bg[1, 0].Value = "Shirley";
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot();

            if (data == null) return;
            dgv_bg.Enabled = dgv_models.Enabled = cb_bg.Enabled = true;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3);
            else { list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc)); Curr_S = list_stuff[list_stuff.Count - 1]; }
        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            if (tc_zgc.SelectedTab != null) { list_stuff.Remove(Curr_S); tc_zgc.SelectedTab.Dispose(); }
            if (tc_zgc.TabPages.Count == 0) dgv_bg.Enabled = dgv_models.Enabled = cb_bg.Enabled = false;
        }



        private void dgv_bg_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_bg.IsCurrentCellDirty) dgv_bg.CommitEdit(DataGridViewDataErrorContexts.Commit); // This fires the cell value changed handler below
        }


        private void dgv_bg_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_bg.Rows[e.RowIndex].Cells[1];
            DataGridViewCheckBoxCell cc = (DataGridViewCheckBoxCell)dgv_bg.Rows[e.RowIndex].Cells[0];


            if (cb.Value.ToString() == "Remove")
            {
                Curr_S.Bg_tag_num = (e.RowIndex.ToString());
                Curr_S.Remove_PolyObj();
                Curr_S.Remove_Line();
                Curr_S.Remove_Mouse_Events();
                return;
            }

            if (cc.State == DataGridViewElementStates.Selected)
            {
                Curr_S.Bg_tag_num = (e.RowIndex.ToString());
                if (cc.Value.ToString() == "True" && cb.Value != null)
                {
                    Curr_S.Bg_tag_type = cb.Value.ToString();
                    Curr_S.Add_Mouse_Events();
                    var max = Curr_S.x[Curr_S.y.IndexOf(Curr_S.y.Max())];
                    Curr_S.x_bg_left = max - 1.0;
                    Curr_S.x_bg_right = max + 1.0;
                    Curr_S.Draw_Polyobj();
                    Curr_S.zgc_plots_MouseUpEvent(null, null);
                }

                else
	            {
                    Curr_S.Remove_PolyObj();
                    Curr_S.Remove_Mouse_Events();
                }
            }         
        }


        private void Tc_zgc_SelectedIndexChanged(object sender, TabControlEventArgs e)
        {
            Curr_S = tc_zgc.TabPages.Count > 0 ?  list_stuff.Find(a => a.Data_name == (sender as TabControl).SelectedTab.Name) : null; // select current stuff-instance
        }


        private void cb_bg_CheckedChanged(object sender, EventArgs e)
        {
            //cb_bg.Checked = cb_bg.Checked == true ? false : true;
            if (cb_bg.Checked)
            {
                //dgv_bg.BackgroundColor = SystemColors.Control;
                dgv_bg.Enabled = true;
                cb_bg.Text = "Disable Background selection";
                //Curr_S.Add_Mouse_Events();
                
            }

            else
            {
                dgv_bg.Enabled = false;
                cb_bg.Text = "Enable Background selection";
                //Curr_S.Remove_Mouse_Events();
                //dgv_bg.BackgroundColor = Color.MediumSpringGreen;
            }
        }









        #endregion //-------------------------------------------------------------------------------------


    }
}
