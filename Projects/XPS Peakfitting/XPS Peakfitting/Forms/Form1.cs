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
using XPS_Peakfitting.Forms;

namespace XPS_Peakfitting
{

    public partial class Form1 : Form
    {


        #region Fields
        private Form2 f2;
        stuff stuff = new stuff();
        private List<ZedGraphControl> _list_zgc = new List<ZedGraphControl>();
        private List<LineItem> _list_lineitem_raw = new List<LineItem>();
        #endregion //--------------------------------------------------------------------------------------





        #region Properties


        #endregion //--------------------------------------------------------------------------------------





        #region Constructor
        public Form1()
        {
            // access Form2 via properties
            f2 = new Form2(this);
            InitializeComponent();
        }
        #endregion //--------------------------------------------------------------------------------------






        #region Methods

        public void tester()
        {
            Console.WriteLine("test");
        }

        #endregion //--------------------------------------------------------------------------------------





        #region Events
        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = stuff.get_values_to_plot();

            if (data == null)
            {
                return;
            }

            if (tc_plots.Contains(_list_zgc.Find(x => x.AccessibleName == (data.Item2 + "_plots"))))
            {
                tc_plots.SelectTab(data.Item2);
            }

            else
            {
                f2.Raw_bg_data_item = data.Item1;
                ZedGraphControl zgc_plots = new ZedGraphControl() { AccessibleName = (data.Item2 + "_plots") };
                _list_zgc.Add(zgc_plots);
                ZedGraphControl zgc_residuals = new ZedGraphControl() { AccessibleName = (data.Item2 + "_residuals") };
                _list_zgc.Add(zgc_residuals);

                GraphPane myPane = zgc_plots.GraphPane;
                TabPage tp = new TabPage();
                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.RowCount = 2;
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));
                tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 500));
                tlp.ColumnCount = 1;
                tc_plots.TabPages.Add(tp);
                tp.Controls.Add(tlp);
                tlp.Controls.Add(zgc_plots);
                tlp.Controls.Add(zgc_residuals);
                tlp.Dock = DockStyle.Fill;
                zgc_residuals.Dock = DockStyle.Fill;
                zgc_plots.Dock = DockStyle.Fill;


                tp.Name = tp.Text = data.Item2;
                tc_plots.SelectedTab = tp;

                int red = 200;
                int green = 200;
                int blue = 200;
                myPane.Title.Text = "";
                //myPane.Title.Text = "";
                myPane.Title.FontSpec.Size = 8;
                //myPane.TitleGap = 1.6f;
                myPane.XAxis.Title.Text = "Binding energy [eV]";
                myPane.XAxis.Title.FontSpec.Size = 8;
                myPane.XAxis.Scale.FontSpec.Size = 8;
                //myPane.XAxis.Scale.IsReverse = true;
                myPane.YAxis.Title.Text = "cps";
                myPane.YAxis.Title.FontSpec.Size = 8;
                myPane.YAxis.Scale.FontSpec.Size = 8;
                myPane.XAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
                myPane.YAxis.Scale.FontSpec.FontColor = Color.FromArgb(20, 20, 20);
                myPane.Title.FontSpec.FontColor = Color.FromArgb(red, green, blue);
                //myPane.YAxis.Color = Color.FromArgb(90, 30, 0);
                //myPane.XAxis.Color = Color.FromArgb(red, green, blue);
                myPane.YAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
                myPane.YAxis.MinorTic.Color = Color.FromArgb(red, green, blue);
                myPane.XAxis.MajorTic.Color = Color.FromArgb(red, green, blue);
                myPane.XAxis.MinorTic.Color = Color.FromArgb(red, green, blue);

                foreach (var item in _list_zgc)
                {
                    Console.WriteLine(item.AccessibleName);
                }
            }

        }

        private void btn_analyse_Click(object sender, EventArgs e)
        {
            if (f2.IsDisposed)
            {
                f2 = new Form2(this);
            }
            f2.Show(); 
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //f2 = new Form2(this);
        }

        private void btn_close_tab_Click(object sender, EventArgs e)
        {
            
            if (tc_plots.SelectedTab !=null)
            {
                var index = _list_zgc.FindIndex(x => x.AccessibleName == tc_plots.SelectedTab.Name + "_plots");
                _list_zgc.RemoveRange(index, 2);
                tc_plots.SelectedTab.Dispose();
            }

            foreach (var item in _list_zgc)
            {
                Console.WriteLine(item.AccessibleName);
            }
        }

        #endregion //--------------------------------------------------------------------------------------


    }
}
