// #define CF 

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Text.RegularExpressions;
#if (CF)
using Microsoft.WindowsCE.Forms;
#endif




namespace SensorScope
{



    public class page
    {
        public string page_name;
        public int page_start;
        public int page_length;
        public int page_interval;
        public bool refresh;
#if (! CF)
        public System.Timers.Timer tmr_page_refresh;
#else
        public Timer tmr_page_refresh;
#endif
        public Boolean page_use_on_ipaq;
        public int count; // For debugging to check how many times in a second it is really refreshed.
        public byte page_signature; // A constant value the must be at the end of the page.

        public page(string name, int start, int length, int interval, Boolean use_on_ipaq,byte signature)
        {
            page_name = name;
            page_start = start;
            page_length = length;
            page_interval = interval;
            refresh = false;
            page_use_on_ipaq = use_on_ipaq;
#if (! CF)
            tmr_page_refresh = new System.Timers.Timer(page_interval);
#else
            tmr_page_refresh = new Timer();
#endif
            page_signature = signature;
        }
    }

    public class parameter : IComparable
    {
#if (!CF)
        static ErrorProvider ErrorProvider1;
#endif
        public string s_label;
        public string s_Address;
        public int n_length;
        public double n_divider;
        public string s_format;
        public char c_type;
        public int n_min;
        public int n_max;
        public string s_access;
        public int n_special_parameter_code;
        public Boolean b_display_on_pc;
        public int n_lab_tab; // # of tab to put this parameter on the ipaq - on lab parameters screen. "" or "0" = don't show.
        public int n_service_tab; // # of tab to put this parameter on the ipaq - on service parameters screen. "" or "0" = don't show.
#if (CF)
        static InputPanel ip;
#endif


        // public char c_refresh_rate; obslete. Pages will set the needed refresh rate.
        // Gui controls
        public Label caption;
        public TextBox value;
        //public NumericUpDown nud;
        public int label_width;
#if (! CF)
        ToolTip m_wndToolTip;
        ToolTip m_labelToolTip;
#endif
        public byte[] raw_data;
        public bool f_param_error; // Mark the this parameter is not active for some reason.
        public bool f_dirty_bit; // Text was changed and set command was sent. Refresh from memory map.
        public bool f_text_changed; // Text was changed. Send set command.
        public int retries; // Count how many retries left.
        public const int retries_init = 3; // When trying to set - set retries to this value
        //        public bool f_mark_error; // Mark than value contains an invalid value.
        private string orig_value; // Support escape character to reload original value.
        private string orig_value_before_strip; // Support escape character to reload original value.


