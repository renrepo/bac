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

        private void btn_open_Click(object sender, EventArgs e)
        {
            var data = m.get_values_to_plot();

            if (data == null) return;
            if (list_stuff.Contains(list_stuff.Find(a => a.Data_name == data.Item3))) tc_zgc.SelectTab(data.Item3);
            else { stuff s = new stuff(data.Item1, data.Item2, data.Item3, tc_zgc); list_stuff.Add(s); s.Draw_Line(data.Item1,data.Item2, data.Item3);
                s.Remove_Line(null);
            }
            

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            if (tc_zgc.SelectedTab != null) list_stuff.RemoveAt(list_stuff.FindIndex(a => a.Data_name == tc_zgc.SelectedTab.Name)); tc_zgc.SelectedTab.Dispose();
        }

        #endregion //-------------------------------------------------------------------------------------
    }
}
