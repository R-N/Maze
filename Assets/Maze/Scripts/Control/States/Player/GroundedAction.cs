using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public abstract class GroundedAction : Grounded {
        public override bool modelFollowMoveDir {
            get {
                return false;
            }
        }
        protected override bool OnTimerEnd(float remainingDT) {
            controller.ChangeState(controller.inputSource.GetButtonDown("Run") ? StateId.running : StateId.normal, remainingDT);
            return true;
        }

        protected override State<PlayerController> _Change(StateId nextState, bool force = false, object arg = null) {
            switch (nextState) {
                case StateId.normal: {
                    return new Normal();
                }
                case StateId.running: {
                    return new Running();
                }
            }
            return null;
        }
    }
}