        // Constructor.
        public parameter(string[] arr)
        {
            const int ColumnsCount = 14;

            SizeF size;
            Graphics g; // For measuring text size.
            Bitmap b;
            int i;
#if (! CF)
            if (ErrorProvider1 == null)
                ErrorProvider1 = new ErrorProvider();

#else
            if (ip == null)
                ip = new InputPanel();
#endif
            if (arr.GetUpperBound(0) > 0 && arr.GetUpperBound(0) < ColumnsCount)
            {
                s_label = "***";
                s_Address = "";
                n_length = 0;
                n_divider = 0;
                s_format = "";
                c_type = '\0';
                n_min = 0;
                n_max = 0;
                s_access = "";
                n_special_parameter_code = 0;
                b_display_on_pc = false;
                n_lab_tab = 0;
                n_service_tab = 0;
                retries = 0;
                return;
            }




            if (arr[0].StartsWith("/_")) // Handle a caption
            {
                c_type = 'C';
                caption = new Label();
                caption.Text = arr[0].Substring(2);
#if (! CF)
                caption.TextAlign = ContentAlignment.MiddleLeft;
                caption.Font = new Font(caption.Font, FontStyle.Bold | FontStyle.Underline);
#else
                caption.TextAlign = ContentAlignment.TopRight;
//                caption.Font = new Font(FontFamily.GenericSansSerif caption.Font, FontStyle.Bold | FontStyle.Underline);
#endif

                b_display_on_pc = arr[11] == "1";
                return;
            }

            // Handle an empty line (vertical space)
            if (arr[0] == "")
            {
                c_type = 'E';
                caption = new Label();
                caption.Text = "";
                caption.Visible = false;
                b_display_on_pc = arr[11] == "1";
                return;
            }

            if (arr[0].StartsWith("/N")) // Handle a new column
            {
                c_type = 'N';
                b_display_on_pc = arr[11] == "1";
                return;
            }


            try
            {
                s_label = arr[0].ToString().Trim();

                // Address
                if (arr[1].StartsWith("0x"))
                    s_Address = arr[1].Substring(2);
                else
                    s_Address = Convert.ToString(Convert.ToInt32(arr[1]), 16);

                // Length
                if (arr[2].StartsWith("0x"))
                    n_length = Convert.ToInt32(arr[2].ToString(), 16);
                else
                    n_length = Convert.ToInt32(arr[2].ToString());

                // Divider
                if (arr[3].StartsWith("0x"))
                    n_divider = Convert.ToInt32(arr[3], 16);
                else
                    n_divider = Convert.ToDouble(arr[3].ToString());

                // Format
                if (arr[4].Length >= 2 && arr[4].Substring(0, 1) == "*")
                    s_format = arr[4].Substring(1);
                else
                    s_format = "";

                // Type
                c_type = Convert.ToChar(arr[5].ToString().ToUpper());

                // Min
                if (arr[6].StartsWith("0x"))
                    n_min = Convert.ToInt32(arr[6].ToString(), 16);
                else
                    n_min = Convert.ToInt32(arr[6].ToString());

                // Max
                if (arr[7].StartsWith("0x"))
                    n_max = Convert.ToInt32(arr[7], 16);
                else
                    n_max = Convert.ToInt32(arr[7]);

                // Access type (R / RW)
                s_access = arr[8].ToUpper();

                // Special parameter code
                if (arr[9] == "") arr[9] = "0";
                n_special_parameter_code = Convert.ToInt32(arr[9]);

                // Display on pc ?
                b_display_on_pc = (arr[11] == "1");

                // Get tab number for lab users
                if (arr[12] == "") arr[12] = "0";
                n_lab_tab = Convert.ToInt32(arr[12]);

                // Get tab number for service users
                if (arr[13] == "") arr[13] = "0";
                n_service_tab = Convert.ToInt32(arr[13]);
            }
            catch (System.ArgumentException e)
            {
                MessageBox.Show("Error in parameter file : " + e.Message);
            }
            catch (FormatException e)
            {
                MessageBox.Show("Error in parameter file : " + e.Message);
            }

            // c_refresh_rate = Convert.ToChar(arr[9].ToString().ToUpper());

            raw_data = new byte[n_length];
            for (i = 0; i < n_length; i++)
                raw_data[i] = 0Xff; ;
            f_dirty_bit = false;
            f_text_changed = false;

            caption = new Label();
            value = new TextBox();
            value.Tag = this;


            // Caption setup aAa
#if (! CF)
            b = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
#else
            b = new Bitmap(1, 1, PixelFormat.Format32bppRgb);
#endif
            g = Graphics.FromImage(b);

            size = g.MeasureString(s_label, caption.Font);
            while (size.Width < 90)
            {
                s_label += ".";
                size = g.MeasureString(s_label, caption.Font);
            }
            label_width = (int)size.Width;

            caption.Text = s_label;
#if (! CF)
            caption.TextAlign = ContentAlignment.MiddleLeft;
#else
            caption.TextAlign = ContentAlignment.TopRight;
#endif

            // Tooltip
#if (! CF)
            m_wndToolTip = new ToolTip();
            m_labelToolTip = new ToolTip();

            // Set up the delays for the ToolTip.
            m_wndToolTip.AutoPopDelay = Int16.MaxValue;
            m_labelToolTip.AutoPopDelay = Int16.MaxValue;
        
//            m_wndToolTip.InitialDelay = 10;
//            m_labelToolTip.InitialDelay = 10;
//            m_wndToolTip.ReshowDelay = 50;
//            m_labelToolTip.ReshowDelay = 50;
            // Force the ToolTip text to be displayed whether or not the form is active.
//            m_wndToolTip.ShowAlways = true;
//            m_labelToolTip.ShowAlways = true;
            
            // Set up the ToolTip text for textbox.
            m_wndToolTip.SetToolTip(value, arr[10]);
            m_labelToolTip.SetToolTip(caption, arr[10]);
            // Set style
            //m_wndToolTip.IsBalloon = true;
            //m_wndToolTip.UseAnimation = false;
#endif

            g.Dispose();
            b.Dispose();

            // Textbox setup
            value.ReadOnly = (s_access == "R") || (c_type == 'S');
            value.TabStop = (!((s_access == "R") || (c_type == 'S')));
#if (! CF)
            value.Enter += new EventHandler(value_Enter);
            value.Leave += new EventHandler(value_Leave);
#else
            value.GotFocus += new EventHandler(value_Enter);
            //value.LostFocus += new EventHandler(value_Leave);
            value.Validating += new System.ComponentModel.CancelEventHandler(value_Validating);
#endif
            value.KeyPress += new KeyPressEventHandler(value_KeyPress);
            value.BackColor = Color.FromArgb(228, 225, 242);
            if (s_access == "R")
                value.ForeColor = Color.FromArgb(20, 20, 180);

            if (c_type == 'D')
                value.MaxLength = 8;
            else if (c_type == 'A')
                value.MaxLength = n_length;
            //else
            //value.ForeColor = Color.FromArgb(0, 0, 0);
        }


