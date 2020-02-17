using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Figure
    {
        public int xstart, xend, ystart, yend, rowcount, colcount;
        public Figure(int _xstart, int _xend, int _ystart, int _yend, int _rowcount, int _colcount)
        {
            xstart = _xstart;
            xend = _xend;
            yend = _yend;
            ystart = _ystart;
            rowcount = _rowcount;
            colcount = _colcount;
        }
        public void swapcoordifbigger()
        {
            if (xstart > xend)
            {
                int buffer = xstart;
                xstart = xend;
                xend = buffer;
            }
            if (ystart > yend)
            {
                int buffer = ystart;
                ystart = yend;
                yend = buffer;
            }
        }
    }
}
