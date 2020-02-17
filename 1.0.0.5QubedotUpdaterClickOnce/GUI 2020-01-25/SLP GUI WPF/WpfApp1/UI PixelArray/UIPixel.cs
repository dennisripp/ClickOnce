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
using System.Windows.Threading;
using System.Reflection;

namespace WpfApp1
{
    class UIPixel
    {
        private Rectangle rect;
        private Boolean status, tempstatus;
        public UIPixel(Rectangle _rect, bool _status)
        {
            rect = _rect;
            status = _status;
            tempstatus = _status;
        }
        public Boolean GetStatus()
        {
            return (status);
        }
        public void SetStatus(Boolean __status)
        {
            status = __status;
        }
        public void SetColor(Brush _brush, MainWindow _mwd)
        {
            rect.Fill = _brush;
    }
        public void SetTmpStatus(bool _tempstatus)
        {
            tempstatus = _tempstatus;
        }

        public bool GetTmpStatus()
        {
            return tempstatus;
        }
        public Brush GetColor()
        {
            return (rect.Fill);
        }
        public double GetLeft()
        {
            return ((Canvas.GetLeft(rect)));
        }
        public double GetTop()
        {
            return ((Canvas.GetTop(rect)));
        }
        public double GetWidth()
        {
            return (rect.Width);
        }
        public double GetHeight()
        {
            return (rect.Height);
        }
        public Rectangle GetRect()
        {
            return rect;
        }
    }
}
