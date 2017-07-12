﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using ZedGraph;
using System.Globalization;
using System.Diagnostics;
using NationalInstruments.Visa;

namespace XPS
{
    public partial class XPS : Form
    {

        string filePath = Path.GetFullPath("Bindungsenergien.csv");
        string filePath2 = Path.GetFullPath("colors2.csv");
        string filePath3 = Path.GetFullPath("electronconfiguration.csv");
        List<List<string>> row = new List<List<string>>();
        List<List<string>> elec_bind = new List<List<string>>();
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        Dictionary<string, string> fab = new Dictionary<string, string>(); 
        GraphPane myPane;
        LineItem myCurve;
        List<string> values = new List<string>();
        public int num_gauss = 20;
        PointPairList list1 = new PointPairList();
        List<string> fuerlabels = new List<string>();
        string[] scores = new string[] { "OZ", "El","K", "L1", "L2", "L3", "M1", "M2",
            "M3","M4","M5","N1","N2","N3","N4","N5","N6","N7","O1","O2","O3","O4",
            "O5","P1","P2","P3"};
        TextObj pane_labs;
        YAxis yaxis = new YAxis();


        int i;
        int end = 0;
        // bw-DoWork

        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        string now = DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss");
        string curr_time;

        private MessageBasedSession iseg;
        private string lastResourceString = null;




        public XPS()
        {
            InitializeComponent();
            myPane = zedGraphControl1.GraphPane;
            Rf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Db.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Sg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Bh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Hs.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Mt.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Ds.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Rg.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Cn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSalmon;
            Uuh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uut.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.PaleGreen;
            Uup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uuq.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Aquamarine;
            Uuo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Orange;
            Np.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Pu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Am.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Cm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Bk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Cf.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Es.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Fm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Md.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            No.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;
            Lr.FlatAppearance.MouseOverBackColor = System.Drawing.Color.MediumSpringGreen;


        }


        private void create_graph (GraphPane myPane)
        {
            myPane.Title.Text = "XPS spectra";
            myPane.Title.FontSpec.Size = 13;
            myPane.TitleGap = 1.6f;
            myPane.XAxis.Title.Text = "binding energy [eV]";
            myPane.XAxis.Title.FontSpec.Size = 11;
            myPane.YAxis.Title.Text = "counts";
            myPane.YAxis.Title.FontSpec.Size = 11;
            myPane.Fill.Color = Color.LightGray;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // StreamReader sr = new StreamReader(filePath);
            row = File.ReadAllLines(filePath).Select(l => l.Split(',').ToList()).ToList();
            elec_bind = File.ReadAllLines(filePath3).Select(l => l.Split(',').ToList()).ToList();
            fab = File.ReadLines(filePath2).Select(line => line.Split(',')).ToDictionary(data => data[0], data => data[1]);
            //MessageBox.Show(row[6][3]);
            var num = row.Count;

            for (int i = 0; i < num; i++)
            {
                dictionary.Add(row[i][1], row[i][0]);
            }
            create_graph(myPane);

            try
            {
                if (!Directory.Exists(path + @"\Logfiles_XPS"))
                {
                    Directory.CreateDirectory(path + @"\Logfiles_XPS");
                }
            }
            catch
            {
                MessageBox.Show("Can't create Folder 'Logfile' on Desktop");
            }


            System.Windows.Forms.Button[] but = {H,He, Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar, K, Ca, Sc,
                                                 Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr, Rb, Sr, Y, Zr,
                                                 Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe, Cs, Ba, La, Hf, Ta,
                                                 W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn, Fr, Ra, Ac, Ce, Pr, Nd,
                                                 Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Th, Pa, U, Rf, Np, Pu, Am, Cm,
                                                 Bk, Cf, Es, Fm, Md, No, Lr};
            foreach (var item in but)
            {
                item.MouseDown += Global_Button_Click;
            }

        }

        private void elementnames_Popup(object sender, PopupEventArgs e)
        {
        }



        private void tableLayoutPanel1_Layout(object sender, LayoutEventArgs e)
        {
            tableLayoutPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        }



