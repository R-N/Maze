using System;
using System.Collections.Generic;

namespace Maze.Control.Buffs {
    public class Key : Buff {
        HashSet<int> keys = new HashSet<int>();
        public Key(int key) : base(0) {
            keys.Add(key);
        }

        public override BuffId id {
            get {
                return BuffId.key;
            }
        }

        public override bool End() {
            return true;
        }

        public override void Stack(Buff buff) {
        }

        public override void Start() {
            GUIManager.SpawnToast("Got key.");
        }

        public override int CheckStack(int key=0) {
            if (key > 0) {
                if (keys.Contains(key)) {
                    return key;
                }
            }
            return base.CheckStack(key);
        }
        protected override bool _TakeStack(int key) {
            return keys.Contains(key);
        }

        protected override void _Update(float dt) {
        }
    }
}
