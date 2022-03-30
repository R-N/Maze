using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control.Debuffs {
    public class Burn : TickingBuff {
        const float delay = 0.5f;
        float damagePerSecond;
        public Burn(float damagePerSecond, float duration, int stack = 1) : base(delay, duration, stack) {
            this.damagePerSecond = damagePerSecond;
        }
        public override BuffId id {
            get {
                return BuffId.burn;
            }
        }

        public override bool End() {
            return true;
        }

        public override void Stack(Buff buff) {
            this.duration = buff.duration;
        }

        public override void Start() {
        }
        public override void OnTick() {
            status.hpCur -= damagePerSecond / delay;
        }

        protected override void _Update(float dt) {

        }
    }

}