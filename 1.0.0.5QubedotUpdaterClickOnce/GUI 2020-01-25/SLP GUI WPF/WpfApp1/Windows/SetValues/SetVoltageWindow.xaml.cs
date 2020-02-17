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
    /// Interaktionslogik für SetVoltageWindow.xaml
    /// </summary>
    /// 

    public partial class SetVoltageWindow : Window
    {

        private UIControl uIControl;

        public SetVoltageWindow(UIControl _uIControl)
        {
            InitializeComponent();            
            UnitCombobox.SelectedIndex = UnitCombobox.Items.Add("mV");
            UnitCombobox.Items.Add("V");
            uIControl = _uIControl;
        }

        private async void SetVoltageButton_Click(object sender, RoutedEventArgs e)
        {
            string s = SetVoltageTextbox.Text;
            double voltage = Convert.ToDouble(s);
            uint voltageInt;
            Boolean warningbool = false;
            if (UnitCombobox.SelectedIndex == 0) voltageInt = Convert.ToUInt32(voltage);
            else voltageInt = Convert.ToUInt32(voltage * 1000);
            if(voltageInt>uIControl.Voltagelimit && uIControl.safemodus == false)
            {
                SetVoltageWindowWarning setVoltagewindowwarning = new SetVoltageWindowWarning(uIControl);
                setVoltagewindowwarning.Show();
                setVoltagewindowwarning.wait2sec();
                Task<Boolean> Taskwait = Task<Boolean>.Factory.StartNew(() => setVoltagewindowwarning.GetStatus());
                warningbool = await Taskwait;
            }
            else
            {
                warningbool = true;
            }
            if (warningbool == true)
            {
                uIControl.SetVoltage(voltageInt);
            }
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnKeyDownHandler(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetVoltageButton_Click(sender, e);
                Close();
            }
        }
    }
}
