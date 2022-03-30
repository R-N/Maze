using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Inputs.Types {
    public class MobileInput : VirtualInput {
        public Vector3 virtualMousePosition { get; private set; }


        protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> m_VirtualAxes =
            new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();
        // Dictionary to store the name relating to the virtual axes
        protected Dictionary<string, CrossPlatformInputManager.VirtualButton> m_VirtualButtons =
            new Dictionary<string, CrossPlatformInputManager.VirtualButton>();
        protected List<string> m_AlwaysUseVirtual = new List<string>();
        // list of the axis and button names that have been flagged to always use a virtual axis or button


        public override bool AxisExists(string name) {
            return m_VirtualAxes.ContainsKey(name);
        }

        public override bool ButtonExists(string name) {
            return m_VirtualButtons.ContainsKey(name);
        }


        public override void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis) {
            // check if we already have an axis with that name and log and error if we do
            if (m_VirtualAxes.ContainsKey(axis.name)) {
                Debug.LogError("There is already a virtual axis named " + axis.name + " registered.");
            } else {
                // add any new axes
                m_VirtualAxes.Add(axis.name, axis);

                // if we dont want to match with the input manager setting then revert to always using virtual
                if (!axis.matchWithInputManager) {
                    m_AlwaysUseVirtual.Add(axis.name);
                }
            }
        }


        public override void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button) {
            // check if already have a buttin with that name and log an error if we do
            if (m_VirtualButtons.ContainsKey(button.name)) {
                Debug.LogError("There is already a virtual button named " + button.name + " registered.");
            } else {
                // add any new buttons
                m_VirtualButtons.Add(button.name, button);

                // if we dont want to match to the input manager then always use a virtual axis
                if (!button.matchWithInputManager) {
                    m_AlwaysUseVirtual.Add(button.name);
                }
            }
        }


        public override void UnRegisterVirtualAxis(string name) {
            // if we have an axis with that name then remove it from our dictionary of registered axes
            if (m_VirtualAxes.ContainsKey(name)) {
                m_VirtualAxes.Remove(name);
            }
        }


        public override void UnRegisterVirtualButton(string name) {
            // if we have a button with this name then remove it from our dictionary of registered buttons
            if (m_VirtualButtons.ContainsKey(name)) {
                m_VirtualButtons.Remove(name);
            }
        }


        // returns a reference to a named virtual axis if it exists otherwise null
        public override CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name) {
            return m_VirtualAxes.ContainsKey(name) ? m_VirtualAxes[name] : null;
        }


        public override void SetVirtualMousePositionX(float f) {
            virtualMousePosition = new Vector3(f, virtualMousePosition.y, virtualMousePosition.z);
        }


        public override void SetVirtualMousePositionY(float f) {
            virtualMousePosition = new Vector3(virtualMousePosition.x, f, virtualMousePosition.z);
        }


        public override void SetVirtualMousePositionZ(float f) {
            virtualMousePosition = new Vector3(virtualMousePosition.x, virtualMousePosition.y, f);
        }

        private void AddButton(string name) {
            // we have not registered this button yet so add it, happens in the constructor
            RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
        }


        private void AddAxes(string name) {
            // we have not registered this button yet so add it, happens in the constructor
            RegisterVirtualAxis(new CrossPlatformInputManager.VirtualAxis(name));
        }


        public override float GetAxis(string name, bool raw) {
            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            return m_VirtualAxes[name].GetValue;
        }


        public override void SetButtonDown(string name) {
            if (!m_VirtualButtons.ContainsKey(name)) {
                AddButton(name);
            }
            m_VirtualButtons[name].Pressed();
        }


        public override void SetButtonUp(string name) {
            if (!m_VirtualButtons.ContainsKey(name)) {
                AddButton(name);
            }
            m_VirtualButtons[name].Released();
        }

        public override void CancelButton(string name) {
            if (!m_VirtualButtons.ContainsKey(name)) {
                AddButton(name);
            }
            m_VirtualButtons[name].Canceled();
        }



        public override void SetAxisPositive(string name) {
            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            m_VirtualAxes[name].Update(1f);
        }


        public override void SetAxisNegative(string name) {
            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            m_VirtualAxes[name].Update(-1f);
        }


        public override void SetAxisZero(string name) {
            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            m_VirtualAxes[name].Update(0f);
        }


        public override void SetAxis(string name, float value) {
            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            m_VirtualAxes[name].Update(value);
        }


        public override bool GetButtonDown(string name) {
            if (m_VirtualButtons.ContainsKey(name)) {
                return m_VirtualButtons[name].GetButtonDown;
            }

            AddButton(name);
            return m_VirtualButtons[name].GetButtonDown;
        }


        public override bool GetButtonUp(string name) {
            if (m_VirtualButtons.ContainsKey(name)) {
                return m_VirtualButtons[name].GetButtonUp;
            }

            AddButton(name);
            return m_VirtualButtons[name].GetButtonUp;
        }


        public override bool GetButton(string name) {
            if (m_VirtualButtons.ContainsKey(name)) {
                return m_VirtualButtons[name].GetButton;
            }

            AddButton(name);
            return m_VirtualButtons[name].GetButton;
        }


        public override Vector3 MousePosition() {
            return virtualMousePosition;
        }

        public override float GetPreviousAxis(string name, bool raw) {

            if (!m_VirtualAxes.ContainsKey(name)) {
                AddAxes(name);
            }
            return m_VirtualAxes[name].GetPreviousValue;
        }

        public override CrossPlatformInputManager.VirtualAxis GetVirtualAxis(string name) {
            if (!AxisExists(name)) {
                AddAxes(name);
            }
            return m_VirtualAxes[name];
        }

        public override CrossPlatformInputManager.VirtualButton GetVirtualButton(string name) {
            if (!ButtonExists(name)) {
                AddButton(name);
            }
            return m_VirtualButtons[name];
        }

        public override bool GetButtonCancel(string name) {
            if (m_VirtualButtons.ContainsKey(name)) {
                return m_VirtualButtons[name].GetButtonUp;
            }

            AddButton(name);
            return m_VirtualButtons[name].GetButtonCancel;
        }

        public override bool GetButtonUpRaw(string name) {
            if (m_VirtualButtons.ContainsKey(name)) {
                return m_VirtualButtons[name].GetButtonUp;
            }

            AddButton(name);
            return m_VirtualButtons[name].GetButtonUpRaw;
        }
    }
}