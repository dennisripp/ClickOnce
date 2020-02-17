using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
namespace WpfApp1
{
    partial class UIControl
    {
        //Sets all pixels to "on" and writes all pixel changes
        public void SetPixels(bool on)
        {
            SetAllPixels(on);
            DSFAllPixel();
            RefreshChangesDSF();
            frameStorage.AddFrame(uIPixelArrayGrid.GetPixelStatus());
        }

        //Toggles one pixel of PixelArrayTmp and then writes the changes of the certain Mode
        public void ToggleOnePixel(uint _col, uint _row)
        {
            SetTogglePixel(_col, _row);
            DSFOnePixel(_col, _row);
        }

        //Set one pixel of PixelArrayTmp and then writes the changes of the certain Mode
        public void SetOnePixel(uint _col, uint _row, bool on)
        {
            SetPixel(_col, _row, on);
            DSFOnePixel(_col, _row);
        }

        //Enables all buttons
        public void SetEnableAllPixel(bool on)
        {
            //enable all pixel
        }

        //Loads an external frame
        public void LoadPixelArray(bool[,] _PixelArray)
        {
            SetAllPixels(_PixelArray);
            RefreshChangesDSF();
        }
        public void PrintChanges(uint _col, uint _row)
        {
            PrintTool _CurrentPrintTool = toolbox.GetCurrentPrintTool();
            Modes.PrintMode _CurrentPrintMode = _CurrentPrintTool.GetPrintMode();
            switch (_CurrentPrintMode)
            {
                case Modes.PrintMode.Invert: ToggleOnePixel(_col, _row); break;
                case Modes.PrintMode.Set: SetOnePixel(_col, _row, true); break;
                case Modes.PrintMode.Delete: SetOnePixel(_col, _row, false); break;
            }
        }
        public void safemoduschanged()
        {
            if(safemodus == true)
            {
                mwd.SafeLabel.Content = "UNSafe Modus";
                mwd.SafeLabel.Background = new SolidColorBrush(Colors.Red);
                //CHANGE MODUS HERE
            }
            else
            {
                mwd.SafeLabel.Content = "Safe Modus";
                mwd.SafeLabel.Background = new SolidColorBrush(Colors.Honeydew);
            }
        }
    }
}