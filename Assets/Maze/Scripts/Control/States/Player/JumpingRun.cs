using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control.Controllers;

namespace Maze.Control.States.Player {
    public class JumpingRun : Jumping {
        
        public override void Update(float dt) {
            controller.status.moveSpeed *= controller.status.runMultiplier;
            base.Update(dt);
        }
        
    }
}
