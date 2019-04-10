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
            this.dgv_bg_from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_bg_to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models = new System.Windows.Forms.DataGridView();
            this.dgv_models_models = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_models_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_cen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_wid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_models_mix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cb_bg = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_models)).BeginInit();
            this.SuspendLayout();
            // 
            // tc_zgc
            // 
            this.tc_zgc.Location = new System.Drawing.Point(3, 161);
            this.tc_zgc.Name = "tc_zgc";
            this.tc_zgc.SelectedIndex = 0;
            this.tc_zgc.Size = new System.Drawing.Size(1580, 698);
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
            this.dgv_bg_model,
            this.dgv_bg_from,
            this.dgv_bg_to});
            this.dgv_bg.Enabled = false;
            this.dgv_bg.Location = new System.Drawing.Point(769, 13);
            this.dgv_bg.Name = "dgv_bg";
            this.dgv_bg.Size = new System.Drawing.Size(317, 119);
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
            this.dgv_bg_model.HeaderText = "Model";
            this.dgv_bg_model.Items.AddRange(new object[] {
            "None",
            "Shirley",
            "Linear",
            "Constant"});
            this.dgv_bg_model.Name = "dgv_bg_model";
            this.dgv_bg_model.Width = 80;
            // 
            // dgv_bg_from
            // 
            this.dgv_bg_from.HeaderText = "from";
            this.dgv_bg_from.Name = "dgv_bg_from";
            this.dgv_bg_from.Width = 80;
            // 
            // dgv_bg_to
            // 
            this.dgv_bg_to.HeaderText = "to";
            this.dgv_bg_to.Name = "dgv_bg_to";
            this.dgv_bg_to.Width = 80;
            // 
            // dgv_models
            // 
            this.dgv_models.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_models.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_models_models,
            this.dgv_models_amp,
            this.dgv_models_cen,
            this.dgv_models_wid,
            this.dgv_models_mix});
            this.dgv_models.Enabled = false;
            this.dgv_models.Location = new System.Drawing.Point(1092, 13);
            this.dgv_models.Name = "dgv_models";
            this.dgv_models.Size = new System.Drawing.Size(485, 142);
            this.dgv_models.TabIndex = 4;
            // 
            // dgv_models_models
            // 
            this.dgv_models_models.HeaderText = "Model";
            this.dgv_models_models.Items.AddRange(new object[] {
            "Lorentzian",
            "Gaussian",
            "Lorentz*Gauss",
            "Lorentz+Gauss"});
            this.dgv_models_models.Name = "dgv_models_models";
            this.dgv_models_models.Width = 80;
            // 
            // dgv_models_amp
            // 
            this.dgv_models_amp.HeaderText = "Amplitude";
            this.dgv_models_amp.Name = "dgv_models_amp";
            this.dgv_models_amp.Width = 80;
            // 
            // dgv_models_cen
            // 
            this.dgv_models_cen.HeaderText = "Center";
            this.dgv_models_cen.Name = "dgv_models_cen";
            this.dgv_models_cen.Width = 80;
            // 
            // dgv_models_wid
            // 
            this.dgv_models_wid.HeaderText = "Sigma";
            this.dgv_models_wid.Name = "dgv_models_wid";
            this.dgv_models_wid.Width = 80;
            // 
            // dgv_models_mix
            // 
            this.dgv_models_mix.HeaderText = "Mixing";
            this.dgv_models_mix.Name = "dgv_models_mix";
            this.dgv_models_mix.Width = 80;
            // 
            // cb_bg
            // 
            this.cb_bg.AutoSize = true;
            this.cb_bg.Enabled = false;
            this.cb_bg.Location = new System.Drawing.Point(769, 138);
            this.cb_bg.Name = "cb_bg";
            this.cb_bg.Size = new System.Drawing.Size(165, 17);
            this.cb_bg.TabIndex = 5;
            this.cb_bg.Text = "Enable Background selection";
            this.cb_bg.UseVisualStyleBackColor = true;
            this.cb_bg.CheckedChanged += new System.EventHandler(this.cb_bg_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.cb_bg);
            this.Controls.Add(this.dgv_models);
            this.Controls.Add(this.dgv_bg);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.tc_zgc);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_models_models;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_cen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_wid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_models_mix;
        private System.Windows.Forms.CheckBox cb_bg;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgv_bg_sel;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_bg_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_bg_from;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_bg_to;
    }
}

