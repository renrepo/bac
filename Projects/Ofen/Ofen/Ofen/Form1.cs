using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;


namespace Ofen
{
    public partial class Form1 : Form
    {
        // Definiere das array befehl_heizen. In dieser werden die einzelnen Profile ( von 1 - 10 ) abgespeichert
        string[] befehl_heizen = new string[10];
        // Das array display_heizen dient dazu, das Profil im Programm anzuzeigen (sonst keine Funktion)
        string[] display_heizen = new string[10];

        public Form1()
        {
            InitializeComponent();
            get_available_ports();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Beim Öffnen des Programms wird befehl_heizen zu einem "Nullbefehl" gemacht
            for (int i = 0; i <= 9; i++)
            {
                befehl_heizen[i] = "TP000000000"; // leerer Befehl für den Ofen
                display_heizen[i] = " "; // Hinter der Profilnummer steht nichts.
            }
            
        }

        // Die verfügbaren Ports werden angezeigt, ein Port kann ausgewählt werden.
        public void get_available_ports()
        {
            string[] portname = SerialPort.GetPortNames();
            port_name_selection.Items.AddRange(portname);
        }
        
        // Die Schnittstelle wird geöffnet und somit wird die Eingabe der Profile freigegeben
        private void port_oeffnen(object sender, MouseEventArgs e)
        {

            if(port_name_selection.Text == "")
            {
                MessageBox.Show("Bitte einen Port auswählen");
            }
            else
            {
                serial_port_ofen.PortName = port_name_selection.Text;
                serial_port_ofen.BaudRate = 2400;
                serial_port_ofen.Open();
                serial_port_ofen.NewLine = "\r\n";

               // serial_port_ofen.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

                port_open.Enabled = false;
                port_close.Enabled = true;

                // Nach dem Öffnen des Ports sind die Eingaben für die Profile verfügbar.
                
                temp_box.Enabled = true;
                zeit_box_h.Enabled = true;
                zeit_box_m.Enabled = true;
                zeit_box_s.Enabled = true;
                profil_nr.Enabled = true;
                button_start.Enabled = true; 
                abbruch.Enabled = true;
                //abfrage.Enabled = true;
                empty_full_profile.Enabled = true;
                eingabe.Enabled = true;
            }
        }
        /*
        private static void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serial_port_ofen = (SerialPort)sender;
            string indata = serial_port_ofen.ReadExisting(); 

            MessageBox.Show(indata);
            //Debug.Print("Data Received:");
            //Debug.Print(indata);
        } */

        private void port_schliessen(object sender, MouseEventArgs e)
        {
            serial_port_ofen.Close(); //Schließt die Schnittstelle
            port_open.Enabled = true;
            port_close.Enabled = false;

            temp_box.Enabled = false;
            zeit_box_h.Enabled = false;
            zeit_box_m.Enabled = false;
            zeit_box_s.Enabled = false;
            profil_nr.Enabled = false;
            button_start.Enabled = false; 
            abbruch.Enabled = false;
            //abfrage.Enabled = false;
            empty_full_profile.Enabled = false;
        }

