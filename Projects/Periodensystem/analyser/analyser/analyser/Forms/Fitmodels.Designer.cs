namespace analyser.Forms
{
    partial class Fitmodels
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
            this.dgv_fit = new System.Windows.Forms.DataGridView();
            this.btn_fit = new System.Windows.Forms.Button();
            this.btn_shirley = new System.Windows.Forms.Button();
            this.tb_select_bg = new System.Windows.Forms.Button();
            this.lb_filename = new System.Windows.Forms.Label();
            this.tc_processing = new System.Windows.Forms.TabControl();
            this.tb_fit = new System.Windows.Forms.TabPage();
            this.dgv_background = new System.Windows.Forms.DataGridView();
            this.dgv_background_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_background_from = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_background_to = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_fit_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_cen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_wid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_mix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fit)).BeginInit();
            this.tc_processing.SuspendLayout();
            this.tb_fit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_background)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_fit
            // 
            this.dgv_fit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_fit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_fit_model,
            this.dgv_fit_amp,
            this.dgv_fit_cen,
            this.dgv_fit_wid,
            this.dgv_fit_mix});
            this.dgv_fit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_fit.Location = new System.Drawing.Point(3, 3);
            this.dgv_fit.Name = "dgv_fit";
            this.dgv_fit.Size = new System.Drawing.Size(537, 159);
            this.dgv_fit.TabIndex = 0;
            // 
            // btn_fit
            // 
            this.btn_fit.Location = new System.Drawing.Point(12, 12);
            this.btn_fit.Name = "btn_fit";
            this.btn_fit.Size = new System.Drawing.Size(75, 23);
            this.btn_fit.TabIndex = 1;
            this.btn_fit.Text = "Process Fit";
            this.btn_fit.UseVisualStyleBackColor = true;
            // 
            // btn_shirley
            // 
            this.btn_shirley.Location = new System.Drawing.Point(103, 11);
            this.btn_shirley.Name = "btn_shirley";
            this.btn_shirley.Size = new System.Drawing.Size(75, 23);
            this.btn_shirley.TabIndex = 2;
            this.btn_shirley.Text = "Shirley";
            this.btn_shirley.UseVisualStyleBackColor = true;
            this.btn_shirley.Click += new System.EventHandler(this.btn_shirley_Click);
            // 
            // tb_select_bg
            // 
            this.tb_select_bg.Location = new System.Drawing.Point(184, 11);
            this.tb_select_bg.Name = "tb_select_bg";
            this.tb_select_bg.Size = new System.Drawing.Size(75, 23);
            this.tb_select_bg.TabIndex = 3;
            this.tb_select_bg.Text = "Background";
            this.tb_select_bg.UseVisualStyleBackColor = true;
            this.tb_select_bg.Click += new System.EventHandler(this.tb_select_bg_Click);
            // 
            // lb_filename
            // 
            this.lb_filename.AutoSize = true;
            this.lb_filename.Location = new System.Drawing.Point(293, 13);
            this.lb_filename.Name = "lb_filename";
            this.lb_filename.Size = new System.Drawing.Size(21, 13);
            this.lb_filename.TabIndex = 4;
            this.lb_filename.Text = "bla";
            // 
            // tc_processing
            // 
            this.tc_processing.Controls.Add(this.tb_fit);
            this.tc_processing.Location = new System.Drawing.Point(3, 150);
            this.tc_processing.Name = "tc_processing";
            this.tc_processing.SelectedIndex = 0;
            this.tc_processing.Size = new System.Drawing.Size(551, 191);
            this.tc_processing.TabIndex = 5;
            // 
            // tb_fit
            // 
            this.tb_fit.Controls.Add(this.dgv_fit);
            this.tb_fit.Location = new System.Drawing.Point(4, 22);
            this.tb_fit.Name = "tb_fit";
            this.tb_fit.Padding = new System.Windows.Forms.Padding(3);
            this.tb_fit.Size = new System.Drawing.Size(543, 165);
            this.tb_fit.TabIndex = 1;
            this.tb_fit.Text = "Fit";
            this.tb_fit.UseVisualStyleBackColor = true;
            // 
            // dgv_background
            // 
            this.dgv_background.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_background.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_background_model,
            this.dgv_background_from,
            this.dgv_background_to,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgv_background.Location = new System.Drawing.Point(3, 345);
            this.dgv_background.Name = "dgv_background";
            this.dgv_background.Size = new System.Drawing.Size(547, 99);
            this.dgv_background.TabIndex = 1;
            // 
            // dgv_background_model
            // 
            this.dgv_background_model.HeaderText = "Model";
            this.dgv_background_model.Items.AddRange(new object[] {
            "Shirley",
            "Linear"});
            this.dgv_background_model.Name = "dgv_background_model";
            // 
            // dgv_background_from
            // 
            this.dgv_background_from.HeaderText = "From";
            this.dgv_background_from.Name = "dgv_background_from";
            // 
            // dgv_background_to
            // 
            this.dgv_background_to.HeaderText = "To";
            this.dgv_background_to.Name = "dgv_background_to";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "---";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "----";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dgv_fit_model
            // 
            this.dgv_fit_model.HeaderText = "Model";
            this.dgv_fit_model.Items.AddRange(new object[] {
            "G",
            "L",
            "GLS",
            "GLP"});
            this.dgv_fit_model.Name = "dgv_fit_model";
            // 
            // dgv_fit_amp
            // 
            this.dgv_fit_amp.HeaderText = "Amplitude";
            this.dgv_fit_amp.Name = "dgv_fit_amp";
            // 
            // dgv_fit_cen
            // 
            this.dgv_fit_cen.HeaderText = "Center";
            this.dgv_fit_cen.Name = "dgv_fit_cen";
            // 
            // dgv_fit_wid
            // 
            this.dgv_fit_wid.HeaderText = "Width";
            this.dgv_fit_wid.Name = "dgv_fit_wid";
            // 
            // dgv_fit_mix
            // 
            this.dgv_fit_mix.HeaderText = "Mixing-Ratio";
            this.dgv_fit_mix.Name = "dgv_fit_mix";
            // 
            // Fitmodels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 450);
            this.Controls.Add(this.dgv_background);
            this.Controls.Add(this.tc_processing);
            this.Controls.Add(this.lb_filename);
            this.Controls.Add(this.tb_select_bg);
            this.Controls.Add(this.btn_shirley);
            this.Controls.Add(this.btn_fit);
            this.Name = "Fitmodels";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Fitmodels_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fit)).EndInit();
            this.tc_processing.ResumeLayout(false);
            this.tb_fit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_background)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_fit;
        private System.Windows.Forms.Button btn_fit;
        private System.Windows.Forms.Button btn_shirley;
        private System.Windows.Forms.Button tb_select_bg;
        private System.Windows.Forms.Label lb_filename;
        private System.Windows.Forms.TabControl tc_processing;
        private System.Windows.Forms.TabPage tb_fit;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_fit_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_cen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_wid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_mix;
        private System.Windows.Forms.DataGridView dgv_background;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_background_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_background_from;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_background_to;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}