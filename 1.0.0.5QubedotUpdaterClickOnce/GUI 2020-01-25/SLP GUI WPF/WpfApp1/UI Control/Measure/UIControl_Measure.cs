using System;
using System.Linq;
using System.IO;
using System.Windows;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class UIControl
    {
        private String[] res;
        private int ExposureVal = -8;
        private double max, Calibrationgraymin=0, min=0;
        private int FrameCount = 3;
        private bool sequence = false, calib = false, normal = false;
        private double[] autorgb;
        private double[,] grayarray;
        private double[,] voltagearray;
        private int AutoCounter = 1;
        private double gr = 0;
        private int step = 75;
        private static string lastpath = "C:\\";
        private string mode = "Cam";
        private string selectedPath="";
        private string[] files;
        private int startwavelength = 100, endwavelength = 1300;
        private int Startsweepvoltage = 2500, Endsweepvoltage = 3500, Stepsweepvoltage=75;
        private int xsweep = 1, ysweep = 1;
        private string measurePath;
        private int lastfilecount = 0;
        private int pixeldelay = 1;
        private int loopms = 50;
        private double[,] Currentarray;
        private double shortcircuitminimum = 100, shortcircuitmaximum = 1000;
        /*
         * Normalmode, gets picture and plots it
         */
        private async void Normalmode()
        {
            if(mode == "Cam")
            {
                if (normal == false)
                {
                    normal = true;
                    while (mwd.opend == true && sequence == false && calib == false)    //if measurewindow is opend, sequence and calib arnt running
                    {
                        Task<BitmapSource> TaskTakePic = Task<BitmapSource>.Factory.StartNew(() => GetPic()); // Gets picture
                        BitmapSource result = await TaskTakePic;    // Wait until picture is there, needed otherwise GUI looks freezed
                        TaskTakePic.Dispose();                      // Dispose bitmapsource to free memory
                        double[] rgb = uICam.getrgb();
                        Plot(result, getSubtractedGray(rgb));       // plots the image and grayvalues
                    }
                }
                if (mwd.opend == false)
                {
                    Task TaskTakePic = Task.Factory.StartNew(() => uICam.Dispose()); //if measurewindow closed, uICam will be disposed after picture is captured
                    TaskTakePic.Wait();
                    uICam = null;
                }
            }
        }
        /*
         * Sequencemode, lights up x,y led and measures the grayvalue of the taken image. Several images are taken and grayvalue will
         * be averaged.
         */
        private async Task SequenceMode()
        {
            ClearArray();
            //ChangeMode(DSPMode);                        // only one ledpixel is set at the time
            StartStopUserDrawInputs(false);             // disables the possibility of a userinput
            SetPixels(false);                           // turns off all leds
            sequence = true;
            normal = false;
            grayarray = new double[RowCount, ColCount];
            SetVoltage((uint)Standardvoltage);                          // set voltage to max
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    if (sequence == true)
                    {
                        if (i != 0) drawActionOutput.SetOutputPixel((uint)i - 1, (uint)j);                      // set last led to off
                        if (i == 0 && j != 0) drawActionOutput.SetOutputPixel((uint)ColCount - 1, (uint)j - 1); // set last led to off
                        DrawOutputFrame();                                                                    // execute led change
                        drawActionOutput.SetOutputPixel((uint)i, (uint)j);                                      // set current led
                        if (voltagearray != null && calib == false) SetVoltage((uint)voltagearray[i, j]);        // loads voltage from the calibration file
                        DrawOutputFrame();                                                                    // execute led change 
                        for (int n = 0; n < FrameCount; n++)    // for several images
                        {
                            Task<BitmapSource> TaskTakePic = Task<BitmapSource>.Factory.StartNew(() => GetPic()); // starts takepic in new task 
                            BitmapSource result = await TaskTakePic; // waits asynchronous until image is taken and bitmapsource is returned. async needed otherwise gui is frozen
                            if (mode == "Cam")
                            {
                                double[] rgb = getSubtractedGray(uICam.getrgb());           // subtract the background of the rgb values
                                Plot(result, rgb);                                          // plots the image
                                double gray = GetGrayValue(rgb);
                                grayarray[i, j] = gray;
                            }
                            if (mode == "Ulbricht")
                            {
                                Task<double> TaskTakeUlbricht = Task<double>.Factory.StartNew(() => getGrayUlbricht(i, j, ""));
                                grayarray[i, j] = await TaskTakeUlbricht;
                            }
                            SetGrayvalueToArrayPixel(i, j, grayarray[i, j]);                       // plots the grayvalue to the measure array
                        }
                    }
                }
            }
            if(mode == "Ulbricht") SaveArrayAsCSV(grayarray, "_"+ startwavelength.ToString() + "-" + endwavelength.ToString() + "nm_µW.csv", measurePath);   // saves the calibration array to a csv file
            if (sequence == true) SetButtonIsEnabled(true, false, true, true, true);
            SetMinMax();                                                            // sets min and max gray value of the measured grayarray
            sequence = false;
            Normalmode();                                                           // starts normalmode again
            StartStopUserDrawInputs(true);                                          // Enables user drawbars
            toolbox.SetCurrentDrawTool(toolbox.CurrentDrawTool);                    // Sets default drawtool(normal)
            lastfilecount = 0;
        }
        /*
         * calibrationmode, runs sequencemode first to get a grayarray. after that the total voltage for the board is adjusted for each led
         * so that every led is returning nearly the same gray value in sequence mode.
         */
        private async void CalibrationMode(object sender, RoutedEventArgs e)
        {
            calib = true;
            normal = false;
            SetVoltage((uint)Endsweepvoltage);                              // sets voltage to max
            bool calibfinished = false;
            SetButtonIsEnabled(false, true, false, false, false);
            SetPixels(false);                               // turns off all leds
            if (voltagearray != null) voltagearray = null;
            await SequenceMode();                           // gets an measured array. async for gui not to freeze
            normal = false;
            SetButtonIsEnabled(false, true, false, false, false);
            voltagearray = new double[RowCount, ColCount];  
            StartStopUserDrawInputs(false);
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    SetVoltage((uint)Startsweepvoltage);   // set voltage to minimum
                    if (i != 0) drawActionOutput.SetOutputPixel((uint)i - 1, (uint)j); // set last led to off
                    if (i == 0 && j != 0) drawActionOutput.SetOutputPixel((uint)ColCount - 1, (uint)j - 1);// set last led to off
                    DrawOutputFrame();
                    drawActionOutput.SetOutputPixel((uint)i, (uint)j);   // turn on current led
                    DrawOutputFrame();
                    if (grayarray[i, j] != 0 && grayarray[i, j] > min)
                    {
                        for (int v = Startsweepvoltage; v < Endsweepvoltage; v = v)        // search for the matching voltage
                        {
                            double gray = 0;
                            if (calib == true)
                            {
                                SetVoltage((uint)v);    //set total voltages, 5 times not sure if its really needed  
                                for (int n = 0; n < FrameCount; n++)
                                {
                                    Task<BitmapSource> TaskTakePic = Task<BitmapSource>.Factory.StartNew(() => GetPic());
                                    BitmapSource result = await TaskTakePic;
                                    double[] rgb = getSubtractedGray(uICam.getrgb());
                                    Plot(result, rgb);
                                    gray = GetGrayValue(rgb);
                                }
                                if (gray >= min)    // if current grayvalue is bigger then minimum gray value the voltage is found and will be saved to the voltagearray
                                {
                                    voltagearray[i, j] = v;
                                    v = Endsweepvoltage + 1;
                                }
                                if (i == RowCount - 1 && j == ColCount - 1) calibfinished = true;
                            }
                            if (gray <= min)
                            {
                                // changes the voltage with a logic so the calibration doesnt take that long
                                if (safemodus == false)
                                {
                                    v = v + step * 4;
                                    if (gray <= min * 3 / 4)
                                    {
                                        v = v - step * 2;
                                        if (gray <= min * 2 / 4)
                                        {
                                            v = v - step;
                                            if (gray <= min * 1 / 4)
                                            {
                                                v = v - step / 2;
                                            }
                                            else v = v - step / 4;
                                        }
                                    }
                                }
                                else
                                {
                                    if(v+step < Endsweepvoltage) v = v + step;
                                }
                            }
                        }
                    }
                    else voltagearray[i, j] = 0;
                    if (grayarray[i, j] == min) voltagearray[i, j] = Endsweepvoltage;
                    if (grayarray[i, j] != 0 && grayarray[i, j] < min) voltagearray[i, j] = 0;
                }
            }
            if (calibfinished == false)
            {
                voltagearray = null;
            }
            else SaveArrayAsCSV(voltagearray, "Calibrationfile", "");   // saves the calibration array to a csv file
            SetVoltage((uint)Standardvoltage);
            SetButtonIsEnabled(true, false, true, true, true);
            calib = false;
            Normalmode();
            StartStopUserDrawInputs(true);
        }
        private async Task VoltageSweepMode()
        {
            ClearArray();
            //ChangeMode(DSPMode);                        // only one ledpixel is set at the time
            StartStopUserDrawInputs(false);             // disables the possibility of a userinput
            sequence = true;
            normal = false;
            int buffer = Endsweepvoltage;
            if (Endsweepvoltage < Startsweepvoltage)
            {
                Endsweepvoltage = Startsweepvoltage;
                Startsweepvoltage = buffer;
            }
            int grenz = 1+ (Endsweepvoltage - Startsweepvoltage) / Stepsweepvoltage;
            double[,] sweeparray = new double[2, grenz];
            //drawActionOutput.SetOutputPixel((uint)xsweep, (uint)ysweep);                            // set current led
            //DrawOutputOnPixel();
            for (int i = 0; i < grenz; i++)
            {
                if (sequence == true)
                {
                    SetVoltage((uint)(Startsweepvoltage + Stepsweepvoltage * i));                          // set voltage to max
                    Task<BitmapSource> TaskTakePic = Task<BitmapSource>.Factory.StartNew(() => GetPic()); // start takepic in new thread 
                    BitmapSource result = await TaskTakePic;                                                // waits asynchronous until image is taken and bitmapsource is returned. async needed otherwise gui is frozen
                    Task<double> TaskTakeUlbricht = Task<double>.Factory.StartNew(() => getGrayUlbricht(xsweep, ysweep, "_x"+ xsweep.ToString()+",y"+ ysweep.ToString()+"_"+(Startsweepvoltage + Stepsweepvoltage * i).ToString()+"V"));
                    sweeparray[0, i] = (double)(Startsweepvoltage + Stepsweepvoltage * i);
                    sweeparray[1, i] = await TaskTakeUlbricht;
                    SetGrayvalueToArrayPixel(xsweep, ysweep, sweeparray[1, i]);
                }
            }
            if (sequence == true) SetButtonIsEnabled(true, false, true, true, true);
            //SetMinMax();                                                            // sets min and max gray value of the measured grayarray
            sequence = false;
            StartStopUserDrawInputs(true);                                          // Enables user drawbars

            toolbox.SetCurrentDrawTool(toolbox.CurrentDrawTool);                    // Sets default drawtool(normal)
            SetVoltage((uint)Standardvoltage);
            lastfilecount = 0;
            SaveArrayAsCSV(sweeparray, Startsweepvoltage.ToString() + "V_" + Endsweepvoltage.ToString() + "V.csv", measurePath);//"x" + xsweep.ToString() + ",y" + ysweep.ToString() + "_" + Startsweepvoltage.ToString()+"V_"+Endsweepvoltage.ToString()+ "V.csv", measurePath);   // saves the calibration array to a csv file
        }
        /*
         * Plots the Image, plots the grayvalue, and the average rgb values
         */
        public void Plot(BitmapSource _source, double[] _rgb)
        {
            mwd.PlotImg.Source = _source;
            _source = null;
            double currentavg = (_rgb[0] + _rgb[1] + _rgb[2]) / 3;
            mwd.PlotGray.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)currentavg, (byte)currentavg, (byte)currentavg));
            // Diffrent textcolor, if background is to dark -> white color
            if (currentavg > 126)
            {
                mwd.grayvaluetext.Foreground = new SolidColorBrush(Colors.Black);
                mwd.rgbvaluetext.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                mwd.grayvaluetext.Foreground = new SolidColorBrush(Colors.White);
                mwd.rgbvaluetext.Foreground = new SolidColorBrush(Colors.White);
            }
            mwd.grayvaluetext.Text = GetGrayValue(_rgb).ToString("0.##");   // Shows grayvalue
            mwd.rgbvaluetext.Text = "R: "+_rgb[0].ToString("0.##")+"  G:" + _rgb[1].ToString("0.##") + "  B:" + _rgb[2].ToString("0.##"); // Shows avrg RGB values
            mwd.PlotAvrageRGB.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)_rgb[0], (byte)_rgb[1], (byte)_rgb[2]));
        }
        /*
         * Set the grayvalue to the x,y rectangle from the array
         */
        private void SetGrayvalueToArrayPixel(int _x, int _y, double _value)
        {
            if(_value<255) rectimg[_x, _y].Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)_value, (byte)_value, (byte)_value));
            else rectimg[_x, _y].Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb((byte)255, (byte)255, (byte)255));
            rectimg[_x, _y].IsEnabled = false;
            if (_value > 126) textarray[_x, _y].Foreground = new SolidColorBrush(Colors.Black);
            else textarray[_x, _y].Foreground = new SolidColorBrush(Colors.White);
            textarray[_x, _y].Text = _value.ToString();
            if(RowCount >8) textarray[_x, _y].FontSize = 7;
            else textarray[_x, _y].FontSize = 11;
        }
        /*
         * Backgroundsubtraction
         */
        private double[] getSubtractedGray(double[] _rgb)
        {
            double[] ret = new double[3];
            if (mwd.autoBrightnessSubtraction.IsChecked == true)
            {
                if (AutoCounter <= FrameCount) // gets rgb values from the background
                {
                    if (AutoCounter == 1) autorgb = new double[3];
                    if (AutoCounter == 1) ResetSliderandTextBox();
                    for (int i = 0; i < 3; i++)
                    {
                        autorgb[i] = autorgb[i] + _rgb[i];
                        if (AutoCounter == FrameCount)
                        {
                            autorgb[i] = autorgb[i] / FrameCount;
                        }
                    }
                    if (AutoCounter == FrameCount)
                    {
                        gr = ((autorgb[0] + autorgb[1] + autorgb[2]) / 3);
                        SetAutoValues();
                    }
                    AutoCounter++;
                }
                if(AutoCounter > FrameCount)    //after enough rgb values are taken set the subtraction values
                {
                    for (int i = 0; i < 3; i++)
                    {
                        if (mwd.rgbBrightnessSubtraction.IsChecked == true)
                        {
                            ret[i] = _rgb[i] - autorgb[i];
                        }
                        if(mwd.grayBrightnessSubtraction.IsChecked == true)
                        {
                            ret[i] =((_rgb[0]+ _rgb[1]+ _rgb[2])/3) - gr;
                        }
                        if (ret[i] < 0) ret[i] = 0;
                    }
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    ret[i] = _rgb[i] - sliderbackground[(i)].Value; // subtracts the values the user set in the sliders
                    if (ret[i] < 0) ret[i] = 0;
                }
            }
            return ret;

        }
        /*
         * Calcs the grayvalue
         */
        private double GetGrayValue(double[] _rgb)
        {
            double plotrgb = 1;
            double[] gray = new double[3];
            gray = _rgb;
            if (mwd.graymoduscomboBox.SelectedIndex == 0)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (_rgb[k] > 0) plotrgb = plotrgb * gray[k];
                }
                if (_rgb[0] == 0 && _rgb[1] == 0 && _rgb[2] == 0) plotrgb = 0;
            }
            //---------------------------------------------------------------------------------------------------------------
            // Diffrent calculation methods
            if (mwd.graymoduscomboBox.SelectedIndex == 1) plotrgb =  (_rgb[0] + _rgb[1] + _rgb[2])/3; // 
            if (mwd.graymoduscomboBox.SelectedIndex == 2) plotrgb =  (0.299 * _rgb[0] + 0.587 * _rgb[1] + 0.114*_rgb[2]); // 
            if (mwd.graymoduscomboBox.SelectedIndex == 3) plotrgb =  Math.Sqrt((0.299*(_rgb[0] * _rgb[0]) + 0.587 * (_rgb[1] + _rgb[1]) + 0.114*(_rgb[2] * _rgb[2]))); //
            if (mwd.graymoduscomboBox.SelectedIndex == 4) plotrgb =  ((_rgb[1]+_rgb[2])/2); // (g+b)/2
            if (mwd.graymoduscomboBox.SelectedIndex == 5) plotrgb =  (_rgb[0]); // r
            if (mwd.graymoduscomboBox.SelectedIndex == 6) plotrgb = (_rgb[1]); // g
            if (mwd.graymoduscomboBox.SelectedIndex == 7) plotrgb = (_rgb[2]); // b

            return plotrgb;
        }
        /*
         * Get minimum and maximum from the measured array
         */
        private void SetMinMax()
        {
            max = 0;
            min = 100000000;
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    if (max < grayarray[i, j]) max = grayarray[i, j];
                    if (min > grayarray[i, j] && grayarray[i, j] != 0) min = grayarray[i, j];
                }
            }
            if (Calibrationgraymin > min) min = Calibrationgraymin; //used if user wants to exclude some leds that have low grayvalue
        }
        /*
         * Opens dialog for saving the array to a .csv file
         */
        private static void SaveArrayAsCSV(double[,] arrayToSave, string _FileName, String _path)
        {
            if (arrayToSave != null)
            {
                string filename = "default.csv";
                if (_path == "")
                {
                    System.Windows.Forms.SaveFileDialog saveFileDialog1;
                    saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
                    saveFileDialog1.InitialDirectory = lastpath;
                    saveFileDialog1.FileName = _FileName;
                    saveFileDialog1.DefaultExt = "csv";
                    saveFileDialog1.Filter = "CSV (*.csv)|*.csv";
                    saveFileDialog1.AddExtension = true;
                    System.Windows.Forms.DialogResult dr = saveFileDialog1.ShowDialog();
                    if (dr == System.Windows.Forms.DialogResult.OK)
                    {
                        filename = saveFileDialog1.FileName; // gets path + name
                        lastpath = System.IO.Path.GetDirectoryName(filename);
                    }
                }
                else
                {
                    filename = _path + "\\" + _FileName;
                }
                using (StreamWriter file = new StreamWriter(filename))
                {
                    for (int j = 0; j < arrayToSave.GetLength(1); j++)
                    {
                        for (int i = 0; i < arrayToSave.GetLength(0); i++)
                        {
                            file.Write(arrayToSave[i,j].ToString("F6"));
                            file.Write("\t"); // it is comman and not a tab
                        }
                        file.Write("\n"); // go to next line
                    }
                }
            }
        }
        /*
         * Opens dialog to select .csv that is loaded to transmitted array
         */
        private void LoadArrayFromCSV(double[,] _arrayto)
        {
            System.Windows.Forms.OpenFileDialog fDialog = new System.Windows.Forms.OpenFileDialog();
            if (fDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            System.IO.FileInfo fInfo = new System.IO.FileInfo(fDialog.FileName);
            string strFileName = fInfo.Name;
            string strFilePath = fInfo.DirectoryName;
            string filePath = strFilePath +"\\"+ strFileName;
            if (filePath.Contains(".csv"))              //dont loads if file isnt a .csv file
            {
                var filearray = File.ReadAllLines(filePath).Select(x => x.Split('\t')).ToArray();
                for (int i = 0; i < filearray.Length; i++)
                {
                    for (int j = 0; j < filearray[i].Length - 1; j++)
                    {
                        if (i < ColCount && j < RowCount) _arrayto[i, j] = Convert.ToInt64(Math.Floor(Convert.ToDouble(filearray[i][j])));
                    }
                }
            }
        }
        private void LoadFile(String _path, String _filename, String[,] _toarray)
        {
            System.IO.FileInfo fInfo = new System.IO.FileInfo(_filename);
            string strFileName = fInfo.Name;
            string strFilePath = fInfo.DirectoryName;
            string filePath = strFilePath + "\\" + strFileName;
            var filearray = File.ReadAllLines(filePath).Select(x => x.Split('\t')).ToArray();
            for (int i = 0; i < filearray.Length; i++)
            {
                for (int j = 0; j < filearray[i].Length; j++)
                {
                    if (i < _toarray.GetLength(0) && j < _toarray.GetLength(1)) _toarray[i, j] = filearray[i][j];//Convert.ToInt64(Math.Floor(Convert.ToDouble(filearray[i][j])));
                }
            }
        }
        private (int, int) GetRowCoulmofFile(String _path, String _filename)
        {
            System.IO.FileInfo fInfo = new System.IO.FileInfo(_filename);
            string strFileName = fInfo.Name;
            string strFilePath = fInfo.DirectoryName;
            string filePath = strFilePath + "\\" + strFileName;
            var filearray = File.ReadAllLines(filePath).Select(x => x.Split('\t')).ToArray();
            return (filearray.Length, filearray[1].Length);
        }
        private BitmapSource GetPic()
        {
            if(mode == "Ulbricht")
            {
                WaitForSpectra();
                return null;
            }
            if(mode == "Cam")
            {
                return uICam.TakePic();
            }
            return null;
        }
        private double getGrayUlbricht(int r, int c, String _filename) // c= row, r= coulmn
        {
            //vlt. kleiner delay
            double ret = 0;
            files = Directory.GetFiles(measurePath);
            double a = files.Length;
            (int, int) tuple;
            if (files.Length > 0)
            {
                tuple = GetRowCoulmofFile(selectedPath, files[files.Length - 1]);
                String[,] array = new String[tuple.Item1, tuple.Item2];
                LoadFile(measurePath, files[files.Length - 1], array);
                String renamed = "";
                if(_filename == "")
                {
                    renamed = files[files.Length - 1].Remove(files[files.Length - 1].IndexOf(".txt")) + "Row_" + c.ToString() + "Col_" + r.ToString()+".txt";
                }
                else
                {
                    renamed = files[files.Length - 1].Remove(files[files.Length - 1].IndexOf(".txt")) + _filename + ".txt";
                }
                File.Copy(files[files.Length - 1], renamed, true);
                File.Delete(files[files.Length - 1]);
                for (int i = 10; i < array.GetLength(0); i++)
                {
                    if (Convert.ToDouble(array[i, 0]) >= startwavelength && Convert.ToDouble(array[i, 0]) <= endwavelength)
                    {
                        ret = ret + Convert.ToDouble(array[i, 1]);
                    }
                }
            }
            return ret;
        }
        private void WaitForSpectra()
        {
            files = Directory.GetFiles(measurePath);
            int a = files.Length;
            if(a == 0)
            {
                while (a == 0)
                {
                    Thread.Sleep(loopms);
                    files = Directory.GetFiles(measurePath);
                    a = files.Length;
                }
            }
            else
            {
                while (a == lastfilecount)
                {
                    Thread.Sleep(loopms);
                    files = Directory.GetFiles(measurePath);
                    a = files.Length;
                }
            }
            lastfilecount = a;
        }
        public void SetCurrentArray(int[,] _CurrentArray)
        {
            Currentarray = new double[RowCount, ColCount];
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    Currentarray[i, j] = _CurrentArray[i, j];
                }
            }
        }
        private void ShowCurrent()
        {
            grayarray = new double[RowCount, ColCount];
            for (int j = 0; j < RowCount; j++)
            {
                for (int i = 0; i < ColCount; i++)
                {
                    //Currentarray[i, j] = 75 * i + 75 * j;
                    grayarray[i, j] = Currentarray[i, j];
                    rectimg[i, j].Fill = new SolidColorBrush(Colors.Yellow);
                    if (Currentarray[i, j] < shortcircuitminimum) rectimg[i, j].Fill = new SolidColorBrush(Colors.Red);
                    if (Currentarray[i, j] > shortcircuitmaximum) rectimg[i, j].Fill = new SolidColorBrush(Colors.Green);
                    textarray[i, j].Text = Currentarray[i, j].ToString();
                }
            }
        }
    }
}
