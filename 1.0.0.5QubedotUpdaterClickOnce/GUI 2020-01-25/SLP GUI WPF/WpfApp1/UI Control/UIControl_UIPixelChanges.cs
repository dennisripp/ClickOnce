using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    partial class UIControl
    {
        //Writes the change of certain pixel in DSP mode (Write ArrayPixel, change Button color, Send message to driver if "connected"
        public void DSPOnePixel(uint _col, uint _row)
        {
            bool on = GetPixelTmpStatus(_col, _row);

            WritePixel(_col, _row, on);
            SetPixelColor(_col, _row, DSPMode);
            bool[,] PixelArrayTmp = GetPixelArrayTmp();
            if (USBConnected())
            {
                uSBDriver.SendFrame(PixelArrayTmp);
            } else
            {
                NoDeviceConnected();
            }
        }

        //Writes all pixel changes in DSP Mode
        public void DSPAllPixel()
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    DSPOnePixel(col, row);
                }
            }
        }

        //Writes the change of certain pixel in DSP mode (Write Button Color)
        public void DSFOnePixel(uint _col, uint _row)
        {
            SetPixelColor(_col, _row, DSFMode);
        }

        //Writes all pixel changes in DSF Mode
        public void DSFAllPixel()
        {
            SetAllPixelColors(DSFMode);
        }

        //Refreshes the changes in DSF Mode (Write PixelArray with tmp values, Write all Button Colors, Send DSF Message to driver if "connected")
        public void RefreshChangesDSF()
        {
            bool[,] PixelArrayTmp = GetPixelArrayTmp();
            WriteAllPixels(PixelArrayTmp);
            SetAllPixelColors(DSFMode);
            if (USBConnected())
            {
                uSBDriver.SendFrame(PixelArrayTmp);
            } else
            {
                NoDeviceConnected();
            }            
        }
    }
}
