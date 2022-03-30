using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class Normal : Grounded {
        public Normal()  { }
        public override bool modelFollowMoveDir {
            get {
                return true;
            }
        }

        public override StateId id {
            get {
                return StateId.normal;
            }
        }

        protected override State<PlayerController> _Change(StateId nextState, bool force=false, object arg=null) {
            switch (nextState) {
                case StateId.running: {
                    return new Running();
                }
                case StateId.jumpingStart: {
                    return new JumpingStart();
                }
                case StateId.spraying: {
                    return new Spraying();
                }
            }
            return null;
        }

        protected bool CheckSpray() {
            RaycastHit hit;
            if (controller.inputSource.GetButtonUp("Spray")
                && Physics.Raycast(controller.camHolder.position, controller.camHolder.forward, out hit, controller.status.attackRange, CamHolder.mask)
                && controller.status.CheckStack(BuffId.spray) > 0
                ) {
                AimHelper.AimSpray(false);
                controller.ChangeState(StateId.spraying);
                return true;
            }
            return false;
        }

        protected override bool _CheckTransition() {
            if (controller.inputSource.GetButton("Run")) {
                controller.ChangeState(StateId.running);
                return true;
            } else if (controller.inputSource.GetButtonUp("Jump")) {
                controller.ChangeState(StateId.jumpingStart);
                return true;
            } else if (CheckSpray()){
                return true;
            }
            return false;
        }

        protected override void _Prepare(State<PlayerController> previousState, object arg=null) {
        }

        protected override void _Update(float dt) {
            if (controller.inputSource.GetButtonDown("Spray")) {
                AimHelper.AimSpray(true);
            } else if (controller.inputSource.GetButtonUpRaw("Spray")) {
                AimHelper.AimSpray(false);
            }
            controller.UpdateLookDirection(dt);
            controller.UpdateMoveDirection(dt);
            controller.UpdateWalk(dt);
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            
        }
    }
}
