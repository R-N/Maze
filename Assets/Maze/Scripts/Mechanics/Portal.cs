using Maze.Control;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics {
    public class Portal : ActionOnControllerContact {
        public Transform target;
        public bool rotate180 = false;
        public bool nullifyDir = true;
        public StateId changeState = StateId.none;
        

        public static void Move(Controller ctrl, Transform target, StateId changeState = StateId.none, bool nullifyDir = true, bool rotate180 = false) {
            if (target == null) {
                target = LevelManager.GetRandomPlayerRespawn();
            }
            Transform trans = ctrl.transform;
            Vector3 halfHeightVector = trans.up * 0.5f * ctrl.height;
            RaycastHit hit;
            float height = 0;
            bool moved = false;
            if (changeState == StateId.none) {
                if (ctrl.isGrounded) {
                    if (Physics.CapsuleCast(
                        trans.position + halfHeightVector,
                        trans.position - halfHeightVector,
                        ctrl.radius,
                        -trans.up,
                        out hit,
                        float.PositiveInfinity,
                        CamHolder.mask
                        )) {
                        height = Vector3.Distance(hit.point, trans.position);
                    }
                }
                if (height > 0) {
                    if (Physics.CapsuleCast(
                        target.position + halfHeightVector,
                        target.position - halfHeightVector,
                        ctrl.radius,
                        -trans.up,
                        out hit,
                        float.PositiveInfinity,
                        CamHolder.mask
                        )) {
                        trans.position = hit.point + trans.up * height;
                        moved = true;
                    }
                }
            }

            if (!moved) {
                trans.position = target.position;
                ctrl.ChangeState(changeState, true);
            }

            trans.rotation = target.rotation;
            if (rotate180) {
                trans.Rotate(Vector3.up, 180, Space.Self);
            }
            if (nullifyDir) {
                ctrl.NullifyDir();
            }
        }



        public override void Action(Controller ctrl) {
            Move(ctrl, target, changeState, nullifyDir, rotate180);

        }
    }
}