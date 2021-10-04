using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using SensorScope;

namespace Sensor_Scope.General_classes
{
    public class cDownloadFirmware
    {
        // Firmware update stuff
        int firmware_version; // Version as recieved from the board.
        const long PAGES = 128; // 504
        const long PAGE_SIZE = 512; // 256
        const long buff_size = PAGES * PAGE_SIZE;
        byte[] hex_buf;
        long[,] pages_checksum; // index 0 = checksum, index 1 = position checksum, index 2 = flag to mark if page empty (only with &hff).
        long page_checksum_from_board;
        long page_position_checksum_from_board;
        long global_checksum;
        long current_page;

        // For progress bar if needed
        public int prgrsMax;
        public int prgrsval;

        // Board modes.
        int current_mode;
        const int CM_NO_RESPONSE = -1;
        const int CM_RUNNING = 0;
        const int CM_DOWNLOADING = 1;

        int timeout_pageread=50; // 12
        int timeout_pagewrite=50; // For write timeout.
        int timeout_parameter_read = 50; // 12 For the functions folder
        int timeout_parameter_write = 50; // For write timeout.

        Parameters c_parameters;
        public bool download_file(bool quick,SerialPort serialPort,String sFilename)
        {

            c_parameters = new Parameters("parameters.csv");

            hex_buf = new byte[buff_size];
            pages_checksum = new long[PAGES, 3]; // ' index 0 = checksum, index 1 = position checksum, index 2 = flag to mark if page empty (only with &hff).

            StreamReader f;
            if (sFilename == "" || !serialPort.IsOpen)
                return false;

            // f = DLGFILEFirmwareUpdte.OpenFile();
            try
            {
                f = File.OpenText(sFilename);
            }
            catch (Exception e)
            {
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

            for (i = 0; i < hex_buf.GetUpperBound(0); i++)
                hex_buf[i] = 0xff;

            base_addr = 0;
            line_counter = 0;
            end_of_file = false;
            prgrsval = 0;
            prgrsMax = (int)PAGES;

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
            //            Debug.WriteLine("Calculated checksum is " + global_checksum.ToString());
            //            Debug.WriteLine("Roni's checksum : " + hex_buf[PAGES * PAGE_SIZE - 1].ToString());
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

            //            f.Close();
            //            serialPort.Close();
            current_page = 0;
            if (!set_download_mode(CM_DOWNLOADING,serialPort))
            {
                //MessageBox.Show("Conuldn't set download mode.\nAborting download.");
                //goto file_fatal_error;
                // Powerup entry
                for (i = 0; i < 250; i++)
                {
                    Application.DoEvents();
                    buff[0] = 0x55;
                    serialPort.Write(buff, 0, 1);
                    delay(5);
                }
            }

            check_current_mode(serialPort);
            if (current_mode != CM_DOWNLOADING)
            {
                MessageBox.Show("Download error. Conuldn't set download mode.");
                goto file_fatal_error;
            }

            current_mode = CM_DOWNLOADING;
            dirty_page_count = 0;

            for (i = 0; i < PAGES; i++)
            {
            retry:
                if (!set_page(i * 2, 3,serialPort))
                {

                    MessageBox.Show("Download error. Couldn't set page.");
                    //dresult = MessageBox.Show("Communication error during download (set page address).\nThe software download was not completed.", "Download Error", MessageBoxButtons. AbortRetryIgnore);
                    //if (dresult == DialogResult.Retry)
                    //    goto retry;
                    // else if (dresult == DialogResult.Ignore)
                    //    goto retry_next;

                    goto file_fatal_error;
                }


                //delay(ack_timeout);
                if (quick)
                {
                    get_page_checksum(ref  page_checksum_from_board, ref page_position_checksum_from_board,serialPort);
                    if (page_checksum_from_board != pages_checksum[current_page, 0] || page_position_checksum_from_board != pages_checksum[current_page, 1])
                    {
                        dirty_page_count++;
                        if (!write_page(current_page,serialPort))
                        {

                            MessageBox.Show("Download error. Couldn't write page.");
                            //dresult = MessageBox.Show("Communication error during download (write page - q).\nThe software download was not completed.", "Download Error", MessageBoxButtons.AbortRetryIgnore );
                            //if (dresult == DialogResult.Retry)
                            //    goto retry;
                            //else if (dresult == DialogResult.Ignore)
                            //    goto retry_next;

                            goto file_fatal_error;
                        }
                    }
                }
                else
                {
                    if (!write_page(current_page,serialPort))
                    {
                        MessageBox.Show("Download error. Couldn't write page.");
                        //dresult = MessageBox.Show("Communication error during download (write page - f).\nThe software download was not completed.", "Download Error", MessageBoxButtons.AbortRetryIgnore);
                        //if (dresult == DialogResult.Retry)
                        //    goto retry;
                        //else if (dresult == DialogResult.Ignore)
                        //    goto retry_next;

                        goto file_fatal_error;
                    }

                }

                current_page++;
                prgrsval++;
                Application.DoEvents();
            retry_next: ;
            }

            prgrsval = 0;
            set_download_mode(CM_RUNNING,serialPort);
            goto skip_dc;

        file_fatal_error:
            set_download_mode(CM_RUNNING, serialPort);
        skip_dc:
            f.Close();
            return true;
        }




        private bool set_download_mode(byte mode, SerialPort serialPort)
        {
            bool rc;
            byte[] buff = new byte[5];

            if (!serialPort.IsOpen)
                return false;

            buff[0] = (byte)'W';
            buff[1] = get_special_field_addr_hi(1);
            buff[2] = get_special_field_addr_lo(1);
            buff[3] = mode;
            buff[4] = get_checksum(buff, 4);
            rc = send_expect(buff, 5, "W", null, 0, timeout_parameter_write,serialPort);

            if (rc)
                current_mode = mode;
            else
                current_mode = CM_NO_RESPONSE;

            return rc;
        }

        private bool set_page(long page, int retries, SerialPort serialPort)
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
            buff[4] = get_checksum(buff, 4);
            if (!send_expect(buff, 5, "W", null, 0, timeout_parameter_write,serialPort))
                return false;
            //delay(ack_timeout);

            buff[0] = 0x57;
            buff[1] = get_special_field_addr_hi(3);
            buff[2] = get_special_field_addr_lo(3);
            temp = (page & 0xFF);
            buff[3] = (byte)temp;
            buff[4] = get_checksum(buff, 4);
            tries = 0;
            do
            {
                rc = send_expect(buff, 5, "W", null, 0, timeout_parameter_write,serialPort);
                tries++;
            } while (tries < retries && !rc);
            return rc;
        }