        void value_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;

            if (tb.Text == "")
            {
                f_dirty_bit = true;
                return;
            }


            if (f_param_error)
                return;

            if (check_value())
            {
                if (tb.Text != orig_value)
                {
                    retries = retries_init;
                    f_text_changed = true;
                }
                else
                    tb.Text = orig_value_before_strip;
#if (CF)
                hide_keyboard();
#endif
            }
            else
                e.Cancel = true;
                //value.Focus();
        }

#if (CF)

        private void show_keyboard()
        {
            ip.Enabled = true;
        }

        private void hide_keyboard()
        {
            ip.Enabled = false;
        }
#endif



        /// <summary>
        /// IComparable.CompareTo implementation.
        /// </summary>
        public int CompareTo(object obj)
        {
            if (obj is parameter)
            {
                parameter param = (parameter)obj;

                return n_special_parameter_code.CompareTo(param.n_special_parameter_code);
            }

            throw new ArgumentException("object is not a parameter");
        }

        void value_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                value.Text = orig_value_before_strip;
                e.Handled = true;
                value.SelectAll();
                return;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                caption.Focus(); // It will also activate value_leave
                e.Handled = true;
            }

            if (e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Delete)
                return;

            if ((c_type == 'B') && (e.KeyChar.CompareTo((char)Keys.D0) < 0 || e.KeyChar.CompareTo((char)Keys.D9) > 0) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            if ((c_type == 'T') && (e.KeyChar.CompareTo((char)Keys.D0) < 0 || e.KeyChar.CompareTo((char)Keys.D9) > 0) && e.KeyChar != '.' && e.KeyChar != ':')
            {
                e.Handled = true;
                return;
            }

            if ((c_type == 'D') && (e.KeyChar.CompareTo((char)Keys.D0) < 0 || e.KeyChar.CompareTo((char)Keys.D9) > 0) && e.KeyChar != '/' && e.KeyChar != '/')
            {
                e.Handled = true;
                return;
            }


            // No limit on 'A' type.
            //if ((c_type == 'A') && (e.KeyChar.CompareTo((char)Keys.D0) < 0 || e.KeyChar.CompareTo((char)Keys.D9) > 0) && e.KeyChar != '.' && e.KeyChar != ':')
            //{
            //    e.Handled = true;
            //    return;
            //}


        }


        //            Const REGULAR_EXP = "(^-?\d{1,3}\.$)|(^-?\d{1,3}$)|(^-?\d{0,3}\.\d{1,2}$)"

        //            If aValue <> "" Then
        //           If Not Regex.IsMatch(aValue, REGULAR_EXP) Then
        //               ErrorProvider.SetError(TextBox1, Invalid Value")
        //               TextBox1.Focus
        //           End If

        void value_Leave(object sender, EventArgs e)
        {

            TextBox tb = (TextBox)sender;

            if (tb.Text == "")
            {
                f_dirty_bit = true;
                return;
            }
            

            if (f_param_error)
                return;

            if (check_value())
            {
                if (tb.Text != orig_value)
                {
                    retries = retries_init;
                    f_text_changed = true;
                }
                else
                    tb.Text = orig_value_before_strip;
#if (CF)
                hide_keyboard();
#endif
                
            }
            else
                value.Focus();
        }

        void value_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            parameter prm = (parameter)tb.Tag;

            orig_value_before_strip = tb.Text;

            if (prm.c_type == 'B' && ! value.ReadOnly )
                tb.Text = strip(tb.Text);

            orig_value = tb.Text;

