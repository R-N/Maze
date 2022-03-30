using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control.Buffs {
    public class Heal : TickingBuff {
        const float delay = 0.5f;
        float healPerSecond;
        float baseHeal;
        public Heal(float healPerSecond, float duration, float baseHeal = 0, int stack=1) : base(delay, duration, stack) {
            this.healPerSecond = healPerSecond;
            if (baseHeal == 0) {
                this.baseHeal = healPerSecond;
            }
        }
        public override BuffId id {
            get {
                return BuffId.heal;
            }
        }

        public override bool End() {
            return true;
        }

        public override void Stack(Buff buff) {
            this.duration = buff.duration;
        }

        public override void Start() {
            status.hpCur += baseHeal;
            GUIManager.SpawnToast("Got healing.");
        }
        public override void OnTick() {
            status.hpCur += healPerSecond / delay;
        }

        protected override void _Update(float dt) {

        }
    }

}