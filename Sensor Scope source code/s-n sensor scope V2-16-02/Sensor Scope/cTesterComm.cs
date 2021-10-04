//#define COMMDEBUG // Force using a port
using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Timers;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Management;
using System.Runtime.InteropServices;
using Sensor_Scope.General_classes;
using System.IO;
using System.Windows.Forms;

namespace Sensor_Scope
{
    public class cTesterComm
    {
        #region "globals"
        FRMMain frmmain = null;

        public cSignalGenerator sigGen = null;

        public String sSignalFilesFolder = ""; // Folder of files for Bench test.

        Boolean testerConnected = false;
        public Boolean isTesterConnected
        {
            get { return testerConnected; }
        }

        Boolean _WaitingForDevice = false;
        public Boolean waitingForDevice
        {
            get { return _WaitingForDevice; }
        }


        Boolean _DataRefreshed = false;
        public Boolean DataRefreshed
        {
            get 
            {
                lock (lockMem) 
                {
                    Boolean tmp = this._DataRefreshed;
                    this._DataRefreshed = false;
                    return tmp; 
                }
            }
        }


        String _sFirmwareNumber = "";
        public String sFirmwareNumber
        {
            get { return _sFirmwareNumber; }
        }

        String _sSerialNumber = "";
        public String sSerialNumber
        {
            get { return _sSerialNumber; }
        }

        String _sSense = "";
        public String sSense
        {
            get { return _sSense; }
        }

        int _nAutoTestBar = 0;
        public int nAutoTestBar
        {
            get { return _nAutoTestBar; }
        }

        private double _sense = 0;
        public double sense
        {
            get { return _sense; }
        }

        private int _status = 0;
        public int status
        {
            get { return _status; }
        }

        String _sStatusString = "";
        public String sStatusString
        {
            get { return _sStatusString; }
        }

        String _sStatusHeader = "";
        public String sStatusHeader
        {
            get { return _sStatusHeader; }
        }

        int _nFrequency = 0;
        int _nFreq2set = 0;
        Boolean doSetFreq = false;
        public int nFrequency
        {
            get { return _nFrequency; }
            set
            {
                _nFreq2set = value;
                doSetFreq = true;
            }
        }

        int _nPressure = 0;
        int _nPressure2set = 0;
        Boolean doSetPressure = false;
        public int nPressure
        {
            get { return _nPressure; }
            set 
            {
                _nPressure2set = value;
                doSetPressure = true;
            }
        }

        public enum KEYS:int
        {
            KEY_NOP = 0,
            KEY_AUTO_TEST_START_SIN = 1,
            KEY_AUTO_TEST_OFF = 2,
            KEY_TESTER_CALIBRATION_START = 3,
            KEY_TESTER_CALIBRATION_CANCEL = 4,
            KEY_TESTER_CALIBRATION_CONTINUE = 5,
            KEY_AUTO_TEST_START_WF1 = 6,
            KEY_AUTO_TEST_START_WF2 = 7,
            KEY_AUTO_TEST_START_WF3 = 8,
        }
        int _nKeyPress = 0;
        Boolean doSetKeyPress = false;
        public int nKeyPress
        {
            set 
            {
                _nKeyPress = value;
                doSetKeyPress = true;
            }
        }

        Boolean do_send_buff1 = false;

        public Boolean do_softwareDownload = false;
        public String sHexFileName;


        int _nRadioButtonManualOperation = 0;
        int _nRadioButtonManualOperation2set = 0;
        Boolean doSetRadioButton = false;
        Boolean doSetBenchRadioButton = false;
        public int nRadioButtonManualOperation
        {
            get { return _nRadioButtonManualOperation; }
            set 
            {
                _nRadioButtonManualOperation2set = value;
                doSetRadioButton = true;
            }
        }

        int _nRadioButtonBenchTestOperation = 0;
        int _nRadioButtonBenchTestOperation2set = 0;

        int _nCalibrationConstant = 0;
        int _nCalibrationConstant2Set = 0;
        Boolean doSetCalibConsts = false;
        public int nCalibrationConstant
        {
            get { return _nCalibrationConstant; }
            set
            {
                _nCalibrationConstant2Set = value;
                doSetCalibConsts = true;
            }
        }

        int _nDACConstant = 0;
        int _nDACConstant2Set = 0;
        Boolean doSetDACConst = false;
        public int nDACConstant
        {
            get { return _nDACConstant; }
            set
            {
                _nDACConstant2Set = value;
                doSetDACConst = true;
            }
        }


        // EEprom stuff
        Boolean doSetEeEqulizer = false;
        int _EE_general_gain = 0;
        public int EE_GeneralGain
        {
            get { return _EE_general_gain; }
            set
            {
                _EE_general_gain = value;
                doSetEeEqulizer = true;
            }
        }

        Boolean doSetEeFactor = false;
        int _EE_16_VS_1_GAIN = 0;
        public int EE_16_Vs_1_Gain
        {
            get { return _EE_16_VS_1_GAIN; }
            set
            {
                _EE_16_VS_1_GAIN = value;
                doSetEeFactor = true;
            }
        }









        ThreadStart tsPrepareBuffers;
        Thread thPrepareBuffers;
        int nOverideMessage = 0; // Overide status messages. // 0 = Don't overide. 100 = Loading signal files.

        public int nRadioButtonBenchTest
        {
            get { return _nRadioButtonBenchTestOperation; }
            set
            {
                if (value == 1)
                {
                    if (thPrepareBuffers == null)
                    {
                        tsPrepareBuffers = new ThreadStart(PrepareBuffers);
                        thPrepareBuffers = new Thread(tsPrepareBuffers);
                        thPrepareBuffers.Start();
                    }
                }
                else
                {
                    _nRadioButtonBenchTestOperation2set = value;
                    doSetBenchRadioButton = true;
                }
            }
        }

        int _emptyBuffStat = 0;











        // lockers
        object lock_wmi = new object();
        object lock_inbuff = new object();
        object lockMem = new object();

        // Well... the serial port itself...
        SerialPort serialPort = null;

