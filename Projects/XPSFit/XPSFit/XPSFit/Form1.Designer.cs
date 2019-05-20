namespace XPSFit
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tc_zgc = new System.Windows.Forms.TabControl();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.dgv_bg = new System.Windows.Forms.DataGridView();
            this.dgv_bg_sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_bg_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_models = new System.Windows.Forms.DataGridView();
            this.dgv_models_models = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_models_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_cen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_wid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_mix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_area = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_area_perc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_A = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_c = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_s = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_m = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_tester = new System.Windows.Forms.Button();
            this.cb_Bg_Sub = new System.Windows.Forms.CheckBox();
            this.btn_fit = new System.Windows.Forms.Button();
            this.cb_disc = new System.Windows.Forms.CheckBox();
            this.comb_disc = new System.Windows.Forms.ComboBox();
            this.btn_save_fig = new System.Windows.Forms.Button();
            this.btn_find_m = new System.Windows.Forms.Button();
            this.lb_iter_text = new System.Windows.Forms.Label();
            this.lb_iter = new System.Windows.Forms.Label();
            this.lb_time_text = new System.Windows.Forms.Label();
            this.lb_time = new System.Windows.Forms.Label();
            this.lb_chisq_text = new System.Windows.Forms.Label();
            this.lb_chisq = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pb_fit_result = new System.Windows.Forms.PictureBox();
            this.lb_fit_converge = new System.Windows.Forms.Label();
            this.gp_energy_calib = new System.Windows.Forms.GroupBox();
            this.cb_energy_calib = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_energy_calib_true = new System.Windows.Forms.TextBox();
            this.tb_energy_calib_meas = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.chb_weight = new System.Windows.Forms.CheckBox();
            this.btn_copy_rows = new System.Windows.Forms.Button();
            this.btn_paste_rows = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_fit_result)).BeginInit();
            this.gp_energy_calib.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tc_zgc
            // 
            this.tc_zgc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_zgc.Location = new System.Drawing.Point(3, 161);
            this.tc_zgc.Name = "tc_zgc";
            this.tc_zgc.SelectedIndex = 0;
            this.tc_zgc.Size = new System.Drawing.Size(1380, 798);
            this.tc_zgc.TabIndex = 0;
            // 
            // btn_open
            // 
            this.btn_open.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_open.BackgroundImage")));
            this.btn_open.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_open.Location = new System.Drawing.Point(7, 4);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(40, 40);
            this.btn_open.TabIndex = 1;
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_close.BackgroundImage")));
            this.btn_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_close.Location = new System.Drawing.Point(99, 4);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(40, 40);
            this.btn_close.TabIndex = 2;
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // dgv_bg
            // 
            this.dgv_bg.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_bg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_bg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_bg_sel,
            this.dgv_bg_model});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_bg.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_bg.Enabled = false;
            this.dgv_bg.Location = new System.Drawing.Point(489, 5);
            this.dgv_bg.Name = "dgv_bg";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_bg.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_bg.Size = new System.Drawing.Size(174, 86);
            this.dgv_bg.TabIndex = 3;
            // 
            // dgv_bg_sel
            // 
            this.dgv_bg_sel.HeaderText = "set";
            this.dgv_bg_sel.Name = "dgv_bg_sel";
            this.dgv_bg_sel.Width = 30;
            // 
            // dgv_bg_model
            // 
            this.dgv_bg_model.HeaderText = "Background";
            this.dgv_bg_model.Items.AddRange(new object[] {
            "Shirley",
            "Linear",
            "Remove"});
            this.dgv_bg_model.Name = "dgv_bg_model";
            this.dgv_bg_model.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_bg_model.Width = 80;
            // 
            // dgv_models
            // 
            this.dgv_models.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_models.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_models.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_models.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_models_models,
            this.dgv_models_amp,
            this.dgv_models_cen,
            this.dgv_models_wid,
            this.dgv_models_mix,
            this.dgv_models_area,
            this.dgv_models_area_perc,
            this.dgv_models_A,
            this.dgv_models_c,
            this.dgv_models_s,
            this.dgv_models_m});
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_models.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_models.Enabled = false;
            this.dgv_models.Location = new System.Drawing.Point(671, 5);
            this.dgv_models.Name = "dgv_models";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_models.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgv_models.Size = new System.Drawing.Size(710, 154);
            this.dgv_models.TabIndex = 4;
            this.dgv_models.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_models_CellClick);
            this.dgv_models.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.dgv_models_CellStateChanged);
            this.dgv_models.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgv_models_KeyUp);
            // 
            // dgv_models_models
            // 
            this.dgv_models_models.HeaderText = "Model";
            this.dgv_models_models.Items.AddRange(new object[] {
            "L",
            "G",
            "GLS",
            "GLP",
            "Remove"});
            this.dgv_models_models.Name = "dgv_models_models";
            this.dgv_models_models.Width = 80;
            // 
            // dgv_models_amp
            // 
            this.dgv_models_amp.HeaderText = "A_init";
            this.dgv_models_amp.Name = "dgv_models_amp";
            this.dgv_models_amp.Width = 60;
            // 
            // dgv_models_cen
            // 
            this.dgv_models_cen.HeaderText = "c_init";
            this.dgv_models_cen.Name = "dgv_models_cen";
            this.dgv_models_cen.Width = 60;
            // 
            // dgv_models_wid
            // 
            this.dgv_models_wid.HeaderText = "FWHM_i.";
            this.dgv_models_wid.Name = "dgv_models_wid";
            this.dgv_models_wid.Width = 60;
            // 
            // dgv_models_mix
            // 
            this.dgv_models_mix.HeaderText = "m_init";
            this.dgv_models_mix.Name = "dgv_models_mix";
            this.dgv_models_mix.Width = 60;
            // 
            // dgv_models_area
            // 
            this.dgv_models_area.HeaderText = "Area_i.";
            this.dgv_models_area.Name = "dgv_models_area";
            this.dgv_models_area.ReadOnly = true;
            this.dgv_models_area.Width = 60;
            // 
            // dgv_models_area_perc
            // 
            this.dgv_models_area_perc.HeaderText = "%";
            this.dgv_models_area_perc.Name = "dgv_models_area_perc";
            this.dgv_models_area_perc.ReadOnly = true;
            this.dgv_models_area_perc.Width = 45;
            // 
            // dgv_models_A
            // 
            this.dgv_models_A.HeaderText = "A";
            this.dgv_models_A.Name = "dgv_models_A";
            this.dgv_models_A.ReadOnly = true;
            this.dgv_models_A.Width = 60;
            // 
            // dgv_models_c
            // 
            this.dgv_models_c.HeaderText = "c";
            this.dgv_models_c.Name = "dgv_models_c";
            this.dgv_models_c.ReadOnly = true;
            this.dgv_models_c.Width = 60;
            // 
            // dgv_models_s
            // 
            this.dgv_models_s.HeaderText = "FWHM";
            this.dgv_models_s.Name = "dgv_models_s";
            this.dgv_models_s.ReadOnly = true;
            this.dgv_models_s.Width = 60;
            // 
            // dgv_models_m
            // 
            this.dgv_models_m.HeaderText = "m";
            this.dgv_models_m.Name = "dgv_models_m";
            this.dgv_models_m.ReadOnly = true;
            this.dgv_models_m.Width = 60;
            // 
            // btn_tester
            // 
            this.btn_tester.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_tester.Location = new System.Drawing.Point(284, 4);
            this.btn_tester.Name = "btn_tester";
            this.btn_tester.Size = new System.Drawing.Size(40, 40);
            this.btn_tester.TabIndex = 6;
            this.btn_tester.Text = "TEST";
            this.btn_tester.UseVisualStyleBackColor = true;
            this.btn_tester.Click += new System.EventHandler(this.btn_tester_Click);
            // 
            // cb_Bg_Sub
            // 
            this.cb_Bg_Sub.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_Bg_Sub.AutoSize = true;
            this.cb_Bg_Sub.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_Bg_Sub.Location = new System.Drawing.Point(0, 0);
            this.cb_Bg_Sub.Margin = new System.Windows.Forms.Padding(0);
            this.cb_Bg_Sub.Name = "cb_Bg_Sub";
            this.cb_Bg_Sub.Size = new System.Drawing.Size(40, 40);
            this.cb_Bg_Sub.TabIndex = 8;
            this.cb_Bg_Sub.Text = "BG SUB";
            this.cb_Bg_Sub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_Bg_Sub.UseVisualStyleBackColor = true;
            this.cb_Bg_Sub.CheckedChanged += new System.EventHandler(this.cb_Bg_Sub_CheckedChanged);
            // 
            // btn_fit
            // 
            this.btn_fit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_fit.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fit.Location = new System.Drawing.Point(145, 4);
            this.btn_fit.Name = "btn_fit";
            this.btn_fit.Size = new System.Drawing.Size(40, 40);
            this.btn_fit.TabIndex = 9;
            this.btn_fit.Text = "FIT";
            this.btn_fit.UseVisualStyleBackColor = true;
            this.btn_fit.Click += new System.EventHandler(this.btn_fit_Click);
            // 
            // cb_disc
            // 
            this.cb_disc.AutoSize = true;
            this.cb_disc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cb_disc.Enabled = false;
            this.cb_disc.Location = new System.Drawing.Point(0, 0);
            this.cb_disc.Margin = new System.Windows.Forms.Padding(0);
            this.cb_disc.Name = "cb_disc";
            this.cb_disc.Size = new System.Drawing.Size(52, 18);
            this.cb_disc.TabIndex = 11;
            this.cb_disc.Text = "DISC";
            this.cb_disc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cb_disc.UseVisualStyleBackColor = true;
            this.cb_disc.CheckedChanged += new System.EventHandler(this.cb_disc_CheckedChanged);
            // 
            // comb_disc
            // 
            this.comb_disc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comb_disc.FormattingEnabled = true;
            this.comb_disc.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "200",
            "400",
            "500"});
            this.comb_disc.Location = new System.Drawing.Point(0, 18);
            this.comb_disc.Margin = new System.Windows.Forms.Padding(0);
            this.comb_disc.Name = "comb_disc";
            this.comb_disc.Size = new System.Drawing.Size(52, 21);
            this.comb_disc.TabIndex = 12;
            // 
            // btn_save_fig
            // 
            this.btn_save_fig.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_save_fig.BackgroundImage")));
            this.btn_save_fig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_save_fig.Location = new System.Drawing.Point(53, 4);
            this.btn_save_fig.Name = "btn_save_fig";
            this.btn_save_fig.Size = new System.Drawing.Size(40, 40);
            this.btn_save_fig.TabIndex = 14;
            this.btn_save_fig.UseVisualStyleBackColor = true;
            this.btn_save_fig.Click += new System.EventHandler(this.btn_save_fig_Click);
            // 
            // btn_find_m
            // 
            this.btn_find_m.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_find_m.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_find_m.Location = new System.Drawing.Point(191, 4);
            this.btn_find_m.Name = "btn_find_m";
            this.btn_find_m.Size = new System.Drawing.Size(40, 40);
            this.btn_find_m.TabIndex = 15;
            this.btn_find_m.Text = "FIT MIX";
            this.btn_find_m.UseVisualStyleBackColor = true;
            this.btn_find_m.Click += new System.EventHandler(this.btn_find_m_Click);
            // 
            // lb_iter_text
            // 
            this.lb_iter_text.AutoSize = true;
            this.lb_iter_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_iter_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_iter_text.Location = new System.Drawing.Point(3, 0);
            this.lb_iter_text.Name = "lb_iter_text";
            this.lb_iter_text.Size = new System.Drawing.Size(64, 18);
            this.lb_iter_text.TabIndex = 16;
            this.lb_iter_text.Text = "Iterations:";
            // 
            // lb_iter
            // 
            this.lb_iter.AutoSize = true;
            this.lb_iter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_iter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_iter.Location = new System.Drawing.Point(73, 0);
            this.lb_iter.Name = "lb_iter";
            this.lb_iter.Size = new System.Drawing.Size(39, 18);
            this.lb_iter.TabIndex = 17;
            this.lb_iter.Text = "---";
            // 
            // lb_time_text
            // 
            this.lb_time_text.AutoSize = true;
            this.lb_time_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_time_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_time_text.Location = new System.Drawing.Point(3, 18);
            this.lb_time_text.Name = "lb_time_text";
            this.lb_time_text.Size = new System.Drawing.Size(64, 18);
            this.lb_time_text.TabIndex = 18;
            this.lb_time_text.Text = "Time [ms]:";
            // 
            // lb_time
            // 
            this.lb_time.AutoSize = true;
            this.lb_time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_time.Location = new System.Drawing.Point(73, 18);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(39, 18);
            this.lb_time.TabIndex = 19;
            this.lb_time.Text = "---";
            // 
            // lb_chisq_text
            // 
            this.lb_chisq_text.AutoSize = true;
            this.lb_chisq_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_chisq_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_chisq_text.Location = new System.Drawing.Point(3, 36);
            this.lb_chisq_text.Name = "lb_chisq_text";
            this.lb_chisq_text.Size = new System.Drawing.Size(64, 18);
            this.lb_chisq_text.TabIndex = 20;
            this.lb_chisq_text.Text = "Chisq:";
            // 
            // lb_chisq
            // 
            this.lb_chisq.AutoSize = true;
            this.lb_chisq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_chisq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_chisq.Location = new System.Drawing.Point(73, 36);
            this.lb_chisq.Name = "lb_chisq";
            this.lb_chisq.Size = new System.Drawing.Size(39, 18);
            this.lb_chisq.TabIndex = 21;
            this.lb_chisq.Text = "---";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Controls.Add(this.lb_iter_text, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_iter, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_chisq, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lb_time_text, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lb_chisq_text, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lb_time, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(505, 98);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(115, 54);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // pb_fit_result
            // 
            this.pb_fit_result.Location = new System.Drawing.Point(629, 99);
            this.pb_fit_result.Name = "pb_fit_result";
            this.pb_fit_result.Size = new System.Drawing.Size(25, 25);
            this.pb_fit_result.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_fit_result.TabIndex = 24;
            this.pb_fit_result.TabStop = false;
            // 
            // lb_fit_converge
            // 
            this.lb_fit_converge.AutoSize = true;
            this.lb_fit_converge.BackColor = System.Drawing.Color.Transparent;
            this.lb_fit_converge.Location = new System.Drawing.Point(627, 131);
            this.lb_fit_converge.Name = "lb_fit_converge";
            this.lb_fit_converge.Size = new System.Drawing.Size(0, 13);
            this.lb_fit_converge.TabIndex = 25;
            // 
            // gp_energy_calib
            // 
            this.gp_energy_calib.Controls.Add(this.cb_energy_calib);
            this.gp_energy_calib.Controls.Add(this.label2);
            this.gp_energy_calib.Controls.Add(this.label1);
            this.gp_energy_calib.Controls.Add(this.tb_energy_calib_true);
            this.gp_energy_calib.Controls.Add(this.tb_energy_calib_meas);
            this.gp_energy_calib.Location = new System.Drawing.Point(7, 55);
            this.gp_energy_calib.Name = "gp_energy_calib";
            this.gp_energy_calib.Size = new System.Drawing.Size(121, 95);
            this.gp_energy_calib.TabIndex = 27;
            this.gp_energy_calib.TabStop = false;
            this.gp_energy_calib.Text = "Energy Calibration";
            // 
            // cb_energy_calib
            // 
            this.cb_energy_calib.AutoSize = true;
            this.cb_energy_calib.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cb_energy_calib.Location = new System.Drawing.Point(16, 70);
            this.cb_energy_calib.Name = "cb_energy_calib";
            this.cb_energy_calib.Size = new System.Drawing.Size(97, 17);
            this.cb_energy_calib.TabIndex = 32;
            this.cb_energy_calib.Text = "Use Calibration";
            this.cb_energy_calib.UseVisualStyleBackColor = true;
            this.cb_energy_calib.CheckedChanged += new System.EventHandler(this.cb_energy_calib_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "True";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Measured";
            // 
            // tb_energy_calib_true
            // 
            this.tb_energy_calib_true.Location = new System.Drawing.Point(64, 44);
            this.tb_energy_calib_true.Name = "tb_energy_calib_true";
            this.tb_energy_calib_true.Size = new System.Drawing.Size(49, 20);
            this.tb_energy_calib_true.TabIndex = 29;
            // 
            // tb_energy_calib_meas
            // 
            this.tb_energy_calib_meas.Location = new System.Drawing.Point(64, 20);
            this.tb_energy_calib_meas.Name = "tb_energy_calib_meas";
            this.tb_energy_calib_meas.Size = new System.Drawing.Size(49, 20);
            this.tb_energy_calib_meas.TabIndex = 28;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AutoSize = true;
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tableLayoutPanel2.Controls.Add(this.cb_disc, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comb_disc, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(332, 4);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(52, 40);
            this.tableLayoutPanel2.TabIndex = 28;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Controls.Add(this.cb_Bg_Sub, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(237, 4);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(40, 40);
            this.tableLayoutPanel3.TabIndex = 29;
            // 
            // chb_weight
            // 
            this.chb_weight.AutoSize = true;
            this.chb_weight.Checked = true;
            this.chb_weight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chb_weight.Location = new System.Drawing.Point(145, 64);
            this.chb_weight.Name = "chb_weight";
            this.chb_weight.Size = new System.Drawing.Size(93, 17);
            this.chb_weight.TabIndex = 30;
            this.chb_weight.Text = "Weight SQSQ";
            this.chb_weight.ThreeState = true;
            this.chb_weight.UseVisualStyleBackColor = true;
            this.chb_weight.CheckStateChanged += new System.EventHandler(this.chb_weight_CheckStateChanged);
            // 
            // btn_copy_rows
            // 
            this.btn_copy_rows.Location = new System.Drawing.Point(408, 5);
            this.btn_copy_rows.Name = "btn_copy_rows";
            this.btn_copy_rows.Size = new System.Drawing.Size(75, 23);
            this.btn_copy_rows.TabIndex = 31;
            this.btn_copy_rows.Text = "Copy rows";
            this.btn_copy_rows.UseVisualStyleBackColor = true;
            this.btn_copy_rows.Click += new System.EventHandler(this.btn_copy_rows_Click);
            // 
            // btn_paste_rows
            // 
            this.btn_paste_rows.Location = new System.Drawing.Point(408, 31);
            this.btn_paste_rows.Name = "btn_paste_rows";
            this.btn_paste_rows.Size = new System.Drawing.Size(75, 23);
            this.btn_paste_rows.TabIndex = 32;
            this.btn_paste_rows.Text = "Paste rows";
            this.btn_paste_rows.UseVisualStyleBackColor = true;
            this.btn_paste_rows.Click += new System.EventHandler(this.btn_paste_rows_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.btn_paste_rows);
            this.Controls.Add(this.btn_copy_rows);
            this.Controls.Add(this.chb_weight);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.gp_energy_calib);
            this.Controls.Add(this.lb_fit_converge);
            this.Controls.Add(this.pb_fit_result);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btn_find_m);
            this.Controls.Add(this.btn_save_fig);
            this.Controls.Add(this.btn_fit);
            this.Controls.Add(this.btn_tester);
            this.Controls.Add(this.dgv_models);
            this.Controls.Add(this.dgv_bg);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.tc_zgc);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "u";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_fit_result)).EndInit();
            this.gp_energy_calib.ResumeLayout(false);
            this.gp_energy_calib.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tc_zgc;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.DataGridView dgv_bg;
        private System.Windows.Forms.DataGridView dgv_models;
        private System.Windows.Forms.Button btn_tester;
        private System.Windows.Forms.CheckBox cb_Bg_Sub;
        private System.Windows.Forms.Button btn_fit;
        private System.Windows.Forms.CheckBox cb_disc;
        private System.Windows.Forms.ComboBox comb_disc;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgv_bg_sel;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_bg_model;
        private System.Windows.Forms.Button btn_save_fig;
        private System.Windows.Forms.Button btn_find_m;
        private System.Windows.Forms.Label lb_iter_text;
        private System.Windows.Forms.Label lb_iter;
        private System.Windows.Forms.Label lb_time_text;
        private System.Windows.Forms.Label lb_time;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_models_models;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_cen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_wid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_mix;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_area;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_area_perc;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_A;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_c;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_s;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_m;
        private System.Windows.Forms.Label lb_chisq_text;
        private System.Windows.Forms.Label lb_chisq;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pb_fit_result;
        private System.Windows.Forms.Label lb_fit_converge;
        private System.Windows.Forms.GroupBox gp_energy_calib;
        private System.Windows.Forms.CheckBox cb_energy_calib;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_energy_calib_true;
        private System.Windows.Forms.TextBox tb_energy_calib_meas;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.CheckBox chb_weight;
        private System.Windows.Forms.Button btn_copy_rows;
        private System.Windows.Forms.Button btn_paste_rows;
    }
}

