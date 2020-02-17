using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Drawing;

namespace WpfApp1
{
    class DrawTool : Tool
    {
        private Modes.ToolMode Mode;
        private Modes.DrawOutput OutputMode;

        public DrawTool(string _filename, Modes.ToolMode _Mode, Modes.DrawOutput _OutputMode, List<Tool> _toolBoxList)
            : base(_filename, _toolBoxList)
        {
            Mode = _Mode;
            OutputMode = _OutputMode;
        }

        public Modes.ToolMode GetDrawMode()
        {
            return Mode;
        }

        public Modes.DrawOutput GetOutputMode()
        {
            return OutputMode;
        }
    }
}
