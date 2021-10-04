using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing.Drawing2D;
using System.Diagnostics;
using System.IO;
using SensorScope;



namespace Sensor_Scope
{
    public partial class FRMMain : Form
    {
        System.Text.ASCIIEncoding enc = new ASCIIEncoding();
        
        String serial;

        char cDataChar = ' '; // The data char received from the sensor.

        // Comm packets
        Boolean do_wdt_error = false;
        Boolean do_checksum_error = false;
        Boolean do_set_serial = false;
        Boolean do_load_defaults = false;
        Boolean do_clear_wdt = false;

        // Comm packets
        byte[] cmd_read_all_ram = { 0xaa, 0x07, 0x52, 0x01, 0xbb, 0x03, 0x01, 0xc2, 0xcc };
        byte[] cmd_read_all_eeprom = { 0xaa, 0x07, 0x52, 0x80, 0x00, 0x07, 0x01, 0x8a, 0xcc };
        byte[] cmd_do_wdt_error = { 0xAA, 0x07, 0x57, 0x01, 0xbc, 0x01, 0x01, 0xc6, 0xCC };
        byte[] cmd_do_chksum_error = { 0xAA, 0x07, 0x57, 0x01, 0xBC, 0x02, 0x01, 0xC7, 0xCC };

        object lock_inbuff = new object();
        Byte[] inbuff = new byte[1024];
        int inbuff_ptr = 0;
        const int memory_size = 64 * 1024; // Memory size of the device
        const int input_buff_size = 16 * 1024; // 16k memory for input buffer
        byte[] memory_map = new byte[memory_size];
        
        String parameters_filename;
        Parameters c_parameters;
        ArrayList arr_timer_read_pages; // An array of timers to activate page reading mechanism
        // GUI parameters to cotrol the position and size of the dynamic controls.
        const int gui_parameters_per_column = 23; // How many parameters we have in each column.
        const int gui_total_vertical_space = 20; // Total space for each line.
        const int gui_vertical_space = 20; // Space for each line.
        const int gui_horizontal_space = 30; // Space between each column
        const int gui_x_start = 0;  // Lines startup position (x).
        const int gui_y_start = 0; // Line's startup position (y).
        const int gui_textbox_width = 70; // Well ... textbox's width ...
        const int gui_label_width = 100; // Well ... label's width ...
        //        const int gui_nud_width = 18; // Numeric updown width
        const int gui_space_between_controls = 0; // The space between label,textbox and numeric updown controls (of the same parameter).

        const int MAX_BUFF_SIZE = 400 + 4; // For graph drawing. 400 + 1 for first "R" and 1 for last checksum...
        const int UNFILTERED_BUFF_ADDR = 0x300;
        const int FILTERED_BUFF_ADDR = 0x500;

        // Board modes.
        int current_mode = 0;
        const int CM_NO_RESPONSE = -1;
        const int CM_RUNNING = 0;
        const int CM_DOWNLOADING = 1;
        const int CM_OFFLINE = 2;
        const int CM_ONLINE = 3;


        long global_checksum;
        bool downloading = false; // Mark that we are in download proccess, and communication flags should not be checked.
        bool check_mode; // Mark that we should check the current mode (running / downloading / No response).

        // Firmware update stuff
        int firmware_version; // Version as recieved from the board.
        const long PAGES = 120;
        const long PAGE_SIZE = 128;
        const long buff_size = PAGES * PAGE_SIZE;
        const int ack_timeout = 1000;
        byte[] hex_buf;
        long[,] pages_checksum; // index 0 = checksum, index 1 = position checksum, index 2 = flag to mark if page empty (only with &hff).
        long page_checksum_from_board;
        long page_position_checksum_from_board;
        long current_page;

        // Communication stuff
        const int timeout_pageread = 200; // 12
        const int timeout_parameter_read = 200; // 12 For the functions folder
        const int timeout_parameter_write = 200; // For write timeout.
        const int timeout_page_write = 200; // For write timeout.
        const int timeout_ack = 200; // For acknowledge of page download.
        int n_err_count; // Count how many failures we had (timeouts).
        

        int last_min_pos = 0;
        int last_max_pos = 0;
        long last_min_y = 0;
        long last_max_y = 0;

        Font myFont = new Font("Arial", 7);
        SolidBrush my_lg_Brush = new SolidBrush(Color.LightGreen);
        SolidBrush my_red_Brush = new SolidBrush(Color.Red);
        SolidBrush my_black_Brush = new SolidBrush(Color.Black);

        // Set format of string.
        StringFormat myFormat = new StringFormat();
        Graphics g;

        public string appPath;
        long n_last_value = -1; // previous measured value.
        long n_current_peak_max = -1;
        int n_current_graph_peak_max = -1;
        long n_current_peak_min = -1;
        int n_current_graph_peak_min = -1;
        float f_temp_max_graph_x = -1; // Store the x,y of the max val/
        float f_temp_max_graph_y = -1;
        long[] n_on_screen_vals;
        long[] n_on_screen_y;
        peaks c_peaks = new peaks(); // Graph peaks.

        string port_name;
        Queue val_queue;
        float mid;
        float span;
        int avg;
        Bitmap graph = new Bitmap(800, 512);
        float last_x;
        float last_y;
        float current_x;
        float current_y;
        float last_x1;
        float last_y1;

        float avg_sum;
        float avg_count;
        bool autocenter = false;
        FileStream sw;
        long checksum_error_counter = 0;
        long packets = 0;
        long timeouts = 0;
        // long missing_packets = 0;
        long last_packet_number = -1;
        System.Threading.Timer tt_comm_reset;
        System.Threading.TimerCallback tt_comm_reset_cb;


        public FRMMain()
        {
            InitializeComponent();
            appPath = Application.StartupPath;
            parameters_filename = appPath + "\\Sensor_parameters.csv";
            LoadParametersControl();
        }

        private void comm_reset(object sender)
        {
            timeouts++;
            val_queue.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            Boolean is_filename_ok = false;

            String f = SensorScope.Default.Setting_FirmFileName;
            try
            {
                FileInfo fi = new FileInfo(f);
                 is_filename_ok = true;
            }
            catch
            {
                f = "";
            }

            if (is_filename_ok)
            {
                TXTFileName.Text = SensorScope.Default.Setting_FirmFileName;
                DLGFILEFirmwareUpdte.FileName = SensorScope.Default.Setting_FirmFileName;
            }
            else
            {
                DLGFILEFirmwareUpdte.FileName = "";
                TXTFileName.Text = "";
            }

            hex_buf = new byte[buff_size];
            pages_checksum = new long[PAGES, 3]; // ' index 0 = checksum, index 1 = position checksum, index 2 = flag to mark if page empty (only with &hff).

            myFormat.FormatFlags = StringFormatFlags.NoWrap;
            n_on_screen_vals = new long[PICGraph.Width];
            n_on_screen_y = new long[PICGraph.Width];
            for (i = 0; i < n_on_screen_vals.Length; i++)
                n_on_screen_vals[i] = -1;
            
            tt_comm_reset_cb = new TimerCallback(comm_reset);
            tt_comm_reset = new System.Threading.Timer(tt_comm_reset_cb);
            last_x = -1;
            val_queue = new Queue();
            avg_count = avg_sum = 0;
            TSStrain.Text = "strain gage:   ";

            port_name = SensorScope.Default.PortName;
            try
            {
                mid = Convert.ToInt32(SensorScope.Default.Mid);
                span = Convert.ToInt32(SensorScope.Default.Span);
                avg = Convert.ToInt32(SensorScope.Default.Average);
            }
            catch
            {
            }

            TXTMid.Text = SensorScope.Default.Mid;
            TXTSpan.Text = SensorScope.Default.Span;
            TXTAverage.Text = SensorScope.Default.Average;
            

            GetPorts();
            i = CMBPortSelector.Items.IndexOf(port_name);
            if (port_name != "" && CMBPortSelector.Items.IndexOf(port_name) < 0)
            {
                CMBPortSelector.Items.Add(port_name);
                CMBPortSelector.SelectedItem = port_name;
            }
            UpdateGraphBackground();
        }

