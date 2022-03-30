using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Mechanics.PowerUps {
    public class Key : PowerUp {
        public int keyId = 0;

        private void Start() {
            SetBuff(new Control.Buffs.Key(keyId));
        }
    }
}