        private bool write_page(long current_page, SerialPort serialPort)
        {
            try
            {

                byte[] buf = new byte[514]; // 258
                byte[] in_buff = new byte[10];
                int i;
                int retries;
                int error_count;
                long checksum;
                bool got_ack;

                if (pages_checksum[current_page, 2] == 0) // The page is empty
                {
                    buf[0] = (byte)'E';
                    checksum = (pages_checksum[current_page, 0] + (byte)'E') % 256;
                    buf[1] = (byte)checksum;
                    goto send_buff;
                }

                buf[0] = (byte)'P';
                i = 0;
                try
                {
                    for (i = 0; i <= 511; i++)
                        buf[i + 1] = hex_buf[current_page * PAGE_SIZE + i];
                }
                catch (Exception e1)
                {
                    MessageBox.Show("here:" + current_page.ToString() + " " + i.ToString());
                }

                checksum = (pages_checksum[current_page, 0] + (byte)'P') % 256;
                buf[513] = (byte)checksum;


            send_buff:
                in_buff[0] = 0;
                retries = 20;
                got_ack = false;
                error_count = 0;
                while (retries > 0 && !got_ack)
                {
                    got_ack = send_expect(buf, 514, "A", null, 0, timeout_pagewrite,serialPort); // 258 ==> 514
                    // delay(ack_timeout);
                    retries = retries - 1;
                    if (!got_ack)
                        error_count++;
                }

                return got_ack;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Data + " " + e.Source + " " + e.Message);
                return false;

            }
        }

        private void delay(int ms)
        {
            Thread.Sleep(ms);
        }


