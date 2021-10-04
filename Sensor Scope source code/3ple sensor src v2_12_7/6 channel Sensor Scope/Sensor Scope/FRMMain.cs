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
using System.IO.Ports;



namespace Sensor_Scope
{
    public partial class FRMMain : Form
    {
        System.Text.ASCIIEncoding enc = new ASCIIEncoding();
        
        String serial;
        int nHourCounter;

        cComm activePort = null;

        const int buffsize = 1024;
        const int memory_size = 64 * buffsize; // Memory size of the device
        const int input_buff_size = 16 * buffsize; // 16k memory for input buffer
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



        
        bool downloading = false; // Mark that we are in download proccess, and communication flags should not be checked.

        

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
        public Int32 StrainGage;
        float mid;
        float span;
        int avg;
        Bitmap graph = new Bitmap(800, 512);
        float last_x;
        float last_y;
        float current_x;
        float current_y;
        float avg_sum;
        float avg_count;
        bool autocenter = false;
        FileStream[] sw = new FileStream[6];
        long checksum_error_counter = 0;
        long packets = 0;
        long timeouts = 0;
        // long missing_packets = 0;
        long last_packet_number = -1;
        System.Threading.Timer tt_comm_reset;
        System.Threading.TimerCallback tt_comm_reset_cb;
        public Queue quwPixels2draw = new Queue();
        public object lockQue = new object();

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

        Control[,] arPortCtrlLine = new Control[6, 5];
        cComm[] arPorts = new cComm[6];
        int numPorts; // How many ports are active
        int nLastActivePort = 0;

        enum DYN_CONTROLS_POS
        {
            RAD,
            PORT,
            OPEN,
            CLOSE,
            LOST
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            Boolean is_filename_ok = false;
            nLastActivePort = SensorScope.Default.LastActivePort;
            

            // Build control array for easy access
            
            for (i = 0; i < 6; i++)
            {
                String sRadio = "radioButton" + i.ToString();
                String sLabel = "lblPort" + i.ToString();
                String sPort = "CMBPortSelector" + i.ToString();
                String sOpen = "RADOpen" + i.ToString();
                String sClose = "RADClose" + i.ToString();
                String sLblLost = "lblLost" + i.ToString();

                arPortCtrlLine[i, (int)DYN_CONTROLS_POS.RAD] = this.panel2.Controls[sRadio];
                arPortCtrlLine[i, (int)DYN_CONTROLS_POS.PORT] = this.Controls[sPort];
                arPortCtrlLine[i, (int)DYN_CONTROLS_POS.OPEN] = this.Controls[sOpen];
                arPortCtrlLine[i, (int)DYN_CONTROLS_POS.CLOSE] = this.Controls[sClose];
                arPortCtrlLine[i, (int)DYN_CONTROLS_POS.LOST] = this.Controls[sLblLost];
                arPorts[i] = new cComm("COM0", 57600, Parity.None, 8, StopBits.One,c_parameters,this);

                if (i == nLastActivePort)
                    ((RadioButton)arPortCtrlLine[i, 0]).Checked = true;
            }
            for (i = 0; i < 6; i++)
                for (int j = 0; j < 5; j++)
                    arPortCtrlLine[i, j].Tag = i;

            numPorts = SensorScope.Default.NumOfPorts;
            numericUpDown1.Value = numPorts;
            refreshPortList();

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
            for (int j = 0; j < 6; j++)
            {
                ((Label)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.LOST]).Text = "-";
                ((Label)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.LOST]).ForeColor = Color.Black;

