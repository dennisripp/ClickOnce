using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    abstract class Animation
    {
        protected List<bool[,]> Frames = new List<bool[,]>();
        protected int[] FrameDurations;
        protected int FrameDuration;
        protected USBDriver uSBDriver;
        protected Modes.FrameDurationSetting setting;
        private List<bool[,]> currentboolframes;
        public virtual void Start() { }
        public virtual void Stop() { }

        public Animation(List<bool[,]> _Frames, int[] _FrameDurations, USBDriver _uSBDriver) {
            FrameDurations = new int[_FrameDurations.Length];
            uSBDriver = _uSBDriver;
            for (int i = 0; i < _Frames.Count; i++) {
                Frames.Add(_Frames[i]);
            }
            for (int i = 0; i < _FrameDurations.Length; i++) {
                FrameDurations[i] = _FrameDurations[i];
            }
            FrameDuration = 0;
            setting = Modes.FrameDurationSetting.multiple;
            currentboolframes = _Frames;
        }

        public Animation(List<bool[,]> _Frames, int _FrameDuration, USBDriver _uSBDriver)
        {
            uSBDriver = _uSBDriver;
            for (int i = 0; i < _Frames.Count; i++) {
                Frames.Add(_Frames[i]);
            }
            FrameDuration = _FrameDuration;
            FrameDurations = new int[0];
            setting = Modes.FrameDurationSetting.single;
            currentboolframes = _Frames;
        }
        public void ChangedValues(int _FrameDuration, int[] _FrameDurations, List<bool[,]> _Frames)
        {
            if (_FrameDuration != 0)
            {
                FrameDuration = _FrameDuration;
                for (int i = 0; i < FrameDurations.Length; i++)
                {
                    FrameDurations[i] = _FrameDuration;
                }
            }
            if (_FrameDurations != null) FrameDurations = _FrameDurations;
            Frames = _Frames;
        }
    }
}