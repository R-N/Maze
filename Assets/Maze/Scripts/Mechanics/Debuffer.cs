using UnityEngine;
using Maze.Control.Controllers;
using Maze.Control;

namespace Maze.Mechanics {
    public abstract class Debuffer : ActionOnControllerContact {

        public abstract void DoDebuff(Status status);

        public override void Action(Controller controller) {
            DoDebuff(controller.status);
        }
    }
}
