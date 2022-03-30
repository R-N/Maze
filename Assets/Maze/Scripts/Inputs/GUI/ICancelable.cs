using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Inputs.GUI {
    public interface ICancelable {
         void OnCancel();
        void SetCancel(bool cancel);
    }
}
