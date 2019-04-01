namespace XPS
{
    partial class Analyser
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
            this.zgc_analyzer = new ZedGraph.ZedGraphControl();
            this.btn_ana_load_data = new System.Windows.Forms.Button();
            this.btn_quantiy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // zgc_analyzer
            // 
            this.zgc_analyzer.Location = new System.Drawing.Point(3, 68);
            this.zgc_analyzer.Name = "zgc_analyzer";
            this.zgc_analyzer.ScrollGrace = 0D;
            this.zgc_analyzer.ScrollMaxX = 0D;
            this.zgc_analyzer.ScrollMaxY = 0D;
            this.zgc_analyzer.ScrollMaxY2 = 0D;
            this.zgc_analyzer.ScrollMinX = 0D;
            this.zgc_analyzer.ScrollMinY = 0D;
            this.zgc_analyzer.ScrollMinY2 = 0D;
            this.zgc_analyzer.Size = new System.Drawing.Size(1527, 571);
            this.zgc_analyzer.TabIndex = 0;
            // 
            // btn_ana_load_data
            // 
            this.btn_ana_load_data.Location = new System.Drawing.Point(13, 13);
            this.btn_ana_load_data.Name = "btn_ana_load_data";
            this.btn_ana_load_data.Size = new System.Drawing.Size(75, 23);
            this.btn_ana_load_data.TabIndex = 1;
            this.btn_ana_load_data.Text = "Open file";
            this.btn_ana_load_data.UseVisualStyleBackColor = true;
            this.btn_ana_load_data.Click += new System.EventHandler(this.btn_ana_load_data_Click);
            // 
            // btn_quantiy
            // 
            this.btn_quantiy.Location = new System.Drawing.Point(116, 12);
            this.btn_quantiy.Name = "btn_quantiy";
            this.btn_quantiy.Size = new System.Drawing.Size(75, 23);
            this.btn_quantiy.TabIndex = 2;
            this.btn_quantiy.Text = "Quantify";
            this.btn_quantiy.UseVisualStyleBackColor = true;
            this.btn_quantiy.Click += new System.EventHandler(this.btn_quantiy_Click);
            // 
            // Analyser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1533, 641);
            this.Controls.Add(this.btn_quantiy);
            this.Controls.Add(this.btn_ana_load_data);
            this.Controls.Add(this.zgc_analyzer);
            this.Name = "Analyser";
            this.Text = "Analyser";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Analyser_FormClosing);
            this.Load += new System.EventHandler(this.Analyser_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zgc_analyzer;
        private System.Windows.Forms.Button btn_ana_load_data;
        private System.Windows.Forms.Button btn_quantiy;
    }
}