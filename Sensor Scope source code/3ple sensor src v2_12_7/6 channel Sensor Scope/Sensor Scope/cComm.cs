using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using System.IO;
using SensorScope;
using System.Diagnostics;

namespace Sensor_Scope
{
    class cComm
    {

        // Comm packets
        public Boolean do_wdt_error = false;
        public Boolean do_checksum_error = false;
        public Boolean do_set_serial = false;
        public Boolean do_load_defaults = false;
        public Boolean do_clear_wdt = false;
        public Boolean do_set_hour_counter = false;
        public int nHourCounter = 0;
        String serial = "";
        public char cDataChar = ' '; // The data char received from the sensor.
        public int nPCurrentPage = 0; // For progress bar update.

        // Comm packets
        byte[] cmd_read_all_ram = { 0xaa, 0x07, 0x52, 0x01, 0xbb, 0x03, 0x01, 0xc2, 0xcc };
        byte[] cmd_read_all_eeprom = { 0xaa, 0x07, 0x52, 0x80, 0x00, 0x09, 0x01, 0x8c, 0xcc };
        byte[] cmd_do_wdt_error = { 0xAA, 0x07, 0x57, 0x01, 0xbc, 0x01, 0x01, 0xc6, 0xCC };
        byte[] cmd_do_chksum_error = { 0xAA, 0x07, 0x57, 0x01, 0xBC, 0x02, 0x01, 0xC7, 0xCC };

        
        // Privates
        FRMMain frmMain = null;
        private SerialPort serialPort;
        int inbuff_ptr = 0;
        const int buffsize = 1024;
        object lock_inbuff = new object();
        public object lock_memoryMap = new object();
        Byte[] inbuff;
        byte[] frame;
        long  _lastFrame;
        public long lastFrame
        {
            get
            {
                return _lastFrame;
            }
        }

        int firstPacketCounter = -1;
        Timer tmr500ms = null;
        TimerCallback tmr500msCb;
        Timer tmrCheckFrames = null;
        TimerCallback tmrCheckFramesCb;


        public FileStream sw = null;
        public void closeFile()
        {
            sw.Flush();
            sw.Close();
            sw = null;
        }

        byte[] hex_buf;
        // Board modes.
        int current_mode = 0;
        const int CM_NO_RESPONSE = -1;
        const int CM_RUNNING = 0;
        const int CM_DOWNLOADING = 1;
        const int CM_OFFLINE = 2;
        const int CM_ONLINE = 3;
        Parameters c_parameters;


        // Firmware update stuff
        int firmware_version; // Version as recieved from the board.

        const long PAGES = 120;
        const long PAGE_SIZE = 128;
        const long buff_size = PAGES * PAGE_SIZE;
        const int ack_timeout = 1000;
        public long lPages = PAGES;
        
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



        // Properties
        long _global_checksum = 0;
        public long global_checksum
        {
            get
            {
                return _global_checksum;
            }
        }

        Boolean _downloading = false;
        public Boolean downloading
        {
            get
            {
                return _downloading;
            }
        }

        String _fileName = "";
        public String fileName
        {
            set
            {
                _fileName = value;
            }
        }


        byte[] _memory_map = null;
        public byte[] memory_map
        {
            set
            {
                _memory_map = value;
            }
        }


        public Boolean isOpen
        {
            get
            {
                return this.serialPort.IsOpen;
            }
        }

        int _packets = 0;
        public int packets
        {
            get
            {
                return _packets;
            }
        }


        int _lostFrames;
        public int lostFrames
        {
            get
            {
                return _lostFrames;
            }

        }

        // port name
        public String portName 
        {
            get
            {
                return serialPort.PortName;
            }
            set
            {
                if (serialPort.IsOpen)
                    return;
                serialPort.PortName = value;
            }
        }

        
        public cComm(String portName, int baudRate, Parity parity, int dataBits, StopBits stopBits,Parameters icParameters,FRMMain iFrmMain)
        {
            this.c_parameters = icParameters;
            serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
            inbuff = new byte[buffsize];
            this.frmMain = iFrmMain;
        }

