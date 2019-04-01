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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dgv_cb_model = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_cen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_wid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_mix = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_fit = new System.Windows.Forms.Button();
            this.btn_shirley = new System.Windows.Forms.Button();
            this.tb_select_bg = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_cb_model,
            this.dgv_amp,
            this.dgv_cen,
            this.dgv_wid,
            this.dgv_mix});
            this.dataGridView1.Location = new System.Drawing.Point(3, 193);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(545, 254);
            this.dataGridView1.TabIndex = 0;
            // 
            // dgv_cb_model
            // 
            this.dgv_cb_model.HeaderText = "Model";
            this.dgv_cb_model.Items.AddRange(new object[] {
            "G",
            "L",
            "GLS",
            "GLP"});
            this.dgv_cb_model.Name = "dgv_cb_model";
            // 
            // dgv_amp
            // 
            this.dgv_amp.HeaderText = "Amplitude";
            this.dgv_amp.Name = "dgv_amp";
            // 
            // dgv_cen
            // 
            this.dgv_cen.HeaderText = "Center";
            this.dgv_cen.Name = "dgv_cen";
            // 
            // dgv_wid
            // 
            this.dgv_wid.HeaderText = "Width";
            this.dgv_wid.Name = "dgv_wid";
            // 
            // dgv_mix
            // 
            this.dgv_mix.HeaderText = "Mixing-Ratio";
            this.dgv_mix.Name = "dgv_mix";
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
            this.tb_select_bg.Location = new System.Drawing.Point(211, 10);
            this.tb_select_bg.Name = "tb_select_bg";
            this.tb_select_bg.Size = new System.Drawing.Size(75, 23);
            this.tb_select_bg.TabIndex = 3;
            this.tb_select_bg.Text = "Background";
            this.tb_select_bg.UseVisualStyleBackColor = true;
            this.tb_select_bg.Click += new System.EventHandler(this.tb_select_bg_Click);
            // 
            // Fitmodels
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 450);
            this.Controls.Add(this.tb_select_bg);
            this.Controls.Add(this.btn_shirley);
            this.Controls.Add(this.btn_fit);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Fitmodels";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Fitmodels_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_cb_model;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_amp;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_cen;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_wid;
        private System.Windows.Forms.Button btn_fit;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_mix;
        private System.Windows.Forms.Button btn_shirley;
        private System.Windows.Forms.Button tb_select_bg;
    }
}