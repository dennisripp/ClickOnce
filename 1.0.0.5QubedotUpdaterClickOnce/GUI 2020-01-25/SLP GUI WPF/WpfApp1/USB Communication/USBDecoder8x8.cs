using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class USBDecoder8x8 : USBDecoder
    {
        private int RowCount;
        private int ColCount;
        public USBDecoder8x8()
        {
            RowCount = 8;
            ColCount = 8;
        }

        //Encodes the Frame 8x8
        public override byte[] EncodeFrame(bool[,] _PixelArray)
        {
            byte[] msg = new byte[RowCount + 2];
            msg[0] = (byte)Modes.Command.RETURN;
            msg[1] = (byte)DSFMode;
            for (int row = 0; row < RowCount; row++)
            {
                byte tmp = new byte();
                for (int col = 0; col < ColCount; col++)
                {
                    if (_PixelArray[col, row])
                    {
                        tmp |= (byte)(1 << col);
                    }
                }
                msg[RowCount - row + 1] = tmp;
            }
            return msg;
        }

        public override byte[] EncodeSetVoltage(uint _Voltage)
        {
            byte[] msg = { (byte)Modes.Command.RETURN, (byte)Modes.Command.SETVOLTAGEMAX, (byte)(_Voltage >> 8), (byte)_Voltage };
            return msg;
        }

        public override byte[] EncodeSetCurrent(uint _Current)
        {
            byte[] msg = { (byte)Modes.Command.RETURN, (byte)Modes.Command.SETCURRENTMAX, (byte)(_Current >> 8), (byte)_Current };
            return msg;
        }
        override public byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int[] _FrameDurations)
        {
            return null; //NOCH ANPASSEN
        }

        override public byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int _FrameDuration)
        {
            return null; //NOCH ANPASSEN
        }
    }
}
