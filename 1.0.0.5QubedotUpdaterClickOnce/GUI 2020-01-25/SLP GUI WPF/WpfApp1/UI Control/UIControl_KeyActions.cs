using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WpfApp1
{
    partial class UIControl
    {

        private bool ctrl_pressed, shift_pressed;
        public void Ctrl_Down()
        {
            ctrl_pressed = true;
        }        

        public void Ctrl_Up()
        {
            ctrl_pressed = false;
        }

        public void Z_Down()
        {
            if (ctrl_pressed)
            {
                if (shift_pressed)
                {
                    CTRL_Shift_Pressed();
                } else
                {
                    CTRL_Z_Pressed();
                }                
            }
        }

        public void Z_Up()
        {
        }

        public void CTRL_Z_Pressed()
        {
            LoadPixelArray(frameStorage.GetPreviousFrameAndSetAsCurrent());
        }

        public void CTRL_Shift_Pressed()
        {
            LoadPixelArray(frameStorage.GetNextFrameAndSetAsCurrent());
        }

        public void Shift_Down()
        {
            shift_pressed = true;
        }

        public void Shift_Up()
        {
            shift_pressed = false;
        }

        public void MouseDownMainWindow(MouseButtonEventArgs e)
        {
            toolbox.MouseDownMainWindow(e);
        }
    }
}
