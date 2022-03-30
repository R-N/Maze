using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control.Buffs {
    public class Spray : Buff {
        public const int maxStack = 3;
        public const int rechargeTime = 5;
        public Spray() : this(maxStack) {
        }
        public Spray(int stack) : base(rechargeTime, stack) {
            continuous = stack >= maxStack;
        }
        public override BuffId id {
            get {
                return BuffId.spray;
            }
        }

        public override bool shouldBeRemoved {
            get {
                return !continuous && duration <= 0;
            }
        }

        public override bool End() {
            Stack(new Spray(1));
            return false;
        }

        public override void Stack(Buff buff) {
            duration = buff.duration;
            stack += buff.stack;
            continuous = stack >= maxStack;
            RefreshStack();
        }

        public override void Start() {
            continuous = stack >= maxStack;
            RefreshStack();
        }

        protected override bool _TakeStack(int stack) {
            bool ret = base._TakeStack(stack);
            RefreshStack();
            return ret;
        }

        void RefreshStack() {
            continuous = stack >= maxStack;
            status.ammo = stack;
            if (continuous) {
                GUIManager.SetSprayRecharge(0);
            }
            //GUIManager.SetSprayCharge(stack);
        }

        protected override void _Update(float dt) {
            if (!continuous) {
                GUIManager.SetSprayRecharge(1 - (duration / rechargeTime));
            }
        }
    }
}
