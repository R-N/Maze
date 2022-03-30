using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Control {
    public abstract class TickingBuff : Buff {
        float delay;
        float timer;
        public TickingBuff(float delay, float duration, int stack=1) : base(duration, stack) {
            this.delay = delay;
            this.timer = delay;
        }

        public override void Update(float dt) {
            base.Update(dt);
            this.timer -= dt;
            if (timer <= 0) {
                OnTick();
                timer += delay;
            }
        }

        public abstract void OnTick();
    }
}
