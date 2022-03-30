using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class JumpingEnd : GroundedAction {
        public const float delay = 0.5f;
        public override bool modelFollowMoveDir {
            get {
                return true;
            }
        }

        public JumpingEnd()  {
        }
        

        public override StateId id {
            get {
                return StateId.jumpingEnd;
            }
        }
        
        

        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            SetTimer(delay);
        }
        

        protected override bool _CheckTransition() {
            return false;
        }

        protected override void _Update(float dt) {
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            
        }
    }
}