        private void GetPorts()
        {
            string[] theSerialPortNames = System.IO.Ports.SerialPort.GetPortNames();
            CMBPortSelector.ResetText();
            CMBPortSelector.Items.AddRange(theSerialPortNames);
        }

        private void CMBPortSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            close_port();
            port_name = CMBPortSelector.SelectedItem.ToString();
            serialPort1.PortName = port_name;
            SensorScope.Default.PortName = port_name;
            SensorScope.Default.Save();
        }

        private void RADOpen_CheckedChanged(object sender, EventArgs e)
        {
            if (RADOpen.Checked)
            {
                try
                {
                    inbuff_ptr = 0;
                    open_port();
                    if (!serialPort1.IsOpen)
                    {
                        RADClose.Checked = true;
                        return;
                    }
                    RADClose.Checked = false;
                    RADOpen.BackColor = Color.LimeGreen;
                    RADClose.BackColor = Color.IndianRed;
                    TSPortStatus.Text = "Port is open.";
                    TSPortStatus.BackColor = Color.LimeGreen;
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Can't open the port:\n" + e1.Message);
                    RADClose.Checked = true;
                }
                
            }
        }

        private void RADClose_CheckedChanged(object sender, EventArgs e)
        {
            if (RADClose.Checked)
            {
                close_port();
                RADOpen.BackColor = Color.Lime;
                RADClose.BackColor = Color.DarkRed;
                TSPortStatus.Text = "Port is closed.";
                TSPortStatus.BackColor = Color.IndianRed;
            }
        }

