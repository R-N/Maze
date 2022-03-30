using System;
using System.Collections.Generic;

namespace Maze.Control.Buffs {
    public class MoveSpeed : Buff {
        float increase;
        public MoveSpeed(float duration, float increase) : base(duration) {
            this.increase = increase;
        }

        public override BuffId id {
            get {
                return BuffId.moveSpeed;
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
        

        protected override void _Update(float dt) {
            status.moveSpeed *= increase;
        }
    }
}