        public void colorchanger(object sender, YAxis ya)
        {
            Button btn = (Button)sender;
           // var panel = sender as Control;
            //var thePanelName = btn.Name;
            string col = fab[btn.Name];
            int zeile = Convert.ToInt32(dictionary[btn.Name]) - 1;
            float value;

            if (btn.ForeColor == Color.DimGray)
            {
                btn.Font = new Font("Arial", 12, FontStyle.Bold);
                btn.ForeColor = Color.Black;
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 2;

                for (int i = 2; i <= 25; i++)
                {
                    bool result = float.TryParse(row[zeile][i], out value);

                    if (result)
                    {
                        pane_labs = new TextObj((row[zeile][i] + "\n" + row[zeile][1] + " " + scores[i]), float.Parse(row[zeile][i], CultureInfo.InvariantCulture), -0.05,
                            CoordType.XScaleYChartFraction, AlignH.Center, AlignV.Center);
                        pane_labs.FontSpec.Size = 10f;
                        pane_labs.FontSpec.Angle = 40;
                        pane_labs.FontSpec.Fill.Color = Color.Transparent;
                        pane_labs.FontSpec.FontColor = Color.DimGray;
                        pane_labs.FontSpec.Border.IsVisible = false;
                        pane_labs.ZOrder = ZOrder.E_BehindCurves;
                        myPane.GraphObjList.Add(pane_labs);
                        fuerlabels.Add(btn.Name);

                        ya = new YAxis();                        
                        ya.Scale.IsVisible = false;
                        ya.Scale.LabelGap = 0f;
                        ya.Title.Gap = 0f;
                        ya.Title.Text = "";
                        ya.Color = Color.FromName(col);
                        ya.AxisGap = 0f;
                        ya.Scale.Format = "#";
                        ya.Scale.Min = 0;
                        ya.Scale.Mag = 0;
                        ya.MajorTic.IsAllTics = false;
                        ya.MinorTic.IsAllTics = false;                      
                        ya.Cross = Double.Parse(row[zeile][i], CultureInfo.InvariantCulture);
                        ya.IsVisible = true;
                        ya.MajorGrid.IsZeroLine = false;
                        // hides xaxis
                        myPane.YAxisList.Add(ya);
                    }
                }
                zedGraphControl1.Refresh();
            }

            else
            {
                btn.ForeColor = Color.DimGray;
                btn.Font = new Font("Arial", 12, FontStyle.Regular);
                btn.FlatAppearance.BorderSize = 1;
                btn.FlatAppearance.BorderColor = Color.DimGray;

                int laenge = fuerlabels.Count - 1;
                for (int y = laenge; y >= 0; y--)
                {
                    if (fuerlabels[y] == btn.Name)
                    {
                        fuerlabels.RemoveAt(y);
                        myPane.GraphObjList.RemoveAt(y);
                        myPane.YAxisList.RemoveAt(y+1);
                    }
                }
                zedGraphControl1.Refresh();
            }
        }


        string path2;

