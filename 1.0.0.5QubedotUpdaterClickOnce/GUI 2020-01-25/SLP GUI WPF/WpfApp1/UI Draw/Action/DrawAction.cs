using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    class DrawAction
    {
        private SolidColorBrush schwarz = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

        private bool figswitch = false;    // is triggert after first mousedown, needed so the over event isnt triggering before the firstdown
        private bool overswitch = false;   // needed so the event is triggert only one time the mouse is moveing above new rectangle, otherwise it would be triggert on every move
        private bool leaveswitch = false, firstclickselectswitch = false, secondselectswitch = false, selectedswitch = false; // for select, not working anyway
        private int rowcount, coulmncount, xstart, ystart, xstartimage, ystartimage, x, y;
        private int xselectstart, yselectstart, xselectend, yselectend, xdragstart, xdragend, ydragstart, ydragend;
        private double leftstart, topstart, leftstartimage, topstartimage;
        private Output output;
        private Select select;
        private Border myBorder;
        private double pixeldistance;
        private Modes.ToolMode currentfigure;
        private Canvas subCanvas;
        public DrawAction(int _rowcount, int _coulmncount, Canvas _subCanvas, double _pixeldistance)
        {
            rowcount = _rowcount;
            coulmncount = _coulmncount;
            output = new Output(_rowcount, _coulmncount);
            pixeldistance = _pixeldistance;

            select = new Select();
            myBorder = new Border();
            subCanvas = _subCanvas;
        }
        /*
         * happens when mousedown is triggert on rectangle
         */
        public void MouseDownFigure(Rectangle _rect)
        {
            switch (currentfigure)
            {
                case Modes.ToolMode.Normal: 
                    getCoord(_rect);
                    output.SetOutputPixel((uint)x, (uint)y); break; // sets the led x,y  on/off after mouse down
                case Modes.ToolMode.Draw:
                    getCoord(_rect);
                    output.SetOutputPixel((uint)x, (uint)y);        // sets the led x,y  on/off after mouse down  
                    overswitch = false; break; 
                
            }
            if (figswitch == false)
            {
                getCoord(_rect);    //sets current x,y values
                xstart = x;         //start position for the figure, might be changed after
                ystart = y;         //start position for the figure, might be changed after
                xstartimage = x;    //start position that wont be changed 
                ystartimage = y;    //start position that wont be changed 
                figswitch = true;
            }
        }
        /*
        * happens when mouseover is triggert on rectangle
        */
        public void MouseOverFigure(Rectangle _rect)
        {
            // overwitch needed so the event is triggert only one time the mouse is moveing above new rectangle, otherwise it would be triggert on every move
            // figwitch is needed so the event is triggert only after mousedown event
            if (figswitch == true && overswitch == true) 
            {
                getCoord(_rect);
                xstart = xstartimage;
                ystart = ystartimage;

                switch (currentfigure)  
                {
                    case Modes.ToolMode.Line:
                        Line newline = new Line(xstart, x, ystart, y, coulmncount, rowcount);       // creates object of the selected figure
                        output.SetOutputFrame(newline.GetLineArray()); break;                       // returns bool array with the figure, figure is just showend not set now
                    case Modes.ToolMode.Square:
                        Square newsquare = new Square(xstart, x, ystart, y, coulmncount, rowcount); // creates object of the selected figure
                        output.SetOutputFrame(newsquare.GetSquareArray()); break;                   // returns bool array with the figure, figure is just showend not set now
                    case Modes.ToolMode.Circle:
                        Circle newcircle = new Circle(xstart, x, ystart, y, coulmncount, rowcount); // creates object of the selected figure
                        output.SetOutputFrame(newcircle.GetCircleArray()); break;                   // returns bool array with the figure, figure is just showend not set now
                    case Modes.ToolMode.Draw:
                        output.SetOutputPixel((uint)x, (uint)y); break;                             // sets the led x,y  on/off after mouseover 

                }
                overswitch = false; //overswitch=false, so next event can only be mouseup or mouseleave. needed so overwitch will happen only one time at one rectangle
            }
        }
        /*
        * happens when mouseup is triggert on rectangle
        */
        public void MouseUpFigure(Rectangle _rect, bool[,] _Frame)
        {
            if (figswitch == true)
            {
                getCoord(_rect);
                xstart = xstartimage; 
                ystart = ystartimage;
                switch (currentfigure)
                {
                    case Modes.ToolMode.Normal:
                        output.SetFrameReadyToSend(true);
                        break;
                    case Modes.ToolMode.Draw:
                        output.SetFrameReadyToSend(true);
                        break;
                    case Modes.ToolMode.Line:
                        Line newline = new Line(xstart, x, ystart, y, coulmncount, rowcount);
                        output.SetOutputFrame(newline.GetLineArray());          // returns bool array with the figure, figure is just showend not set now (gui shows the figure, leds off)
                        output.SetFrameReadyToSend(true);                       // figure will be set now (leds on)
                        figswitch = false; break;                               // figswitch set to false again so the next figure can be drawn
                    case Modes.ToolMode.Square:
                        Square newsquare = new Square(xstart, x, ystart, y, coulmncount, rowcount);
                        output.SetOutputFrame(newsquare.GetSquareArray());      // returns bool array with the figure, figure is just showend not set now
                        output.SetFrameReadyToSend(true);                       // figure will be set on/off now
                        figswitch = false; break;
                    case Modes.ToolMode.Circle:
                        Circle newcircle = new Circle(xstart, x, ystart, y, coulmncount, rowcount); // 
                        output.SetOutputFrame(newcircle.GetCircleArray());      // returns bool array with the figure, figure is just showend not set now
                        output.SetFrameReadyToSend(true);                       // figure will be set on/off now        
                        figswitch = false; break;
                }
                figswitch = false;
            }
        }
        public void MouseLeave()
        {
            overswitch = true; // if mouse leaves rectangle the next overevent can happen
        }
        /*
         * not working anyway right now
         */
        public void MouseDownSelect(Rectangle _rect, bool[,] _startselectarray)
        {
            leaveswitch = false;
            getCoord(_rect);
            if (firstclickselectswitch == false)
            {
                xselectstart = x;
                yselectstart = y;
                xstartimage = x;
                ystartimage = y;
                leftstart = Canvas.GetLeft(_rect);
                leftstartimage = leftstart;
                topstart = Canvas.GetTop(_rect);
                topstartimage = topstart;
                firstclickselectswitch = true;
            }
            else
            {
                if (secondselectswitch == true)
                {
                    selectedswitch = true;
                    select.SelectArray(xselectstart, xselectend, yselectstart, yselectend, rowcount, coulmncount, _startselectarray);
                    xdragstart = x;
                    ydragstart = y;
                    secondselectswitch = false;
                }
            }
        }
        /*
         * not working anyway right now
         */
        public void MouseUpSelect(Rectangle _rect, bool[,] _startselectarray)
        {
            if (secondselectswitch == false && leaveswitch == false)
            {
                secondselectswitch = true;
            }
            if (selectedswitch == true)
            {
                getCoord(_rect);
                xdragend = x;
                ydragend = y;
                output.SetOutputFrame(select.ShiftSelectedArray(xdragstart, xdragend, ydragstart, ydragend, rowcount, coulmncount, _startselectarray));
                output.SetFrameReadyToSend(true);
                ToolChangedResetSwitches();
                RemoveBorder();
            }
        }
        /*
         * not working anyway right now
         */
        public void MouseOverSelect(Rectangle _rect, bool[,] _startselectarray)
        {
            if (overswitch == true)
            {
                if (selectedswitch == true)
                {
                    getCoord(_rect);
                    xdragend = x;
                    ydragend = y;
                    output.SetOutputFrame(select.ShiftSelectedArray(xdragstart, xdragend, ydragstart, ydragend, rowcount, coulmncount, _startselectarray));
                    overswitch = false;
                }
                else
                {
                    if (firstclickselectswitch == true && secondselectswitch == false)
                    {
                        getCoord(_rect);
                        xstart = xstartimage;
                        ystart = ystartimage;

                        xselectend = x;
                        yselectend = y;

                        leftstart = leftstartimage;
                        topstart = topstartimage;

                        swapcoordifbigger(_rect);
                        myBorder.Height = ((_rect.Height + pixeldistance) * (Math.Abs(y - ystart) + 1)) - pixeldistance;
                        myBorder.Width = ((_rect.Width + pixeldistance) * (Math.Abs(x - xstart) + 1)) - pixeldistance;
                        myBorder.BorderThickness = new Thickness(0.5);
                        myBorder.BorderBrush = Brushes.Black;
                        subCanvas.Children.Remove(myBorder);
                        subCanvas.Children.Add(myBorder);
                        Canvas.SetLeft(myBorder, leftstart);
                        Canvas.SetTop(myBorder, topstart);
                        Canvas.SetZIndex(myBorder, 2);
                        overswitch = true;
                    }
                }
            }
        }

        public Output GetOutput()
        {
            return output.GetOutput();
        }
        /*
         * sets the position of the rectangle
         */
        public void getCoord(Rectangle _rect)
        {
            x = (int)((Canvas.GetLeft(_rect)) / (_rect.Width + pixeldistance));
            y = (int)((Canvas.GetTop(_rect)) / (_rect.Height + pixeldistance));
        }
        /*
         * if x or y of the first click are smaller then after 2nd click they will be swaped, so the figure calculation still works
         */
        public void swapcoordifbigger(Rectangle _rect)
        {
            if (xstart > x)
            {
                int buffer = xstart;
                xstart = x;
                x = buffer;
                leftstart = Canvas.GetLeft(_rect);
            }
            if (ystart > y)
            {
                int buffer = ystart;
                ystart = y;
                y = buffer;
                topstart = Canvas.GetTop(_rect);
            }
        }

        public void SetCurrentFigure(Modes.ToolMode _NextDrawTool)
        {
            currentfigure = _NextDrawTool;
        }
        
        public void ToolChangedResetSwitches()
        {
            figswitch = false;
            overswitch = false;
            firstclickselectswitch = false;
            secondselectswitch = false;
            selectedswitch = false;
            leaveswitch = true;

        }
        public void RemoveBorder()
        {
            subCanvas.Children.Remove(myBorder);
        }
        public void Tilt90degree(bool[,] _currentframe)
        {
            bool[,] ret = new bool[_currentframe.GetLength(0), _currentframe.GetLength(1)];
            int n = _currentframe.GetLength(0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = _currentframe[n - j - 1, i];
                }
            }
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (_currentframe[i, j] == true)
                    {
                        if (ret[i, j] == false) ret[i, j] = true;
                        else ret[i, j] = false;
                    }
                }
            }
            output.SetOutputFrame(ret);
            output.SetFrameReadyToSend(true);
        }
        public void FrameMirrorV(bool[,] _currentframe)
        {
            bool[,] ret = new bool[_currentframe.GetLength(0), _currentframe.GetLength(1)];
            int n = _currentframe.GetLength(0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = _currentframe[n - i - 1, j];
                }
            }
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (_currentframe[i, j] == true)
                    {
                        if (ret[i, j] == false) ret[i, j] = true;
                        else ret[i, j] = false;
                    }
                }
            }
            output.SetOutputFrame(ret);
            output.SetFrameReadyToSend(true);
        }
        public void FrameMirrorH(bool[,] _currentframe)
        {
            bool[,] ret = new bool[_currentframe.GetLength(0), _currentframe.GetLength(1)];
            int n = _currentframe.GetLength(0);
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    ret[i, j] = _currentframe[i, n - 1- j];
                }
            }
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    if (_currentframe[i, j] == true)
                    {
                        if (ret[i, j] == false) ret[i, j] = true;
                        else ret[i, j] = false;
                    }
                }
            }
            output.SetOutputFrame(ret);
            output.SetFrameReadyToSend(true);
        }
        public void ToolChanged()
        {
            figswitch = false;
            overswitch = false;
            firstclickselectswitch = false;
            secondselectswitch = false;
            selectedswitch = false;
            leaveswitch = true;

        }

    }

}

