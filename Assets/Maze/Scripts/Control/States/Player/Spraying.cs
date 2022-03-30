using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.Controllers;
using Maze.Mechanics;

namespace Maze.Control.States.Player {
    public class Spraying : GroundedAction {
        public const float delay = 0.5f;

        public Spraying()  {
        }

        public override bool modelFollowMoveDir {
            get {
                return false;
            }
        }


        public override StateId id {
            get {
                return StateId.spraying;
            }
        }
        
        protected override bool _CheckTransition() {
            return false;
        }
        

        protected override void _Update(float dt) {
        }
        
        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            SetTimer(delay);
        }

        protected override bool OnTimerEnd(float remainingDT) {
            RaycastHit hit;
            if (
                Physics.Raycast(controller.camHolder.position, controller.camHolder.forward, out hit, controller.status.attackRange, AimHelper.mask)
                && controller.status.TakeStack(BuffId.spray, 1)
                ) {
                Transform spray = GameObject.Instantiate<Transform>(
                    controller.status.attackProjectile,
                    hit.point + hit.normal * 0.05f,
                    Quaternion.LookRotation(-hit.normal, controller.camHolder.up)
                );
                spray.SetParent(hit.transform, true);
                spray.GetComponent<Decal>().affectedLayers = CamHolder.mask;
            }
            return base.OnTimerEnd(remainingDT);
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            
        }
    }
}
