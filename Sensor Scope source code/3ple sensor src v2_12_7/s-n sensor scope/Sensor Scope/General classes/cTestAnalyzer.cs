using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using MigraDoc.Rendering;
using MigraDoc.DocumentObjectModel;
using PdfSharp.Pdf;
using MigraDoc.DocumentObjectModel.Tables;
using System.Drawing;

namespace Sensor_Scope.General_classes
{
    class cTestAnalyzer
    {
        private String sRegularBed_D_Path = @"c:\equipment_check\AME-00200-short\sensor_TF_short.exe";
        private String sTripleBed_T_Path = @"c:\equipment_check\AME-00250-3pl\Triple_bed_Sensor_TF.exe";
        private String sTripleChair_S_Path = @"c:\equipment_check\AME-01260-chair\Chair_Sensor_TF.exe";

        private String _sMatLabOutput = "";
        public String sMatLabOutput
        {
            get { return _sMatLabOutput; }
        }


        private String _sLogsPath;
        public String sLogsPath
        {
            get { return _sLogsPath; }
        }

        private String _sBoardType;
        public String sBoardType
        {
            get { return _sBoardType; }
        }

        private String _sSerialNumber;
        public String sSerialNumber
        {
            get { return _sSerialNumber; }
        }
        public String sShortSerialNumber
        {
            get 
            {
                if (_sSerialNumber.Length > 4)
                    return _sSerialNumber.Substring(_sSerialNumber.Length - 4);
                else
                    return _sSerialNumber;
            }
        }


        private String _sTestersName;
        public String sTestersName
        {
            get { return _sTestersName; }
        }

        private String _sDsteNumber;
        public String sDsteNumber
        {
            get { return _sDsteNumber; }
        }


        public enum STATE
        {
            IDLE,
            MATLAB,
            GEN_REPORT
        }
        private STATE _state = STATE.IDLE;
        internal STATE State
        {
            get { return _state; }
        }


        public cTestAnalyzer(String sLogs, String sName,String sDsteNum,String sSerialNumber)
        {
            _sLogsPath = sLogs;
            _sTestersName = sName;
            _sDsteNumber = sDsteNum;
            // _sSerialNumber = sSerialNumber.PadLeft(4, '0');
            _sSerialNumber = sSerialNumber;//.PadLeft(4, '0');

            if (sLogs.StartsWith(@"C:\equipment_check\AME-00200"))
                _sBoardType = "D";
            else if (sLogs.StartsWith(@"C:\equipment_check\AME-00250-3pl"))
                _sBoardType = "T";
            else if (sLogs.StartsWith(@"C:\equipment_check\AME-01260-chair"))
                _sBoardType = "S";
            else 
                _sBoardType = "-"; // Unknown
        }


        internal string analyze()
        {
            String sTmp = "";

            _state = STATE.MATLAB;
            String sDestination = "";
            String sFolderPrefix = "";
            String sResultsFolder;
            String sDestinationFolder = ".";
            switch (_sBoardType)
            {
                case "D":
                    sDestination = sRegularBed_D_Path;
                    sFolderPrefix = "sensor_";
                    sResultsFolder = @"c:\equipment_check\Results\AME-00200-short\" + sSerialNumber;
                    sDestinationFolder = @"c:\equipment_check\Results\AME-00200-short\pdf";
                    break;
                case "T":
                    sDestination = sTripleBed_T_Path;
                    sFolderPrefix = "coin_";
                    sResultsFolder = @"c:\equipment_check\Results\triple_bed\" + sSerialNumber;
                    sDestinationFolder = @"c:\equipment_check\Results\triple_bed\pdf";
                    break;
                case "S":
                    sDestination = sTripleChair_S_Path;
                    sFolderPrefix = "coin_";
                    sResultsFolder = @"c:\equipment_check\Results\triple_chair\" + sSerialNumber;
                    sDestinationFolder = @"c:\equipment_check\Results\triple_chair\pdf";
                    break;
                default:
                    return "Unknown sensor type detected: ('" + sBoardType + "').";
            }

            // Copy files
//            if (!checkFiles())
//                return "Can't find log files.";
//            sTmp = copyFiles(Path.GetDirectoryName(sDestination) + "\\" + sFolderPrefix + sShortSerialNumber);
//            if (sTmp != "")
//                return "Failed to copy logs to destination folder:\n" + sTmp;

            // Create pdf folder
            if (!Directory.Exists(sDestinationFolder))
                Directory.CreateDirectory(sDestinationFolder);
            // Activcate matlab
            Process p = new Process();
            p.StartInfo.Arguments = sSerialNumber + " " + sDsteNumber;
            p.StartInfo.FileName = sDestination;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            sbStdOut = new StringBuilder();
            p.OutputDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.ErrorDataReceived += new DataReceivedEventHandler(p_OutputDataReceived);
            p.Start();
            p.BeginErrorReadLine();
            p.BeginOutputReadLine();
            p.WaitForExit();
            if (p.ExitCode != 0)
                return "Matlab exited with code " + p.ExitCode.ToString();
            _state = STATE.GEN_REPORT;
            switch (_sBoardType)
            {
                case "D":
                    sTmp = createPdf_D(sResultsFolder, sDestinationFolder);
                    break;
                case "T":
                    sTmp = createPdf_T(sResultsFolder, sDestinationFolder);
                    break;
                case "S":
                    sTmp = createPdf_S(sResultsFolder, sDestinationFolder);
                    break;
            }

            if (sTmp != "")
                return "Failed to create PDF file: " + sTmp;



            return "";
        }

