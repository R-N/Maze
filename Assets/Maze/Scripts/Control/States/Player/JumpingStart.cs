using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class JumpingStart : Grounded {
        public const float delay = 0.5f;

        public JumpingStart() {
        }
        

        public override StateId id {
            get {
                return StateId.jumpingStart;
            }
        }

        public override bool modelFollowMoveDir {
            get {
                return true;
            }
        }

        protected override State<PlayerController> _Change(StateId nextState, bool force = false, object arg = null) {
            switch (nextState) {
                case StateId.jumping: {
                    return new Jumping();
                }
            }
            return null;
        }

        protected override bool _CheckTransition() {
            return false;
        }

        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            SetTimer(delay);
        }

        protected override void _Update(float dt) {
        }

        protected override bool OnTimerEnd(float remainingDT) {
            controller.AddGravityVelocity(controller.status.jumpMultiplier * controller.status.moveSpeed);
            controller.ChangeState(StateId.jumping, remainingDT);
            return true;
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            
        }
    }
}