        int data_coutner = 0;

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (!bW_data.IsBusy)
            {
                end = 0;
                string source;
                if (Al_anode.Checked){source = "Aluminium";}
                else {source = "Magnesium";}
                myCurve = myPane.AddCurve("",
                list1, Color.Black, SymbolType.None);
                curr_time = now;
                string u = tb_safe.Text + curr_time;
                DirectoryInfo dl =  Directory.CreateDirectory(Path.Combine(path + @"\Logfiles_XPS\", " " + tb_safe.Text + "_" + curr_time + "\\"));
                path2 = dl.FullName;
                using (var file = new StreamWriter(path2  +"data" + data_coutner + ".txt", true))
                {
                    file.WriteLine("#XPS-spectrum" + Environment.NewLine);
                    file.WriteLine("#Date/time: \t{0}", DateTime.Now.ToString("yyyy-MM-dd__HH-mm-ss"));
                    file.WriteLine("#X-ray source: \t{0}", source + Environment.NewLine);
                    file.WriteLine("#E_b \t counts");                   
                }

                bW_data.RunWorkerAsync(); //run bW if it is not still running
                btn_start.Enabled = false;
                btn_can.Enabled = true;
                tb_show.Enabled = true;
                tb_safe.Enabled = false;
                Mg_anode.Enabled = false;
                Al_anode.Enabled = false;
            }

            else
	        {
                    MessageBox.Show("BW busy!");
            }                      
        }



        private void bW_data_DoWork(object sender, DoWorkEventArgs e)
        {
            for (i = 0; i <= num_gauss; i++) 
            {
                end += i;
                values.Add(end + "\t" + 2 * end);
                bW_data.ReportProgress(100 * i / num_gauss, end);
                //safer.safe_line(path + @"\gauss", end.ToString("000000000"));
                using (var file = new StreamWriter(path2 +  "data" + data_coutner + ".txt", true))
                {
                    file.WriteLine(i.ToString("000") + "\t" + end.ToString("00000"));
                }
                Thread.Sleep(500);

                if (bW_data.CancellationPending) // condition is true, if gauss is cancelled (CancelAsync())            
                {
                    e.Cancel = true;
                    bW_data.ReportProgress(0);
                    break; //warum? ist wichtig!
                }
            }
            //safer.safe(path,list_gauss);
            e.Result = end; //stores the results of what has been done in bW
        }

        private void bW_data_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            lb_perc_gauss.Text = e.ProgressPercentage.ToString() + " %";
            tb_show.Text = Convert.ToString(e.UserState);
            // x = e.ProgressPercentage
            //list1.Add(i, Convert.ToDouble(e.UserState));
            list1.Add(i, end);
            myCurve.AddPoint(i, end);

            zedGraphControl1.Invalidate();
            zedGraphControl1.AxisChange();
        }

        private void bW_data_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Ereignis! occures when bW operation has completed, has been cancelled or has raised an exception
            if (e.Cancelled)
            {
                tb_show.Text = "Stop!";
                using (var file = new StreamWriter(path2 + "data" + data_coutner + ".txt", true))
                {
                    file.WriteLine(Environment.NewLine + "#S C A N  C A N C E L L E D");
                }
              //  zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot" + data_coutner + ".png"));
               // safe_fig.Enabled = true;
                showdata.Enabled = true;
                fig_name.Enabled = true;
                data_coutner += 1;
            }

            else if (e.Error != null)
            {  // an exception instance, if an error occurs during asynchronous operation, otherwise null
                tb_show.Text = e.Error.Message;
            }

            else
            {
                //tb_show.Text = Convert.ToString(e.UserState);
                btn_can.Enabled = false;
                btn_clear.Enabled = true;
                fig_name.Enabled = true;
                // safe_fig.Enabled = true;
                showdata.Enabled = true;
               // zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, "plot" + data_coutner + ".png"));
                data_coutner += 1;
            }
           
        }



        private void btn_can_Click(object sender, EventArgs e)
        {
            if (bW_data.IsBusy) // .IsBusy is true, if bW is running, otherwise false
            {
                bW_data.CancelAsync(); //cancels the background operation and sets CancellationPendiung to true!
                btn_clear.Enabled = true;
                btn_can.Enabled = false;
            }
        }



        private void btn_clear_Click(object sender, EventArgs e)
        {
            if (bW_data.IsBusy)
            {
                MessageBox.Show("Backgroundworker is still busy!");
            }

            else
            {
                tb_show.Text = "";
                lb_perc_gauss.Text = "%";
                btn_start.Enabled = true;
                btn_clear.Enabled = false;
                showdata.Enabled = false;
                safe_fig.Enabled = false;
                tb_safe.Enabled = true;
                fig_name.Enabled = false;
                if (Mg_anode.Checked) {Mg_anode.Enabled = true;}
                    else { Al_anode.Enabled = true;}
                fig_name.Clear();
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                list1.Clear();
                fuerlabels.Clear();
                myPane.YAxisList.Clear();
                myPane.AddYAxis("counts");
                progressBar1.Value = 0;
                create_graph(myPane);
                                                      zedGraphControl1.AxisChange();
                zedGraphControl1.Refresh();
            }
        }


