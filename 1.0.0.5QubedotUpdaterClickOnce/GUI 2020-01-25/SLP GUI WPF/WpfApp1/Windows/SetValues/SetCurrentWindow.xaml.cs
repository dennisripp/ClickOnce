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

namespace WpfApp1.Windows.SetValues
{
    /// <summary>
    /// Interaktionslogik für SetCurrentWindow.xaml
    /// </summary>
    public partial class SetCurrentWindow : Window
    {

        UIControl uIControl;
        public SetCurrentWindow(UIControl _uIControl)
        {
            InitializeComponent();
            UnitCombobox.SelectedIndex = UnitCombobox.Items.Add("mA");
            UnitCombobox.Items.Add("A");
            uIControl = _uIControl;
        }

        private void SetCurrentButton_Click(object sender, RoutedEventArgs e)
        {
            string s = SetCurrentTextbox.Text;
            double current = Convert.ToDouble(s);
            if (UnitCombobox.SelectedIndex == 0)
            {
                uint currentInt = Convert.ToUInt32(current);
                uIControl.SetCurrent(currentInt);
            }
            else
            {
                uint currentInt = Convert.ToUInt32(current * 1000);
                uIControl.SetCurrent(currentInt);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SetCurrentButton_Click(sender, e);
                Close();
            }
        }
    }
}
