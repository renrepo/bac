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
            this.tc_zgc = new System.Windows.Forms.TabControl();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.dgv_bg = new System.Windows.Forms.DataGridView();
            this.dgv_bg_sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgv_bg_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_models = new System.Windows.Forms.DataGridView();
            this.btn_tester = new System.Windows.Forms.Button();
            this.cb_Bg_Sub = new System.Windows.Forms.CheckBox();
            this.btn_fit = new System.Windows.Forms.Button();
            this.cb_disc = new System.Windows.Forms.CheckBox();
            this.comb_disc = new System.Windows.Forms.ComboBox();
            this.tb_chi2 = new System.Windows.Forms.TextBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).BeginInit();
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
            this.btn_open.Location = new System.Drawing.Point(13, 13);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(70, 35);
            this.btn_open.TabIndex = 1;
            this.btn_open.Text = "Open";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_close
            // 
            this.btn_close.Location = new System.Drawing.Point(90, 13);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(70, 35);
            this.btn_close.TabIndex = 2;
            this.btn_close.Text = "Close";
            this.btn_close.UseVisualStyleBackColor = true;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // dgv_bg
            // 
            this.dgv_bg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_bg_sel,
            this.dgv_bg_model});
            this.dgv_bg.Enabled = false;
            this.dgv_bg.Location = new System.Drawing.Point(508, 5);
            this.dgv_bg.Name = "dgv_bg";
            this.dgv_bg.Size = new System.Drawing.Size(155, 101);
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
            this.dgv_models.Enabled = false;
            this.dgv_models.Location = new System.Drawing.Point(671, 5);
            this.dgv_models.Name = "dgv_models";
            this.dgv_models.Size = new System.Drawing.Size(710, 154);
            this.dgv_models.TabIndex = 4;
            this.dgv_models.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_models_CellClick);
            this.dgv_models.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgv_models_KeyUp);
            // 
            // btn_tester
            // 
            this.btn_tester.Location = new System.Drawing.Point(166, 13);
            this.btn_tester.Name = "btn_tester";
            this.btn_tester.Size = new System.Drawing.Size(70, 35);
            this.btn_tester.TabIndex = 6;
            this.btn_tester.Text = "TEST";
            this.btn_tester.UseVisualStyleBackColor = true;
            this.btn_tester.Click += new System.EventHandler(this.btn_tester_Click);
            // 
            // cb_Bg_Sub
            // 
            this.cb_Bg_Sub.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_Bg_Sub.AutoSize = true;
            this.cb_Bg_Sub.Location = new System.Drawing.Point(508, 136);
            this.cb_Bg_Sub.Name = "cb_Bg_Sub";
            this.cb_Bg_Sub.Size = new System.Drawing.Size(57, 23);
            this.cb_Bg_Sub.TabIndex = 8;
            this.cb_Bg_Sub.Text = "BG-Sub.";
            this.cb_Bg_Sub.UseVisualStyleBackColor = true;
            this.cb_Bg_Sub.CheckedChanged += new System.EventHandler(this.cb_Bg_Sub_CheckedChanged);
            // 
            // btn_fit
            // 
            this.btn_fit.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_fit.Location = new System.Drawing.Point(608, 112);
            this.btn_fit.Name = "btn_fit";
            this.btn_fit.Size = new System.Drawing.Size(55, 47);
            this.btn_fit.TabIndex = 9;
            this.btn_fit.Text = "F I T";
            this.btn_fit.UseVisualStyleBackColor = true;
            this.btn_fit.Click += new System.EventHandler(this.btn_fit_Click);
            // 
            // cb_disc
            // 
            this.cb_disc.Appearance = System.Windows.Forms.Appearance.Button;
            this.cb_disc.AutoSize = true;
            this.cb_disc.Enabled = false;
            this.cb_disc.Location = new System.Drawing.Point(568, 136);
            this.cb_disc.Name = "cb_disc";
            this.cb_disc.Size = new System.Drawing.Size(36, 23);
            this.cb_disc.TabIndex = 11;
            this.cb_disc.Text = "disc";
            this.cb_disc.UseVisualStyleBackColor = true;
            this.cb_disc.CheckedChanged += new System.EventHandler(this.cb_disc_CheckedChanged);
            // 
            // comb_disc
            // 
            this.comb_disc.FormattingEnabled = true;
            this.comb_disc.Items.AddRange(new object[] {
            "20",
            "50",
            "100",
            "200",
            "400",
            "500",
            "1000"});
            this.comb_disc.Location = new System.Drawing.Point(557, 111);
            this.comb_disc.Name = "comb_disc";
            this.comb_disc.Size = new System.Drawing.Size(47, 21);
            this.comb_disc.TabIndex = 12;
            // 
            // tb_chi2
            // 
            this.tb_chi2.Location = new System.Drawing.Point(508, 112);
            this.tb_chi2.Name = "tb_chi2";
            this.tb_chi2.Size = new System.Drawing.Size(46, 20);
            this.tb_chi2.TabIndex = 13;
            // 
            // dgv_models_models
            // 
            this.dgv_models_models.HeaderText = "Fitmodel";
            this.dgv_models_models.Items.AddRange(new object[] {
            "Lorentz",
            "Gauss",
            "Gauss-Lorentz",
            "GLP",
            "Remove"});
            this.dgv_models_models.Name = "dgv_models_models";
            this.dgv_models_models.Width = 80;
            // 
            // dgv_models_amp
            // 
            this.dgv_models_amp.HeaderText = "Amplitude";
            this.dgv_models_amp.Name = "dgv_models_amp";
            this.dgv_models_amp.Width = 60;
            // 
            // dgv_models_cen
            // 
            this.dgv_models_cen.HeaderText = "Center";
            this.dgv_models_cen.Name = "dgv_models_cen";
            this.dgv_models_cen.Width = 60;
            // 
            // dgv_models_wid
            // 
            this.dgv_models_wid.HeaderText = "Sigma";
            this.dgv_models_wid.Name = "dgv_models_wid";
            this.dgv_models_wid.Width = 60;
            // 
            // dgv_models_mix
            // 
            this.dgv_models_mix.HeaderText = "Mixing";
            this.dgv_models_mix.Name = "dgv_models_mix";
            this.dgv_models_mix.Width = 60;
            // 
            // dgv_models_area
            // 
            this.dgv_models_area.HeaderText = "Area";
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
            this.dgv_models_s.HeaderText = "sig";
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 961);
            this.Controls.Add(this.tb_chi2);
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
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Click += new System.EventHandler(this.Form1_Click);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).EndInit();
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
        private System.Windows.Forms.TextBox tb_chi2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgv_bg_sel;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_bg_model;
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
    }
}

