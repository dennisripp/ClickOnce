using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Frame
    {
        private bool[,] frame;

        public Frame(bool[,] _frame)
        {
            frame = _frame;
        }

        public bool[,] GetFrame()
        {
            return frame;
        }
    }
}
