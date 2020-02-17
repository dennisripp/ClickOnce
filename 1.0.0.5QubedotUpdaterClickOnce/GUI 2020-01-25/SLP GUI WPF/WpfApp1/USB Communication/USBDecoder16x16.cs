using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class USBDecoder16x16 : USBDecoder
    {
        private int RowCount;
        private int ColCount;
        public USBDecoder16x16()
        {
            RowCount = 16;
            ColCount = 16;
        }

        //Encodes the Frame 16x16
        override public byte[] EncodeFrame(bool[,] _PixelArray){
            bool[,] RotatedPixelArray = new bool[RowCount, ColCount];
            for (int i = 0; i < ColCount; i++){
                int x = 0;
                for (int j = RowCount - 1; j >= 0; j--){
                    RotatedPixelArray[j, i] = _PixelArray[i, x];
                    x++;
                }
            }
            byte[] msg = new byte[RowCount * 2 + 2]; // *2 16x16
            msg[0] = (byte)Modes.Command.RETURN;
            msg[1] = (byte)DSFMode;
            for (int row = 0; row < RowCount; row++){
                byte tmp = new byte();
                byte tmp2 = new byte();
                for (int col = 0; col < ColCount; col++){
                    if (RotatedPixelArray[col, row]){
                        if (col > 7) //16x16
                            tmp2 |= (byte)(1 << (col - 8)); //16x16
                        else
                            tmp |= (byte)(1 << col);
                    }
                }
                msg[row * 2 + 3] = tmp;
                msg[row * 2 + 2] = tmp2; //16x16
            }
            return msg;
        }

        //there is no Current regulation for 16x16
        override public byte[] EncodeSetCurrent(uint _Current){
            return base.EncodeSetCurrent(_Current);
        }

        //there is no voltage regulation for 16x16
        override public byte[] EncodeSetVoltage(uint _Voltage){
            return base.EncodeSetVoltage(_Voltage);
        }

        override public byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int[] _FrameDurations){
            List<byte> msg_list = new List<byte>();
            msg_list.Add((byte)Modes.Command.RETURN);
            msg_list.Add((byte)Modes.Command.LOADFRAMES);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[0]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[1]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[2]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[3]);
            msg_list.Add((byte)Modes.FrameDurationSetting.multiple);
            for(int i = 0; i < _FrameDurations.Length; i++){
                byte[] tmp = BitConverter.GetBytes(_FrameDurations[i]);
                msg_list.Add(tmp[0]);
                msg_list.Add(tmp[1]);
                msg_list.Add(tmp[2]);
                msg_list.Add(tmp[3]);
            }
            for(int Framecount = 0; Framecount < _Frames.Count; Framecount++){
                byte[] tmp = EncodeFrame(_Frames[Framecount]);
                for (int ByteCount = 0; ByteCount < (RowCount * ColCount / 8); ByteCount++)
                    msg_list.Add(tmp[ByteCount]);
            }
            byte[] msg = new byte[msg_list.Count];
            for (int Bytecount = 0; Bytecount < msg_list.Count; Bytecount++)
                msg[Bytecount] = msg_list[Bytecount];
            return msg;
        }

        override public byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int _FrameDuration){
            List<byte> msg_list = new List<byte>();
            msg_list.Add((byte)Modes.Command.RETURN);
            msg_list.Add((byte)Modes.Command.LOADFRAMES);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[0]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[1]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[2]);
            msg_list.Add(BitConverter.GetBytes(_Frames.Count)[3]);
            msg_list.Add((byte)Modes.FrameDurationSetting.single);
            byte[] tmp = BitConverter.GetBytes(_FrameDuration);
            msg_list.Add(tmp[0]);
            msg_list.Add(tmp[1]);
            msg_list.Add(tmp[2]);
            msg_list.Add(tmp[3]);
            for (int Framecount = 0; Framecount < _Frames.Count; Framecount++)
            {
                byte[] tmp2 = EncodeFrame(_Frames[Framecount]);
                for (int ByteCount = 0; ByteCount < (RowCount * ColCount / 8); ByteCount++)
                    msg_list.Add(tmp2[ByteCount+2]);
            }
            byte[] msg = new byte[msg_list.Count];
            for (int Bytecount = 0; Bytecount < msg_list.Count; Bytecount++)
                msg[Bytecount] = msg_list[Bytecount];
            return msg;
        }
    }
}