                String tmpPort_name = "COM" + SensorScope.Default["Port" + j.ToString()].ToString();
                i = ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).Items.IndexOf(tmpPort_name);
                if (tmpPort_name.Length > 3 && tmpPort_name != "" && ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).Items.IndexOf(tmpPort_name) < 0)
                {
                    ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).Items.Add(tmpPort_name);
                    ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).SelectedItem = tmpPort_name;
                    arPorts[j].portName = tmpPort_name;
                }
                else if (i > 0)
                {
                    try
                    {
                        ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).SelectedIndex = i;
                        arPorts[j].portName = ((ComboBox)arPortCtrlLine[j, (int)DYN_CONTROLS_POS.PORT]).SelectedItem.ToString();
                    }
                    catch (Exception e1)
                    {
                        MessageBox.Show(e1.Message + "  " + tmpPort_name + " " + i.ToString());
                    }
                }
            }
            UpdateGraphBackground();
        }


        private void refreshPortList()
        {
            int nLastActivePort = 0;


            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    arPortCtrlLine[i, j].Visible = i < numericUpDown1.Value;
                }
                if (arPortCtrlLine[i, 0].Visible)
                    nLastActivePort = i;
            }
            if (numPorts > nLastActivePort + 1)
                numPorts = nLastActivePort + 1;
            //((RadioButton)arPortCtrlLine[numPorts, 0]).Checked = true;
        }

        private void GetPorts()
        {
            string[] theSerialPortNames = System.IO.Ports.SerialPort.GetPortNames();

            for (int i = 0; i < 6; i++)
            {
                ((ComboBox)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.PORT]).Items.Clear();
                ((ComboBox)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.PORT]).Items.AddRange(theSerialPortNames);
            }
        }

        private void CMBPortSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            // close_port();
            ComboBox cmb = (ComboBox)sender;
            port_name = cmb.SelectedItem.ToString();
            int portNum = int.Parse(port_name.Substring(3));
            //serialPort1.PortName = port_name;
            String settingsName = "Port" + cmb.Name.Substring(cmb.Name.Length - 1).ToString();
            SensorScope.Default[settingsName] = portNum.ToString();
            arPorts[(int)cmb.Tag].portName = port_name;
            SensorScope.Default.Save();
        }

        private void RADOpen_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            int index = (int)rad.Tag;
            cComm comm = arPorts[index];
            RadioButton radClose = (RadioButton)arPortCtrlLine[index,(int) DYN_CONTROLS_POS.CLOSE];
            if (rad.Checked)
            {
                try
                {
                    comm.resetPort();
                    comm.Open();
                    
                    if (! comm.isOpen)
                    {
                        radClose.Checked = true;
                        return;
                    }
                    rad.Checked = false;
                    rad.BackColor = Color.DarkGreen;
                    radClose.BackColor = Color.IndianRed;
                    TSPortStatus.Text = "Port is open.";
                    TSPortStatus.BackColor = Color.LimeGreen;
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Can't open the port:\n" + e1.Message);
                    radClose.Checked = true;
                }
                
            }
        }

        private void RADClose_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rad = (RadioButton)sender;
            int index = (int)rad.Tag;
            RadioButton radOpen = (RadioButton)arPortCtrlLine[index, (int)DYN_CONTROLS_POS.OPEN];

            if (rad.Checked)
            {
                cComm serialPort = arPorts[index];
                serialPort.Close();
                radOpen.BackColor = Color.Lime;
                rad.BackColor = Color.DarkRed;
                TSPortStatus.Text = "Port is closed.";
                TSPortStatus.BackColor = Color.IndianRed;
            }
        }

        private void TMRDateAndTime_Tick(object sender, EventArgs e)
        {
            if (activePort != null) Debug.WriteLine(activePort.downloading.ToString());
            String sDataChars = "";

            this.TMRDateAndTime.Enabled = false;
            if (DateTime.Now.TimeOfDay.ToString().Length > 8)
                TSTime.Text = DateTime.Now.TimeOfDay.ToString().Remove(8);
            else
                TSTime.Text = DateTime.Now.TimeOfDay.ToString();
            TSDate.Text = DateTime.Now.Date.ToShortDateString();

            TSBadChecksum.Text = "   Bad checksums: " + checksum_error_counter.ToString() + "   "; 
            // TSMissingsPackets.Text = "   Missing packets: " + missing_packets.ToString() + "   "; 
            LBLPackets.Text = packets.ToString();
            TSTimeouts.Text = "   Timeouts: " + timeouts.ToString() + "   ";

            for (int i = 0; i < 6; i++)
            {
                if (arPorts[i] != null)
                {
                    ((Label)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.LOST]).Text = arPorts[i].lostFrames.ToString();
                    if (arPorts[i].cDataChar != ' ' && !sDataChars.Contains(arPorts[i].cDataChar.ToString()))
                        sDataChars += arPorts[i].cDataChar.ToString();
                }
            }
            lblDataChar.Text = sDataChars;

            if (activePort != null && activePort.downloading)
                this.progressBar1.Value = activePort.nPCurrentPage;
            else
                this.progressBar1.Value = 0;

            this.TMRDateAndTime.Enabled = true;
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
            last_x = -1;
            PICGraph.Image = graph;
        }

        private void CHKSaveToFile_CheckedChanged(object sender, EventArgs e)
        {
            DialogResult rc;

            if (CHKSaveToFile.Checked)
            {
                
                for (int i = 0;i<numericUpDown1.Value;i++)
                    if (!arPorts[i].isOpen)
                    {
                        rc = MessageBox.Show("Not all ports are open.\nOpen all?", "Open all ports?", MessageBoxButtons.YesNoCancel);
                        if (rc == DialogResult.Yes)
                            btnOpenAll_Click(null, null);
                        else if (rc == DialogResult.Cancel)
                        {
                            CHKSaveToFile.Checked = false;
                            return;
                        }
                    }
                numericUpDown1.Enabled = false;
                rc = saveFileDialog1.ShowDialog();
                if (rc != DialogResult.OK)
                {
                    CHKSaveToFile.Checked = false;
                    return;
                }
                String sPath = Path.GetDirectoryName(saveFileDialog1.FileName);
                String sPrefix = Path.GetFileNameWithoutExtension(saveFileDialog1.FileName);
                String sExt = Path.GetExtension(saveFileDialog1.FileName);
                
                for (int i = 0; i < numPorts; i++)
                {
                    String sFileName = sPath + "\\" + sPrefix + i.ToString() + (sExt == "" ? "" : "." + sExt);
                    arPorts[i].sw = new FileStream(sFileName, FileMode.Create);
                }
            }
            else
            {
                closeFiles();
                numericUpDown1.Enabled = true;
            }

        }

        private void closeFiles()
        {
            for (int i = 0; i < numPorts; i++)
            {
                if (sw[i] != null)
                {
                    arPorts[i].closeFile();
                }
            }
        }

        private void FRMMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            closeFiles();
            close_all_ports();
        }

        private void close_all_ports()
        {
            for (int i = 0; i < 6; i++)
                if (arPorts[i].isOpen)
                {
                    try
                    {
                        arPorts[i].Close();
                    }
                    catch { };
                }
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
            activePort.fileName = TXTFileName.Text;
            if (activePort.download_file(false, true) == cComm.DOWNLOAD_RESULTS.NO_ERROR)
            {
                CMDCalcChecksum.Text = "$" + Convert.ToString(activePort.global_checksum, 16);
                TMRHideChecksum.Interval = 2000;
                TMRHideChecksum.Enabled = true;
            }

        }

        private int get_index()
        {
            for (int i = 0; i < 6; i++)
                if (((RadioButton)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.RAD]).Checked)
                    return i;
            return -1;
        }

        private void TMRHideChecksum_Tick(object sender, EventArgs e)
        {
            TMRHideChecksum.Enabled = false;
            CMDCalcChecksum.Text = "Calc Checksum";
            CMDDownload.Enabled = true;
        }








        private void open_port(SerialPort serialPort1)
        {

            if (serialPort1.IsOpen)
                return;

            try
            {
                serialPort1.Open();
            }
            catch (Exception e1)
            {
                MessageBox.Show("Error while opening the serial port :\n" + e1.Message);
            }
            //            update_status_bar_and_menu();
        }

        private void close_port(SerialPort serialPort1)
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
//                    Debug.WriteLine(cParameter.s_label);
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



        private void TMRRefreshMem_Tick(object sender, EventArgs e)
        {

            int i = 0;
            try
            {
                if (activePort == null || activePort.downloading || !activePort.isOpen)
                    return;
                
                TMRRefreshMem.Enabled = false;
                lock (activePort.lock_memoryMap)
                {

                    #region "ram"
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

                    // WDT events
                    LBLWDTEvents.Text = memory_map[0x8006].ToString();

                    // Serial number
                    if (!TXTSerialNumber.Focused && !activePort.do_set_serial)
                    {
                        TXTSerialNumber.Text = enc.GetString(memory_map, 0x8000, 6);
                    }

                    // Hours counter
                    if (!txtHourCounter.Focused && !activePort.do_set_hour_counter)
                    {
                        txtHourCounter.Text = Convert.ToString(memory_map[0x8007] * 256 + memory_map[0x8008]);
                    }


                    #endregion
                }
            }
            catch (Exception e1)
            {
                cErrorHandler.show_error(e1);
            }
                TMRRefreshMem.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (activePort!= null)
                activePort.do_wdt_error = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (activePort != null && activePort.isOpen)
                activePort.do_checksum_error = true;
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
            if (activePort != null && activePort.isOpen)
            {
                activePort.do_set_serial = true;
                serial = TXTSerialNumber.Text;
            }
        }

        private void TXTSerialNumber_Enter(object sender, EventArgs e)
        {
            TXTSerialNumber.SelectAll();
        }

        private void TXTSerialNumber_Click(object sender, EventArgs e)
        {
            TXTSerialNumber.SelectAll();
        }




        private void BTNLoadDefaults_Click(object sender, EventArgs e)
        {
            if (activePort != null && activePort.isOpen)
                activePort.do_load_defaults = true;
        }

        private void CMDDownload_Click(object sender, EventArgs e)
        {
            if (activePort != null)
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = (int)activePort.lPages;
                progressBar1.Refresh();

                activePort.fileName = TXTFileName.Text;
                activePort.download_file();
            }
        }

        private void TMRNoCommDetection_Tick(object sender, EventArgs e)  
        {
            if (activePort == null || (activePort != null && (Environment.TickCount - activePort.lastFrame) > 1000 && activePort.isOpen))
                pic_bw_background.Visible = true;
            else if (activePort != null && activePort.isOpen)
                pic_bw_background.Visible = false;
        }

        private void TXTFileName_Validated(object sender, EventArgs e)
        {
            DLGFILEFirmwareUpdte.FileName = TXTFileName.Text;
            SensorScope.Default.Setting_FirmFileName = TXTFileName.Text;
            SensorScope.Default.Save();
        }

        private void BTNRstWdt_Click(object sender, EventArgs e)
        {
            if (activePort != null && activePort.isOpen)
                activePort.do_clear_wdt = true;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            refreshPortList();
            numPorts = (int)numericUpDown1.Value;
            SensorScope.Default.NumOfPorts = numPorts;
            SensorScope.Default.Save();
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;

            if (rb.Checked)
            {
                int portNum = int.Parse(rb.Name.Substring(rb.Name.Length - 1));
                SensorScope.Default.LastActivePort = portNum;
                nLastActivePort = portNum;
                SensorScope.Default.Save();
                if (activePort != null)
                {
                    lock (activePort.lock_memoryMap)
                    {
                        activePort.memory_map = null;
                    }
                }
                activePort = arPorts[portNum];
                lock (activePort.lock_memoryMap)
                {
                    activePort.memory_map = memory_map;
                }
            }
        }

        private void btnOpenAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                RadioButton rad = (RadioButton)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.OPEN];
                if (!arPorts[i].isOpen)
                    rad.Checked = true; // Will cause an open command .
            }
        }

        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                RadioButton rad = (RadioButton)arPortCtrlLine[i, (int)DYN_CONTROLS_POS.CLOSE];
                if (arPorts[i].isOpen)
                    rad.Checked = true;
            }
        }

        private void txtHourCounter_Click(object sender, EventArgs e)
        {
            txtHourCounter.SelectAll();
        }

        private void txtHourCounter_Enter(object sender, EventArgs e)
        {
            txtHourCounter.SelectAll();
        }

        private void txtHourCounter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                label32.Focus();
                e.Handled = true;
            }
        }

        private void txtHourCounter_Validated(object sender, EventArgs e)
        {

            try
            {
                if (activePort != null && activePort.isOpen)
                {
                    activePort.nHourCounter = int.Parse(txtHourCounter.Text.Trim());
                    activePort.do_set_hour_counter = true;
                }
            }
            catch { };
        }

        void showThreadPoolInfo()
        {
            int d, i1;
            ThreadPool.GetAvailableThreads(out d, out i1);
            Debug.WriteLine(d + "   " + i1);
        }

        private void tmrRefreshGraph_Tick(object sender, EventArgs e)
        {
            tmrRefreshGraph.Enabled = false;
            lock (quwPixels2draw)
            {
                while (quwPixels2draw.Count > 0)
                {
                    Int32 v = (Int32)quwPixels2draw.Dequeue();
                    avg_sum += v;
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
                        draw_pixel(current_x, current_y);
                        check_peaks(v, Convert.ToInt32(current_x), Convert.ToInt32(current_y));
                        current_x = ((current_x + 1) % PICGraph.Width);
                        n_on_screen_vals[(int)current_x] = Convert.ToInt64(value);
                        n_on_screen_y[(int)current_x] = Convert.ToInt64(current_y);
                        avg_sum = avg_count = 0;
                        String sTmp = StrainGage.ToString();
                        sTmp = sTmp.PadLeft(16-sTmp.Length,' ');
                        TSStrain.Text = "strain gage: " + sTmp;
                    }
                }
            }
            find_min_max();
            tmrRefreshGraph.Enabled = true;
        }

    }
}