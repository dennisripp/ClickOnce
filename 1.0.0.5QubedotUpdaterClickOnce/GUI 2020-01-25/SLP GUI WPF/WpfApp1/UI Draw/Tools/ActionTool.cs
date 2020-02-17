using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class ActionTool : Tool
    {
        private Modes.Action actionMode;

        public ActionTool(string _filename, Modes.Action _actionMode, List<Tool> _toolBoxList)
            : base(_filename, _toolBoxList)
        {
            actionMode = _actionMode;
        }

        public Modes.Action GetActionMode()
        {
            return actionMode;
        }
    }
}
