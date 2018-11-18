using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace PE
{
 
    class HV_device
    {
        private string IP_adress;
        ManualResetEvent _suspend_background_measurement;
        Ivi.Visa.IMessageBasedSession session;

        public HV_device(string IP)
        {  //Parameterized constructor
            _suspend_background_measurement = new ManualResetEvent(true);
            IP_adress = IP;

            using (var rm = new ResourceManager())
            {
                try
                {
                    session = (IMessageBasedSession)rm.Open("TCPIP0::" + IP_adress + "::10001::SOCKET");
                    //Xray_HV_is_open = true;
                    // no timeout-error when reading back from Iseg-device after query (e.g. ":MEAS:VOLT? (@0)\n") was send 
                    //(if no query was send, a readback will take about 2000ms (default timeout) and give a "null"-result)
                    ((INativeVisaSession)session).SetAttributeBoolean(NativeVisaAttribute.SuppressEndEnabled, false);
                    //Iseg_DPS_session.Text = "Iseg Xray connected";
                    //c.BackColor = Color.LightGreen;
                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to Open Device at IP-adress " + IP_adress + " !");
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
        }

         public async Task<int> write_to_iseg(string command)
        {
            _suspend_background_measurement.Reset();
            session.RawIO.Write(String.Format(command));
            await Task.Delay(10);
            _suspend_background_measurement.Set();
            return 1;
        }

        public async Task<string> read_iseg(string command)
        {
            string reading = string.Empty;
            _suspend_background_measurement.Reset();
            session.RawIO.Write(String.Format(command));
            await Task.Delay(10);
            reading = session.RawIO.ReadString();
            _suspend_background_measurement.Set();
            return reading;
        }



        public async Task<int> reset_channels()
        {
            await write_to_iseg("*RST\n");
            return 1;
        }


        public async Task<int> channel_on(int channel)
        {
            await write_to_iseg(":VOLT ON,(@" + channel.ToString() + ")\n");
            return 1;
        }


        public async Task<int> channel_off(int channel)
        {
            await write_to_iseg(":VOLT OFF,(@" + channel.ToString() + ")\n");
            return 1;
        }


        public async Task<int> set_voltage(double voltage, int channel)
        {
            await write_to_iseg(":VOLT " + voltage.ToString() + ",(@" + channel.ToString() + ")\n");
            return 1;
        }


        public async Task<int> emergency_off()
        {
            await write_to_iseg(":VOLT EMCY OFF\n");
            return 1;
        }


        public async Task<int> voltage_ramp(double percent)
        {
            await write_to_iseg(":CONF:RAMP:VOLT " + percent.ToString() + "}%/s\n");
            return 1;
        }

        public async Task<int> check_dps()
        {
            await write_to_iseg("CONF:HVMICC HV_OK\n");
            return 1;
        }


        //resonable only for DPS
        public async Task<int> clear_emergency()
        {
            await write_to_iseg(":VOLT EMCY CLR,(@0-5)\n");
            return 1;
        }


        public async Task<string> read_voltage(int channel)
        {
            string reading = await read_iseg(":MEAS:VOLT? (@" + channel.ToString() + ")\n");
            return reading;
        }


        public async Task<int> set_K_P(double K_P)
        {
            await write_to_iseg(":CONF:FILA:EMI:P "+ K_P.ToString() + "\n");
            return 1;
        }


        public async Task<int> set_K_I(double K_I)
        {
            await write_to_iseg(":CONF:FILA:EMI:I " + K_I.ToString() + "\n");
            return 1;
        }


        public async Task<int> set_K_D(double K_D)
        {
            await write_to_iseg(":CONF:FILA:EMI:D " + K_D.ToString() + "\n");
            return 1;
        }


        public async Task<int> filament_current_min(double fil_curr_min)
        {
            await write_to_iseg(":CONF:FILA:CURR:MIN " + fil_curr_min.ToString() + "\n");
            return 1;
        }


        public async Task<int> filament_current_max(double fil_curr_max)
        {
            await write_to_iseg(":CONF:FILA:CURR:MAX " + fil_curr_max.ToString() + "\n");
            return 1;
        }



        public async Task<int> waiter()
        {
            await Task.Delay(10000);
            return 1;
        }


        public void raw_write_syn(string command)
        {
            session.RawIO.Write(command);
        }


        public string raw_read_syn()
        {
            return session.RawIO.ReadString();
        }


        public async Task<int> dispose()
        {
            await reset_channels();
            session.Dispose();
            return 1;
        }



        //public double getVolume()
        //{
        //    return length * breadth * height;
        //}
    }
}
