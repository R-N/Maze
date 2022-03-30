using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class Jumping : Basic {
        public override bool modelFollowMoveDir {
            get {
                return true;
            }
        }
        public override StateId id {
            get {
                return StateId.jumping;
            }
        }
        

        protected override State<PlayerController> _Change(StateId nextState, bool force=false, object arg=null) {
            switch (nextState) {
                case StateId.jumpingEnd: {
                    return new JumpingEnd();
                }
            }
            return null;
        }

        protected override bool _CheckTransition() {
            return false;
        }

        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            //startFrame = Time.frameCount;
        }

        //long startFrame = 0;
        //long jumpFrame = 0;

        protected override void _Update(float dt) {
            controller.UpdateLookDirection(dt);
            controller.UpdateWalk(dt);
            //if (!controller.isGrounded) jumpFrame = Time.frameCount;
            //else OnGrounded();
        }

        protected override bool OnUngrounded() {
            //jumpFrame = Time.frameCount;
            return false;
        }

        protected override bool OnGrounded() {
            //if (jumpFrame > startFrame) {
                UnityEngine.Debug.Log("ON GROUNDED");
                controller.ChangeState(StateId.jumpingEnd);
                return true;
            //}
            //return false;
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            
        }
    }
}
