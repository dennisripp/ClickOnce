using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class Modes
    {
        public enum Command : uint
        {
            DIRECTSETPIXEL = 0,
            DIRECTSETFRAME = 1,
            SETVOLTAGEMAX = 2,
            SETCURRENTMAX = 3,
            ANIMATION = 4,
            RETURN = 5,
            LOADFRAMES = 6,
            SELECTANIMATION = 7,
            STOP = 8,
            STAY = 9,
            DIRECTCLEARPIXEL = 10,
            HARDWAREREQUEST = 11,
            PixelCurrentRequest = 12
        }
        public enum FrameDurationSetting : uint
        {
            single,
            multiple
        }


        public enum DrawOutput : uint
        {
            Pixel,
            Frame
        };

        public enum ToolMode : uint
        {
            Normal = 0,
            Draw = 1,
            Line = 2,
            Square = 3,
            Circle = 4,
            Select
        };
        public enum PrintMode : uint
        {
            Invert,
            Set,
            Delete
        };

        public enum Action : uint
        {
            Undo,
            Reundo,
            Tilt,
            MirrorH,
            MirrorV,
            None
        };

		public enum Hardware : uint
		{
			Demo8x8 = 0xFE,
			Demo16x16 = 0xFF,
			None = 0x00
		}
    }
}
