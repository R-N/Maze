using UnityEngine;
using Maze.Control;
using System.Collections.Generic;

namespace Maze.Mechanics {
    public class Damager : ActionOnControllerContact {
        public float damage;

        public override void Action(Controller controller) {
            if (damage != 0) {
                controller.status.Damage(damage);
            }
        }
    }
}
