using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Sensor_Scope
{
    class peaks
    {
        
        // One peaks info.
        public class c_peak
        {
            long n_max = 0;
            long n_min = 0;
            float n_max_graph_x = -1;
            float n_max_graph_y = -1;
            float n_min_graph_x = -1;
            float n_min_graph_y = -1;
            long n_diff = 0;
            Boolean did_draw = false; // Did we draw the peaks mark?

            public c_peak(long p_max, long p_min, long p_max_graph_x, long p_max_graph_y, long p_min_graph_x, long p_min_graph_y)
            {
                n_max = p_max;
                n_min = p_min;
                n_max_graph_x = p_max_graph_x;
                n_max_graph_y = p_max_graph_y;
                n_min_graph_x = p_min_graph_x;
                n_min_graph_y = p_min_graph_y;
                n_diff = p_max - p_min;
            }
        }

        public long n_all_min_peak = -1;
        public long n_all_max_peak = -1;
        public long n_all_diff = -1;

        public ArrayList Peaks = new ArrayList(); 
    }
}