        // Diese Funktion liest die Werte aus den Textfeldern aus, überprüft die Korrektheit der Eingaben
        // und erzeugt ein Profil für den Ofen
        private void eingabe_MouseClick(object sender, MouseEventArgs e)
        {
            string temperatur = "abc"; // Definiere Variable für Temperatur
            int pruef_temperatur = 0;  // Prüfvariable: wenn die Variable = 0 bleibt, wird kein Befehl für den Ofen erzeugt.

            // Prüfe, ob die eingegebene Temperatur zwischen 0 und 400 liegt. Falls nur ein oder zweistellige
            // Werte eingegeben werden, werden zwei oder eine Null hinzugefügt.
            if (Convert.ToInt16(temp_box.Text) <= 400)
            {
                if (temp_box.Text.Length == 3)
                {
                    temperatur = temp_box.Text;
                    pruef_temperatur = 1;
                }
                if (temp_box.Text.Length == 2)
                {
                    temperatur = "0" + temp_box.Text;
                    pruef_temperatur = 1;
                }
                if (temp_box.Text.Length == 1)
                {
                    temperatur = "00" + temp_box.Text;
                    pruef_temperatur = 1;
                }
            }
            else
            {
                MessageBox.Show("Temperatureingabe ungültig");
            }

            //----------------------------------------------------------------------

            // Nun dasselbe für die Zeit:
            string haltezeit_h = "ab";
            int pruef_zeit_h = 0;

            if (Convert.ToInt16(zeit_box_h.Text) <= 99)
            {
                if (zeit_box_h.Text.Length == 2)
                {
                    haltezeit_h = zeit_box_h.Text;
                    pruef_zeit_h = 1;
                }
                if (zeit_box_h.Text.Length == 1)
                {
                    haltezeit_h = "0" + zeit_box_h.Text;
                    pruef_zeit_h = 1;
                }
            }
            else
            {
                MessageBox.Show("Stundeneingabe ungültig");
            }

            //----------------------------------------------------------------------

            string haltezeit_m = "ab";
            int pruef_zeit_m = 0;

            if (Convert.ToInt16(zeit_box_m.Text) <= 59)
            {
                if (zeit_box_m.Text.Length == 2)
                {
                    haltezeit_m = zeit_box_m.Text;
                    pruef_zeit_m = 1;
                }
                if (zeit_box_m.Text.Length == 1)
                {
                    haltezeit_m = "0" + zeit_box_m.Text;
                    pruef_zeit_m = 1;
                }
            }
            else
            {
                MessageBox.Show("Minuteneingabe ungültig");
            }

            //----------------------------------------------------------------------

            string haltezeit_s = "ab";
            int pruef_zeit_s = 0;

            if (Convert.ToInt16(zeit_box_s.Text) <= 59)
            {
                if (zeit_box_s.Text.Length == 2)
                {
                    haltezeit_s = zeit_box_s.Text;
                    pruef_zeit_s = 1;
                }
                if (zeit_box_s.Text.Length == 1)
                {
                    haltezeit_s = "0" + zeit_box_s.Text;
                    pruef_zeit_s = 1;
                }
            }
            else
            {
                MessageBox.Show("Sekundeneingabe ungültig");
            }

            //----------------------------------------------------------------------

            // Einlesen der Profilnummer
            string profil = profil_nr.Text;
            int nummer = Convert.ToInt16(profil_nr.Text) - 1; // Schreibe Profil 1-10 in Profil 0-9 um (wichtig für string später)
            int pruef_profil = 0;

            // Kontrollvariable: ist die Profilnummer zwischen 0 und 9 (bzw. ist die Eingabe zwischen 1 und 10?) 
            if (nummer >= 0 && nummer <= 9)
            {
                pruef_profil = 1;
            }
            else
            {
                MessageBox.Show("Profil ungültig");
            }


            //----------------------------------------------------------------------

            // Zusammenfügen der Werte in einen String, der an den Ofen weitergegeben werden kann,

            //----------------------------------------------------------------------

            // befehl_heizen soll ein 10-elementiger string sein, wo alle 10 Temperaturprofile abgespeichert werden können. 

            // Falls alle pruef-Variablen 1 sind, d.h. alle Parameter im korrekten Wertebereich, wird ein Eintrag im Array mit
            // dem eingegebenen Profil beschrieben.
            if (pruef_temperatur == 1 && pruef_zeit_h == 1 && pruef_zeit_m == 1 && pruef_zeit_s == 1 && pruef_profil == 1)
            {
                befehl_heizen[nummer] = "TP" + temperatur + haltezeit_h + haltezeit_m + haltezeit_s;
                display_heizen[nummer] = temperatur + "°C, " + haltezeit_h + ":" + haltezeit_m + ":" + haltezeit_s;
               // MessageBox.Show(befehl_heizen[nummer]);
               // MessageBox.Show(display_heizen[nummer]);

            }

            profil_1.Text = "Profil 1: " + display_heizen[0];
            profil_2.Text = "Profil 2: " + display_heizen[1];
            profil_3.Text = "Profil 3: " + display_heizen[2];
            profil_4.Text = "Profil 4: " + display_heizen[3];
            profil_5.Text = "Profil 5: " + display_heizen[4];
            profil_6.Text = "Profil 6: " + display_heizen[5];
            profil_7.Text = "Profil 7: " + display_heizen[6];
            profil_8.Text = "Profil 8: " + display_heizen[7];
            profil_9.Text = "Profil 9: " + display_heizen[8];
            profil_10.Text = "Profil 10: " + display_heizen[9];

            // nun soll der finale Befehl, der an den Ofen gegeben wird, zusammengesetzt werden. Dazu werden die im array
            // befehl_heizen gespeicherten Profile hintereinandergefügt.
           /*  string heizen_final = " ";
             for (int i = 0; i <= 9; i++)
             {
                 heizen_final += befehl_heizen[i];
             }

            // MessageBox.Show(heizen_final); */

        }

