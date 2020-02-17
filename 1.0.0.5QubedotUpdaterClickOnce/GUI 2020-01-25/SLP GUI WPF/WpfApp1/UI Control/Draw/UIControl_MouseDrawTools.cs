using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Input;
using System.Windows.Controls;

namespace WpfApp1
{
    partial class UIControl
    {
        public void MouseDownToolsInit()
        {
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                Image i_tool = _tool.img;
                int index = i;
                i_tool.MouseDown += delegate (object sender, MouseButtonEventArgs e) { R_MouseDownTool(sender, e, index); };
            }
        }

        public void R_MouseDownTool(object sender, MouseButtonEventArgs e, int index)
        {
            Tool _tool = toolBoxList[index];
            switch (_tool)
            {
                case PrintTool _printTool:
                    toolbox.SetCurrentPrintTool(_printTool);
                    break;
                case DrawTool _drawTool:
                    toolbox.SetCurrentDrawTool(_drawTool);
                    break;
                case ActionTool _actionTool:
                    switch (_actionTool.GetActionMode())
                    {
                        case Modes.Action.Undo:
                            LoadPixelArray(frameStorage.GetPreviousFrameAndSetAsCurrent());
                            break;
                        case Modes.Action.Reundo:
                            LoadPixelArray(frameStorage.GetNextFrameAndSetAsCurrent());
                            break;
                        case Modes.Action.Tilt:
                            drawAction.Tilt90degree(frameStorage.GetCurrentFrame());//LoadPixelArray(drawAction.Tilt90degree(frameStorage.GetCurrentFrame()));
                            DrawOutput();
                            break;
                        case Modes.Action.MirrorH:
                            drawAction.FrameMirrorH(frameStorage.GetCurrentFrame());
                            DrawOutput();
                            break;
                        case Modes.Action.MirrorV:
                            drawAction.FrameMirrorV(frameStorage.GetCurrentFrame());
                            DrawOutput();
                            break;
                    }
                    break;
            }
            drawAction.ToolChanged();
        }
    }
}