        public void labelchanger(object sender)
        {
            var panel = sender as Control;
            //var thePanelName = panel.Name;
            //https://stackoverflow.com/questions/8000957/mouseenter-mouseleave-objectname
            int zeile = Convert.ToInt32(dictionary[panel.Name]) - 1;

            System.Windows.Forms.Label[] li = {label4,label6,label8,label10,label12,label14,label16,label18,label20,label22,
                                                   label24,label26,label28,label30,label32,label34,label36,label38,label40,label42,
                                                    label44,label46,label48,label50};
            System.Windows.Forms.Label[] eb = {s1,s2,s3,s4,s5,s6,s7,s8,s9,s10,s11,s12,s13,s14,s15,s16,s17,s18,s19,s20,s21,s22,
                                                   s23,s24,s25,s26,s27,s28,s29,s30,s31,s32,s33,s34,s35,s36,s37,s38,s39,s40,s41,s42,
                                                   s43,s44,s45,s46,s47,s48,s49,s50,s51,s52,s53,s54,s55,s56,s57,s58,s59};


            if (label51.Text == elementnames.GetToolTip(panel))
            {
                foreach (System.Windows.Forms.Label label in li)
                {
                    label.Text = "";
                }
                label51.Text = "";
                label52.Text = "";

                foreach (System.Windows.Forms.Label label in eb)
                {
                    label.Text = "--";
                }
            }

            else
            {
                int count = 2;
                foreach (System.Windows.Forms.Label label in li)
                {
                    label.Text = row[zeile][count];
                    count += 1;
                }
                label51.Text = elementnames.GetToolTip(panel);
                label52.Text = dictionary[panel.Name];

                count = 1;
                foreach (System.Windows.Forms.Label label in eb)
                {
                    label.Text = elec_bind[zeile][count];
                    count += 1;
                }
            }
        }

              

        private void Global_Button_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                colorchanger((Button)sender, yaxis);
            }
            if (e.Button == MouseButtons.Right)
            {
                labelchanger(sender);
            }
        }

          
        



        private async void safe_fig_Click(object sender, EventArgs e)
        {
            zedGraphControl1.MasterPane.GetImage().Save(Path.Combine(path2, fig_name.Text + ".png"));
            safe_fig.Text = "Fig. saved";
            safe_fig.BackColor = Color.LimeGreen;
            await Task.Delay(800);
            safe_fig.Text = "Save fig.";
            safe_fig.BackColor = Color.Transparent;
        }



        private void showdata_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", path2 + "data" + (data_coutner - 1) + ".txt");
        }



        private void tb_safe_TextChanged(object sender, EventArgs e)
        {
            tb_safe.BackColor = Color.LightGray;
            enable_start();
        }



        private void fig_name_TextChanged(object sender, EventArgs e)
        {
            if (fig_name.Text == "")
            {
                safe_fig.Enabled = false;
            }
            else
            {
                safe_fig.Enabled = true;
            }
        }



        private void Mg_anode_CheckedChanged(object sender, EventArgs e)
        {
            Mg_anode.BackColor = SystemColors.Control;
            Al_anode.BackColor = SystemColors.Control;
            if (Mg_anode.Checked)
            {
                Al_anode.Enabled = false;
            }

            else
            {
                Al_anode.Enabled = true;
            }
            enable_start();
        }



        private void Al_anode_CheckedChanged(object sender, EventArgs e)
        {
            Al_anode.BackColor = SystemColors.Control;
            Mg_anode.BackColor = SystemColors.Control;
            if (Al_anode.Checked)
            {
                Mg_anode.Enabled = false;
            }

            else
            {
                Mg_anode.Enabled = true;
            }
            enable_start();
        }

        private void enable_start ()
        {
            btn_start.Enabled = tb_safe.Text != string.Empty && (Al_anode.Checked || Mg_anode.Checked);
        }