        ~cComm()
        {
            if (this.serialPort.IsOpen)
                this.serialPort.Close();
        }

        private void serialPort_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int n_bytes = serialPort.BytesToRead;
                if (inbuff_ptr + n_bytes > buffsize)
                {
                    serialPort.DiscardInBuffer();
                    inbuff_ptr = 0;
                    return;
                }
                lock (lock_inbuff)
                {
                    serialPort.Read(inbuff, inbuff_ptr, n_bytes);
                    inbuff_ptr += n_bytes;
                }
            }
            catch (Exception e1)
            {
                cErrorHandler.show_error(e1);
            }
        }

        public void resetPort()
        {
            inbuff_ptr = 0;
            this._lostFrames = 0;
            _lastFrame = Environment.TickCount;
        }

        public Boolean Open()
        {
            if (this.serialPort.IsOpen)
                return false;


            try
            {
                this.serialPort.Open();
                // Activate polling timers
                tmrCheckFramesCb = new TimerCallback(checkFrames);
                tmrCheckFrames = new Timer(tmrCheckFramesCb);
                tmrCheckFrames.Change(10, 10);
                tmr500msCb = new TimerCallback(checkStatus);
                tmr500ms = new Timer(tmr500msCb);
                tmr500ms.Change(500, 500);
                return this.serialPort.IsOpen;
            }
            catch (Exception e)
            {
                cErrorHandler.show_error(e);
                return false;
            }
        }

        public void Close()
        {
            if (tmrCheckFrames != null)
                tmrCheckFrames.Change(Timeout.Infinite,Timeout.Infinite);
            if (tmr500ms != null)
                tmr500ms.Change(Timeout.Infinite, Timeout.Infinite);
            Thread.Sleep(100);
            this.serialPort.Close();
            return;
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
            long start = System.Environment.TickCount;
            long tick = start;
            long diff = Math.Abs(_lastFrame - tick);

            if (diff > 187)
            {
                _lostFrames++;
            }
            _lastFrame = tick;
            Byte temp_byte1, b1, b2, b3;
            Int32 checksum, my_checksum;
            int i;
            Int32[] values = new Int32[30];

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
                // Get the strain gage
                b1 = inbuff[96];
                b2 = inbuff[97];
                b3 = inbuff[98];
                frmMain.StrainGage = b1 * 65536 + b2 * 256 + b3;

                // Ok, we got a packet. Let's roll.
                _packets++;
                if (sw != null)
                {
                    if (firstPacketCounter == -1) // this is our first packet
                        firstPacketCounter = (int)inbuff[5];
                    int nPacketNum = (int)inbuff[5] - firstPacketCounter;
                    if (nPacketNum < 0)
                        nPacketNum += 256;
                    Byte bTmp = (Byte)(nPacketNum & 0xff);
                    sw.WriteByte(bTmp);
                    for (i = 0; i < 4; i++)
                    {
                        bTmp = (byte)((tick & (0xff << (i * 8))) / Math.Pow(2, i * 8));
                        sw.WriteByte(bTmp);
                    }
                }
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
                    if (_memory_map != null) 
                    {
                        lock (frmMain.quwPixels2draw)
                            frmMain.quwPixels2draw.Enqueue(values[i]);
                    }
                }
            }

            #endregion

            #region "ram"
            else if (address == "1bb") // Got a sample frame
            {
                Array.Copy(inbuff, 6, _memory_map, 0x1bb, 3);
            }
            #endregion


            #region "eeprom"
            else if (address == "8000") // Got a sample frame
            {
                Array.Copy(inbuff, 6, _memory_map, 0x8000, 9);
            }
            #endregion
        }

        public void download_file()
        {
            Thread t = new Thread (doDownload);          // Kick off a new thread
            t.Start();                               // running WriteY()
         }
 
        void doDownload()
        {
            download_file(false, false);
        }

        public enum DOWNLOAD_RESULTS
        {
            NO_ERROR = 0,
            NO_FILE_NAME = 1,
            PORT_CLOSED = 2,
            ERROR_OPENING_FILE = 3,
            ERROR_IN_FILE = 4,
            CANT_SET_DOWNLOLAD_MODE = 5,
            CANT_SET_PAGE = 6,
            CANT_WRITE_PAGE = 7,
        }

        public DOWNLOAD_RESULTS download_file(bool quick, Boolean only_calc_checksum)
        {
            StreamReader f;
            hex_buf = new byte[buff_size];
            pages_checksum = new long[PAGES, 3]; // ' index 0 = checksum, index 1 = position checksum, index 2 = flag to mark if page empty (only with &hff).


            if (!serialPort.IsOpen && !only_calc_checksum)
                serialPort.Open();
            if (_fileName == "")
            {
                //if (fileName == "")
                //    MessageBox.Show("Please select a file."); TODO
                return DOWNLOAD_RESULTS.NO_FILE_NAME;
            }

            if (!serialPort.IsOpen && !only_calc_checksum)
            {
                return DOWNLOAD_RESULTS.PORT_CLOSED;
            }

            try
            {
                f = File.OpenText(_fileName);
            }
            catch (Exception e)
            {
                serialPort.Close();
                return DOWNLOAD_RESULTS.ERROR_OPENING_FILE;
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
            // DialogResult dresult;
            byte[] buff = new byte[10];
            DOWNLOAD_RESULTS rc;

            // CMDDownload.Enabled = false;
            _downloading = true;
            Thread.Sleep(200);


            for (i = 0; i < hex_buf.GetUpperBound(0); i++)
                hex_buf[i] = 0xff;

            base_addr = 0;
            line_counter = 0;
            end_of_file = false;

            while (!f.EndOfStream && !end_of_file)
            {
                line_counter++;
                input_line = f.ReadLine();
                if (input_line.Length < 11)
                {
                    // MessageBox.Show("Error : invalid data  (line too short) in line no. " + line_counter + ".\nAborting download.");
                    rc = DOWNLOAD_RESULTS.ERROR_IN_FILE;
                    goto file_fatal_error;
                }
                line_data_size = Convert.ToInt32(input_line.Substring(1, 2), 16);
                input_line = input_line.Substring(0, 11 + (line_data_size * 2)); // cut off unneeded data.
                line_offset_addr = Convert.ToInt64(input_line.Substring(3, 4), 16);
                line_data_type = Convert.ToInt32(input_line.Substring(7, 2));
                if (input_line.Length != 11 + (line_data_size * 2))
                {
                    // MessageBox.Show("Error : invalid data (wrong line length) in line no. " + line_counter.ToString() + ".\nAborting download.");
                    rc = DOWNLOAD_RESULTS.ERROR_IN_FILE;
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
                         // MessageBox.Show("Invalid data (bad type) in line no. " + line_counter.ToString() + ".\nAborting download.");
                        rc = DOWNLOAD_RESULTS.ERROR_IN_FILE;
                        goto file_fatal_error;
                }
            }

            _global_checksum = 0;
            for (i = 0; i < buff_size - 1; i++)
                _global_checksum = global_checksum + hex_buf[i];

            _global_checksum = global_checksum & 0xFF;
            if (only_calc_checksum)
            {
                _downloading = false;
                return DOWNLOAD_RESULTS.NO_ERROR;
            }

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
            check_current_mode(serialPort);
            if (current_mode != CM_DOWNLOADING)
            {
                rc = DOWNLOAD_RESULTS.CANT_SET_DOWNLOLAD_MODE;
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
                //                Debug.WriteLine("writserioaing page #" + i.ToString());
                if (!set_page( i, 3))
                {

                    // MessageBox.Show("Download error. Couldn't set page (" + current_page.ToString() + ").");
                    rc = DOWNLOAD_RESULTS.CANT_SET_PAGE;
                    goto file_fatal_error;
                }

                //delay(ack_timeout);
                if (!write_page(serialPort, current_page))
                {
                    //MessageBox.Show("Download error. Couldn't write page.");
                    rc = DOWNLOAD_RESULTS.CANT_WRITE_PAGE;
                    goto file_fatal_error;
                }
                current_page++;
                Thread.Sleep(0);
                nPCurrentPage = (int)current_page;
            retry_next: ;
            }

            set_download_mode_old_frame(CM_RUNNING);
            // MessageBox.Show("Download completed!");
            goto skip_dc;

        file_fatal_error:
            set_download_mode_old_frame(CM_RUNNING);
            check_current_mode(serialPort);
        skip_dc:
            f.Close();
//             CMDDownload.Enabled = true; TODO
            inbuff_ptr = 0;
            _downloading = false;
            serialPort.Close();
            nPCurrentPage = 0;
            return DOWNLOAD_RESULTS.NO_ERROR;
        }

        private bool set_download_mode_old_frame(byte mode)
        {
            bool rc;
            byte[] buff = new byte[5];
            if (!serialPort.IsOpen)
                return false;

            buff[0] = (byte)'W';
            buff[1] = get_special_field_addr_hi(1);
            buff[2] = get_special_field_addr_lo(1);
            buff[3] = mode;
            buff[4] = (byte)get_checksum(buff, 4);
            rc = send_expect(serialPort, buff, 5, "W", null, 0, timeout_parameter_write);

            if (rc)
                current_mode = mode;
            else
                current_mode = CM_NO_RESPONSE;

            return rc;
        }


        private bool write_page(SerialPort serialPort1, long current_page)
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
                    got_ack = send_expect(serialPort1, buf, 2, "A", null, 0, 100);
                else
                    got_ack = send_expect(serialPort1, buf, (int)PAGE_SIZE + 2, "A", null, 0, 100);

                // delay(ack_timeout);
                retries = retries - 1;
                if (!got_ack)
                    error_count++;
            }

            return got_ack;
        }


        private bool set_page(long page, int retries)
        {
            byte[] buff = new byte[5];
            long temp;
            Boolean rc;
            int tries;

            if (!serialPort.IsOpen)
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
                rc = send_expect(serialPort, buff, 5, "W", null, 0, timeout_parameter_write);
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
                rc = send_expect(serialPort, buff, 5, "W", null, 0, timeout_parameter_write);
                tries++;
            } while (tries < retries && !rc);
            return rc;
        }


        private bool set_download_mode(byte mode)
        {
            byte[] buff = new byte[5];
            
            if (!serialPort.IsOpen)
                return false;

            Byte[] frame;

            frame = create_write_frame(0x29a, mode);
            serialPort.Write(frame, 0, frame.Length);
            Thread.Sleep(100);
            current_mode = mode;
            return true;
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

        private void check_current_mode(SerialPort serialPort1)
        {

            byte[] buff_out = new byte[10];
            byte[] buff_in = new byte[10];

            if (!serialPort1.IsOpen)
                return;

            buff_out[0] = (byte)'R';
            buff_out[1] = get_special_field_addr_hi(1);
            buff_out[2] = get_special_field_addr_lo(1);
            buff_out[3] = 1;
            buff_out[4] = (byte)get_checksum(buff_out, 4);
            //            Debug.WriteLine("Sending check mode packet");
            if (!send_expect(serialPort1, buff_out, 5, "", buff_in, 3, ack_timeout) || buff_in[2] != get_checksum(buff_in, 2))
                current_mode = CM_NO_RESPONSE;
            else
                current_mode = buff_in[1];
        }


        private bool send_expect(SerialPort serialPort1, byte[] send, int send_length, string expect, byte[] response, int response_length, int timeout)
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
                //MessageBox.Show("An error occured while accessing the serial port:\n" + e.Message);
                //                t_check_rs232_send_flags.CancelAsync();
                Thread.Sleep(1000);
                serialPort.Close();

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
                        Thread.Sleep(10);
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



        // Timer delegates
        private void checkFrames(Object stateInfo)
        {
            int i;

            if (downloading)
                return;

            Boolean checked_buff = false;
            tmrCheckFrames.Change(Timeout.Infinite, Timeout.Infinite);

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
                        if (inbuff_ptr >= 2 && inbuff[2] != 'D' && inbuff[2] != 'T' && inbuff[2] != 'V' && inbuff[2] != 'S')
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
                                //dispatch_packet(j);
                                reset_buff(inbuff[1] + 2);
                                checked_buff = true;
                                //                            Debug.WriteLine(inbuff_ptr.ToString());
                            }
                        }
                        else
                            checked_buff = true;
                    }
                }
            tmrCheckFrames.Change(10,10);
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


        int secState = 0;// Check if 1 sec elapsed in the 500ms timer.
        public void checkStatus(Object stateInfo)
        {
            int i;
            tmr500ms.Change(Timeout.Infinite, Timeout.Infinite);
            if (++secState == 2) // 1 sec elapsed.
            {

                // Update lost packets
                long tick = System.Environment.TickCount;
                long diff = Math.Abs(tick - _lastFrame);
                int sec = (int)(diff / 1000);
                if (sec > 0)
                {
                    this._lostFrames += sec * 8;
                    _lastFrame = tick;
                }
            }

            // 500ms elapsed. If we are the active channel - do memory refresh.
            lock (lock_memoryMap)
            {
                if (_memory_map != null && isOpen)
                {
                    serialPort.Write(cmd_read_all_ram, 0, cmd_read_all_ram.Length); 
                    Thread.Sleep(20);

                    // eeprm
                    serialPort.Write(cmd_read_all_eeprom, 0, cmd_read_all_eeprom.Length); 
                    Thread.Sleep(20);
                    #region "send errors"
                     //WDT and checksum errors TODO
                    if (do_clear_wdt)
                    {
                        do_clear_wdt = false;
                        frame = create_write_frame(0x8006, 0);
                        serialPort.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                    }
                    if (do_wdt_error)
                    {
                        do_wdt_error = false;
                        serialPort.Write(cmd_do_wdt_error, 0, cmd_do_wdt_error.Length);
                        Thread.Sleep(20);
                    }
                    if (do_checksum_error)
                    {
                        do_checksum_error = false;
                        serialPort.Write(cmd_do_chksum_error, 0, cmd_do_chksum_error.Length);
                        Thread.Sleep(20);
                    }

                    if (do_load_defaults)
                    {
                        frame = create_write_frame(0x1be, 1);
                        serialPort.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                        do_load_defaults = false;
                    }

                    if (do_set_serial) // Set the serial number
                    {
                        for (i = 0; i < serial.Length; i++)
                        {
                            frame = create_write_frame(0x8000 + i, (Byte)serial[i]);
                            serialPort.Write(frame, 0, frame.Length);
                            Thread.Sleep(100);
                        }
                        // Right pad with zeroes.
                        for (i = serial.Length; i < 6; i++)
                        {
                            frame = create_write_frame(0x8000 + i, (Byte)0);
                            serialPort.Write(frame, 0, frame.Length);
                            Thread.Sleep(100);
                        }
                        do_set_serial = false;
                    }

                    if (do_set_hour_counter)
                    {
                        frame = create_write_frame(0x8007, (byte)(nHourCounter >> 8));
                        serialPort.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                        frame = create_write_frame(0x8008, (byte)(nHourCounter & 0x00ff));
                        serialPort.Write(frame, 0, frame.Length);
                        Thread.Sleep(100);
                        do_set_hour_counter = false;
                    }

                    #endregion

                }

            }



            tmr500ms.Change(500, 500);
        }
    }
}
