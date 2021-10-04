namespace Sensor_Scope
{
    partial class frmCalibSettings
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
            this.txtGeneralGain = new System.Windows.Forms.TextBox();
            this.txt16vs1gain = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tmrRefreshVal = new System.Windows.Forms.Timer(this.components);
            this.tmrAutoClose = new System.Windows.Forms.Timer(this.components);
            this.picClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // txtGeneralGain
            // 
            this.txtGeneralGain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(194)))), ((int)(((byte)(218)))));
            this.txtGeneralGain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGeneralGain.Location = new System.Drawing.Point(155, 102);
            this.txtGeneralGain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGeneralGain.Name = "txtGeneralGain";
            this.txtGeneralGain.Size = new System.Drawing.Size(95, 15);
            this.txtGeneralGain.TabIndex = 1;
            this.txtGeneralGain.Click += new System.EventHandler(this.txtEqulizer_Click);
            this.txtGeneralGain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCalib_KeyPress);
            // 
            // txt16vs1gain
            // 
            this.txt16vs1gain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(175)))), ((int)(((byte)(194)))), ((int)(((byte)(218)))));
            this.txt16vs1gain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt16vs1gain.Location = new System.Drawing.Point(155, 139);
            this.txt16vs1gain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txt16vs1gain.Name = "txt16vs1gain";
            this.txt16vs1gain.Size = new System.Drawing.Size(95, 15);
            this.txt16vs1gain.TabIndex = 2;
            this.txt16vs1gain.Click += new System.EventHandler(this.txtEqulizer_Click);
            this.txt16vs1gain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCalib_KeyPress);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(159, 197);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tmrRefreshVal
            // 
            this.tmrRefreshVal.Enabled = true;
            this.tmrRefreshVal.Interval = 250;
            this.tmrRefreshVal.Tick += new System.EventHandler(this.tmrRefreshVal_Tick);
            // 
            // tmrAutoClose
            // 
            this.tmrAutoClose.Enabled = true;
            this.tmrAutoClose.Interval = 60000;
            this.tmrAutoClose.Tick += new System.EventHandler(this.tmrAutoClose_Tick);
            // 
            // picClose
            // 
            this.picClose.BackColor = System.Drawing.Color.Transparent;
            this.picClose.Location = new System.Drawing.Point(235, 9);
            this.picClose.Margin = new System.Windows.Forms.Padding(4);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(35, 31);
            this.picClose.TabIndex = 3;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // frmCalibSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Sensor_Scope.Properties.Resources.Sensor_calibration;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(276, 191);
            this.Controls.Add(this.picClose);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt16vs1gain);
            this.Controls.Add(this.txtGeneralGain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCalibSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Constant settings";
            this.Load += new System.EventHandler(this.frmCalibSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox txtGeneralGain;
        public System.Windows.Forms.TextBox txt16vs1gain;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Timer tmrRefreshVal;
        private System.Windows.Forms.Timer tmrAutoClose;
        private System.Windows.Forms.PictureBox picClose;
    }
}