        // Timers
        System.Timers.Timer tmrCheckPort; // Enumerate the ports. Look for our port and open it automatically.
        System.Timers.Timer tmrCommPolling; // Check communication flags and perform comm accordingly.
        const int nTmrCommPolling = 5; // initial value for this timer (200ms)
        System.Timers.Timer tmrTimeout; // com timeout timer


        // Communication flags
        Boolean timedOut = false;
        Boolean gotError = false;
        Boolean doResetPort = false; // Mark that port searcher should reset the port.
        Boolean doRefreshMemoryImage = false; // Read memory pages into local buffer

        // Communication counters
        int nCommPackets = 0;
        int nCommTimeouts = 0;
        int nCommInvalidFrames = 0;

        // Communication buffers
        // Buffers
        int inbuffPtr = 0; // Input buffer pointer
        byte[] inbuff = new byte[1024]; // input b

        // Memory images of the tester
        byte[] testerMem = new byte[MEM_RAM_END - MEM_BASE_AAP_EAAP];
        // Memory image for the eeprom
        byte[] testerEEMem = new byte[MEM_EEPROM_END - MEM_BASE_EEPROM];

        // Last number of detected devices
        int lastNumOfDevices = 0;

        
        // Communication thread
        ThreadStart ts;
        Thread thComm;
        #endregion


        #region "Memory map"
        // fixed address application EEprom parameters
        int[] MEM_SIZES_RAM = { 2, 1, 2, 1, 1, 2, 2 ,1,1,1,1,1,1,2,2};
        const int MEM_BASE_AAP_EAAP = 0x2000;
        const int MEM_FIRMWARE_NUMBER = MEM_BASE_AAP_EAAP + 0x00; // FORMAT = 0.00 (/ 100).
        const int MEM_SERIAL_NUMBER = MEM_BASE_AAP_EAAP + 0x02; // 8 Bit serial number (000)
        const int MEM_SENSE = MEM_BASE_AAP_EAAP + 0x03; // 16 bit value. (000.00g).
        const int MEM_AUTOMATIC_TEST = MEM_BASE_AAP_EAAP + 0x05; // 8 bit. For progress bar 0-255.
        const int MEM_STATUS_STRING_CODE = MEM_BASE_AAP_EAAP + 0x06; // 8 bit. (0.0Code of string to display.
        const int MEM_FREQUENCY = MEM_BASE_AAP_EAAP + 0x07; // 16 bit. (0.0Hz)
        const int MEM_PRESSURE= MEM_BASE_AAP_EAAP + 0x09; // 16 bit. (000.00g)
        const int MEM_KEY_PRESS = MEM_BASE_AAP_EAAP + 0x0B; // 8 BIT Key code is set by the PC. Cleared by the sensor tester.
                                                            // Code 00 – No operation
                                                            // Code 01 – Automatic test “start” key
                                                            // Code 02 – Automatic test “Cancel” key
                                                            // Code 03 – Tester calibration “start” key
                                                            // Code 04 – Tester calibration “Cancel” key
        const int MEM_RADI_BUTTON_MANUAL_OPERATION = MEM_BASE_AAP_EAAP + 0x0C;  // Manual operation radio button.
                                                                                // Status is always read from the sensor tester.
                                                                                // Code 00 – Disabled
                                                                                // Code 01 – Enabled
        const int MEM_RADI_BUTTON_BENCH_TEST = MEM_BASE_AAP_EAAP + 0X0D; // Bench test radio button.
        const int MEM_BUFFER_EMPTY = MEM_BASE_AAP_EAAP + 0X0E; // bit 0 - 1 if buffer is busy, 0 = buffer empty.
        // const int MEM_REGISTER_CORRUPTION = MEM_BASE_AAP_EAAP + 0X0F; // Register corruption
        // const int MEM_SOFTWARE_CHECKSUM = MEM_BASE_AAP_EAAP + 0X10;   // Software checksum
        const int MEM_CALIBRATION_CONSTANT = MEM_BASE_AAP_EAAP + 0X11;           // Calibration constant 
        const int MEM_DAC_CONSTANT = MEM_BASE_AAP_EAAP + 0X13;           // Calibration constant 2 Bytes!!

        
        const int MEM_RAM_END = MEM_BASE_AAP_EAAP + 0X15; // Please update
        #endregion

        #region "Memory map 2"
        int[] MEM_SIZES_EEPROM = { 1, 2, 2, 2, 1, 1};
        const int MEM_BASE_EEPROM = 0x1000;
        const int MEM_EE_SERIAL_NUMBER = MEM_BASE_EEPROM + 0x00;
        const int MEM_EE_CAL_WEIGHT_FACTOR = MEM_BASE_EEPROM + 0x01;
        const int MEM_EE_CAL_ZERO_WEIGHT = MEM_BASE_EEPROM + 0x03;
        const int MEM_EE_PRESSURE = MEM_BASE_EEPROM + 0x05;
        const int EE_GENERAL_GAIN = MEM_BASE_EEPROM + 0x07; // 1% per unit. Range 20% to 200% (20 to 200)
        const int EE_16_VS_1_GAIN = MEM_BASE_EEPROM + 0x08; // 16 bit. (0.0Hz)

        const int MEM_EEPROM_END = MEM_BASE_EEPROM + 0X09; // Please update
        #endregion
        
        

        #region "Communication commands"
        const int COM_READ_FRAME_LENGTH = 5; // R,AddrMsb,AddrLsb,Count,Checksum
        const int COM_READ_FRAME_RESPONSE_LENGTH = 2; // + data length...
        const int COM_WRITE_FRAME_LENGTH = 9;
        const int COM_WRITE_FRAME_RESPONSE_LENGTH = 8;

        byte[] comCmdReadAllAapPage = { (byte)'R', MEM_BASE_AAP_EAAP >> 8, MEM_BASE_AAP_EAAP & 0xff, MEM_RAM_END - MEM_BASE_AAP_EAAP, 0x0 }; // checksum will be calculated on load event.
        byte[] comCmdReadEEpromPage = { (byte)'R', MEM_BASE_EEPROM >> 8, MEM_BASE_EEPROM & 0xff, MEM_EEPROM_END - MEM_BASE_EEPROM, 0x0 }; // checksum will be calculated on load event.
        
