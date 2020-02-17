using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Windows.SetValues;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Configuration.Assemblies;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Policy;
using System.Deployment.Application;
using System.ComponentModel;


// Test2



namespace WpfApp1
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        

        UIControl uIControl;
        ConnectDevice connectDevice;
        
        public MainWindow()
        {
            InitializeComponent();
            
            createuIControl();
            

            /*uIControl = new UIControl(this);
            if (uIControl.connected)    // raus wenn arbeiten ohne demonstrator
            {
                uIControl.ShowUI(MyGrid);
                uIControl.SetCurrentArray(uIControl.PixelCheck());
                uIControl.SetPixels(false);
                MeasureFieldCanvas.Visibility = Visibility.Hidden;
            }
            else
            {
                ChangeElementsEnable(false);
            }*/

            this.Width = 500;
        }
        private void createuIControl()
        {
            uIControl = null;
            uIControl = new UIControl(this);
            if (uIControl.connected)    // raus wenn arbeiten ohne demonstrator
            {
                uIControl.ShowUI(MyGrid);
                uIControl.SetCurrentArray(uIControl.PixelCheck());
                uIControl.SetPixels(false);
                MeasureFieldCanvas.Visibility = Visibility.Hidden;
                ChangeElementsEnable(true);
            }
            else
            {
                ChangeElementsEnable(false);
            }
        }
        private void createuIControlDraw(int _ColCount, int _RowCount)
        {
            uIControl = null;
            uIControl = new UIControl(this);
            uIControl.setRowColCount(_RowCount, _ColCount);
            uIControl.ShowUI(MyGrid);
            uIControl.SetPixels(false);
            uIControl.Drawmode = true;
            MeasureFieldCanvas.Visibility = Visibility.Hidden;
            ChangeElementsEnableDrawmode(true);
        }

        private void ChangeElementsEnable(bool _status)
        {
            Menu11.IsEnabled = _status;
            Menu12.IsEnabled = _status;
            Menu13.IsEnabled = _status;
            Menu22.IsEnabled = _status;
            Menu23.IsEnabled = _status;
            Menu14.IsEnabled = _status;
            if (_status == true) Menu16.IsEnabled = false;
        }
        private void ChangeElementsEnableDrawmode(bool _status)
        {
            Menu11.IsEnabled = _status;
            Menu12.IsEnabled = _status;
            Menu13.IsEnabled = _status;
            Menu14.IsEnabled = _status;
            if (_status == true)
            {
                Menu16.IsEnabled = false;
            }
        }
        private void MainWindowCLosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode) uIControl.Close();
        }
        //Clear Array
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode) uIControl.SetPixels(false);
        }
        //Connect Device
        private void Menu_Click_Connect(object sender, RoutedEventArgs e)
        {
            connectDevice = new ConnectDevice(uIControl);
            //connectDevice.Show();
            createuIControl();
        }

        //Store Frame to BMP
        private void MenuItem_Click_ExportBMP(object sender, RoutedEventArgs e)
        {
            uIControl.saveBMP();
        }

        //Load Frame from BMP Class
        private void MenuItem_Click_ImportBMP(object sender, RoutedEventArgs e)
        {
            uIControl.importBMP();
        }

        private void MenuItem_Click_Voltage(object sender, RoutedEventArgs e)
        {
            //set voltage
            if (uIControl.connected || uIControl.Drawmode)
            {
                SetVoltageWindow setVoltageWindow = new SetVoltageWindow(uIControl);
                setVoltageWindow.Show();
            }
        }

        private void MenuItem_Click_Current(object sender, RoutedEventArgs e)
        {
            //set current
            SetCurrentWindow setCurrentWindow = new SetCurrentWindow(uIControl);
            setCurrentWindow.Show();
        }
        public bool opend = false;
        private void MenuItem_Click_measurestart(object sender, RoutedEventArgs e)
        {
            if (opend == false)
            {
                opend = true;
                uIControl.DrawField();
                Menustart.IsEnabled = false;
                Menustop.IsEnabled = true;
            }
        }
        private void MenuItem_Click_measurestop(object sender, RoutedEventArgs e)
        {
            if (opend == true)
            {
                opend = false;
                uIControl.DeleteField();
                Menustart.IsEnabled = true;
                Menustop.IsEnabled = false;
            }
        }



        //Select All
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode) uIControl.SetPixels(true);
        }

        public void MessageBoxShow(string s)
        {
            System.Windows.MessageBox.Show(s);
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                switch (e.Key)
                {
                    case Key.LeftCtrl:
                        uIControl.Ctrl_Up();
                        break;
                    case Key.Z:
                        uIControl.Z_Up();
                        break;
                    case Key.LeftShift:
                        uIControl.Shift_Up();
                        break;
                }
            }
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                switch (e.Key)
                {
                    case Key.LeftCtrl:
                        uIControl.Ctrl_Down();
                        break;
                    case Key.Z:
                        uIControl.Z_Down();
                        break;
                    case Key.LeftShift:
                        uIControl.Shift_Down();
                        break;
                }
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode) uIControl.MouseDownMainWindow(e);
        }


        private void CancelSequencerButton_Click(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                AnimationElements.Visibility = Visibility.Hidden;
                uIControl.CancelCreateAnimation();
            }
        }
        private async void MenuItem_SaveClick(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected)
            {
                Boolean warningbool = false;
                SetVoltageWindowWarning setVoltagewindowwarning = new SetVoltageWindowWarning(uIControl);
                setVoltagewindowwarning.Show();
                setVoltagewindowwarning.wait2sec();
                Task<Boolean> Taskwait = Task<Boolean>.Factory.StartNew(() => setVoltagewindowwarning.GetStatus());
                warningbool = await Taskwait;
                if (warningbool == true)
                {
                    uIControl.safemodus = true;
                    uIControl.safemoduschanged();

                }
                else
                {
                    uIControl.safemodus = false;
                    uIControl.safemoduschanged();
                }
            }
        }

        private void MenuItem_Click_ImportGif(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                ResetGifParamters();
                uIControl.ImportAnimationFromGIF();
                DrawModeElements.Visibility = Visibility.Hidden;
            }
        }

        private void Frametime_TextChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int time = 0;
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(Frametimebox.Text, out time))
                {
                    if (time <= 10)
                    {
                        time = 10;
                        Frametimebox.Text = "10";
                    }
                    uIControl.ChangedValues(time);
                }
                else
                {
                    time = 10;
                    Frametimebox.Text = "10";
                }
            }
        }

        private void invertradioCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)  uIControl.SetInvertGif((Boolean)invertradioCheckbox.IsChecked);
        }

        private void MirrorHradioCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)  uIControl.SetMirrorHGif();
        }

        private void MirrorVradioCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)  uIControl.SetMirrorVGif();
        }

        private void Button_90tilt_Click(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)  uIControl.TiltGif90();
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected)
            {
                AnimationLoadButton.IsEnabled = false;
                AnimationStreamButton.IsEnabled = true;
                uIControl.StartLoadAnimation();
            }
        }
        private void StreamButton_Click(object sender, RoutedEventArgs e)
        {
            AnimationLoadButton.IsEnabled = true;
            AnimationStreamButton.IsEnabled = false;
            uIControl.StartLoadAnimation();
        }
        private void ResetGifParamters()
        {
            mirrorHradioCheckbox.IsChecked = false;
            mirrorVradioCheckbox.IsChecked = false;
            invertradioCheckbox.IsChecked = false;
        }

        private void MenuItem_Click_CreateGif(object sender, RoutedEventArgs e)
        {
            DrawModeElements.Visibility = Visibility.Hidden;
            uIControl.InitCreateAnimation(int.Parse(FrameCountTextBox.Text), int.Parse(CreateAnimationTimer.Text), false);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode) uIControl.SaveCreateAnimation();
        }
        private void CreateAnimationTimerChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int createanimationTimer;
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(CreateAnimationTimer.Text, out createanimationTimer))
                {
                    if (createanimationTimer < 10)
                    {
                        createanimationTimer = 10;
                        CreateAnimationTimer.Text = "10";
                    }
                    uIControl.ChangeTimer(createanimationTimer);
                }
                else
                {
                    createanimationTimer = 10;
                    CreateAnimationTimer.Text = "10";
                    uIControl.ChangeTimer(createanimationTimer);
                }
            }
        }
        private void PixelCountDrawmodeChanged(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int Col, RowCount;
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(PixelCounttextbox.Text, out Col))
                {
                    if (Col < 1)
                    {
                        Col = 1;
                        PixelCounttextbox.Text = "1";
                    }
                }
                else
                {
                    Col = 1;
                    PixelCounttextbox.Text = "1";
                }
                createuIControlDraw(Col, Col);
            }
        }
        
        private void StopStreamButton_Click(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                uIControl.StopAnimation();
                AnimationElements.Visibility = Visibility.Hidden;
                Menu14.IsEnabled = true;
            }
        }

        private void MenuItem_Click_ExportGif(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)  uIControl.ExportToGif();
        }

        private void MenuItem_Click_Drawonly(object sender, RoutedEventArgs e)
        {
            createuIControlDraw(16,16);
            DrawModeElements.Visibility = Visibility.Visible;
        }

        private void MenuItem_Click_AddGiftoAnimation(object sender, RoutedEventArgs e)
        {
            if (uIControl.connected || uIControl.Drawmode)
            {
                uIControl.AddGifToAnimation();
            }
        }

        private void MenuItem_Click_EditAnimation(object sender, RoutedEventArgs e)
        {
            uIControl.StopAnimation();
            uIControl.InitCreateAnimation(0, 100, true);
        }


        private void LastFraneButton_Click(object sender, RoutedEventArgs e)
        {
            uIControl.CreateAnimationLastFrame();
        }

        private void FrameCountTextboxchange(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int newcount = 1;
            if (e.Key == Key.Enter)
            {
                if (int.TryParse(FrameCountTextBox.Text, out newcount))
                {
                    if (newcount < 1)
                    {
                        newcount = 1;
                        FrameCountTextBox.Text = "1";
                    }
                }
                else
                {
                    FrameCountTextBox.Text = "1";
                }
                uIControl.ChangeMaxFrameCount(newcount);
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
        // Mainwindow -> Info -> About
        private void About(object sender, RoutedEventArgs e)
        {   
            AboutWindow win2 = new AboutWindow();
            win2.Owner = Application.Current.MainWindow;
            win2.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            win2.Show();
            
        }
        // Mainwindow -> Info -> Update

        /*     public void button1_Click(object sender, RoutedEventArgs e)
             {
                 // button1_Click.DoUpdate();
                // UpdateApplication();
                /// string updatepath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                /// string file = updatepath.Substring(0, updatepath.Length - 18) + @"\WpfApp1\Updater\TestApp\bin\Debug\TestApp.exe";
                /// Process.Start(file);
             }

            */


        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }








    }
}
