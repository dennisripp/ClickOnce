using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing;
using System.Drawing.Imaging;

namespace WpfApp1
{
    class AnimationToGIF
    {
        private int RowCount, ColCount;
        private byte[] GifAnimation = { 33, 255, 11, 78, 69, 84, 83, 67, 65, 80, 69, 50, 46, 48, 3, 1, 0, 0, 0 };
        private byte[] Delay = { 255, 0 };
        private MemoryStream MS;
        private BinaryReader BR;
        private BinaryWriter BW;
        public AnimationToGIF(int _RowCount, int _ColCount)
        {
            RowCount = _RowCount;
            ColCount = _ColCount;            
        }

        public GifBitmapEncoder ConvertAnimationToGIF(SteuerungSequenz steuersq)
        {
            int _FrameCountMax = steuersq.GetPresetCountMax();
            FrameToBmp _frameToBMP = new FrameToBmp(ColCount, RowCount);
            Bitmap[] _BMPs = new Bitmap[_FrameCountMax];
            bool[,,] _Frames = steuersq.GetFrames();
            GifBitmapEncoder _Encoder = new GifBitmapEncoder();
            for (int _FrameCount = 0; _FrameCount < _FrameCountMax; _FrameCount++)
            {
                bool[,] _Frame = GetFrame(_Frames, _FrameCount);
                _BMPs[_FrameCount] = _frameToBMP.ConvertFrameToBMP(_Frame);
                BitmapSource _BMPSource = ConvertBMPToBMPSource(_BMPs[_FrameCount]);
                _Encoder.Frames.Add(BitmapFrame.Create(_BMPSource));
            }
            InitGif((System.Drawing.Image[])_BMPs);
            return _Encoder;
        }

        public void SaveAnimationAsGif(SteuerungSequenz _steuersq, string _pathname)
        {
            string name = _pathname + "new.gif";
            FileStream _fileStream = new FileStream(name, FileMode.Create);
            GifBitmapEncoder _Encoder = ConvertAnimationToGIF(_steuersq);
            _Encoder.Save(_fileStream);
        }

        private BitmapSource ConvertBMPToBMPSource(System.Drawing.Bitmap bitmap)
        {
            var bitmapData = bitmap.LockBits(
                new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height,
                bitmap.HorizontalResolution, bitmap.VerticalResolution,
                PixelFormats.Bgr24, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        private bool[,] GetFrame(bool[,,] _Frames, int _FrameCount)
        {
            bool[,] _RFrame = new bool[ColCount, RowCount];
            for(int _col = 0; _col < ColCount; _col++)
            {
                for (int _row = 0; _row < RowCount; _row++)
                {
                    _RFrame[_col, _row] = _Frames[_col, _row, _FrameCount];
                }
            }
            return _RFrame;
        }
        
        public void InitGif(System.Drawing.Image[] Files)
        {
            string GifFile = "C:\\t\\MultiFrame.gif";
            //string[] Files = Directory.GetFiles(JpegFolder, "*.jpg");
            MS = new MemoryStream();
            BR = new BinaryReader(MS);
            BW = new BinaryWriter(new FileStream(GifFile, FileMode.Create));

            Files[0].Save(MS, ImageFormat.Gif);
            byte[] B = MS.ToArray();
            B[10] = (byte)(B[10] & 0X78); //No global color table.
            BW.Write(B, 0, 13);
            BW.Write(GifAnimation);
            WriteGifImg(B, BW);
            for (int I = 1; I < Files.Length; I++)
            {
                MS.SetLength(0);
                Files[I].Save(MS, ImageFormat.Gif);
                B = MS.ToArray();
                WriteGifImg(B, BW);
            }
            BW.Write(B[B.Length - 1]);
            BW.Close();
            MS.Dispose();
        }
        public void WriteGifImg(byte[] B, BinaryWriter BW)
        {
            B[785] = Delay[0]; //5 secs delay
            B[786] = Delay[1];
            B[798] = (byte)(B[798] | 0X87);
            BW.Write(B, 781, 18);
            BW.Write(B, 13, 768);
            BW.Write(B, 799, B.Length - 800);
        }

        //public bool[,,] GetFramesFromGIF(string _pathname)
        //{

        //}
    }
}
