using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sensor_Scope
{
    public partial class frmCalibSettings : Form
    {
        cTesterComm ctester;
        public frmCalibSettings(cTesterComm i_ctester)
        {
            InitializeComponent();
            ctester = i_ctester;
        }

        private void txtCalib_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            Double dTmp;
            TextBox tb = (TextBox)sender;
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 13 && e.KeyChar != '.' && e.KeyChar != (Char)Keys.Back && e.KeyChar != (Char)Keys.Delete)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 13)
            {
                if (!double.TryParse(tb.Text, out dTmp))
                    button1.Focus();

                if (dTmp > 3.5)
                    dTmp = 3.5;
                if (dTmp < 0.5)
                    dTmp = 0.5;

                dTmp *= 1000;
                if (tb.Name == txtCalib.Name)
                    ctester.nCalibrationConstant = (int)dTmp;
                else if (tb.Name == txtDac.Name)
                    ctester.nDACConstant = (int)dTmp;
                button1.Focus();
            }
        }
    }
}
