using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    class Running : Normal {
        public Running(){ }
        public override StateId id {
            get {
                return StateId.running;
            }
        }
        protected override State<PlayerController> _Change(StateId nextState, bool force=false, object arg=null) {
            switch (nextState) {
                case StateId.normal: {
                    return new Normal();
                }
                case StateId.jumpingRun: {
                    return new JumpingRun();
                }
                case StateId.spraying: {
                    return new Spraying();
                }
            }
            return null;
        }

        protected override bool _CheckTransition() {
            if (!controller.inputSource.GetButton("Run")) {
                controller.ChangeState(StateId.normal);
                return true;
            } else if (controller.inputSource.GetButtonUp("Jump")) {
                controller.AddGravityVelocity(controller.status.jumpMultiplier / controller.status.runMultiplier * controller.status.moveSpeed);
                controller.ChangeState(StateId.jumpingRun);
                return true;
            } else if (CheckSpray()) {
                return true;
            }
            return false;
        }

        protected override void _Prepare(State<PlayerController> previousState, object arg=null) {
        }

        public override void Update(float dt) {
            controller.status.moveSpeed *= controller.status.runMultiplier;
            base.Update(dt);
        }
    }
}
