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
            this.SuspendLayout();
            // 
            // btn1_div
            // 
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
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
    }
}

