using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Inputs.Sources {
    public class UserInput : InputSource {
        private static UserInput _instance = null;
        public static UserInput instance {
            get {
                if (_instance == null) {
                    _instance = new UserInput();
                }
                return _instance;
            }
        }

        public UserInput() {
            _instance = this;
        }

        public override void CancelButton(string name) {
            CrossPlatformInputManager.CancelButton(name);
        }

        public override float GetAxis(string name) {
            return CrossPlatformInputManager.GetAxis(name);
        }

        public override bool GetButton(string name) {
            return CrossPlatformInputManager.GetButton(name);
        }

        public override bool GetButtonDown(string name) {
            return CrossPlatformInputManager.GetButtonDown(name);
        }

        public override bool GetButtonUp(string name) {
            return CrossPlatformInputManager.GetButtonUp(name);
        }

        public override float GetPreviousAxis(string name) {
            return CrossPlatformInputManager.GetPreviousAxis(name);
        }

        public override void SetAxis(string name, float value) {
            CrossPlatformInputManager.SetAxis(name, value);
        }

        public override void SetButtonDown(string name) {
            CrossPlatformInputManager.SetButtonDown(name);
        }

        public override void SetButtonUp(string name) {
            CrossPlatformInputManager.SetButtonUp(name);
        }

        public override bool GetButtonUpRaw(string name) {
            return CrossPlatformInputManager.GetButtonUpRaw(name);
        }

        public override bool GetButtonCancel(string name) {
            return CrossPlatformInputManager.GetButtonCancel(name);
        }
    }

}