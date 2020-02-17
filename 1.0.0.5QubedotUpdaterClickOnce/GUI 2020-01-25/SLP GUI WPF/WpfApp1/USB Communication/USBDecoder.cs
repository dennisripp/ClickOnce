using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    abstract class USBDecoder
    {
        protected Modes.Command DSFMode = Modes.Command.DIRECTSETFRAME;

        public virtual byte[] EncodeSetVoltage(uint _Voltage){ return null; }
        public virtual byte[] EncodeSetCurrent(uint _Current) { return null; }
        public virtual byte[] EncodeFrame(bool[,] _PixelArray) { return null; }
        public virtual byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int[] _FrameDurations) { return null; }
        public virtual byte[] EncodeAnimationUpload(List<bool[,]> _Frames, int _FrameDuration) { return null; }
        public byte[] EncodeAnimationStop() {
            byte[] msg = new byte[2];
            msg[0] = (byte)Modes.Command.STOP;
            msg[1] = (byte)Modes.Command.RETURN;
            return msg;
        }
    }
}