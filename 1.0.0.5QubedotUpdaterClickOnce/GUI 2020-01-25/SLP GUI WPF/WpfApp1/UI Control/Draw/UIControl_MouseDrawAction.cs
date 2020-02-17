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
        bool pressed;
        /*
         * Set the click, mouseup/down, leave and over event for every rectangle of the pixelarray and set the leave event for grid.
         * mouseup/down, leave and over are needed for the figure drawings
         */
        public void MouseDrawActionInit(Grid _myGrid, Canvas _subCanvas)
        {
            for (int i = 0; i < ColCount; i++)
            {
                for (int j = 0; j < RowCount; j++)
                {
                    Rectangle r = pixelArray[i, j].GetRect();
                    r.MouseUp += R_MouseUp;
                    r.MouseDown += R_MouseDown;
                    r.MouseMove += delegate (object sender, System.Windows.Input.MouseEventArgs e) { Over(sender, e, _subCanvas); };

                    r.MouseLeave += new System.Windows.Input.MouseEventHandler(Leave); 
                }
            }
            _myGrid.MouseLeave += new System.Windows.Input.MouseEventHandler(Leave);    
            _myGrid.MouseUp += Leave;

        }

        public void Leave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Rectangle)
            {
                drawAction.MouseLeave(); 
            }
            else
            // if mouse is leaving the grid current drawing figure will be stopped
            if (sender is Grid)
            {
                DrawTool CurrentDrawTool = toolbox.GetCurrentDrawTool();
                Modes.ToolMode CurrentToolMode = CurrentDrawTool.GetDrawMode();
                ResetTmpValues();
                SetAllPixelColors(DSFMode);
                drawAction.ToolChangedResetSwitches();
                drawAction.RemoveBorder();
            }
        }

        public void Over(object sender, System.Windows.Input.MouseEventArgs e, Canvas _subCanvas)
        {
            DrawTool CurrentDrawTool = toolbox.GetCurrentDrawTool();
            Modes.ToolMode CurrentToolMode = CurrentDrawTool.GetDrawMode();
            if (CurrentToolMode == Modes.ToolMode.Select)  //executes mouseoverselect if current drawtool is select
            {
                bool[,] _Frame = GetPixelArray();
                drawAction.MouseOverSelect((Rectangle)sender, _Frame);
            }
            else  //executes mouseoverfigure for all other drawtools
            {
                drawAction.SetCurrentFigure(CurrentToolMode);
                drawAction.MouseOverFigure((Rectangle)sender);
            }
            if (pressed)
            {
                DrawOutput();
            }
        }

        public void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pressed = true;
            DrawTool CurrentDrawTool = toolbox.GetCurrentDrawTool();
            Modes.ToolMode CurrentToolMode = CurrentDrawTool.GetDrawMode();
            //PrintTool CurrentPrintTool = toolbox.GetCurrentPrintTool();
            //Modes.PrintMode CurrentPrintMode = CurrentPrintTool.GetPrintMode();
            if (CurrentDrawTool.GetDrawMode() == Modes.ToolMode.Select) 
            {
                bool[,] _Frame = GetPixelArray();
                drawAction.MouseDownSelect((Rectangle)sender, _Frame);  //executes mosuedownselect if current drawtool is select
            }
            else
            {
                drawAction.MouseDownFigure((Rectangle)sender);      //executes mousedownfigure for all other drawtools
            }
            DrawOutput();
        }

        public void R_MouseUp(object sender, MouseButtonEventArgs e)
        {
            pressed = false;
            DrawTool CurrentDrawTool = toolbox.GetCurrentDrawTool();
            Modes.ToolMode CurrentToolMode = CurrentDrawTool.GetDrawMode();
            bool[,] _Frame = GetPixelArray();
            //executes mouseupselect if current drawtool is select
            if (CurrentToolMode == Modes.ToolMode.Select) drawAction.MouseUpSelect((Rectangle)sender, _Frame);
            else
            {   //executes mouseupfigure for all other drawtools
                drawAction.MouseUpFigure((Rectangle)sender, _Frame);
            }
            DrawOutput();
        }
    }
}
