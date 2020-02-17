using System;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Windows.Forms;
namespace WpfApp1
{
    public partial class UIControl
    {

        /*
         * Event for videosource drop down, sets the videosource and shows the possible resolutionvalues in resolutiondropdown
         */
        private void comboBoxVideoSource_DropDown(object sender, System.EventArgs e)
        {
            if (mwd.comboBoxVideoSource.SelectedValue != null)
            {
                if (mwd.comboBoxVideoSource.SelectedValue == "Ulbricht" || mwd.comboBoxVideoSource.SelectedValue == "Short Circuit")
                {
                    mwd.CamElements.Visibility = Visibility.Hidden;
                    mwd.shortcircuitElements.Visibility = Visibility.Hidden;
                    mwd.UlbrichtElements.Visibility = Visibility.Hidden;
                    if (mwd.comboBoxVideoSource.SelectedValue == "Ulbricht") mwd.UlbrichtElements.Visibility = Visibility.Visible;
                    if (mwd.comboBoxVideoSource.SelectedValue == "Short Circuit") mwd.shortcircuitElements.Visibility = Visibility.Visible;
                    FrameCount = 1;
                }
                else
                {
                    FrameCount = 3;
                    mwd.CamElements.Visibility = Visibility.Visible;
                    mwd.UlbrichtElements.Visibility = Visibility.Hidden;
                    mwd.shortcircuitElements.Visibility = Visibility.Hidden;
                    mwd.comboBoxResolution.Items.Clear();
                    Resolutionlist = GetAllAvailableResolution(dsDevice[mwd.comboBoxVideoSource.SelectedIndex]);
                    for (int i = 0; i < Resolutionlist.Count; i++)
                    {
                        mwd.comboBoxResolution.Items.Insert(i, i + " " + Resolutionlist[i].ToString());
                    }
                }
                SetButtonIsEnabled(false, false, true, false, false);
            }
        }
        /*
         * Event for resolution change, sets the resolution
         */
        private void comboBoxResolution_DropDown(object sender, System.EventArgs e)
        {
            if (mwd.comboBoxVideoSource.SelectedValue != null && mwd.comboBoxResolution.SelectedValue != null)
            {
                res = Resolutionlist[mwd.comboBoxResolution.SelectedIndex].Split(new char[] { 'x' });
            }
        }
        /*
         * Event for select or change camera
         */
        private void SelectCameraButtonEvent(object sender, RoutedEventArgs e)
        {
            SetButtonIsEnabled(false, false, true, false, false);
            if (mwd.selectcamButton.IsEnabled == true && mwd.comboBoxVideoSource.SelectedValue != null && (mwd.comboBoxResolution.SelectedValue != null || mwd.comboBoxVideoSource.SelectedValue == "Ulbricht"))
            {
                if (mwd.comboBoxVideoSource.SelectedValue != "Ulbricht" && mwd.comboBoxVideoSource.SelectedValue != "Short Circuit")
                {
                    uICam.CamChange(mwd.comboBoxVideoSource.SelectedIndex, ExposureVal, Convert.ToInt32(res[0]), Convert.ToInt32(res[1])); // transmits paramters to uIcam, waits until camera is changed
                    Normalmode();                                  // Shows normal Cam Signal
                    // resets properties, otherwise there will be bugs 
                    mwd.mirrorV.IsEnabled = true;
                    mwd.mirrorH.IsEnabled = true;
                    uICam.SetMirrorbools((bool)mwd.mirrorH.IsChecked, (bool)mwd.mirrorV.IsChecked);
                    mwd.autoBrightnessSubtraction.IsEnabled = true;
                    mwd.autoBrightnessSubtraction.IsChecked = false;
                    mode = "Cam";
                }
                else
                {
                    if (mwd.comboBoxVideoSource.SelectedValue == "Ulbricht") mode = "Ulbricht";
                }
                SetButtonIsEnabled(true, false, true, false, true);  // Sets buttons enabled/disabled
                if (mwd.comboBoxVideoSource.SelectedValue == "Short Circuit") SetButtonIsEnabled(false, false, true, false, false);
            }
        }
        /*
         * Events for 3 sliderchanges, changes slidertextboxvalue
         */
        private void sliderbackground_ValueChanged1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            int _index = 0;
            int val = (int)sliderbackground[_index].Value;
            if (val > 255) val = 255;

