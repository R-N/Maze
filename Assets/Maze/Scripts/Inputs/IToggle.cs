using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Inputs {
    public interface IToggle {
        void SetViewState(bool on);
        void SetToggleEventHandler(ToggleEvent handler);
        void SetToggleOnHandler(ToggleEvent handler);
        void SetToggleOffHandler(ToggleEvent handler);
    }
}
