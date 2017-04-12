namespace multithread
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
            this.btn_fib = new System.Windows.Forms.Button();
            this.btn_fib_can = new System.Windows.Forms.Button();
            this.btn_gauss = new System.Windows.Forms.Button();
            this.btn_gauss_can = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.tb_fib = new System.Windows.Forms.TextBox();
            this.tb_gauss = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_tester = new System.Windows.Forms.Button();
            this.bW_fib = new System.ComponentModel.BackgroundWorker();
            this.bW_gauss = new System.ComponentModel.BackgroundWorker();
            this.lb_perc_gauss = new System.Windows.Forms.Label();
            this.btn_clear = new System.Windows.Forms.Button();
            this.tb_gauss_startvalue = new System.Windows.Forms.TextBox();
            this.tb_fib_startvalue = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lb_perc_fib = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_fib
            // 
            this.btn_fib.Enabled = false;
            this.btn_fib.Location = new System.Drawing.Point(13, 22);
            this.btn_fib.Name = "btn_fib";
            this.btn_fib.Size = new System.Drawing.Size(82, 23);
            this.btn_fib.TabIndex = 0;
            this.btn_fib.Text = "Fibonacci";
            this.btn_fib.UseVisualStyleBackColor = true;
            this.btn_fib.Click += new System.EventHandler(this.btn_fib_Click);
            // 
            // btn_fib_can
            // 
            this.btn_fib_can.Location = new System.Drawing.Point(13, 52);
            this.btn_fib_can.Name = "btn_fib_can";
            this.btn_fib_can.Size = new System.Drawing.Size(82, 23);
            this.btn_fib_can.TabIndex = 1;
            this.btn_fib_can.Text = "Cancel Fib";
            this.btn_fib_can.UseVisualStyleBackColor = true;
            this.btn_fib_can.Click += new System.EventHandler(this.btn_fib_can_Click);
            // 
            // btn_gauss
            // 
            this.btn_gauss.Enabled = false;
            this.btn_gauss.Location = new System.Drawing.Point(13, 82);
            this.btn_gauss.Name = "btn_gauss";
            this.btn_gauss.Size = new System.Drawing.Size(82, 23);
            this.btn_gauss.TabIndex = 2;
            this.btn_gauss.Text = "Gauß";
            this.btn_gauss.UseVisualStyleBackColor = true;
            this.btn_gauss.Click += new System.EventHandler(this.btn_gauss_Click);
            // 
            // btn_gauss_can
            // 
            this.btn_gauss_can.Location = new System.Drawing.Point(13, 112);
            this.btn_gauss_can.Name = "btn_gauss_can";
            this.btn_gauss_can.Size = new System.Drawing.Size(82, 23);
            this.btn_gauss_can.TabIndex = 3;
            this.btn_gauss_can.Text = "Cancel Gauß";
            this.btn_gauss_can.UseVisualStyleBackColor = true;
            this.btn_gauss_can.Click += new System.EventHandler(this.btn_gauss_can_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(113, 82);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(176, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(113, 22);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(176, 23);
            this.progressBar2.TabIndex = 5;
            // 
            // tb_fib
            // 
            this.tb_fib.Location = new System.Drawing.Point(189, 149);
            this.tb_fib.Name = "tb_fib";
            this.tb_fib.Size = new System.Drawing.Size(100, 20);
            this.tb_fib.TabIndex = 6;
            // 
            // tb_gauss
            // 
            this.tb_gauss.Location = new System.Drawing.Point(189, 206);
            this.tb_gauss.Name = "tb_gauss";
            this.tb_gauss.Size = new System.Drawing.Size(100, 20);
            this.tb_gauss.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Fibonacci";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Gauß";
            // 
            // btn_tester
            // 
            this.btn_tester.Location = new System.Drawing.Point(26, 206);
            this.btn_tester.Name = "btn_tester";
            this.btn_tester.Size = new System.Drawing.Size(55, 23);
            this.btn_tester.TabIndex = 10;
            this.btn_tester.Text = "Tester";
            this.btn_tester.UseVisualStyleBackColor = true;
            this.btn_tester.Click += new System.EventHandler(this.btn_tester_Click);
            // 
            // bW_fib
            // 
            this.bW_fib.WorkerReportsProgress = true;
            this.bW_fib.WorkerSupportsCancellation = true;
            this.bW_fib.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bW_fib_DoWork);
            this.bW_fib.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bW_fib_ProgressChanged);
            this.bW_fib.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bW_fib_RunWorkerCompleted);
            // 
            // bW_gauss
            // 
            this.bW_gauss.WorkerReportsProgress = true;
            this.bW_gauss.WorkerSupportsCancellation = true;
            this.bW_gauss.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bW_gauss_DoWork);
            this.bW_gauss.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bW_gauss_ProgressChanged);
            this.bW_gauss.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bW_gauss_RunWorkerCompleted);
            // 
            // lb_perc_gauss
            // 
            this.lb_perc_gauss.AutoSize = true;
            this.lb_perc_gauss.Location = new System.Drawing.Point(199, 52);
            this.lb_perc_gauss.Name = "lb_perc_gauss";
            this.lb_perc_gauss.Size = new System.Drawing.Size(0, 13);
            this.lb_perc_gauss.TabIndex = 11;
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(26, 146);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(54, 23);
            this.btn_clear.TabIndex = 12;
            this.btn_clear.Text = "clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // tb_gauss_startvalue
            // 
            this.tb_gauss_startvalue.Location = new System.Drawing.Point(106, 206);
            this.tb_gauss_startvalue.Name = "tb_gauss_startvalue";
            this.tb_gauss_startvalue.Size = new System.Drawing.Size(60, 20);
            this.tb_gauss_startvalue.TabIndex = 13;
            this.tb_gauss_startvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_gauss_startvalue.TextChanged += new System.EventHandler(this.tb_gauss_startvalue_TextChanged_1);
            // 
            // tb_fib_startvalue
            // 
            this.tb_fib_startvalue.Location = new System.Drawing.Point(106, 149);
            this.tb_fib_startvalue.Name = "tb_fib_startvalue";
            this.tb_fib_startvalue.Size = new System.Drawing.Size(60, 20);
            this.tb_fib_startvalue.TabIndex = 14;
            this.tb_fib_startvalue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tb_fib_startvalue.TextChanged += new System.EventHandler(this.tb_fib_startvalue_TextChanged_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(110, 133);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "start value";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(110, 187);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "start value";
            // 
            // lb_perc_fib
            // 
            this.lb_perc_fib.AutoSize = true;
            this.lb_perc_fib.Location = new System.Drawing.Point(189, 112);
            this.lb_perc_fib.Name = "lb_perc_fib";
            this.lb_perc_fib.Size = new System.Drawing.Size(0, 13);
            this.lb_perc_fib.TabIndex = 17;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(309, 261);
            this.Controls.Add(this.lb_perc_fib);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_fib_startvalue);
            this.Controls.Add(this.tb_gauss_startvalue);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.lb_perc_gauss);
            this.Controls.Add(this.btn_tester);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_gauss);
            this.Controls.Add(this.tb_fib);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_gauss_can);
            this.Controls.Add(this.btn_gauss);
            this.Controls.Add(this.btn_fib_can);
            this.Controls.Add(this.btn_fib);
            this.Name = "Form1";
            this.Text = "Multithreads";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_fib;
        private System.Windows.Forms.Button btn_fib_can;
        private System.Windows.Forms.Button btn_gauss;
        private System.Windows.Forms.Button btn_gauss_can;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.TextBox tb_fib;
        private System.Windows.Forms.TextBox tb_gauss;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_tester;
        private System.ComponentModel.BackgroundWorker bW_fib;
        private System.ComponentModel.BackgroundWorker bW_gauss;
        private System.Windows.Forms.Label lb_perc_gauss;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.TextBox tb_gauss_startvalue;
        private System.Windows.Forms.TextBox tb_fib_startvalue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lb_perc_fib;
    }
}

