using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Sensor_Scope
{
    class cErrorHandler
    {

        static public void show_error(Exception e1)
        {
            MessageBox.Show("An error occured:\n" + e1.Message);
        }

    }
}
