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
using System.Threading;
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für NewWindow.xaml
    /// </summary>
    public partial class SetSaveModus : Window
    {
        UIControl uIControl;
        public Boolean continuebool = false, changehappend = false;
        public SetSaveModus(UIControl _uIControl)
        {
            InitializeComponent();
            uIControl = _uIControl;
            //this.Closed += new CancelEventArgs(Closings);
            this.Closing += CloseingWindow;
        }
        public async void wait2sec()
        {
            await Task.Delay(1000);
            ContinueButton.IsEnabled=true;
        }
        private void Cancle_Click(object sender, RoutedEventArgs e)
        {
            continuebool = false;
            changehappend = true;
            Close();
        }

        private void Continue_Click(object sender, RoutedEventArgs e)
        {
            continuebool = true;
            changehappend = true;
            Close();
        }
        public Boolean GetStatus()
        {
            while(changehappend == false)
            {
            }
            return continuebool;
        }
        private void CloseingWindow(object sender, CancelEventArgs e)
        {
            continuebool = false;
            changehappend = true;
        }
    }
}
