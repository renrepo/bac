using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

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
            List<double> yy = new List<double>();
            List<double> yy_calc = new List<double>();
            List<double> xx = new List<double>();

            double time = 0;
            double[] a = new double[] {40000.0,368.4,1.2, 1.0, 50.0, 40000.0, 374.2, 1.2, 1.0, 40.0 };
            //double[] a = new double[] { 50000.0, 368.4, 5.0, 40000.0, 372.4, 5.0};
            double[] x = Curr_S.x.ToArray();
            //double[] y = Curr_S.y.ToArray();
            double sum = 0.0;
            List<double> erg = new List<double>();
            List<double> bg = new List<double>();
            for (int i = 0; i < Curr_S.y.Count; i++)
            {
                for (int j = 0; j < Curr_S.Bg_Sub.Count; j++)
                {
                    sum += Curr_S.Bg_Sub[j][i];
                }
                bg.Add(sum);
                if (sum != 0)
                {
                    erg.Add(Curr_S.y[i] - sum);
                    xx.Add(x[i]);
                }
                //erg.Add(sum == 0 ? 0 : Curr_S.y[i] - sum);
                sum = 0.0;
            }
            double[] y = erg.ToArray();
            double[] x_crop = xx.ToArray(); 
            double[] w = new double[x.Length];
            for (int i = 0; i < x_crop.Count(); i++)
            {
                //w[i] = 1.0 / (y[i] * y[i]);
                w[i] = 1.0;
                //w[i] = y[i];
            }
            LMAFunction f = new CustomFunction();

            LMA algorithm = new LMA(f, ref x_crop, ref y, ref w ,ref a);
            //algorithm.hold(4,0.0); // zero pure gaussian
            //algorithm.hold(3, 1);
            //algorithm.hold(11, 0.0);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            for (int i = 0; i < 1; i++)
            {              
                algorithm.fit();
            }
            double[] paras = algorithm.A;
            //double[] paras = a;
            foreach (var item in paras)
            {
                Console.WriteLine(item);
            }
            for (int l = 0; l < x.Length; l++)
            {
                double ln = - 4.0 * Math.Log(2.0);
                int i, na = a.Count();
                double fac, ex, argG,argL1,argL2,G,L,V,m, arg,L1,L2;
                double yi = 0.0;
                for (i = 0; i < na - 1; i += 5)
                {
                    /***--------------------------------------------------------------------------------------G A U S S I A N
                    arg = (x[l] - paras[i + 1]) / paras[i + 2];
                    ex = Math.Exp(-Math.Pow(arg, 2) * ln);
                    if (a.Length % 3 != 0) MessageBox.Show("Invalid number of parameters for Gaussian");
                    fac = paras[i] * ex * 2.0 * arg;
                    yi += paras[i] * ex;
                    ***/
                    /***--------------------------------------------------------------------------------------L O R E N T Z I A N
                    argL1 = (x[l] - paras[i + 1]) / a[i + 2];
                    argL2 = (x[l] - paras[i + 1] - 0.416) / a[i + 2];

                    L1 = 1.0 / (1.0 + 4.0 * Math.Pow(argL1, 2));
                    L2 = 1.0 / (1.0 + 4.0 * Math.Pow(argL2, 2)) / 2.0;

                    yi += paras[i] * (L1 + L2);

                    /***-------------------------------------------------------------------------------------- G L 
                    ***/
                    m = paras[i + 4] * 0.01;
                    
                    argL1 = (x[l] - paras[i + 1]) / paras[i + 2];
                    argL2 = (x[l] - paras[i + 1] - 0.416) / paras[i + 2];
                    argG = (x[l] - paras[i + 1]) / paras[i + 3];

                    G = Math.Exp(ln * Math.Pow(argG, 2));
                    L = 1.0 / (1.0 + 4.0 * Math.Pow(argL1, 2)) + 1.0 / (1.0 + 4.0 * Math.Pow(argL2, 2)) / 2.0;

                    yi += paras[i] * (m * L + (1.0 - m) * G);


                }
                yy.Add(bg[l] == 0 ? bg[l] : bg[l] + yi);
                //yy.Add(yi);
            }

            time += sw.Elapsed.TotalMilliseconds;
            btn_tester.Text = (time/1).ToString("0.00");
            Curr_S.Draw_Line(x.ToList(),yy,"Fit");
            

        }

        #endregion //-------------------------------------------------------------------------------------





        #region Events

        private void Form1_Load(object sender, EventArgs e)
        {
            tc_zgc.Selected += new TabControlEventHandler(Tc_zgc_SelectedIndexChanged);
            dgv_bg.CurrentCellDirtyStateChanged += new EventHandler(dgv_bg_CurrentCellDirtyStateChanged);
            dgv_bg.CellValueChanged += new DataGridViewCellEventHandler(dgv_bg_CellValueChanged);
            dgv_bg[1, 0].Value = "Shirley";

            dgv_models.CurrentCellDirtyStateChanged += new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
            dgv_models.CellValueChanged += new DataGridViewCellEventHandler(dgv_models_CellValueChanged);
            dgv_models[0, 0].Value = "Gauss-Lorentz";
            dgv_models[5, 0].Value = "#############";
            dgv_models[4, 0].Value = String.Empty;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot();

            if (data == null) return;
            dgv_bg.Enabled = dgv_models.Enabled = true;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3);
            else
            {
                list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc));
                Curr_S = list_stuff[list_stuff.Count - 1];
                Curr_S.Bg_Sub.Add(new double[data.Item2.Count]);
                for (int i = 0; i < data.Item2.Count; i++)
                {
                    Curr_S.Bg_Sub[0][i] = 0;
                }
            }
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
                    Curr_S.Bg_Sub.RemoveAt(e.RowIndex);
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
                        dgv_bg[1, e.RowIndex + 1].Value = "Shirley";

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
            double sum = 0.0;
            List<double> res = new List<double>();
            if (cb_Bg_Sub.Checked)
            {
                foreach (var item in Curr_S.List_LineItem)
                {
                    item.IsVisible = false;
                }
                List<double> erg = new List<double>();
                for (int i = 0; i < Curr_S.y.Count; i++)
                {
                    for (int j = 0; j < Curr_S.Bg_Sub.Count; j++)
                    {
                        sum += Curr_S.Bg_Sub[j][i];
                    }
                    erg.Add(sum);
                    res.Add(sum == 0 ? 0 : Curr_S.y[i] - sum);
                    sum = 0.0;
                }
                
                tc_zgc.Refresh();
                Curr_S.Draw_Line(Curr_S.x, res , Curr_S.Data_name + "_bg_sub");
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


        private void dgv_models_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgv_models.IsCurrentCellDirty) dgv_models.CommitEdit(DataGridViewDataErrorContexts.Commit); // This fires the cell value changed handler below
        }


        private void dgv_models_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)dgv_models.Rows[e.RowIndex].Cells[0];
            //DataGridViewCheckBoxCell cc = (DataGridViewCheckBoxCell)dgv_models.Rows[e.RowIndex].Cells[0];


            if (cb.State == DataGridViewElementStates.Selected && cb.Value != null)
            {
                dgv_models.CurrentCellDirtyStateChanged -= new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
                dgv_models.CellValueChanged -= new DataGridViewCellEventHandler(dgv_models_CellValueChanged);


                switch (cb.Value)
                {
                    case ("Gauss"):
                    case ("Lorentz"):
                        dgv_models[4, e.RowIndex].Value = (cb.Value.ToString() == "Gauss") ? 100 : 0;
                        dgv_models[5, e.RowIndex].Value = "##########";
                        dgv_models[4, e.RowIndex].ReadOnly = dgv_models[5, e.RowIndex].ReadOnly = true;
                        //if (get_paras(e.RowIndex, 3) != null) para.Add(get_paras(e.RowIndex, 3));
                        break;

                    case ("Gauss-Lorentz"):
                        dgv_models[5, e.RowIndex].Value = "##########";
                        dgv_models[4, e.RowIndex].Value = String.Empty;
                        dgv_models[5, e.RowIndex].ReadOnly = false;
                        //if (get_paras(e.RowIndex, 4) != null) para.Add(get_paras(e.RowIndex, 4));
                        break;

                    case ("GLP"):
                        dgv_models[4, e.RowIndex].Value = dgv_models[5, e.RowIndex].Value = String.Empty;
                        dgv_models[4, e.RowIndex].ReadOnly = dgv_models[5, e.RowIndex].ReadOnly = false;
                        //if (get_paras(e.RowIndex, 5) != null) para.Add(get_paras(e.RowIndex, 5));
                        break;

                    case ("Remove"):                       
                        for (int i = 1; i < 6; i++)
                        {
                            dgv_models[i, e.RowIndex].Value = String.Empty;
                            dgv_models[i, e.RowIndex].ReadOnly = false;
                        }
                        dgv_models.Rows.RemoveAt(e.RowIndex);
                        break;
                }
                dgv_models.CurrentCellDirtyStateChanged += new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
                dgv_models.CellValueChanged += new DataGridViewCellEventHandler(dgv_models_CellValueChanged);
            }

        }


        private void btn_fit_Click(object sender, EventArgs e)
        {

        }

        private void dgv_models_MouseEnter(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            //var row = dgv.CurrentCell.RowIndex;
            //if (para.Count() < row)
            //{
                //para.Add(get_paras())
            //}
            
        }


        #endregion //-------------------------------------------------------------------------------------






        #region Methods

        private double[] get_paras(int rowindex, int numbers)
        {
            bool tryparse;
            double[] paras = new double[numbers];

            for (int i = 0; i < numbers; i++)
            {
                tryparse = double.TryParse(dgv_models[rowindex, i + 1].Value.ToString().Replace(",", "."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out double value);
                
                if (tryparse && value > 0) paras[i] = value;
                else return null; MessageBox.Show("Type in Numbers for Fitparameters.");
                
            }
            return paras;
        }

        #endregion

    }
}
