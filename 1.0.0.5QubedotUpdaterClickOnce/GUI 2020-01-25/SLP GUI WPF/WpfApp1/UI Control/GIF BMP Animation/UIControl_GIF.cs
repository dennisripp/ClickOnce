using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
namespace WpfApp1
{

    partial class UIControl
    {
        private List<bool[,]> BoolFrames;
        private int Delay = 100;
        private int[] DelayTimes;
        private StreamAnimation streamAnimation;
        private LoadAnimation loadAnimation;

        public async void ImportAnimationFromGIF()
        {
            string s = binpath.Substring(0, binpath.Length - 18) + @"\GIFs\"; 
            GifToAnimation gifToAnimation;
            Task<GifToAnimation> TaskTakePic = Task<GifToAnimation>.Factory.StartNew(() => createGifToAnimation(s) );
            gifToAnimation = await TaskTakePic;
            mwd.AnimationElements.Visibility = Visibility.Visible;
            BoolFrames = gifToAnimation.GetBoolFrameList();
            DelayTimes = gifToAnimation.getDelayTimes();
            if (streamAnimation != null) streamAnimation.Stop();
            if(DelayTimes != null)
            {
                if (DelayTimes.Length == 0)
                {
                    streamAnimation = new StreamAnimation(BoolFrames, Delay, uSBDriver, this);
                }
                else
                {
                    mwd.Frametimebox.Text = DelayTimes[0].ToString();
                    streamAnimation = new StreamAnimation(BoolFrames, DelayTimes, uSBDriver, this);
                }
                streamAnimation.Start();
                ChangeGifMenuStatus(false);
            }
            else
            {
                mwd.AnimationElements.Visibility = Visibility.Hidden;
                ChangeGifMenuStatus(true);
            }
        }
        private void ChangeGifMenuStatus(bool _status)
        {
            mwd.Menu14.IsEnabled = _status;
            mwd.Menu15.IsEnabled = false;
            mwd.Menu17.IsEnabled = false;
            mwd.Menu18.IsEnabled = false;
            if (_status == false)
            {
                mwd.Menu15.IsEnabled = true;
                mwd.Menu17.IsEnabled = true;
                mwd.Menu18.IsEnabled = true;
            }
            
        }
        private void StartStreamAnimation(List<bool[,]> _Boolframes, int[] _Delaytimes)
        {
            DelayTimes = _Delaytimes;
            BoolFrames = _Boolframes;
            streamAnimation = new StreamAnimation(_Boolframes, _Delaytimes, uSBDriver, this);
            streamAnimation.Start();
            ChangeGifMenuStatus(false);
            StartStopUserDrawInputs(false);
        }
        public void StopAnimation()
        {
            if (streamAnimation != null) streamAnimation.Stop();
            if (loadAnimation != null) loadAnimation.Stop();
            ChangeGifMenuStatus(true);
            StartStopUserDrawInputs(true);
            mwd.AnimationElements.Visibility = Visibility.Hidden;
            
        }
        public async void AddGifToAnimation()
        {
            string s = binpath.Substring(0, binpath.Length - 18) + @"\GIFs\";
            GifToAnimation gifToAnimation;
            Task<GifToAnimation> TaskTakePic = Task<GifToAnimation>.Factory.StartNew(() => createGifToAnimation(s));
            gifToAnimation = await TaskTakePic;
            mwd.AnimationElements.Visibility = Visibility.Visible;
            List<bool[,]> AddBoolFrames = gifToAnimation.GetBoolFrameList();
            int[] AddDelayTimes = gifToAnimation.getDelayTimes();
            if (AddDelayTimes != null)
            {
                int[] buffer = DelayTimes;
                DelayTimes = new int[buffer.Length + AddDelayTimes.Length];
                if (AddDelayTimes.Length == 0)
                {
                    for(int i = 0; i < buffer.Length + AddDelayTimes.Length; i++)
                    {
                        if (i < buffer.Length)DelayTimes[i] = Delay;
                        else DelayTimes[i] = Delay;
                    }
                }
                else
                {
                    for (int i = 0; i < buffer.Length + AddDelayTimes.Length; i++)
                    {
                        if (i < buffer.Length) DelayTimes[i] = buffer[i];
                        else DelayTimes[i] = AddDelayTimes[i - buffer.Length];
                    }
                }
                BoolFrames.AddRange(AddBoolFrames);
                streamAnimation.ChangedValues(0, DelayTimes, BoolFrames);
            }
        }
        private GifToAnimation createGifToAnimation(String _s)
        {
            return new GifToAnimation(ColCount, RowCount, _s);
        }
        public void StartLoadAnimation()
        {
            if (streamAnimation != null) streamAnimation.Stop();
            StartStopUserDrawInputs(false);
            loadAnimation = new LoadAnimation(BoolFrames, Delay, uSBDriver);
            loadAnimation.Start();
        }
        public void ChangedValues(int _FrameDuration)
        {
            Delay = _FrameDuration;
            for (int i = 0; i < DelayTimes.Length; i++)
            {
                DelayTimes[i] = Delay;
            }
            streamAnimation.ChangedValues(Delay, null, BoolFrames);
        }
        public void SetInvertGif(Boolean _status)
        {
            for (int c = 0; c < BoolFrames.Count; c++)
            {
                for (int i = 0; i < BoolFrames[c].GetLength(0); i++)
                {
                    for (int j = 0; j < BoolFrames[c].GetLength(1); j++)
                    {
                        if (BoolFrames[c][i, j] == false) BoolFrames[c][i, j] = true;
                        else BoolFrames[c][i, j] = false;
                    }
                }
            }
            streamAnimation.ChangedValues(Delay, null, BoolFrames);
        }
        public void SetMirrorHGif()
        {
            for (int c = 0; c < BoolFrames.Count; c++)
            {
                bool[,] buffer = new bool[BoolFrames[c].GetLength(0), BoolFrames[c].GetLength(1)];
                int n = BoolFrames[c].GetLength(0);
                for (int i = 0; i < BoolFrames[c].GetLength(0); i++)
                {
                    for (int j = 0; j < BoolFrames[c].GetLength(1); j++)
                    {
                        buffer[i, j] = BoolFrames[c][i, n - 1 - j];
                    }
                }
                BoolFrames[c] = buffer;
            }
            streamAnimation.ChangedValues(Delay, null, BoolFrames);
        }
        public void SetMirrorVGif()
        {
            for (int c = 0; c < BoolFrames.Count; c++)
            {
                bool[,] buffer = new bool[BoolFrames[c].GetLength(0), BoolFrames[c].GetLength(1)];
                int n = BoolFrames[c].GetLength(0);
                for (int i = 0; i < BoolFrames[c].GetLength(0); i++)
                {
                    for (int j = 0; j < BoolFrames[c].GetLength(1); j++)
                    {
                        buffer[i, j] = BoolFrames[c][n - i - 1, j];
                    }
                }
                BoolFrames[c] = buffer;
            }
            streamAnimation.ChangedValues(Delay, null, BoolFrames);
        }
        public void TiltGif90()
        {
            for (int c = 0; c < BoolFrames.Count; c++)
            {
                bool[,] buffer = new bool[BoolFrames[c].GetLength(0), BoolFrames[c].GetLength(1)];
                int n = BoolFrames[c].GetLength(0);
                for (int i = 0; i < BoolFrames[c].GetLength(0); i++)
                {
                    for (int j = 0; j < BoolFrames[c].GetLength(1); j++)
                    {
                        buffer[i, j] = BoolFrames[c][n - j - 1, i];
                    }
                }
                BoolFrames[c] = buffer;
            }
            streamAnimation.ChangedValues(Delay, null, BoolFrames);
        }
        public void SendAnimationStream(bool[,] _Frame)
        {
            //communicationState.SendDSFMessage(_Frame);
            Action action = () =>
            {
                LoadPixelArray(_Frame);
            };
            Application.Current.Dispatcher.BeginInvoke(action);
        }
        public async void ExportToGif()
        {
            string filename = "default.gif";
            lastpath = binpath.Substring(0, binpath.Length - 18) + @"\GIFs\";
            System.Windows.Forms.SaveFileDialog saveFileDialog1;
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            saveFileDialog1.InitialDirectory = lastpath;
            saveFileDialog1.FileName = "filename";
            saveFileDialog1.DefaultExt = "gif";
            saveFileDialog1.Filter = "gif (*.gif)|*.gif";
            saveFileDialog1.AddExtension = true;
            System.Windows.Forms.DialogResult dr = saveFileDialog1.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;
                lastpath = System.IO.Path.GetDirectoryName(filename);
                AnimationToGIF animationToGIF = new AnimationToGIF(RowCount, ColCount);
                Task TaskTakeSave = Task.Factory.StartNew(() => animationToGIF.SaveAnimationAsGif(filename, lastpath, BoolFrames, DelayTimes, Delay));
                await TaskTakeSave;
                //animationToGIF.SaveAnimationAsGif(filename, lastpath, BoolFrames, DelayTimes, Delay);
            }

        }
    }
}
