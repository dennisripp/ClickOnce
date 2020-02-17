using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Imaging;
using ImageMagick;
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
        private USBDecoder16x16 Decoder16x16;
        private USBDecoder8x8 Decoder8x8;
        public AnimationToGIF(int _RowCount, int _ColCount)
        {
            RowCount = _RowCount;
            ColCount = _ColCount;
            Decoder16x16 = new USBDecoder16x16();
            Decoder8x8 = new USBDecoder8x8();
        }

        public void SaveAnimationAsGif(string _pathforgif, string _pathforhex, List<bool[,]> _frames, int[] _Delays, int _Delay)
        {
            int _FrameCountMax = _frames.Count();
            FrameToBmp _frameToBMP = new FrameToBmp(ColCount, RowCount);
            List<Bitmap> BitmapList = new List<Bitmap>();
            GifBitmapEncoder _Encoder = new GifBitmapEncoder();
            for (int i = 0; i < _FrameCountMax; i++)
            {
                BitmapList.Add(_frameToBMP.ConvertFrameToBMP(_frames[i]));
            }
            BitmapListToGif(BitmapList, _pathforgif, _Delays, _Delay);
            BitmapListtoHex(BitmapList, _pathforhex);
        }
        private void  BitmapListToGif(List<Bitmap> _BitmapList, string _path, int[] _Delays, int _delay)
        {
            using (MagickImageCollection collection = new MagickImageCollection())
            {

                // Add first image and set the animation delay to 100ms
                for (int i = 0; i < _BitmapList.Count; i++)
                {
                    for (int n = 0; n < _BitmapList[i].Width; n++)
                    {
                        for (int m = 0; m < _BitmapList[i].Height; m++)
                        {
                            if (_BitmapList[i].GetPixel(n, m).A == 0) _BitmapList[i].SetPixel(n, m, System.Drawing.Color.White);
                        }
                    }
                    collection.Add(new MagickImage(_BitmapList[i]));
                    if(_Delays != null)
                    {
                        if(_Delays.Length !=0)
                        {
                            collection[i].AnimationDelay = _Delays[i] / 10;
                        }
                        else collection[i].AnimationDelay = _delay / 10;
                    }
                    else collection[i].AnimationDelay = _delay / 10;

                }

                // Optionally reduce colors
                QuantizeSettings settings = new QuantizeSettings();
                settings.Colors = 255;
                collection.Quantize(settings);

                // Optionally optimize the images (images should have the same size).
                collection.OptimizePlus();

                // Save gif
                collection.Write(_path);// "C:/Users/student/Desktop/SLP/GUI/2019.07.17/SLP GUI WPF/GIFs/ThankYou.gif");
            }
        }
        private void BitmapListtoHex(List<Bitmap> _BitmapList, string _path)
        {
            string tofile = "";
            System.IO.File.WriteAllText(_path + "\\hex.txt", tofile);
            //hier alles für zu hex
            for (int i = 0; i < _BitmapList.Count; i++)
            {
                bool[,] currentbool = new bool[_BitmapList[i].Width, _BitmapList[i].Height];
                for (int y = 0; y < _BitmapList[i].Height; y++)
                {
                    for (int x = 0; x < _BitmapList[i].Width; x++)
                    {
                        if ((_BitmapList[i].GetPixel(x, y).R + _BitmapList[i].GetPixel(x, y).G + _BitmapList[i].GetPixel(x, y).B) / 3 < 200) currentbool[x, y] = true;
                    }
                }
                string hex ="";
                byte[] msg = new byte[(ColCount * RowCount +16)/8];
                if(RowCount == 16) msg = Decoder16x16.EncodeFrame(currentbool);
                if (RowCount == 8) msg = Decoder8x8.EncodeFrame(currentbool);
                byte[] msg2 = new byte[(ColCount * RowCount)/8];
                for (int s = 0; s < msg2.Count(); s++)
                {
                    msg2[s] = msg[s + 2];
                }
                hex = BitConverter.ToString(msg2);
                if (i == _BitmapList.Count - 1) tofile = tofile + hex;
                else tofile = tofile + hex + "\n";
            }
            System.IO.File.WriteAllText(_path + "\\hex.txt", tofile);
        }
    }
}
