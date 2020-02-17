using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace WpfApp1
{
    public class Ulbricht
    {
        private static int delay = 300;
        private InputSimulator sim = new InputSimulator();
        public Ulbricht()
        {

        }
        /*
         * Opens by clicks in the SpectrILight III Softeware the "Timeline" and pasts the Numberscans, Time for each scan and the Path to 
         * the right editfield. If resolution changed edit the positions.
         */
        public void clicks(String _NumberScans, String _TimeScans, String _Path)
        {
            // Open Timeline
            LeftMouseClick(1700-1280, 10);
            sim.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_T);
            Thread.Sleep(500);
            //Data Format
            LeftMouseClick(1950 - 1280, 490);
            //Number of Scans
            LeftMouseClick(1650 - 1280, 490);
            LeftMouseTrippleClick(1800 - 1280, 490); // Editfield
            sim.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            sim.Keyboard.TextEntry(_NumberScans); //Number of Scans
            //Time interval
            LeftMouseClick(1745 - 1280, 560);
            LeftMouseTrippleClick(1700 - 1280, 575); // Editfield
            sim.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            sim.Keyboard.TextEntry(_TimeScans); // Time between Scans
            //Path
            LeftMouseTrippleClick(2050 - 1280, 370);
            sim.Keyboard.KeyPress(VirtualKeyCode.DELETE);
            sim.Keyboard.TextEntry(_Path); // Path
            //Start                            
            LeftMouseClick(2070 - 1280, 665);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;


        //This simulates a left mouse click
        public void LeftMouseDown(int xpos, int ypos)
        {
            Thread.Sleep(delay);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
        }
        public void LeftMouseUp(int xpos, int ypos)
        {
            Thread.Sleep(delay);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }

        public void LeftMouseClick(int xpos, int ypos)
        {
            Thread.Sleep(delay);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        public void LeftMouseTrippleClick(int xpos, int ypos)
        {
            Thread.Sleep(delay);
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
    }
}
