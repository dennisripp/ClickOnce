using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Square : Figure
    {
        public Square(int _xstart, int _xend, int _ystart, int _yend, int _rowcount, int _colcount) : base(_xstart, _xend, _ystart, _yend, _rowcount, _colcount)
        {
        }
        /*
         * returns boolaray, all true values of the array will give the circle
         */
        public bool[,] GetSquareArray()
        {
            bool[,] output;
            output = new bool[rowcount, colcount];
            swapcoordifbigger();
            for (int i = xstart; i <= xend; i++)
            {
                for (int j = ystart; j <= yend; j++)
                {
                    output[i, j] = true;
                }
            }
            return output;
        }
    }
    class Line : Figure
    {
        public Line(int _xstart, int _xend, int _ystart, int _yend, int _rowcount, int _colcount) : base(_xstart, _xend, _ystart, _yend, _rowcount, _colcount)
        {
        }
        /*
         * returns boolaray, all true values of the array will give the line
         */
        public bool[,] GetLineArray()
        {
            bool[,] output;
            output = new bool[rowcount, colcount];

            int dx = Math.Abs(xend - xstart), sx = xstart < xend ? 1 : -1;
            int dy = -Math.Abs(yend - ystart), sy = ystart < yend ? 1 : -1;
            int err = dx + dy, e2;

            while (true)
            {
                output[xstart, ystart] = true;
                if (xstart == xend && ystart == yend) break;
                e2 = 2 * err;
                if (e2 > dy) { err += dy; xstart += sx; }
                if (e2 < dx) { err += dx; ystart += sy; }
            }
            return output;
        }
    }
    /*
    * returns boolaray, all true values of the array will give the circle
    */
    class Circle : Figure
    {
        bool[,] output;
        public Circle(int _xstart, int _xend, int _ystart, int _yend, int _rowcount, int _colcount) : base(_xstart, _xend, _ystart, _yend, _rowcount, _colcount)
        {
        }
        public bool[,] GetCircleArray()
        {
            output = new bool[rowcount, colcount];
            int radius = (int)(Math.Abs(Math.Sqrt((xend - xstart) * (xend - xstart) + (yend - ystart) * (yend - ystart))));


            if (radius <= 2)
            {
                int x1, y1;
                for (int j = 0; j <= radius; j++)
                {
                    for (int i = 0; i < 360; i += 1)
                    {
                        x1 = (int)(j * Math.Cos(i * 3.1415926535 / 180));
                        y1 = (int)(j * Math.Sin(i * 3.1415926535 / 180));
                        changeoutput(xstart + x1, ystart + y1);
                    }
                }
            }
            else
            {
                radius = radius + 1;
                int x0 = xstart, y0 = ystart;
                int y = 0;
                int dx = 1;
                int dy = 1;
                int x = radius - 1;
                int err = dx - (radius << 1);
                while (x >= y)
                {
                    for (int i = -x; i <= x; i++)
                    {
                        changeoutput(x0 + i, y0 + y);
                        changeoutput(x0 + i, y0 - y);
                    }
                    for (int i = -y; i <= y; i++)
                    {
                        changeoutput(x0 + i, y0 + x);
                        changeoutput(x0 - i, y0 - x);
                    }
                    if (err <= 0)
                    {
                        y++;
                        err += dy;
                        dy += 2;
                    }
                    if (err > 0)
                    {
                        x--;
                        dx += 2;
                        err += dx - (radius << 1);
                    }
                }
            }
            return output;
        }
        public void changeoutput(int _x, int _y)
        {
            if (_x >= 0 && _y >= 0 && _y < rowcount && _x < colcount)
            {
                output[_x, _y] = true;
            }
        }

    }

}
