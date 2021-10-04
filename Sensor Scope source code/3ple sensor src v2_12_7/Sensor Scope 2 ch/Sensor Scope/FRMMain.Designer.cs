namespace Sensor_Scope
{
    partial class FRMMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FRMMain));
            this.LBLMid = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CMBPortSelector = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.RADOpen = new System.Windows.Forms.RadioButton();
            this.RADClose = new System.Windows.Forms.RadioButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSStrain = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSBadChecksum = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSTimeouts = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSPortStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.TMRDateAndTime = new System.Windows.Forms.Timer(this.components);
            this.TXTMid = new System.Windows.Forms.TextBox();
            this.TXTSpan = new System.Windows.Forms.TextBox();
            this.TXTAverage = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.CHKSaveToFile = new System.Windows.Forms.CheckBox();
            this.BTNAutoCenter = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.LBLLastPeak = new System.Windows.Forms.Label();
            this.LBLAllp2p = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.TXTFileName = new System.Windows.Forms.TextBox();
            this.CMDBrowse = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label40 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CMDCalcChecksum = new System.Windows.Forms.Button();
            this.CMDDownload = new System.Windows.Forms.Button();
            this.DLGFILEFirmwareUpdte = new System.Windows.Forms.OpenFileDialog();
            this.TMRHideChecksum = new System.Windows.Forms.Timer(this.components);
            this.TMRCheckFrames = new System.Windows.Forms.Timer(this.components);
            this.PNLStatus = new System.Windows.Forms.Panel();
            this.LBLRegCorruption = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.LBLWDTEvents = new System.Windows.Forms.Label();
            this.LBLVersion = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.TMRRefreshMem = new System.Windows.Forms.Timer(this.components);
            this.PNLErrorGen = new System.Windows.Forms.Panel();
            this.BTNRstWdt = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.PNLSerialNumber = new System.Windows.Forms.Panel();
            this.TXTSerialNumber = new System.Windows.Forms.TextBox();
            this.label44 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LBLPackets = new System.Windows.Forms.Label();
            this.BTNLoadDefaults = new System.Windows.Forms.Button();
            this.TMRNoCommDetection = new System.Windows.Forms.Timer(this.components);
            this.pic_bw_background = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PICGraph = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PNLStatus.SuspendLayout();
            this.PNLErrorGen.SuspendLayout();
            this.PNLSerialNumber.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_bw_background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PICGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // LBLMid
            // 
            this.LBLMid.AutoSize = true;
            this.LBLMid.ForeColor = System.Drawing.SystemColors.Desktop;
            this.LBLMid.Location = new System.Drawing.Point(2, 522);
            this.LBLMid.Name = "LBLMid";
            this.LBLMid.Size = new System.Drawing.Size(41, 13);
            this.LBLMid.TabIndex = 2;
            this.LBLMid.Text = "Center:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(148, 522);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Span:";
            // 
            // CMBPortSelector
            // 
            this.CMBPortSelector.FormattingEnabled = true;
            this.CMBPortSelector.Location = new System.Drawing.Point(833, 446);
            this.CMBPortSelector.Name = "CMBPortSelector";
            this.CMBPortSelector.Size = new System.Drawing.Size(62, 21);
            this.CMBPortSelector.TabIndex = 5;
            this.CMBPortSelector.SelectedIndexChanged += new System.EventHandler(this.CMBPortSelector_SelectedIndexChanged);
            this.CMBPortSelector.Click += new System.EventHandler(this.CMBPortSelector_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(801, 449);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port:";
            // 
            // RADOpen
            // 
            this.RADOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.RADOpen.AutoSize = true;
            this.RADOpen.BackColor = System.Drawing.Color.Lime;
            this.RADOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RADOpen.Location = new System.Drawing.Point(897, 445);
            this.RADOpen.Name = "RADOpen";
            this.RADOpen.Size = new System.Drawing.Size(43, 23);
            this.RADOpen.TabIndex = 7;
            this.RADOpen.Text = "Open";
            this.RADOpen.UseVisualStyleBackColor = false;
            this.RADOpen.CheckedChanged += new System.EventHandler(this.RADOpen_CheckedChanged);
            // 
            // RADClose
            // 
            this.RADClose.Appearance = System.Windows.Forms.Appearance.Button;
            this.RADClose.AutoSize = true;
            this.RADClose.BackColor = System.Drawing.Color.IndianRed;
            this.RADClose.Checked = true;
            this.RADClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RADClose.Location = new System.Drawing.Point(943, 445);
            this.RADClose.Name = "RADClose";
            this.RADClose.Size = new System.Drawing.Size(43, 23);
            this.RADClose.TabIndex = 8;
            this.RADClose.TabStop = true;
            this.RADClose.Text = "Close";
            this.RADClose.UseVisualStyleBackColor = false;
            this.RADClose.CheckedChanged += new System.EventHandler(this.RADClose_CheckedChanged);
            // 
            // serialPort1
            // 
            this.serialPort1.BaudRate = 57600;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSTime,
            this.TSDate,
            this.TSStrain,
            this.TSBadChecksum,
            this.TSTimeouts,
            this.toolStripStatusLabel1,
            this.TSPortStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 544);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(992, 22);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSTime
            // 
            this.TSTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSTime.Name = "TSTime";
            this.TSTime.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.TSTime.Size = new System.Drawing.Size(4, 17);
            // 
            // TSDate
            // 
            this.TSDate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSDate.Name = "TSDate";
            this.TSDate.Size = new System.Drawing.Size(4, 17);
            // 
            // TSStrain
            // 
            this.TSStrain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.TSStrain.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSStrain.Name = "TSStrain";
            this.TSStrain.Size = new System.Drawing.Size(4, 17);
            // 
            // TSBadChecksum
            // 
            this.TSBadChecksum.BackColor = System.Drawing.Color.IndianRed;
            this.TSBadChecksum.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSBadChecksum.ForeColor = System.Drawing.Color.White;
            this.TSBadChecksum.Name = "TSBadChecksum";
            this.TSBadChecksum.Size = new System.Drawing.Size(4, 17);
            // 
            // TSTimeouts
            // 
            this.TSTimeouts.BackColor = System.Drawing.Color.IndianRed;
            this.TSTimeouts.ForeColor = System.Drawing.Color.White;
            this.TSTimeouts.Name = "TSTimeouts";
            this.TSTimeouts.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(883, 17);
            this.toolStripStatusLabel1.Spring = true;
            // 
            // TSPortStatus
            // 
            this.TSPortStatus.BackColor = System.Drawing.Color.IndianRed;
            this.TSPortStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSPortStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TSPortStatus.Name = "TSPortStatus";
            this.TSPortStatus.RightToLeftAutoMirrorImage = true;
            this.TSPortStatus.Size = new System.Drawing.Size(78, 17);
            this.TSPortStatus.Text = "Port is closed.";
            // 
            // TMRDateAndTime
            // 
            this.TMRDateAndTime.Enabled = true;
            this.TMRDateAndTime.Interval = 1000;
            this.TMRDateAndTime.Tick += new System.EventHandler(this.TMRDateAndTime_Tick);
            // 
            // TXTMid
            // 
            this.TXTMid.Location = new System.Drawing.Point(39, 518);
            this.TXTMid.Name = "TXTMid";
            this.TXTMid.Size = new System.Drawing.Size(59, 20);
            this.TXTMid.TabIndex = 12;
            this.TXTMid.Leave += new System.EventHandler(this.TXTMid_Leave);
            this.TXTMid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            // 
            // TXTSpan
            // 
            this.TXTSpan.Location = new System.Drawing.Point(184, 518);
            this.TXTSpan.Name = "TXTSpan";
            this.TXTSpan.Size = new System.Drawing.Size(59, 20);
            this.TXTSpan.TabIndex = 13;
            this.TXTSpan.Leave += new System.EventHandler(this.TXTSpan_Leave);
            this.TXTSpan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            // 
            // TXTAverage
            // 
            this.TXTAverage.Location = new System.Drawing.Point(304, 518);
            this.TXTAverage.Name = "TXTAverage";
            this.TXTAverage.Size = new System.Drawing.Size(27, 20);
            this.TXTAverage.TabIndex = 15;
            this.TXTAverage.Text = "12";
            this.TXTAverage.Leave += new System.EventHandler(this.TXTAverage_Leave);
            this.TXTAverage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label5.Location = new System.Drawing.Point(247, 522);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Averaging:";
            // 
            // CHKSaveToFile
            // 
            this.CHKSaveToFile.AutoSize = true;
            this.CHKSaveToFile.Location = new System.Drawing.Point(729, 520);
            this.CHKSaveToFile.Name = "CHKSaveToFile";
            this.CHKSaveToFile.Size = new System.Drawing.Size(72, 17);
            this.CHKSaveToFile.TabIndex = 16;
            this.CHKSaveToFile.Text = "Log to file";
            this.CHKSaveToFile.UseVisualStyleBackColor = true;
            this.CHKSaveToFile.CheckedChanged += new System.EventHandler(this.CHKSaveToFile_CheckedChanged);
            // 
            // BTNAutoCenter
            // 
            this.BTNAutoCenter.Location = new System.Drawing.Point(102, 517);
            this.BTNAutoCenter.Name = "BTNAutoCenter";
            this.BTNAutoCenter.Size = new System.Drawing.Size(41, 23);
            this.BTNAutoCenter.TabIndex = 17;
            this.BTNAutoCenter.Text = "Auto";
            this.BTNAutoCenter.UseVisualStyleBackColor = true;
            this.BTNAutoCenter.Click += new System.EventHandler(this.BTNAutoCenter_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(357, 512);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "max-min:";
            // 
            // LBLLastPeak
            // 
            this.LBLLastPeak.AutoSize = true;
            this.LBLLastPeak.Location = new System.Drawing.Point(419, 528);
            this.LBLLastPeak.Name = "LBLLastPeak";
            this.LBLLastPeak.Size = new System.Drawing.Size(16, 13);
            this.LBLLastPeak.TabIndex = 20;
            this.LBLLastPeak.Text = "---";
            // 
            // LBLAllp2p
            // 
            this.LBLAllp2p.AutoSize = true;
            this.LBLAllp2p.Location = new System.Drawing.Point(419, 514);
            this.LBLAllp2p.Name = "LBLAllp2p";
            this.LBLAllp2p.Size = new System.Drawing.Size(16, 13);
            this.LBLAllp2p.TabIndex = 21;
            this.LBLAllp2p.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(361, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 13);
            this.label4.TabIndex = 22;
            this.label4.Text = "last p-p:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 16);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(90, 13);
            this.label38.TabIndex = 24;
            this.label38.Text = "File to download :";
            // 
            // TXTFileName
            // 
            this.TXTFileName.Location = new System.Drawing.Point(2, 32);
            this.TXTFileName.Name = "TXTFileName";
            this.TXTFileName.Size = new System.Drawing.Size(175, 20);
            this.TXTFileName.TabIndex = 25;
            this.TXTFileName.Validated += new System.EventHandler(this.TXTFileName_Validated);
            // 
            // CMDBrowse
            // 
            this.CMDBrowse.Location = new System.Drawing.Point(6, 58);
            this.CMDBrowse.Name = "CMDBrowse";
            this.CMDBrowse.Size = new System.Drawing.Size(73, 23);
            this.CMDBrowse.TabIndex = 26;
            this.CMDBrowse.Text = "Browse";
            this.CMDBrowse.UseVisualStyleBackColor = true;
            this.CMDBrowse.Click += new System.EventHandler(this.CMDBrowse_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label6.Location = new System.Drawing.Point(32, 3);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "Software download";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label40);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.CMDCalcChecksum);
            this.panel1.Controls.Add(this.CMDDownload);
            this.panel1.Controls.Add(this.label38);
            this.panel1.Controls.Add(this.CMDBrowse);
            this.panel1.Controls.Add(this.TXTFileName);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Location = new System.Drawing.Point(805, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(181, 131);
            this.panel1.TabIndex = 28;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Location = new System.Drawing.Point(3, 111);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(51, 13);
            this.label40.TabIndex = 31;
            this.label40.Text = "Progress:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(56, 112);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(118, 13);
            this.progressBar1.TabIndex = 32;
            // 
            // CMDCalcChecksum
            // 
            this.CMDCalcChecksum.Location = new System.Drawing.Point(6, 86);
            this.CMDCalcChecksum.Name = "CMDCalcChecksum";
            this.CMDCalcChecksum.Size = new System.Drawing.Size(169, 23);
            this.CMDCalcChecksum.TabIndex = 30;
            this.CMDCalcChecksum.Text = "Calc Checksum";
            this.CMDCalcChecksum.UseVisualStyleBackColor = true;
            this.CMDCalcChecksum.Click += new System.EventHandler(this.CMDCalcChecksum_Click);
            // 
            // CMDDownload
            // 
            this.CMDDownload.Location = new System.Drawing.Point(102, 58);
            this.CMDDownload.Name = "CMDDownload";
            this.CMDDownload.Size = new System.Drawing.Size(73, 23);
            this.CMDDownload.TabIndex = 29;
            this.CMDDownload.Text = "Download";
            this.CMDDownload.UseVisualStyleBackColor = true;
            this.CMDDownload.Click += new System.EventHandler(this.CMDDownload_Click);
            // 
            // TMRHideChecksum
            // 
            this.TMRHideChecksum.Tick += new System.EventHandler(this.TMRHideChecksum_Tick);
            // 
            // TMRCheckFrames
            // 
            this.TMRCheckFrames.Enabled = true;
            this.TMRCheckFrames.Interval = 10;
            this.TMRCheckFrames.Tick += new System.EventHandler(this.TMRCheckFrames_Tick);
            // 
            // PNLStatus
            // 
            this.PNLStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.PNLStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNLStatus.Controls.Add(this.LBLRegCorruption);
            this.PNLStatus.Controls.Add(this.label47);
            this.PNLStatus.Controls.Add(this.LBLWDTEvents);
            this.PNLStatus.Controls.Add(this.LBLVersion);
            this.PNLStatus.Controls.Add(this.label22);
            this.PNLStatus.Controls.Add(this.label21);
            this.PNLStatus.Controls.Add(this.label20);
            this.PNLStatus.Location = new System.Drawing.Point(805, 177);
            this.PNLStatus.Name = "PNLStatus";
            this.PNLStatus.Size = new System.Drawing.Size(181, 69);
            this.PNLStatus.TabIndex = 29;
            // 
            // LBLRegCorruption
            // 
            this.LBLRegCorruption.AutoSize = true;
            this.LBLRegCorruption.Location = new System.Drawing.Point(81, 51);
            this.LBLRegCorruption.Name = "LBLRegCorruption";
            this.LBLRegCorruption.Size = new System.Drawing.Size(25, 13);
            this.LBLRegCorruption.TabIndex = 12;
            this.LBLRegCorruption.Text = "000";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(4, 51);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(80, 13);
            this.label47.TabIndex = 11;
            this.label47.Text = "Reg corruption:";
            // 
            // LBLWDTEvents
            // 
            this.LBLWDTEvents.AutoSize = true;
            this.LBLWDTEvents.Location = new System.Drawing.Point(81, 36);
            this.LBLWDTEvents.Name = "LBLWDTEvents";
            this.LBLWDTEvents.Size = new System.Drawing.Size(25, 13);
            this.LBLWDTEvents.TabIndex = 4;
            this.LBLWDTEvents.Text = "000";
            // 
            // LBLVersion
            // 
            this.LBLVersion.AutoSize = true;
            this.LBLVersion.Location = new System.Drawing.Point(81, 20);
            this.LBLVersion.Name = "LBLVersion";
            this.LBLVersion.Size = new System.Drawing.Size(34, 13);
            this.LBLVersion.TabIndex = 4;
            this.LBLVersion.Text = "00.00";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(4, 36);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(71, 13);
            this.label22.TabIndex = 1;
            this.label22.Text = "WDT events:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(4, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(45, 13);
            this.label21.TabIndex = 1;
            this.label21.Text = "Version:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label20.Location = new System.Drawing.Point(44, 4);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(90, 13);
            this.label20.TabIndex = 0;
            this.label20.Text = "Status && errors";
            // 
            // TMRRefreshMem
            // 
            this.TMRRefreshMem.Enabled = true;
            this.TMRRefreshMem.Interval = 500;
            this.TMRRefreshMem.Tick += new System.EventHandler(this.TMRRefreshMem_Tick);
            // 
            // PNLErrorGen
            // 
            this.PNLErrorGen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.PNLErrorGen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNLErrorGen.Controls.Add(this.BTNRstWdt);
            this.PNLErrorGen.Controls.Add(this.button2);
            this.PNLErrorGen.Controls.Add(this.button3);
            this.PNLErrorGen.Controls.Add(this.label10);
            this.PNLErrorGen.Location = new System.Drawing.Point(805, 250);
            this.PNLErrorGen.Name = "PNLErrorGen";
            this.PNLErrorGen.Size = new System.Drawing.Size(181, 71);
            this.PNLErrorGen.TabIndex = 30;
            // 
            // BTNRstWdt
            // 
            this.BTNRstWdt.Location = new System.Drawing.Point(89, 19);
            this.BTNRstWdt.Name = "BTNRstWdt";
            this.BTNRstWdt.Size = new System.Drawing.Size(63, 21);
            this.BTNRstWdt.TabIndex = 3;
            this.BTNRstWdt.Text = "Rst WDT";
            this.BTNRstWdt.UseVisualStyleBackColor = true;
            this.BTNRstWdt.Click += new System.EventHandler(this.BTNRstWdt_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(26, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 21);
            this.button2.TabIndex = 2;
            this.button2.Text = "WDT";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(26, 43);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(126, 21);
            this.button3.TabIndex = 2;
            this.button3.Text = "Software checksum";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label10.Location = new System.Drawing.Point(42, 4);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Error generator";
            // 
            // PNLSerialNumber
            // 
            this.PNLSerialNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PNLSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNLSerialNumber.Controls.Add(this.TXTSerialNumber);
            this.PNLSerialNumber.Controls.Add(this.label44);
            this.PNLSerialNumber.Location = new System.Drawing.Point(805, 324);
            this.PNLSerialNumber.Name = "PNLSerialNumber";
            this.PNLSerialNumber.Size = new System.Drawing.Size(181, 69);
            this.PNLSerialNumber.TabIndex = 31;
            // 
            // TXTSerialNumber
            // 
            this.TXTSerialNumber.AcceptsReturn = true;
            this.TXTSerialNumber.Font = new System.Drawing.Font("Courier New", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.TXTSerialNumber.Location = new System.Drawing.Point(55, 26);
            this.TXTSerialNumber.MaxLength = 6;
            this.TXTSerialNumber.Name = "TXTSerialNumber";
            this.TXTSerialNumber.Size = new System.Drawing.Size(69, 26);
            this.TXTSerialNumber.TabIndex = 1;
            this.TXTSerialNumber.Text = "WWWWWW";
            this.TXTSerialNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TXTSerialNumber.Validated += new System.EventHandler(this.TXTSerialNumber_Validated);
            this.TXTSerialNumber.Click += new System.EventHandler(this.TXTSerialNumber_Click);
            this.TXTSerialNumber.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXTSerialNumber_KeyPress);
            this.TXTSerialNumber.Enter += new System.EventHandler(this.TXTSerialNumber_Enter);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label44.Location = new System.Drawing.Point(46, 4);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(86, 13);
            this.label44.TabIndex = 0;
            this.label44.Text = "Serial Number";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(901, 450);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(35, 13);
            this.label32.TabIndex = 32;
            this.label32.Text = "label7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(481, 515);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Packets:";
            // 
            // LBLPackets
            // 
            this.LBLPackets.AutoSize = true;
            this.LBLPackets.Location = new System.Drawing.Point(531, 515);
            this.LBLPackets.Name = "LBLPackets";
            this.LBLPackets.Size = new System.Drawing.Size(16, 13);
            this.LBLPackets.TabIndex = 21;
            this.LBLPackets.Text = "---";
            // 
            // BTNLoadDefaults
            // 
            this.BTNLoadDefaults.Location = new System.Drawing.Point(805, 399);
            this.BTNLoadDefaults.Name = "BTNLoadDefaults";
            this.BTNLoadDefaults.Size = new System.Drawing.Size(181, 25);
            this.BTNLoadDefaults.TabIndex = 33;
            this.BTNLoadDefaults.Text = "Load defaults";
            this.BTNLoadDefaults.UseVisualStyleBackColor = true;
            this.BTNLoadDefaults.Click += new System.EventHandler(this.BTNLoadDefaults_Click);
            // 
            // TMRNoCommDetection
            // 
            this.TMRNoCommDetection.Enabled = true;
            this.TMRNoCommDetection.Interval = 500;
            this.TMRNoCommDetection.Tick += new System.EventHandler(this.TMRNoCommDetection_Tick);
            // 
            // pic_bw_background
            // 
            this.pic_bw_background.Image = global::Sensor_Scope.Properties.Resources.bwbackground11;
            this.pic_bw_background.Location = new System.Drawing.Point(803, 176);
            this.pic_bw_background.Name = "pic_bw_background";
            this.pic_bw_background.Size = new System.Drawing.Size(192, 250);
            this.pic_bw_background.TabIndex = 35;
            this.pic_bw_background.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(844, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(109, 37);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 23;
            this.pictureBox1.TabStop = false;
            // 
            // PICGraph
            // 
            this.PICGraph.BackColor = System.Drawing.Color.Black;
            this.PICGraph.Location = new System.Drawing.Point(-1, 0);
            this.PICGraph.Name = "PICGraph";
            this.PICGraph.Size = new System.Drawing.Size(800, 512);
            this.PICGraph.TabIndex = 0;
            this.PICGraph.TabStop = false;
            // 
            // FRMMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(992, 566);
            this.Controls.Add(this.pic_bw_background);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.LBLPackets);
            this.Controls.Add(this.LBLAllp2p);
            this.Controls.Add(this.LBLLastPeak);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BTNAutoCenter);
            this.Controls.Add(this.CHKSaveToFile);
            this.Controls.Add(this.TXTAverage);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.TXTSpan);
            this.Controls.Add(this.TXTMid);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.RADClose);
            this.Controls.Add(this.RADOpen);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CMBPortSelector);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.LBLMid);
            this.Controls.Add(this.PICGraph);
            this.Controls.Add(this.label32);
            this.Controls.Add(this.BTNLoadDefaults);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.PNLStatus);
            this.Controls.Add(this.PNLErrorGen);
            this.Controls.Add(this.PNLSerialNumber);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FRMMain";
            this.Text = "EarlySense Rev 5-4 Sensor-Scope  V1.12 (2 Channels)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FRMMain_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRMMain_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PNLStatus.ResumeLayout(false);
            this.PNLStatus.PerformLayout();
            this.PNLErrorGen.ResumeLayout(false);
            this.PNLErrorGen.PerformLayout();
            this.PNLSerialNumber.ResumeLayout(false);
            this.PNLSerialNumber.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic_bw_background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PICGraph)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.PictureBox PICGraph;
        private System.Windows.Forms.Label LBLMid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CMBPortSelector;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton RADOpen;
        private System.Windows.Forms.RadioButton RADClose;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel TSPortStatus;
        private System.Windows.Forms.ToolStripStatusLabel TSTime;
        private System.Windows.Forms.ToolStripStatusLabel TSDate;
        private System.Windows.Forms.Timer TMRDateAndTime;
        private System.Windows.Forms.TextBox TXTMid;
        private System.Windows.Forms.TextBox TXTSpan;
        private System.Windows.Forms.TextBox TXTAverage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox CHKSaveToFile;
        private System.Windows.Forms.Button BTNAutoCenter;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripStatusLabel TSStrain;
        private System.Windows.Forms.ToolStripStatusLabel TSBadChecksum;
        private System.Windows.Forms.ToolStripStatusLabel TSTimeouts;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label LBLLastPeak;
        private System.Windows.Forms.Label LBLAllp2p;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TextBox TXTFileName;
        private System.Windows.Forms.Button CMDBrowse;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button CMDDownload;
        private System.Windows.Forms.OpenFileDialog DLGFILEFirmwareUpdte;
        private System.Windows.Forms.Button CMDCalcChecksum;
        private System.Windows.Forms.Timer TMRHideChecksum;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer TMRCheckFrames;
        private System.Windows.Forms.Panel PNLStatus;
        private System.Windows.Forms.Label LBLRegCorruption;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label LBLWDTEvents;
        private System.Windows.Forms.Label LBLVersion;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Timer TMRRefreshMem;
        private System.Windows.Forms.Panel PNLErrorGen;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Panel PNLSerialNumber;
        private System.Windows.Forms.TextBox TXTSerialNumber;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LBLPackets;
        private System.Windows.Forms.Button BTNLoadDefaults;
        private System.Windows.Forms.Timer TMRNoCommDetection;
        private System.Windows.Forms.PictureBox pic_bw_background;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button BTNRstWdt;
    }
}

