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
    class SelectionBar : DrawBar
    {
        private Canvas subCanvas, myCanvas;
        private bool Open;
        private Tool currentTool;
        public SelectionBar(List<Tool> _toolBoxList, double _x, double _y, Tool _currentTool, Canvas _myCanvas)
            : base(_toolBoxList, _x, _y)
        {
            int _height = (GetCountOfThisType(_currentTool.GetType())-1)*ButtonHeight;

            currentTool = _currentTool;
            myCanvas = _myCanvas;

            subCanvas = new Canvas();
            subCanvas.Height = _height;
            subCanvas.Width = ButtonWidth;
            Canvas.SetLeft(subCanvas, PosX);
            Canvas.SetTop(subCanvas, PosY);
            _myCanvas.Children.Add(subCanvas);

            Border myBorder = new Border();
            myBorder.Height = _height + 1;
            myBorder.Width = ButtonWidth + 1;
            myBorder.BorderThickness = new Thickness(0.3);
            myBorder.BorderBrush = Brushes.Black;
            subCanvas.Children.Add(myBorder);

            DrawToolList(_currentTool, subCanvas);
            Open = true;
        }

        public void Close()
        {
            RemoveToolList(currentTool, subCanvas);
            myCanvas.Children.Remove(subCanvas);
            Open = false;
        }

        public bool GetOpen()
        {
            return Open;
        }

        public Tool GetTool()
        {
            return currentTool;
        }
    }
}
