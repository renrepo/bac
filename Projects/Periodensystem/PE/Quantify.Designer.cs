namespace XPS
{
    partial class Quantify
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
            this.Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_col_center = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_col_sig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_col_amp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.dgv_col_center,
            this.dgv_col_sig,
            this.dgv_col_amp});
            this.dataGridView1.Location = new System.Drawing.Point(3, 93);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(674, 202);
            this.dataGridView1.TabIndex = 0;
            // 
            // Type
            // 
            this.Type.HeaderText = "Modeltype";
            this.Type.Items.AddRange(new object[] {
            "G",
            "L",
            "GLS",
            "GLP"});
            this.Type.Name = "Type";
            // 
            // dgv_col_center
            // 
            this.dgv_col_center.HeaderText = "Center";
            this.dgv_col_center.Name = "dgv_col_center";
            // 
            // dgv_col_sig
            // 
            this.dgv_col_sig.HeaderText = "Sigma";
            this.dgv_col_sig.Name = "dgv_col_sig";
            // 
            // dgv_col_amp
            // 
            this.dgv_col_amp.HeaderText = "Amplitude";
            this.dgv_col_amp.Name = "dgv_col_amp";
            // 
            // Quantiy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 344);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Quantiy";
            this.Text = "Quantify";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewComboBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_col_center;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_col_sig;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_col_amp;
    }
}