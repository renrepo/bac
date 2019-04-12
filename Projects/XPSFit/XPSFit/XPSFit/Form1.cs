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
        List<DataGridView> List_DGV_bg = new List<DataGridView>();
        List<DataGridView> List_DGV_models = new List<DataGridView>();

        int old_row_index = -1;

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

        private void btn_tester_Click(object sender, EventArgs e)
        {
            //dgv_bg.Rows.RemoveAt(1);
            dgv_bg.CellValueChanged -= new DataGridViewCellEventHandler(dgv_bg_CellValueChanged);
            dgv_bg[0, 0].Value = "True";
            dgv_bg[0, 0].Value = "false";
            dgv_bg.CellValueChanged += new DataGridViewCellEventHandler(dgv_bg_CellValueChanged);
        }

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
            dgv_bg.Enabled = dgv_models.Enabled = true;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3);
            else { list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc)); Curr_S = list_stuff[list_stuff.Count - 1]; }
            Curr_S.Bg_Sub = data.Item2.ToArray();
        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            if (tc_zgc.SelectedTab != null) { list_stuff.Remove(Curr_S); tc_zgc.SelectedTab.Dispose(); }
            if (tc_zgc.TabPages.Count == 0) dgv_bg.Enabled = dgv_models.Enabled = false;
        }



        private void dgv_bg_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_bg.IsCurrentCellDirty) dgv_bg.CommitEdit(DataGridViewDataErrorContexts.Commit); // This fires the cell value changed handler below
        }


        private void dgv_bg_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_bg.Rows[e.RowIndex].Cells[1];
            DataGridViewCheckBoxCell cc = (DataGridViewCheckBoxCell)dgv_bg.Rows[e.RowIndex].Cells[0];

            if (cb.Value == null)
            {
                return;
            }

            if (cb.State == DataGridViewElementStates.Selected)
            {
                if (cc.Value != null && cc.Value.ToString() == "True" && cb.Value.ToString() != "Remove")
                {
                    Curr_S.Bg_tag_type = cb.Value.ToString();
                    Curr_S.zgc_plots_MouseUpEvent(null, null);
                }

                if (cc.Value != null && cb.Value.ToString() == "Remove")
                {
                    if (cc.Value.ToString() != "True")
                    {
                        MessageBox.Show("Select BG to delete");
                        return;
                    }
                    Curr_S.Bg_tag_num = (e.RowIndex);
                    Curr_S.Remove_PolyObj();
                    Curr_S.Remove_Line(e.RowIndex.ToString());
                    //Curr_S.Remove_Line(null); //-------------------------------------------------------------------------------null????
                    Curr_S.Remove_Mouse_Events();
                    Curr_S.Bg_Bounds.RemoveRange(e.RowIndex * 2, 2);
                    dgv_bg.Rows.RemoveAt(e.RowIndex);                   

                    if (e.RowIndex == 0)
                    {
                        dgv_bg[1, 0].Value = "Shirley";
                    }
                    return;
                }

                else
                {
                    return;
                }
            }

            if (cc.State == DataGridViewElementStates.Selected)
            {
                if (cb.Value != null && cb.Value.ToString() != "Remove" && cc.Value.ToString() == "True")
                { 
                    if (e.RowIndex * 2  <= Curr_S.Bg_Bounds.Count)
                    {
                        Curr_S.Bg_tag_num = (e.RowIndex);
                        //DataGridViewRow row = (DataGridViewRow)dgv_bg.Rows[0].Clone();
                        //dgv_bg.Rows.Add(row);
                        Curr_S.Bg_tag_type = cb.Value.ToString();
                        Curr_S.Draw_Polyobj();
                        Curr_S.Remove_Mouse_Events();
                        Curr_S.Add_Mouse_Events();
                        Curr_S.zgc_plots_MouseUpEvent(null, null);

                        if (old_row_index != -1 && old_row_index != e.RowIndex)
                        {
                            dgv_bg[0, old_row_index].Value = "False";
                            Curr_S.Hide_PolyObj(old_row_index);
                        }
                        old_row_index = e.RowIndex;
                    }

                    else
                    {
                        Curr_S.Remove_Mouse_Events();
                        MessageBox.Show("Use upper rows.");
                        return;
                    }

                }
                else if (cb.Value == null)
                {
                    Curr_S.Remove_Mouse_Events();
                    cc.Value = "False";
                    MessageBox.Show("No entry in Combobox");
                    return;
                }

                else
                {
                    Curr_S.Remove_Mouse_Events();
                    Curr_S.Hide_PolyObj(e.RowIndex);                  
                }
            }

            else
            {

            }
        }


        private void Tc_zgc_SelectedIndexChanged(object sender, TabControlEventArgs e)
        {
            Curr_S = tc_zgc.TabPages.Count > 0 ?  list_stuff.Find(a => a.Data_name == (sender as TabControl).SelectedTab.Name) : null; // select current stuff-instance
        }

        private void cb_Bg_Sub_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_Bg_Sub.Checked)
            {
                foreach (var item in Curr_S.List_LineItem)
                {
                    item.IsVisible = false;
                }
                List<double> erg = new List<double>();
                for (int i = 0; i < Curr_S.y.Count; i++)
                {
                    erg.Add(Curr_S.y[i] - Curr_S.Bg_Sub[i]);
                }
                tc_zgc.Refresh();
                Curr_S.Draw_Line(Curr_S.x, erg , Curr_S.Data_name + "_bg_sub");
                cb_Bg_Sub.BackColor = Color.MediumSpringGreen;
            }

            if (!cb_Bg_Sub.Checked)
            {
                foreach (var item in Curr_S.List_LineItem)
                {
                    item.IsVisible = true;
                }
                Curr_S.Hide_Line(Curr_S.Data_name + "_bg_sub");
                Curr_S.Draw_Line(Curr_S.x, Curr_S.y.ToList(), Curr_S.Data_name);
                cb_Bg_Sub.BackColor = SystemColors.Control;
            }
        }







        #endregion //-------------------------------------------------------------------------------------


    }
}
