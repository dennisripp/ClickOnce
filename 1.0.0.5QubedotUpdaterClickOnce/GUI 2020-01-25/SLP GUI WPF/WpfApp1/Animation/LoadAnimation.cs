using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class LoadAnimation : Animation
    {
        public LoadAnimation(List<bool[,]> _Frames, int[] _FrameDurations, USBDriver _uSBDriver) : base(_Frames, _FrameDurations, _uSBDriver) { }
        public LoadAnimation(List<bool[,]> _Frames, int _FrameDuration, USBDriver _uSBDriver) : base(_Frames, _FrameDuration, _uSBDriver) { }

        override public void Start(){
            if(setting == Modes.FrameDurationSetting.multiple)
                uSBDriver.UploadAnimation(Frames, FrameDurations);
            else
                uSBDriver.UploadAnimation(Frames, FrameDuration);
        }
        public override void Stop(){
            // Thread Stoppen
        }
    }
}