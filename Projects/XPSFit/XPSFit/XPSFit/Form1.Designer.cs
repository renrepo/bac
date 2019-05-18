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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.cb_weight = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pb_fit_result = new System.Windows.Forms.PictureBox();
            this.lb_fit_converge = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.gp_energy_calib = new System.Windows.Forms.GroupBox();
            this.tb_energy_calib_meas = new System.Windows.Forms.TextBox();
            this.tb_energy_calib_true = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cb_energy_calib = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_fit_result)).BeginInit();
            this.gp_energy_calib.SuspendLayout();
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
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_bg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_bg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_bg_sel,
            this.dgv_bg_model});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_bg.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_bg.Enabled = false;
            this.dgv_bg.Location = new System.Drawing.Point(508, 5);
            this.dgv_bg.Name = "dgv_bg";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_bg.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgv_bg.Size = new System.Drawing.Size(155, 79);
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
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_models.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
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
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_models.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgv_models.Enabled = false;
            this.dgv_models.Location = new System.Drawing.Point(671, 5);
            this.dgv_models.Name = "dgv_models";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_models.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
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
            this.btn_tester.Location = new System.Drawing.Point(247, 42);
            this.btn_tester.Name = "btn_tester";
            this.btn_tester.Size = new System.Drawing.Size(70, 35);
            this.btn_tester.TabIndex = 6;
            this.btn_tester.Text = "TEST";
            this.btn_tester.UseVisualStyleBackColor = true;
            this.btn_tester.Click += new System.EventHandler(this.btn_tester_Click);
            // 
            // cb_Bg_Sub
            // 
            this.cb_Bg_Sub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_Bg_Sub.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_Bg_Sub.AutoSize = true;
            this.cb_Bg_Sub.Location = new System.Drawing.Point(260, 0);
            this.cb_Bg_Sub.Name = "cb_Bg_Sub";
            this.cb_Bg_Sub.Size = new System.Drawing.Size(57, 23);
            this.cb_Bg_Sub.TabIndex = 8;
            this.cb_Bg_Sub.Text = "BG SUB";
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
            this.cb_disc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cb_disc.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_disc.AutoSize = true;
            this.cb_disc.Enabled = false;
            this.cb_disc.Location = new System.Drawing.Point(323, 0);
            this.cb_disc.Name = "cb_disc";
            this.cb_disc.Size = new System.Drawing.Size(71, 23);
            this.cb_disc.TabIndex = 11;
            this.cb_disc.Text = "DISCRETE";
            this.cb_disc.UseVisualStyleBackColor = true;
            this.cb_disc.CheckedChanged += new System.EventHandler(this.cb_disc_CheckedChanged);
            // 
            // comb_disc
            // 
            this.comb_disc.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comb_disc.FormattingEnabled = true;
            this.comb_disc.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "200",
            "400",
            "500",
            "1000"});
            this.comb_disc.Location = new System.Drawing.Point(351, 49);
            this.comb_disc.Name = "comb_disc";
            this.comb_disc.Size = new System.Drawing.Size(47, 21);
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
            this.lb_iter_text.Size = new System.Drawing.Size(51, 20);
            this.lb_iter_text.TabIndex = 16;
            this.lb_iter_text.Text = "Iterations:";
            // 
            // lb_iter
            // 
            this.lb_iter.AutoSize = true;
            this.lb_iter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_iter.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_iter.Location = new System.Drawing.Point(63, 0);
            this.lb_iter.Name = "lb_iter";
            this.lb_iter.Size = new System.Drawing.Size(19, 15);
            this.lb_iter.TabIndex = 17;
            this.lb_iter.Text = "---";
            // 
            // lb_time_text
            // 
            this.lb_time_text.AutoSize = true;
            this.lb_time_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_time_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_time_text.Location = new System.Drawing.Point(3, 20);
            this.lb_time_text.Name = "lb_time_text";
            this.lb_time_text.Size = new System.Drawing.Size(38, 20);
            this.lb_time_text.TabIndex = 18;
            this.lb_time_text.Text = "Time [ms]:";
            // 
            // lb_time
            // 
            this.lb_time.AutoSize = true;
            this.lb_time.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_time.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_time.Location = new System.Drawing.Point(63, 20);
            this.lb_time.Name = "lb_time";
            this.lb_time.Size = new System.Drawing.Size(19, 15);
            this.lb_time.TabIndex = 19;
            this.lb_time.Text = "---";
            // 
            // lb_chisq_text
            // 
            this.lb_chisq_text.AutoSize = true;
            this.lb_chisq_text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_chisq_text.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_chisq_text.Location = new System.Drawing.Point(3, 40);
            this.lb_chisq_text.Name = "lb_chisq_text";
            this.lb_chisq_text.Size = new System.Drawing.Size(41, 15);
            this.lb_chisq_text.TabIndex = 20;
            this.lb_chisq_text.Text = "Chisq:";
            // 
            // lb_chisq
            // 
            this.lb_chisq.AutoSize = true;
            this.lb_chisq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_chisq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_chisq.Location = new System.Drawing.Point(63, 40);
            this.lb_chisq.Name = "lb_chisq";
            this.lb_chisq.Size = new System.Drawing.Size(19, 15);
            this.lb_chisq.TabIndex = 21;
            this.lb_chisq.Text = "---";
            // 
            // cb_weight
            // 
            this.cb_weight.FormattingEnabled = true;
            this.cb_weight.Items.AddRange(new object[] {
            "sq",
            "sqsq",
            "one"});
            this.cb_weight.Location = new System.Drawing.Point(432, 63);
            this.cb_weight.Name = "cb_weight";
            this.cb_weight.Size = new System.Drawing.Size(70, 21);
            this.cb_weight.TabIndex = 22;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Controls.Add(this.lb_iter_text, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_iter, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lb_chisq, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lb_time_text, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lb_chisq_text, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lb_time, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(508, 90);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(105, 60);
            this.tableLayoutPanel1.TabIndex = 23;
            // 
            // pb_fit_result
            // 
            this.pb_fit_result.Location = new System.Drawing.Point(629, 91);
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
            this.lb_fit_converge.Location = new System.Drawing.Point(627, 123);
            this.lb_fit_converge.Name = "lb_fit_converge";
            this.lb_fit_converge.Size = new System.Drawing.Size(0, 13);
            this.lb_fit_converge.TabIndex = 25;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(400, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(85, 17);
            this.radioButton1.TabIndex = 26;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // gp_energy_calib
            // 
            this.gp_energy_calib.Controls.Add(this.cb_energy_calib);
            this.gp_energy_calib.Controls.Add(this.label2);
            this.gp_energy_calib.Controls.Add(this.label1);
            this.gp_energy_calib.Controls.Add(this.tb_energy_calib_true);
            this.gp_energy_calib.Controls.Add(this.tb_energy_calib_meas);
            this.gp_energy_calib.Location = new System.Drawing.Point(7, 60);
            this.gp_energy_calib.Name = "gp_energy_calib";
            this.gp_energy_calib.Size = new System.Drawing.Size(121, 95);
            this.gp_energy_calib.TabIndex = 27;
            this.gp_energy_calib.TabStop = false;
            this.gp_energy_calib.Text = "Energy Calibration";
            // 
            // tb_energy_calib_meas
            // 
            this.tb_energy_calib_meas.Location = new System.Drawing.Point(64, 20);
            this.tb_energy_calib_meas.Name = "tb_energy_calib_meas";
            this.tb_energy_calib_meas.Size = new System.Drawing.Size(49, 20);
            this.tb_energy_calib_meas.TabIndex = 28;
            // 
            // tb_energy_calib_true
            // 
            this.tb_energy_calib_true.Location = new System.Drawing.Point(64, 44);
            this.tb_energy_calib_true.Name = "tb_energy_calib_true";
            this.tb_energy_calib_true.Size = new System.Drawing.Size(49, 20);
            this.tb_energy_calib_true.TabIndex = 29;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "True";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.gp_energy_calib);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.lb_fit_converge);
            this.Controls.Add(this.pb_fit_result);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.cb_weight);
            this.Controls.Add(this.btn_find_m);
            this.Controls.Add(this.btn_save_fig);
            this.Controls.Add(this.comb_disc);
            this.Controls.Add(this.cb_disc);
            this.Controls.Add(this.btn_fit);
            this.Controls.Add(this.cb_Bg_Sub);
            this.Controls.Add(this.btn_tester);
            this.Controls.Add(this.dgv_models);
            this.Controls.Add(this.dgv_bg);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.tc_zgc);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
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
        private System.Windows.Forms.ComboBox cb_weight;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pb_fit_result;
        private System.Windows.Forms.Label lb_fit_converge;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox gp_energy_calib;
        private System.Windows.Forms.CheckBox cb_energy_calib;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_energy_calib_true;
        private System.Windows.Forms.TextBox tb_energy_calib_meas;
    }
}

