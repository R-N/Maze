using UnityEngine;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class Falling : Basic {
        public override bool modelFollowMoveDir {
            get {
                return false;
            }
        }
        public override StateId id {
            get {
                return StateId.falling;
            }
        }

        protected override State<PlayerController> _Change(StateId nextState, bool force = false, object arg = null) {
            switch (nextState) {
                case StateId.lying: {
                    return new Lying();
                }
            }
            return null;
        }

        protected override bool _CheckTransition() {
            return false;
        }
        //long startFrame = 0;
        //long fallFrame = 0;

        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            Vector3 dir = controller.moveDirection;
            //startFrame = Time.frameCount;
            if (dir.sqrMagnitude > 0) {
                Vector3 localDir = controller.transform.InverseTransformDirection(dir).normalized;
                controller.ModelFollowMoveDir(-dir.x, -dir.z);
            }
        }

        protected override void _Update(float dt) {
            controller.Move(controller.moveDirection * dt);
            //if (!controller.isGrounded) fallFrame = Time.frameCount;
            //else OnGrounded();
        }

        protected override bool OnUngrounded() {
            //fallFrame = Time.frameCount;
            return false;
        }

        protected override bool OnGrounded() {
            //if (fallFrame > startFrame) {
                controller.ChangeState(StateId.lying);
                return true;
            //}
            //return false;
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
        }
    }
}
