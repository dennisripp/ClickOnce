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
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class CreateAnimationWindow : Window
    {
        static private UIControl uIControl;
        public CreateAnimationWindow()
        {
            InitializeComponent();
        }

        public CreateAnimationWindow(UIControl _uIControl)
        {
            uIControl = _uIControl;
            InitializeComponent();
        }
        private void Button_Next_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int AnzPresets = Int32.Parse(AnzPresetsTxtBox.Text);
                if (AnzPresets < 1) AnzPresets = 1;
                Close();
            }
            catch
            {
                MessageBox.Show("Please fill in");
            }
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
