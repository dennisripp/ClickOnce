using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace WpfApp1
{
    abstract class Tool
    {
        public System.Windows.Controls.Image img { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public string Filename { get; set; }

        public Tool(string _filename, List<Tool> _toolBoxList)
        {
            img = new System.Windows.Controls.Image(); 
            Filename = _filename;
            _toolBoxList.Add(this);
        }
    }    
}
