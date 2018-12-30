using System;
using System.Threading;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;
using System.Windows.Forms;





namespace XPS
{
    
    class HV_device
    {
        private string IP_adress;
        public bool Is_session_open { get; set; }
        ManualResetEvent _suspend_background_measurement = new ManualResetEvent(true);
        Ivi.Visa.IMessageBasedSession session;


        public HV_device()
        {  //Parameterized constructor
            //same as above but for the on/off switches in "Iseg ControL" tab
        }

        //~HV_device() { }
        
         
            

        public async Task<int> write_to_iseg(string command)
        {
            _suspend_background_measurement.Reset();
            session.RawIO.Write(String.Format(command));
            await Task.Delay(25);
            _suspend_background_measurement.Set();
            return 1;
        }

        public async Task<string> read_iseg(string command)
        {
            string reading = string.Empty;
            _suspend_background_measurement.Reset();
            session.RawIO.Write(String.Format(command));
            await Task.Delay(25);
            reading = session.RawIO.ReadString();
            await Task.Delay(25);
            _suspend_background_measurement.Set();
            return reading;
        }


        public async Task<int> open_session(string IP)
        {
            IP_adress = IP;
            using (var rm = new ResourceManager())
            {

               session = (IMessageBasedSession)rm.Open("TCPIP0::" + IP_adress + "::10001::SOCKET");
               ((INativeVisaSession)session).SetAttributeBoolean(NativeVisaAttribute.SuppressEndEnabled, false);
                await Task.Delay(50);
                Is_session_open = true;
                await clear();
                await reset_channels();
            }
            return 1;
        }


        public async Task<int> reset_channels()
        {
            await write_to_iseg("*RST\n");
            return 1;
        }


        public async Task<int> clear()
        {
            await write_to_iseg("*CLS\n");
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


        public async Task<int> set_current(double current, int channel)
        {
            await write_to_iseg(":CURR " + current.ToString() + ",(@" + channel.ToString() + ")\n");
            return 1;
        }


        public async Task<int> limit_current(double current)
        {
            await write_to_iseg(":CURR:LIM " + current.ToString() + "\n");
            return 1;
        }


        public async Task<int> emergency_off(int channel)
        {
            await write_to_iseg(":VOLT EMCY OFF,(@" + channel.ToString() + ")\n");
            return 1;
        }


        public async Task<int> voltage_ramp(double percent)
        {
            await write_to_iseg(":CONF:RAMP:VOLT " + percent.ToString() + "%/s\n");
            return 1;
        }


        public async Task<int> current_ramp(double percent)
        {
            await write_to_iseg(":CONF:RAMP:CURR " + percent.ToString() + "%/s\n");
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


        public async Task<string> read_current(int channel)
        {
            string reading = await read_iseg(":MEAS:CURR? (@" + channel.ToString() + ")\n");
            return reading;
        }


        public string read_arc()
        {
            //session.RawIO.Write(String.Format("*IDN?\n"));
            //Task.Delay(25);
            session.RawIO.Write(String.Format(":CONF:ARC:RAMP?\n"));
            string reading = session.RawIO.ReadString();
            return reading;
        }


        public async Task<int> select_cathode(double cathode)
        {
            await write_to_iseg(":CONFIGURE:OUTPUT " + cathode.ToString() + ", (@1)\n");
            return 1;
        }


        public async Task<int> set_K_P(double K_P)
        {
            await write_to_iseg(":CONF:FILA:EMI:P "+ K_P.ToString() + "\n");
            return 1;
        }


        public async Task<int> set_vnom(double voltage, int channel)
        {
            string answern = await read_iseg("*INSTR?\n");
            string answer0 = await read_iseg("*IDN?\n");
            await write_to_iseg("CLS\n");
            await write_to_iseg("CONF:HVMICC HV_NOT_OK\n");
            session.RawIO.Write(String.Format("CONF:HVMICC?\n"));
            string answer1 = session.RawIO.ReadString();
            await write_to_iseg("SYS:USER:CONF 5200048\n");
            string answer2 = await read_iseg(":READ:MOD:EV:STAT?\n");
            await write_to_iseg("SYS:USER:CONF:WRITE:VNOM 1000, (@1)\n");
            await write_to_iseg("SYS:USER:CALIB V,(@1)\n");
            await write_to_iseg("SLEEP 1000\n");
            string answer3 = await read_iseg("SYS:USER:READ:VNOM? (@1)\n");
            await write_to_iseg("SYS:RECA 1\n");
            await write_to_iseg("CONF:HVMICC HV_OK\n");
            //await write_to_iseg("STOP\n");
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


        public void raw_read_syn(int channel)
        {
            try
            {
                session.RawIO.Write(String.Format(":MEAS:VOLT? (@" + channel.ToString() + ")\n"));
            }
            catch (Exception)
            {
            }
        }


        public string raw_read_syn()
        {
            try
            {
                return session.RawIO.ReadString();
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }


        public async Task<int> dispose()
        {
            await reset_channels();
            session.Dispose();
            Is_session_open = false;
            return 1;
        }


        //public double getVolume()
        //{
        //    return length * breadth * height;
        //}
    }




}
