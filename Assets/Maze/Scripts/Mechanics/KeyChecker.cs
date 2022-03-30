using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Control;
using UnityEngine.Events;

namespace Maze.Mechanics {
    public enum KeyCheckerState {
        locked,
        unlocked,
        opened
    }
    public class KeyChecker : ActionOnControllerContact {

        public int keyId = 0;

        public UnityEvent onUnlock;

        public KeyCheckerState state = KeyCheckerState.locked;

        public override void Action(Controller controller) {
            Check(controller);
        }

        public virtual void OnUnlock() {
            onUnlock.Invoke();
        }

        public void Check(Controller ctrl) {
            if (keyId == 0 || ctrl.status.CheckStack(BuffId.key, keyId) == keyId) {
                OnUnlock();
            }
        }
    }
}
