using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows;

namespace WpfApp1
{
    partial class UIControl
    {
        private List<bool[,]> SaveFrames;
        private int currentCount = 1, currentCountMax = 1;
        private int Timer = 100;
        private int[] AnimationDelayTimes;
        public void InitCreateAnimation(int _maxCount, int _Timer, bool _edit)
        {
            SaveFrames = new List<bool[,]>();
            currentCountMax = _maxCount;
            currentCount = 1;
            AnimationDelayTimes = new int[currentCountMax + 1];
            mwd.CreateAnimationElements.Visibility = Visibility.Visible;
            if (_edit == true)
            {
                SaveFrames = BoolFrames;
                currentCountMax = BoolFrames.Count;
                currentCount = BoolFrames.Count;
                AnimationDelayTimes = DelayTimes;
                LoadPixelArray(SaveFrames[currentCount - 1]);
            }
            mwd.SequenceLabel.Content = "Frame " + currentCount.ToString() + " of " + currentCountMax.ToString();
            ChangeStatusMenu1(false);
        }
        public void SaveCreateAnimation()
        {
            AnimationDelayTimes[currentCount - 1] = Timer;
            currentCount++;
            mwd.SequenceLabel.Content = "Frame " + currentCount.ToString() + " of " + currentCountMax.ToString();
            bool[,] _CurrentFrame = uIPixelArrayGrid.GetPixelStatus();
            if (currentCount - 2 < SaveFrames.Count) SaveFrames.RemoveAt(currentCount - 2);
            SaveFrames.Insert(currentCount - 2, _CurrentFrame);
            if (currentCount-1 < SaveFrames.Count) LoadPixelArray(SaveFrames[currentCount-1]);
            if (currentCount > currentCountMax)
            {
                CancelCreateAnimation();
                mwd.AnimationElements.Visibility = Visibility.Visible;
                StartStreamAnimation(SaveFrames, AnimationDelayTimes);
            }
        }
        public void CancelCreateAnimation()
        {
            ChangeStatusMenu1(true);
            mwd.CreateAnimationElements.Visibility = Visibility.Hidden;
            currentCount = 1;
        }
        private void ChangeStatusMenu1(bool _status)
        {
            mwd.Menu11.IsEnabled = _status;
            mwd.Menu12.IsEnabled = _status;
            mwd.Menu13.IsEnabled = _status;
            mwd.Menu14.IsEnabled = _status;
        }
        public void ChangeMaxFrameCount(int _newframecount)
        {
            while (SaveFrames.Count >= _newframecount)
            {
                SaveFrames.RemoveAt(SaveFrames.Count - 1);
                currentCount = SaveFrames.Count +1;
            }
            currentCountMax = _newframecount;
            mwd.SequenceLabel.Content = "Frame " + currentCount.ToString() + " of " + currentCountMax.ToString();
            int[] AnimationDelaybuffer = AnimationDelayTimes;
            AnimationDelayTimes = new int[currentCountMax + 1];
            for (int i = 0; i < AnimationDelayTimes.Count(); i++)
            {
                if (i < AnimationDelaybuffer.Count() - 1) AnimationDelayTimes[i] = AnimationDelaybuffer[i];
            }
        }
        public void ChangeTimer(int _Timer)
        {
            Timer = _Timer;
        }
        public void CreateAnimationLastFrame()
        {
            if (currentCount - 1 >= 1)
            {
                currentCount = currentCount - 1;
                mwd.SequenceLabel.Content = "Frame " + currentCount.ToString() + " of " + currentCountMax.ToString();
                if (currentCount - 1 >= 0) LoadPixelArray(SaveFrames[currentCount - 1]);
                else SetPixels(false);
            }
        }
    }
}
