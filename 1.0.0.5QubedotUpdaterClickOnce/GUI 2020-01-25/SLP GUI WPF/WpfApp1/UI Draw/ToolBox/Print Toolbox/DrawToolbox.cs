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
using System.Drawing;

namespace WpfApp1
{
    class DrawToolbox : DrawBar
    {
        private Canvas myCanvas, subCanvas;
        private SelectionBar selectionBar;
        private DrawTool currentDrawTool;
        private PrintTool currentPrintTool;
        protected string binpath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


        public DrawToolbox(Canvas _myCanvas, List<Tool> _toolBoxList, int _x, int _y)
            : base(_toolBoxList, _x, _y)
        {
            myCanvas = _myCanvas;
            LoadImages(_toolBoxList);
        }

        private void LoadImages(List<Tool> _toolBoxList)
        {
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                System.Windows.Controls.Image _img = _tool.img;
                _img.Width = ButtonWidth;
                _img.Height = ButtonHeight;
                 ConvertImage((Bitmap)Properties.Resources.ResourceManager.GetObject(_tool.Filename), _img);

                //string s = binpath + @"\Images\";
                //ConvertImage(new Bitmap(s + _tool.Filename), _img);
            }
        }

        private void ConvertImage(System.Drawing.Bitmap bitmap, System.Windows.Controls.Image _img)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();
            _img.Source = bi;
        }

        public void DrawToolBar(DrawTool _currentDrawTool, PrintTool _currentPrintTool)
        {
            currentDrawTool = _currentDrawTool;
            currentPrintTool = _currentPrintTool;

            subCanvas = new Canvas();

            int height = ButtonHeight*4;
            int widht = ButtonWidth*1;

            subCanvas.Height = height;
            subCanvas.Width = widht;
            Canvas.SetLeft(subCanvas, PosX);
            Canvas.SetTop(subCanvas, PosY);
            myCanvas.Children.Add(subCanvas);

            Border myBorder = new Border();
            myBorder.Height = height + 1;
            myBorder.Width = widht + 1;
            myBorder.BorderThickness = new Thickness(0.3);
            myBorder.BorderBrush = System.Windows.Media.Brushes.Black;
            subCanvas.Children.Add(myBorder);

            DrawToolBarList(_currentDrawTool, _currentPrintTool);
        }

        public void ReplaceItemImage(Tool _currentTool, Tool _nextTool)
        {
            System.Windows.Controls.Image _currentImage = _currentTool.img;
            double _x = Canvas.GetLeft(_currentImage);
            double _y = Canvas.GetTop(_currentImage);
            myCanvas.Children.Remove(_currentImage);

            System.Windows.Controls.Image _nextImage = _nextTool.img;
            Canvas.SetLeft(_nextImage, _x);
            Canvas.SetTop(_nextImage, _y);
            myCanvas.Children.Add(_nextImage);
        }

        private void DrawToolBarList(DrawTool _currentDrawTool, PrintTool _currentPrintTool)
        {
            int ToolNumber = 0;
            for (int i = 0; i < toolBoxList.Count; i++)
            {
                Tool _tool = toolBoxList[i];
                if (_tool == _currentDrawTool || _tool == _currentPrintTool || _tool is ActionTool)
                {
                    System.Windows.Controls.Image _img = _tool.img;
                    Canvas.SetLeft(_img, 0);
                    Canvas.SetTop(_img, ButtonHeight * ToolNumber);
                    subCanvas.Children.Add(_img);
                    ToolNumber++;
                }
                if(_tool is DrawTool || _tool is PrintTool)
                {
                    int index = i;
                    System.Windows.Controls.Image _img = _tool.img;
                    _img.MouseDown += delegate (object sender, MouseButtonEventArgs e) { MouseDownImage(sender, e, index); };
                }
            }
        }

        public void MouseDownImage(object sender, System.Windows.Input.MouseEventArgs e, int index)
        {
                Tool _clickedTool = toolBoxList[index];
                if (_clickedTool == currentDrawTool || _clickedTool == currentPrintTool)
                {
                    double _x = Canvas.GetLeft((System.Windows.Controls.Image)sender) + ButtonWidth + 1;
                    double _y = Canvas.GetTop((System.Windows.Controls.Image)sender);
                    if (selectionBar == null)
                    {
                        selectionBar = new SelectionBar(toolBoxList, _x, _y, _clickedTool, subCanvas);
                    }
                    else
                    {
                        if (selectionBar.GetOpen())
                        {
                            selectionBar.Close();
                            if (selectionBar.GetTool() != _clickedTool)
                            {
                                selectionBar = new SelectionBar(toolBoxList, _x, _y, _clickedTool, subCanvas);
                            }
                        }
                        else
                        {
                            selectionBar = new SelectionBar(toolBoxList, _x, _y, _clickedTool, subCanvas);
                        }
                    }
                }
                else
                {
                    Type _toolType = _clickedTool.GetType();
                    if (_toolType == typeof(DrawTool))
                    {
                        System.Windows.Controls.Image _img = currentDrawTool.img;
                        subCanvas.Children.Remove(_img);
                        currentDrawTool = (DrawTool)_clickedTool;
                        selectionBar.Close();
                        _img = currentDrawTool.img;
                        Canvas.SetLeft(_img, 0);
                        Canvas.SetTop(_img, 0);
                        subCanvas.Children.Add(_img);
                    }
                    else if (_toolType == typeof(PrintTool))
                    {
                        System.Windows.Controls.Image _img = currentPrintTool.img;
                        subCanvas.Children.Remove(_img);
                        currentPrintTool = (PrintTool)_clickedTool;
                        selectionBar.Close();
                        _img = currentPrintTool.img;
                        Canvas.SetLeft(_img, 0);
                        Canvas.SetTop(_img, ButtonHeight);
                        subCanvas.Children.Add(_img);
                    }
                }
        }

        public void MouseDownMainWindow(MouseButtonEventArgs e)
        {
            System.Windows.Point p = e.GetPosition(myCanvas);
            double x = p.X;
            double y = p.Y;
            double Start_X = Canvas.GetLeft(subCanvas);
            double Start_Y = Canvas.GetTop(subCanvas);
            double End_X = subCanvas.ActualWidth + Start_X;
            double End_Y = subCanvas.ActualHeight + Start_Y;
            if(!((x < End_X) && (x > Start_X) && (y < End_Y) && (y > Start_Y)))
            {
                if ((selectionBar != null) && selectionBar.GetOpen())
                {
                    selectionBar.Close();
                }
            }
        }
        public void SetCurrentDrawToolPosition(DrawTool _drawtool)
        {
            System.Windows.Controls.Image _img = currentDrawTool.img;
            subCanvas.Children.Remove(_img);
            currentDrawTool = (DrawTool)_drawtool;
            _img = currentDrawTool.img;
            Canvas.SetLeft(_img, 0);
            Canvas.SetTop(_img, 0);
            subCanvas.Children.Add(_img);
        }
    }
}