        #endregion



        #region "Constructor and Destructor"
        public cTesterComm(FRMMain i_frmMain) // constructor  
        {
            this.frmmain = i_frmMain; // gain access to some parameters on the main form.
            // Calculate checksums.
            calcChecksum(comCmdReadAllAapPage);
            calcChecksum(comCmdReadEEpromPage);

            emptyBuff = new short[124];
            Array.Clear(emptyBuff,0,emptyBuff.Length);
//            for (int i = 0; i < 250; i++)
//                emptyBuff[i] = 0x80; // Little endian ==> = 0x8000

            tmrCheckPort = new System.Timers.Timer(200);
            tmrCheckPort.Elapsed += new ElapsedEventHandler(tmrCheckPort_Elapsed);
            tmrCheckPort.Start();
            tmrTimeout = new System.Timers.Timer(5);
            tmrTimeout.Elapsed += new ElapsedEventHandler(tmrReadTimeout_Elapsed);
        }

        public void cTesterCommCLose() // Destructor 
        {
            this.tmrCheckPort.Stop();
            if (thComm != null)
                thComm.Abort();
            Thread.Sleep(100);
            
            if (serialPort != null)
            {
                killTimers();
                serialPort.Close();
            }

        }
        #endregion

        #region "timers"
        /// <summary>
        /// Enumerate serial port and look for our device.
        /// </summary>
        void tmrCheckPort_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (serialPort != null && !doResetPort && serialPort.IsOpen)
                return;

            try
            {
                tmrCheckPort.Stop();
                if (doResetPort)
                {
                    doResetPort = false;
                    testerConnected = false;
                    nCommPackets = 0;
                    doRefreshMemoryImage = false;
                    doSetFreq = false;
                    doSetKeyPress = false;
                    doSetPressure = false;
                    doSetRadioButton = false;
                    doSetBenchRadioButton = false;
                    do_send_buff1 = false;
                    do_softwareDownload = false;
                    doSetCalibConsts  = false;
                    doSetDACConst = false;
                    doSetEeEqulizer = false;
                    doSetEeFactor = false;
                    
                    if (serialPort != null)
                        serialPort.Close();
                    Thread.Sleep(2000);
                    serialPort = null;
                }
#if COMMDEBUG
                String sPortName = "com15";
#else
                String sPortName = CheckIfTesterConnected1();
#endif
                if (sPortName != "")
                {
                    // We've found the device. Prepare the serial port and open it.
                    //serialPort = new SerialPort(sPortName, 57600, Parity.None, 8, StopBits.One);
                    serialPort = new SerialPort(sPortName, 256000, Parity.None, 8, StopBits.One);
                    serialPort.ReceivedBytesThreshold = 1;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(serialPort_DataReceived);
                    serialPort.Open();
//                    tmrCommPolling = new System.Timers.Timer(nTmrCommPolling);
//                    tmrCommPolling.Elapsed += new ElapsedEventHandler(tmrCommPolling_Elapsed);
//                    tmrCommPolling.Start();
                    ts = new ThreadStart(comThread);
                    thComm = new Thread(ts);
                    thComm.Priority = ThreadPriority.Highest;
                    thComm.Start();

                    this.testerConnected = true;
                    this._WaitingForDevice = false;
                }
                else
                {
                    nCommPackets = 0;
                    this._WaitingForDevice = false;
                }

                tmrCheckPort.Start();
            }
            catch (Exception e1)
            {
                Debug.WriteLine("comm polling exception: " + e1.Message + "(" + tmrCheckPort.Enabled + ")");
//                MessageBox.Show("Error on tester module: " + e1.Message + "trace: " + e1.StackTrace);


                killTimers();
                doResetPort = true;
                tmrCheckPort.Start();
                //frmmain.add2log("Failed on port polling: " + e1.Message);
            }
        }
    
