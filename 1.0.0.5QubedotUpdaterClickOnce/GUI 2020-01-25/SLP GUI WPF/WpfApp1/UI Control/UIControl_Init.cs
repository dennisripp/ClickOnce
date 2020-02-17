using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
namespace WpfApp1
{
    public partial class UIControl
    { //Oft müssen UI-pixel farben und/oder pixel gesetzt werden und/oder Übertragen werden. Diese Klasse koordiniert diese Aktionen
        private USBDriver uSBDriver;
        private MainWindow mwd;
        private Toolbox toolbox;
        private UIPixelArrayGrid uIPixelArrayGrid;
        private DrawAction drawAction;
        private Output drawActionOutput;
        private List<Tool> toolBoxList;
        private UIPixel[,] pixelArray;
        private FrameStorage frameStorage;
        private FrameToBmp frameToBmp;
        public UICam uICam;
        private Modes.Hardware Hardware;
        public bool connected;
        public bool Drawmode;
        private int RowCount;
        private int ColCount;
        readonly private Modes.Command DSPMode = Modes.Command.DIRECTSETPIXEL;
        readonly private Modes.Command DSFMode = Modes.Command.DIRECTSETFRAME;
        readonly private SolidColorBrush blue = new SolidColorBrush(Color.FromArgb(255, 0, 203, 255));
        readonly private SolidColorBrush grey = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221));
        readonly private SolidColorBrush green = new SolidColorBrush(Color.FromArgb(255, 46, 255, 0));
        readonly private SolidColorBrush red = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public Boolean safemodus = false;
        readonly private int Standardvoltage = 3300; //3.3 V
        readonly public int Voltagelimit = 3500; //3.5 V Above warning dialog will pop u
        private static int panelsizex = 300, panelsizey = 300;
        public UIControl(MainWindow _mwd)
        {
            mwd = _mwd;
            uSBDriver = new USBDriver();
            SetVoltage((uint)Standardvoltage);                                 
            TryToConnectDevice();
        }

        public void ShowUI(Grid _Grid)
        {
            uIPixelArrayGrid = new UIPixelArrayGrid(RowCount, ColCount, calcPixelDist(), mwd);
            pixelArray = uIPixelArrayGrid.GetUIPixels();
            frameToBmp = new FrameToBmp(RowCount, ColCount);
            toolbox = new Toolbox();
            Canvas myCanvas = new Canvas();
            _Grid.Children.Add(myCanvas);
            Canvas subCanvas = new Canvas();
            _Grid.Background = Brushes.Transparent;
            myCanvas.Children.Add(subCanvas);
            drawAction = new DrawAction(RowCount, ColCount, subCanvas, calcPixelDist());
            drawActionOutput = drawAction.GetOutput();
            uIPixelArrayGrid.DrawPannelPixels(subCanvas, 300, 300);
            toolbox.DrawToolSelection(myCanvas);
            toolBoxList = toolbox.GetToolBox();
            MouseDrawActionInit(_Grid, subCanvas);
            MouseDownToolsInit();
            frameStorage = new FrameStorage(uIPixelArrayGrid.GetPixelStatus());
        }

        public void Close()
        {
            if (streamAnimation != null) streamAnimation.Stop();
            if (loadAnimation != null) loadAnimation.Stop();
            SetPixels(false);
        }

        private double calcPixelDist()
        {
            if ((1 - (0.99 * RowCount / 100)) > 0)
            {
                return (1 - (0.99 * RowCount / 100));
            }
            else
            {
                return 0.01;
            }
        }
        public void setRowColCount(int _RowCount, int _ColCount)
        {
            RowCount = _RowCount;
            ColCount = _ColCount;
        }
    }
}
