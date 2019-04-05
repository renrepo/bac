namespace analyser
{
    partial class Form_Main
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
            this.btn_load_data = new System.Windows.Forms.Button();
            this.btn_processing = new System.Windows.Forms.Button();
            this.tc_plots = new System.Windows.Forms.TabControl();
            this.btn_del = new System.Windows.Forms.Button();
            this.btn_bg = new System.Windows.Forms.Button();
            this.btn_bg_add = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_load_data
            // 
            this.btn_load_data.Location = new System.Drawing.Point(12, 13);
            this.btn_load_data.Name = "btn_load_data";
            this.btn_load_data.Size = new System.Drawing.Size(66, 34);
            this.btn_load_data.TabIndex = 1;
            this.btn_load_data.Text = "Load Data";
            this.btn_load_data.UseVisualStyleBackColor = true;
            this.btn_load_data.Click += new System.EventHandler(this.btn_load_data_Click);
            // 
            // btn_processing
            // 
            this.btn_processing.Location = new System.Drawing.Point(84, 13);
            this.btn_processing.Name = "btn_processing";
            this.btn_processing.Size = new System.Drawing.Size(68, 34);
            this.btn_processing.TabIndex = 2;
            this.btn_processing.Text = "Processing";
            this.btn_processing.UseVisualStyleBackColor = true;
            this.btn_processing.Click += new System.EventHandler(this.btn_processing_Click);
            // 
            // tc_plots
            // 
            this.tc_plots.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tc_plots.Location = new System.Drawing.Point(4, 159);
            this.tc_plots.Name = "tc_plots";
            this.tc_plots.SelectedIndex = 0;
            this.tc_plots.Size = new System.Drawing.Size(1594, 738);
            this.tc_plots.TabIndex = 3;
            this.tc_plots.SelectedIndexChanged += new System.EventHandler(this.tc_plots_SelectedIndexChanged);
            // 
            // btn_del
            // 
            this.btn_del.Location = new System.Drawing.Point(158, 13);
            this.btn_del.Name = "btn_del";
            this.btn_del.Size = new System.Drawing.Size(75, 34);
            this.btn_del.TabIndex = 4;
            this.btn_del.Text = "Close Tab";
            this.btn_del.UseVisualStyleBackColor = true;
            this.btn_del.Click += new System.EventHandler(this.btn_del_Click);
            // 
            // btn_bg
            // 
            this.btn_bg.Location = new System.Drawing.Point(240, 13);
            this.btn_bg.Name = "btn_bg";
            this.btn_bg.Size = new System.Drawing.Size(75, 34);
            this.btn_bg.TabIndex = 6;
            this.btn_bg.Text = "BG";
            this.btn_bg.UseVisualStyleBackColor = true;
            this.btn_bg.Click += new System.EventHandler(this.btn_bg_Click);
            // 
            // btn_bg_add
            // 
            this.btn_bg_add.Location = new System.Drawing.Point(322, 13);
            this.btn_bg_add.Name = "btn_bg_add";
            this.btn_bg_add.Size = new System.Drawing.Size(75, 34);
            this.btn_bg_add.TabIndex = 7;
            this.btn_bg_add.Text = "Add BG";
            this.btn_bg_add.UseVisualStyleBackColor = true;
            this.btn_bg_add.Click += new System.EventHandler(this.btn_bg_add_Click);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1600, 900);
            this.Controls.Add(this.btn_bg_add);
            this.Controls.Add(this.btn_bg);
            this.Controls.Add(this.btn_del);
            this.Controls.Add(this.tc_plots);
            this.Controls.Add(this.btn_processing);
            this.Controls.Add(this.btn_load_data);
            this.Name = "Form_Main";
            this.Text = "Analyser";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_load_data;
        private System.Windows.Forms.Button btn_processing;
        public System.Windows.Forms.TabControl tc_plots;
        private System.Windows.Forms.Button btn_del;
        private System.Windows.Forms.Button btn_bg;
        private System.Windows.Forms.Button btn_bg_add;
    }
}

