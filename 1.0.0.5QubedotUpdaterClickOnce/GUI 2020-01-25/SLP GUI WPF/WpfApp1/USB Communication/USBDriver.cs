using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class USBDriver
    {
        private USBInterface uSBInterface;
        private USBDecoder uSBDecoder;
        private ControlUSBConnection controlUSBConnection;
        private string VID = "03EB";
        private string PID = "2404";
        private bool ModeChanged = true;
        private Modes.Hardware Hardware;

        public USBDriver()
        {
            uSBInterface = new USBInterface();
            controlUSBConnection = new ControlUSBConnection();
        }

        public void UploadAnimation(List<bool[,]> _Frames, int[] _FrameDurations)
        {
            SendMessage(uSBDecoder.EncodeAnimationUpload(_Frames, _FrameDurations));
        }

        public void UploadAnimation(List<bool[,]> _Frames, int _FrameDuration)
        {
            SendMessage(uSBDecoder.EncodeAnimationUpload(_Frames, _FrameDuration));
        }

        public void StopAnimation()
        {
            SendMessage(uSBDecoder.EncodeAnimationStop());
        }

        public Modes.Hardware HardwareRequest()
        {
            byte[] msg = { (byte)Modes.Command.HARDWAREREQUEST, (byte)Modes.Command.RETURN };
            SendMessage(msg);
            msg = ReceiveMessage(1);
            switch (msg[0]){
                case (byte)Modes.Hardware.Demo8x8:
                    uSBDecoder = new USBDecoder8x8();
                    Hardware = Modes.Hardware.Demo8x8;
                    return Modes.Hardware.Demo8x8;
                case (byte)Modes.Hardware.Demo16x16:
                    uSBDecoder = new USBDecoder16x16();
                    Hardware = Modes.Hardware.Demo16x16;
                    return Modes.Hardware.Demo16x16;
                default:
                    return Modes.Hardware.None;
            }
        }

        public string ConnectAtStart()
        {
            List<string> ComPortList = controlUSBConnection.ComPortNames(VID, PID);
            if (ComPortList.Count == 0){
                return "";
            }
            else{
                List<string> _ActivPortsOfMatchingIDs = new List<string>();
                _ActivPortsOfMatchingIDs = uSBInterface.GetConnectedPortsOfMatchingIDs(ComPortList);
                if (_ActivPortsOfMatchingIDs.Count > 1){
                    return "";
                }
                else{
                    return _ActivPortsOfMatchingIDs[0];
                }
            }
        }

        public byte[] PixelCheck()
        {
            byte[] msg = { (byte)Modes.Command.PixelCurrentRequest, (byte)Modes.Command.RETURN };
            SendMessage(msg);

            byte[] recv_msg = ReceiveMessage(1024);
            return recv_msg;
        }

        public void SendFrame(bool[,] _Frame)
        {
            if (uSBInterface.GetConnect()){
                if(Hardware != Modes.Hardware.None){
                    byte[] msg = uSBDecoder.EncodeFrame(_Frame);
                    SendMessage(msg);
                    byte[] return_msg = new byte[1];
                    return_msg[0] = (byte)Modes.Command.RETURN;
                    SendMessage(return_msg);
                }                
            }
        }

        public void SetVoltage(uint _voltage)
        {
            byte[] msg = uSBDecoder.EncodeSetVoltage(_voltage);
            if(msg != null)
            {
                SendMessage(msg);
                ModeChanged = true;
            }
           
        }

        public void SetCurrent(uint _current)
        {
            byte[] msg = uSBDecoder.EncodeSetCurrent(_current);
            if (msg != null)
            {
                SendMessage(msg);
                ModeChanged = true;
            }
        }

        public bool USBConnected()
        {
            return uSBInterface.GetConnect();
        }

        private void SendMessage(byte[] _msg)
        {
            try{
                uSBInterface.SendMessage(_msg, _msg.Length);
            }
            catch (TimeoutException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
            catch (InvalidOperationException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
            catch (System.IO.IOException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
        }

        private byte[] ReceiveMessage(int _Count)
        {
            byte[] msg = new byte[_Count];
            try{
                msg = uSBInterface.ReceiveMsg(_Count);
            }
            catch (TimeoutException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
            catch (InvalidOperationException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
            catch (System.IO.IOException e){
                controlUSBConnection.NoDeviceConnected(e);
            }
            return msg;
        }

        public string[] GetPorts()
        {
            return uSBInterface.GetAvailablePorts();
        }

        public void ConnectDeviceUSB(string _ComPort)
        {
            uSBInterface.ConnectDevice(_ComPort);
        }

    }
}