        StringBuilder sbStdOut;
        void p_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            sbStdOut.Append(e.Data);
        }

        private string createPdf_T(String sResultsFolder,String sDestinationFolder)
        {
            Table table;

            String sImageFilePath = sResultsFolder + @"\Coin_" + sSerialNumber + ".emf";
            String sCsvPath = sResultsFolder + @"\Sensor_" + sSerialNumber + "_Coin.csv";
            String sResult = "";

            if (!File.Exists(sImageFilePath) || !File.Exists(sCsvPath))
                return "Can't find results file(s) in " + sResultsFolder;

            #region "document initialization and globals init"
            // Create a new MigraDoc document
            Document document = new Document();
            Paragraph paragraph;
            // Add a section to the document
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.TopMargin = "2.0cm";
            section.PageSetup.BottomMargin = "2.0cm";
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.HeaderDistance = Unit.FromCentimeter(0.2);
            #endregion

            #region "Define styles"
            // Header color
            Style style = document.Styles.AddStyle("myheader", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Underline = Underline.Single;
            style.Font.Bold = true;

            // Section start header (a. b. ...)
            style = document.Styles.AddStyle("sectionHeadStart", "Normal");
            style.Font.Color = Colors.GreenYellow;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Section header's text
            style = document.Styles.AddStyle("sectionHeadText", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Sub Section header's text
            style = document.Styles.AddStyle("subSection", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Italic = true;

            // Tables style
            style = document.Styles.AddStyle("firstColumn", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("firstColumn1", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("fnt12NoBold", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;

            style = document.Styles.AddStyle("fnt12NoBoldUnderline", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;
            style.Font.Underline = Underline.Single;


            // Cell style
            style = document.Styles.AddStyle("normalCell", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;

            #endregion

            #region "page 1"
            #region "Set header"
            HeaderFooter header = section.Headers.Primary;
            //            paragraph = new Paragraph();
            //           //paragraph.Format.Alignment = ParagraphAlignment.Left;
            //          paragraph.AddFormattedText("Sensor ID: Sensor (" + sSerialNumber +  ") DSTE No.: " + sDsteNumber,"myheader");
            //         paragraph.AddImage(@"images/logo.png");
            //        header.Add(paragraph);
            Table tbl_header = new Table();
            tbl_header.Borders.Style = MigraDoc.DocumentObjectModel.BorderStyle.None;
            tbl_header.Borders.Visible = false;
            double dTmp = document.DefaultPageSetup.PageWidth.Centimeter / 4;
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp * 3));
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp));

            Row row = tbl_header.AddRow();
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddFormattedText("Sensor ID: Sensor (" + sSerialNumber + ") DSTE No.: " + sDsteNumber, "myheader");
            row.Cells[0].Add(paragraph);
            row.Cells[1].AddImage(@"images/logo.png");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.VerticalAlignment = VerticalAlignment.Center;
            header.Add(tbl_header);
            #endregion

            #region "set footer"
            // Set Footer
            // Form type and revision
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText("Form: #F-33  Rev. 02");
            // Add paragraph to footer.
            section.Footers.Primary.Add(paragraph);

            // Create a paragraph with centered page number. See definition of style "Footer".
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddPageField();
            paragraph.AddText(" / ");
            paragraph.AddSectionPagesField();
            section.Footers.Primary.Add(paragraph);

            
            // Name and date
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText("Name:" + sTestersName + "\t");
            paragraph.AddDateField("dd.MM.yyyy HH:mm");
            section.Footers.Primary.Add(paragraph);

            #endregion


            #region "Section A: Transfer function of sensor"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("A.", "sectionHeadStart");
            paragraph.AddFormattedText(" Transfer Function of sensor (y axis=[Volt]):", "sectionHeadText");
            paragraph.AddLineBreak();
            String sTempFile = sImageFilePath.Replace(".emf", ".bmp");
            Image img = Image.FromFile(sImageFilePath);
            img = new Bitmap(img, new Size(433, 324));
            img.Save(sTempFile);
            paragraph.AddImage(sTempFile);
            section.Add(paragraph);
            #endregion

            #region "Details table"
            paragraph = new Paragraph();
            paragraph.AddFormattedText("Details:", "subSection");
            section.Add(paragraph);
            String sLine;
            String[] sPlitted1;
            String[] sPlitted2;
            String[] sPlitted3;
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                table = new Table();
                table.Borders.Visible = true;

                // First line (frequencies)
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');

                // Prepare the columns
                table.AddColumn(); // Left header
                for (int i = 0; i < sPlitted1.Length; i++) // Foreach value in first line
                {
                    table.AddColumn(Unit.FromCentimeter(2));
                }
                // Insert the values
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[0].Cells[0].AddParagraph("Frequency");
                table.Rows[1].Cells[0].AddParagraph("Average Amp");
                table.Rows[2].Cells[0].AddParagraph("STD");
                table.Rows[0].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[1].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[2].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < sPlitted1.Length; i++)
                {
                    paragraph = table.Rows[0].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[0].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[1].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[2].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }

                row = table.AddRow();
                row.Height = Unit.FromCentimeter(0.1);
                sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');
                // Add the next 3 rows.
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[4].Cells[0].AddParagraph("Frequency");
                table.Rows[5].Cells[0].AddParagraph("Average Amp");
                table.Rows[6].Cells[0].AddParagraph("STD");
                table.Rows[4].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[5].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[6].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < 10; i++)
                {
                    paragraph = table.Rows[4].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[4].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[5].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[6].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }



            }
            section.Add(table);
            #endregion
            #endregion

            #region "page 2"
            section.AddPageBreak();

            #region "Section B. PZE"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("B.", "sectionHeadStart");
            paragraph.AddFormattedText(" PZE:", "sectionHeadText");
            paragraph.AddFormattedText("     ", "myheader");
            section.Add(paragraph);
            #endregion


            #region "Section C. Results:"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("C.", "sectionHeadStart");
            paragraph.AddFormattedText(" Results:", "sectionHeadText");
            section.Add(paragraph);
            // Add the table
            table = new Table();
            table.Borders.Visible = true;

            // Prepare the columns
            double[] tblWidths = { 3.4, 3.6, 3, 4.8, 4.6, 3.3 };
            String[] firstHeader = { "Parameter", "Test\nrepetitiveness\n(if all 3 tests are similar enough)", "Straightness of TF", "Sensitivity of sensor", "", "DSTE Serial Number" };
            String[] secondHeader = { "Test Requirements", "%Average STD of tests of average amp", "Average STD", "Average Amp", "Amp of each Freq." };
            String[] thirdHeader = { "Pass Criteria", "< 0.6", "< 7800", "222000<=Amp<=320000", "222000<=Amp<=330000" };

            // Create 6 columns
            for (int i = 0; i < 6; i++)
            {
                table.AddColumn(Unit.FromCentimeter(tblWidths[i]));
            }

            // Prepare 4 lines.
            table.AddRow();
            table.AddRow();
            table.AddRow();
            table.AddRow();
            // Header 1
            row = table.Rows[0];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 6; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.LightGreen;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(firstHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }
            row.Cells[3].MergeRight = 1;
            row.Cells[5].MergeDown = 2;

            // Header 2
            row = table.Rows[1];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 5; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.GreenYellow;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(secondHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }

            // Header 3
            row = table.Rows[2];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 5; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                // Text
                paragraph = row.Cells[i].AddParagraph(thirdHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }

            // Results
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                for (int i = 0; i < 15; i++)
                    sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sr.ReadLine();
                sLine = sr.ReadLine();
                sResult = sLine.Split(',')[2];
                row = table.Rows[3];
                row.Cells[0].Shading.Color = Colors.LightGray;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                paragraph = row.Cells[0].AddParagraph("Test Results");
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                for (int i = 2; i <= 6; i++)
                {
                    if (i == 6)
                    {
                        String sTmp = ""; // Get all DSTE numbers from all cells, for some reason it is splitted.
                        int j = 6;
                        while (j < sPlitted1.Length && sPlitted1[j] != "")
                        {
                            sTmp += sPlitted1[j];
                            ++j;
                        }
                        paragraph = row.Cells[i - 1].AddParagraph(sTmp);
                    }
                    else
                        paragraph = row.Cells[i-1].AddParagraph(sPlitted1[i]);
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }
            }
            section.Add(table);
            #endregion

            #region "Signature"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();

            // Pass / fail
            paragraph.AddFormattedText("Pass/Fail:  ", "firstColumn");
            paragraph.AddFormattedText(sResult, "fnt12NoBold");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            // Name
            paragraph.AddFormattedText("Name:", "firstColumn");
            paragraph.AddFormattedText(sTestersName, "fnt12NoBoldUnderline");
            paragraph.AddText(" ");
            paragraph.AddSpace(20);
            // Signature
            paragraph.AddFormattedText("Signature:", "firstColumn");
            paragraph.AddText(" ");
            paragraph.AddFormattedText("________________________", "fnt12NoBoldUnderline");

            section.Add(paragraph);
            #endregion


            #endregion

            #region "Create the pdf"
            document.UseCmykColor = true;
            bool unicode = false;
            // An enum indicating whether to embed fonts or not.
            // This setting applies to all font programs used in the document.
            // This setting has no effect on the RTF renderer.
            // (The term 'font program' is used by Adobe for a file containing a font. Technically a 'font file'
            // is a collection of small programs and each program renders the glyph of a character when executed.
            // Using a font in PDFsharp may lead to the embedding of one or more font programms, because each outline
            // (regular, bold, italic, bold+italic, ...) has its own fontprogram)
            PdfFontEmbedding embedding = PdfFontEmbedding.Always;  // Set to PdfFontEmbedding.None or PdfFontEmbedding.Always only
            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            // Save the document...
            string filename = sSerialNumber + DateTime.Now.ToString(" yyyy-MM-dd HH-mm-ss") + ".pdf";
            pdfRenderer.PdfDocument.Save(sDestinationFolder + "\\" + filename);
            // ...and start a viewer.
            Process.Start(sDestinationFolder + "\\" + filename);

            #endregion



            return "";
        }

