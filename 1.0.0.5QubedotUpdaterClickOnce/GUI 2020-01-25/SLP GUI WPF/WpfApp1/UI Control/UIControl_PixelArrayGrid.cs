using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1
{
    partial class UIControl
    {
        public void SetTogglePixel(uint _col, uint _row)
        {
            uIPixelArrayGrid.TogglePixelTmpStatus(_col, _row);
        }

        //Sets a single pixel of PixelArrayTmp
        public void SetPixel(uint _col, uint _row, bool on)
        {
            uIPixelArrayGrid.SetPixelTmpStatus(_col, _row, on);
        }

        //Sets a certain Pixel of PixelArray to "on"
        public void WritePixel(uint _col, uint _row, bool on)
        {
            uIPixelArrayGrid.SetPixelStatus(_col, _row, on);
        }

        //Sets all Pixels of PixelArray to the particular Pixel of the Input Array
        public void WriteAllPixels(bool[,] _PixelArray)
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    bool on = _PixelArray[col, row];
                    WritePixel(col, row, on);
                }
            }
        }

        public void SetAllPixels(bool[,] _PixelArray)
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    bool on = _PixelArray[col, row];
                    SetPixel(col, row, on);
                }
            }
        }

        //Sets all Pixels of PixelArray to "on"
        public void WriteAllPixels(bool on)
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    WritePixel(col, row, on);
                }
            }
        }

        //Sets all Pixels of PixelArrayTmp to "on"
        public void SetAllPixels(bool on)
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    SetPixel(col, row, on);
                }
            }
        }

        //PixelArrayTmp will be resetted to PixelArray
        public void ResetTmpValues()
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    bool Status = uIPixelArrayGrid.GetPixelStatus(col, row);
                    uIPixelArrayGrid.SetPixelTmpStatus(col, row, Status);
                }
            }
        }

        //Sets all Button Colors (as an Examble if there was a change of more than just one pixel)
        public void SetAllPixelColors(Modes.Command CurrentMode)
        {
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    SetPixelColor(col, row, CurrentMode);
                }
            }
        }

        //Sets Button Color of one pixel depanding on current Communication Mode
        public void SetPixelColor(uint _col, uint _row, Modes.Command CurrentMode)
        {
            switch (CurrentMode)
            {
                case Modes.Command.DIRECTSETPIXEL: SetPixelColorDSP(_col, _row); break;
                case Modes.Command.DIRECTSETFRAME: SetPixelColorDSF(_col, _row); break;
            }
        }

        //Sets Button Color of one pixel "Direct Set Pixel" Mode
        private void SetPixelColorDSP(uint _col, uint _row)
        {
            bool PixelOn = uIPixelArrayGrid.GetPixelStatus(_col, _row);

            if (PixelOn)
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, blue);
            }
            else
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, grey);
            }
        }

        //Sets Button Color of one pixel "Direct Set Frame" Mode
        private void SetPixelColorDSF(uint _col, uint _row)
        {
            bool CurrentPixelOn = uIPixelArrayGrid.GetPixelStatus(_col, _row);
            bool NewPixelOn = uIPixelArrayGrid.GetPixelTmpStatus(_col, _row);

            if (CurrentPixelOn && NewPixelOn)
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, blue);
            }
            else if (!CurrentPixelOn && !NewPixelOn)
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, grey);
            }
            else if (!CurrentPixelOn && NewPixelOn)
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, green);
            }
            else if (CurrentPixelOn && !NewPixelOn)
            {
                uIPixelArrayGrid.SetPixelColor(_col, _row, red);
            }
        }

        //returns PixelArrayTmp
        public bool[,] GetPixelArrayTmp()
        {
            bool[,] PixelArrayTmp = new bool[RowCount, ColCount];
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    PixelArrayTmp[col, row] = uIPixelArrayGrid.GetPixelTmpStatus(col, row);
                }
            }
            return PixelArrayTmp;
        }

        public bool GetPixelTmpStatus(uint _col, uint _row)
        {
            return uIPixelArrayGrid.GetPixelTmpStatus(_col, _row);
        }

        public bool[,] GetPixelArray()
        {
            bool[,] _pixelArray = new bool[ColCount,RowCount];
            for (uint row = 0; row < RowCount; row++)
            {
                for (uint col = 0; col < ColCount; col++)
                {
                    _pixelArray[col, row] = GetPixelStatus(col, row);
                }
            }
            return _pixelArray;
        }

        public bool GetPixelStatus(uint _col, uint _row)
        {
            return uIPixelArrayGrid.GetPixelStatus(_col, _row);
        }
    }
}
