using Emgu.CV;
using Emgu.CV.CvEnum;
namespace WpfApp1
{
    public partial class UICam
    {
        /*
         * Creats videocapturedevice(camera) with properties
         */
        public void StartVideo(int _VideoSourceIndex,int _exposure, int _resx, int _resy)
        {
            StopVideo();                                                        // delete existing object
            _capture = new VideoCapture(CaptureType.DShow + _VideoSourceIndex); // Creats cameraobject with backend =directshow, has to be DShow(directshow) otherwise memory leak
            if (_capture.GetCaptureProperty(CapProp.AutoExposure) != 0 && _capture.GetCaptureProperty(CapProp.Exposure) != 0) // if exposure is possible to set
            {
                _capture.SetCaptureProperty(CapProp.Exposure, _exposure);   // set exposurevalue
                _capture.SetCaptureProperty(CapProp.AutoExposure, 0);       // deactivetes autoexposure
            }
            _capture.SetCaptureProperty(CapProp.FrameHeight, _resy);        // sets resolution       
            _capture.SetCaptureProperty(CapProp.FrameWidth, _resx);
            camchange = false;                                              // needed so no picture is captured while camera is changing
        }
        /*
         * Stops running videocapture, frees memory and delete the object
         */
        public void StopVideo()
        {
            if (_capture != null)
            {
                _capture.Dispose();
                _capture = null;
            }
        }
    }
}
