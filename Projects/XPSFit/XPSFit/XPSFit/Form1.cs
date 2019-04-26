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
            //fit(new double[]{ 40000.0, 368.4, 1.0, 1.0, 82.0, 40000.0, 374.2, 1.0, 1.0, 22.0 }); 
            var x = Curr_S.x;
            var y = Curr_S.y;
            List<double> x_stripped = new List<double>();
            List<double> y_stripped = new List<double>();
            for (int i = 0; i < x.Count; i++)
            {
                if (i % 10 == 0)
                {
                    x_stripped.Add(x[i]);
                    y_stripped.Add(y[i]);
                }
            }
            int lag = 15;
            double threshold = 4.0;
            double influence = 0.5;

            var output = ZScore.StartAlgo(y_stripped, lag, threshold, influence);
            Curr_S.Draw_Line(x_stripped, output.signals.Select(i => (double)i).ToList(), "zscore", "dot", "line");
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

            comb_disc.SelectedIndex = 1;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot();

            if (data == null) return;
            dgv_bg.Enabled = dgv_models.Enabled = cb_disc.Enabled = true;
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
                Curr_S.Draw_Line(Curr_S.x, res , Curr_S.Data_name + "_bg_sub", "dot", "noline");
                cb_Bg_Sub.BackColor = Color.MediumSpringGreen;
            }

            if (!cb_Bg_Sub.Checked)
            {
                foreach (var item in Curr_S.List_LineItem)
                {
                    item.IsVisible = true;
                }
                Curr_S.Hide_Line(Curr_S.Data_name + "_bg_sub");
                Curr_S.Draw_Line(Curr_S.x, Curr_S.y.ToList(), Curr_S.Data_name, "dot", "noline");
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
                        dgv_models[4, e.RowIndex].Value = 50;
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
                        if (Curr_S.paras.Count != 0) Curr_S.paras.RemoveRange(e.RowIndex, 4);
                        break;
                }
                dgv_models.CurrentCellDirtyStateChanged += new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
                dgv_models.CellValueChanged += new DataGridViewCellEventHandler(dgv_models_CellValueChanged);
            }

        }


        private void btn_fit_Click(object sender, EventArgs e)
        {
            if (Curr_S.paras == null) return;
            if (Curr_S.paras.Count > 0) fit(Curr_S.paras.ToArray());
            
        }


        private void cb_disc_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_disc.Checked)
            {
                var erg = Curr_S.discreter(Curr_S.x, Curr_S.y, Convert.ToInt32(comb_disc.SelectedItem));
                Curr_S.Hide_Line(Curr_S.Data_name);
                Curr_S.Draw_Line(erg.Item1, erg.Item2, Curr_S.Data_name + "_disc", "dot", "noline");
                cb_disc.BackColor = Color.MediumSpringGreen;
                Curr_S.x_temp = erg.Item1;
                Curr_S.y_temp = erg.Item2;
                //double[] c = new double[11];
                //Curr_S.SavGol(Curr_S.x, Curr_S.y, c, 11, 5, 5, 0, 4);
            }
            else
            {
                Curr_S.Hide_Line(Curr_S.Data_name + "_disc");
                Curr_S.Draw_Line(Curr_S.x, Curr_S.y.ToList(), Curr_S.Data_name, "dot", "noline");
                cb_disc.BackColor = System.Drawing.SystemColors.Control;
            }

        }


        #endregion //-------------------------------------------------------------------------------------






        #region Methods

        private double[] get_paras(int rowindex)
        {
            bool tryparse;
            double[] paras = new double[4];

            for (int i = 0; i < 4; i++)
            {
                tryparse = double.TryParse(dgv_models[i + 1,rowindex].Value.ToString().Replace(",", "."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out double value);

                if (tryparse && value >= 0)
                {
                    paras[i] = value;
                }
                else
                {
                    MessageBox.Show("Type in Numbers for Fitparameters."); return null;
                }         
            }
            //var b = paras.ToList();
            //b.Insert(3, b[2]);
            //paras = b.ToArray();
            return paras;
        }



        #endregion






        private void dgv_models_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                double[] erg = new double[4];
                DataGridView dgv = sender as DataGridView;
                int row = dgv.CurrentCell.RowIndex - 1;
                if (dgv_models[0, row].Value != null && e.KeyData == Keys.Enter)
                {
                    for (int i = 1; i < 5; i++)
                    {
                        if (dgv[i, row].Value == null) return;
                    }
                    erg = get_paras(row);
                    if (Curr_S.paras.Count > 5 * row)
                    {
                        Curr_S.paras.RemoveRange(row * 4, 4);
                        Curr_S.paras.InsertRange(row * 4, erg);
                    }
                    else
                    {
                        Curr_S.paras.AddRange(erg);
                        
                    }
                    fit(Curr_S.paras.ToArray());
                }
            }
        }




        private void fit(double[] a)
        {
            List<double> yy = new List<double>();
            List<double> yy_calc = new List<double>();
            List<double> xx = new List<double>();
            List<double> residuals = new List<double>();

            double time = 0;
            
            //double[] a = new double[] { 40000.0, 368.4, 1.0, 1.0, 22.0, 40000.0, 374.2, 1.0, 1.0, 22.0 };
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
            double[] w = new double[x_crop.Length];
            var ymax = y.Max();
            for (int i = 0; i < x_crop.Count(); i++)
            {
                //w[i] = 1.0 / (y[i] * y[i]);
                w[i] = Math.Max(1.0, Math.Sqrt(Math.Sqrt(Math.Abs(y[i]))));
                //w[i] = 1/ y[i];
                //w[i] = y[i] / ymax;
                //w[i] = Math.Sqrt(y[i]);
                //w[i] = 1.0;
            }
            LMAFunction f = new CustomFunction();

            LMA algorithm = new LMA(f, ref x_crop, ref y, ref w, ref a);
            //algorithm.hold(4,Curr_S.paras[4]); // zero pure gaussian
            //algorithm.hold(3, 1.0);
            //algorithm.hold(8, 1.0);
            // algorithm.hold(9, 2.0);
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Cursor.Current = Cursors.WaitCursor;
            for (int i = 0; i < 1; i++)
            {
                algorithm.fit();
            }
            Cursor.Current = Cursors.Default;
            double[] paras = algorithm.A;
            tb_chi2.Text = Math.Round(algorithm.Chi2 / (x_crop.Length - paras.Length), 2).ToString();
            //double[] paras = a;
            foreach (var item in paras)
            {
                Console.WriteLine(Math.Round(item,3));
            }
            //Console.WriteLine(algorithm.GetHWHM(a, 0.2, 5.0) * 2.0);
            for (int l = 0; l < x.Length; l++)
            {
                //double ln = -4.0 * Math.Log(2.0);
                int i, na = a.Count();
                double argG1, argG2, argL1, argL2, G, L, m, argG, arg;
                double yi = 0.0;
                double ln = Math.Log(2.0);
                double sqln = Math.Sqrt(ln);
                double pi = Math.PI;
                double sqpi = Math.Sqrt(pi);               
                for (i = 0; i < na - 1; i += 4)
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
                    
                    ***/
                    //-------------------------------------------------------------------------------------- G L S


                    arg = (x[l] - paras[i + 1]) / paras[i + 2];
                    G = Math.Exp(-ln * arg * arg) * sqln / sqpi;
                    L = 1.0 / (1.0 + arg * arg) / pi;
                    m = paras[i + 3];


                    yi += paras[i] * (m * L + (1.0 - m) * G) / paras[i + 2]; // + (1.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[i + 1]-o)) + 1.0 / a[i+2] + 1.0 / a[i+2]) * 100;



                    /***
                    m = paras[i + 3];

                    
                    argL1 = (x[l] - paras[i + 1]) / paras[i + 2];
                    //argL2 = (x[l] - paras[i + 1] - 0.416) / paras[i + 2];
                    argG1 = (x[l] - paras[i + 1]) / paras[i + 2];
                    //argG2 = (x[l] - paras[i + 1] - 0.416) / paras[i + 3];

                    G = Math.Exp(ln * Math.Pow(argG1, 2));
                    L = 1.0 / (1.0 + 4.0 * Math.Pow(argL1, 2));

                    yi += paras[i] * (m * L + (1.0 - m) * G);
                    ***/

                    /***

                    // -------------------------------------------------------------------------------------- G L P
                    m = paras[i + 4];

                    argL1 = (x[l] - paras[i + 1]) / paras[i + 2];
                    argG = (x[l] - paras[i + 1]) / paras[i + 3];

                    G = Math.Exp(ln * (1 - m) * Math.Pow(argG, 2));
                    L = 1.0 / (1.0 + 4.0 * m * Math.Pow(argL1, 2));

                    yi += paras[i] * G * L;
                    

                    ***/

                }
                yy.Add(bg[l] == 0 ? bg[l] : bg[l] + yi);
                residuals.Add(bg[l] == 0 ? 0 : bg[l] + yi - Curr_S.y[l]);
                //yy.Add(yi);
            }

            time += sw.Elapsed.TotalMilliseconds;
            btn_tester.Text = (time / 1).ToString("0.00");
            Curr_S.Draw_Line(x.ToList(), yy, "Fit", "none", "line");
            //Curr_S.TEMP_Draw_Residuals(x.ToList(), residuals, "residuals");
            Curr_S.Draw_Residuals(x.ToList(), residuals, "residuals");
        }
    }
}