        void tmrReadTimeout_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                timedOut = true;
                nCommTimeouts++;
                HandleInvalidFrame(false);
                tmrTimeout.Stop();
            }
            catch (Exception e1)
            {
                Debug.WriteLine("***** Error: " + e1.Message);
                doResetPort = true;
            }
        }



        long nLastPolling = Environment.TickCount;
        public Boolean downloading = false;
        public cDownloadFirmware cdf = null;
        void comThread()
        {
            while (true)
            {
                try
                {
                    if (Environment.TickCount - nLastPolling < 100)
                    {
                        Thread.Sleep(0);
                        continue;
                    }
                    nLastPolling = Environment.TickCount;


                    // Download software
                    if (do_softwareDownload)
                    {
                        do_softwareDownload = false;
                        downloading = true;
                         cdf = new cDownloadFirmware();
                        cdf.download_file(true, serialPort,this.sHexFileName);
                        //Thread.Sleep(2000);
                        downloading = false;
                    }

                    // Read memory pages
                    //showTime("Read pages");
                    refreshPages();
                    //showTime("After Read pages");

                    // Send buffer 1
                    if (do_send_buff1 && _nRadioButtonBenchTestOperation == 1)
                    {
                        do_send_buff1 = false;
                        if (sigGen != null &&  sigGen.nEmptyBuffersBetweenFiles == 0)
                            sendBuffer(0x2500);
                        else
                            sendBuffer(0x2500, true);
                    }

                    // Set commands
                    // Radio button
                    if (doSetRadioButton)
                    {
                        doSetRadioButton = false;
                        write1b(MEM_RADI_BUTTON_MANUAL_OPERATION, (byte)_nRadioButtonManualOperation2set);
                    }

                    // Bench test radio button
                    if (doSetBenchRadioButton)
                    {
                        doSetBenchRadioButton = false;
                        blockCount = 0;
                        write1b(MEM_RADI_BUTTON_BENCH_TEST, (byte)_nRadioButtonBenchTestOperation2set);
                        log("test bench button was clicked. Setting bench test to " + _nRadioButtonBenchTestOperation2set.ToString());
                    }

                    // frequency
                    if (doSetFreq)
                    {
                        doSetFreq = false;
                        write2bytesWithAck(MEM_FREQUENCY, (byte)(_nFreq2set >> 8), (byte)(_nFreq2set & 0xff), 3);
                    }

                    // Pressure
                    if (doSetPressure)
                    {
                        doSetPressure = false;
                        write2bytesWithAck(MEM_PRESSURE, (byte)(_nPressure2set >> 8), (byte)(_nPressure2set & 0xff), 3);
                    }

                    // Key
                    if (doSetKeyPress)
                    {
                        doSetKeyPress = false;
                        write1b(MEM_KEY_PRESS, (byte)this._nKeyPress);
                        this._nKeyPress = (int)KEYS.KEY_NOP;
                    }

                    // Calib consts
                    if (doSetCalibConsts)
                    {
                        doSetCalibConsts = false;
                        write2bytesWithAck(MEM_CALIBRATION_CONSTANT, (byte)(_nCalibrationConstant2Set >> 8), (byte)(_nCalibrationConstant2Set & 0xff), 3);
                    }

                    // DAC consts
                    if (doSetDACConst)
                    {
                        doSetDACConst = false;
                        write2bytesWithAck(MEM_DAC_CONSTANT, (byte)(_nDACConstant2Set >> 8), (byte)(_nDACConstant2Set & 0xff), 3);
                    }

                    // EEporom fields
                    if (doSetEeEqulizer)
                    {
                        doSetEeEqulizer = false;
                        write1b(EE_GENERAL_GAIN, (byte)this._EE_general_gain);
                    }
                    if (doSetEeFactor)
                    {
                        doSetEeFactor = false;
                        write1b(EE_16_VS_1_GAIN, (byte)this._EE_16_VS_1_GAIN);
                    }

                }
                catch (IOException ioExp)
                {
                    try { serialPort.Close(); }
                    catch { };
                    lastNumOfDevices = 0;
                    doResetPort = true;
                    break;
                }
                catch (ThreadAbortException)
                {
                    Debug.WriteLine("Aborting comm thread.");
                    break;
                }
                catch (Exception e2)
                {
                    Debug.WriteLine("Aloha... still here..." + e2.StackTrace);
                    Debug.WriteLine(e2.Message);
                }
                // doResetPort = true;
                //if (tmrCommPolling != null)
                //    tmrCommPolling.Start();
            }
        }
        
        long lasttick = Environment.TickCount;
        private void showTime(String s)
        {
            long diff = Environment.TickCount - lasttick;
            lasttick = Environment.TickCount;
            Debug.WriteLine(s + "  -  " + diff.ToString());
        }

        short[] emptyBuff; // Load event feels it with 0x8000
        private void sendBuffer(int startAddr,Boolean sendEmpty=false)
        {
            int length;
            short[] buff;
            if (!sendEmpty)
            {
                buff = sigGen.getNextBuffer(out length);
            }
            else
            {
                buff = emptyBuff;
                length = 124;
            }

            if (writeBlockWithAck(startAddr, buff, 3, length))
            {
                if (sendEmpty)
                {
                    sigGen.nEmptyBuffersBetweenFiles--;
                    Debug.WriteLine(sigGen.nEmptyBuffersBetweenFiles.ToString());
                }
                else
                    sigGen.updateBuffPtr();
            }
            else
                log("Failed to send packet.");
        }

        Boolean foundWriteAck = false;
        private void write2bytesWithAck(int addr, byte p, byte p_2,int retries)
        {
            try
            {
                int nRetries = 0;
                // first byte
                foundWriteAck = false;
                do
                {
                    serialPort.DiscardInBuffer();
                    write1b(addr, p);
                    // Thread.Sleep(30);
                    wait(200);
                    ++nRetries;
                    
                } while (nRetries < retries && !foundWriteAck);

                if (!foundWriteAck)
                    return;
                foundWriteAck = false;
                nRetries = 0;
                // Second byte
                do
                {
                    serialPort.DiscardInBuffer();
                    write1b(addr + 1, p_2);
                    wait(200);
                    ++nRetries;
                    if (!foundWriteAck)
                        Debug.WriteLine("Failed to write block to address " + Convert.ToString(addr, 16) + " try #" + nRetries.ToString());
                } while (nRetries < retries && !foundWriteAck);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        private void wait(int ms)
        {
            long startTime = System.Environment.TickCount;
            while (System.Environment.TickCount - startTime < ms && !foundWriteAck)
            {
                Thread.Sleep(0);
            }
        }

        byte blockCount = 0;
        private Boolean writeBlockWithAck(int addr, short[] buff, int retries,int length)
        {
            try
            {
                int nRetries = 0;
                byte[] comCmd = new byte[length * 2 + 5 + 1]; // 4 for header, length * 2 for samples 1 for block counter and 1 for check sum
                comCmd[0] = (byte)'B';
                // Set addr
                comCmd[1] = (byte)(addr >> 8);
                comCmd[2] = (byte)(addr & 0xff);
                // set data length 
                comCmd[3] = (byte)(length * 2+1);
                // Copy the data
                for (int i=0;i<length;i++)
                {
                    comCmd[4 + i * 2] = (byte)(buff[i] >> 8);
                    comCmd[4+ i * 2 + 1] = (byte)(buff[i] & 0xff);
                }
                blockCount++;
                //Debug.WriteLine("\aBlock count = " + blockCount.ToString());
                comCmd[4 + length * 2] = blockCount;
                // Calculate the checksum
                calcChecksum(comCmd);
                // Send the command.
                foundWriteAck = false;
                do
                {
                    serialPort.DiscardInBuffer();
//                    Debug.WriteLine("Sending buffer. Index = " + sigGen.buffIndex.ToString());
                    write(comCmd);
                    ++nRetries;
                    wait(200);
                    if (!foundWriteAck)
                        Debug.WriteLine("Didn't get ack for buffer at " + DateTime.Now.TimeOfDay.ToString());
                    inbuffPtr = 0;
                } while (nRetries < retries && !foundWriteAck);

                return foundWriteAck;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }

        private void refreshPages()
        {
            serialPort.DiscardInBuffer();
            inbuffPtr = 0;
            if (send(comCmdReadAllAapPage, 1))
            {
                if (nCommPackets < 10)
                    ++nCommPackets;
            }
            else
                nCommPackets = 0;
            Thread.Sleep(10);

            serialPort.DiscardInBuffer();
            inbuffPtr = 0;
            if (send(comCmdReadEEpromPage, 1))
            {
                if (nCommPackets < 10)
                    ++nCommPackets;
            }
            else
                nCommPackets = 0;


        }



        private String CheckIfTesterConnected()
        {
            Debug.WriteLine("Checking registry for slabser0: " + DateTime.Now.Second.ToString() + "*");
            RegistryKey regHive = Registry.LocalMachine;
            string regKey = @"HARDWARE\DEVICEMAP\SERIALCOMM";
            String[] possibleRegNames = { @"\Device\slabser0", @"\Device\silabser0", 
                                          @"\Device\slabser1", @"\Device\silabser1",
                                          @"\Device\slabser2", @"\Device\silabser2",
                                          @"\Device\slabser3", @"\Device\silabser3",
                                          @"\Device\slabser4", @"\Device\silabser4",
                                          @"\Device\slabser5", @"\Device\silabser5"};
            String sConfirmedPort = "";



            RegistryKey key = regHive.OpenSubKey(regKey, false);
            if (key != null)
            {
                // We've found the device. Prepare the serial port and open it.
                String sPortName;
                foreach (String regName in possibleRegNames)
                {
                    sPortName = (string)key.GetValue(regName);
                    if (sPortName != null)
                    {
                        try
                        {
                            serialPort = new SerialPort(sPortName, 256000, Parity.None, 8, StopBits.One);
                            serialPort.ReceivedBytesThreshold = 1;
                            serialPort.Open();
                            serialPort.DiscardInBuffer();
                            serialPort.Write(comCmdReadAllAapPage, 0, comCmdReadAllAapPage.Length);
                            Thread.Sleep(50);
                            if (serialPort.BytesToRead == 2 + comCmdReadAllAapPage[3])
                                sConfirmedPort = sPortName;
                            serialPort.Close();
                            serialPort = null;
                        }
                        catch
                        {
                            Debug.WriteLine("");
                        }
                    }
                    if (sConfirmedPort != "")
                        return sConfirmedPort;
                }
            }
            return "";
        }

        int showedCount = 0;
        /// <summary>
        /// if tester connected - return the port name. Else - return an empty string.
        /// </summary>
        /// <returns></returns>
        private String CheckIfTesterConnected1()
        {
            ++showedCount;
//            if (showedCount == 1)
//                MessageBox.Show("Checking if tester connected.");

            uint numOfDevices = 0;
            IntPtr handle = IntPtr.Zero;
            IntPtr devName;
            byte devNameLength = 0;
            String sPortName = "";
            int res = cEncapsulateCp210x.getNumDevices(ref numOfDevices);
//            if (showedCount == 1)
//                MessageBox.Show("last num of devices = " + lastNumOfDevices.ToString() + "  new num of devices = " + numOfDevices.ToString());

            if (numOfDevices == lastNumOfDevices)
                          return "";
            else
            {
                if (numOfDevices < lastNumOfDevices)
                {
                    lastNumOfDevices = (int)numOfDevices;
                    return "";
                }
            }

            while (numOfDevices != lastNumOfDevices)
            {
                this._WaitingForDevice = true;
                lastNumOfDevices = (int)numOfDevices;    
                Thread.Sleep(500);
                res = cEncapsulateCp210x.getNumDevices(ref numOfDevices);
            }
            

            if (res == 0 && numOfDevices > 0)
            {
                for (uint nDevNum = 0; nDevNum < numOfDevices; nDevNum++)
                {
//                    if (showedCount == 1)
//                        MessageBox.Show("Trying to open device number " + nDevNum.ToString());
                    res = cEncapsulateCp210x.open(nDevNum, ref handle);
                    if (res != 0)
                        continue;
                    devName = Marshal.AllocHGlobal(100);
                    res = cEncapsulateCp210x.getDeviceProductString(handle, devName, ref devNameLength, true);

                    String sDevName = Marshal.PtrToStringAnsi(devName);

                    res = cEncapsulateCp210x.getDeviceInterfaceString(handle, (Byte)1,devName,  ref devNameLength, true);
                    sDevName = Marshal.PtrToStringAnsi(devName);


                    if (sDevName != "EarlySense Sensor Tester")
                        goto skip;
                    // Get vid, pid, and serial number to get the com port from the registry.
                    ushort vid = 0, pid = 0;
                    res = cEncapsulateCp210x.getDevicePid(handle, ref pid);
                    if (res != 0)
                        goto skip;
                    res = cEncapsulateCp210x.getDeviceVid(handle, ref vid);
                    if (res != 0)
                        goto skip;

                    res = cEncapsulateCp210x.getDeviceSerialNumber(handle,devName,ref devNameLength,true);
                    if (res != 0)
                        goto skip;
                    String serial = Marshal.PtrToStringAnsi(devName);
                // Get the port's name from the registry
                    RegistryKey regHive = Registry.LocalMachine;
                    string regKey = @"SYSTEM\CurrentControlSet\Enum\USB\Vid_"+Convert.ToString(vid,16).PadLeft(4,'0') + "&Pid_" + Convert.ToString(pid,16).PadLeft(4,'0') + "\\" + serial.PadLeft(4,'0') + "\\Device Parameters";
                    string regName1 = "PortName";
                    RegistryKey key = regHive.OpenSubKey(regKey,false);
                    if (key != null)
                        sPortName = (string)key.GetValue(regName1);

                    if (sPortName == null)
                        sPortName = "";
                
                skip:
                    res = cEncapsulateCp210x.close(handle);
                    Marshal.FreeHGlobal(devName);
                    if (sPortName != "")
                        break;
                }
                return sPortName;
            }
            return String.Empty;
        }

        private void killTimers()
        {
            if (tmrCommPolling != null)
            {
                tmrCommPolling.Stop();
                tmrCommPolling = null;
            }
        }

        #endregion

        #region "I/O routines"

        // Data received event
        void serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (downloading)
                return;
            //lock (lock_inbuff)
            lock (lockMem)
            {
                try
                {
//                    Debug.WriteLine(inbuffPtr);
                    if (inbuffPtr + serialPort.BytesToRead > inbuff.Length)
                    {
                        inbuffPtr = 0;
                        serialPort.DiscardInBuffer();
                        return;
                    }
                    int i = serialPort.Read(inbuff, inbuffPtr, serialPort.BytesToRead);
//                    Debug.WriteLine("Got " + i.ToString() + " bytes.");
                    inbuffPtr += i;
                    if (inbuffPtr == 1)
                    {
                        if (inbuff[0] == (byte)'W')
                        {
                            foundWriteAck = true;
                            inbuffPtr = 0;
                        }
                        else
                            if (inbuff[0] == (byte)'E')
                                gotError = true;
                    }

                }
                catch (Exception e1) {
                    Debug.WriteLine(e1.Message + " " + e1.StackTrace);

                };
            }
        }

        private void write(byte[] buff)
        {
            serialPort.Write(buff, 0, buff.Length);
        }

        private void write1b(int addr, byte val)
        {
            byte[] comCmd = { (byte)'W', 0x00,    0x00,      0x00,       0x00};
            //                         addr msb  addr lsb   Data byte  chksum 
            //                       0     1      2             3         4         

            // Set shoe header
            //comCmd[0] = (byte)(shoe == SHOE.LEFT ? 0xA9 : 0xAA);
            // Set addr
            comCmd[1] = (byte)(addr >> 8);
            comCmd[2] = (byte)(addr & 0xff);
            // set data byte
            comCmd[3] = val;
            // Calculate the checksum
            calcChecksum(comCmd);
            // Send the command.
            write(comCmd);
        }

        int countDiffFrames = 0; // For debuggging and stopping sending only after 2 frames with status <> 9.
        int countChecksumError = 0;
        private Boolean send(byte[] outBuff, int nRetries)
        {
            int nExpectedLength = 0;
            int nTry = 0;
            Boolean gotResponse = false;
            int addr;

            if (tmrTimeout.Enabled || outBuff.Length < 5)
                return false; // someone is trying to interrupt.
            addr = outBuff[1] * 256 + outBuff[2];
            // Calculate the length according to message type
            switch (outBuff[0])
            {
                case (Byte)'R': // Read message.
                    nExpectedLength = 2 + outBuff[3];
                    break;
            }

            do
            {
                timedOut = false;
                gotError = false;
                write(outBuff);
                // tmrTimeout.Start();
                int starttime = Environment.TickCount;
                while (!timedOut && inbuffPtr < nExpectedLength && !gotError)
                {
                    Thread.Sleep(0);
                    timedOut = System.Environment.TickCount - starttime > 200;
                    if (timedOut)
                        Debug.WriteLine("Gotyou.");
                }
                // tmrTimeout.Stop();
                if (!timedOut && !gotError) // Check the validity of the response
                {
                    switch (outBuff[0])
                    {
                        case (Byte)'R': // Read message.
                            bool isEeprom = outBuff[1] == (MEM_BASE_EEPROM>>8) && (outBuff[2] & 0xff) == 0;
                            // Check the checksum
                            if (!checksumOk(outBuff[3], inbuff))
                            {
                                ++countChecksumError;
                                Debug.WriteLine("Checksum error! ("+ countChecksumError.ToString() + ")");
                                dumpBuffer(inbuff, outBuff[3]);
                                break;
                            }


                            int sum = 0;
                            for (int i = 1; i < outBuff[3]; i++)
                                sum += inbuff[i];
                            if (sum == 0)
                            {
                                Debug.WriteLine("buffer is all zeroes.");
                                MessageBox.Show("Buffer is all zeroes.");
                                log("Buffer is all zeroes.");
                            }

                            gotResponse = true;
                            // Copy to the correct place
                            lock (lockMem)
                            {
                                if (!isEeprom) // Ram page
                                {
                                    if (!checkBuffIntegrity(inbuff))
                                    {
                                        log("Invalid page detected.");
                                        Debug.WriteLine("Invalid page detected.");
                                        dumpBuffer(inbuff, outBuff[3]);
                                        break;
                                    }

                                    Array.Copy(inbuff, 1, testerMem, 0, outBuff[3]);
                                    // Set values
                                    double dTmp = (double)get2ByteVal(testerMem, MEM_FIRMWARE_NUMBER) / 100;
                                    this._sFirmwareNumber = dTmp.ToString("0.00");
                                    this._sSerialNumber = get1ByteVal(testerMem, MEM_SERIAL_NUMBER).ToString("000");
                                    dTmp = (double)get2ByteVal(testerMem, MEM_SENSE) / 100;
                                    this._sense = dTmp;
                                    this._sSense = dTmp.ToString("000.00g");
                                    this._nAutoTestBar = get1ByteVal(testerMem, MEM_AUTOMATIC_TEST);
                                    int nTmp;
                                    _status = get1ByteVal(testerMem, MEM_STATUS_STRING_CODE); 
                                    if (nOverideMessage != 0)
                                        nTmp = nOverideMessage;
                                    else
                                        nTmp = get1ByteVal(testerMem, MEM_STATUS_STRING_CODE);
                                    
                                    switch (nTmp & 0x7f)
                                    {
                                        case 0:
                                            this._sStatusString = "";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 1:
                                            this._sStatusString = "Sine sequence\nin progress.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 2:
                                            this._sStatusString = "Automatic test\ncompleted.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 3:
                                            this._sStatusString = "Sine wave signal\nis active.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 4:
                                            this._sStatusString = "Error on test!\nCheck sensor position,\nand Re-start test.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 5:
                                            this._sStatusString = "Calibration step 1:\nPlace 40g\non sensing area\nand press \"continue\"";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 6:
                                            this._sStatusString = "Calibration step 2:\nplace 40g+40g\non sensing area\nand press \"continue\"";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 7:
                                            this._sStatusString = "Calibration not valid.\nProcess is canceled.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 8:
                                            this._sStatusString = "Calibration completed successfully.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 9:
                                            //if (sigGen == null || !sigGen.finished)
                                            //{
                                            //    if (sigGen == null)
                                            //        sigGen = new cSignalGenerator(sSignalFilesFolder);
                                            //}
                                            String s1, s2;
                                            sigGen.waitingForStart = false;
                                            sigGen.getCurrentStatus(out s1, out s2);
                                            if (s1.Length > 50)
                                                this._sStatusString = s1.Substring(0, 50) + "\n\n\n" + s2;
                                            else
                                                this._sStatusString = s1 + "\n\n\n" + s2;
                                            this._sStatusHeader = "Bench test in progress";
                                            break;

                                        case 10:
                                            this._sStatusString = "Bench test completed.";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 11:
                                            this._sStatusString = "Now playing\nWaveform 1";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 12:
                                            this._sStatusString = "Now playing\nWaveform 2";
                                            this._sStatusHeader = "Status";
                                            break;
                                        case 13:
                                            this._sStatusString = "Now playing\nWaveform 3";
                                            this._sStatusHeader = "Status";
                                            break;
                                        // Overide messages
                                        case 100:
                                            this._sStatusString = "Loading signal files,\nPlease wait.";
                                            this._sStatusHeader = "Status";
                                            break;


                                        default:
                                            this._sStatusString = "";
                                            break;
                                    }
                                    if ((nTmp & 0x7f) != 9 && sigGen != null && !sigGen.waitingForStart)
                                    {

                                        log("Status changed to " + (nTmp & 0x7f));
                                        dumpBuffer(inbuff, outBuff[3]);
                                        ++countDiffFrames;
                                        if (!sigGen.finished && countDiffFrames > 1)
                                            log("signal generation stopped. Moving to state " + (nTmp & 0x7f).ToString());
                                        sigGen = null;
                                    }
                                    else
                                        countDiffFrames = 0;


                                    if ((nTmp & 0x80) == 0x80) // Got a red alert
                                        this._sStatusString += "R";
                                    else
                                        this._sStatusString += " ";

                                    // Frequency
                                    this._nFrequency = get2ByteVal(testerMem, MEM_FREQUENCY);

                                    // Pressure
                                    this._nPressure = get2ByteVal(testerMem, MEM_PRESSURE);

                                    // RadioButton
                                    this._nRadioButtonManualOperation = get1ByteVal(testerMem, MEM_RADI_BUTTON_MANUAL_OPERATION);

                                    // Bench test radio button
                                    this._nRadioButtonBenchTestOperation = get1ByteVal(testerMem, MEM_RADI_BUTTON_BENCH_TEST);

                                    // Calibration constants
                                    this._nCalibrationConstant = get2ByteVal(testerMem, MEM_CALIBRATION_CONSTANT);

                                    // DAC Constant
                                    this._nDACConstant = get2ByteVal(testerMem, MEM_DAC_CONSTANT);

                                    // Check for new buffer.
                                    this._emptyBuffStat = get1ByteVal(testerMem, MEM_BUFFER_EMPTY);
                                    //                                Debug.WriteLine(this._emptyBuffStat);
                                    if ((this._emptyBuffStat & 0x01) == 0x00 && sigGen != null && !sigGen.finished)
                                        do_send_buff1 = true;

                                    set1ByteVal(testerMem, MEM_BUFFER_EMPTY, (byte)(_emptyBuffStat | 0x01));
                                    this._DataRefreshed = true;
                                }
                                else // EEprom page
                                {
                                    if (!checkEEBuffIntegrity(inbuff))
                                    {
                                        log("Invalid ee page detected.");
                                        Debug.WriteLine("Invalid ee page detected.");
                                        dumpBuffer(inbuff, outBuff[3]);
                                        break;
                                    }

                                    
                                    Array.Copy(inbuff, 1, testerEEMem, 0, outBuff[3]);
                                    if (! doSetEeEqulizer)
                                        _EE_general_gain = getEe1ByteVal(testerEEMem, EE_GENERAL_GAIN);
                                    
                                    if (!doSetEeFactor)
                                        _EE_16_VS_1_GAIN = getEe1ByteVal(testerEEMem, EE_16_VS_1_GAIN);


                                    // Set values
                                    /*
                                    double dTmp = (double)get2ByteVal(testerMem, MEM_FIRMWARE_NUMBER) / 100;
                                    this._sFirmwareNumber = dTmp.ToString("0.00");
                                    this._sSerialNumber = get1ByteVal(testerMem, MEM_SERIAL_NUMBER).ToString("000");
                                    dTmp = (double)get2ByteVal(testerMem, MEM_SENSE) / 100;
                                    this._sSense = dTmp.ToString("000.00g");
                                    this._nAutoTestBar = get1ByteVal(testerMem, MEM_AUTOMATIC_TEST);
                                    int nTmp;
                                    if (nOverideMessage != 0)
                                        nTmp = nOverideMessage;
                                    else
                                        nTmp = get1ByteVal(testerMem, MEM_STATUS_STRING_CODE);
                                    */
                                }
                            }
                            break;
                        case (Byte)'W': // Write message
                            break;
                    }
                    // Debug.WriteLine("Correct packet");
                }
                else if (timedOut)
                    Debug.WriteLine("Time out");
                else if (gotError)
                    Debug.WriteLine("Got error");
                inbuffPtr = 0;
            } while (++nTry < nRetries && !gotResponse);
            return gotResponse;
        }

        int diff2Offset = MEM_BASE_AAP_EAAP - 1;
        int ee_diff2Offset = MEM_BASE_EEPROM - 1;
        int MAX_STATUS = 20;
        int MAX_KEYPRESS = 10;
        private bool checkBuffIntegrity(byte[] inbuff)
        {
            if ((inbuff[MEM_STATUS_STRING_CODE - diff2Offset] & 0x7f) > MAX_STATUS)
                return false;

            if (inbuff[MEM_KEY_PRESS - diff2Offset] > MAX_KEYPRESS)
                return false;

            if (inbuff[MEM_RADI_BUTTON_MANUAL_OPERATION - diff2Offset] > 1)
                return false;

            if (inbuff[MEM_RADI_BUTTON_BENCH_TEST - diff2Offset] > 1)
                return false;

            if (inbuff[MEM_BUFFER_EMPTY - diff2Offset] > 1)
                return false;
            
                    return true;
        }
        private bool checkEEBuffIntegrity(byte[] inbuff)
        {
            int tmp;
            tmp = getEe1ByteVal(inbuff, EE_GENERAL_GAIN);
            if (tmp < 20 || tmp > 200)
                return false;
            tmp = getEe1ByteVal(inbuff, EE_16_VS_1_GAIN);
            if (tmp < 20 || tmp > 200)
                return false;

            return true;
        }



        private void dumpBuffer(byte[] inbuff, byte p)
        {
            return;
            using (StreamWriter sw = new StreamWriter(DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + " status " + inbuff[7].ToString() + " detected.csv"))
            {
                sw.WriteLine("Length," + p.ToString());
                for (int i = 0; i <= p + 1; i++)
                {
                    sw.WriteLine(i.ToString() + "," + inbuff[i].ToString());
                    Debug.Write(inbuff[i].ToString() + " ");
                    Debug.WriteLine("");
                }
            }
        }

        private void log(String s)
        {
            return;
            using (StreamWriter sw = new StreamWriter("log.csv",true))
            {
                sw.WriteLine(DateTime.Now.ToString() + "," + s);
            }
        }


        private bool checkBurst()
        {
            // Check 4 lengths (e8 = 232. see read command structure).
            if (inbuff[9] != 0xe8 || inbuff[243] != 0xe8 || inbuff[477] != 0xe8 || inbuff[711] != 0xe8)
                return false;

            // Check 4 addresses: 280, 505, 730 and 955 (280 + 0, 280 + 225, 280 + 225 X 2 and 280 + 225 X 3)
            if (inbuff[11] * 256 + inbuff[12] != 0x280 || inbuff[245] * 256 + inbuff[246] != 0x361 || inbuff[479] * 256 + inbuff[480] != 0x442 || inbuff[713] * 256 + inbuff[714] != 0x523)
                return false;

            // Check checksums
            int checksum1 = 0,
                checksum2 = 0,
                checksum3 = 0,
                checksum4 = 0;

            for (int i = 8; i < 239; i++)
            {
                checksum1 += inbuff[i];
                checksum2 += inbuff[i + 234];
                checksum3 += inbuff[i + 468];
                checksum4 += inbuff[i + 702];
            }
            if (inbuff[239] * 256 + inbuff[240] != checksum1 || inbuff[473] * 256 + inbuff[474] != checksum2 || inbuff[707] * 256 + inbuff[708] != checksum3 || inbuff[941] * 256 + inbuff[942] != checksum4)
                return false;

            // Check footer
            if (inbuff[241] != 0xcc || inbuff[475] != 0xcc || inbuff[709] != 0xcc || inbuff[943] != 0xcc)
                return false;

            return true;
        }


        #region "utils"

        private int getEe1ByteVal(byte[] ptr, int memPtr)
        {
            memPtr -= MEM_BASE_EEPROM;
            lock (lockMem)
            {
                return ptr[memPtr];
            }
        }

        private int get1ByteVal(byte[] ptr, int memPtr)
        {
            memPtr -= MEM_BASE_AAP_EAAP;
            lock (lockMem)
            {
                return ptr[memPtr];
            }
        }

        private void set1ByteVal(byte[] ptr, int memPtr,byte value)
        {
            memPtr -= MEM_BASE_AAP_EAAP;
            lock (lockMem)
            {
                ptr[memPtr] = value;
            }
        }



        private int get2ByteVal(byte[] ptr, int memPtr)
        {
            memPtr -= MEM_BASE_AAP_EAAP;
            lock (lockMem)
            {
                return ptr[memPtr] * 256 + ptr[memPtr + 1];
            }
        }

        private int get3ByteVal(byte[] ptr, int memPtr)
        {
            memPtr -= MEM_BASE_AAP_EAAP;
            lock (lockMem)
            {
                return ptr[memPtr] * 65536 + ptr[memPtr + 1] * 256 + ptr[memPtr + 2];
            }
        }


        private void calcChecksum(byte[] frame)  // Calculate the checksum of a frame and insert it into position. 
        {
            int len = frame.Length - 1;
            Int16 chkSum = 0;
            for (int i = 0; i < len; i++)
                chkSum += frame[i];
            frame[len] = (byte)(chkSum & 0xff);

        }

        private bool checksumOk(int length,byte[] buff)
        {
            int sum = 0;
            for (int i = 0; i <= length; i++)
                sum += buff[i] ;
            sum = sum & 0xff;
            int checksum = buff[length + 1];
            return (sum == checksum);
        }
        #endregion
        private void HandleInvalidFrame(Boolean bCount)
        {
            if (bCount)
                nCommInvalidFrames++;
            if (serialPort != null)
                serialPort.DiscardInBuffer();
            inbuffPtr = 0; // Clean the buffer.
            timedOut = true;
        }
        #endregion

        private void PrepareBuffers()
        {
            nOverideMessage = 100;
            sigGen = new cSignalGenerator(sSignalFilesFolder);
            if (!sigGen.finished)
            {
                sigGen.waitingForStart = true;
                _nRadioButtonBenchTestOperation2set = 1;
                doSetBenchRadioButton = true;
            }
            thPrepareBuffers = null;
            nOverideMessage = 0;
        }


    }
}
