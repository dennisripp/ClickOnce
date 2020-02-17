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
    class PrintTool : Tool
    {
        private Modes.PrintMode Mode;

        public PrintTool(string _filename, Modes.PrintMode _Mode, List<Tool> _toolBoxList)
            : base(_filename, _toolBoxList)
        {
            Mode = _Mode;
        }

        public Modes.PrintMode GetPrintMode()
        {
            return Mode;
        }
    }
}
