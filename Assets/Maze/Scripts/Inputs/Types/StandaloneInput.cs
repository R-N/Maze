using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Inputs.Types {
    public class StandaloneInput : VirtualInput {
        class Axis {
            float value;
            float previousValue;
            float valueRaw;
            float previousValueRaw;
            long lastSet;

            public void Update(float value, float valueRaw) {
                if (lastSet < Time.frameCount) {
                    lastSet = Time.frameCount;
                    previousValue = this.value;
                    previousValueRaw = this.valueRaw;
                }
                this.value = value;
                this.valueRaw = valueRaw;
            }

            public float GetValue() {
                return value;
            }

            public float GetValueRaw() {
                return valueRaw;
            }

            public float GetPreviousValue() {
                return previousValue;
            }

            public float GetPreviousValueRaw() {
                return previousValueRaw;
            }
        }
        Dictionary<string, Axis> m_savedAxes = new Dictionary<string, Axis>();

        public override bool AxisExists(string name) {
            if (m_savedAxes.ContainsKey(name)) {
                return true;
            }
            try {
                Input.GetAxis(name);
                return true;
            } catch (ArgumentException) {
                return false;
            }
        }

        public override bool ButtonExists(string name) {
            try {
                Input.GetButton(name);
                return true;
            } catch (ArgumentException) {
                return false;
            }
        }
        public override float GetAxis(string name, bool raw) {
            if (!AxisExists(name)) {
                return 0;
            }
            if (!m_savedAxes.ContainsKey(name)) {
                m_savedAxes[name] = new Axis();
            }
            Axis axis = m_savedAxes[name];
            axis.Update(Input.GetAxis(name), Input.GetAxisRaw(name));
            return raw ? axis.GetValueRaw() : axis.GetValue();
        }

        public override float GetPreviousAxis(string name, bool raw) {
            if (!m_savedAxes.ContainsKey(name)) {
                return 0;
            }
            Axis axis = m_savedAxes[name];
            return raw ? axis.GetPreviousValueRaw() : axis.GetPreviousValue();
        }


        public override bool GetButton(string name) {
            if (!ButtonExists(name)) {
                return false;
            }
            return Input.GetButton(name);
        }


        public override bool GetButtonDown(string name) {
            if (!ButtonExists(name)) {
                return false;
            }
            return Input.GetButtonDown(name);
        }


        public override bool GetButtonUp(string name) {
            if (!ButtonExists(name)) {
                return false;
            }
            return Input.GetButtonUp(name);
        }


        public override void SetButtonDown(string name) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetButtonUp(string name) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisPositive(string name) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisNegative(string name) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxisZero(string name) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override void SetAxis(string name, float value) {
            throw new Exception(
                " This is not possible to be called for standalone input. Please check your platform and code where this is called");
        }


        public override Vector3 MousePosition() {
            return Input.mousePosition;
        }

        public override void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis) {
            throw new NotImplementedException();
        }

        public override void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button) {
            throw new NotImplementedException();
        }

        public override void UnRegisterVirtualAxis(string name) {
            throw new NotImplementedException();
        }

        public override void UnRegisterVirtualButton(string name) {
            throw new NotImplementedException();
        }

        public override CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name) {
            throw new NotImplementedException();
        }

        public override void SetVirtualMousePositionX(float f) {
            throw new NotImplementedException();
        }

        public override void SetVirtualMousePositionY(float f) {
            throw new NotImplementedException();
        }

        public override void SetVirtualMousePositionZ(float f) {
            throw new NotImplementedException();
        }

        public override void CancelButton(string name) {
            throw new NotImplementedException();
        }

        public override CrossPlatformInputManager.VirtualAxis GetVirtualAxis(string name) {
            throw new NotImplementedException();
        }

        public override CrossPlatformInputManager.VirtualButton GetVirtualButton(string name) {
            throw new NotImplementedException();
        }

        public override bool GetButtonCancel(string name) {
            throw new NotImplementedException();
        }

        public override bool GetButtonUpRaw(string name) {
            return GetButtonUp(name);
        }
    }
}