namespace Ofen
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.abbruch = new System.Windows.Forms.Button();
            this.temp_box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.zeit_box_h = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.zeit_box_m = new System.Windows.Forms.TextBox();
            this.zeit_box_s = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.eingabe = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.profil_nr = new System.Windows.Forms.TextBox();
            this.button_start = new System.Windows.Forms.Button();
            this.profil_1 = new System.Windows.Forms.Label();
            this.profil_2 = new System.Windows.Forms.Label();
            this.profil_3 = new System.Windows.Forms.Label();
            this.profil_4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.profil_5 = new System.Windows.Forms.Label();
            this.profil_6 = new System.Windows.Forms.Label();
            this.profil_7 = new System.Windows.Forms.Label();
            this.profil_8 = new System.Windows.Forms.Label();
            this.profil_9 = new System.Windows.Forms.Label();
            this.profil_10 = new System.Windows.Forms.Label();
            this.gesamtprofil = new System.Windows.Forms.Label();
            this.serial_port_ofen = new System.IO.Ports.SerialPort(this.components);
            this.port_name_selection = new System.Windows.Forms.ComboBox();
            this.port_name = new System.Windows.Forms.Label();
            this.port_open = new System.Windows.Forms.Button();
            this.port_close = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label_temp = new System.Windows.Forms.Label();
            this.label_status = new System.Windows.Forms.Label();
            this.label_profilnr = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.abfrage = new System.Windows.Forms.Button();
            this.empty_full_profile = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // abbruch
            // 
            this.abbruch.BackColor = System.Drawing.Color.Red;
            this.abbruch.Cursor = System.Windows.Forms.Cursors.Hand;
            this.abbruch.Enabled = false;
            this.abbruch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.abbruch.Location = new System.Drawing.Point(346, 355);
            this.abbruch.Name = "abbruch";
            this.abbruch.Size = new System.Drawing.Size(121, 50);
            this.abbruch.TabIndex = 10;
            this.abbruch.Text = "Abbruch";
            this.abbruch.UseVisualStyleBackColor = false;
            this.abbruch.MouseClick += new System.Windows.Forms.MouseEventHandler(this.abbruch_MouseClick);
            // 
            // temp_box
            // 
            this.temp_box.Enabled = false;
            this.temp_box.Location = new System.Drawing.Point(43, 93);
            this.temp_box.Name = "temp_box";
            this.temp_box.Size = new System.Drawing.Size(100, 20);
            this.temp_box.TabIndex = 0;
            this.temp_box.Text = "000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(35, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Temperatur";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // zeit_box_h
            // 
            this.zeit_box_h.Enabled = false;
            this.zeit_box_h.Location = new System.Drawing.Point(43, 179);
            this.zeit_box_h.Name = "zeit_box_h";
            this.zeit_box_h.Size = new System.Drawing.Size(100, 20);
            this.zeit_box_h.TabIndex = 2;
            this.zeit_box_h.Text = "00";
            this.zeit_box_h.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Control;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(40, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Haltezeit";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 163);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Stunden (00-99)";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 202);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Minuten (00-59)";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 241);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Sekunden (00-59)";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // zeit_box_m
            // 
            this.zeit_box_m.Enabled = false;
            this.zeit_box_m.Location = new System.Drawing.Point(43, 218);
            this.zeit_box_m.Name = "zeit_box_m";
            this.zeit_box_m.Size = new System.Drawing.Size(100, 20);
            this.zeit_box_m.TabIndex = 7;
            this.zeit_box_m.Text = "00";
            // 
            // zeit_box_s
            // 
            this.zeit_box_s.Enabled = false;
            this.zeit_box_s.Location = new System.Drawing.Point(43, 257);
            this.zeit_box_s.Name = "zeit_box_s";
            this.zeit_box_s.Size = new System.Drawing.Size(100, 20);
            this.zeit_box_s.TabIndex = 8;
            this.zeit_box_s.Text = "00";
            this.zeit_box_s.TextChanged += new System.EventHandler(this.textBox4_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "in °C (000-400)";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // eingabe
            // 
            this.eingabe.Cursor = System.Windows.Forms.Cursors.Hand;
            this.eingabe.Enabled = false;
            this.eingabe.Location = new System.Drawing.Point(43, 323);
            this.eingabe.Name = "eingabe";
            this.eingabe.Size = new System.Drawing.Size(100, 33);
            this.eingabe.TabIndex = 11;
            this.eingabe.Text = "Profil hinzufügen";
            this.eingabe.UseVisualStyleBackColor = true;
            this.eingabe.MouseClick += new System.Windows.Forms.MouseEventHandler(this.eingabe_MouseClick);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(40, 291);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Profil Nr. (1-10)";
            // 
            // profil_nr
            // 
            this.profil_nr.Enabled = false;
            this.profil_nr.Location = new System.Drawing.Point(123, 288);
            this.profil_nr.Name = "profil_nr";
            this.profil_nr.Size = new System.Drawing.Size(20, 20);
            this.profil_nr.TabIndex = 13;
            this.profil_nr.Text = "1";
            this.profil_nr.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.profil_nr.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button_start
            // 
            this.button_start.Enabled = false;
            this.button_start.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button_start.Location = new System.Drawing.Point(219, 355);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(121, 50);
            this.button_start.TabIndex = 15;
            this.button_start.Text = "Start";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.MouseClick += new System.Windows.Forms.MouseEventHandler(this.start);
            // 
            // profil_1
            // 
            this.profil_1.AutoSize = true;
            this.profil_1.Location = new System.Drawing.Point(3, 0);
            this.profil_1.Name = "profil_1";
            this.profil_1.Size = new System.Drawing.Size(42, 13);
            this.profil_1.TabIndex = 16;
            this.profil_1.Text = "Profil 1:";
            this.profil_1.Click += new System.EventHandler(this.label8_Click_1);
            // 
            // profil_2
            // 
            this.profil_2.AutoSize = true;
            this.profil_2.Location = new System.Drawing.Point(3, 26);
            this.profil_2.Name = "profil_2";
            this.profil_2.Size = new System.Drawing.Size(42, 13);
            this.profil_2.TabIndex = 17;
            this.profil_2.Text = "Profil 2:";
            // 
            // profil_3
            // 
            this.profil_3.AutoSize = true;
            this.profil_3.Location = new System.Drawing.Point(3, 52);
            this.profil_3.Name = "profil_3";
            this.profil_3.Size = new System.Drawing.Size(42, 13);
            this.profil_3.TabIndex = 18;
            this.profil_3.Text = "Profil 3:";
            // 
            // profil_4
            // 
            this.profil_4.AutoSize = true;
            this.profil_4.Location = new System.Drawing.Point(3, 78);
            this.profil_4.Name = "profil_4";
            this.profil_4.Size = new System.Drawing.Size(42, 13);
            this.profil_4.TabIndex = 19;
            this.profil_4.Text = "Profil 4:";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.profil_1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.profil_4, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.profil_2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.profil_3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.profil_5, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.profil_6, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.profil_7, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.profil_8, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.profil_9, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.profil_10, 0, 9);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(219, 77);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(248, 262);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // profil_5
            // 
            this.profil_5.AutoSize = true;
            this.profil_5.Location = new System.Drawing.Point(3, 104);
            this.profil_5.Name = "profil_5";
            this.profil_5.Size = new System.Drawing.Size(42, 13);
            this.profil_5.TabIndex = 20;
            this.profil_5.Text = "Profil 5:";
            // 
            // profil_6
            // 
            this.profil_6.AutoSize = true;
            this.profil_6.Location = new System.Drawing.Point(3, 130);
            this.profil_6.Name = "profil_6";
            this.profil_6.Size = new System.Drawing.Size(42, 13);
            this.profil_6.TabIndex = 21;
            this.profil_6.Text = "Profil 6:";
            // 
            // profil_7
            // 
            this.profil_7.AutoSize = true;
            this.profil_7.Location = new System.Drawing.Point(3, 156);
            this.profil_7.Name = "profil_7";
            this.profil_7.Size = new System.Drawing.Size(42, 13);
            this.profil_7.TabIndex = 22;
            this.profil_7.Text = "Profil 7:";
            // 
            // profil_8
            // 
            this.profil_8.AutoSize = true;
            this.profil_8.Location = new System.Drawing.Point(3, 182);
            this.profil_8.Name = "profil_8";
            this.profil_8.Size = new System.Drawing.Size(42, 13);
            this.profil_8.TabIndex = 23;
            this.profil_8.Text = "Profil 8:";
            // 
            // profil_9
            // 
            this.profil_9.AutoSize = true;
            this.profil_9.Location = new System.Drawing.Point(3, 208);
            this.profil_9.Name = "profil_9";
            this.profil_9.Size = new System.Drawing.Size(42, 13);
            this.profil_9.TabIndex = 24;
            this.profil_9.Text = "Profil 9:";
            // 
            // profil_10
            // 
            this.profil_10.AutoSize = true;
            this.profil_10.Location = new System.Drawing.Point(3, 234);
            this.profil_10.Name = "profil_10";
            this.profil_10.Size = new System.Drawing.Size(48, 13);
            this.profil_10.TabIndex = 25;
            this.profil_10.Text = "Profil 10:";
            // 
            // gesamtprofil
            // 
            this.gesamtprofil.AutoSize = true;
            this.gesamtprofil.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gesamtprofil.Location = new System.Drawing.Point(216, 49);
            this.gesamtprofil.Name = "gesamtprofil";
            this.gesamtprofil.Size = new System.Drawing.Size(96, 16);
            this.gesamtprofil.TabIndex = 21;
            this.gesamtprofil.Text = "Gesamtprofil";
            this.gesamtprofil.Click += new System.EventHandler(this.label8_Click_2);
            // 
            // serial_port_ofen
            // 
            this.serial_port_ofen.BaudRate = 2400;
            // 
            // port_name_selection
            // 
            this.port_name_selection.FormattingEnabled = true;
            this.port_name_selection.Location = new System.Drawing.Point(535, 74);
            this.port_name_selection.Name = "port_name_selection";
            this.port_name_selection.Size = new System.Drawing.Size(121, 21);
            this.port_name_selection.TabIndex = 22;
            // 
            // port_name
            // 
            this.port_name.AutoSize = true;
            this.port_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.port_name.Location = new System.Drawing.Point(532, 49);
            this.port_name.Name = "port_name";
            this.port_name.Size = new System.Drawing.Size(117, 16);
            this.port_name.TabIndex = 23;
            this.port_name.Text = "Port auswählen:";
            this.port_name.Click += new System.EventHandler(this.label8_Click_3);
            // 
            // port_open
            // 
            this.port_open.Location = new System.Drawing.Point(535, 101);
            this.port_open.Name = "port_open";
            this.port_open.Size = new System.Drawing.Size(61, 36);
            this.port_open.TabIndex = 24;
            this.port_open.Text = "Port öffnen";
            this.port_open.UseVisualStyleBackColor = true;
            this.port_open.MouseClick += new System.Windows.Forms.MouseEventHandler(this.port_oeffnen);
            // 
            // port_close
            // 
            this.port_close.Enabled = false;
            this.port_close.Location = new System.Drawing.Point(595, 101);
            this.port_close.Name = "port_close";
            this.port_close.Size = new System.Drawing.Size(61, 36);
            this.port_close.TabIndex = 25;
            this.port_close.Text = "Port schließen";
            this.port_close.UseVisualStyleBackColor = true;
            this.port_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.port_schliessen);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label8.Location = new System.Drawing.Point(532, 160);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "Statusabfrage";
            // 
            // label_temp
            // 
            this.label_temp.AutoSize = true;
            this.label_temp.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label_temp.Location = new System.Drawing.Point(3, 24);
            this.label_temp.Name = "label_temp";
            this.label_temp.Size = new System.Drawing.Size(64, 13);
            this.label_temp.TabIndex = 27;
            this.label_temp.Text = "Temperatur:";
            // 
            // label_status
            // 
            this.label_status.AutoSize = true;
            this.label_status.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label_status.Location = new System.Drawing.Point(3, 48);
            this.label_status.Name = "label_status";
            this.label_status.Size = new System.Drawing.Size(40, 13);
            this.label_status.TabIndex = 28;
            this.label_status.Text = "Status:";
            // 
            // label_profilnr
            // 
            this.label_profilnr.AutoSize = true;
            this.label_profilnr.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label_profilnr.Location = new System.Drawing.Point(3, 0);
            this.label_profilnr.Name = "label_profilnr";
            this.label_profilnr.Size = new System.Drawing.Size(33, 13);
            this.label_profilnr.TabIndex = 29;
            this.label_profilnr.Text = "Profil:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.label_profilnr, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label_temp, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label_status, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(535, 198);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(130, 74);
            this.tableLayoutPanel2.TabIndex = 30;
            // 
            // abfrage
            // 
            this.abfrage.Enabled = false;
            this.abfrage.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.abfrage.Location = new System.Drawing.Point(535, 285);
            this.abfrage.Name = "abfrage";
            this.abfrage.Size = new System.Drawing.Size(130, 39);
            this.abfrage.TabIndex = 31;
            this.abfrage.Text = "Abfrage";
            this.abfrage.UseVisualStyleBackColor = true;
            this.abfrage.Click += new System.EventHandler(this.abfrage_Click);
            // 
            // empty_full_profile
            // 
            this.empty_full_profile.Enabled = false;
            this.empty_full_profile.Location = new System.Drawing.Point(43, 362);
            this.empty_full_profile.Name = "empty_full_profile";
            this.empty_full_profile.Size = new System.Drawing.Size(100, 43);
            this.empty_full_profile.TabIndex = 32;
            this.empty_full_profile.Text = "Gesamtprofil leeren";
            this.empty_full_profile.UseVisualStyleBackColor = true;
            this.empty_full_profile.MouseClick += new System.Windows.Forms.MouseEventHandler(this.empty_all);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(759, 442);
            this.Controls.Add(this.empty_full_profile);
            this.Controls.Add(this.abfrage);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.port_close);
            this.Controls.Add(this.port_open);
            this.Controls.Add(this.port_name);
            this.Controls.Add(this.port_name_selection);
            this.Controls.Add(this.gesamtprofil);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.profil_nr);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.eingabe);
            this.Controls.Add(this.abbruch);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.zeit_box_s);
            this.Controls.Add(this.zeit_box_m);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.zeit_box_h);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.temp_box);
            this.Name = "Form1";
            this.Text = "TLD-Ofen";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button abbruch;
        private System.Windows.Forms.TextBox temp_box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox zeit_box_h;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox zeit_box_m;
        private System.Windows.Forms.TextBox zeit_box_s;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button eingabe;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox profil_nr;
        private System.Windows.Forms.Button button_start;
        private System.Windows.Forms.Label profil_1;
        private System.Windows.Forms.Label profil_2;
        private System.Windows.Forms.Label profil_3;
        private System.Windows.Forms.Label profil_4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label profil_5;
        private System.Windows.Forms.Label profil_6;
        private System.Windows.Forms.Label profil_7;
        private System.Windows.Forms.Label profil_8;
        private System.Windows.Forms.Label profil_9;
        private System.Windows.Forms.Label profil_10;
        private System.Windows.Forms.Label gesamtprofil;
        private System.IO.Ports.SerialPort serial_port_ofen;
        private System.Windows.Forms.ComboBox port_name_selection;
        private System.Windows.Forms.Label port_name;
        private System.Windows.Forms.Button port_open;
        private System.Windows.Forms.Button port_close;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label_temp;
        private System.Windows.Forms.Label label_status;
        private System.Windows.Forms.Label label_profilnr;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button abfrage;
        private System.Windows.Forms.Button empty_full_profile;
    }
}

