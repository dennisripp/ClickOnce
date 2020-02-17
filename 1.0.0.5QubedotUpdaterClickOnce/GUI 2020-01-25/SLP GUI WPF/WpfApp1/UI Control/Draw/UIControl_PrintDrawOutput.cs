using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    partial class UIControl
    {
        public void DrawOutput()
        {
            DrawTool _CurrentDrawTool = toolbox.GetCurrentDrawTool();
            Modes.DrawOutput _CurrentOutputMode = _CurrentDrawTool.GetOutputMode();
            switch (_CurrentOutputMode)
            {
                case Modes.DrawOutput.Pixel: DrawOutputFrame(); break;
                case Modes.DrawOutput.Frame: DrawOutputFrame(); break;
            }
        }

        public void DrawOutputOnPixel()
        {
            if (drawActionOutput.GetPixelReadyToSend())
            {
                uint _col = drawActionOutput.GetCol();
                uint _row = drawActionOutput.GetRow();
                PrintChanges(_col, _row);
                drawActionOutput.SetPixelReadyToSend(false);
                bool[,] _frame = uIPixelArrayGrid.GetPixelStatus();
                frameStorage.AddFrame(_frame);
            }
        }

        public void DrawOutputFrame()
        {
            if (drawActionOutput.GetFrameReadyToRead())
            {
                ResetTmpValues();
                SetAllPixelColors(DSFMode);
                bool[,] _DrawOutputFrame = drawActionOutput.GetOutputFrame();
                drawActionOutput.SetFrameReadyToRead(false);
                for (uint _col = 0; _col < ColCount; _col++)
                {
                    for (uint _row = 0; _row < RowCount; _row++)
                    {
                        if (_DrawOutputFrame[_col, _row])
                        {
                            PrintChanges(_col, _row);
                        }
                    }
                }
                if (drawActionOutput.GetFrameReadyToSend())
                {
                    RefreshChangesDSF();
                    drawActionOutput.SetFrameReadyToSend(false);
                    bool[,] _frame = uIPixelArrayGrid.GetPixelStatus();
                    frameStorage.AddFrame(_frame);
                }
            }
        }
    }
}
