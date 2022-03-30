using System;
using System.Collections.Generic;

namespace Maze.Inputs {
    public interface IButton {
        void SetButtonEventHandler(ButtonEvent handler);
    }
}
