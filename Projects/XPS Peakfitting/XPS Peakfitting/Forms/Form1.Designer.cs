namespace XPS_Peakfitting
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
            this.tc_plots = new System.Windows.Forms.TabControl();
            this.btn_open = new System.Windows.Forms.Button();
            this.btn_analyse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tc_plots
            // 
            this.tc_plots.Location = new System.Drawing.Point(0, 56);
            this.tc_plots.Name = "tc_plots";
            this.tc_plots.SelectedIndex = 0;
            this.tc_plots.Size = new System.Drawing.Size(1586, 806);
            this.tc_plots.TabIndex = 2;
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(12, 12);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(57, 37);
            this.btn_open.TabIndex = 3;
            this.btn_open.Text = "Open";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.btn_open_Click);
            // 
            // btn_analyse
            // 
            this.btn_analyse.Location = new System.Drawing.Point(76, 12);
            this.btn_analyse.Name = "btn_analyse";
            this.btn_analyse.Size = new System.Drawing.Size(58, 37);
            this.btn_analyse.TabIndex = 4;
            this.btn_analyse.Text = "Analyse";
            this.btn_analyse.UseVisualStyleBackColor = true;
            this.btn_analyse.Click += new System.EventHandler(this.btn_analyse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1584, 861);
            this.Controls.Add(this.btn_analyse);
            this.Controls.Add(this.btn_open);
            this.Controls.Add(this.tc_plots);
            this.Name = "Form1";
            this.Text = "XPS Peakfitting";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tc_plots;
        private System.Windows.Forms.Button btn_open;
        private System.Windows.Forms.Button btn_analyse;
    }
}

