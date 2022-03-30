using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;
using UnityEngine;

namespace Maze.Control.States.Player {
    public class Lying : Grounded {
        public const float delay = 1f;
        public override bool modelFollowMoveDir {
            get {
                return false;
            }
        }

        public Lying() {
        }


        public override StateId id {
            get {
                return StateId.lying;
            }
        }



        protected override void _Prepare(State<PlayerController> previousState, object arg = null) {
            controller.moveDirection = Vector3.zero;
            SetTimer(delay);
            controller.head.gameObject.SetActive(false);
        }

        protected override bool _CheckTransition() {
            return false;
        }

        protected override void _Update(float dt) {
        }

        protected override bool OnTimerEnd(float remainingDT) {
            controller.ChangeState(StateId.gettingUp, remainingDT);
            return true;
        }

        protected override State<PlayerController> _Change(StateId nextState, bool force = false, object arg = null) {
            switch (nextState) {
                case StateId.gettingUp: {
                    return new GettingUp();
                }
            }
            return null;
        }

        protected override void _End(StateId nextState, bool force = false, object arg = null)
        {
            controller.head.gameObject.SetActive(true);
        }
    }
}
