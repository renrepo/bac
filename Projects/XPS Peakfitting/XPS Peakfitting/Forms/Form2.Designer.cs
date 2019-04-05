namespace XPS_Peakfitting.Forms
{
    partial class Form2
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
            this.dgv_fit_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_fit_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_cen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_wid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_fit_mix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_bg = new System.Windows.Forms.DataGridView();
            this.btn_form2_test = new System.Windows.Forms.Button();
            this.dgv_bg_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_bg_start = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_bg_end = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).BeginInit();
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
            this.dgv_fit.Location = new System.Drawing.Point(2, 137);
            this.dgv_fit.Name = "dgv_fit";
            this.dgv_fit.Size = new System.Drawing.Size(580, 150);
            this.dgv_fit.TabIndex = 0;
            // 
            // dgv_fit_model
            // 
            this.dgv_fit_model.HeaderText = "Model";
            this.dgv_fit_model.Name = "dgv_fit_model";
            this.dgv_fit_model.Width = 80;
            // 
            // dgv_fit_amp
            // 
            this.dgv_fit_amp.HeaderText = "amplitude";
            this.dgv_fit_amp.Name = "dgv_fit_amp";
            this.dgv_fit_amp.Width = 80;
            // 
            // dgv_fit_cen
            // 
            this.dgv_fit_cen.HeaderText = "center";
            this.dgv_fit_cen.Name = "dgv_fit_cen";
            this.dgv_fit_cen.Width = 80;
            // 
            // dgv_fit_wid
            // 
            this.dgv_fit_wid.HeaderText = "sigma";
            this.dgv_fit_wid.Name = "dgv_fit_wid";
            this.dgv_fit_wid.Width = 80;
            // 
            // dgv_fit_mix
            // 
            this.dgv_fit_mix.HeaderText = "Mixing";
            this.dgv_fit_mix.Name = "dgv_fit_mix";
            this.dgv_fit_mix.Width = 80;
            // 
            // dgv_bg
            // 
            this.dgv_bg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_bg.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_bg_model,
            this.dgv_bg_start,
            this.dgv_bg_end});
            this.dgv_bg.Location = new System.Drawing.Point(2, 294);
            this.dgv_bg.Name = "dgv_bg";
            this.dgv_bg.Size = new System.Drawing.Size(580, 65);
            this.dgv_bg.TabIndex = 1;
            // 
            // btn_form2_test
            // 
            this.btn_form2_test.Location = new System.Drawing.Point(450, 53);
            this.btn_form2_test.Name = "btn_form2_test";
            this.btn_form2_test.Size = new System.Drawing.Size(75, 23);
            this.btn_form2_test.TabIndex = 2;
            this.btn_form2_test.Text = "button1";
            this.btn_form2_test.UseVisualStyleBackColor = true;
            this.btn_form2_test.Click += new System.EventHandler(this.btn_form2_test_Click);
            // 
            // dgv_bg_model
            // 
            this.dgv_bg_model.HeaderText = "Model";
            this.dgv_bg_model.Items.AddRange(new object[] {
            "Shirley",
            "Linear",
            "Touugrad",
            "Constant"});
            this.dgv_bg_model.Name = "dgv_bg_model";
            this.dgv_bg_model.Width = 80;
            // 
            // dgv_bg_start
            // 
            this.dgv_bg_start.HeaderText = "from";
            this.dgv_bg_start.Name = "dgv_bg_start";
            this.dgv_bg_start.Width = 80;
            // 
            // dgv_bg_end
            // 
            this.dgv_bg_end.HeaderText = "to";
            this.dgv_bg_end.Name = "dgv_bg_end";
            this.dgv_bg_end.Width = 80;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 361);
            this.Controls.Add(this.btn_form2_test);
            this.Controls.Add(this.dgv_bg);
            this.Controls.Add(this.dgv_fit);
            this.Name = "Form2";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.dgv_fit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_bg)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_fit;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_fit_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_cen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_wid;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_fit_mix;
        private System.Windows.Forms.DataGridView dgv_bg;
        private System.Windows.Forms.Button btn_form2_test;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_bg_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_bg_start;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_bg_end;
    }
}