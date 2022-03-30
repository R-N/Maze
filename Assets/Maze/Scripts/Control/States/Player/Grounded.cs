using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public abstract class Grounded : Basic {

        protected override bool OnUngrounded() {
            Vector3 pos = controller.transform.position;
            Vector3 up = controller.transform.up;
            Vector3 halfHeightVector = up * 0.5f * controller.height;
            RaycastHit hit;
            if (!Physics.CapsuleCast(
                pos + halfHeightVector,
                pos - halfHeightVector,
                controller.radius,
                -up,
                out hit,
                controller.stepOffset,
                CamHolder.mask
                )) {
                controller.ChangeState(StateId.falling, true);
                return true;
            }
            return false;
        }

    }
}
