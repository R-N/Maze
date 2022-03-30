using System;
using System.Collections.Generic;
using UnityEngine;


namespace Maze.Inputs {
    public abstract class VirtualInput {


        public abstract bool AxisExists(string name);

        public abstract bool ButtonExists(string name);


        public abstract void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis);


        public abstract void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button);


        public abstract void UnRegisterVirtualAxis(string name);

        public abstract void UnRegisterVirtualButton(string name);


        // returns a reference to a named virtual axis if it exists otherwise null
        public abstract CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name);


        public abstract void SetVirtualMousePositionX(float f);

        public abstract void SetVirtualMousePositionY(float f);


        public abstract void SetVirtualMousePositionZ(float f);


        public float GetAxis(string name) {
            return GetAxis(name, false);
        }
        public float GetAxisRaw(string name) {
            return GetAxis(name, true);
        }
        public abstract float GetAxis(string name, bool raw);

        public float GetPreviousAxis(string name) {
            return GetPreviousAxis(name, false);
        }
        public float GetPreviousAxisRaw(string name) {
            return GetPreviousAxis(name, true);
        }
        public abstract float GetPreviousAxis(string name, bool raw);

        public abstract bool GetButton(string name);
        public abstract bool GetButtonDown(string name);
        public abstract bool GetButtonUp(string name);
        public abstract bool GetButtonCancel(string name);
        public abstract bool GetButtonUpRaw(string name);

        public abstract void SetButtonDown(string name);
        public abstract void SetButtonUp(string name);
        public abstract void CancelButton(string name);
        public abstract void SetAxisPositive(string name);
        public abstract void SetAxisNegative(string name);
        public abstract void SetAxisZero(string name);
        public abstract void SetAxis(string name, float value);
        public abstract Vector3 MousePosition();

        public abstract CrossPlatformInputManager.VirtualAxis GetVirtualAxis(string name);
        public abstract CrossPlatformInputManager.VirtualButton GetVirtualButton(string name);
    }
}