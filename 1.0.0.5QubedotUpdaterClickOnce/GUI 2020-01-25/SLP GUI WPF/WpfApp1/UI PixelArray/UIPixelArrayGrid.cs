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
    class UIPixelArrayGrid
    {
        static readonly private SolidColorBrush blue = new SolidColorBrush(Color.FromArgb(255, 0, 203, 255));
        static readonly private SolidColorBrush grey = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
        static readonly private SolidColorBrush green = new SolidColorBrush(Color.FromArgb(255, 46, 255, 0));
        static readonly private SolidColorBrush red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        private static int rowcount, coulmncount;
        private double pixeldistance;
        private UIPixel[,] pixelarray;
        private MainWindow mwd;

        public UIPixelArrayGrid(int _rowcount, int _coulmncount, double _pixeldistance, MainWindow _mwd)
        {
            rowcount = _rowcount;
            coulmncount = _coulmncount;
            pixelarray = new UIPixel[coulmncount, rowcount];
            UIPixelInit();
            pixeldistance = _pixeldistance;
            mwd = _mwd;
        }
        /*
         * creates the UIPixelarray
         */
        public void UIPixelInit()
        {   
            for (int i = 0; i < coulmncount; i++)
            {
                for (int j = 0; j < rowcount; j++)
                {
                    Rectangle r = new Rectangle();
                    r.Fill = grey;
                    pixelarray[i, j] = new UIPixel(r, false);
                }
            }
        }
        /*
         * draws the panel the rectangle pixels are shown on
         */
        public void DrawPannelPixels(Canvas _subCanvas, int panelsizex, int panelsizey)
        {
            // Creat subpanel for the pixelarray
            Canvas subCanvas = _subCanvas;
            subCanvas.Height = panelsizey;
            subCanvas.Width = panelsizex;
            subCanvas.Background = Brushes.White;
            Canvas.SetLeft(subCanvas, 120);
            Canvas.SetTop(subCanvas, 50);

            Border myBorder = new Border();
            myBorder.Height = panelsizey + 4;
            myBorder.Width = panelsizex + 4;
            myBorder.BorderThickness = new Thickness(0.3);
            myBorder.BorderBrush = Brushes.Black;
            subCanvas.Children.Add(myBorder);

            // Creat rectangle array
            for (int i = 0; i < coulmncount; i++)
            {
                for (int j = 0; j < rowcount; j++)
                {
                    Rectangle _r = pixelarray[i, j].GetRect();
                    _r.Width = (((subCanvas.Width) / coulmncount) - pixeldistance);
                    _r.Height = (((subCanvas.Height) / rowcount) - pixeldistance);
                    subCanvas.Children.Add(_r);
                    Canvas.SetLeft(_r, (i * (subCanvas.Width / coulmncount)) + 3);
                    Canvas.SetTop(_r, (j * (subCanvas.Height / rowcount)) + 3);
                    Canvas.SetZIndex(_r, 1);
                }
            }
        }

        //Nur temporär.. Andere Lösung finden!
        public UIPixel[,] GetUIPixels()
        {
            return pixelarray;
        }
        /*
         * returns pixelstatus of x,y pixel
         */

        public bool GetPixelStatus(uint _col, uint _row)
        {
            return pixelarray[_col, _row].GetStatus();
        }
        /*
         * returns every status as array
         */
        public bool[,] GetPixelStatus()
        {
            bool[,] _pixelArrayStatus = new bool[rowcount, coulmncount];
            for (int _row = 0; _row < coulmncount; _row++)
            {
                for (int _col = 0; _col < rowcount; _col++)
                {
                    _pixelArrayStatus[_col, _row] = pixelarray[_col, _row].GetStatus();
                }
            }
            return _pixelArrayStatus;
        }

        public bool GetPixelTmpStatus(uint _col, uint _row)
        {
            return pixelarray[_col, _row].GetTmpStatus();
        }
        public void SetPixelColor(uint _col, uint _row, SolidColorBrush _color)
        {
            pixelarray[_col, _row].SetColor(_color, mwd);
        }
        public void TogglePixelStatus(uint _col, uint _row)
        {
            bool NewStatus = !pixelarray[_col, _row].GetStatus();
            pixelarray[_col, _row].SetStatus(NewStatus);
        }
        public void TogglePixelTmpStatus(uint _col, uint _row)
        {
            bool NewStatus = !pixelarray[_col, _row].GetTmpStatus();
            pixelarray[_col, _row].SetTmpStatus(NewStatus);
        }
        public void SetPixelTmpStatus(uint _col, uint _row, bool on)
        {
            pixelarray[_col, _row].SetTmpStatus(on);
        }
        public void SetPixelStatus(uint _col, uint _row, bool on)
        {
            pixelarray[_col, _row].SetStatus(on);
        }
        /*
         * needed for the measuresequence function, enables or disables the rectangle ( cant be clicked anymore)
         */
        public void ChangeActivateArray(bool _status)
        {
            for (int i = 0; i < coulmncount; i++)
            {
                for (int j = 0; j < rowcount; j++)
                {
                    pixelarray[i, j].GetRect().IsEnabled = _status;
                }
            }
        }
    }
}
