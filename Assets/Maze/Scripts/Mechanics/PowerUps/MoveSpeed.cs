
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Mechanics.PowerUps {
    public class MoveSpeed : PowerUp{
        public float buffDuration = 20;
        public float speedIncrease = 1.5f;

        private void Start() {
            SetBuff(new Control.Buffs.MoveSpeed(buffDuration, speedIncrease));
        }
    }
}
