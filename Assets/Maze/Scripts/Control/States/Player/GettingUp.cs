using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class GettingUp : GroundedAction {
        public override StateId id {
            get {
                return StateId.gettingUp;
            }
        }
        

        protected override bool _CheckTransition() {
            return false;
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
        }

        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            SetTimer(0.5f);
        }

        protected override void _Update(float dt) {

        }
        
    }
}