        // Das Gesamtprofil wird an den Ofen gegeben. Zuvor wird geprüft, ob der Ofen noch ein Profil bearbeitet.
        private void start(object sender, MouseEventArgs e)
        {
            /* // Abfrage: Bearbeitet der Ofen noch ein Profil?
             string abfrage = "ST" + @"\" + "r" + @"\" + "n";
             MessageBox.Show(abfrage);
             serial_port_ofen.WriteLine(abfrage);
             string status = serial_port_ofen.ReadLine();
             int status_int = Convert.ToInt16(status[2].ToString());

             if(status_int == 1 || status_int == 2)
             {
                 MessageBox.Show("Es sind noch Profile in Bearbeitung. Bevor neue Profile gesendet werden, muss der vorige Heizvorgang beendet oder abgebrochen werden.");
             }
             if (status_int == 0)
             { 
                 // nun soll der finale Befehl, der an den Ofen gegeben wird, zusammengesetzt werden. Dazu werden die im array
                 // befehl_heizen gespeicherten Profile hintereinandergefügt.
                 string heizen_final = " ";
                 for (int i = 0; i <= 9; i++)
                 {
                     heizen_final += befehl_heizen[i];
                 }

            // byte[] MyMessage = System.Text.Encoding.UTF8.getBytes(heizen_final);
            // serial_port_ofen.Write(MyMessage, 0, MyMessage.Length);

            serial_port_ofen.Write(heizen_final);
            MessageBox.Show("Heizung startet");
             } */

            for (int i = 0; i <= 9; i++)
            {
                serial_port_ofen.WriteLine(befehl_heizen[i]);
               
            }

           // serial_port_ofen.WriteLine("TP100000030\r");
            MessageBox.Show("alles gesendet");
        }

        private void empty_all(object sender, MouseEventArgs e)
        {
            // Macht den Befehl Heizen zu einem Nullbefehl
            for (int i = 0; i <= 9; i++)
            {
                befehl_heizen[i] = "TP000000000"; // leerer Befehl für den Ofen
                display_heizen[i] = " "; // Hinter der Profilnummer steht nichts.

            }

            //Profilanzeige auch in der Programmansicht leeren
            profil_1.Text = "Profil 1: " + display_heizen[0];
            profil_2.Text = "Profil 2: " + display_heizen[1];
            profil_3.Text = "Profil 3: " + display_heizen[2];
            profil_4.Text = "Profil 4: " + display_heizen[3];
            profil_5.Text = "Profil 5: " + display_heizen[4];
            profil_6.Text = "Profil 6: " + display_heizen[5];
            profil_7.Text = "Profil 7: " + display_heizen[6];
            profil_8.Text = "Profil 8: " + display_heizen[7];
            profil_9.Text = "Profil 9: " + display_heizen[8];
            profil_10.Text = "Profil 10: " + display_heizen[9];
        }

        private void abbruch_MouseClick(object sender, MouseEventArgs e)
        {
            serial_port_ofen.WriteLine("NA");
            MessageBox.Show("Der Heizvorgang wurde abgebrochen");
        }
        
        private void statusabfrage(object sender, MouseEventArgs e)
        { 
        /*
            serial_port_ofen.WriteLine("ST");
            //MessageBox.Show("test");
            //Thread.Sleep(1000);
            //string status = serial_port_ofen.ReadExisting(); // .ReadLine();

            //string status = serial_port_ofen.ReadLine();
            //string status = serial_port_ofen.ReadTo("\r");
            //MessageBox.Show(status);
            //string status = serial_port_ofen.Read(, 0,);
            //serial_port_ofen.ReadTimeout = 500;
            //byte[] ein_byte;
            //string status = serial_port_ofen.Read(ein_byte,0,8).ToString;
            //MessageBox.Show(status);
            string status = "ab2defghig";
            label_temp.Text = "Temperatur: " + status[5].ToString() + status[6].ToString() + status[7].ToString();
            label_profilnr.Text = "Profil: " + status[3].ToString() + status[4].ToString();

            int status_int = Convert.ToInt16(status[2].ToString());

            if(status_int == 0)
            {
                label_status.Text = "Status: kein Profil";
            }
            if(status_int == 1)
            {
                label_status.Text = "Status: Kühlen/Heizen";
            }
            if(status_int == 2)
            {
                label_status.Text = "Status: konst. Temp.";
            }
        */
        }

        private void textBox4_TextChanged(object sender, EventArgs e) { }

        private void textBox2_TextChanged(object sender, EventArgs e) { }

        private void label1_Click(object sender, EventArgs e) { }

        private void label2_Click(object sender, EventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void label5_Click(object sender, EventArgs e) { }
 
        private void label4_Click(object sender, EventArgs e) { }

        private void label6_Click(object sender, EventArgs e) { }

        private void textBox1_TextChanged(object sender, EventArgs e){ }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e){ }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e){ }

        private void label8_Click(object sender, EventArgs e){ }

        private void label8_Click_1(object sender, EventArgs e){ }

        private void label8_Click_2(object sender, EventArgs e){ }

        private void label8_Click_3(object sender, EventArgs e){ }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void abfrage_Click(object sender, EventArgs e)
        {
            //serial_port_ofen.WriteLine("ST");
        }
    }
}