        private string createPdf_D(String sResultsFolder, String sDestinationFolder)
        {
            Table table;

            String sImageFilePath = sResultsFolder + @"\sensor_" + sSerialNumber + ".emf";
            String sCsvPath = sResultsFolder + @"\Sensor_" + sSerialNumber + ".csv";
            String sResult = "";

            if (!File.Exists(sImageFilePath) || !File.Exists(sCsvPath))
                return "Can't find results file(s) in " + sResultsFolder;

            #region "document initialization and globals init"
            // Create a new MigraDoc document
            Document document = new Document();
            Paragraph paragraph;
            // Add a section to the document
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.TopMargin = "2.0cm";
            section.PageSetup.BottomMargin = "2.0cm";
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.HeaderDistance = Unit.FromCentimeter(0.2);
            #endregion

            #region "Define styles"
            // Header color
            Style style = document.Styles.AddStyle("myheader", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Underline = Underline.Single;
            style.Font.Bold = true;

            // Section start header (a. b. ...)
            style = document.Styles.AddStyle("sectionHeadStart", "Normal");
            style.Font.Color = Colors.GreenYellow;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Section header's text
            style = document.Styles.AddStyle("sectionHeadText", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Sub Section header's text
            style = document.Styles.AddStyle("subSection", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Italic = true;

            // Tables style
            style = document.Styles.AddStyle("firstColumn", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("firstColumn1", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("fnt12NoBold", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;

            style = document.Styles.AddStyle("fnt12NoBoldUnderline", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;
            style.Font.Underline = Underline.Single;


            // Cell style
            style = document.Styles.AddStyle("normalCell", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;

            #endregion

            #region "page 1"
            #region "Set header"
            HeaderFooter header = section.Headers.Primary;
            //            paragraph = new Paragraph();
            //           //paragraph.Format.Alignment = ParagraphAlignment.Left;
            //          paragraph.AddFormattedText("Sensor ID: Sensor (" + sSerialNumber +  ") DSTE No.: " + sDsteNumber,"myheader");
            //         paragraph.AddImage(@"images/logo.png");
            //        header.Add(paragraph);
            Table tbl_header = new Table();
            tbl_header.Borders.Style = MigraDoc.DocumentObjectModel.BorderStyle.None;
            tbl_header.Borders.Visible = false;
            double dTmp = document.DefaultPageSetup.PageWidth.Centimeter / 4;
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp * 3));
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp));

            Row row = tbl_header.AddRow();
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddFormattedText("Sensor ID: Sensor (" + sSerialNumber + ") DSTE No.: " + sDsteNumber, "myheader");
            row.Cells[0].Add(paragraph);
            row.Cells[1].AddImage(@"images/logo.png");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.VerticalAlignment = VerticalAlignment.Center;
            header.Add(tbl_header);
            #endregion

            #region "set footer"
            // Set Footer
            // For type and revision
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText("Form #: F-28  Rev:02");
            // Add paragraph to footer.
            section.Footers.Primary.Add(paragraph);

            // Create a paragraph with centered page number. See definition of style "Footer".
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddPageField();
            paragraph.AddText(" / ");
            paragraph.AddSectionPagesField();
            // Add paragraph to footer.
            section.Footers.Primary.Add(paragraph);

            // Name and date
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText("Name:" + sTestersName + "\t");
            paragraph.AddDateField("dd.MM.yyyy HH:mm");
            section.Footers.Primary.Add(paragraph);



            #endregion


            #region "Section A: Transfer function of sensor"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("A.", "sectionHeadStart");
            paragraph.AddFormattedText(" Transfer Function of sensor (y axis=[Volt]):", "sectionHeadText");
            paragraph.AddLineBreak();
            String sTempFile = sImageFilePath.Replace(".emf", ".bmp");
            Image img = Image.FromFile(sImageFilePath);
            img = new Bitmap(img, new Size(433, 324));
            img.Save(sTempFile);
            paragraph.AddImage(sTempFile);
            section.Add(paragraph);
            #endregion

            #region "Details table"
            paragraph = new Paragraph();
            paragraph.AddFormattedText("Details:", "subSection");
            section.Add(paragraph);
            String sLine;
            String[] sPlitted1;
            String[] sPlitted2;
            String[] sPlitted3;
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                table = new Table();
                table.Borders.Visible = true;

                // First line (frequencies)
                sLine = sr.ReadLine();
                sLine = removeEndChar(',', sLine);
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');

                // Prepare the columns
                table.AddColumn(); // Left header
                for (int i = 0; i < sPlitted1.Length; i++) // Foreach value in first line
                {
                    table.AddColumn(Unit.FromCentimeter(2));
                }
                // Insert the values
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[0].Cells[0].AddParagraph("Frequency");
                table.Rows[1].Cells[0].AddParagraph("Average Amp");
                table.Rows[2].Cells[0].AddParagraph("STD");
                table.Rows[0].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[1].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[2].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < sPlitted1.Length; i++)
                {
                    paragraph = table.Rows[0].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[0].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[1].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[2].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }

                row = table.AddRow();
                row.Height = Unit.FromCentimeter(0.1);
                sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');
                /*
                // Add the next 3 rows.
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[4].Cells[0].AddParagraph("Frequency");
                table.Rows[5].Cells[0].AddParagraph("Average Amp");
                table.Rows[6].Cells[0].AddParagraph("STD");
                table.Rows[4].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[5].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[6].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < 10; i++)
                {
                    paragraph = table.Rows[4].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[4].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[5].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[6].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }

                 */
            }
            section.Add(table);
            #endregion
            #endregion

            #region "page 2"
            section.AddPageBreak();

            #region "Section B. PZE"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("B.", "sectionHeadStart");
            paragraph.AddFormattedText(" PZE:", "sectionHeadText");
            paragraph.AddFormattedText("     ", "myheader");
            section.Add(paragraph);
            #endregion


            #region "Section C. Results:"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("C.", "sectionHeadStart");
            paragraph.AddFormattedText(" Results:", "sectionHeadText");
            section.Add(paragraph);
            // Add the table
            table = new Table();
            table.Borders.Visible = true;

            // Prepare the columns
            double[] tblWidths = { 3.4, 3.6, 3, 4.8, 4.6, 3.3 };
            String[] firstHeader = { "Parameter", "Test\nrepetitiveness\n(if all 3 tests are similar enough)", "Straightness of TF", "Sensitivity of sensor", "", "DSTE Serial Number" };
            String[] secondHeader = { "Test Requirements", "%Average STD of tests of average amp", "Average STD", "Average Amp", "Amp of each Freq." };
            String[] thirdHeader = { "Pass Criteria", "< 0.6", "< 9900", "222000<=Amp<=320000", "222000<=Amp<=330000" };

            // Create 6 columns
            for (int i = 0; i < 6; i++)
            {
                table.AddColumn(Unit.FromCentimeter(tblWidths[i]));
            }

            // Prepare 4 lines.
            table.AddRow();
            table.AddRow();
            table.AddRow();
            table.AddRow();
            // Header 1
            row = table.Rows[0];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 6; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.LightGreen;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(firstHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }
            row.Cells[3].MergeRight = 1;
            row.Cells[5].MergeDown = 2;

            // Header 2
            row = table.Rows[1];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 5; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.GreenYellow;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(secondHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }

            // Header 3
            row = table.Rows[2];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 5; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                // Text
                paragraph = row.Cells[i].AddParagraph(thirdHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }

            // Results
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                for (int i = 0; i < 15; i++)
                    sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sr.ReadLine();
                sLine = sr.ReadLine();
                sResult = sLine.Split(',')[1];
                row = table.Rows[3];
                row.Cells[0].Shading.Color = Colors.LightGray;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                paragraph = row.Cells[0].AddParagraph("Test Results");
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                for (int i = 1; i <= 5; i++)
                {
                    if (i == 5)
                    {
                        String sTmp = ""; // Get all DSTE numbers from all cells, for some reason it is splitted.
                        int j = 5;
                        while (j < sPlitted1.Length && sPlitted1[j] != "")
                        {
                            sTmp += sPlitted1[j];
                            ++j;
                        }
                        paragraph = row.Cells[i].AddParagraph(sTmp);
                    }
                    else
                        paragraph = row.Cells[i].AddParagraph(sPlitted1[i]);
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }
            }
            section.Add(table);
            #endregion

            #region "Signature"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();

            // Pass / fail
            paragraph.AddFormattedText("Pass/Fail:  ", "firstColumn");
            paragraph.AddFormattedText(sResult, "fnt12NoBold");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();


            // Name
            paragraph.AddFormattedText("Name:", "firstColumn");
            paragraph.AddFormattedText(sTestersName, "fnt12NoBoldUnderline");
            paragraph.AddText(" ");
            paragraph.AddSpace(20);
            // Signature
            paragraph.AddFormattedText("Signature:", "firstColumn");
            paragraph.AddText(" ");
            paragraph.AddFormattedText("________________________", "fnt12NoBoldUnderline");

            section.Add(paragraph);
            #endregion


            #endregion

            #region "Create the pdf"
            document.UseCmykColor = true;
            bool unicode = false;
            // An enum indicating whether to embed fonts or not.
            // This setting applies to all font programs used in the document.
            // This setting has no effect on the RTF renderer.
            // (The term 'font program' is used by Adobe for a file containing a font. Technically a 'font file'
            // is a collection of small programs and each program renders the glyph of a character when executed.
            // Using a font in PDFsharp may lead to the embedding of one or more font programms, because each outline
            // (regular, bold, italic, bold+italic, ...) has its own fontprogram)
            PdfFontEmbedding embedding = PdfFontEmbedding.Always;  // Set to PdfFontEmbedding.None or PdfFontEmbedding.Always only
            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            // Save the document...
            string filename = sSerialNumber + DateTime.Now.ToString(" yyyy-MM-dd HH-mm-ss") + ".pdf";
            pdfRenderer.PdfDocument.Save(sDestinationFolder + "\\" + filename);
            // ...and start a viewer.
            Process.Start(sDestinationFolder + "\\" + filename);

            #endregion



            return "";
        }



        private string createPdf_S(String sResultsFolder, String sDestinationFolder)
        {
            Table table;

            String sImageFilePath = sResultsFolder + @"\Coin_" + sSerialNumber + ".emf";
            String sCsvPath = sResultsFolder + @"\Sensor_" + sSerialNumber + "_Coin.csv";
            String sResult = "";

            if (!File.Exists(sImageFilePath) || !File.Exists(sCsvPath))
                return "Can't find results file(s) in " + sResultsFolder;

            #region "document initialization and globals init"
            // Create a new MigraDoc document
            Document document = new Document();
            Paragraph paragraph;
            // Add a section to the document
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section.PageSetup.StartingNumber = 1;
            section.PageSetup.TopMargin = "2.0cm";
            section.PageSetup.BottomMargin = "2.0cm";
            section.PageSetup.Orientation = MigraDoc.DocumentObjectModel.Orientation.Landscape;
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.HeaderDistance = Unit.FromCentimeter(0.2);
            #endregion

            #region "Define styles"
            // Header color
            Style style = document.Styles.AddStyle("myheader", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Underline = Underline.Single;
            style.Font.Bold = true;

            // Section start header (a. b. ...)
            style = document.Styles.AddStyle("sectionHeadStart", "Normal");
            style.Font.Color = Colors.GreenYellow;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Section header's text
            style = document.Styles.AddStyle("sectionHeadText", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Bold = true;
            style.Font.Size = 16;
            // Sub Section header's text
            style = document.Styles.AddStyle("subSection", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 16;
            style.Font.Italic = true;

            // Tables style
            style = document.Styles.AddStyle("firstColumn", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("firstColumn1", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;
            style.Font.Bold = true;

            style = document.Styles.AddStyle("fnt12NoBold", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;

            style = document.Styles.AddStyle("fnt12NoBoldUnderline", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 12;
            style.Font.Bold = false;
            style.Font.Underline = Underline.Single;


            // Cell style
            style = document.Styles.AddStyle("normalCell", "Normal");
            style.Font.Color = Colors.Black;
            style.Font.Size = 8;

            #endregion

            #region "page 1"
            #region "Set header"
            HeaderFooter header = section.Headers.Primary;
            //            paragraph = new Paragraph();
            //           //paragraph.Format.Alignment = ParagraphAlignment.Left;
            //          paragraph.AddFormattedText("Sensor ID: Sensor (" + sSerialNumber +  ") DSTE No.: " + sDsteNumber,"myheader");
            //         paragraph.AddImage(@"images/logo.png");
            //        header.Add(paragraph);
            Table tbl_header = new Table();
            tbl_header.Borders.Style = MigraDoc.DocumentObjectModel.BorderStyle.None;
            tbl_header.Borders.Visible = false;
            double dTmp = document.DefaultPageSetup.PageWidth.Centimeter / 4;
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp * 3));
            tbl_header.AddColumn(Unit.FromCentimeter(dTmp));

            Row row = tbl_header.AddRow();
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddFormattedText("Chair Sensor ID: Sensor (" + sSerialNumber + ") DSTE No.: " + sDsteNumber, "myheader");
            row.Cells[0].Add(paragraph);
            row.Cells[1].AddImage(@"images/logo.png");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Right;
            row.VerticalAlignment = VerticalAlignment.Center;
            header.Add(tbl_header);
            #endregion

            #region "set footer"
            // Set Footer
            // For type and revision
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText("Form #: F-32  Rev: 01");
            // Add paragraph to footer.
            section.Footers.Primary.Add(paragraph);

            // Create a paragraph with centered page number. See definition of style "Footer".
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            paragraph.AddPageField();
            paragraph.AddText(" / ");
            paragraph.AddSectionPagesField();
            // Add paragraph to footer.
            section.Footers.Primary.Add(paragraph);

            // Name and date
            paragraph = new Paragraph();
            paragraph.Format.Alignment = ParagraphAlignment.Right;
            paragraph.AddText("Name:" + sTestersName + "\t");
            paragraph.AddDateField("dd.MM.yyyy HH:mm");
            section.Footers.Primary.Add(paragraph);



            #endregion

            #region "Section 1: Accelometer"
            // Serial number
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("1. Accelerometer", "sectionHeadStart");
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("A. ", "sectionHeadStart");
            paragraph.AddFormattedText("Board S/N: ", "subSection");
            paragraph.AddFormattedText(sSerialNumber, "fnt12NoBoldUnderline");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            section.Add(paragraph);

            // Results
            paragraph = new Paragraph();
            paragraph.AddFormattedText("B. ", "sectionHeadStart");
            paragraph.AddFormattedText("Result: ", "subSection");
            paragraph.AddLineBreak();
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                String sTmp = "";
                for (int i = 0; i <= 21; i++)
                    sTmp = sr.ReadLine();
                sTmp = sTmp.Split(',')[0];
                paragraph.AddFormattedText(sTmp, "firstColumn");
            }

            section.Add(paragraph);

            section.AddPageBreak();
            #endregion

            #region "Section A: Transfer function of sensor"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("2. Coin", "sectionHeadStart");
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("A.", "sectionHeadStart");
            paragraph.AddFormattedText(" Transfer Function of sensor (y axis=[Volt]):", "sectionHeadText");
            paragraph.AddLineBreak();
            String sTempFile = sImageFilePath.Replace(".emf", ".bmp");
            Image img = Image.FromFile(sImageFilePath);
            img = new Bitmap(img, new Size(433, 324));
            img.Save(sTempFile);
            paragraph.AddImage(sTempFile);
            section.Add(paragraph);
            #endregion

            #region "Details table"
            paragraph = new Paragraph();
            paragraph.AddFormattedText("Details:", "subSection");
            section.Add(paragraph);
            String sLine;
            String[] sPlitted1;
            String[] sPlitted2;
            String[] sPlitted3;
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                table = new Table();
                table.Borders.Visible = true;

                // First line (frequencies)
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');

                // Prepare the columns
                table.AddColumn(); // Left header
                for (int i = 0; i < sPlitted1.Length; i++) // Foreach value in first line
                {
                    table.AddColumn(Unit.FromCentimeter(2));
                }
                // Insert the values
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[0].Cells[0].AddParagraph("Frequency");
                table.Rows[1].Cells[0].AddParagraph("Average Amp");
                table.Rows[2].Cells[0].AddParagraph("STD");
                table.Rows[0].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[1].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[2].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < sPlitted1.Length; i++)
                {
                    paragraph = table.Rows[0].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[0].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[1].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[2].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }

                row = table.AddRow();
                row.Height = Unit.FromCentimeter(0.1);
                sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted2 = sLine.Split(',');
                sLine = sr.ReadLine();
                sPlitted3 = sLine.Split(',');
                // Add the next 3 rows.
                table.AddRow();
                table.AddRow();
                table.AddRow();
                table.Rows[4].Cells[0].AddParagraph("Frequency");
                table.Rows[5].Cells[0].AddParagraph("Average Amp");
                table.Rows[6].Cells[0].AddParagraph("STD");
                table.Rows[4].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[5].Cells[0].Format.Shading.Color = Colors.LightGray;
                table.Rows[6].Cells[0].Format.Shading.Color = Colors.LightGray;
                for (int i = 0; i < 10; i++)
                {
                    paragraph = table.Rows[4].Cells[i + 1].AddParagraph(sPlitted1[i]);
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    table.Rows[4].Cells[i + 1].Shading.Color = Colors.GreenYellow;
                    paragraph = table.Rows[5].Cells[i + 1].AddParagraph(round(sPlitted2[i], 1));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                    paragraph = table.Rows[6].Cells[i + 1].AddParagraph(round(sPlitted3[i], 4));
                    paragraph.Style = "normalCell";
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }



            }
            section.Add(table);
            #endregion
            #endregion

            #region "page 2"
            section.AddPageBreak();

            #region "Section B. PZE"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("B.", "sectionHeadStart");
            paragraph.AddFormattedText(" PZE:", "sectionHeadText");
            paragraph.AddFormattedText("     ", "myheader");
            section.Add(paragraph);
            #endregion


            #region "Section C. Results:"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddFormattedText("C.", "sectionHeadStart");
            paragraph.AddFormattedText(" Results:", "sectionHeadText");
            section.Add(paragraph);
            // Add the table
            table = new Table();
            table.Borders.Visible = true;

            // Prepare the columns
            double[] tblWidths = { 3.4, 3.3, 3, 3.4, 4.2, 4.2,2.8 };

            String[] firstHeader = { "Frequency\n[Hz]", "0.1-16", "1-2", "2-16", "2-16","",""};
            String[] secondHeader = { "Parameter", "Test\nrepetitiveness\n(if all 3 tests are similar enough)","Ration","Straightness of TF in high freq.", "Sensitivity of sensor", "", "DSTE Serial Number" };
            String[] thirdHeader = { "Test Requirements", "Average %(STD of tests / Amp)", "Ratio 2/1 Hz", "Average STD", "Average Amp","Amp of each Freq." };
            String[] FourthHeader = { "Pass Criteria", "< 0.6", "< 1.0386", "<7800", "Amp<=320000","222000<=Amp<=330000","" };

            // Create 7 columns
            for (int i = 0; i < 7; i++)
            {
                table.AddColumn(Unit.FromCentimeter(tblWidths[i]));
            }

            // Prepare 5 lines.
            table.AddRow();
            table.AddRow();
            table.AddRow();
            table.AddRow();
            table.AddRow();
            // Header 1
            row = table.Rows[0];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 7; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;

                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.LightGreen;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(firstHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }
            row.Cells[4].MergeRight = 1;

            // Header 2
            row = table.Rows[1];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 7; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.LightGreen;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(secondHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }
            row.Cells[4].MergeRight = 1;
            row.Cells[6].MergeDown = 2;


            // Header 3
            row = table.Rows[2];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 6; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.GreenYellow;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }
                // Text
                paragraph = row.Cells[i].AddParagraph(thirdHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }

            // Header 4
            row = table.Rows[3];
            row.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < 6; i++)
            {
                // Shading
                if (i == 0)
                {
                    row.Cells[0].Shading.Color = Colors.LightGray;
                    row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                }
                else
                {
                    row.Cells[i].Shading.Color = Colors.LightBlue;
                    row.Cells[i].Format.Alignment = ParagraphAlignment.Center;
                }

                // Text
                paragraph = row.Cells[i].AddParagraph(FourthHeader[i]);
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Center;
            }


            // Results
            using (StreamReader sr = new StreamReader(sCsvPath))
            {
                for (int i = 0; i < 12; i++)
                    sr.ReadLine();
                sLine = sr.ReadLine();
                sPlitted1 = sLine.Split(',');
                sr.ReadLine();
                sLine = sr.ReadLine();
                sResult = sLine.Split(',')[2];
                row = table.Rows[4];
                row.Cells[0].Shading.Color = Colors.LightGray;
                row.Cells[0].Format.Alignment = ParagraphAlignment.Left;
                paragraph = row.Cells[0].AddParagraph("Test Results");
                paragraph.Style = "firstColumn1";
                paragraph.Format.Alignment = ParagraphAlignment.Left;
                for (int i = 2; i <= 7; i++)
                {
                    if (i == 7)
                    {
                        String sTmp = ""; // Get all DSTE numbers from all cells, for some reason it is splitted.
                        int j = 7;
                        while (j < sPlitted1.Length && sPlitted1[j] != "")
                        {
                            sTmp += sPlitted1[j];
                            ++j;
                        }
                        paragraph = row.Cells[i -1].AddParagraph(sTmp);
                    }
                    else
                        paragraph = row.Cells[i - 1].AddParagraph(sPlitted1[i]);
                    paragraph.Format.Alignment = ParagraphAlignment.Center;
                }
            }
            section.Add(table);
            #endregion

            #region "Signature"
            paragraph = new Paragraph();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();

            // Pass / fail
            paragraph.AddFormattedText("Pass/Fail:  ", "firstColumn");
            paragraph.AddFormattedText(sResult, "fnt12NoBold");
            paragraph.AddLineBreak();
            paragraph.AddLineBreak();


            // Name
            paragraph.AddFormattedText("Name:", "firstColumn");
            paragraph.AddFormattedText(sTestersName, "fnt12NoBoldUnderline");
            paragraph.AddText(" ");
            paragraph.AddSpace(20);
            // Signature
            paragraph.AddFormattedText("Signature:", "firstColumn");
            paragraph.AddText(" ");
            paragraph.AddFormattedText("________________________", "fnt12NoBoldUnderline");

            section.Add(paragraph);
            #endregion


            #endregion

            #region "Create the pdf"
            document.UseCmykColor = true;
            bool unicode = false;
            // An enum indicating whether to embed fonts or not.
            // This setting applies to all font programs used in the document.
            // This setting has no effect on the RTF renderer.
            // (The term 'font program' is used by Adobe for a file containing a font. Technically a 'font file'
            // is a collection of small programs and each program renders the glyph of a character when executed.
            // Using a font in PDFsharp may lead to the embedding of one or more font programms, because each outline
            // (regular, bold, italic, bold+italic, ...) has its own fontprogram)
            PdfFontEmbedding embedding = PdfFontEmbedding.Always;  // Set to PdfFontEmbedding.None or PdfFontEmbedding.Always only
            // Create a renderer for the MigraDoc document.
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(unicode, embedding);

            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;

            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            // Save the document...
            string filename = sSerialNumber + DateTime.Now.ToString(" yyyy-MM-dd HH-mm-ss") + ".pdf";
            pdfRenderer.PdfDocument.Save(sDestinationFolder + "\\" + filename);
            // ...and start a viewer.
            Process.Start(sDestinationFolder + "\\" + filename);

            #endregion



            return "";
        }

        
        private string round(string p, int p_2)
        {
            double d = 0;
            double.TryParse(p, out d);
            return Math.Round(d, p_2).ToString();
        }

        private string copyFiles(String sDestination)
        {
            DialogResult rc;
            // Check for log files
            String[] files = Directory.GetFiles(sLogsPath);
            if (files.Length == 0)
                return "No log files were found.";

            try
            {
            // Create destination path
                if (Directory.Exists(sDestination))
                {
                    rc = MessageBox.Show("Destination folder " + sDestination + " already exist.\nFiles in this folder might be overwritten.\nAre you sure you want to continue?","Warning",MessageBoxButtons.YesNo);
                    if (rc != DialogResult.Yes)
                        return "Canceled by the user.";

                }
                Directory.CreateDirectory(sDestination);
                foreach (String s in files)
                {
                    FileInfo fi = new FileInfo(s);
                    if (fi.Length > 204800) // SKip small files
                        File.Copy(s,sDestination + "\\" + Path.GetFileName(s),true);
                }
            }
            catch (Exception e)
            {
                return "Failed to copy files to destination " + sDestination + ".\n" + e.Message;
            }



            return "";
        }

        private Boolean checkFiles()
        {
            // Check for log files path
            if (!Directory.Exists(_sLogsPath))
                return false;

            return true;
        }

        private string removeEndChar(char char2remove, string str)
        {
            int i;

            for (i = str.Length-1;i>0;i--)
            {
                if (str[i] != char2remove)
                    break;
            }

            if (i + 1 < str.Length)
                return str.Remove(i + 1);
            else
                return str;
        }
    }
}