//################################################################################################################################################################
            // ISEG TERMINAL


        private void openSessionButton_Click(object sender, EventArgs e)
        {
            using (SelectResource sr = new SelectResource())
            {
                if (lastResourceString != null)
                {
                    sr.ResourceName = lastResourceString;
                }
                DialogResult result = sr.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    lastResourceString = sr.ResourceName;
                    Cursor.Current = Cursors.WaitCursor;
                    using (var rmSession = new ResourceManager())
                    {
                        try
                        {
                            iseg = (MessageBasedSession)rmSession.Open(sr.ResourceName);
                            iseg.RawIO.Write("CONF:HVMICC HV_OK\n");

                            //SetupControlState(true);
                        }
                        catch (InvalidCastException)
                        {
                            MessageBox.Show("Resource selected must be a message-based session");
                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show(exp.Message);
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
        }

        private void closeSession_Click(object sender, System.EventArgs e)
        {
            iseg.RawIO.Write("*RST\n");
            iseg.Dispose();
        }

        private void query_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                // string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                string textToWrite = writeTextBox.Text + '\n';
                iseg.RawIO.Write(textToWrite);
                Thread.Sleep(5);
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
                Thread.Sleep(5);
                readTextBox.Text = iseg.RawIO.ReadString();
                // WARUM KLAPPT DAS NUR BEI ZUWEIMAL LESEN?
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void write_Click(object sender, EventArgs e)
        {
            try
            {
                string textToWrite = ReplaceCommonEscapeSequences(writeTextBox.Text);
                iseg.RawIO.Write(textToWrite);
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
        }

        private void read_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                readTextBox.Text = InsertCommonEscapeSequences(iseg.RawIO.ReadString());
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            readTextBox.Text = String.Empty;
        }

        private string ReplaceCommonEscapeSequences(string s)
        {
            return s.Replace("\\n", "\n").Replace("\\r", "\r");
        }

        private string InsertCommonEscapeSequences(string s)
        {
            return s.Replace("\n", "\\n").Replace("\r", "\\r");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            iseg.RawIO.Write("*RST\n");
            iseg.RawIO.Write(":VOLT 25.000,(@5)\n");
            iseg.RawIO.Write(":VOLT ON,(@5)\n");
        }

        private void stat1_CheckedChanged(object sender, EventArgs e)
        {
            if (stat1.Checked)
            {
                stat1.BackColor = Color.LimeGreen;
                //stat1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightSeaGreen;
                stat1.Text = "On";
            }

            else
            {
                stat1.BackColor = SystemColors.ControlLightLight;
            }
        }

        private void stat2_CheckedChanged(object sender, EventArgs e)
        {
            if (stat2.Checked)
            {
                stat2.BackColor = Color.LimeGreen;
                stat2.Text = "On";
            }

            else
            {
                stat2.BackColor = SystemColors.ControlLightLight;
                stat2.Text = "Off";
            }
        }

        private void stat3_CheckedChanged(object sender, EventArgs e)
        {
            if (stat3.Checked)
            {
                stat3.BackColor = Color.LimeGreen;
                stat3.Text = "On";
            }

            else
            {
                stat3.BackColor = SystemColors.ControlLightLight;
                stat3.Text = "Off";
            }
        }

        private void stat4_CheckedChanged(object sender, EventArgs e)
        {
            if (stat4.Checked)
            {
                stat4.BackColor = Color.LimeGreen;
                stat4.Text = "On";
            }

            else
            {
                stat4.BackColor = SystemColors.ControlLightLight;
                stat4.Text = "Off";
            }
        }

        private void stat5_CheckedChanged(object sender, EventArgs e)
        {
            if (stat5.Checked)
            {
                stat5.BackColor = Color.LimeGreen;
                stat5.Text = "On";
            }

            else
            {
                stat5.BackColor = SystemColors.ControlLightLight;
                stat5.Text = "Off";
            }
        }

        private void stat6_CheckedChanged(object sender, EventArgs e)
        {
            if (stat6.Checked)
            {
                stat6.BackColor = Color.LimeGreen;
                stat6.Text = "On";
            }

            else
            {
                stat6.BackColor = SystemColors.ControlLightLight;
                stat6.Text = "Off";
            }
        }


 
        //##########################################################################################################################################

    }
}




//bugs:
// - nach clear führt das abwählen von elementen zzu einem error (da ebtl. noch in liste gespeichert)