        private void TMRDateAndTime_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.TimeOfDay.ToString().Length > 8)
                TSTime.Text = DateTime.Now.TimeOfDay.ToString().Remove(8);
            else
                TSTime.Text = DateTime.Now.TimeOfDay.ToString();
            TSDate.Text = DateTime.Now.Date.ToShortDateString();

            TSBadChecksum.Text = "   Bad checksums: " + checksum_error_counter.ToString() + "   "; 
            // TSMissingsPackets.Text = "   Missing packets: " + missing_packets.ToString() + "   "; 
            LBLPackets.Text = packets.ToString();
            TSTimeouts.Text = "   Timeouts: " + timeouts.ToString() + "   ";
            lblDataChar.Text = cDataChar.ToString();

            // label3.Text = val_queue.Count.ToString();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

            try
            {
                int n_bytes = serialPort1.BytesToRead;
                if (inbuff_ptr + n_bytes > inbuff.Length)
                {
                    serialPort1.DiscardInBuffer();
                    // MessageBox.Show("Overflow");
                    Debug.WriteLine("Overflow");
                    inbuff_ptr = 0;
                    return;
                }
                lock (lock_inbuff)
                {
                    serialPort1.Read(inbuff, inbuff_ptr, n_bytes);
                    inbuff_ptr += n_bytes;
//                    Debug.WriteLine("Got " + n_bytes.ToString() + " bytes.");
                }
            }
            catch (Exception e1)
            {
                show_error(e1);
            }
        }

        private void show_error(Exception e1)
        {
            RADClose.Checked = true;
            MessageBox.Show("An error occured:\n" + e1.Message);
        }

        private void reset_buff(int i)
        {
            if (i >= inbuff_ptr)
                inbuff_ptr = 0;
            else
            {
                Array.Copy(inbuff, i, inbuff, 0, inbuff_ptr - i);
                inbuff_ptr = inbuff_ptr - i;
            }
        }

        private void dispatch_packet()
        {
            Byte temp_byte1, b1, b2, b3;
            Int32 checksum, my_checksum;
            int i;
            Int32[] values = new Int32[30];
            Int32 StrainGage;

            String address = Convert.ToString(inbuff[3] * 256 + inbuff[4], 16).ToLower();


            #region "sample"
            if (address == "101") // Got a sample frame
            {
                if (inbuff[1] < 10)
                    return;
                
                for (i = 0; i < 30; i++)
                {
                    b1 = inbuff[6 + i * 3];
                    b2 = inbuff[6 + i * 3 + 1];
                    b3 = inbuff[6 + i * 3 + 2];
                    values[i] = b1 * 65536 + b2 * 256 + b3;
                }

                // Update X Y and Z
                switch (inbuff[96])
                {
                    case 0:
                        tsslblX.Text = "X = " + Convert.ToString(inbuff[97] + inbuff[98] * 256);
                        break;
                    case 1:
                        tsslblY.Text = "Y = " + Convert.ToString(inbuff[97] + inbuff[98] * 256);
                        break;
                    case 2:
                        tsslblZ.Text = "Z = " + Convert.ToString(inbuff[97] + inbuff[98] * 256);
                        break;
                }

                // Get the strain gage
                b1 = inbuff[96];
                b2 = inbuff[97];
                b3 = inbuff[98];
                StrainGage = b1 * 65536 + b2 * 256 + b3;

                // Ok, we got a packet. Let's roll.
                packets++;

                for (i = 0; i < 30; i++)
                {
                    // Write the value to the file.
                    if (sw != null)
                    {
                        Int32 val32 = values[i];
                        Byte b = (byte)(val32 & 0xff);
                        sw.WriteByte(b);
                        b = (byte)((val32 & 0x0000ff00) / Math.Pow(2, 8));
                        sw.WriteByte(b);
                        b = (byte)((val32 & 0x00ff0000) / Math.Pow(2, 16));
                        sw.WriteByte(b);
                        b = 0;
                        sw.WriteByte(b);
                    }

                    avg_sum += values[i];
                    avg_count++;
                    if (avg_count >= avg)
                    {
                        float value = avg_sum / avg_count;

                        if (autocenter)
                        {
                            clear_screen();
                            autocenter = false;
                            mid = value;
                            TXTMid.Text = mid.ToString();
                        }

                        // current_y = 256 - (value - mid) / span * 256;
                        current_y = 512 - ((value - (mid - span / 2)) / span) * 512;
                        if (i % 2 == 0)
                            draw_pixel(current_x, current_y);
                        else
                            draw_pixel1(current_x, current_y);

                        check_peaks(values[i], Convert.ToInt32(current_x), Convert.ToInt32(current_y));
                        current_x = ((current_x + 1) % PICGraph.Width);
                        n_on_screen_vals[(int)current_x] = Convert.ToInt64(value);
                        n_on_screen_y[(int)current_x] = Convert.ToInt64(current_y);
                        avg_sum = avg_count = 0;
                        TSStrain.Text = "strain gage: " + StrainGage.ToString();
                    }
                }
                if (sw != null)
                {
                    // Write the index
                    sw.WriteByte(inbuff[96]);
                    sw.WriteByte(0);
                    sw.WriteByte(0);
                    sw.WriteByte(0);

                    // Write the value
                    sw.WriteByte(inbuff[97]);
                    sw.WriteByte(inbuff[98]);
                    sw.WriteByte(0);
                    sw.WriteByte(0);
                }
                find_min_max();
            }
            #endregion

            #region "ram"
            else if (address == "1bb") // Got a sample frame
            {
                reset_no_comm_timer();
                Array.Copy(inbuff, 6, memory_map, 0x1bb, 3);
            }
            #endregion


            #region "eeprom"
            else if (address == "8000") // Got a sample frame
            {
                reset_no_comm_timer();
                Array.Copy(inbuff, 6, memory_map, 0x8000, 7);
            }
            #endregion

            else
            {
                Debug.WriteLine(address.ToString());
            }


        }

        private void reset_no_comm_timer()
        {
            if (! downloading)
                pic_bw_background.Visible = false;
            TMRNoCommDetection.Enabled = false;
            TMRNoCommDetection.Interval = 1000;
            TMRNoCommDetection.Enabled = true;

        }

        private void find_min_max()
        {
            int i;
            long min = long.MaxValue;
            long max = long.MinValue;
            int min_pos = 0;
            int max_pos = 0;

            for (i = 0; i < n_on_screen_vals.Length && n_on_screen_vals[i] != -1; i++)
            {
                if (n_on_screen_vals[i] > max)
                {
                    max = n_on_screen_vals[i];
                    max_pos = i;
                }
                if (n_on_screen_vals[i] < min)
                {
                    min = n_on_screen_vals[i];
                    min_pos = i;
                }
            }
            if (last_min_pos != min_pos || last_max_pos != max_pos)
            {
                g.DrawString("min", myFont, my_black_Brush, last_min_pos, last_min_y, myFormat);
                g.DrawString("max", myFont, my_black_Brush, last_max_pos, last_max_y, myFormat);
                last_min_pos = min_pos;
                last_max_pos = max_pos;
                last_min_y = n_on_screen_y[min_pos] + 10;
                last_max_y = n_on_screen_y[max_pos] - 20;
                last_min_y = Math.Min(500, last_min_y);
                last_max_y = Math.Max(10, last_max_y);

                LBLAllp2p.Text = Convert.ToString(max - min);
                g.DrawString("min", myFont, my_lg_Brush, min_pos, last_min_y, myFormat);
                g.DrawString("max", myFont, my_red_Brush, max_pos, last_max_y, myFormat);
            }
        }

        private void check_peaks(int p, int graph_x,int graph_y)
        {
            g = Graphics.FromImage(graph);
            if (n_current_graph_peak_min == -1 && graph_y <= n_current_graph_peak_max) // We are still going up or even.
            {
                n_current_graph_peak_max = graph_y;
                n_current_peak_max = p;
                f_temp_max_graph_x = graph_x;
                f_temp_max_graph_y = graph_y;
            }
            else if (n_current_graph_peak_min == -1) // Get the first value for reference
            {
                n_current_graph_peak_min = graph_y;
            }
            else if (graph_y >= n_current_graph_peak_min) // Got a new minimum
            {
                n_current_graph_peak_min = graph_y;
                n_current_peak_min = p;
            }
            else // We got a new rise
            {
                // Save the current peak
                // peaks.c_peak new_peak = new peaks.c_peak(n_current_peak_max,n_current_peak_min,graph_x,graph_y,f_temp_max_graph_x,f_temp_max_graph_y);
                // c_peaks.Peaks.Add(new_peak);

                //graph.SetPixel((int)f_temp_max_graph_x, (int)f_temp_max_graph_y, Color.Red);
                //graph.SetPixel((int)graph_x, (int)graph_y, Color.Red);
                if (n_current_peak_max != -1 && n_current_peak_min != -1 && Math.Abs(n_current_peak_max - n_current_peak_min) > 0.025 * span)
                {
                    g.DrawString("x", myFont, my_red_Brush, f_temp_max_graph_x - 5, f_temp_max_graph_y - 10, myFormat);
                    g.DrawString("x", myFont, my_lg_Brush, graph_x - 5, graph_y, myFormat);
                    LBLLastPeak.Text = Convert.ToString(n_current_peak_max - n_current_peak_min);
                }
                n_current_graph_peak_max = graph_y;
                n_current_peak_max = p;
                n_current_graph_peak_min = -1;
                n_current_peak_min = -1;
                
            }


        }


        private void draw_pixel(float x, float y)
        {
            Graphics g = Graphics.FromImage(graph);
            Pen p = new Pen(Color.Yellow);
            float next_x = ((current_x + 1) % 800);
            if (Convert.ToInt32(y) < 0)
                y = 0;
            if (Convert.ToInt32(y) >= graph.Height)
                y = graph.Height - 1;

            p.Color = Color.Black;
            g.DrawLine(p, x, 250, x, 262);
            p.Color = Color.Yellow;
            if (last_x != -1 && last_x < current_x)
                g.DrawLine(p, last_x, last_y, x, y);
            last_x = x;
            last_y = y;
            graph.SetPixel(Convert.ToInt32(x), 256, Color.Gray);
            graph.SetPixel(Convert.ToInt32(x), Convert.ToInt32(y), Color.Yellow);

            // Clear the next pixel
            p.Color = Color.Black;
            g.DrawLine(p, next_x, 0, next_x, 512); // Vertical black line
            for (int i = 0; i < 512; i += 64)
                graph.SetPixel(Convert.ToInt32(next_x), i, Color.FromArgb(60,60,60));
            // graph.SetPixel(Convert.ToInt32(next_x), 256, Color.LightGreen);
            p.Color = Color.LightGreen;
            g.DrawLine(p,next_x, 250, next_x, 262);
            PICGraph.Image = graph;
        }

        private void draw_pixel1(float x, float y)
        {
            Graphics g = Graphics.FromImage(graph);
            Pen p = new Pen(Color.LightBlue);
            float next_x = ((current_x + 1) % 800);
            if (Convert.ToInt32(y) < 0)
                y = 0;
            if (Convert.ToInt32(y) >= graph.Height)
                y = graph.Height - 1;

            p.Color = Color.Black;
            g.DrawLine(p, x, 250, x, 262);
            p.Color = Color.LightBlue;
            if (last_x1 != -1 && last_x1 < current_x)
                g.DrawLine(p, last_x1, last_y1, x, y);
            last_x1 = x;
            last_y1 = y;
            graph.SetPixel(Convert.ToInt32(x), 256, Color.Gray);
            graph.SetPixel(Convert.ToInt32(x), Convert.ToInt32(y), Color.LightBlue);

            // Clear the next pixel
            p.Color = Color.Black;
            g.DrawLine(p, next_x, 0, next_x, 512); // Vertical black line
            for (int i = 0; i < 512; i += 64)
                graph.SetPixel(Convert.ToInt32(next_x), i, Color.FromArgb(60, 60, 60));
            // graph.SetPixel(Convert.ToInt32(next_x), 256, Color.LightGreen);
            p.Color = Color.LightGreen;
            g.DrawLine(p, next_x, 250, next_x, 262);
            PICGraph.Image = graph;
        }


        
        private void mark_x(float x, float y)
        {
        }



        private void UpdateGraphBackground()
        {
            int i;
            long mid = PICGraph.Height / 2;
            Graphics g = Graphics.FromImage(graph);
            // SolidBrush sbr = new SolidBrush(Color.Yellow);
            Pen p = new Pen(Color.FromArgb(60, 60, 60));
//            p.DashStyle = DashStyle.Dot;
            // Draw the middle line.
            for (i=0; i<512; i+=64)
                g.DrawLine(p, 0, i, PICGraph.Width, i);
            p.Color = Color.Gray;
            g.DrawLine(p, 0, mid, PICGraph.Width, mid);

            PICGraph.Image = graph;

            return;
        }


        private void CheckHex(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox) sender;
            if ((e.KeyChar < '0' || e.KeyChar > '9') && (e.KeyChar < 'a' || e.KeyChar > 'f') && (e.KeyChar < 'A' || e.KeyChar > 'F') && e.KeyChar != '\b' && e.KeyChar != (char)Keys.Delete && e.KeyChar != (char)Keys.Enter)
                e.Handled = true;

            if (e.KeyChar == (char)Keys.Enter)
            {
                LBLMid.Focus();
                e.Handled = true;
            }
        }

        private void CheckNum(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b' && e.KeyChar != (char)Keys.Delete && e.KeyChar != (char)Keys.Enter)
                e.Handled = true;

            if (e.KeyChar == (char)Keys.Enter)
            {
                LBLMid.Focus();
                e.Handled = true;
            }
        }

        
        private void TXTSpan_Leave(object sender, EventArgs e)
        {
            try
            {
                span = Convert.ToInt32(TXTSpan.Text);
                SensorScope.Default.Span = TXTSpan.Text;
                SensorScope.Default.Save();
                clear_screen();
            }
            catch
            {
                MessageBox.Show("Invalid span value.");
                TXTSpan.Focus();
                TXTSpan.Select(0,TXTSpan.Text.Length);
            }
        }


        private void TXTMid_Leave(object sender, EventArgs e)
        {
            try
            {
                mid = Convert.ToInt32(TXTMid.Text);
                SensorScope.Default.Mid = TXTMid.Text;
                SensorScope.Default.Save();
                clear_screen();
            }
            catch
            {
                MessageBox.Show("Invalid mid value.");
                TXTMid.Focus();
                TXTMid.Select(0, TXTMid.Text.Length);
            }
        }

        private void TXTAverage_Leave(object sender, EventArgs e)
        {
            try
            {
                avg = Convert.ToInt32(TXTAverage.Text);
                if (avg <= 0 || avg > 99)
                {
                    avg = 99;
                    TXTAverage.Text = "99";
                }
                SensorScope.Default.Average = TXTAverage.Text;
                SensorScope.Default.Save();
            }
            catch
            {
                MessageBox.Show("Invalid averaging value.");
                TXTAverage.Focus();
                TXTAverage.Select(0, TXTMid.Text.Length);
            }

        }

        private void BTNAutoCenter_Click(object sender, EventArgs e)
        {
            autocenter = true;
        }

        private void clear_screen()
        {
            graph = new Bitmap(800, 512);
            UpdateGraphBackground();
            avg_count = avg_sum = 0;
            current_x = 0;
            last_x = last_x1 = -1;
            PICGraph.Image = graph;
        }

        private void CHKSaveToFile_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult rc;

            if (CHKSaveToFile.Checked)
            {
                rc = saveFileDialog1.ShowDialog();
                if (rc != DialogResult.OK)
                {
                    CHKSaveToFile.Checked = false;
                    return;
                }
                sw = new FileStream(saveFileDialog1.FileName, FileMode.Create);

            }
            else
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                    sw = null;
                }
            }

        }

        private void FRMMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sw != null)
            {
                sw.Flush();
                sw.Close();
            }
            close_port();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            FileStream ttt = new FileStream(saveFileDialog1.FileName, FileMode.Open);
            int b1, b2, b3, b4;
            Int32 t;
            b1 = ttt.ReadByte();
            b2 = ttt.ReadByte();
            b3 = ttt.ReadByte();
            b4 = ttt.ReadByte();
            t = (int)(b4 * Math.Pow(2, 24) + b3 * Math.Pow(2, 16) + b2 * Math.Pow(2, 8) + b1);
            ttt.Close();
        }

        private void CMBPortSelector_Click(object sender, EventArgs e)
        {
            CMBPortSelector.Items.Clear();
            GetPorts();
        }

        private void FRMMain_Shown(object sender, EventArgs e)
        {
            
        }

        private void CMDBrowse_Click(object sender, EventArgs e)
        {
            // int version;
            try
            {
                FileInfo fi = new FileInfo(DLGFILEFirmwareUpdte.FileName);
            }
            catch
            {
                DLGFILEFirmwareUpdte.FileName = "";
            }


            DLGFILEFirmwareUpdte.InitialDirectory = appPath;
            DLGFILEFirmwareUpdte.Filter = "hex files (*.hex)|*.hex|All files (*.*)|*.*";
            DLGFILEFirmwareUpdte.FilterIndex = 1;
            DLGFILEFirmwareUpdte.RestoreDirectory = true;

            if (DLGFILEFirmwareUpdte.ShowDialog() == DialogResult.OK)
            {
                TXTFileName.Text = DLGFILEFirmwareUpdte.FileName;
                SensorScope.Default.Setting_FirmFileName = DLGFILEFirmwareUpdte.FileName;
                SensorScope.Default.Save();
            }

        }

        private void CMDCalcChecksum_Click(object sender, EventArgs e)
        {
            if (download_file(false, true))
            {
                CMDCalcChecksum.Text = "$" + Convert.ToString(global_checksum, 16);
                TMRHideChecksum.Interval = 2000;
                TMRHideChecksum.Enabled = true;
            }

        }

        private void TMRHideChecksum_Tick(object sender, EventArgs e)
        {
            TMRHideChecksum.Enabled = false;
            CMDCalcChecksum.Text = "Calc Checksum";
            CMDDownload.Enabled = true;
        }


        private void download_file()
        {
            download_file(false, false);
        }

        private bool download_file(bool quick, Boolean only_calc_checksum)
        {
            StreamReader f;


            open_port();
            if (DLGFILEFirmwareUpdte.FileName == "" || !serialPort1.IsOpen)
            {
                close_port();
                if (DLGFILEFirmwareUpdte.FileName == "")
                    MessageBox.Show("Please select a file.");
                return false;
            }

            // f = DLGFILEFirmwareUpdte.OpenFile();
            try
            {
                f = File.OpenText(DLGFILEFirmwareUpdte.FileName);
            }
            catch (Exception e)
            {
                close_port();
                MessageBox.Show("Can't open the hex file : \n" + e.Message);
                return false;
            }

            long i, j;
            String input_line;
            long base_addr;
            bool end_of_file; // Mark that a record of type 01 (end of file) was found.
            int line_data_size;
            long line_offset_addr;
            int line_data_type;
            String line_data;
            String line_checksum;
            long line_counter;
            int dirty_page_count;
            DialogResult dresult;
            byte[] buff = new byte[10];

            CMDDownload.Enabled = false;
            downloading = true;
            Thread.Sleep(200);


            for (i = 0; i < hex_buf.GetUpperBound(0); i++)
                hex_buf[i] = 0xff;

            base_addr = 0;
            line_counter = 0;
            end_of_file = false;
            progressBar1.Value = 0;
            progressBar1.Maximum = (int)PAGES;
            progressBar1.Refresh();

            while (!f.EndOfStream && !end_of_file)
            {
                line_counter++;
                input_line = f.ReadLine();
                if (input_line.Length < 11)
                {
                    MessageBox.Show("Error : invalid data  (line too short) in line no. " + line_counter + ".\nAborting download.");
                    goto file_fatal_error;
                }
                line_data_size = Convert.ToInt32(input_line.Substring(1, 2), 16);
                input_line = input_line.Substring(0, 11 + (line_data_size * 2)); // cut off unneeded data.
                line_offset_addr = Convert.ToInt64(input_line.Substring(3, 4), 16);
                line_data_type = Convert.ToInt32(input_line.Substring(7, 2));
                if (input_line.Length != 11 + (line_data_size * 2))
                {
                    MessageBox.Show("Error : invalid data (wrong line length) in line no. " + line_counter.ToString() + ".\nAborting download.");
                    goto file_fatal_error;
                }
                line_data = input_line.Substring(9, line_data_size * 2);
                line_checksum = input_line.Substring(10 + line_data_size * 2 - 1, 2);
                switch (line_data_type)
                {
                    case 0: //  Data line
                        for (i = 0; i < (line_data.Length / 2); i++)
                        {
                            if (base_addr + line_offset_addr + i >= PAGES * PAGE_SIZE)
                                end_of_file = true;
                            else
                                hex_buf[base_addr + line_offset_addr + i] = Convert.ToByte(line_data.Substring((int)i * 2, 2), 16);
                        }
                        break;

                    case 1: // End of file
                        end_of_file = true;
                        break;

                    case 2: // Set base address
                        base_addr = Convert.ToInt64(line_data, 16) * 0x10;
                        break;

                    default:
                        MessageBox.Show("Invalid data (bad type) in line no. " + line_counter.ToString() + ".\nAborting download.");
                        goto file_fatal_error;
                }
            }

            global_checksum = 0;
            for (i = 0; i < buff_size - 1; i++)
                global_checksum = global_checksum + hex_buf[i];

            global_checksum = global_checksum & 0xFF;
            if (only_calc_checksum)
            {
                downloading = false;
                return true;
            }
            Debug.WriteLine("Calculated checksum is " + global_checksum.ToString());
            Debug.WriteLine("Roni's checksum : " + hex_buf[PAGES * PAGE_SIZE - 1].ToString());
            // OK We've got the memory map. Let's calculate the checksums.
            hex_buf[PAGES * PAGE_SIZE - 1] = (byte)global_checksum;  // this is a place for the checksum, so we don't want it in the checksum itself.

            for (i = 0; i < PAGES; i++)
            {
                pages_checksum[i, 0] = 0;
                pages_checksum[i, 1] = 0;
                pages_checksum[i, 2] = 0;
                for (j = 0; j < PAGE_SIZE; j++)
                {
                    pages_checksum[i, 0] = (pages_checksum[i, 0] + hex_buf[i * PAGE_SIZE + j]);
                    pages_checksum[i, 1] = (pages_checksum[i, 1] + hex_buf[i * PAGE_SIZE + j] * j);
                    if (hex_buf[i * PAGE_SIZE + j] != 0xFF)
                        pages_checksum[i, 2] = 1;
                }
                pages_checksum[i, 0] = pages_checksum[i, 0] & 0xFF;
                pages_checksum[i, 1] = pages_checksum[i, 1] & 0xFF;
            }

            current_page = 0;
            set_download_mode(CM_DOWNLOADING);
            check_current_mode();
            if (current_mode != CM_DOWNLOADING)
            {
                MessageBox.Show("Download error. Conuldn't set download mode.");
                goto file_fatal_error;
            }

            current_mode = CM_DOWNLOADING;
            dirty_page_count = 0;
//            using (FileStream fs = new FileStream("c:\\test.hex", FileMode.Create))
//            {
//                fs.Write(hex_buf, 0, (int)buff_size);
//            }
            for (i = 0; i < PAGES; i++)
            {
            retry:
//                Debug.WriteLine("writing page #" + i.ToString());
                if (!set_page(i, 3))
                {

                    MessageBox.Show("Download error. Couldn't set page (" + current_page.ToString() + ").");
                    //dresult = MessageBox.Show("Communication error during download (set page address).\nThe software download was not completed.", "Download Error", MessageBoxButtons. AbortRetryIgnore);
                    //if (dresult == DialogResult.Retry)
                    //    goto retry;
                    // else if (dresult == DialogResult.Ignore)
                    //    goto retry_next;

                    goto file_fatal_error;
                }

                //delay(ack_timeout);
                if (!write_page(current_page))
                {
                    MessageBox.Show("Download error. Couldn't write page.");
                    //dresult = MessageBox.Show("Communication error during download (write page - f).\nThe software download was not completed.", "Download Error", MessageBoxButtons.AbortRetryIgnore);
                    //if (dresult == DialogResult.Retry)
                    //    goto retry;
                    //else if (dresult == DialogResult.Ignore)
                    //    goto retry_next;

                    goto file_fatal_error;
                }
                current_page++;
                progressBar1.Value++;
                Application.DoEvents();
            retry_next: ;
            }

            progressBar1.Value = 0;
            set_download_mode_old_frame(CM_RUNNING);
            // MessageBox.Show("Download completed!");
            goto skip_dc;

        file_fatal_error:
            set_download_mode_old_frame(CM_RUNNING);
            check_current_mode();
        skip_dc:
            f.Close();
            CMDDownload.Enabled = true;
            inbuff_ptr = 0;
            downloading = false;
            close_port();
            return true;
        }


        private bool set_download_mode_old_frame(byte mode)
        {
            bool rc;
            byte[] buff = new byte[5];

            if (!serialPort1.IsOpen)
                return false;

            buff[0] = (byte)'W';
            buff[1] = get_special_field_addr_hi(1);
            buff[2] = get_special_field_addr_lo(1);
            buff[3] = mode;
            buff[4] = (byte)get_checksum(buff, 4);
            rc = send_expect(buff, 5, "W", null, 0, timeout_parameter_write);

            if (rc)
                current_mode = mode;
            else
                current_mode = CM_NO_RESPONSE;

            return rc;
        }


        private bool set_download_mode(byte mode)
        {
            bool rc;
            byte[] buff = new byte[5];

            if (!serialPort1.IsOpen)
                return false;

            Byte[] frame;

            frame = create_write_frame(0x29a, mode);
            serialPort1.Write(frame, 0, frame.Length);
            Thread.Sleep(100);

            current_mode = mode;
            return true;
        }

        private bool set_page(long page, int retries)
        {
            byte[] buff = new byte[5];
            long temp;
            Boolean rc;
            int tries;

            if (!serialPort1.IsOpen)
                return false;



            buff[0] = 0x57;
            buff[1] = get_special_field_addr_hi(2);
            buff[2] = get_special_field_addr_lo(2);
            temp = (page & 0xFF00) / 256; // Download mode
            buff[3] = (byte)temp;
            buff[4] = (byte)get_checksum(buff, 4);
            tries = 0;
            do
            {
                rc = send_expect(buff, 5, "W", null, 0, timeout_parameter_write);
                if (!rc)
                    Debug.WriteLine("set page failed (1)" + current_page.ToString());
                tries++;
            } while (tries < retries && !rc);
            if (!rc)
                return false;
            
            //delay(ack_timeout);

            buff[0] = 0x57;
            buff[1] = get_special_field_addr_hi(3);
            buff[2] = get_special_field_addr_lo(3);
            temp = (page & 0xFF);
            buff[3] = (byte)temp;
            buff[4] = (byte)get_checksum(buff, 4);
            tries = 0;
            do
            {
                rc = send_expect(buff, 5, "W", null, 0, timeout_parameter_write);
                if (!rc)
                    Debug.WriteLine("set page failed (2)" + current_page.ToString());
                tries++;
            } while (tries < retries && !rc);
            return rc;
        }

        private void open_port()
        {

            if (serialPort1.IsOpen)
                return;

            try
            {
                serialPort1.PortName = SensorScope.Default.PortName;
                serialPort1.Open();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error while opening the serial port :\n" + e1.Message);
            }
            //            update_status_bar_and_menu();
        }

        private void close_port()
        {
            if (!serialPort1.IsOpen)
            delay(100);
            try
            {
                serialPort1.Close();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error while closing the serial port :\n" + e1.Message);
            }
            //            update_status_bar_and_menu();

        }

        private void delay(int ms)
        {
            Thread.Sleep(ms);
        }

        private void check_current_mode()
        {

            byte[] buff_out = new byte[10];
            byte[] buff_in = new byte[10];

            check_mode = false;
            if (!serialPort1.IsOpen)
                return;


            buff_out[0] = (byte)'R';
            buff_out[1] = get_special_field_addr_hi(1);
            buff_out[2] = get_special_field_addr_lo(1);
            buff_out[3] = 1;
            buff_out[4] = (byte)get_checksum(buff_out, 4);
            Debug.WriteLine("Sending check mode packet");
            if (!send_expect(buff_out, 5, "", buff_in, 3, ack_timeout) || buff_in[2] != get_checksum(buff_in, 2))
                current_mode = CM_NO_RESPONSE;
            else
                current_mode = buff_in[1];
        }

        private bool write_page(long current_page)
        {
            byte[] buf = new byte[PAGE_SIZE + 2];
            byte[] in_buff = new byte[10];
            int i;
            int retries;
            int error_count;
            long checksum;
            bool got_ack;

            if (pages_checksum[current_page, 2] == 0) // The page is empty
            {
                buf[0] = (byte)'E';
                buf[1] = (byte)'E';
                // checksum = (pages_checksum[current_page, 0] + (byte)'E') % 256;
                // buf[1] = (byte)checksum;
                goto send_buff;
            }

            buf[0] = (byte)'P';
            for (i = 0; i < PAGE_SIZE; i++)
                buf[i + 1] = hex_buf[current_page * PAGE_SIZE + i];

            checksum = (pages_checksum[current_page, 0] + (byte)'P') % 256;
            buf[PAGE_SIZE + 1] = (byte)checksum;


        send_buff:
            in_buff[0] = 0;
            retries = 20;
            got_ack = false;
            error_count = 0;
            while (retries > 0 && !got_ack)
            {
                if (buf[0] == 'E')
                    got_ack = send_expect(buf, 2, "A", null, 0, 100);
                else
                    got_ack = send_expect(buf, (int)PAGE_SIZE + 2, "A", null, 0, 100);

                // delay(ack_timeout);
                retries = retries - 1;
                if (!got_ack)
                    error_count++;
            }

            return got_ack;
        }


        private byte get_special_field_addr_hi(int special_field_code)
        {
            parameter special_param;

            if (special_field_code > c_parameters.special_params.Count)
                return 0;

            special_param = (parameter)c_parameters.special_params[special_field_code - 1];
            return (byte)(Convert.ToInt32(special_param.s_Address, 16) / 256);
        }

        private byte get_special_field_addr_lo(int special_field_code)
        {
            parameter special_param;

            if (special_field_code > c_parameters.special_params.Count)
                return 0;

            special_param = (parameter)c_parameters.special_params[special_field_code - 1];
            return (byte)(Convert.ToInt32(special_param.s_Address, 16) % 256);
        }

        long get_checksum(byte[] buff, int length)
        {
            int i;
            long sum;

            sum = 0;
            for (i = 0; i < length; i++)
                sum = (sum + buff[i]);

            return sum;
        }



        private bool send_expect(byte[] send, int send_length, string expect, byte[] response, int response_length, int timeout)
        {
            byte[] in_buff;
            int i;
            int local_timeout;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (!serialPort1.IsOpen)
                return false;

            try
            {
                serialPort1.DiscardInBuffer();
                serialPort1.ReadTimeout = timeout;
                serialPort1.Write(send, 0, send_length);
                inbuff_ptr = 0;
            }
            catch (IOException e)
            {
                MessageBox.Show("An error occured while accessing the serial port:\n" + e.Message);
//                t_check_rs232_send_flags.CancelAsync();
                Thread.Sleep(1000);
                close_port();

                return false;
            }
            try
            {
                if (response_length > 0)
                {
                    int k = 0;
                    int refTick = System.Environment.TickCount;
                    do
                    {
                        Thread.Sleep(1);
                        Application.DoEvents();
                        k++;
//                        Debug.WriteLine(k.ToString());
                    } while (inbuff_ptr < response_length && refTick + timeout > System.Environment.TickCount);


                    if (inbuff_ptr < response_length)
                    {
//                        Debug.WriteLine("send_expect timeout.");
                        return false;
                    }
                    //serialPort1.ReadTimeout = timeout_parameter_read;
                    //serialPort.Read(response, i, response_length - i);
                    // copy the last x bytess to inbuff
                    //serialPort1.Read(response, 0, response_length);
                    for (i = 0; i < response_length; i++)
                    {
                        response[i] = inbuff[i];
                    }

                    return true;
                }
                else if (expect.Length > 0)
                {
                    int refTick = System.Environment.TickCount;
                    do
                    {
                        Thread.Sleep(1);
                        Application.DoEvents();
                    } while (inbuff_ptr < expect.Length && refTick + timeout_parameter_read > System.Environment.TickCount);
                    
                    String s_temp = enc.GetString(inbuff, 0, expect.Length);
//                    Debug.WriteLine("got:" + s_temp + "  expected: " + expect + "pointer = " + inbuff_ptr.ToString());

                    return (s_temp.Contains(expect));
                }
                else
                    return true;
            }
            catch (System.IO.IOException e)
            {
                n_err_count++;
                return false;
            }
            catch (System.TimeoutException e)
            {
                n_err_count++;
                return false;
            }
            catch
            {
                n_err_count++;
                return false;
            }
        }

        private Boolean LoadParametersControl()
        {
            int i, j;
            int cur_x, cur_y; // For positioning the dynamic allocated controls.
            int max_width; // Current examained column max width.
            ArrayList column_width = new ArrayList();
            parameter cParameter;
            Boolean FoundNewColumn;

            c_parameters = new Parameters(parameters_filename);
            if (c_parameters.f_load_error)
                return false;

            arr_timer_read_pages = new ArrayList();
            memory_map = new byte[memory_size];

            n_err_count = 0;

            cur_x = gui_x_start;
            cur_y = gui_y_start;
            // Calculate columns width by checking the text length of each label.
            // for (i = 0; i < c_parameters.params_arr.Count; i += gui_parameters_per_column)

            i = 0;
            while (i < c_parameters.params_arr.Count)
            {
                max_width = 0;
                j = 0;
                FoundNewColumn = false;
                while (i + j < c_parameters.params_arr.Count && !FoundNewColumn)
                {
                    cParameter = (parameter)c_parameters.params_arr[i + j];
                    Debug.WriteLine(cParameter.s_label);
                    FoundNewColumn = cParameter.b_display_on_pc && cParameter.c_type == 'N';
                    if (!FoundNewColumn && cParameter.b_display_on_pc)
                    {
                        if (max_width < cParameter.label_width)
                            max_width = cParameter.label_width;
                    }
                    else if (cParameter.b_display_on_pc)
                    {
                        j++;
                        j--;
                    }
                    j++;
                }
                j++; // Skip the "/N" Line.
                // column_width.Add(max_width);
                column_width.Add(gui_label_width);
                i += j;
            }

            // Create an array of controls of label, textbox and an updown control
            j = 0;
            for (i = 0; i < c_parameters.params_arr.Count; i++)
            {
                cParameter = (parameter)c_parameters.params_arr[i];

                if (!cParameter.b_display_on_pc)
                    continue;

                if (cParameter.c_type != 'N') // Not a sign for a new column
                {
                    // Add the caption to the folder's controls
                    cParameter.caption.Visible = true;
                    cParameter.caption.SetBounds(cur_x, cur_y, (int)column_width[j], gui_vertical_space);
                    //FLDRMonitor.Controls.Add(cParameter.caption);
                }
                else
                    j++;
                if (cParameter.c_type != 'C' && cParameter.c_type != 'E' && cParameter.c_type != 'N')
                {
                    // Add mouse event to activate text focus
                    // cParameter.value.MouseClick += new MouseEventHandler(value_MouseClick);

                    // Add the textbox to the folder's controls
                    cParameter.value.Visible = true;
                    cParameter.value.SetBounds(cur_x + (int)column_width[j] + gui_space_between_controls, cur_y, gui_textbox_width, gui_vertical_space);
                    //FLDRMonitor.Controls.Add(cParameter.value);
                    // Add the numeric updown control
                    //                cParameter.nud.Visible = true;
                    //                cParameter.nud.SetBounds(cur_x + (int)column_width[i / gui_parameters_per_column] +  cParameter.value.Width + gui_space_between_controls, cur_y, gui_nud_width, gui_vertical_space);
                    //                FLDRMonitor.Controls.Add(cParameter.nud);

                }
                cur_y = cur_y + gui_total_vertical_space;

                //                if (((i + 1) % gui_parameters_per_column) == 0) // Recalculate positions if a column is changed.
                //                {
                //                    cur_x = cur_x + (int)column_width[i / gui_parameters_per_column] + gui_textbox_width + gui_horizontal_space;
                //                    cur_y = gui_y_start;
                //                }
                if (cParameter.c_type == 'N') // Recalculate positions if a column is changed.
                {
                    // cur_x = cur_x + (int)column_width[i / gui_parameters_per_column] + gui_textbox_width + gui_horizontal_space;
                    cur_x = cur_x + (int)column_width[j - 1] + gui_textbox_width + gui_horizontal_space;
                    cur_y = gui_y_start;
                }



            }
            return true;
        }


        private void UnLoadParametersControl()
        {
            int i;
            parameter param;

            // Free the parameters

            foreach (page p in c_parameters.mem_pages)
            {
                p.tmr_page_refresh.Enabled = false;
            }

            for (i = 0; i < c_parameters.params_arr.Count; i++)
            {
                param = (parameter)c_parameters.params_arr[i];
                if (!param.b_display_on_pc)
                {
                    param = null;
                    continue;
                }
                //                FLDRMonitor.Controls.Remove(param.caption);
                //                FLDRMonitor.Controls.Remove(param.value);
                param.caption = null;
                param.value = null;
                param = null;
            }
            c_parameters = null;
        }

        Boolean IsFileExist(String FilePath)
        {
            FileInfo fi = new FileInfo(FilePath);
            return fi.Exists;
        }

        Boolean IsFileWriteable(String FilePath)
        {
            FileStream fs;

            try
            {
                fs = File.Open(FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException e1)
            {
                return false;
            }
            fs.Close();
            return true;
        }

        private void TMRCheckFrames_Tick(object sender, EventArgs e)
        {
            int i;

            if (downloading)
                return;
            
            Boolean checked_buff = false;
            TMRCheckFrames.Enabled = false;
            Debug.WriteLine(inbuff_ptr.ToString());

            lock (lock_inbuff)
            {
                // Check buffer validity
                while (!checked_buff && inbuff_ptr > 0)
                {
                    // Check for a correct header
                    i = 0;
                    while (i < inbuff_ptr && inbuff[i] != 0xaa) // 
                        i++;

                    if (i > 0)
                        reset_buff(i);

                    // Check the message type
                    if (inbuff_ptr >= 2 && inbuff[2] != 'D' && inbuff[2] != 'V' && inbuff[2] != 'T' && inbuff[2] != 'S')
                        reset_buff(3);
                    else
                    {
                        checked_buff = true;
                        if (inbuff[2] != 'V')
                            cDataChar = (char)inbuff[2];
                    }


                    // Check if we've got a full frame
                    if (inbuff_ptr >= 1 && inbuff_ptr >= (inbuff[1] + 2))
                    {
                        // Check the length
//                        if (inbuff[1] != 7 + inbuff[5])
//                        {
//                            reset_buff(1);
//                            checked_buff = false;
//                        }
                        // Check the checksum
// else
                        if (!check_checksum(inbuff))
                        {
                            reset_buff(1);
                            checked_buff = false;
                        }
                        else
                        {
                            dispatch_packet();
                            reset_buff(inbuff[1] + 2);
                            checked_buff = true;
                            Debug.WriteLine(inbuff_ptr.ToString());
                        }
                    }
                    else
                        checked_buff = true;
                }
            }
            TMRCheckFrames.Enabled = true;
        }

        private bool check_checksum(byte[] inbuff)
        {
            try
            {
                long checksum = inbuff[inbuff[1] - 1] * 256 + inbuff[inbuff[1]];
                long calced_checksum = get_checksum(inbuff, inbuff[1] - 1);
                return (calced_checksum == checksum || calced_checksum == checksum - 1);
            }
            catch
            {
                return false;
            }
        }

        private void TMRRefreshMem_Tick(object sender, EventArgs e)
        {
            int i = 0;
            byte[] frame;
            try
            {

                if (downloading || !serialPort1.IsOpen)
                    return;
                TMRRefreshMem.Enabled = false;

                #region "ram"
                serialPort1.Write(cmd_read_all_ram, 0, cmd_read_all_ram.Length);
                Thread.Sleep(20);

                Debug.WriteLine("Sending a read request.");
                // Status and errors
                Double version = Convert.ToDouble((double)memory_map[0x01bb]) / 100.0;

                LBLVersion.Text = version.ToString("0#.#0");

                int temp = memory_map[0x1bd];
                if (temp == 32)
                {
                    if (LBLRegCorruption.Text != "OK")
                    {
                        LBLRegCorruption.Text = "OK";
                        LBLRegCorruption.ForeColor = Color.Green;
                    }
                }
                else if (LBLRegCorruption.Text != temp.ToString())
                {
                    LBLRegCorruption.Text = temp.ToString();
                    LBLRegCorruption.ForeColor = Color.Red;
                }

                #endregion

                #region "eeprom"
                serialPort1.Write(cmd_read_all_eeprom, 0, cmd_read_all_eeprom.Length);
                Thread.Sleep(20);

                // WDT events
                LBLWDTEvents.Text = memory_map[0x8006].ToString();

                // Serial number
                if (!TXTSerialNumber.Focused && !do_set_serial)
                {
                    TXTSerialNumber.Text = enc.GetString(memory_map, 0x8000, 6);
                }


                #endregion

                #region "send errors"
                // WDT and checksum errors
                if (do_clear_wdt)
                {
                    do_clear_wdt = false;
                    frame = create_write_frame(0x8006, 0);
                    serialPort1.Write(frame, 0, frame.Length);
                    Thread.Sleep(100);
                }
                if (do_wdt_error)
                {
                    do_wdt_error = false;
                    serialPort1.Write(cmd_do_wdt_error, 0, cmd_do_wdt_error.Length);
                    Thread.Sleep(20);
                }
                if (do_checksum_error)
                {
                    do_checksum_error = false;
                    serialPort1.Write(cmd_do_chksum_error, 0, cmd_do_chksum_error.Length);
                    Thread.Sleep(20);
                }

                if (do_load_defaults)
                {
                    do_load_defaults = false;
                    frame = create_write_frame(0x1be, 1);
                    serialPort1.Write(frame, 0, frame.Length);
                    Thread.Sleep(100);
                }

                if (do_set_serial) // Set the serial number
                {
                    do_set_serial = false;
                    for (i = 0; i < serial.Length; i++)
                    {
                        frame = create_write_frame(0x8000 + i, (Byte)serial[i]);
                        serialPort1.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                    }
                    // Right pad with zeroes.
                    for (i = serial.Length; i < 6; i++)
                    {
                        frame = create_write_frame(0x8000 + i, (Byte)0);
                        serialPort1.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                    }
                }

                #endregion
            }
            catch (Exception e1)
            {
                show_error(e1);
            }
                TMRRefreshMem.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            do_wdt_error = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            do_checksum_error = true;
        }

        private void TXTSerialNumber_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                label32.Focus();
                e.Handled = true;
            }

        }

        private void TXTSerialNumber_Validated(object sender, EventArgs e)
        {
            serial = TXTSerialNumber.Text;
            do_set_serial = true;
        }

        private void TXTSerialNumber_Enter(object sender, EventArgs e)
        {
            TXTSerialNumber.SelectAll();
        }

        private void TXTSerialNumber_Click(object sender, EventArgs e)
        {
            TXTSerialNumber.SelectAll();
        }

        private Byte[] create_write_frame(long address, byte value)
        {
            byte[] new_cmd = { 0xAA, 0x07, 0x57, 0x00, 0x00, 0x00, 0x00, 0x00, 0xCC };
            new_cmd[3] = (byte)(address / 256);
            new_cmd[4] = (byte)(address % 256);
            new_cmd[5] = value;

            Int16 checksum = 0;
            for (int i = 0; i <= 5; i++) // Get the checksum of the first 6 bytes.
                checksum += new_cmd[i];

            new_cmd[6] = (byte)(checksum / 256);
            new_cmd[7] = (byte)(checksum % 256);
            return new_cmd;
        }

        private Byte[] create_write_frame(long address, String value, int length)
        {
            int i = 0;
            int ul = 0;
            byte[] new_cmd = new byte[8 + length];
            new_cmd[0] = 0xaa;
            new_cmd[1] = 0x07;
            new_cmd[2] = 0x57;
            new_cmd[8 + length - 1] = 0xcc;

            new_cmd[3] = (byte)(address / 256);
            new_cmd[4] = (byte)(address % 256);
            if (value.Length <= length)
                ul = value.Length;
            else
                ul = length;

            for (i = 0; i < ul; i++)
                new_cmd[5 + i] = (byte)value[i];

            for (i = ul; i < length; i++) // Feel the rest with nulls.
                new_cmd[5 + i] = 0;

            Int16 checksum = 0;
            for (i = 0; i <= 4 + length; i++) // Get the checksum of the first bytes.
                checksum += new_cmd[i];

            new_cmd[8 + length - 3] = (byte)(checksum / 256);
            new_cmd[8 + length - 2] = (byte)(checksum % 256);
            return new_cmd;
        }



        private void BTNLoadDefaults_Click(object sender, EventArgs e)
        {
            do_load_defaults = true;
        }

        private void CMDDownload_Click(object sender, EventArgs e)
        {
            download_file();
        }

        private void TMRNoCommDetection_Tick(object sender, EventArgs e)
        {
            pic_bw_background.Visible = true;
        }

        private void TXTFileName_Validated(object sender, EventArgs e)
        {
            DLGFILEFirmwareUpdte.FileName = TXTFileName.Text;
            SensorScope.Default.Setting_FirmFileName = TXTFileName.Text;
            SensorScope.Default.Save();
        }



        private void BTNRstWdt_Click(object sender, EventArgs e)
        {
            do_clear_wdt = true;
        }
    }
}