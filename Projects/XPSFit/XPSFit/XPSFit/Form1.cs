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
using ZedGraph;

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
        List<List<double>> DgvData = new List<List<double>>();

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
            tc_zgc.Selecting += new TabControlCancelEventHandler(Tc_zgc_Selecting);
            tc_zgc.Selected += new TabControlEventHandler(Tc_zgc_Selected);
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
                //if (Curr_S != null) save_input();
                list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc));
                Curr_S = list_stuff[list_stuff.Count - 1];
                Curr_S.Bg_Sub.Add(new double[data.Item2.Count]);
                for (int i = 0; i < data.Item2.Count; i++)
                {
                    Curr_S.Bg_Sub[0][i] = 0;
                }
                dgv_models.Rows.Clear();
                dgv_models[0, 0].Value = "Gauss-Lorentz";
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
            /***
            if (Curr_S != null)
            {
                var rows = Curr_S.data.GetLength(0);
                var cols = Curr_S.data.GetLength(1);
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++)
                    {
                        dgv_models[j, i].Value = Curr_S.data[i, j];
                    }
                }
            }
            ***/
        }


        private void Tc_zgc_Selected(object sender, TabControlEventArgs e)
        {
            //Curr_S = tc_zgc.TabPages.Count > 0 ? list_stuff.Find(a => a.Data_name == (sender as TabControl).SelectedTab.Name) : null; // select current stuff-instance
            load_input();
        }

        private void Tc_zgc_Selecting(object sender, TabControlCancelEventArgs e)
        {
            save_input();
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
                        //dgv_models[5, e.RowIndex].Value = "##########";
                        dgv_models[4, e.RowIndex].ReadOnly = dgv_models[5, e.RowIndex].ReadOnly = true;
                        //if (get_paras(e.RowIndex, 3) != null) para.Add(get_paras(e.RowIndex, 3));
                        break;

                    case ("Gauss-Lorentz"):
                        //dgv_models[5, e.RowIndex].Value = "##########";
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
                Curr_S.x_temp = Curr_S.x;
                Curr_S.y_temp = Curr_S.y;
                Curr_S.x = erg.Item1;
                Curr_S.y = erg.Item2;
                //double[] c = new double[11];
                //Curr_S.SavGol(Curr_S.x, Curr_S.y, c, 11, 5, 5, 0, 4);
            }
            else
            {
                Curr_S.x = Curr_S.x_temp;
                Curr_S.y = Curr_S.y_temp;
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
                    //paras[i] = i == 0 ? value / 100.0 : value;
                    paras[i] = i == 3 ? value / 100.0 : value;
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


        public void save_input()
        {
            var rows = dgv_models.RowCount;
            var cols = dgv_models.ColumnCount;
            Curr_S.data = new string[rows, cols];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    var d = dgv_models[j, i].Value ?? "";
                    Curr_S.data[i, j] = d.ToString();
                }
            }
            dgv_models.Rows.Clear();
            dgv_models[0, 0].Value = "Gauss-Lorentz";
        }


        public void load_input()
        {
            if (Curr_S != null)
            {
                var rows = Curr_S.data.GetLength(0);
                var cols = Curr_S.data.GetLength(1);
                for (int i = 0; i < rows - 1; i++) dgv_models.Rows.Add();
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < cols; j++) dgv_models[j, i].Value = Curr_S.data[i, j];
                }
            }
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
                        if (dgv[i, row].Value == null || dgv[i, row].Value.ToString() == "") return;
                    }
                    double[] x = Curr_S.x.ToArray();
                    double[] y = Curr_S.y.ToArray();
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
                    //fit(Curr_S.paras.ToArray());
                    LMAFunction f = new CustomFunction();
                    double[] parameters = Curr_S.paras.ToArray();
                    double[] dy = new double[Curr_S.paras.Count()];
                    List<double> bg = new List<double>();
                    double sum = 0.0;
                    for (int i = 0; i < Curr_S.y.Count; i++)
                    {
                        for (int j = 0; j < Curr_S.Bg_Sub.Count; j++)
                        {
                            sum += Curr_S.Bg_Sub[j][i];
                        }
                        f.GetY(x[i], ref parameters, ref y[i], ref dy);
                        y[i] += sum;
                        sum = 0.0;
                    }
                    var LI = Curr_S.Draw_Line(x.ToList(),y.ToList(),"kommt noch", "none", "line");
                    LI.Line.Fill = new Fill(Color.Coral, Color.LightCoral, 90F);
                    Curr_S.zgc_plots.Invalidate();
                    Curr_S.zgc_plots.AxisChange();
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
            for (int i = 0; i < x_crop.Count(); i++)
            {
                //w[i] = 1.0 / (y[i] * y[i]);
                w[i] = Math.Max(1.0, Math.Sqrt(Math.Sqrt(Math.Abs(y[i]))));
                //w[i] = Math.Max(1.0, Math.Sqrt(Math.Abs(y[i])));
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
            
            for (int i = 0; i < paras.Count(); i+=4)
            {
                dgv_models[7, i / 4].Value = Math.Floor(paras[i] * paras[i + 2]); // Amplitude
                dgv_models.Rows[i / 4].Cells[7].Style.ForeColor = Color.Gray;
                dgv_models[8, i / 4].Value = Math.Round(paras[i + 1],3); // Center
                dgv_models.Rows[i / 4].Cells[8].Style.ForeColor = Color.Gray;
                dgv_models[9, i / 4].Value = Math.Round(paras[i + 2] * 2.0, 2); // Sigma
                dgv_models.Rows[i / 4].Cells[9].Style.ForeColor = Color.Gray;
                dgv_models[10, i / 4].Value = Math.Round(paras[i + 2] * 100.0 , 1); // mixing-Ratio
                dgv_models.Rows[i / 4].Cells[10].Style.ForeColor = Color.Gray;
            }

            double[] Area = new double[paras.Count() / 4];

            for (int l = 0; l < x.Length - 1; l++)
            {
                //double ln = -4.0 * Math.Log(2.0);
                int i, na = a.Count();
                double argG1, argG2, argL1, argL2, G, L, m, argG, arg;
                double yi = 0.0;
                double ln = Math.Log(2.0);
                double sqln = Math.Sqrt(ln);
                double pi = Math.PI;
                double sqpi = Math.Sqrt(pi);

                double ym = 0.0;
                double yp = 0.0;
                double xm = 0.0;
                double xp = 0.0;
                double nom = 0.0;

                
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

                    ym = paras[i] * (m * L + (1.0 - m) * G) / paras[i + 2];

                    yi += ym; // + (1.0/(m * (1.0 -m )) + 1.0/a[i] + (u - o)/((a[i+1]-u)*(a[i + 1]-o)) + 1.0 / a[i+2] + 1.0 / a[i+2]) * 100;

                    //ym = y[l];
                    //yp = y[l + 1];
                    xm = x[l];
                    xp = x[l + 1];
                    //nom = ym > 0 ? ym : -ym;
                    //nom = (((yp > 0 ? yp : -yp) + (ym > 0 ? ym : -ym))) / 2.0;
                    //Area[i] += (nom > 0 ? nom : -nom) * (xp > xm ? (xp - xm) : (xm - xp));
                    Area[i/4] += (ym > 0 ? ym : -ym) * (xp > xm ? (xp - xm) : (xm - xp));

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

            //Console.WriteLine(m.GetArea(x.ToList(), yy));
            var AreaSum = 0.0;
            foreach (var item in Area) AreaSum += item;

            foreach (var item in Area) Console.WriteLine("Area = {0},   Anteil = {1}", Math.Floor(item), Math.Floor(item) / AreaSum);
            for (int i = 0; i < Area.Length; i++)
            {
                dgv_models[5, i].Value = Math.Floor(Area[i]);
                dgv_models.Rows[i].Cells[5].Style.ForeColor = Color.Gray;
                dgv_models[6, i].Value = Math.Round(Area[i] / AreaSum * 100.0, 1);
                dgv_models.Rows[i].Cells[6].Style.ForeColor = Color.Gray;
            }
            time += sw.Elapsed.TotalMilliseconds;
            btn_tester.Text = (time / 1).ToString("0.00");
            Curr_S.Draw_Line(x.ToList(), yy, "Fit", "none", "line");
            //Curr_S.TEMP_Draw_Residuals(x.ToList(), residuals, "residuals");
            Curr_S.Draw_Residuals(x.ToList(), residuals, "residuals");
        }

        private void dgv_models_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            //var erg = dgv_models[e.ColumnIndex,e.RowIndex].Value;
            //if (erg != null) Console.WriteLine("Row {0}   Column {1}    Value {2}", e.RowIndex, e.ColumnIndex, erg.ToString());
        }
    }
}