        private void check_current_mode(SerialPort serialPort)
        {

            byte[] buff_out = new byte[10];
            byte[] buff_in = new byte[10];

            if (!serialPort.IsOpen)
                return;


            buff_out[0] = (byte)'R';
            buff_out[1] = get_special_field_addr_hi(1);
            buff_out[2] = get_special_field_addr_lo(1);
            buff_out[3] = 1;
            buff_out[4] = get_checksum(buff_out, 4);
            if (!send_expect(buff_out, 5, "", buff_in, 3, timeout_parameter_read,serialPort) || buff_in[2] != get_checksum(buff_in, 2))
                current_mode = CM_NO_RESPONSE;
            else
                current_mode = buff_in[1];
        }

        private byte get_special_field_addr_hi(int special_field_code)
        {
            parameter special_param;

            if (c_parameters.hashSpecialParams[special_field_code] == null)
                return 0;

            special_param = (parameter)c_parameters.hashSpecialParams[special_field_code];
            return (byte)(Convert.ToInt32(special_param.s_Address, 16) / 256);
        }

        private byte get_special_field_addr_lo(int special_field_code)
        {
            parameter special_param;

            if (c_parameters.hashSpecialParams[special_field_code] == null)
                return 0;

            special_param = (parameter)c_parameters.hashSpecialParams[special_field_code];
            return (byte)(Convert.ToInt32(special_param.s_Address, 16) % 256);
        }

        private void get_page_checksum(ref long page_checksum, ref long ord_page_checksum, SerialPort serialPort)
        {
            byte[] buff = new byte[5];
            byte[] in_buff = new byte[10];
            bool rc;

            if (!serialPort.IsOpen)
            {
                page_checksum = ord_page_checksum = -1;
                return;
            }

            buff[0] = (byte)'R'; // 0x52; // 'R'
            buff[1] = get_special_field_addr_hi(4);
            buff[2] = get_special_field_addr_lo(4);
            buff[3] = 2; // 2 bytes to read
            buff[4] = get_checksum(buff, 4);
            rc = send_expect(buff, 5, "", in_buff, 4, timeout_parameter_read,serialPort);
            if (!rc)
            {
                page_checksum = ord_page_checksum = -1;
                return;
            }

            page_checksum = in_buff[1];
            ord_page_checksum = in_buff[2];
            return;
        }

        byte get_checksum(byte[] buff, int length)
        {
            int i;
            long sum;

            sum = 0;
            for (i = 0; i < length; i++)
                sum = (sum + buff[i]) % 256;

            return (byte)sum;
        }

        private bool send_expect(byte[] send, int send_length, string expect, byte[] response, int response_length, int timeout, SerialPort serialPort)
        {
            byte[] in_buff;
            int i;
            int local_timeout;
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            if (!serialPort.IsOpen)
                return false;

            try
            {
                serialPort.DiscardInBuffer();
                serialPort.ReadTimeout = timeout;
                serialPort.Write(send, 0, send_length);
            }
            catch (IOException e)
            {
                MessageBox.Show("An error occured while accessing the serial port:\n" + e.Message);
                Thread.Sleep(1000);
                serialPort.Close();

                return false;
            }
            try
            {
                if (response_length > 0)
                {

                    int refTick = System.Environment.TickCount;
                    do
                    {
                        Thread.Sleep(1);
                    } while (serialPort.BytesToRead < response_length && refTick + timeout_parameter_read > System.Environment.TickCount);

                    if (serialPort.BytesToRead < response_length)
                    {
                        return false;
                    }

                    serialPort.ReadTimeout = timeout_parameter_read;
                    //serialPort.Read(response, i, response_length - i);
                    serialPort.Read(response, 0, response_length);

                    return true;
                }
                else if (expect.Length > 0)
                {
                    in_buff = new byte[expect.Length];
                    serialPort.ReadTimeout = timeout_parameter_read;
                    serialPort.Read(in_buff, 0, expect.Length);
                    return (enc.GetString(in_buff) == expect);
                }
                else
                    return true;
            }
            catch (System.IO.IOException e)
            {
                //                Debug.WriteLine(e.Message);
                return false;
            }
            catch (System.TimeoutException e)
            {
                //                Debug.WriteLine(e.Message);
                return false;
            }
            catch
            {
                return false;
            }
        }


    }
}
