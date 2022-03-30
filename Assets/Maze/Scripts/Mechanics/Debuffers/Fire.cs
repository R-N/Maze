using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control;
using Maze.Control.Debuffs;

namespace Maze.Mechanics.Debuffers {
    public class Fire : Debuffer {
        public float damagePerSecond = 0;
        public float burnDuration = 0;
        

        public override void DoDebuff(Status status) {
            Buff burn = new Burn(damagePerSecond, burnDuration);
            status.AddBuff(burn);
        }
    }
}
