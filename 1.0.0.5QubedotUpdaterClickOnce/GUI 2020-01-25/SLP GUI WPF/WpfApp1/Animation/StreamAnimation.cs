using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace WpfApp1
{
    class StreamAnimation : Animation
    {
        private int FrameCounter;
        private UIControl uIControl;
        public StreamAnimation(List<bool[,]> _Frames, int[] _FrameDurations, USBDriver _uSBDriver, UIControl _uiControl) : base(_Frames, _FrameDurations, _uSBDriver) {
            uIControl = _uiControl;
        }
        public StreamAnimation(List<bool[,]> _Frames, int _FrameDuration, USBDriver _uSBDriver, UIControl _uiControl) : base(_Frames, _FrameDuration, _uSBDriver) {
            uIControl = _uiControl;
        }
        private Thread thr;
        override public void Start(){
            thr = new Thread(Routine);
            thr.Start();
        }
        public override void Stop(){
            if(thr.IsAlive == true) thr.Abort();
        }
        private void SetFrameCounter(){
            FrameCounter = (FrameCounter + 1) % Frames.Count;
        }
        private void Routine()
        {
            while(true)
            {
                uIControl.SendAnimationStream(Frames[FrameCounter]);
                SetFrameCounter();
                if (FrameDurations.Length == 0)Thread.Sleep(FrameDuration);
                else
                {
                    if (FrameDurations[FrameCounter] < 10) Thread.Sleep(10);
                    else Thread.Sleep(FrameDurations[FrameCounter]);
                }
            }
        }
    }
}