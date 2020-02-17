using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Threading;

namespace WpfApp1
{
    class USBInterface
    {
        private SerialPort VirtualPort1;

        public USBInterface()
        {
            VirtualPort1 = new SerialPort();
        }

        public void ConnectDevice(string _ComPort)
        {
            try
            {
                VirtualPort1.PortName = _ComPort;
                VirtualPort1.BaudRate = 115200;
                VirtualPort1.Open();
            }
            catch (System.IO.IOException)
            {
                // No Access available
            }
            catch (UnauthorizedAccessException)
            {
                // Access denied
            }
            catch (ArgumentException)
            {
                // no port selected
            }
            catch (InvalidOperationException)
            {
                // still running
            }
        }

        public string[] GetAvailablePorts()
        {
            string[] ports = SerialPort.GetPortNames();
            return ports;
        }

        public void SendMessage(byte[] dataArray, int count)
        {            
            VirtualPort1.Write(dataArray, 0, count);
        }

        public bool GetConnect()
        {
            return VirtualPort1.IsOpen;
        }

        public List<string> GetConnectedPortsOfMatchingIDs(List<string> _ComportNames)
        {
            string[] _Ports = SerialPort.GetPortNames();
            List<string> ConnectedPortsOfMatchingIDs = new List<string>();
            for(int i = 0; i < _Ports.Length; i++)
            {
                for(int j = 0; j < _ComportNames.Count; j++)
                {
                    if (_Ports[i] == _ComportNames[j])
                    {
                        ConnectedPortsOfMatchingIDs.Add(_ComportNames[j]);
                    }
                }
            }
            return ConnectedPortsOfMatchingIDs;
        }

        public byte[] ReceiveMsg(int _Count)
        {

            Thread.Sleep(_Count);
            byte[] msg = new byte[_Count];
            VirtualPort1.Read(msg, 0, _Count);
            return msg;
        }
    }
}
