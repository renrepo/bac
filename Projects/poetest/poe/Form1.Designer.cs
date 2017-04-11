namespace poe
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
            this.btn1_div = new System.Windows.Forms.Button();
            this.btn2_mult = new System.Windows.Forms.Button();
            this.tb1 = new System.Windows.Forms.TextBox();
            this.tb2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn3_sub = new System.Windows.Forms.Button();
            this.btn4_add = new System.Windows.Forms.Button();
            this.tb_erg = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_clear = new System.Windows.Forms.Button();
            this.btn_fib = new System.Windows.Forms.Button();
            this.btn_end_fib = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn1_div
            // 
            this.btn1_div.Enabled = false;
            this.btn1_div.Location = new System.Drawing.Point(34, 113);
            this.btn1_div.Name = "btn1_div";
            this.btn1_div.Size = new System.Drawing.Size(117, 23);
            this.btn1_div.TabIndex = 0;
            this.btn1_div.Text = "Division";
            this.btn1_div.UseVisualStyleBackColor = true;
            this.btn1_div.Click += new System.EventHandler(this.btn1_div_Click);
            // 
            // btn2_mult
            // 
            this.btn2_mult.Enabled = false;
            this.btn2_mult.Location = new System.Drawing.Point(34, 85);
            this.btn2_mult.Name = "btn2_mult";
            this.btn2_mult.Size = new System.Drawing.Size(117, 23);
            this.btn2_mult.TabIndex = 1;
            this.btn2_mult.Text = "Multiplikation";
            this.btn2_mult.UseVisualStyleBackColor = true;
            this.btn2_mult.Click += new System.EventHandler(this.btn2_mult_Click);
            // 
            // tb1
            // 
            this.tb1.Location = new System.Drawing.Point(34, 44);
            this.tb1.Name = "tb1";
            this.tb1.Size = new System.Drawing.Size(48, 20);
            this.tb1.TabIndex = 2;
            this.tb1.TextChanged += new System.EventHandler(this.tb1_TextChanged);
            // 
            // tb2
            // 
            this.tb2.Location = new System.Drawing.Point(100, 44);
            this.tb2.Name = "tb2";
            this.tb2.Size = new System.Drawing.Size(51, 20);
            this.tb2.TabIndex = 3;
            this.tb2.TextChanged += new System.EventHandler(this.tb2_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "a";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(97, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(13, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "b";
            // 
            // btn3_sub
            // 
            this.btn3_sub.Enabled = false;
            this.btn3_sub.Location = new System.Drawing.Point(34, 143);
            this.btn3_sub.Name = "btn3_sub";
            this.btn3_sub.Size = new System.Drawing.Size(117, 23);
            this.btn3_sub.TabIndex = 6;
            this.btn3_sub.Text = "Subtraktion";
            this.btn3_sub.UseVisualStyleBackColor = true;
            this.btn3_sub.Click += new System.EventHandler(this.btn3_sub_Click);
            // 
            // btn4_add
            // 
            this.btn4_add.Enabled = false;
            this.btn4_add.Location = new System.Drawing.Point(34, 173);
            this.btn4_add.Name = "btn4_add";
            this.btn4_add.Size = new System.Drawing.Size(117, 23);
            this.btn4_add.TabIndex = 7;
            this.btn4_add.Text = "Addition";
            this.btn4_add.UseVisualStyleBackColor = true;
            this.btn4_add.Click += new System.EventHandler(this.btn4_add_Click);
            // 
            // tb_erg
            // 
            this.tb_erg.Location = new System.Drawing.Point(172, 44);
            this.tb_erg.Name = "tb_erg";
            this.tb_erg.Size = new System.Drawing.Size(100, 20);
            this.tb_erg.TabIndex = 8;
            this.tb_erg.TextChanged += new System.EventHandler(this.tb_erg_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Ergebnis";
            // 
            // btn_clear
            // 
            this.btn_clear.Location = new System.Drawing.Point(172, 231);
            this.btn_clear.Name = "btn_clear";
            this.btn_clear.Size = new System.Drawing.Size(75, 23);
            this.btn_clear.TabIndex = 10;
            this.btn_clear.Text = "clear";
            this.btn_clear.UseVisualStyleBackColor = true;
            this.btn_clear.Click += new System.EventHandler(this.btn_clear_Click);
            // 
            // btn_fib
            // 
            this.btn_fib.Location = new System.Drawing.Point(172, 172);
            this.btn_fib.Name = "btn_fib";
            this.btn_fib.Size = new System.Drawing.Size(75, 23);
            this.btn_fib.TabIndex = 11;
            this.btn_fib.Text = "Fibonacci";
            this.btn_fib.UseVisualStyleBackColor = true;
            this.btn_fib.Click += new System.EventHandler(this.btn_fib_Click);
            // 
            // btn_end_fib
            // 
            this.btn_end_fib.Location = new System.Drawing.Point(172, 202);
            this.btn_end_fib.Name = "btn_end_fib";
            this.btn_end_fib.Size = new System.Drawing.Size(91, 23);
            this.btn_end_fib.TabIndex = 12;
            this.btn_end_fib.Text = "end Fibonacci";
            this.btn_end_fib.UseVisualStyleBackColor = true;
            this.btn_end_fib.Click += new System.EventHandler(this.btn_end_fib_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(34, 202);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(117, 23);
            this.progressBar1.TabIndex = 13;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 231);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "status";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_end_fib);
            this.Controls.Add(this.btn_fib);
            this.Controls.Add(this.btn_clear);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_erg);
            this.Controls.Add(this.btn4_add);
            this.Controls.Add(this.btn3_sub);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb2);
            this.Controls.Add(this.tb1);
            this.Controls.Add(this.btn2_mult);
            this.Controls.Add(this.btn1_div);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn1_div;
        private System.Windows.Forms.Button btn2_mult;
        private System.Windows.Forms.TextBox tb1;
        private System.Windows.Forms.TextBox tb2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn3_sub;
        private System.Windows.Forms.Button btn4_add;
        private System.Windows.Forms.TextBox tb_erg;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_clear;
        private System.Windows.Forms.Button btn_fib;
        private System.Windows.Forms.Button btn_end_fib;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label4;
    }
}

