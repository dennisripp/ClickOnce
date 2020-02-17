using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    partial class UIControl
    {
        protected string binpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public void saveBMP()
        {
            bool[,] _currentPixelArrayStatus = GetPixelArray();
            string s = binpath.Substring(0, binpath.Length - 18) + @"\BMPs\";
            frameToBmp.saveSettingAsBmp(_currentPixelArrayStatus, s);
        }

        public void importBMP()
        {
            DrawTool _currentDrawTool = toolbox.GetCurrentDrawTool();
            PrintTool _currentPrintTool = toolbox.GetCurrentPrintTool();
            DrawTool _tmpDrawTool;
            PrintTool _tmpPrintTool;

            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                if (_tool is PrintTool)
                {
                    _tmpPrintTool = (PrintTool)_tool;
                    Modes.PrintMode _printMode = _tmpPrintTool.GetPrintMode();
                    if (_printMode == Modes.PrintMode.Set)
                    {
                        toolbox.SetCurrentPrintTool(_tmpPrintTool);
                        break;
                    }
                }
            }

            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                if (_tool is DrawTool)
                {
                    _tmpDrawTool = (DrawTool)_tool;
                    Modes.DrawOutput _outMode = _tmpDrawTool.GetOutputMode();
                    if (_outMode == Modes.DrawOutput.Frame)
                    {
                        toolbox.SetCurrentDrawTool(_tmpDrawTool);
                        string s = binpath.Substring(0, binpath.Length - 18) + @"\BMPs\";
                        bool[,] _PixelArray = frameToBmp.getSetting(s);
                        SetPixels(false);
                        drawActionOutput.SetOutputFrame(_PixelArray);
                        drawActionOutput.SetFrameReadyToSend(true);
                        DrawOutput();
                        toolbox.SetCurrentDrawTool(_currentDrawTool);
                        toolbox.SetCurrentPrintTool(_currentPrintTool);
                        break;
                    }
                }
            }
        }
    }
}
