using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Output
    {
        private bool FrameReadyToSend = false; //flag: UIControl will send the new Frame to the Driver if this flag has been set
        private bool PixelReadyToSend = false;
        private bool FrameReadyToRead = false;

        //private uint col, row; //Coordinations for the changing pixel
        private bool[,] OutputFrame;
        private uint RowCount, ColCount;

        public Output(int _RowCount, int _ColCount)
        {
            RowCount = Convert.ToUInt32(_RowCount);
            ColCount = Convert.ToUInt32(_ColCount);
            OutputFrame = new bool[_ColCount, _RowCount];
        }     

        public void SetOutputPixel(uint _col, uint _row)
        {
            bool[,] frame = new bool[ColCount, RowCount];
            frame[_col, _row] = true;
            OutputFrame = frame;
            FrameReadyToRead = true;
            SetFrameReadyToSend(true);
        }

        public void SetOutputFrame(bool[,] _Frame)
        {
            OutputFrame = _Frame;
            FrameReadyToRead = true;
        }

        public void SetFrameReadyToSend(bool _Ready)
        {
            FrameReadyToSend = _Ready;
        }

        public Output GetOutput()
        {
            return this;
        }

        public uint GetCol()
        {
            return ColCount;
        }

        public uint GetRow()
        {
            return RowCount;
        }

        public bool[,] GetOutputFrame()
        {
            return OutputFrame;
        }

        public bool GetFrameReadyToSend()
        {
            return FrameReadyToSend;
        }

        public bool GetPixelReadyToSend()
        {
            return PixelReadyToSend;
        }

        public void SetPixelReadyToSend(bool _PixelReadyToSend)
        {
            PixelReadyToSend = _PixelReadyToSend;
        }

        public bool GetFrameReadyToRead()
        {
            return FrameReadyToRead;
        }

        public void SetFrameReadyToRead(bool _FrameReadyToRead)
        {
            FrameReadyToRead = _FrameReadyToRead;
        }
    }
}
