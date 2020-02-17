using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für ConnectDevice.xaml
    /// </summary>
    public partial class ConnectDevice : Window
    {
        private UIControl uIControl;
        public ConnectDevice()
        {
            InitializeComponent();            
        }

        public ConnectDevice(UIControl _uIControl)
        {
            InitializeComponent();
            uIControl = _uIControl;
            comboBox1.Items.Clear();
            GetPorts();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uIControl.ConnectDeviceUSB(comboBox1.Text);
            if (uIControl.USBConnected())
            {
                Close();
                uIControl.RefreshUIElementsOnConnected();
            }
            else
            {
                MessageBox.Show("Connection failed");
            }
        }


        private void GetPorts()
        {
            string[] available_ports = uIControl.GetPortsUSB();
            for (int i = 0; i < available_ports.Length; i++)
            {
                comboBox1.Items.Add(available_ports[i]);
                comboBox1.SelectedIndex = 0;
            }
        }
    }
}
