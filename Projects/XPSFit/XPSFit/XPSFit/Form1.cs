﻿using System;
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
using System.IO;

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
        int oldrow = -1;
        int oldcol = -1;
        bool discret = false;

        #endregion //-------------------------------------------------------------------------------------





        #region Properties



        #endregion //-------------------------------------------------------------------------------------





        #region Constructor

        public Form1()
        {
            InitializeComponent();
        }

        #endregion //-------------------------------------------------------------------------------------





        #region Tests


        private void btn_tester_Click(object sender, EventArgs e)
        { 
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
            Curr_S.Draw_Line(x_stripped, output.signals.Select(i => (double)i).ToList(), "zscore", SymbolType.Plus, true, Color.DarkOrange);
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
            dgv_models.CurrentCellDirtyStateChanged += new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
            dgv_models.CellValueChanged += new DataGridViewCellEventHandler(dgv_models_CellValueChanged);

            dgv_bg[1, 0].Value = "Shirley"; // some default values;
            dgv_models[0, 0].Value = "GLS";
            dgv_models[4, 0].Value = String.Empty;
            comb_disc.SelectedIndex = 1;
        }

        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot(); // get x and y values and filename

            if (data == null || data.Item1.Count == 0 || data.Item2.Count == 0) return;
            dgv_bg.Enabled = dgv_models.Enabled = cb_disc.Enabled = true;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3); // switch to tab if dataset is already loaded
            else
            {
                list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc)); // create new instance of stuff-class
                Curr_S = list_stuff[list_stuff.Count - 1];                              // Curr_S = this new instance
                Curr_S.Bg_Sub.Add(new double[data.Item2.Count]); // Bg array with length = number of datapoints, each element = 0
                for (int i = 0; i < data.Item2.Count; i++)
                {
                    Curr_S.Bg_Sub[0][i] = 0;
                }
                dgv_models.Rows.Clear();    // clear dgv_models, remove entries from previous fits
                dgv_models[0, 0].Value = "GLS"; // default 
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

            if (cb.Value == null) return;

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


        private void Tc_zgc_Selected(object sender, TabControlEventArgs e)
        {
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
                Curr_S.Draw_Line(Curr_S.x, res , Curr_S.Data_name + "_bg_sub", SymbolType.Plus, false, Color.Black);
                cb_Bg_Sub.BackColor = Color.MediumSpringGreen;
            }

            if (!cb_Bg_Sub.Checked)
            {
                foreach (var item in Curr_S.List_LineItem) item.IsVisible = true;

                Curr_S.Hide_Line(Curr_S.Data_name + "_bg_sub");
                Curr_S.Draw_Line(Curr_S.x, Curr_S.y.ToList(), Curr_S.Data_name, SymbolType.Plus, false, Color.Black);
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

            if (cb.State == DataGridViewElementStates.Selected && cb.Value != null)
            {
                dgv_models.CurrentCellDirtyStateChanged -= new EventHandler(dgv_models_CurrentCellDirtyStateChanged);
                dgv_models.CellValueChanged -= new DataGridViewCellEventHandler(dgv_models_CellValueChanged);

                switch (cb.Value)
                {
                    case ("G"):
                    case ("L"):
                        dgv_models[4, e.RowIndex].Value = (cb.Value.ToString() == "G") ? 100 : 0;
                        dgv_models[4, e.RowIndex].ReadOnly = dgv_models[5, e.RowIndex].ReadOnly = true;
                        break;

                    case ("GLS"):
                    case ("GLP"):
                        dgv_models[4, e.RowIndex].Value = 50;
                        dgv_models[5, e.RowIndex].ReadOnly = true;
                        dgv_models[4, e.RowIndex].ReadOnly = false;
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
            if (Curr_S == null || Curr_S.paras == null) return;
            Hide_Bg_Selection();
            if (Curr_S != null) Curr_S.Remove_Line("kommt noch");
            if (Curr_S.paras.Count > 0) fit(Curr_S.paras.ToArray(), -1.0);     
        }


        private void cb_disc_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_disc.Checked)
            {
                var erg = Curr_S.discreter(Curr_S.x, Curr_S.y, Convert.ToInt32(comb_disc.SelectedItem));
                Curr_S.Hide_Line(Curr_S.Data_name);
                Curr_S.Draw_Line(erg.Item1, erg.Item2, Curr_S.Data_name + "_disc", SymbolType.Plus, false, Color.ForestGreen);
                cb_disc.BackColor = Color.MediumSpringGreen;
                Curr_S.x_temp = Curr_S.x;
                Curr_S.y_temp = Curr_S.y;
                Curr_S.x = erg.Item1;
                Curr_S.y = erg.Item2;
                discret = true;
            }
            else
            {
                Curr_S.x = Curr_S.x_temp;
                Curr_S.y = Curr_S.y_temp;
                Curr_S.Hide_Line(Curr_S.Data_name + "_disc");
                Curr_S.Draw_Line(Curr_S.x, Curr_S.y.ToList(), Curr_S.Data_name, SymbolType.Plus, false, Color.ForestGreen);
                cb_disc.BackColor = System.Drawing.SystemColors.Control;
                discret = false;
            }
        }



        private double fit(double[] a, double p)
        {
            Cursor.Current = Cursors.WaitCursor;

            LMAFunction f;

            List<double> y_fit_in = new List<double>();
            List<double> y_fit_out = new List<double>();
            List<double> x_fit_out = new List<double>();
            List<double> residuals = new List<double>();
            List<double> x_temp = new List<double>();
            List<double> y_temp = new List<double>();
            List<string> models = new List<string>();
            List<double> bg = new List<double>(); // total background (sum of all partial backgrounds)

            Stopwatch sw = new Stopwatch();

            int na = a.Count();
            int num_models = na / 4;
            double[][] a_split = new double[num_models][];
            double[] Area = new double[num_models];
            var AreaSum = 0.0;
            double sum = 0.0;

            double[] x = Curr_S.x.ToArray();
            double[] y = Curr_S.y.ToArray();


            for (int i = 0; i < y.Length; i++)
            {
                for (int j = 0; j < Curr_S.Bg_Sub.Count; j++)
                {
                    sum += Curr_S.Bg_Sub[j][i]; // add-up all selecetd backgrounds
                }
                bg.Add(sum);
                if (sum != 0) // fit only values for which a background is selected
                {
                    y_temp.Add(y[i] - sum);
                    x_temp.Add(x[i]);
                }
                sum = 0.0;
            }
            double[] y_crop = y_temp.ToArray();
            double[] x_crop = x_temp.ToArray();

            double[] w = new double[x_crop.Length];
            for (int i = 0; i < x_crop.Count(); i++) w[i] = Math.Max(1.0, Math.Sqrt(Math.Abs(y_crop[i]))); // weighting

            /***
            if (dgv_models[0, 0].Value.ToString() == "GLP") f = new GLP();
            else if (dgv_models[0, 0].Value.ToString() == "GLS") f = new GLS();
            else if (dgv_models[0, 0].Value.ToString() == "L") f = new LorentzianFunction();
            else if (dgv_models[0, 0].Value.ToString() == "G") f = new GaussianFunction();
            else f = new GLS();
            ***/

            f = new custom();

            for (int i = 0; i < num_models; i++) models.Add(dgv_models[0,i].Value.ToString());

            f.Models = models.ToArray();

            LMA algorithm = new LMA(f, ref x_crop, ref y_crop, ref w, ref a);

            for (int i = 0; i < na - 1; i += 4) // vary m-parameter or set fix (single fit)
            {
                double hold_value = (p > -1.0) ? p : a[i + 3];
                algorithm.hold(i + 3, hold_value);
            }

            sw.Start();
            algorithm.fit(); // execute fit!

            double[] paras = algorithm.A;
            Curr_S.fit_results.Clear(); // fit results needed for Draw_initial method (switch between initial and final plot when mouse on dgv_models cells)
            Curr_S.fit_results.AddRange(paras);

            tb_chi2.Text = Math.Round(algorithm.RedChi2, 2).ToString();
            lb_iter.Text = algorithm.Iter.ToString();

            // get 2D-List containing each model-parameters
            for (int i = 0; i < num_models; i++) a_split[i] = new double[] { paras[i * 4], paras[i * 4 + 1], paras[i * 4 + 2], paras[i * 4 + 3] }; 

            double[] dummy = new double[na]; // dummy parameters for dyda in f.GetY (not needed here)

            double yi = 0.0;
            for (int l = 0; l < x.Length - 1; l++) // Calculate plot-values
            {
                double xm = x[l];
                double xp = x[l + 1];
                double y_sum = 0.0;


                for (int j = 0; j < num_models; j++)
                {
                    f.GetY(x[l], ref a_split[j], ref yi, ref dummy, 0.0);
                    y_sum += yi;
                    Area[j] += (yi > 0 ? yi : -yi) * (xp > xm ? (xp - xm) : (xm - xp)); // calculate area of each peak seperately
                }

                if (bg[l] == 0) //fit values where no bg is selected
                {
                    y_fit_out.Add(y[l] + y_sum);
                    x_fit_out.Add(x[l]);
                }
                else
                {
                    y_fit_in.Add(bg[l] + y_sum);
                }
                residuals.Add(bg[l] == 0 ? 0 : bg[l] + y_sum - y[l]); // full x-range damit residual-plot senkrecht über fit sichtbar              
            }
            Curr_S.Draw_Line(x_crop.ToList(), y_fit_in, "Fit_in", SymbolType.None, true, Color.Black);
            Curr_S.Draw_Line(x_fit_out.ToList(), y_fit_out, "Fit_out", SymbolType.Circle, false, Color.LimeGreen);
            Curr_S.Draw_Residuals(x.ToList(), residuals, "residuals");

            foreach (var item in Area) AreaSum += item; // Full area (sum of all peaks)

            for (int i = 0; i < na; i += 4) // show fit results in dgv_models table
            {
                for (int j = 5; j < 11; j++) dgv_models.Rows[i / 4].Cells[j].Style.ForeColor = Color.Gray;
                dgv_models[5, i / 4].Value = Math.Floor(Area[i / 4]); // Area              
                dgv_models[6, i / 4].Value = Math.Round(Area[i / 4] / AreaSum * 100.0, 1); // Area in %
                dgv_models[7, i / 4].Value = Math.Floor(paras[i]); // Amplitude
                dgv_models[8, i / 4].Value = Math.Round(paras[i + 1], 3); // Center
                dgv_models[9, i / 4].Value = Math.Round(paras[i + 2] * 2.0, 2); // Sigma
                dgv_models[10, i / 4].Value = Math.Floor(paras[i + 3] * 100.0); // mixing-Ratio
            }

            lb_time.Text = sw.Elapsed.TotalMilliseconds.ToString("0");
            Cursor.Current = Cursors.Default;

            return algorithm.RedChi2;
        }


       
        private void dgv_models_CellClick(object sender, DataGridViewCellEventArgs e)
        {           
           Hide_Bg_Selection();
           DataGridView dgv = sender as DataGridView;
           Draw_initial(dgv, dgv.CurrentCell.RowIndex, dgv.CurrentCell.ColumnIndex);        
        }


        private void Form1_Click(object sender, EventArgs e)
        {
            if (Curr_S != null) Curr_S.Remove_Line("kommt noch");
        }


        private void dgv_models_KeyUp(object sender, KeyEventArgs e) // Damit Initial-Plot auch bei Enter gezeichnet wird
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataGridView dgv = sender as DataGridView;
                Draw_initial(dgv, dgv.CurrentCell.RowIndex - 1, dgv.CurrentCell.ColumnIndex);
            }
        }


        private void dgv_models_CellStateChanged(object sender, DataGridViewCellStateChangedEventArgs e) // Pfeiltasten im dgv_models zeichnen jeweiligen Plot
        {
            Hide_Bg_Selection();
            DataGridViewElementStates state = e.StateChanged;
            int curr_row = e.Cell.RowIndex;
            int curr_col = e.Cell.ColumnIndex;
            if (oldrow != e.Cell.RowIndex || (oldcol == 4 && curr_col == 5) || (oldcol == 5 && curr_col == 4))
            {
                Draw_initial(dgv_models, curr_row, curr_col);
            }
            oldrow = curr_row;
            oldcol = curr_col;
        }

        #endregion //-------------------------------------------------------------------------------------






        #region Methods

        public void Hide_Bg_Selection()
        {
            for (int i = 0; i < Curr_S.List_PolyObj.Count; i++)
            {
                if (dgv_bg.Rows[i].Cells[0].State == DataGridViewElementStates.Selected)
                {
                    dgv_bg[0, i].Value = "false";
                    Curr_S.Hide_PolyObj(i);
                }
            }
        }


        private double[] get_paras(int rowindex)
        {
            bool tryparse;
            double[] paras = new double[4];

            for (int i = 0; i < 4; i++)
            {
                tryparse = double.TryParse(dgv_models[i + 1,rowindex].Value.ToString().Replace(",", "."), System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture, out double value);

                //if (tryparse && value >= 0) paras[i] = (i == 2) ? (value / 2.0) : (i == 3) ? (value / 100.0) : value;
                if (tryparse && value >= 0) paras[i] = (i == 3) ? (value / 100.0) : value;
                else
                {
                    MessageBox.Show("Type in Numbers for Fitparameters.");
                    return null;
                }         
            }
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
            dgv_models[0, 0].Value = "GLS";
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


        private void Draw_initial(DataGridView dgv, int row, int column)
        {
            if (Curr_S.Bg_Bounds.Count() < 2) return;
            double[] erg = new double[4];
            if (dgv_models[0, row].Value != null)
            {
                List<double> x_crop = new List<double>();
                List<double> y_crop = new List<double>();
                for (int i = 1; i < 5; i++)
                {
                    if (dgv[i, row].Value == null || dgv[i, row].Value.ToString() == "") return;
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

                LMAFunction f = new GLS();

                double[] parameters;
                if (Curr_S.fit_results.Count > 5 * row && column > 4)
                {
                    parameters = Curr_S.fit_results.GetRange(row * 4, 4).ToArray();
                }
                else
                {
                    parameters = Curr_S.paras.GetRange(row * 4, 4).ToArray();
                }
                //double[] parameters = Curr_S.paras.GetRange(row * 4, 4).ToArray(); //----------------------------------------------------------0.75 va 1.5 SIGMA???????

                double[] dy = new double[Curr_S.paras.Count()];
                List<double> bg = new List<double>();
                double sum = 0.0;
                for (int i = 0; i < Curr_S.y.Count; i++)
                {
                    var yi = Curr_S.y[i];
                    for (int j = 0; j < Curr_S.Bg_Sub.Count; j++)
                    {
                        sum += Curr_S.Bg_Sub[j][i];
                    }
                    if (sum != 0)
                    {
                        f.GetY(Curr_S.x[i], ref parameters, ref yi, ref dy, 0.0);
                        yi += sum;
                        y_crop.Add(yi);
                        x_crop.Add(Curr_S.x[i]);
                        sum = 0.0;
                    }
                }
                Curr_S.Remove_Line("kommt noch");
                var LI = Curr_S.Draw_Line(x_crop, y_crop, "kommt noch", SymbolType.None, true, Color.Gold);
                //LI.Line.Fill = new Fill(Color.Coral, Color.LightCoral, 90F);
                LI.Line.Fill = new Fill(Color.Gold, Color.Goldenrod, 90F);
                //LI.Line.Fill = new Fill(Color.PeachPuff, Color.Peru, 90F);
                //LI.Line.Fill = new Fill(Color.LightSkyBlue, Color.DeepSkyBlue, 90F);
                //LI.Line.Fill = new Fill(Color.LightSkyBlue, Color.CornflowerBlue, 90F);
                //LI.Line.Fill = new Fill(Color.LightSkyBlue, Color.SkyBlue, Color.DeepSkyBlue, 90F);
                var BG = Curr_S.List_LineItem.Find(a => a.Tag.ToString() == Curr_S.Bg_tag_num.ToString());
                BG.Line.Fill = new Fill(Color.White);
                Curr_S.zgc_plots.Invalidate();
                Curr_S.zgc_plots.AxisChange();
            }
        }

        private void btn_save_fig_Click(object sender, EventArgs e)
        {
            var path = Curr_S.save_data();
            if (path == String.Empty) return;
            using (var file = new StreamWriter(path + ".txt", true))
            {
                file.WriteLine("Filename: {0}", Curr_S.Data_name);
                file.WriteLine("Number of datapoints: {0}", Curr_S.x.Count() + "\n");
                if (discret) file.WriteLine("Energy stepwidth: {0}", cb_disc.Text.ToString());

                for (int i = 0; i < dgv_models.ColumnCount; i++) file.Write(dgv_models.Columns[i].HeaderText.ToString() + "\t");
                file.Write("\n");
                int j = 0;
                try
                {
                    while (dgv_models[0, j].Value != null)
                    {
                        for (int i = 0; i < dgv_models.ColumnCount; i++)
                        {
                            string value = (dgv_models[i, j].Value ?? String.Empty).ToString();
                            file.Write(value + "\t");
                        }
                        file.Write("\n");
                        j++;
                    }
                }
                catch (Exception) { }
            }

        }

        private void btn_find_m_Click(object sender, EventArgs e)
        {
            if (Curr_S == null || Curr_S.paras == null) return;
            if (Curr_S.paras.Count > 0)
            {
                List<double> Redchi_list = new List<double>();
                double[] pars = Curr_S.paras.ToArray();
                for (int i = 1; i < 100; i += 7)
                {
                    double p = i / 100.0;
                    double redchi = fit(pars, p);
                    Redchi_list.Add(redchi);
                    tc_zgc.Refresh();
                    dgv_models.Refresh();
                }
                int min = 1 + Redchi_list.IndexOf(Redchi_list.Min()) * 7;
                int start = (min == 99) ? (min == 1) ? 93 : 0 : min - 6;
                Redchi_list.Clear();
                for (int i = start; i < start + 13; i++)
                {
                    double p = i / 100.0;
                    double redchi = fit(pars, p);
                    Redchi_list.Add(redchi);
                    tc_zgc.Refresh();
                    dgv_models.Refresh();
                    lb_iter.Refresh();
                    lb_time.Refresh();
                }
                int result = Redchi_list.IndexOf(Redchi_list.Min()) + start;
                Console.WriteLine("Minimum m = {0} at Residual STD {1}", result, Redchi_list.Min());
                fit(pars, result / 100.0); // Plot final result
            }

        }


        #endregion
        
        
    }
}


/***
 * 
 * --- TODO ---
 * weights
 * Sicherung Shirley-BG-Range > Fitrange !
 * ***/


