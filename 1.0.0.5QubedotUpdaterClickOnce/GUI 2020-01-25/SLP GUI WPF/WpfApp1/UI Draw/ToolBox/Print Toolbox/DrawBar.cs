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
    abstract class DrawBar
    {
        protected int ButtonWidth = 25;
        protected int ButtonHeight = 25;
        protected List<Tool> toolBoxList;
        protected double PosX, PosY;
        public DrawBar(List<Tool> _toolBoxList, double _PosX, double _PosY)
        {
            toolBoxList = _toolBoxList;
            PosX = _PosX;
            PosY = _PosY;
        }

        protected void DrawToolList(Tool _currentTool, Canvas _subCanvas)
        {
            int ToolNumber = 0;
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                Type toolType = toolBoxList[i].GetType();
                Type currentToolType = _currentTool.GetType();
                if ((toolType == currentToolType) && toolBoxList[i] != _currentTool)
                {
                    Image _img = _tool.img;
                    Canvas.SetLeft(_img, 0);
                    Canvas.SetTop(_img, ButtonHeight * ToolNumber);
                    _subCanvas.Children.Add(_img);
                    ToolNumber++;
                }
            }
        }

        protected void RemoveToolList(Tool _currentTool, Canvas _subCanvas)
        {
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                Type toolType = toolBoxList[i].GetType();
                Type currentToolType = _currentTool.GetType();
                if ((toolType == currentToolType) && toolBoxList[i] != _currentTool)
                {
                    Image _img = _tool.img;
                    _subCanvas.Children.Remove(_img);
                }
            }
        }

        protected int GetCountOfThisType(Type _type)
        {
            int count = 0;
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                if (_type == toolBoxList[i].GetType())
                {
                    count++;
                }
            }
            return count;
        }
    }
}
