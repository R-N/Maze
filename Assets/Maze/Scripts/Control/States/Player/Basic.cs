using System;
using System.Collections.Generic;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public abstract class Basic : State<PlayerController> {
        public override State<PlayerController> Change(StateId nextState, bool force = false, object arg = null) {
            switch (nextState) {
                case StateId.falling: {
                    if (force) return new Falling();
                    break;
                }
                case StateId.jumping: {
                    if (force) return new Jumping();
                    break;
                }
            }
            return base.Change(nextState, force, arg);
        }

    }
}
