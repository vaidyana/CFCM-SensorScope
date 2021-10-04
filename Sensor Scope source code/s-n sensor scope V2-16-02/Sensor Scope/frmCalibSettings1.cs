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
        cTesterComm ctester1;

        public frmCalibSettings(cTesterComm i_ctester)
        {
            InitializeComponent();
            ctester1 = i_ctester;
        }

        private void txtCalib_KeyPress(object sender, KeyPressEventArgs e)
        {
            tmrAutoClose.Stop();
            tmrAutoClose.Start();
            int nTmp;
            TextBox tb = (TextBox)sender;
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 13 && e.KeyChar != '.' && e.KeyChar != (Char)Keys.Back && e.KeyChar != (Char)Keys.Delete)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == 13)
            {
                e.Handled = true;

                if (!int.TryParse(tb.Text, out nTmp))
                    button1.Focus();

                if (nTmp > 250)
                    nTmp = 250;
                if (nTmp < 20)
                    nTmp = 20;

                if (tb.Name == txtGeneralGain.Name)
                    ctester1.EE_GeneralGain = nTmp;
                else if (tb.Name == txt16vs1gain.Name)
                    ctester1.EE_16_Vs_1_Gain = nTmp;
                button1.Focus();
            }
        }

        private void tmrRefreshVal_Tick(object sender, EventArgs e)
        {
            int tmp = -1;
            int.TryParse(txtGeneralGain.Text,out tmp);
            if (!txtGeneralGain.Focused && (tmp != ctester1.EE_GeneralGain || !txtGeneralGain.Text.EndsWith("%") ))
                txtGeneralGain.Text = ctester1.EE_GeneralGain.ToString() + "%";

            tmp = -1;
            int.TryParse(txt16vs1gain.Text, out tmp);
            if (!txt16vs1gain.Focused && (tmp != ctester1.EE_16_Vs_1_Gain || !txt16vs1gain.Text.EndsWith("%")))
                txt16vs1gain.Text = ctester1.EE_16_Vs_1_Gain.ToString() + "%";

        }

        private void frmCalibSettings_Load(object sender, EventArgs e)
        {
            tmrRefreshVal_Tick(null,null);
        }

        private void tmrAutoClose_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEqulizer_Click(object sender, EventArgs e)
        {
            if (FRMMain.CurrentInputBox != null)
                return;
            tmrAutoClose.Stop();
            String res = "";
            Point location = new Point(this.Location.X+14, this.Location.Y + this.Size.Height);
            DialogResult rc = FRMMain.InputBox("Password required", "Please enter password:", ref res,location);
            if (rc == System.Windows.Forms.DialogResult.OK && res == "2984")
            {
                TextBox tb = (TextBox)sender;
                tb.SelectAll();
                tb.Focus();
            }
            else
                button1.Focus();
            tmrAutoClose.Start();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
