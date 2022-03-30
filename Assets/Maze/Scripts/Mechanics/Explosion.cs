using Maze.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics {
    public class Explosion : ActionOnControllerContact {
        public Vector2 hitVelocity;

        public override void Action(Controller controller) {
            if (hitVelocity.sqrMagnitude > 0) {
                Vector3 vel = new Vector3(0, hitVelocity.y, 0) + (controller.transform.position - transform.position).NullifyY().normalized * hitVelocity.x;
                Debug.Log("vel: " + vel);
                controller.ThrowOff(vel);
            }
        }
    }
}
