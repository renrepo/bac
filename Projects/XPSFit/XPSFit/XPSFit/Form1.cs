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
        }


        private void Tc_zgc_SelectedIndexChanged(object sender, TabControlEventArgs e)
        {
            Curr_S = tc_zgc.TabPages.Count > 0 ?  list_stuff.Find(a => a.Data_name == (sender as TabControl).SelectedTab.Name) : null;
        }


        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot();

            if (data == null) return;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3);
            else { list_stuff.Add(new stuff(data.Item1, data.Item2, data.Item3, tc_zgc)); Curr_S = list_stuff[list_stuff.Count - 1]; }
        }


        private void btn_close_Click(object sender, EventArgs e)
        {
            if (tc_zgc.SelectedTab != null)
            {
                list_stuff.Remove(Curr_S);
                tc_zgc.SelectedTab.Dispose();
            }            
        }

        #endregion //-------------------------------------------------------------------------------------

        
    }
}
