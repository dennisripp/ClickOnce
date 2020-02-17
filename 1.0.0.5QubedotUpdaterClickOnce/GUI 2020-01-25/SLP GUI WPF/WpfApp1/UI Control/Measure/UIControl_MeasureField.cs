using System;
using System.IO;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Input;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using System.Windows.Markup.Primitives;
using DirectShowLib;
namespace WpfApp1
{
    public partial class UIControl
    {
        private TextBox[] sliderbackgroundtextbox = new TextBox[3];
        private Slider[] sliderbackground = new Slider[3];
        private System.Windows.Shapes.Rectangle[,] rectimg;
        private TextBlock[,] textarray;
        private int topleftpixelx = 0, buttomleftpixelx = 0, topleftpixely = 0, buttomleftpixely = 0, linethickness = 3;
        private static int imgwidth = 320 / 2;
        private List<String> Resolutionlist;
        private DsDevice[] dsDevice;
        private int pixeldistance=1;
        /*
         * Creats the measurefield
         */
        public void DrawField()
        {
            if (sliderbackground[0] == null)
            {
                sliderbackground[0] = mwd.sliderbackground1;
                sliderbackground[1] = mwd.sliderbackground2;
                sliderbackground[2] = mwd.sliderbackground3;
                sliderbackgroundtextbox[0] = mwd.sliderbackgroundtextbox1;
                sliderbackgroundtextbox[1] = mwd.sliderbackgroundtextbox1;
                sliderbackgroundtextbox[2] = mwd.sliderbackgroundtextbox1;
                SetEvents();
                WriteVideoSourceToCombobox();
                DrawPixelArray();
            }
            mwd.Width = mwd.Width + 750;
            mwd.Height = mwd.Height;
            mwd.MeasureFieldCanvas.Visibility = Visibility.Visible;
            uICam = new UICam();
        }
        /*
         * Delets the measurefield
         */
        public void DeleteField()
        {
            if (normal == false) uICam = null;
            normal = false;
            calib = false;
            sequence = false;
            mwd.Width = mwd.Width - 750;
            mwd.Height = mwd.Height;
            mwd.MeasureFieldCanvas.Visibility = Visibility.Hidden;
            mwd.PlotImg.Source = null;
            mwd.PlotGray.Fill = Brushes.White;
            mwd.PlotAvrageRGB.Fill = Brushes.White;
            mwd.startSequenceButton.IsEnabled = false;
            mwd.stopSequenceButton.IsEnabled = false;
        }


