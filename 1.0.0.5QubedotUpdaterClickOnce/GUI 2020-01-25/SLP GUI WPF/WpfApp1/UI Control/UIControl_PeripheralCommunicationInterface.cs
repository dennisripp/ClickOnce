using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    partial class UIControl
    {

        public bool TryConnectingAtStart()
        {
            try{
                string _ComPortName = uSBDriver.ConnectAtStart();
                if (_ComPortName != ""){
                    ConnectDeviceUSB(_ComPortName);
                    if (USBConnected()){
                        return true;
                    }
                    else{
                        return false;
                    }
                }
                else{
                    return false;
                }
            }
            catch{
                return false;
            }            
        }


        public string[] GetPortsUSB()
        {
            return uSBDriver.GetPorts();
        }

        public void RefreshUIElementsOnConnected()
        {
            if (USBConnected()){
                DeviceConnected();
            }
            else{
                NoDeviceConnected();
            }
        }

        public void ConnectDeviceUSB(string _ComPort)
        {
            uSBDriver.ConnectDeviceUSB(_ComPort);
        }

        public bool USBConnected()
        {
            return uSBDriver.USBConnected();
        }

		public void DeviceConnected()
		{
			//SetPixels(false);
			switch (Hardware){
				case Modes.Hardware.Demo8x8:
					mwd.DeviceStatusLabel.Content = "Device Connected: SLP 8x8 LED Array";
					break;
				case Modes.Hardware.Demo16x16:
					mwd.DeviceStatusLabel.Content = "Device Connected: SLP 16x16 LED Array";
					break;
			}
            mwd.ClearButton.IsEnabled = true;
            mwd.SelectAllButton.IsEnabled = true;
            SetEnableAllPixel(true);
        }

        public Int32[,] PixelCheck()
        {
            byte[] msg = uSBDriver.PixelCheck();
            Int32[] currents = new Int32[msg.Length / 4];
            int x = 0;
            for (int i = 0; i < msg.Length; i += 4){
                currents[x] = (Int32)(msg[i] << 24 | msg[i+1] << 16 | msg[i + 2] << 8 | msg[i + 3]);
                x++;
            }
            Int32[,] ret = new Int32[ColCount, RowCount];
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    ret[i,j] = currents[j * RowCount + i];
                }
            }
            return ret;            
        }

        public void NoDeviceConnected()
        {
            try{
                mwd.ClearButton.IsEnabled = false;
                mwd.SelectAllButton.IsEnabled = false;
                SetEnableAllPixel(false);
                //mwd.MessageBoxShow("Device disconnected");
            }
            catch (NullReferenceException) { }
        }

        public void SetVoltage(uint _voltage)
        {
            if (USBConnected()){
                uSBDriver.SetVoltage(_voltage);
            }
            else{
                NoDeviceConnected();
            }
        }

        public void SetCurrent(uint _current)
        {
            if (USBConnected()){
                uSBDriver.SetCurrent(_current);
            } else{
                NoDeviceConnected();
            }
        }

        public void TryToConnectDevice()
        {
            if (TryConnectingAtStart()){
				connected = true;
				Hardware = uSBDriver.HardwareRequest();
				switch (Hardware){
					case Modes.Hardware.Demo8x8:
						RowCount = 8;
						ColCount = 8;
						break;
					case Modes.Hardware.Demo16x16:
						RowCount = 16;
						ColCount = 16;
						break;
				}
				DeviceConnected();
			}
            else{
                mwd.DeviceStatusLabel.Content = "I Could not find any devices";
            }
        }
    }
}
