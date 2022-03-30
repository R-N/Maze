
using UnityEngine;
using Maze.Control;

namespace Maze.Mechanics {
    public class ExitPortal : ActionOnControllerContact {
        public override void Action(Controller controller) {
            if (controller.tag == "Player") {
                LevelManager.OnLevelClear();
            }
        }
    }
}