        /*
         * Set the events
         */
        private void SetEvents()
        {
            mwd.comboBoxVideoSource.DropDownClosed += new System.EventHandler(comboBoxVideoSource_DropDown);
            mwd.comboBoxResolution.DropDownClosed += new System.EventHandler(comboBoxResolution_DropDown);
            mwd.SliderExposure.ValueChanged += new RoutedPropertyChangedEventHandler<double>(SliderExposure_ValueChanged);
            mwd.selectcamButton.Click += new System.Windows.RoutedEventHandler(SelectCameraButtonEvent);
            mwd.autoBrightnessSubtraction.Checked += new RoutedEventHandler(AutoModusChecked);
            mwd.grayBrightnessSubtraction.Checked += new RoutedEventHandler(Brightnessmoduschanged);
            mwd.rgbBrightnessSubtraction.Checked += new RoutedEventHandler(Brightnessmoduschanged);
            mwd.mirrorH.Checked += new RoutedEventHandler(MirrorChangedEvent);
            mwd.mirrorH.Unchecked += new RoutedEventHandler(MirrorChangedEvent);
            mwd.mirrorV.Checked += new RoutedEventHandler(MirrorChangedEvent);
            mwd.mirrorV.Unchecked += new RoutedEventHandler(MirrorChangedEvent);
            mwd.startSequenceButton.Click += new System.Windows.RoutedEventHandler(startSequenceButtonMouseDown);
            mwd.stopSequenceButton.Click += new System.Windows.RoutedEventHandler(stopSequenceButtonMouseDown);
            mwd.SkipFrameCounttextbox.KeyUp += new KeyEventHandler(SkipFrameCountChange);
            mwd.SkipFrameCounttextbox.KeyUp += new KeyEventHandler(steptextboxChange);
            mwd.repaintbutton.Click += new System.Windows.RoutedEventHandler(RepaintArray);
            mwd.calibbutton.Click += new System.Windows.RoutedEventHandler(CalibrationMode);
            mwd.savebutton.Click += new System.Windows.RoutedEventHandler(SaveMeasureButtonEvent);
            mwd.mingrayvaluetextbox.KeyUp += new KeyEventHandler(mingrayvalueChanged);
            mwd.loadbutton.Click += new System.Windows.RoutedEventHandler(LoadcalibbuttonEvent);
            sliderbackgroundtextbox[0].KeyUp += new KeyEventHandler(Slider_TextboxChange1);
            sliderbackground[0].ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderbackground_ValueChanged1);
            sliderbackgroundtextbox[1].KeyUp += new KeyEventHandler(Slider_TextboxChange2);
            sliderbackground[1].ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderbackground_ValueChanged2);
            sliderbackgroundtextbox[2].KeyUp += new KeyEventHandler(Slider_TextboxChange3);
            sliderbackground[2].ValueChanged += new RoutedPropertyChangedEventHandler<double>(sliderbackground_ValueChanged3);
            mwd.topleftboxx.KeyUp += new KeyEventHandler(ReducedAreatopleftchangex);
            mwd.buttomrightboxx.KeyUp += new KeyEventHandler(ReducedAreabuttomrightchangex);
            mwd.topleftboxy.KeyUp += new KeyEventHandler(ReducedAreatopleftchangey);
            mwd.buttomrightboxy.KeyUp += new KeyEventHandler(ReducedAreabuttomrightchangey);
            mwd.linethicknessbox.KeyUp += new KeyEventHandler(ReducedArealinethicknessboxchange);
            mwd.Startwavelength.KeyUp += new KeyEventHandler(Ulbrichtvalueschanged);
            mwd.Endwavelength.KeyUp += new KeyEventHandler(Ulbrichtvalueschanged);
            mwd.Timerpixeldelay.KeyUp += new KeyEventHandler(Ulbrichtvalueschanged);
            mwd.Startsweepvoltageedit.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
            mwd.Endsweepvoltageedit.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
            mwd.xSweepvoltageedit.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
            mwd.ySweepvoltageedit.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
            mwd.starSweep.Click += new System.Windows.RoutedEventHandler(Startsweep); 
            mwd.CurrentShow.Click += new System.Windows.RoutedEventHandler(CurrentShowClick);
            mwd.lowercurrent.KeyUp += new KeyEventHandler(currentlimitchanged);
            mwd.highercurrent.KeyUp += new KeyEventHandler(currentlimitchanged);
            mwd.Startsweepvoltageedit2.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
            mwd.Endsweepvoltageedit2.KeyUp += new KeyEventHandler(Sweepvolatgechanged);
        }
        /*
         * Creats the pixelarray with the textarray
         */
        public void DrawPixelArray()
        {
            //---------------------------------------------------------------------------------------------------------------
            Canvas subsubCanvas = new Canvas();
            subsubCanvas.Height = panelsizey;
            subsubCanvas.Width = panelsizex;
            mwd.MeasureFieldCanvas.Children.Add(subsubCanvas);
            Canvas.SetLeft(subsubCanvas, 200 + imgwidth + 10);
            Canvas.SetTop(subsubCanvas, 0);
            // Creat subpanel for the pixelarray and the textarray for the gray values
            rectimg = new System.Windows.Shapes.Rectangle[ColCount, RowCount];
            textarray = new TextBlock[ColCount, RowCount];
            Border myBorder = new Border();
            myBorder.Height = panelsizey + 4;
            myBorder.Width = panelsizex + 4;
            myBorder.BorderThickness = new Thickness(0.3);
            myBorder.BorderBrush = System.Windows.Media.Brushes.Black;
            subsubCanvas.Children.Add(myBorder);
            //---------------------------------------------------------------------------------------------------------------
            // Creat rectangle array and textarray
            //---------------------------------------------------------------------------------------------------------------

            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    //---------------------------------------------------------------------------------------------------------------
                    // rectarray
                    rectimg[i, j] = new System.Windows.Shapes.Rectangle();
                    rectimg[i, j].Width = (((subsubCanvas.Width) / ColCount) - pixeldistance);
                    rectimg[i, j].Height = (((subsubCanvas.Height) / RowCount) - pixeldistance);
                    subsubCanvas.Children.Add(rectimg[i, j]);
                    Canvas.SetLeft(rectimg[i, j], (i * (subsubCanvas.Width / ColCount)) + 3);
                    Canvas.SetTop(rectimg[i, j], (j * (subsubCanvas.Height / RowCount)) + 3);
                    Canvas.SetZIndex(rectimg[i, j], 1);
                    //---------------------------------------------------------------------------------------------------------------
                    // textarray
                    textarray[i, j] = new TextBlock();
                    textarray[i, j].Width = (((subsubCanvas.Width) / ColCount) - pixeldistance);
                    textarray[i, j].Height = (((subsubCanvas.Height) / RowCount) - pixeldistance);
                    textarray[i, j].HorizontalAlignment = HorizontalAlignment.Left;
                    textarray[i, j].VerticalAlignment = VerticalAlignment.Top;
                    subsubCanvas.Children.Add(textarray[i, j]);
                    Canvas.SetLeft(textarray[i, j], (i * (subsubCanvas.Width / ColCount))+5);
                    Canvas.SetTop(textarray[i, j], (j * (subsubCanvas.Height / RowCount))+2);
                    Canvas.SetZIndex(textarray[i, j], 1);
                }
            }
        }
        /*
        * Clears the color of rectangle and the text value
        */
        public void ClearArray()
        {
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    rectimg[i, j].Fill = System.Windows.Media.Brushes.White;
                    textarray[i, j].Text = "";
                }
            }
        }
        /*
        * Sets enable property,(button is clickable) 
        */
        private void SetButtonIsEnabled(bool _statusstart, bool _statusstop, bool _statusselect, bool _repaintstatus, bool _calibstatus)
        {
            if (mwd.startSequenceButton != null) // only if the buttons exist
            {
                mwd.startSequenceButton.IsEnabled = _statusstart;
                mwd.starSweep.IsEnabled = _statusstart;
                mwd.stopSequenceButton.IsEnabled = _statusstop;
                mwd.selectcamButton.IsEnabled = _statusselect;
                mwd.repaintbutton.IsEnabled = _repaintstatus;
                mwd.savebutton.IsEnabled = _repaintstatus;
                mwd.calibbutton.IsEnabled = _calibstatus;
            }
        }
        /*
        * Enables/Disables user input
        */
        public void StartStopUserDrawInputs(bool _status)
        {
            if (_status == true)
            {
                SetPixels(false);
                toolbox.SetDefaultDrawTool();
            }
            mwd.SkipFrameCounttextbox.IsReadOnly = !_status;
            mwd.ClearButton.IsEnabled = _status;
            mwd.SelectAllButton.IsEnabled = _status;
            toolbox.ChangeActivateTools(_status);
            uIPixelArrayGrid.ChangeActivateArray(_status);
        }
        /*
         * Writes available videosources to videosoourcecombobox
         */
        private void WriteVideoSourceToCombobox()
        {
            dsDevice = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            for (int i = 0; i < dsDevice.Length; i++)
            {
                mwd.comboBoxVideoSource.Items.Add(i + " " + dsDevice[i].Name);
                /* 
                 * Show every videosource thats ever been used. if resolution of the videosource cant be read the videosource isnt 
                 * connected to the pc and will be removed
                 */
                if (GetAllAvailableResolution(dsDevice[i])[0] == "Error")
                {
                    mwd.comboBoxVideoSource.Items.Remove(i + " " + dsDevice[i].Name);
                }
            }
            mwd.comboBoxVideoSource.Items.Add("Ulbricht");
            mwd.comboBoxVideoSource.Items.Add("Short Circuit");
        }
        /*
        * Returns resolutionlist of videodevice
        */
        private List<string> GetAllAvailableResolution(DsDevice vidDev)
        {
            try
            {
                int hr, bitCount = 0;

                IBaseFilter sourceFilter = null;

                var m_FilterGraph2 = new FilterGraph() as IFilterGraph2;
                hr = m_FilterGraph2.AddSourceFilterForMoniker(vidDev.Mon, null, vidDev.Name, out sourceFilter);
                var pRaw2 = DsFindPin.ByCategory(sourceFilter, PinCategory.Capture, 0);
                var AvailableResolutions = new List<string>();

                VideoInfoHeader v = new VideoInfoHeader();
                IEnumMediaTypes mediaTypeEnum;
                hr = pRaw2.EnumMediaTypes(out mediaTypeEnum);

                AMMediaType[] mediaTypes = new AMMediaType[1];
                IntPtr fetched = IntPtr.Zero;
                hr = mediaTypeEnum.Next(1, mediaTypes, fetched);

                while (fetched != null && mediaTypes[0] != null)
                {
                    Marshal.PtrToStructure(mediaTypes[0].formatPtr, v);
                    if (v.BmiHeader.Size != 0 && v.BmiHeader.BitCount != 0)
                    {
                        if (v.BmiHeader.BitCount > bitCount)
                        {
                            AvailableResolutions.Clear();
                            bitCount = v.BmiHeader.BitCount;
                        }
                        AvailableResolutions.Add(v.BmiHeader.Width + "x" + v.BmiHeader.Height);
                    }
                    hr = mediaTypeEnum.Next(1, mediaTypes, fetched);
                }
                return AvailableResolutions;
            }
            catch (Exception)
            {
                List<string> ret = new List<string>();
                ret.Add("Error");
                return ret;
            }
        }
    }
}
