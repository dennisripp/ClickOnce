using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Win32;
using System.IO;

namespace WpfApp1
{
    class FrameToBmp
    {
        private int ColCount, RowCount;

        public FrameToBmp(int _ColCount, int _RowCount)
        {
            ColCount = _ColCount;
            RowCount = _RowCount;            
        }

        public void saveSettingAsBmp(bool[,] _Frame, string _pathname)
        {
            Bitmap bmp1 = new Bitmap(ColCount, RowCount);
            bmp1 = ConvertFrameToBMP(_Frame);
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = _pathname;
            saveFile.Filter = "Bitmap Image|*.bmp";
            saveFile.Title = "Save an Image File";
            saveFile.ShowDialog();
            if (saveFile.FileName != "")
            {
                System.IO.FileStream fs = (System.IO.FileStream)saveFile.OpenFile();
                bmp1.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
            }
        }

        public Bitmap ConvertFrameToBMP(bool[,] _Frame)
        {
            Bitmap bmp1 = new Bitmap(ColCount, RowCount);
            for (int row = 0; row < RowCount; row++)
            {
                for (int col = 0; col < ColCount; col++)
                {
                    if (_Frame[col, row]) //row=y, col=x
                    {
                        bmp1.SetPixel(col, row, Color.FromArgb(255, 0, 203, 255)); //blue
                    }
                    else
                    {
                        bmp1.SetPixel(col, row, Color.FromArgb(255, 221, 221, 221)); //grey
                    }
                }
            }
            return bmp1;
        }

        public bool[,] getSetting(string _pathname)
        {            
            bool[,] LED_ON = new bool[ColCount,RowCount];
            OpenFileDialog loadFile = new OpenFileDialog();
            loadFile.InitialDirectory = _pathname;
            loadFile.Filter = "Bitmap Image|*.bmp";
            loadFile.Title = "Load an Image File";
            loadFile.ShowDialog();

            if(loadFile.FileName != "")
            {
                Bitmap bmp1;
                System.IO.FileStream fs = (System.IO.FileStream)loadFile.OpenFile();
                bmp1 = new Bitmap(fs);
                for (int col = 0; col < ColCount; col++)
                {
                    for (int row = 0; row < RowCount; row++)
                    {
                        if (bmp1.GetPixel(col, row) == Color.FromArgb(255, 0, 203, 255)) //row=y, col=x
                        {
                            LED_ON[col, row] = true;
                        }
                        else
                        {
                            LED_ON[col, row] = false;
                        }
                    }
                }
            }            
            return LED_ON;
        }
    }
}