#if (CF)
            show_keyboard();
#endif
            tb.SelectAll();
        }

        // remove none digits characters from end of a string
        public String strip(String S_in)
        {
            int i;
            Char ch;

            if (S_in.Length == 0)
                return "";

            for (i = 0; i < S_in.Length; i++)
            {
                ch = Convert.ToChar(S_in.Substring(i, 1));

                if ((ch < '0' || ch > '9') && ch != '.')
                    break;
            }

            return S_in.Substring(0, i);
        }

        public void update(byte[] buff)
        {
            int i;
            double val;
            String[] formats;
            String s_1, s_2;
            String sf1, sf2;
            int n_addr;



            n_addr = Convert.ToInt32(s_Address, 16);
            val = 0;
            switch (c_type)
            {
                case 'B':
                    for (i = 0; i < n_length; i++)
                    {
                        raw_data[i] = buff[n_addr + i];
                        val += buff[n_addr + i] * (long)Math.Pow(256, (n_length - i - 1));
                    }
                    if (s_format.StartsWith("X"))
                        value.Text = string.Format("{0:" + s_format + "}", (int)(val / n_divider));
                    else
                        value.Text = string.Format("{0:" + s_format + "}", val / n_divider);

                    break;

                case 'S':
                    raw_data[0] = buff[n_addr];
                    for (i = 1; i < n_length; i++)
                    {
                        raw_data[i] = buff[n_addr + i];
                        val += buff[n_addr + i] * (long)Math.Pow(256, (n_length - i - 1));
                    }
                    switch (raw_data[0])
                    {
                        case 0: // Negetive result
                            value.Text = "-" + val.ToString();
                            break;
                        case 1: // Positive result (+)
                            value.Text = "+" + val.ToString();
                            break;
                        case 2: // Under-range
                            value.Text = "Under-range";
                            break;
                        case 3: // Over-range
                            value.Text = "Over-range";
                            break;
                        case 4: // Under-flow
                            value.Text = "Under-flow";
                            break;
                        case 5: // Over-flow
                            value.Text = "Over-flow";
                            break;
                        case 19: // Unavailable
                            value.Text = "Unavailable";
                            break;
                        default:
                            value.Text = "Invalid sign";
                            break;
                    }
                    break;

                case 'T':
                    for (i = 0; i < n_length; i++)
                        raw_data[i] = buff[n_addr + i];
                    value.Text = String.Format("{0:0#}:{1:0#}:{2:0#}", raw_data[2], raw_data[1], raw_data[0]);
                    break;

                case 'A':
                    value.Text = "";
                    for (i = 0; i < n_length; i++)
                    {
                        raw_data[i] = buff[n_addr + i];
                        value.Text = value.Text + Convert.ToChar(buff[n_addr + i]);
                    }
                    break;

                case 'D':
                    raw_data[0] = buff[n_addr];
                    for (i = 1; i < n_length; i++)
                        raw_data[i] = buff[n_addr + i];
                    String Day = Convert.ToString(buff[n_addr]);
                    Day = Day.PadLeft(2, '0');
                    String Month = Convert.ToString(buff[n_addr + 1]);
                    Month = Month.PadLeft(2, '0');
                    String Year = Convert.ToString(buff[n_addr + 2]);
                    Year = Year.PadLeft(2, '0');
                    value.Text = Day + "/" + Month + "/" + Year;
                    break;

                case 'L':
                    for (i = 0; i < n_length; i++)
                        raw_data[i] = buff[n_addr + i];
                    formats = s_format.Split(':');
                    sf1 = formats[0];
                    if (formats.GetUpperBound(0) == 1)
                        sf2 = formats[1];
                    else
                        sf2 = "";
                    value.Text = String.Format("{0:" + sf1 + "}:{1:" + sf2 + "}", buff[n_addr] * 256 + buff[n_addr + 1], buff[n_addr + 2] * 256 + buff[n_addr + 3]);
                    break;

                    case 'H':
                         String s_tmp = "";
                        for (i = 0; i < n_length; i++)
                        {
                            raw_data[i] = buff[n_addr + i];
                            s_tmp += String.Format("{0:X2}", buff[n_addr + i]);
                        }
                        if (value.Font.Size >= 8)
                            value.Font = new Font(value.Font.FontFamily, 7, value.Font.Style);
                        value.Text = s_tmp;
                        break;
            }
        }

        private bool check_value()
        {
            bool valid_value;

            if (value.ReadOnly)
                return true;

            valid_value = true;

            if (value.Text != "")
            {
                switch (c_type)
                {
                    case 'B':
                        if (n_divider > 1)
                            valid_value = Regex.IsMatch(value.Text.Trim(), "(^[0-9]+.?[0-9]+$)|(^[0-9]+$)");
                        else
                            valid_value = Regex.IsMatch(value.Text.Trim(), "(^[0-9]+$)");
#if (! CF)
                        if (!valid_value)
                            ErrorProvider1.SetError(value, "Invalid value");
#endif
                        break;

                    case 'T':
                        valid_value = Regex.IsMatch(value.Text.Trim(), "(^[0-9]{2}:[0-9]{2}:[0-9]{2}$)");
#if (! CF)
                        if (!valid_value)
                            ErrorProvider1.SetError(value, "Invalid time format (use HH:MM:SS");
                        else
#endif
                            if (Convert.ToInt16(value.Text.Trim().Substring(0, 2)) > 23 || Convert.ToInt16(value.Text.Trim().Substring(3, 2)) > 59 || Convert.ToInt16(value.Text.Trim().Substring(6, 2)) > 59)
                            {
                                valid_value = false;
#if (! CF)
                                ErrorProvider1.SetError(value, "Invalid time format (use HH:MM:SS");
#endif
                            }
                        break;

                    case 'A':
                        valid_value = (value.Text.Length >= 1);
                        //valid_value = (value.Text.Length == n_length);
#if (! CF)
                        if (!valid_value)
                            ErrorProvider1.SetError(value, "Wrong string length. Please type exactly " + n_length.ToString() + " Characters.");
#endif
                        break;

                    case 'D':
                        if (value.Text.Length > 8)
                        {
                            valid_value = false;
#if (! CF)
                            ErrorProvider1.SetError(value, "Wrong date values. Please use the format \"DD/MM/YY\"");
#endif
                            return false;
                        }
                        // Check the date format
                        try
                        {
                            String[] date = value.Text.Split('/');
                            if (date.GetUpperBound(0) != 2 || date[0].Length > 2 || date[1].Length > 2 || date[2].Length > 2)
                                throw new ArgumentOutOfRangeException();
                            DateTime dt = new DateTime(Convert.ToUInt16(date[2]), Convert.ToUInt16(date[1]), Convert.ToUInt16(date[0]));
                        }
                        catch (ArgumentOutOfRangeException e)
                        {

                            valid_value = false;
#if (! CF)
                            ErrorProvider1.SetError(value, "Wrong date values. Please use the format \"DD/MM/YY\"");
#endif
                            return false;
                        }
                        break;

                }
            }
            else
                valid_value = false;

            if (valid_value && c_type == 'B')
            {
                valid_value = (Convert.ToDouble(value.Text) >= n_min && Convert.ToDouble(value.Text) <= n_max);
#if (! CF)
                if (!valid_value)
                    ErrorProvider1.SetError(value, "Out of range");
#endif
            }
#if (! CF)
            if (valid_value)
                ErrorProvider1.SetError(value, "");
#endif
            return valid_value;
        }


        public bool data_changed(byte[] buff)
        {
            int i;
            bool f_changed;


            if (value == null || (s_access == "RW" && value.Focused))
                return false;

            if (f_dirty_bit)
            {
                f_dirty_bit = false;
                return true;
            }

            f_changed = false;
            for (i = 0; i < n_length && !f_changed; i++)
            {
                f_changed = buff[Convert.ToInt32(s_Address, 16) + i] != raw_data[i];
                //                Debug.WriteLine (buff[Convert.ToInt32(s_Address, 16) + i].ToString() + "   " + raw_data[i].ToString());
            }

            //if (f_changed && caption.Text.StartsWith("Pulse width"))
            //    i = 0;

            return f_changed;
        }

    }


    class Parameters
    {
        public const int max_page_size = 254;
        public ArrayList mem_pages;
        public bool f_load_error;
        public string s_error_message;
        public ArrayList params_arr = new ArrayList();
        public ArrayList special_params = new ArrayList();
#if (CF)
        public ArrayList arr_lab_tabs = new ArrayList();  // Tab names for laboratory users
        public ArrayList arr_tech_tabs = new ArrayList(); // Tab names for technical support users
        
#endif

        private string s_Param_FileName;

        public Parameters(string s_FileName)
        {
            mem_pages = new ArrayList();
            s_Param_FileName = s_FileName;
            f_load_error = !Load_Parameters();
        }

        ~Parameters()
        {
            int i;
            // Stop pages timers and free memory
            foreach (page p in mem_pages)
                p.tmr_page_refresh = null;

            for (i = 0; i < mem_pages.Count; i++)
                mem_pages[i] = null;

            // Release parameters array allocated memory.
            for (i = 0; i < params_arr.Count; i++)
                params_arr[i] = null;

            params_arr = null;

        }



        private bool Load_Parameters()
        {
            char[] splitChar = { ',' };
            string[] lineArray;
            string line;
            parameter param;
            page c_page;
            int line_count;
            string s_name;
            int n_pagestart;
            int n_pagelength;
            int n_interval;
            int n_param_start; // Start position of current parameter - as integer (for mapping checkups)
            int n_param_end;   // End   position of current parameter - as integer (for mapping checkups)
            string s_not_mapped;
            bool f_mapped; // For mapping checkup of each parameter
            Boolean use_on_ipaq;
            byte signature;
            int i;

            line_count = 0;
            s_not_mapped = "";
            try
            {
                StreamReader srRead = File.OpenText(s_Param_FileName);

                while (!srRead.EndOfStream)
                {
                    line = srRead.ReadLine();
                    line_count++;
                    if (line.StartsWith(";")) // Handle remarks lines
                        continue;

#if (CF)
                    // Handle tabs
                    int comma_pos;

                    if (line.StartsWith("/TLab"))
                    {
                        comma_pos = line.IndexOf(',');
                        arr_lab_tabs.Add(line.Substring(5,comma_pos - 5));
                        continue;
                    }

                    if (line.StartsWith("/TTec"))
                    {
                        comma_pos = line.IndexOf(',');
                        arr_tech_tabs.Add(line.Substring(5,comma_pos - 5));
                        continue;
                    }
#else
                    if (line.StartsWith("/TLab"))
                        continue;

                    if (line.StartsWith("/TTec"))
                        continue;
#endif


                    lineArray = line.Split(splitChar);
                    if (lineArray[0].ToUpper() == "*P")
                    {

                        s_name = lineArray[1];

                        if (lineArray.GetUpperBound(0) < 7)
                        {
                            MessageBox.Show("Parameters loading error: Invalid page definition line at line #" + line_count);
                            continue;
                        }


                        // Page start address
                        if (lineArray[2].StartsWith("0x"))
                            n_pagestart = Convert.ToInt32(lineArray[2], 16);
                        else
                            n_pagestart = Convert.ToInt32(lineArray[2]);

                        // Page Length
                        if (lineArray[3].StartsWith("0x"))
                            n_pagelength = Convert.ToInt32(lineArray[3], 16);
                        else
                            n_pagelength = Convert.ToInt32(lineArray[3]);

                        // Refresh interval for the page
                        if (lineArray[4].StartsWith("0x"))
                            n_interval = Convert.ToInt32(lineArray[4], 16);
                        else
                            n_interval = Convert.ToInt32(lineArray[4]);

                        // Check for enough info for page definition (*p,start,length)


                        // Handle invalid page size
                        if (n_pagelength > max_page_size)
                        {
                            MessageBox.Show("Parameters loading Error: Invalid page size (" + lineArray[2] + ") on line no. " + line_count.ToString());
                            continue;
                        }

                        // Handle memory overflow
                        if (n_pagestart + n_pagelength - 1 > 0x3fff)
                        {
                            MessageBox.Show("Parameters loading Error: Invalid memory addresses (overflow) on line " + line_count.ToString());
                            continue;
                        }
                        //Handle interval validity
                        if (n_interval == 0)
                        {
                            MessageBox.Show("Parameters loading Error: Invalid interval on line " + line_count.ToString());
                            continue;
                        }

                        // Check if ipaq should use this page
                        use_on_ipaq = (lineArray[5] == "1");

                        // Page signature
                        signature = Convert.ToByte(lineArray[6]);

                        // Add the page
                        c_page = new page(s_name, n_pagestart, n_pagelength, n_interval, use_on_ipaq,signature);
                        mem_pages.Add(c_page);
                        continue;
                    }
                    // Check for enough data for a new value definition

                    // Check for empty line

                    //                    Debug.WriteLine(lineArray.GetUpperBound(0).ToString());

                    if (lineArray.Length == 16)
                        param = new parameter(lineArray);
                    else
                        continue;

                    params_arr.Add(param);
                    if (param.n_special_parameter_code > 0)
                        special_params.Add(param);



                    if (param.c_type == 'C' || param.c_type == 'E' || param.c_type == 'N')
                        continue;

                    // Check if the parameter is mapped
                    if (lineArray.GetUpperBound(0) != 14) // Check for enough parameters
                        continue;

#if (! CF)
                    if (!param.b_display_on_pc)
                        continue;
#else
                    if (param.n_lab_tab == 0 && param.n_service_tab == 0)
                        continue;
#endif


                    f_mapped = false;
                    for (i = 0; i < mem_pages.Count && !f_mapped; i++)
                    {
                        c_page = (page)mem_pages[i];
                        n_param_start = Convert.ToInt32(param.s_Address, 16);
                        n_param_end = Convert.ToInt32(param.s_Address, 16) + param.n_length - 1;
                        if (n_param_start >= c_page.page_start && n_param_end <= c_page.page_start + c_page.page_length - 1)
                            f_mapped = true;
                    }
                    if (!f_mapped)
                    {
                        if (s_not_mapped.Length == 0)
                            s_not_mapped += param.s_label + "(line " + line_count.ToString() + ")";
                        else
                            s_not_mapped += ", " + param.s_label + "(line " + line_count.ToString() + ")";
                        param.caption.BackColor = Color.Red;
                    }
                    param.f_param_error = !f_mapped;
                    Debug.WriteLine(param.s_label);

                }

                srRead.Close();
                if (s_not_mapped.Length > 0)
                    MessageBox.Show("Parameters loading warning : The following parameters are not mapped :\n" + s_not_mapped);

                special_params.Sort();

                // Check Special parameters consistency.
                if (special_params.Count > 0)
                {
                    for (i = 0; i < special_params.Count; i++)
                    {
                        if (((parameter)special_params[i]).n_special_parameter_code != i + 1)
                        {
                            MessageBox.Show("Inconsistency in special parameters definitions");
                            return false;
                        }
                    }

                }
                return true;
            }
            catch (IOException e)
            {
                f_load_error = true;
                // MessageBox.Show("Error while loading parameters file.\n" + e.Message);
                s_error_message = e.Message;
                return false;
            }

        }



    }




}

