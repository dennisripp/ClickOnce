using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.InteropServices;

namespace WpfApp1
{
    partial class Toolbox
    {
        public DrawTool CurrentDrawTool;
        private PrintTool CurrentPrintTool;

        private List<Tool> toolBoxList = new List<Tool>();
        public DrawTool Normal, Square, Draw, Line, Circle;//, Select;
        private PrintTool Invert, Set, Delete;
        private ActionTool Undo, Reundo,Tilt, MirrorH, MirrorV;
        public DrawToolbox drawToolbox;
        public Toolbox()
        {
            //DrawTools init
            Normal = new DrawTool("normal", Modes.ToolMode.Normal, Modes.DrawOutput.Pixel, toolBoxList);
            Draw = new DrawTool("draw", Modes.ToolMode.Draw, Modes.DrawOutput.Pixel, toolBoxList);
            Line = new DrawTool("line", Modes.ToolMode.Line, Modes.DrawOutput.Frame, toolBoxList);
            Circle = new DrawTool("circle", Modes.ToolMode.Circle, Modes.DrawOutput.Frame, toolBoxList);
            Square = new DrawTool("square", Modes.ToolMode.Square, Modes.DrawOutput.Frame, toolBoxList);
            //Select = new DrawTool("select", Modes.ToolMode.Select, Modes.DrawOutput.Frame, toolBoxList); not working

            //PrintTools init
            Invert = new PrintTool("inv", Modes.PrintMode.Invert, toolBoxList);
            Set = new PrintTool("set", Modes.PrintMode.Set, toolBoxList);
            Delete = new PrintTool("del", Modes.PrintMode.Delete, toolBoxList);

            //Tools init
            Undo = new ActionTool("undo", Modes.Action.Undo, toolBoxList);
            Reundo = new ActionTool("reundo", Modes.Action.Reundo, toolBoxList);
            Tilt = new ActionTool("tilt", Modes.Action.Tilt, toolBoxList);
            MirrorH = new ActionTool("Mih", Modes.Action.MirrorH, toolBoxList);
            MirrorV= new ActionTool("Miv", Modes.Action.MirrorV, toolBoxList);
            CurrentDrawTool = Normal;
            CurrentPrintTool = Invert;            
        }
        /*
         *  sets tools to be clickable or not, needed for the measuerment
         */
        public void ChangeActivateTools(bool _status)
        {
            Normal.img.IsEnabled = _status;
            Draw.img.IsEnabled = _status;
            Line.img.IsEnabled = _status;
            Circle.img.IsEnabled = _status;
            Square.img.IsEnabled = _status;
            //Select.img.IsEnabled = _status;
            Undo.img.IsEnabled = _status;
            Reundo.img.IsEnabled = _status;
            Tilt.img.IsEnabled = _status;
            MirrorH.img.IsEnabled = _status;
            MirrorV.img.IsEnabled = _status;
        }
        public void DrawToolSelection(Canvas _myCanvas)
        {
            drawToolbox = new DrawToolbox(_myCanvas, toolBoxList, 432, 50);
            drawToolbox.DrawToolBar(CurrentDrawTool, CurrentPrintTool);
        }
        
        public DrawTool GetCurrentDrawTool()
        {
            return CurrentDrawTool;
        }

        public PrintTool GetCurrentPrintTool()
        {
            return CurrentPrintTool;
        }

        public void SetCurrentDrawTool(DrawTool _newDrawTool)
        {
            CurrentDrawTool = _newDrawTool;
        }
        public void SetCurrentPrintTool(PrintTool _newPrintTool)
        {
            CurrentPrintTool = _newPrintTool;
        }

        public List<Tool> GetToolBox()
        {
            return toolBoxList;
        }

        public void MouseDownMainWindow(MouseButtonEventArgs e)
        {
            drawToolbox.MouseDownMainWindow(e);
        }
        public void SetDefaultDrawTool()
        {
            SetCurrentDrawTool(Normal);
            drawToolbox.SetCurrentDrawToolPosition(Normal);
        }
    }
}
