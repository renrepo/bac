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

    public partial class Form_Main : Form
    {
        Form_Fitmodels fm;
        GetData GD;
        List<zgc_class> zgc_list = new List<zgc_class>();




        //bg_processing sh;


        #region Constructor
        public Form_Main()
        {
            InitializeComponent();
        }
        #endregion


        #region Load_Main
        private void Main_Load(object sender, EventArgs e)
        {
            GD = new GetData();
            //sh = new bg_processing();
            fm = new Form_Fitmodels(this);
        }
        #endregion

//#################################################################################################################################################################################
        #region Methods
        private void btn_load_data_Click(object sender, EventArgs e)
        {    
            var data = GD.get_values_to_plot();

            if (zgc_list.Contains(zgc_list.Find(x => x.class_name == data.Item3)))
            {
                tc_plots.SelectTab(data.Item3);
            }

            else
            {
                zgc_class zw = new zgc_class(data.Item3, tc_plots);
                zgc_list.Add(zw);
                zw.plot_data(data.Item1, data.Item2, data.Item3 + "_data");
            }                 
        }

        private void btn_processing_Click(object sender, EventArgs e)
        {
            if (fm.IsDisposed)
            {
                fm = new Form_Fitmodels(this);
            }
            fm.Show();
        }
        

        private void btn_del_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {
                zgc_list.RemoveAt(zgc_list.FindIndex(x => x.class_name == tc_plots.SelectedTab.Name));
                tc_plots.TabPages.Remove(tc_plots.SelectedTab);
            }                           
        }


        private void btn_bg_Click(object sender, EventArgs e)
        {
            if (tc_plots.SelectedTab != null)
            {
                var zc = zgc_list.Find(x => x.class_name == tc_plots.SelectedTab.Name);
                zc.disable_zoom();
                zc.zgc.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseDownEvent);
                zc.zgc.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseUpEvent);
                zc.zgc.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseMoveEvent);
            }
        }

        public void get_coordinates()
        {
            if (tc_plots.SelectedTab != null)
            {
                var zc = zgc_list.Find(x => x.class_name == tc_plots.SelectedTab.Name);
                zc.disable_zoom();
                zc.zgc.MouseDownEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseDownEvent);
                zc.zgc.MouseUpEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseUpEvent);
                zc.zgc.MouseMoveEvent += new ZedGraph.ZedGraphControl.ZedMouseEventHandler(zc.zgc_MouseMoveEvent);
            }
        }


        private void btn_bg_add_Click(object sender, EventArgs e)
        {
            List<ZedGraphControl> zgc_list = new List<ZedGraphControl>();
            zgc_list.Add(new ZedGraphControl() { AccessibleName = "z1" });
            zgc_list.RemoveAt(zgc_list.FindIndex(x => x.AccessibleName == "z1"));
        }


        private void tc_plots_SelectedIndexChanged(object sender, EventArgs e)
        {
            fm.tb_changed(tc_plots.SelectedTab.Name);
        }


        #endregion
    }

}
