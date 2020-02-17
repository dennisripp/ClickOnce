using System;
using System.Drawing;
using System.Windows;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace WpfApp1
{
    public partial class UICam : IDisposable
    {
        private VideoCapture _capture = null;
        private double[] rgb;
        private bool camchange = true, squareforgraycalcchange = false, mirrorH=false, mirrorV=false;
        private int SelectedIndex, ExposureVal, resx, resy;
        private int topleftpixelx = 0, buttomrightpixelx = 0, topleftpixely = 0, buttomrightpixely = 0, linethickness = 3;
        private int buffertopleftpixelx = 0, bufferbuttomrightpixelx = 0, buffertopleftpixely = 0, bufferbuttomrightpixely = 0, bufferlinethickness = 3;
        public UICam()
        {
            rgb = new double[3];
        }
        /*
         * Get single frame from camera as bitmapsource and returns it
         */
        public BitmapSource TakePic()
        {
            if (_capture != null && camchange != true && squareforgraycalcchange != true)
            {
                Mat fram = new Mat();       // Videocapture returns frame in "Mat" format
                _capture.Read(fram);        // Get frame
                if (fram != null)
                {
                    Bitmap bitmap = fram.Bitmap;    // Get bitmap from mat
                    if (bitmap != null)
                    {
                        if (mirrorH == true) bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);  // Flip if check checkBox is pressed
                        if (mirrorV == true) bitmap.RotateFlip(RotateFlipType.Rotate180FlipY);
                        BitmapSource ret = CreateBitmapSourceFromBitmap(DrawSquare(bitmap));    // Get Bitmapsource from bitmap, if reduced area is used also draw the square
                        ret.Freeze();                                                           // Freeze needed otherwise thread probles
                        Task TaskGetGray = Task.Factory.StartNew(() => CalcAverageRGB(ret));          // Get async gray calculation, has to be async otherwise gui freezes
                        TaskGetGray.Wait();                                                     // Wait until gray calculation is done
                        fram.Dispose();                                                         // Free used memory
                        return ret;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if(camchange == true) StartVideo(SelectedIndex, ExposureVal, resx, resy);   //Change camera or camera properties, needed so cam is changed after picture is captured
                if (squareforgraycalcchange == true) changesquarevalues();                  // if squarevalue changed, change squarevalue in this class after picture is captured
                return null;
            }
        }
        /*
         * Transfers the camera index and properties 
         */
        public void CamChange(int _SelectedIndex, int _ExposureVal, int _resx, int _resy) 
        {
            SelectedIndex = _SelectedIndex;
            ExposureVal = _ExposureVal;
            resx = _resx;
            resy = _resy;
            camchange = true;
        }
        public double[] getrgb()
        {
            return rgb;
        }
        /*
         * Transfers the flip/mirror bools 
         */
        public void SetMirrorbools(bool _mirrorH, bool _mirrorV)
        {
            mirrorH = _mirrorH;
            mirrorV = _mirrorV;
        }
        /*
         * Transfers the squarevalues for the reduced area function 
         */
        private void changesquarevalues()
        {
            topleftpixelx = buffertopleftpixelx;
            buttomrightpixelx = bufferbuttomrightpixelx;
            topleftpixely = buffertopleftpixely;
            buttomrightpixely = bufferbuttomrightpixely;
            linethickness = bufferlinethickness;
            squareforgraycalcchange = false;
        }
        /*
         * Sets the squarevalues after the camera isnt capturing an image
         */
        public void setsquarevalues(int _topleftpixelx, int _buttomrightpixelx, int _topleftpixely, int _buttomrightpixely, int _linethickness)
        {
            buffertopleftpixelx = _topleftpixelx;
            bufferbuttomrightpixelx = _buttomrightpixelx;
            buffertopleftpixely = _topleftpixely;
            bufferbuttomrightpixely = _buttomrightpixely;
            bufferlinethickness = _linethickness;
            squareforgraycalcchange = true;
        }
        /*
         * Gets the avrage RGB value of the whole image
         */
        private void CalcAverageRGB(BitmapSource _bitsource)
        {
            int[,,] array = new int[_bitsource.PixelWidth, _bitsource.PixelHeight, 4];
            int stride = _bitsource.PixelWidth * ((_bitsource.Format.BitsPerPixel + 7) / 8);
            byte[] bytearray = new byte[_bitsource.PixelHeight * stride];
            _bitsource.CopyPixels(bytearray, stride, 0);    // get pixel from Bitmapsource
            double r = 0, g = 0, b = 0, a = 0;
            int starti = 0, startj = 0, endi = _bitsource.PixelWidth, endj = _bitsource.PixelHeight, count = (_bitsource.PixelWidth * _bitsource.PixelHeight);
            if (topleftpixelx<buttomrightpixelx && topleftpixely<buttomrightpixely) // Sets start end end values for the loop, needed for the reduced area function
            {
                if (topleftpixelx >= 0 && topleftpixely >= 0)
                {
                    int pixelwidth = _bitsource.PixelWidth;
                    int pixelhight = _bitsource.PixelHeight;
                    if (topleftpixelx < pixelwidth && topleftpixely < pixelhight && buttomrightpixelx < pixelwidth && buttomrightpixely < pixelhight)
                    {
                        starti = topleftpixelx + 1;
                        startj = topleftpixely + 1;
                        endi = buttomrightpixelx - 1;
                        endj = buttomrightpixely - 1;
                        count = ((endj - startj) * (endi - starti));
                    }
                }
            }
            for (int i = starti; i < endi; i++)
            {
                for (int j = startj; j < endj; j++)
                {
                    for (int n = 0; n < 4; n++)
                    {
                        array[i, j, n] = bytearray[(i + (_bitsource.PixelWidth * j)) * 4 + n]; //Get rgb from i,j pixel
                    }
                    //sum up every channel
                    b = b + array[i, j, 0];
                    g = g + array[i, j, 1];
                    r = r + array[i, j, 2];
                    a = a + array[i, j, 3];
                }
            }
            //calc gray value
            rgb[0] = r / count;
            rgb[1] = g / count;
            rgb[2] = b / count;
            _bitsource.Freeze(); //needed otherwise thread problems
        }
        /*
         * Get bitmapsource from bitmap, bitmapsource needed for plotting 
         */
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);
        private BitmapSource CreateBitmapSourceFromBitmap(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap); // important, otherwise memory overload
            }
        }
        /*
         * Draw the reduced area square on the bitmap
         */
        private Bitmap DrawSquare(Bitmap _src)
        {
            if (topleftpixelx < buttomrightpixelx && topleftpixely < buttomrightpixely)
            {
                int pixelwidth = _src.Width;
                int pixelhight = _src.Height;
                for (int d = 0; d < linethickness; d++)
                {
                    if (topleftpixelx < pixelwidth && topleftpixely < pixelhight && buttomrightpixelx < pixelwidth && buttomrightpixely < pixelhight)
                    {
                        if (topleftpixelx >= 0)
                        {
                            for (int i = topleftpixelx; i <= buttomrightpixelx; i++)
                            {
                                if (topleftpixely - d >= 0) _src.SetPixel(i, topleftpixely - d, Color.Red);
                                if (buttomrightpixely + d < pixelhight) _src.SetPixel(i, buttomrightpixely + d, Color.Red);
                            }
                        }
                        if (topleftpixely >= 0)
                        {
                            for (int i = topleftpixely; i <= buttomrightpixely; i++)
                            {
                                if (topleftpixelx - d >= 0) _src.SetPixel(topleftpixelx - d, i, Color.Red);
                                if (buttomrightpixelx + d < pixelwidth) _src.SetPixel(buttomrightpixelx + d, i, Color.Red);
                            }
                        }
                    }
                }
            }
            return _src;
        }
        /*
         *  Dispose stuff to free the memory of the object
         */
        bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                StopVideo();
                rgb = null;
                //
            }

            disposed = true;
        }
    }
}
