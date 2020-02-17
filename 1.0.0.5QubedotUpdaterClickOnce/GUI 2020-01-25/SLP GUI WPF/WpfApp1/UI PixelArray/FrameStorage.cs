using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class FrameStorage
    {
        private List<Frame> FrameList = new List<Frame>();
        private Frame CurrentFrame;

        public FrameStorage(bool[,] _Frame)
        {
            Frame _NewFrame = new Frame(_Frame);
            FrameList.Add(_NewFrame);
            CurrentFrame = _NewFrame;
        }

        public void AddFrame(bool[,] _Frame)
        {
            Frame _NewFrame = new Frame(_Frame);
            if (FrameList.Count >= 100)
            {
                FrameList.RemoveAt(0);
            }
            if (CurrentFrame == FrameList.Last())
            {
                FrameList.Add(_NewFrame);                
            }
            else
            {
                int index = FrameList.FindIndex(_i => _i == CurrentFrame);
                FrameList.Insert(index + 1, _NewFrame);
                for(int i = index + 2; i < FrameList.Count; i++)
                {
                    FrameList.RemoveAt(i);
                }                
            }
            CurrentFrame = _NewFrame;
        }

        public bool[,] GetPreviousFrameAndSetAsCurrent()
        {
            int index = FrameList.FindIndex(_i => _i == CurrentFrame);
            if(index > 0)
            {
                CurrentFrame = FrameList[index - 1];
            }
            return CurrentFrame.GetFrame();
        }

        public bool[,] GetPreviousFrame()
        {
            int index = FrameList.FindIndex(_i => _i == CurrentFrame);
            if (index > 0)
            {
                return FrameList[index - 1].GetFrame();
            } else
            {
                return CurrentFrame.GetFrame();
            }            
        }

        public bool[,] GetNextFrameAndSetAsCurrent()
        {
            int index = FrameList.FindIndex(_i => _i == CurrentFrame);
            if (index < FrameList.Count - 1)
            {
                CurrentFrame = FrameList[index + 1];
            }
            return CurrentFrame.GetFrame();
        }
        public bool[,] GetCurrentFrame()
        {
            return CurrentFrame.GetFrame();
        }
    }
}
