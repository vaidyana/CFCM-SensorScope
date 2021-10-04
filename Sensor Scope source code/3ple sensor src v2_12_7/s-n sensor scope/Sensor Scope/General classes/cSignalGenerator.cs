using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace Sensor_Scope.General_classes
{
    /// <summary>
    /// This class is responsible for generating signal buffer from files on a specified folder.
    /// </summary>
    public class cSignalGenerator
    {
        String _folderName;
        public Boolean finished = false;
        public Boolean waitingForStart = false; // Indicat that we still expect "9" - bench test status. (buffers prepared).
        String[] files; // List of files to send
        int _fileIndex, _buffIndex; // Pointers on current file/source list and pointer in current used buff.
        const int maxPeak2Peak = 450000; // Max peak to peak is 450000 (-225000 - 225000). Units in DSU.
        int peak = maxPeak2Peak / 20;

        // Four properties for the progress bars
        public int fileIndex
        {
            get
            {
                if (lstBuffers == null || lstBuffers.Count == 0)
                    return 0;
                if (!finished)
                    return _fileIndex + 1;
                else
                    return _fileIndex;
            }
        }
        public int nTotalFiles
        {
            get
            {
                if (lstBuffers != null) return lstBuffers.Count;
                else return 0;
            }
        }
        public int buffIndex
        {
            get { return _buffIndex; }
        }

        public int CurrentTotalSize
        {
            get 
            {
                if (lstBuffers == null || lstBuffers.Count == 0)
                    return 0;
                if (_fileIndex < lstBuffers.Count)
                    return lstBuffers[_fileIndex].Length;
                else
                    return lstBuffers[lstBuffers.Count -1].Length; ;
            }
        }


        public int  nEmptyBuffersBetweenFiles = 0;
        Int16[] buff; // Buffer for returning values to stream.
        List<int[]> lstBuffers;
        List<int> lstAverages; // Average for each buffer
        double nDivider = 1;
        double nOffset = 0;

        public cSignalGenerator(String sFolderName)
        {
            this.finished = false;
            this._folderName = sFolderName;
            // If folder doesn;t exist or doesn't contain csv files - mark as finished and return.
            if (!Directory.Exists(sFolderName) || Directory.GetFiles(this._folderName, "*.csv").Length == 0)
            {
                finished = true;
                MessageBox.Show("Invalid folder / files.");
                return;
            }

            if (!prepareBuffers())
            {
                finished = true;
                MessageBox.Show("Failed to prepare buffers.");
            }

            Debug.WriteLine("signal generator was created (finished = " + finished.ToString() + ").");
       }

        ~cSignalGenerator()
        {
            Debug.WriteLine("signal generator Destroyed.");
        }
        static bool isNotEmpty(String s)
        {
            return s != "";
        }


        private bool prepareBuffers()
        {
            try
            {
                int value;
                // Prepare the buffers.
                buff = new short[124];
                lstBuffers = new List<int[]>();
                lstAverages = new List<int>();
                _fileIndex = 0;
                _buffIndex = 0;
                cNaturalComparer.NaturalFileInfoNameComparer cNatCompare = new cNaturalComparer.NaturalFileInfoNameComparer();
                files = Directory.GetFiles(_folderName, "*.csv");
                Array.Sort(files, cNatCompare);
                foreach (String file in files)
                {
                    using (StreamReader sr = new StreamReader(file))
                    {
                        String sLine = sr.ReadLine();
                        String[] splitted = sLine.Split(',');
                        splitted = Array.FindAll(splitted, isNotEmpty);
                        int[] buff1 = new int[splitted.Length];
                        int i = 0;
                        long sum = 0; // For averaging
                        int count = 0;
                        foreach (String s in splitted)
                        {
                            if (!int.TryParse(s, out value))
                            {
                                MessageBox.Show("Error on signal files: invalid number:" + s);
                                files = null;
                                return false;
                            }
                            value /= 10;
                            buff1[i++] = value;
                            sum += value;
                            count++;
                        }
                        lstBuffers.Add(buff1);
                        int avg = (int)(sum / count);
                        lstAverages.Add(avg);
                        // Check peak2peak
                        for (int j = 0;j<buff1.Length;j++)
                        {
                            int value1 = buff1[j] - avg;
                            if (value1 > peak || value1 < -peak)
                            {
                                MessageBox.Show("Signal amplitude is too high.\nCheck value #" + j.ToString() + " in the file \"" + Path.GetFileName(file) + "\".\nAverage = " + (10 * avg).ToString() + " value = " + value1 * 10 + ".\nMax allowed difference from average is " + peak + "Bench test stopped.");
                                finished = true;
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                finished = true;
                MessageBox.Show("Failed to prepare signal buffers: " + e.Message);
                return false;
            }
        }

        
        public void updateBuffPtr()
        {
            if (_buffIndex + 124 > lstBuffers[_fileIndex].Length)
                _buffIndex = lstBuffers[_fileIndex].Length;
            else
                _buffIndex += 124;
//            Debug.WriteLine("Updating buff index: " + _buffIndex.ToString());

            if (_buffIndex >= lstBuffers[_fileIndex].Length) // Finished the file
            {
                _fileIndex++;
                if (_fileIndex == lstBuffers.Count)
                    finished = true;
                else
                {
                    _buffIndex = 0;
                    nEmptyBuffersBetweenFiles = 2;
                }
            }

        }

        public short[] getNextBuffer(out int length)
        {
//            Debug.WriteLine("getting next buffer.");
            int i = 0;
            int tmpBuffIndex = _buffIndex;
            while (i < 124 && tmpBuffIndex < lstBuffers[_fileIndex].Length)
            {
                short value;
                int value1 = lstBuffers[_fileIndex][tmpBuffIndex] - lstAverages[_fileIndex];// +0x8000;
                value = (short)value1;

                buff[i] = value;
                tmpBuffIndex++;
                i++;
            }

            length = i;
            return buff;    
        }

        int lastFile = 0;
        int lastBuff = 0;
        public void getCurrentStatus(out String sFilename,out String sFileCount)
        {
            if (lstBuffers != null && _fileIndex >= 0 && _fileIndex < lstBuffers.Count)
            {
                sFilename = Path.GetFileNameWithoutExtension(files[_fileIndex]);
//                if (sFilename.Length > 16)
//                    sFilename = sFilename.Substring(0, 14) + "..";
            }
            else
                sFilename = "";

            if (lstBuffers != null && _fileIndex >= 0 && _fileIndex <= lstBuffers.Count && lstBuffers.Count != 0)
            {
                
                int percentage = 100;
                if (_fileIndex < lstBuffers.Count)
                {
                    percentage = (int)(_buffIndex / (double)lstBuffers[_fileIndex].Length * 100);
                    sFileCount = String.Format("{0} / {1} ({2}%)", _fileIndex + 1, lstBuffers.Count, percentage);
                }
                else
                    sFileCount = String.Format("{0} / {1} ({2}%)", _fileIndex, lstBuffers.Count, percentage);
                if (lastFile > _fileIndex || lastFile == _fileIndex && lastBuff > _buffIndex)
                {
                    Debug.WriteLine(sFileCount);
                    MessageBox.Show("Signal files backtrack detected (" + lastFile + " " + lastBuff + " " + _fileIndex + " " + _buffIndex + ").");
                }
                lastFile = _fileIndex;
                lastBuff = _buffIndex;
            }
            else
                sFileCount = "";
        }
    }
}
