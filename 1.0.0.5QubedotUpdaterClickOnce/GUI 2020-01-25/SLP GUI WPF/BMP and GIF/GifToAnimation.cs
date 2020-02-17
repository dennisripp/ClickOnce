using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;

namespace WpfApp1
{
    class GifToAnimation
    {
        int ColCount;
        int RowCount;
        private List<bool[,]> BoolFramelist = new List<bool[,]>();
        private int[] DelayTimes;
        public GifToAnimation(int _ColCount, int _RowCount, string path)
        {
            ColCount = _ColCount;
            RowCount = _RowCount;            
            OpenFileDialog loadFile = new OpenFileDialog();
            loadFile.InitialDirectory = path;
            loadFile.Filter = "Graphics Interchange Format |*.GIF";
            loadFile.Title = "Load GIF";
            loadFile.ShowDialog();
            if (loadFile.FileName != "")
            {
                Image[] frames = getFrames(Image.FromFile(loadFile.FileName));
                DelayTimes = new int[frames.Length];
                int PropertyTagFrameDelay = 0x5100;                
                for (int i = 0; i < frames.Length; i++)
                {
                    BoolFramelist.Add(BmpToBoolFrame(new Bitmap(frames[i])));
                    if (BoolFramelist.Count > 0)
                    {
                        PropertyItem prop = frames[0].GetPropertyItem(PropertyTagFrameDelay);
                        DelayTimes[i] = (prop.Value[0] + prop.Value[1] * 256) * 10; //in milliseconds
                    }
                }                
            }
        }

        public List<bool[,]> GetBoolFrameList()
        {
            return BoolFramelist;
        }

        public int[] getDelayTimes()
        {
            return DelayTimes;
        }
 
        private Image[] getFrames(Image originalImg)
        {
            int numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);
            Image[] frames = new Image[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                int FrameRowCount = originalImg.Height;
                int FrameColCount = originalImg.Width;
                if((FrameColCount % ColCount == 0)&&(FrameRowCount % RowCount == 0))
                {
                    originalImg.SelectActiveFrame(FrameDimension.Time, i);
                    frames[i] = ((Image)originalImg.Clone());
                }
                
            }
            return frames;
        }

        private bool[,] BmpToBoolFrame(Bitmap _bmp)
        {
            bool[,] BoolFrametmp = new bool[_bmp.Width, _bmp.Height];
            bool[,] BoolFrame = new bool[ColCount, RowCount];
            bool invert = false;
            for (int col = 0; col < _bmp.Width; col++)
            {
                for (int row = 0; row < _bmp.Height; row++)
                {
                    if ((_bmp.GetPixel(col, row).R > 128) && (_bmp.GetPixel(col, row).G > 169) && (_bmp.GetPixel(col, row).B > 169) && (_bmp.GetPixel(col, row).A > 169)) //row=y, col=x
                    {
                        if (invert)
                        {
                            BoolFrametmp[col, row] = true;
                        }
                        else
                        {
                            BoolFrametmp[col, row] = false;
                        }                        
                    }
                    else
                    {
                        if (invert)
                        {
                            BoolFrametmp[col, row] = false;
                        }
                        else
                        {
                            BoolFrametmp[col, row] = true;
                        }
                    }
                }
            }
            int average = 0;
            for (int row = 0; row < RowCount; row++)
            {
                for(int col = 0; col < ColCount; col++)
                {
                    for(int pixelWidth = 0; pixelWidth < _bmp.Width/ColCount; pixelWidth++)
                    {
                        for(int pixelHeight = 0; pixelHeight < _bmp.Height/RowCount; pixelHeight++)
                        {
                            if(BoolFrametmp[col*_bmp.Width/ColCount + pixelWidth, row*_bmp.Height/RowCount + pixelHeight])
                            {
                                average++;
                            }
                        }
                    }
                    if(average > (_bmp.Width / ColCount)*(_bmp.Height / RowCount) * 0.3)
                    {
                        BoolFrame[col, row] = true;
                    }
                    average = 0;
                }
            }            
            return BoolFrame;
        }
    }
}