            sliderbackgroundtextbox[_index].Text = val.ToString();
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackgroundtextbox[i].Text = val.ToString();
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        private void sliderbackground_ValueChanged2(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            int _index = 1;
            int val = (int)sliderbackground[_index].Value;
            if (val > 255) val = 255;

            sliderbackgroundtextbox[_index].Text = val.ToString();
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackgroundtextbox[i].Text = val.ToString();
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        private void sliderbackground_ValueChanged3(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            int _index = 2;
            int val = (int)sliderbackground[_index].Value;
            if (val > 255) val = 255;

            sliderbackgroundtextbox[_index].Text = val.ToString();
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackgroundtextbox[i].Text = val.ToString();
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        /*
         * Event for 3 Slider_textboxchanges, sets slidervalues
         */
        private void Slider_TextboxChange1(object sender, EventArgs e)
        {
            int _index = 0;
            int val = 0;
            if (int.TryParse(sliderbackgroundtextbox[_index].Text, out val)) { }
            if (val > 255) val = 255;

            sliderbackground[_index].Value = val; // on change
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        private void Slider_TextboxChange2(object sender, EventArgs e)
        {
            int _index = 1;
            int val = 0;
            if (int.TryParse(sliderbackgroundtextbox[_index].Text, out val)) { }
            if (val > 255) val = 255;

            sliderbackground[_index].Value = val; // on change
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        private void Slider_TextboxChange3(object sender, EventArgs e)
        {
            int _index = 2;
            int val = 0;
            if (int.TryParse(sliderbackgroundtextbox[_index].Text, out val)) { }
            if (val > 255) val = 255;

            sliderbackground[_index].Value = val; // on change
            if (mwd.grayBrightnessSubtraction.IsChecked == true)
            {
                for (int i = 0; i < 3; i++)
                {
                    sliderbackground[i].Value = val;
                }
            }
            if (AutoCounter > FrameCount) mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        /*
         * Resets slider and slider textbox values
         */
        private void ResetSliderandTextBox()
        {
            for (int i = 0; i < 3; i++)
            {
                sliderbackgroundtextbox[i].Text = 0.ToString();
                sliderbackground[i].Value = 0;
            }
        }
        /*
         * Sets the slidervalues from autobackgroundsubtraction
         */
        private void SetAutoValues()
        {
            for (int i = 0; i < 3; i++)
            {
                if (mwd.grayBrightnessSubtraction.IsChecked == true)
                {
                    sliderbackgroundtextbox[i].Text = gr.ToString();
                    sliderbackground[i].Value = gr;
                }
                if (mwd.rgbBrightnessSubtraction.IsChecked == true)
                {
                    sliderbackgroundtextbox[i].Text = autorgb[i].ToString();
                    sliderbackground[i].Value = autorgb[i];
                }
            }
        }
        /*
         * Events that sets skiped frames if uservalue is an int
         */
        private void SkipFrameCountChange(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.SkipFrameCounttextbox.Text, out FrameCount))
                mwd.autoBrightnessSubtraction.IsChecked = false;
        }
        /*
         * Event for exposurevalue change
         */
        private void SliderExposure_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ExposureVal = (int)mwd.SliderExposure.Value;
            mwd.SliderExposuretext.Text = "Exposure Value: " + ExposureVal.ToString(); // Sets the string above slider
            ExposureVal = ExposureVal * (-1);
        }
        /*
         * Event for flip/mirror function
         */
        private void MirrorChangedEvent(Object sender, EventArgs e)
        {
            uICam.SetMirrorbools((bool)mwd.mirrorH.IsChecked, (bool)mwd.mirrorV.IsChecked);
        }
        /*
         * Event for brightnessmoduschanged, toggles the input so only one checkbox is set
         */
        private void Brightnessmoduschanged(Object sender, EventArgs e)
        {
            System.Windows.Controls.RadioButton curCheckbox = sender as System.Windows.Controls.RadioButton;

            if (curCheckbox == mwd.grayBrightnessSubtraction)
            {
                mwd.rgbBrightnessSubtraction.IsChecked = false;
            }
            else
            {
                mwd.grayBrightnessSubtraction.IsChecked = false;
            }
            mwd.autoBrightnessSubtraction.IsChecked = false;
            curCheckbox.IsChecked = true;
        }
        /*
         * 
         */
        private void AutoModusChecked(Object sender, EventArgs e)
        {
            AutoCounter = 1;
        }
        /*
         * Event for starting the sequence
         */
        private void startSequenceButtonMouseDown(object sender, RoutedEventArgs e)
        {
            if (mwd.startSequenceButton.IsEnabled == true)
            {
                if (mode == "Ulbricht")
                {
                    FolderBrowserDialog fbd = new FolderBrowserDialog();
                    DialogResult result = fbd.ShowDialog();
                    if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                    {
                        selectedPath = fbd.SelectedPath;
                        Directory.CreateDirectory(selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd"));
                        measurePath = selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + "\\" + System.IO.Directory.GetDirectories(selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + "\\").Length.ToString() + "_" + DateTime.Now.ToString("HH mm");
                        Directory.CreateDirectory(measurePath);
                        Ulbricht ulbricht = new Ulbricht();
                        mwd.WindowState = WindowState.Minimized;
                        ulbricht.clicks((RowCount * ColCount).ToString(), ((int)(pixeldelay)).ToString(), measurePath);
                        mwd.WindowState = WindowState.Maximized;
                        mwd.WindowState = WindowState.Normal;
                        mwd.Topmost = true;
                        mwd.Show();
                        mwd.Topmost = false;
                        SequenceMode();
                        SetButtonIsEnabled(false, true, false, false, false);
                    }
                }
                else
                {
                    SequenceMode();
                    SetButtonIsEnabled(false, true, false, false, false);
                }
            }
        }
        private void Startsweep(object sender, RoutedEventArgs e)
        {
            if (mwd.starSweep.IsEnabled == true)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                DialogResult result = fbd.ShowDialog();
                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    selectedPath = fbd.SelectedPath;
                    Directory.CreateDirectory(selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd"));
                    measurePath = selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + "\\" + System.IO.Directory.GetDirectories(selectedPath + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + "\\").Length.ToString() + "_" + DateTime.Now.ToString("HH mm");
                    Directory.CreateDirectory(measurePath);
                    Ulbricht ulbricht = new Ulbricht();
                    mwd.WindowState = WindowState.Minimized;
                    ulbricht.clicks((1 + (Endsweepvoltage - Startsweepvoltage) / Stepsweepvoltage).ToString(), ((int)(pixeldelay)).ToString(), measurePath);
                    mwd.WindowState = WindowState.Maximized;
                    mwd.WindowState = WindowState.Normal;
                    mwd.Topmost = true;
                    mwd.Show();
                    mwd.Topmost = false;
                    VoltageSweepMode();
                    SetButtonIsEnabled(false, true, false, false, false);
                }
            }
        }
        /*
         * Event for stoping sequence or calibration
         */
        private void stopSequenceButtonMouseDown(object sender, RoutedEventArgs e)
        {
            if (mwd.stopSequenceButton.IsEnabled == true)
            {
                StartStopUserDrawInputs(true);  // User can change pixel by hand again
                SetButtonIsEnabled(true, false, true, false, true);
                sequence = false;
                calib = false;
            }
        }
        /*
         *  Scales the array up to 0-255 value
         */
        private void RepaintArray(object sender, RoutedEventArgs e)
        {
            SetMinMax();
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    if (grayarray[i, j] != 0)
                    {
                        grayarray[i, j] = (int)(255 * grayarray[i, j] / max);
                        SetGrayvalueToArrayPixel(i, j, grayarray[i, j]);
                    }
                }
            }
            SetButtonIsEnabled(mwd.startSequenceButton.IsEnabled, mwd.stopSequenceButton.IsEnabled, mwd.selectcamButton.IsEnabled, true, true);
        }
        /*
         * Event for textboxchange
         */
        private void steptextboxChange(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.steptextbox.Text, out step)) ;
        }
        /*
         * Reducedarea values changed
         */
        private void ReducedAreatopleftchangex(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.topleftboxx.Text, out topleftpixelx)) uICam.setsquarevalues(topleftpixelx, buttomleftpixelx, topleftpixely, buttomleftpixely, linethickness);
        }

        private void ReducedAreabuttomrightchangex(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.buttomrightboxx.Text, out buttomleftpixelx)) uICam.setsquarevalues(topleftpixelx, buttomleftpixelx, topleftpixely, buttomleftpixely, linethickness);
        }
        private void ReducedAreatopleftchangey(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.topleftboxy.Text, out topleftpixely)) uICam.setsquarevalues(topleftpixelx, buttomleftpixelx, topleftpixely, buttomleftpixely, linethickness);
        }
        private void ReducedAreabuttomrightchangey(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.buttomrightboxy.Text, out buttomleftpixely)) uICam.setsquarevalues(topleftpixelx, buttomleftpixelx, topleftpixely, buttomleftpixely, linethickness);
        }
        private void ReducedArealinethicknessboxchange(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.linethicknessbox.Text, out linethickness)) uICam.setsquarevalues(topleftpixelx, buttomleftpixelx, topleftpixely, buttomleftpixely, linethickness);
        }
        /*
         * Minimum gray value changed for calibration
         */
        private void mingrayvalueChanged(object sender, EventArgs e)
        {
            if (double.TryParse(mwd.mingrayvaluetextbox.Text, out Calibrationgraymin)) ; //set textbox.text to Calibrationgraymin if, textbox.text is an integer
        }
        /*
         * Buttonevent for saveing array from sequence to .csv
         */
        private void SaveMeasureButtonEvent(object sender, RoutedEventArgs e)
        {
            if (mwd.savebutton.IsEnabled == true)
            {
                SaveArrayAsCSV(grayarray, "FileName", "");
            }
        }
        /*
         * Buttonevent for loading calibration array from .csv
         */
        private void LoadcalibbuttonEvent(object sender, RoutedEventArgs e)
        {
            if (mwd.loadbutton.IsEnabled == true)
            {
                voltagearray = new double[ColCount, RowCount];
                LoadArrayFromCSV(voltagearray);
            }
        }

        private void Ulbrichtvalueschanged(object sender, EventArgs e)
        {
            if (int.TryParse(mwd.Timerpixeldelay.Text, out pixeldelay)) ;
            if (int.TryParse(mwd.Startwavelength.Text, out startwavelength)) ;
            if (int.TryParse(mwd.Endwavelength.Text, out endwavelength)) ;
        }
        private async void Sweepvolatgechanged(object sender, RoutedEventArgs e)
        {
            Boolean warningbool = true;
            System.Windows.Controls.TextBox _sender = (System.Windows.Controls.TextBox)sender;
            double currentvalue = 0;
            if (double.TryParse(_sender.Text, out currentvalue)) ;
            if (currentvalue > Voltagelimit && safemodus == false)
            {
                SetVoltageWindowWarning setVoltagewindowwarning = new SetVoltageWindowWarning(this);
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
                if (int.TryParse(mwd.Startsweepvoltageedit.Text, out Startsweepvoltage)) ;
                if (int.TryParse(mwd.Endsweepvoltageedit.Text, out Endsweepvoltage)) ;
                if (int.TryParse(mwd.StepSweepvoltageedit.Text, out Stepsweepvoltage)) ;
                if (int.TryParse(mwd.xSweepvoltageedit.Text, out xsweep)) ;
                if (int.TryParse(mwd.ySweepvoltageedit.Text, out ysweep)) ;
            }
            else
            {
                _sender.Text = Endsweepvoltage.ToString();
            }
        }
        private void CurrentShowClick(object sender, RoutedEventArgs e)
        {
            ShowCurrent();
            mwd.savebutton.IsEnabled = true;

        }
        private void currentlimitchanged(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(mwd.lowercurrent.Text, out shortcircuitminimum)) ;
            if (double.TryParse(mwd.highercurrent.Text, out shortcircuitmaximum)) ;
        }
    }
}

