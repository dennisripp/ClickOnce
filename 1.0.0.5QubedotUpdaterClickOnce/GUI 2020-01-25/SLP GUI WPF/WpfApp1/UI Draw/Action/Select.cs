using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    /*
     * select is currently not working
     */
    class Select
    {
        private int xstart, xend, ystart, yend, rowcount, colcount, xstartimage, ystartimage, selectedheight, selectedwidth;
        private bool[,] selectedarray, startarray;
        public Select()
        {
        }
        public void SelectArray(int _xselectstart, int _xselectend, int _yselectstart, int _yselectend, int _rowcount, int _colcount, bool[,] _startarray)
        {
            startarray = new bool[_colcount, _rowcount];
            xstart = _xselectstart;
            xend = _xselectend;
            yend = _yselectend;
            ystart = _yselectstart;
            rowcount = _rowcount;
            colcount = _colcount;
            for (int i = 0; i < colcount; i++)
            {
                for (int j = 0; j < rowcount; j++)
                {
                    startarray[i, j] = _startarray[i, j];
                }
            }
            swapcoordifbigger();
            selectedwidth = xend - xstart;
            selectedheight = yend - ystart;
            selectedarray = new bool[selectedwidth + 1, selectedheight + 1];
            for (int i = xstart; i <= xend; i++)
            {
                for (int j = ystart; j <= yend; j++)
                {
                    selectedarray[i - xstart, j - ystart] = startarray[i, j];
                }
            }
            xstartimage = xstart;
            ystartimage = ystart;
        }
        public bool[,] ShiftSelectedArray(int _xstart, int _xend, int _ystart, int _yend, int _rowcount, int _colcount, bool[,] _startarray)
        {
            bool[,] outputbool = new bool[colcount, rowcount];
            int subx, suby, newx, newy;
            xstart = _xstart;
            xend = _xend;
            yend = _yend;
            ystart = _ystart;
            rowcount = _rowcount;
            colcount = _colcount;

            subx = xstart - xstartimage;
            suby = ystart - ystartimage;
            newx = xend - subx;
            newy = yend - suby;
            for (int i = 0; i <= colcount; i++)
            {
                for (int j = 0; j <= rowcount; j++)
                {
                    if (j >= 0 && j < rowcount && i >= 0 && i < colcount)
                    {
                        if (i >= xstartimage && i <= xstartimage + selectedwidth && j >= ystartimage && j <= ystartimage + selectedheight)
                        {
                            if (startarray[i, j] == true)
                            {
                                outputbool[i, j] = true;
                            }
                        }
                        if ((i - newx) >= 0 && (i - newx) <= (selectedwidth) && (j - newy) <= (selectedheight) && (j - newy) >= 0)
                        {
                            if (i >= xstartimage && i <= xstartimage + selectedwidth && j >= ystartimage && j <= ystartimage + selectedheight)
                            {
                                if (startarray[i, j] == true)
                                {
                                    outputbool[i, j] = true;
                                }
                                if (startarray[i, j] == true)
                                {
                                    if (selectedarray[i - newx, j - newy] == true)
                                    {
                                        outputbool[i, j] = false;
                                    }
                                    else
                                    {
                                        outputbool[i, j] = true;
                                    }
                                }
                                else
                                {
                                    if (selectedarray[i - newx, j - newy] == true)
                                    {
                                        outputbool[i, j] = true;
                                    }
                                    else
                                    {
                                        outputbool[i, j] = false;
                                    }
                                }
                            }
                            else
                            {
                                if (selectedarray[i - newx, j - newy] == true)
                                {
                                    if (startarray[i, j] == true)
                                    {
                                        outputbool[i, j] = false;
                                    }
                                    else
                                    {
                                        outputbool[i, j] = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return outputbool;
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
