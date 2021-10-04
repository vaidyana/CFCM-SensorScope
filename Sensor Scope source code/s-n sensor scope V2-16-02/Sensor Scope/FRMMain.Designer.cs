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
            this.RADClose = new System.Windows.Forms.RadioButton();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.TSTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSDate = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSBadChecksum = new System.Windows.Forms.ToolStripStatusLabel();
            this.TSTimeouts = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblX = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblY = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslblZ = new System.Windows.Forms.ToolStripStatusLabel();
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
            this.label32 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.LBLPackets = new System.Windows.Forms.Label();
            this.BTNLoadDefaults = new System.Windows.Forms.Button();
            this.TMRNoCommDetection = new System.Windows.Forms.Timer(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.lblDataChar = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblHourCounter = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnClearHourCounter = new System.Windows.Forms.Button();
            this.btnSetHourCounter = new System.Windows.Forms.Button();
            this.txtHourCounter = new System.Windows.Forms.TextBox();
            this.RADOpen = new System.Windows.Forms.RadioButton();
            this.imgListTestRadio = new System.Windows.Forms.ImageList(this.components);
            this.chkEncrypted = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtNewDataChar = new System.Windows.Forms.TextBox();
            this.btnSetChar = new System.Windows.Forms.Button();
            this.btnReport = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.txtTestersName = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.lblReportGenStatus = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label26 = new System.Windows.Forms.Label();
            this.lblLastReportResult = new System.Windows.Forms.Label();
            this.bgwTestReportGen = new System.ComponentModel.BackgroundWorker();
            this.label44 = new System.Windows.Forms.Label();
            this.txtFullSerial = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btnSetSerial = new System.Windows.Forms.Button();
            this.lblMemVersion = new System.Windows.Forms.Label();
            this.lblMemYear = new System.Windows.Forms.Label();
            this.lblMemSN = new System.Windows.Forms.Label();
            this.lblMemChecksum = new System.Windows.Forms.Label();
            this.chkAutoChecksum = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.PNLSerialNumber = new System.Windows.Forms.Panel();
            this.txtDste = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.picStartAutoTestWF3 = new System.Windows.Forms.PictureBox();
            this.picStartAutoTestWF2 = new System.Windows.Forms.PictureBox();
            this.picStartAutoTestWF1 = new System.Windows.Forms.PictureBox();
            this.pnlDownloading = new System.Windows.Forms.Panel();
            this.prgrsTesterDownload = new System.Windows.Forms.ProgressBar();
            this.label17 = new System.Windows.Forms.Label();
            this.picTesterDisabled = new System.Windows.Forms.PictureBox();
            this.lblTesterStatus = new System.Windows.Forms.Label();
            this.picContinueTesterCalibration = new System.Windows.Forms.PictureBox();
            this.prgrsOverallProgress = new System.Windows.Forms.ProgressBar();
            this.prgrsBenchFile = new System.Windows.Forms.ProgressBar();
            this.lblStatusHeader = new System.Windows.Forms.Label();
            this.picActivateBenchTest = new System.Windows.Forms.PictureBox();
            this.lblBTFolderName = new System.Windows.Forms.Label();
            this.picSelectFolder = new System.Windows.Forms.PictureBox();
            this.lblPleaseWait = new System.Windows.Forms.Label();
            this.picTestManualOperation = new System.Windows.Forms.PictureBox();
            this.txtTestPressure = new System.Windows.Forms.TextBox();
            this.txtTestFreq = new System.Windows.Forms.TextBox();
            this.prgrsTestAutoTest = new System.Windows.Forms.ProgressBar();
            this.lblTestPressureSense = new System.Windows.Forms.Label();
            this.lblTestSerialNumber = new System.Windows.Forms.Label();
            this.lblTestFirmware = new System.Windows.Forms.Label();
            this.picStartTesterCalibration = new System.Windows.Forms.PictureBox();
            this.picCancelTesterCalibration = new System.Windows.Forms.PictureBox();
            this.picAutoTestOff = new System.Windows.Forms.PictureBox();
            this.picStartAutoTestSin = new System.Windows.Forms.PictureBox();
            this.pic_bw_background = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PICGraph = new System.Windows.Forms.PictureBox();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.PNLStatus.SuspendLayout();
            this.PNLErrorGen.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.PNLSerialNumber.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF1)).BeginInit();
            this.pnlDownloading.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTesterDisabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContinueTesterCalibration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picActivateBenchTest)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectFolder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTestManualOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartTesterCalibration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancelTesterCalibration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoTestOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestSin)).BeginInit();
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
            this.LBLMid.Size = new System.Drawing.Size(54, 17);
            this.LBLMid.TabIndex = 2;
            this.LBLMid.Text = "Center:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label1.Location = new System.Drawing.Point(148, 522);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Span:";
            // 
            // CMBPortSelector
            // 
            this.CMBPortSelector.FormattingEnabled = true;
            this.CMBPortSelector.Location = new System.Drawing.Point(834, 517);
            this.CMBPortSelector.Name = "CMBPortSelector";
            this.CMBPortSelector.Size = new System.Drawing.Size(62, 24);
            this.CMBPortSelector.TabIndex = 5;
            this.CMBPortSelector.SelectedIndexChanged += new System.EventHandler(this.CMBPortSelector_SelectedIndexChanged);
            this.CMBPortSelector.Click += new System.EventHandler(this.CMBPortSelector_Click);
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label2.Location = new System.Drawing.Point(802, 520);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Port:";
            // 
            // RADClose
            // 
            this.RADClose.Appearance = System.Windows.Forms.Appearance.Button;
            this.RADClose.AutoSize = true;
            this.RADClose.BackColor = System.Drawing.Color.IndianRed;
            this.RADClose.Checked = true;
            this.RADClose.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RADClose.Location = new System.Drawing.Point(944, 516);
            this.RADClose.Name = "RADClose";
            this.RADClose.Size = new System.Drawing.Size(53, 27);
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
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.TSTime,
            this.TSDate,
            this.TSBadChecksum,
            this.TSTimeouts,
            this.tsslblX,
            this.tsslblY,
            this.tsslblZ,
            this.toolStripStatusLabel1,
            this.TSPortStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 584);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1197, 29);
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // TSTime
            // 
            this.TSTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSTime.Name = "TSTime";
            this.TSTime.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.TSTime.Size = new System.Drawing.Size(4, 24);
            // 
            // TSDate
            // 
            this.TSDate.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSDate.Name = "TSDate";
            this.TSDate.Size = new System.Drawing.Size(4, 24);
            // 
            // TSBadChecksum
            // 
            this.TSBadChecksum.BackColor = System.Drawing.Color.IndianRed;
            this.TSBadChecksum.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSBadChecksum.ForeColor = System.Drawing.Color.White;
            this.TSBadChecksum.Name = "TSBadChecksum";
            this.TSBadChecksum.Size = new System.Drawing.Size(4, 24);
            // 
            // TSTimeouts
            // 
            this.TSTimeouts.BackColor = System.Drawing.Color.IndianRed;
            this.TSTimeouts.ForeColor = System.Drawing.Color.White;
            this.TSTimeouts.Name = "TSTimeouts";
            this.TSTimeouts.Size = new System.Drawing.Size(0, 24);
            // 
            // tsslblX
            // 
            this.tsslblX.Name = "tsslblX";
            this.tsslblX.Size = new System.Drawing.Size(36, 24);
            this.tsslblX.Text = "X = ";
            // 
            // tsslblY
            // 
            this.tsslblY.Name = "tsslblY";
            this.tsslblY.Size = new System.Drawing.Size(35, 24);
            this.tsslblY.Text = "Y = ";
            // 
            // tsslblZ
            // 
            this.tsslblZ.Name = "tsslblZ";
            this.tsslblZ.Size = new System.Drawing.Size(36, 24);
            this.tsslblZ.Text = "Z = ";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoSize = false;
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(1086, 24);
            // 
            // TSPortStatus
            // 
            this.TSPortStatus.BackColor = System.Drawing.Color.IndianRed;
            this.TSPortStatus.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.TSPortStatus.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.TSPortStatus.Name = "TSPortStatus";
            this.TSPortStatus.RightToLeftAutoMirrorImage = true;
            this.TSPortStatus.Size = new System.Drawing.Size(103, 24);
            this.TSPortStatus.Text = "Port is closed.";
            // 
            // TMRDateAndTime
            // 
            this.TMRDateAndTime.Enabled = true;
            this.TMRDateAndTime.Interval = 50;
            this.TMRDateAndTime.Tick += new System.EventHandler(this.TMRDateAndTime_Tick);
            // 
            // TXTMid
            // 
            this.TXTMid.Location = new System.Drawing.Point(39, 518);
            this.TXTMid.Name = "TXTMid";
            this.TXTMid.Size = new System.Drawing.Size(59, 22);
            this.TXTMid.TabIndex = 12;
            this.TXTMid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            this.TXTMid.Leave += new System.EventHandler(this.TXTMid_Leave);
            // 
            // TXTSpan
            // 
            this.TXTSpan.Location = new System.Drawing.Point(184, 518);
            this.TXTSpan.Name = "TXTSpan";
            this.TXTSpan.Size = new System.Drawing.Size(59, 22);
            this.TXTSpan.TabIndex = 13;
            this.TXTSpan.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            this.TXTSpan.Leave += new System.EventHandler(this.TXTSpan_Leave);
            // 
            // TXTAverage
            // 
            this.TXTAverage.Location = new System.Drawing.Point(304, 518);
            this.TXTAverage.Name = "TXTAverage";
            this.TXTAverage.Size = new System.Drawing.Size(27, 22);
            this.TXTAverage.TabIndex = 15;
            this.TXTAverage.Text = "12";
            this.TXTAverage.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CheckNum);
            this.TXTAverage.Leave += new System.EventHandler(this.TXTAverage_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.Desktop;
            this.label5.Location = new System.Drawing.Point(247, 522);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "Averaging:";
            // 
            // CHKSaveToFile
            // 
            this.CHKSaveToFile.AutoSize = true;
            this.CHKSaveToFile.Location = new System.Drawing.Point(5, 541);
            this.CHKSaveToFile.Name = "CHKSaveToFile";
            this.CHKSaveToFile.Size = new System.Drawing.Size(92, 21);
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
            this.label3.Size = new System.Drawing.Size(64, 17);
            this.label3.TabIndex = 18;
            this.label3.Text = "max-min:";
            // 
            // LBLLastPeak
            // 
            this.LBLLastPeak.AutoSize = true;
            this.LBLLastPeak.Location = new System.Drawing.Point(419, 528);
            this.LBLLastPeak.Name = "LBLLastPeak";
            this.LBLLastPeak.Size = new System.Drawing.Size(23, 17);
            this.LBLLastPeak.TabIndex = 20;
            this.LBLLastPeak.Text = "---";
            // 
            // LBLAllp2p
            // 
            this.LBLAllp2p.AutoSize = true;
            this.LBLAllp2p.Location = new System.Drawing.Point(419, 514);
            this.LBLAllp2p.Name = "LBLAllp2p";
            this.LBLAllp2p.Size = new System.Drawing.Size(23, 17);
            this.LBLAllp2p.TabIndex = 21;
            this.LBLAllp2p.Text = "---";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(361, 526);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 22;
            this.label4.Text = "last p-p:";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(3, 16);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(118, 17);
            this.label38.TabIndex = 24;
            this.label38.Text = "File to download :";
            // 
            // TXTFileName
            // 
            this.TXTFileName.Location = new System.Drawing.Point(2, 32);
            this.TXTFileName.Name = "TXTFileName";
            this.TXTFileName.Size = new System.Drawing.Size(175, 22);
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
            this.label6.Size = new System.Drawing.Size(144, 17);
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
            this.label40.Size = new System.Drawing.Size(69, 17);
            this.label40.TabIndex = 31;
            this.label40.Text = "Progress:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(56, 112);
            this.progressBar1.Maximum = 120;
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
            this.LBLRegCorruption.Size = new System.Drawing.Size(32, 17);
            this.LBLRegCorruption.TabIndex = 12;
            this.LBLRegCorruption.Text = "000";
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Location = new System.Drawing.Point(4, 51);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(106, 17);
            this.label47.TabIndex = 11;
            this.label47.Text = "Reg corruption:";
            // 
            // LBLWDTEvents
            // 
            this.LBLWDTEvents.AutoSize = true;
            this.LBLWDTEvents.Location = new System.Drawing.Point(81, 36);
            this.LBLWDTEvents.Name = "LBLWDTEvents";
            this.LBLWDTEvents.Size = new System.Drawing.Size(32, 17);
            this.LBLWDTEvents.TabIndex = 4;
            this.LBLWDTEvents.Text = "000";
            // 
            // LBLVersion
            // 
            this.LBLVersion.AutoSize = true;
            this.LBLVersion.Location = new System.Drawing.Point(81, 20);
            this.LBLVersion.Name = "LBLVersion";
            this.LBLVersion.Size = new System.Drawing.Size(44, 17);
            this.LBLVersion.TabIndex = 4;
            this.LBLVersion.Text = "00.00";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(4, 36);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(90, 17);
            this.label22.TabIndex = 1;
            this.label22.Text = "WDT events:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(4, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(60, 17);
            this.label21.TabIndex = 1;
            this.label21.Text = "Version:";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label20.Location = new System.Drawing.Point(44, 4);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(118, 17);
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
            this.label10.Size = new System.Drawing.Size(121, 17);
            this.label10.TabIndex = 0;
            this.label10.Text = "Error generator";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(901, 522);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(46, 17);
            this.label32.TabIndex = 32;
            this.label32.Text = "label7";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(481, 515);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 17);
            this.label7.TabIndex = 18;
            this.label7.Text = "Packets:";
            // 
            // LBLPackets
            // 
            this.LBLPackets.AutoSize = true;
            this.LBLPackets.Location = new System.Drawing.Point(531, 515);
            this.LBLPackets.Name = "LBLPackets";
            this.LBLPackets.Size = new System.Drawing.Size(23, 17);
            this.LBLPackets.TabIndex = 21;
            this.LBLPackets.Text = "---";
            // 
            // BTNLoadDefaults
            // 
            this.BTNLoadDefaults.Location = new System.Drawing.Point(694, 516);
            this.BTNLoadDefaults.Name = "BTNLoadDefaults";
            this.BTNLoadDefaults.Size = new System.Drawing.Size(105, 25);
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
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(481, 526);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 17);
            this.label14.TabIndex = 86;
            this.label14.Text = "Data Char:";
            // 
            // lblDataChar
            // 
            this.lblDataChar.AutoSize = true;
            this.lblDataChar.Location = new System.Drawing.Point(536, 526);
            this.lblDataChar.Name = "lblDataChar";
            this.lblDataChar.Size = new System.Drawing.Size(23, 17);
            this.lblDataChar.TabIndex = 85;
            this.lblDataChar.Text = "---";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label15.Location = new System.Drawing.Point(47, 1);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(105, 17);
            this.label15.TabIndex = 9;
            this.label15.Text = "Hour Counter";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(0, 18);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(59, 17);
            this.label16.TabIndex = 87;
            this.label16.Text = "Current:";
            // 
            // lblHourCounter
            // 
            this.lblHourCounter.AutoSize = true;
            this.lblHourCounter.Location = new System.Drawing.Point(44, 18);
            this.lblHourCounter.Name = "lblHourCounter";
            this.lblHourCounter.Size = new System.Drawing.Size(33, 17);
            this.lblHourCounter.TabIndex = 88;
            this.lblHourCounter.Text = "-----";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnClearHourCounter);
            this.panel2.Controls.Add(this.btnSetHourCounter);
            this.panel2.Controls.Add(this.txtHourCounter);
            this.panel2.Controls.Add(this.lblHourCounter);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.label15);
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Location = new System.Drawing.Point(805, 454);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(181, 58);
            this.panel2.TabIndex = 89;
            // 
            // btnClearHourCounter
            // 
            this.btnClearHourCounter.Location = new System.Drawing.Point(120, 32);
            this.btnClearHourCounter.Name = "btnClearHourCounter";
            this.btnClearHourCounter.Size = new System.Drawing.Size(55, 23);
            this.btnClearHourCounter.TabIndex = 91;
            this.btnClearHourCounter.Text = "Clear";
            this.btnClearHourCounter.UseVisualStyleBackColor = true;
            this.btnClearHourCounter.Click += new System.EventHandler(this.btnClearHourCounter_Click);
            // 
            // btnSetHourCounter
            // 
            this.btnSetHourCounter.Location = new System.Drawing.Point(61, 32);
            this.btnSetHourCounter.Name = "btnSetHourCounter";
            this.btnSetHourCounter.Size = new System.Drawing.Size(55, 23);
            this.btnSetHourCounter.TabIndex = 90;
            this.btnSetHourCounter.Text = "<==Set";
            this.btnSetHourCounter.UseVisualStyleBackColor = true;
            this.btnSetHourCounter.Click += new System.EventHandler(this.btnSetHourCounter_Click);
            // 
            // txtHourCounter
            // 
            this.txtHourCounter.Location = new System.Drawing.Point(3, 34);
            this.txtHourCounter.Name = "txtHourCounter";
            this.txtHourCounter.Size = new System.Drawing.Size(52, 22);
            this.txtHourCounter.TabIndex = 89;
            this.txtHourCounter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TXT_KeyPressCheckInteger);
            // 
            // RADOpen
            // 
            this.RADOpen.Appearance = System.Windows.Forms.Appearance.Button;
            this.RADOpen.AutoSize = true;
            this.RADOpen.BackColor = System.Drawing.Color.Lime;
            this.RADOpen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.RADOpen.Location = new System.Drawing.Point(898, 516);
            this.RADOpen.Name = "RADOpen";
            this.RADOpen.Size = new System.Drawing.Size(53, 27);
            this.RADOpen.TabIndex = 7;
            this.RADOpen.Text = "Open";
            this.RADOpen.UseVisualStyleBackColor = false;
            this.RADOpen.CheckedChanged += new System.EventHandler(this.RADOpen_CheckedChanged);
            // 
            // imgListTestRadio
            // 
            this.imgListTestRadio.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgListTestRadio.ImageStream")));
            this.imgListTestRadio.TransparentColor = System.Drawing.Color.Transparent;
            this.imgListTestRadio.Images.SetKeyName(0, "Manual_button_disabled.bmp");
            this.imgListTestRadio.Images.SetKeyName(1, "Manual_button_enabled.bmp");
            // 
            // chkEncrypted
            // 
            this.chkEncrypted.AutoSize = true;
            this.chkEncrypted.Checked = true;
            this.chkEncrypted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEncrypted.Location = new System.Drawing.Point(84, 541);
            this.chkEncrypted.Name = "chkEncrypted";
            this.chkEncrypted.Size = new System.Drawing.Size(125, 21);
            this.chkEncrypted.TabIndex = 91;
            this.chkEncrypted.Text = "Use encryption";
            this.chkEncrypted.UseVisualStyleBackColor = true;
            this.chkEncrypted.CheckedChanged += new System.EventHandler(this.chkEncrypted_CheckedChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(464, 545);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(99, 17);
            this.label18.TabIndex = 92;
            this.label18.Text = "set Data Char:";
            // 
            // txtNewDataChar
            // 
            this.txtNewDataChar.Location = new System.Drawing.Point(545, 542);
            this.txtNewDataChar.MaxLength = 1;
            this.txtNewDataChar.Name = "txtNewDataChar";
            this.txtNewDataChar.Size = new System.Drawing.Size(25, 22);
            this.txtNewDataChar.TabIndex = 93;
            // 
            // btnSetChar
            // 
            this.btnSetChar.Enabled = false;
            this.btnSetChar.Location = new System.Drawing.Point(576, 540);
            this.btnSetChar.Name = "btnSetChar";
            this.btnSetChar.Size = new System.Drawing.Size(49, 23);
            this.btnSetChar.TabIndex = 94;
            this.btnSetChar.Text = "Set";
            this.btnSetChar.UseVisualStyleBackColor = true;
            this.btnSetChar.Click += new System.EventHandler(this.btnSetChar_Click);
            // 
            // btnReport
            // 
            this.btnReport.Location = new System.Drawing.Point(5, 565);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(138, 23);
            this.btnReport.TabIndex = 96;
            this.btnReport.Text = "Generate report";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(149, 570);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(102, 17);
            this.label23.TabIndex = 97;
            this.label23.Text = "Tester\'s name:";
            // 
            // txtTestersName
            // 
            this.txtTestersName.Location = new System.Drawing.Point(231, 567);
            this.txtTestersName.Name = "txtTestersName";
            this.txtTestersName.Size = new System.Drawing.Size(100, 22);
            this.txtTestersName.TabIndex = 98;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(501, 570);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(163, 17);
            this.label24.TabIndex = 99;
            this.label24.Text = "Report generator status:";
            // 
            // lblReportGenStatus
            // 
            this.lblReportGenStatus.AutoSize = true;
            this.lblReportGenStatus.Location = new System.Drawing.Point(628, 570);
            this.lblReportGenStatus.Name = "lblReportGenStatus";
            this.lblReportGenStatus.Size = new System.Drawing.Size(30, 17);
            this.lblReportGenStatus.TabIndex = 100;
            this.lblReportGenStatus.Text = "Idle";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(-5, 564);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1201, 1);
            this.panel4.TabIndex = 101;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(724, 570);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(78, 17);
            this.label26.TabIndex = 103;
            this.label26.Text = "Last result:";
            // 
            // lblLastReportResult
            // 
            this.lblLastReportResult.AutoSize = true;
            this.lblLastReportResult.Location = new System.Drawing.Point(785, 570);
            this.lblLastReportResult.Name = "lblLastReportResult";
            this.lblLastReportResult.Size = new System.Drawing.Size(0, 17);
            this.lblLastReportResult.TabIndex = 104;
            // 
            // bgwTestReportGen
            // 
            this.bgwTestReportGen.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwTestReportGen_DoWork);
            this.bgwTestReportGen.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwTestReportGen_RunWorkerCompleted);
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.label44.Location = new System.Drawing.Point(46, 0);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(111, 17);
            this.label44.TabIndex = 0;
            this.label44.Text = "Serial Number";
            // 
            // txtFullSerial
            // 
            this.txtFullSerial.Location = new System.Drawing.Point(93, 74);
            this.txtFullSerial.MaxLength = 10;
            this.txtFullSerial.Name = "txtFullSerial";
            this.txtFullSerial.Size = new System.Drawing.Size(69, 22);
            this.txtFullSerial.TabIndex = 2;
            this.txtFullSerial.TextChanged += new System.EventHandler(this.txtFullSerial_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(58, 36);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 17);
            this.label12.TabIndex = 3;
            this.label12.Text = "Checksum:";
            // 
            // btnSetSerial
            // 
            this.btnSetSerial.Enabled = false;
            this.btnSetSerial.Location = new System.Drawing.Point(14, 100);
            this.btnSetSerial.Name = "btnSetSerial";
            this.btnSetSerial.Size = new System.Drawing.Size(75, 23);
            this.btnSetSerial.TabIndex = 5;
            this.btnSetSerial.Text = "Set";
            this.btnSetSerial.UseVisualStyleBackColor = true;
            this.btnSetSerial.Click += new System.EventHandler(this.btnSetSerial_Click);
            // 
            // lblMemVersion
            // 
            this.lblMemVersion.AutoSize = true;
            this.lblMemVersion.Location = new System.Drawing.Point(44, 20);
            this.lblMemVersion.Name = "lblMemVersion";
            this.lblMemVersion.Size = new System.Drawing.Size(0, 17);
            this.lblMemVersion.TabIndex = 5;
            // 
            // lblMemYear
            // 
            this.lblMemYear.AutoSize = true;
            this.lblMemYear.Location = new System.Drawing.Point(113, 20);
            this.lblMemYear.Name = "lblMemYear";
            this.lblMemYear.Size = new System.Drawing.Size(0, 17);
            this.lblMemYear.TabIndex = 5;
            // 
            // lblMemSN
            // 
            this.lblMemSN.AutoSize = true;
            this.lblMemSN.Location = new System.Drawing.Point(43, 36);
            this.lblMemSN.Name = "lblMemSN";
            this.lblMemSN.Size = new System.Drawing.Size(0, 17);
            this.lblMemSN.TabIndex = 5;
            // 
            // lblMemChecksum
            // 
            this.lblMemChecksum.AutoSize = true;
            this.lblMemChecksum.Location = new System.Drawing.Point(116, 36);
            this.lblMemChecksum.Name = "lblMemChecksum";
            this.lblMemChecksum.Size = new System.Drawing.Size(0, 17);
            this.lblMemChecksum.TabIndex = 5;
            // 
            // chkAutoChecksum
            // 
            this.chkAutoChecksum.AutoSize = true;
            this.chkAutoChecksum.Checked = true;
            this.chkAutoChecksum.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAutoChecksum.Location = new System.Drawing.Point(93, 101);
            this.chkAutoChecksum.Name = "chkAutoChecksum";
            this.chkAutoChecksum.Size = new System.Drawing.Size(111, 21);
            this.chkAutoChecksum.TabIndex = 6;
            this.chkAutoChecksum.Text = "Auto chksum";
            this.chkAutoChecksum.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(3, 55);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(110, 17);
            this.label13.TabIndex = 7;
            this.label13.Text = "Manual chksum:";
            this.label13.Visible = false;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(93, 53);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(40, 22);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDown1.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(60, 17);
            this.label8.TabIndex = 9;
            this.label8.Text = "Version:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(73, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(52, 17);
            this.label9.TabIndex = 10;
            this.label9.Text = "MMYY:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 17);
            this.label11.TabIndex = 11;
            this.label11.Text = "S.N:";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(23, 77);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(87, 17);
            this.label19.TabIndex = 12;
            this.label19.Text = "Set serial to:";
            // 
            // PNLSerialNumber
            // 
            this.PNLSerialNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.PNLSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PNLSerialNumber.Controls.Add(this.label19);
            this.PNLSerialNumber.Controls.Add(this.label11);
            this.PNLSerialNumber.Controls.Add(this.label9);
            this.PNLSerialNumber.Controls.Add(this.label8);
            this.PNLSerialNumber.Controls.Add(this.numericUpDown1);
            this.PNLSerialNumber.Controls.Add(this.label13);
            this.PNLSerialNumber.Controls.Add(this.chkAutoChecksum);
            this.PNLSerialNumber.Controls.Add(this.lblMemChecksum);
            this.PNLSerialNumber.Controls.Add(this.lblMemSN);
            this.PNLSerialNumber.Controls.Add(this.lblMemYear);
            this.PNLSerialNumber.Controls.Add(this.lblMemVersion);
            this.PNLSerialNumber.Controls.Add(this.btnSetSerial);
            this.PNLSerialNumber.Controls.Add(this.label12);
            this.PNLSerialNumber.Controls.Add(this.txtFullSerial);
            this.PNLSerialNumber.Controls.Add(this.label44);
            this.PNLSerialNumber.Location = new System.Drawing.Point(805, 324);
            this.PNLSerialNumber.Name = "PNLSerialNumber";
            this.PNLSerialNumber.Size = new System.Drawing.Size(181, 128);
            this.PNLSerialNumber.TabIndex = 95;
            // 
            // txtDste
            // 
            this.txtDste.Location = new System.Drawing.Point(388, 567);
            this.txtDste.Name = "txtDste";
            this.txtDste.Size = new System.Drawing.Size(100, 22);
            this.txtDste.TabIndex = 106;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(337, 570);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(61, 17);
            this.label25.TabIndex = 105;
            this.label25.Text = "DSTE #:";
            // 
            // panel3
            // 
            this.panel3.BackgroundImage = global::Sensor_Scope.Properties.Resources.Enabled;
            this.panel3.Controls.Add(this.picTesterDisabled);
            this.panel3.Controls.Add(this.picStartAutoTestWF3);
            this.panel3.Controls.Add(this.picStartAutoTestWF2);
            this.panel3.Controls.Add(this.picStartAutoTestWF1);
            this.panel3.Controls.Add(this.pnlDownloading);
            this.panel3.Controls.Add(this.lblTesterStatus);
            this.panel3.Controls.Add(this.picContinueTesterCalibration);
            this.panel3.Controls.Add(this.prgrsOverallProgress);
            this.panel3.Controls.Add(this.prgrsBenchFile);
            this.panel3.Controls.Add(this.lblStatusHeader);
            this.panel3.Controls.Add(this.picActivateBenchTest);
            this.panel3.Controls.Add(this.lblBTFolderName);
            this.panel3.Controls.Add(this.picSelectFolder);
            this.panel3.Controls.Add(this.lblPleaseWait);
            this.panel3.Controls.Add(this.picTestManualOperation);
            this.panel3.Controls.Add(this.txtTestPressure);
            this.panel3.Controls.Add(this.txtTestFreq);
            this.panel3.Controls.Add(this.prgrsTestAutoTest);
            this.panel3.Controls.Add(this.lblTestPressureSense);
            this.panel3.Controls.Add(this.lblTestSerialNumber);
            this.panel3.Controls.Add(this.lblTestFirmware);
            this.panel3.Controls.Add(this.picStartTesterCalibration);
            this.panel3.Controls.Add(this.picCancelTesterCalibration);
            this.panel3.Controls.Add(this.picAutoTestOff);
            this.panel3.Controls.Add(this.picStartAutoTestSin);
            this.panel3.Location = new System.Drawing.Point(989, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(208, 550);
            this.panel3.TabIndex = 90;
            // 
            // picStartAutoTestWF3
            // 
            this.picStartAutoTestWF3.BackColor = System.Drawing.Color.Transparent;
            this.picStartAutoTestWF3.Location = new System.Drawing.Point(90, 182);
            this.picStartAutoTestWF3.Name = "picStartAutoTestWF3";
            this.picStartAutoTestWF3.Size = new System.Drawing.Size(26, 23);
            this.picStartAutoTestWF3.TabIndex = 96;
            this.picStartAutoTestWF3.TabStop = false;
            this.picStartAutoTestWF3.Click += new System.EventHandler(this.picStartAutoTestWF3_Click);
            // 
            // picStartAutoTestWF2
            // 
            this.picStartAutoTestWF2.BackColor = System.Drawing.Color.Transparent;
            this.picStartAutoTestWF2.Location = new System.Drawing.Point(59, 183);
            this.picStartAutoTestWF2.Name = "picStartAutoTestWF2";
            this.picStartAutoTestWF2.Size = new System.Drawing.Size(26, 23);
            this.picStartAutoTestWF2.TabIndex = 95;
            this.picStartAutoTestWF2.TabStop = false;
            this.picStartAutoTestWF2.Click += new System.EventHandler(this.picStartAutoTestWF2_Click);
            // 
            // picStartAutoTestWF1
            // 
            this.picStartAutoTestWF1.BackColor = System.Drawing.Color.Transparent;
            this.picStartAutoTestWF1.Location = new System.Drawing.Point(25, 183);
            this.picStartAutoTestWF1.Name = "picStartAutoTestWF1";
            this.picStartAutoTestWF1.Size = new System.Drawing.Size(27, 23);
            this.picStartAutoTestWF1.TabIndex = 94;
            this.picStartAutoTestWF1.TabStop = false;
            this.picStartAutoTestWF1.Click += new System.EventHandler(this.picStartAutoTestWF1_Click);
            // 
            // pnlDownloading
            // 
            this.pnlDownloading.Controls.Add(this.prgrsTesterDownload);
            this.pnlDownloading.Controls.Add(this.label17);
            this.pnlDownloading.Location = new System.Drawing.Point(20, 51);
            this.pnlDownloading.Name = "pnlDownloading";
            this.pnlDownloading.Size = new System.Drawing.Size(170, 99);
            this.pnlDownloading.TabIndex = 92;
            this.pnlDownloading.Visible = false;
            // 
            // prgrsTesterDownload
            // 
            this.prgrsTesterDownload.Location = new System.Drawing.Point(6, 32);
            this.prgrsTesterDownload.Name = "prgrsTesterDownload";
            this.prgrsTesterDownload.Size = new System.Drawing.Size(155, 23);
            this.prgrsTesterDownload.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgrsTesterDownload.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(2, 9);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(181, 17);
            this.label17.TabIndex = 0;
            this.label17.Text = "Downloading firmware...";
            // 
            // picTesterDisabled
            // 
            this.picTesterDisabled.Image = global::Sensor_Scope.Properties.Resources.Disabled;
            this.picTesterDisabled.Location = new System.Drawing.Point(0, 0);
            this.picTesterDisabled.Name = "picTesterDisabled";
            this.picTesterDisabled.Size = new System.Drawing.Size(208, 550);
            this.picTesterDisabled.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTesterDisabled.TabIndex = 0;
            this.picTesterDisabled.TabStop = false;
            // 
            // lblTesterStatus
            // 
            this.lblTesterStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblTesterStatus.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTesterStatus.Location = new System.Drawing.Point(18, 74);
            this.lblTesterStatus.Name = "lblTesterStatus";
            this.lblTesterStatus.Size = new System.Drawing.Size(172, 72);
            this.lblTesterStatus.TabIndex = 6;
            this.lblTesterStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picContinueTesterCalibration
            // 
            this.picContinueTesterCalibration.BackColor = System.Drawing.Color.Transparent;
            this.picContinueTesterCalibration.Location = new System.Drawing.Point(81, 463);
            this.picContinueTesterCalibration.Name = "picContinueTesterCalibration";
            this.picContinueTesterCalibration.Size = new System.Drawing.Size(45, 20);
            this.picContinueTesterCalibration.TabIndex = 93;
            this.picContinueTesterCalibration.TabStop = false;
            this.picContinueTesterCalibration.Click += new System.EventHandler(this.picContinueTesterCalibration_Click);
            // 
            // prgrsOverallProgress
            // 
            this.prgrsOverallProgress.Location = new System.Drawing.Point(27, 136);
            this.prgrsOverallProgress.Maximum = 255;
            this.prgrsOverallProgress.Name = "prgrsOverallProgress";
            this.prgrsOverallProgress.Size = new System.Drawing.Size(156, 10);
            this.prgrsOverallProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgrsOverallProgress.TabIndex = 20;
            this.prgrsOverallProgress.Visible = false;
            // 
            // prgrsBenchFile
            // 
            this.prgrsBenchFile.Location = new System.Drawing.Point(26, 98);
            this.prgrsBenchFile.Maximum = 255;
            this.prgrsBenchFile.Name = "prgrsBenchFile";
            this.prgrsBenchFile.Size = new System.Drawing.Size(156, 10);
            this.prgrsBenchFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgrsBenchFile.TabIndex = 19;
            this.prgrsBenchFile.Visible = false;
            // 
            // lblStatusHeader
            // 
            this.lblStatusHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblStatusHeader.Font = new System.Drawing.Font("Arial", 11.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatusHeader.Location = new System.Drawing.Point(17, 57);
            this.lblStatusHeader.Name = "lblStatusHeader";
            this.lblStatusHeader.Size = new System.Drawing.Size(173, 23);
            this.lblStatusHeader.TabIndex = 18;
            this.lblStatusHeader.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // picActivateBenchTest
            // 
            this.picActivateBenchTest.Location = new System.Drawing.Point(111, 373);
            this.picActivateBenchTest.Name = "picActivateBenchTest";
            this.picActivateBenchTest.Size = new System.Drawing.Size(20, 20);
            this.picActivateBenchTest.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picActivateBenchTest.TabIndex = 17;
            this.picActivateBenchTest.TabStop = false;
            this.picActivateBenchTest.Click += new System.EventHandler(this.picActivateBenchTest_Click);
            // 
            // lblBTFolderName
            // 
            this.lblBTFolderName.BackColor = System.Drawing.Color.Transparent;
            this.lblBTFolderName.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblBTFolderName.Location = new System.Drawing.Point(74, 401);
            this.lblBTFolderName.Name = "lblBTFolderName";
            this.lblBTFolderName.Size = new System.Drawing.Size(109, 16);
            this.lblBTFolderName.TabIndex = 16;
            this.lblBTFolderName.Click += new System.EventHandler(this.lblBTFolderName_Click);
            // 
            // picSelectFolder
            // 
            this.picSelectFolder.BackColor = System.Drawing.Color.Transparent;
            this.picSelectFolder.Location = new System.Drawing.Point(19, 399);
            this.picSelectFolder.Name = "picSelectFolder";
            this.picSelectFolder.Size = new System.Drawing.Size(171, 19);
            this.picSelectFolder.TabIndex = 15;
            this.picSelectFolder.TabStop = false;
            this.picSelectFolder.Click += new System.EventHandler(this.picSelectFolder_Click);
            // 
            // lblPleaseWait
            // 
            this.lblPleaseWait.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.lblPleaseWait.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblPleaseWait.ForeColor = System.Drawing.Color.Red;
            this.lblPleaseWait.Location = new System.Drawing.Point(18, 74);
            this.lblPleaseWait.Name = "lblPleaseWait";
            this.lblPleaseWait.Size = new System.Drawing.Size(172, 72);
            this.lblPleaseWait.TabIndex = 14;
            this.lblPleaseWait.Text = "Please wait...";
            this.lblPleaseWait.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picTestManualOperation
            // 
            this.picTestManualOperation.Location = new System.Drawing.Point(111, 291);
            this.picTestManualOperation.Name = "picTestManualOperation";
            this.picTestManualOperation.Size = new System.Drawing.Size(20, 20);
            this.picTestManualOperation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTestManualOperation.TabIndex = 13;
            this.picTestManualOperation.TabStop = false;
            this.picTestManualOperation.Click += new System.EventHandler(this.picTestManualOperation_Click);
            // 
            // txtTestPressure
            // 
            this.txtTestPressure.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(194)))), ((int)(((byte)(218)))));
            this.txtTestPressure.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTestPressure.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtTestPressure.Location = new System.Drawing.Point(116, 234);
            this.txtTestPressure.Name = "txtTestPressure";
            this.txtTestPressure.Size = new System.Drawing.Size(65, 18);
            this.txtTestPressure.TabIndex = 12;
            this.txtTestPressure.Text = "A";
            this.txtTestPressure.Click += new System.EventHandler(this.txtTestPressure_Click);
            this.txtTestPressure.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtTestPressure.Validating += new System.ComponentModel.CancelEventHandler(this.txtTestPressure_Validating);
            // 
            // txtTestFreq
            // 
            this.txtTestFreq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(194)))), ((int)(((byte)(218)))));
            this.txtTestFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTestFreq.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.txtTestFreq.Location = new System.Drawing.Point(116, 319);
            this.txtTestFreq.Name = "txtTestFreq";
            this.txtTestFreq.Size = new System.Drawing.Size(67, 18);
            this.txtTestFreq.TabIndex = 11;
            this.txtTestFreq.Text = "A";
            this.txtTestFreq.Click += new System.EventHandler(this.txtTestFreq_Click);
            this.txtTestFreq.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_KeyPress);
            this.txtTestFreq.Validating += new System.ComponentModel.CancelEventHandler(this.txtTestFreq_Validating);
            // 
            // prgrsTestAutoTest
            // 
            this.prgrsTestAutoTest.Location = new System.Drawing.Point(28, 130);
            this.prgrsTestAutoTest.Maximum = 255;
            this.prgrsTestAutoTest.Name = "prgrsTestAutoTest";
            this.prgrsTestAutoTest.Size = new System.Drawing.Size(156, 10);
            this.prgrsTestAutoTest.TabIndex = 10;
            // 
            // lblTestPressureSense
            // 
            this.lblTestPressureSense.AutoSize = true;
            this.lblTestPressureSense.BackColor = System.Drawing.Color.Transparent;
            this.lblTestPressureSense.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTestPressureSense.Location = new System.Drawing.Point(114, 493);
            this.lblTestPressureSense.Name = "lblTestPressureSense";
            this.lblTestPressureSense.Size = new System.Drawing.Size(32, 17);
            this.lblTestPressureSense.TabIndex = 9;
            this.lblTestPressureSense.Text = "000";
            // 
            // lblTestSerialNumber
            // 
            this.lblTestSerialNumber.AutoSize = true;
            this.lblTestSerialNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblTestSerialNumber.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTestSerialNumber.Location = new System.Drawing.Point(114, 509);
            this.lblTestSerialNumber.Name = "lblTestSerialNumber";
            this.lblTestSerialNumber.Size = new System.Drawing.Size(32, 17);
            this.lblTestSerialNumber.TabIndex = 8;
            this.lblTestSerialNumber.Text = "000";
            // 
            // lblTestFirmware
            // 
            this.lblTestFirmware.AutoSize = true;
            this.lblTestFirmware.BackColor = System.Drawing.Color.Transparent;
            this.lblTestFirmware.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.lblTestFirmware.Location = new System.Drawing.Point(114, 524);
            this.lblTestFirmware.Name = "lblTestFirmware";
            this.lblTestFirmware.Size = new System.Drawing.Size(32, 17);
            this.lblTestFirmware.TabIndex = 7;
            this.lblTestFirmware.Text = "000";
            // 
            // picStartTesterCalibration
            // 
            this.picStartTesterCalibration.BackColor = System.Drawing.Color.Transparent;
            this.picStartTesterCalibration.Location = new System.Drawing.Point(26, 463);
            this.picStartTesterCalibration.Name = "picStartTesterCalibration";
            this.picStartTesterCalibration.Size = new System.Drawing.Size(46, 20);
            this.picStartTesterCalibration.TabIndex = 4;
            this.picStartTesterCalibration.TabStop = false;
            this.picStartTesterCalibration.Click += new System.EventHandler(this.picStartTesterCalibration_Click);
            // 
            // picCancelTesterCalibration
            // 
            this.picCancelTesterCalibration.BackColor = System.Drawing.Color.Transparent;
            this.picCancelTesterCalibration.Location = new System.Drawing.Point(136, 463);
            this.picCancelTesterCalibration.Name = "picCancelTesterCalibration";
            this.picCancelTesterCalibration.Size = new System.Drawing.Size(47, 20);
            this.picCancelTesterCalibration.TabIndex = 3;
            this.picCancelTesterCalibration.TabStop = false;
            this.picCancelTesterCalibration.Click += new System.EventHandler(this.picCancelTesterCalibration_Click);
            // 
            // picAutoTestOff
            // 
            this.picAutoTestOff.BackColor = System.Drawing.Color.Transparent;
            this.picAutoTestOff.Location = new System.Drawing.Point(155, 183);
            this.picAutoTestOff.Name = "picAutoTestOff";
            this.picAutoTestOff.Size = new System.Drawing.Size(29, 23);
            this.picAutoTestOff.TabIndex = 2;
            this.picAutoTestOff.TabStop = false;
            this.picAutoTestOff.Click += new System.EventHandler(this.picCancelAutoTest_Click);
            // 
            // picStartAutoTestSin
            // 
            this.picStartAutoTestSin.BackColor = System.Drawing.Color.Transparent;
            this.picStartAutoTestSin.Location = new System.Drawing.Point(123, 183);
            this.picStartAutoTestSin.Name = "picStartAutoTestSin";
            this.picStartAutoTestSin.Size = new System.Drawing.Size(27, 23);
            this.picStartAutoTestSin.TabIndex = 1;
            this.picStartAutoTestSin.TabStop = false;
            this.picStartAutoTestSin.Click += new System.EventHandler(this.picStartAutoTest_Click);
            // 
            // pic_bw_background
            // 
            this.pic_bw_background.Image = global::Sensor_Scope.Properties.Resources.bwbackground2;
            this.pic_bw_background.Location = new System.Drawing.Point(803, 176);
            this.pic_bw_background.Name = "pic_bw_background";
            this.pic_bw_background.Size = new System.Drawing.Size(186, 337);
            this.pic_bw_background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic_bw_background.TabIndex = 35;
            this.pic_bw_background.TabStop = false;
            this.pic_bw_background.Tag = "803, 176";
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
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(1197, 613);
            this.Controls.Add(this.txtDste);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.lblLastReportResult);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lblReportGenStatus);
            this.Controls.Add(this.label24);
            this.Controls.Add(this.txtTestersName);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.btnReport);
            this.Controls.Add(this.PNLSerialNumber);
            this.Controls.Add(this.btnSetChar);
            this.Controls.Add(this.txtNewDataChar);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.chkEncrypted);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblDataChar);
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
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FRMMain";
            this.Tag = "1203, 613";
            this.Text = "3ple EarlySense Sensor-Scope  V2.16.02";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FRMMain_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.FRMMain_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PNLStatus.ResumeLayout(false);
            this.PNLStatus.PerformLayout();
            this.PNLErrorGen.ResumeLayout(false);
            this.PNLErrorGen.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.PNLSerialNumber.ResumeLayout(false);
            this.PNLSerialNumber.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestWF1)).EndInit();
            this.pnlDownloading.ResumeLayout(false);
            this.pnlDownloading.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picTesterDisabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picContinueTesterCalibration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picActivateBenchTest)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSelectFolder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTestManualOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartTesterCalibration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCancelTesterCalibration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoTestOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picStartAutoTestSin)).EndInit();
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
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label LBLPackets;
        private System.Windows.Forms.Button BTNLoadDefaults;
        private System.Windows.Forms.Timer TMRNoCommDetection;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button BTNRstWdt;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ToolStripStatusLabel tsslblX;
        private System.Windows.Forms.ToolStripStatusLabel tsslblY;
        private System.Windows.Forms.ToolStripStatusLabel tsslblZ;
        private System.Windows.Forms.Label lblDataChar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnClearHourCounter;
        private System.Windows.Forms.Button btnSetHourCounter;
        private System.Windows.Forms.TextBox txtHourCounter;
        private System.Windows.Forms.Label lblHourCounter;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox picStartTesterCalibration;
        private System.Windows.Forms.PictureBox picCancelTesterCalibration;
        private System.Windows.Forms.Label lblTestFirmware;
        private System.Windows.Forms.Label lblTestPressureSense;
        private System.Windows.Forms.Label lblTestSerialNumber;
        private System.Windows.Forms.ProgressBar prgrsTestAutoTest;
        private System.Windows.Forms.TextBox txtTestFreq;
        private System.Windows.Forms.TextBox txtTestPressure;
        private System.Windows.Forms.RadioButton RADOpen;
        private System.Windows.Forms.ImageList imgListTestRadio;
        private System.Windows.Forms.Label lblPleaseWait;
        private System.Windows.Forms.PictureBox pic_bw_background;
        private System.Windows.Forms.CheckBox chkEncrypted;
        private System.Windows.Forms.PictureBox picTestManualOperation;
        private System.Windows.Forms.PictureBox picAutoTestOff;
        private System.Windows.Forms.PictureBox picStartAutoTestSin;
        private System.Windows.Forms.PictureBox picSelectFolder;
        private System.Windows.Forms.Label lblBTFolderName;
        private System.Windows.Forms.PictureBox picActivateBenchTest;
        private System.Windows.Forms.Label lblStatusHeader;
        private System.Windows.Forms.ProgressBar prgrsOverallProgress;
        private System.Windows.Forms.ProgressBar prgrsBenchFile;
        private System.Windows.Forms.PictureBox picContinueTesterCalibration;
        private System.Windows.Forms.Label lblTesterStatus;
        private System.Windows.Forms.Button btnSetChar;
        private System.Windows.Forms.TextBox txtNewDataChar;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.TextBox txtTestersName;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label lblReportGenStatus;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblLastReportResult;
        private System.Windows.Forms.Label label26;
        private System.ComponentModel.BackgroundWorker bgwTestReportGen;
        private System.Windows.Forms.Panel PNLSerialNumber;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkAutoChecksum;
        private System.Windows.Forms.Label lblMemChecksum;
        private System.Windows.Forms.Label lblMemSN;
        private System.Windows.Forms.Label lblMemYear;
        private System.Windows.Forms.Label lblMemVersion;
        private System.Windows.Forms.Button btnSetSerial;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtFullSerial;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox txtDste;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Panel pnlDownloading;
        private System.Windows.Forms.ProgressBar prgrsTesterDownload;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.PictureBox picTesterDisabled;
        private System.Windows.Forms.PictureBox picStartAutoTestWF2;
        private System.Windows.Forms.PictureBox picStartAutoTestWF1;
        private System.Windows.Forms.PictureBox picStartAutoTestWF3;
    }
}

