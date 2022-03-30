using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Inputs.Types {
    public class HybridInput : VirtualInput {
        StandaloneInput hardwareInput;
        MobileInput touchInput;
        public HybridInput(StandaloneInput hardwareInput, MobileInput touchInput) {
            this.hardwareInput = hardwareInput;
            this.touchInput = touchInput;
        }


        public override bool AxisExists(string name) {
            return touchInput.AxisExists(name) || hardwareInput.AxisExists(name);
        }

        public override bool ButtonExists(string name) {
            return touchInput.ButtonExists(name) || hardwareInput.ButtonExists(name);
        }


        public override void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis) {
            touchInput.RegisterVirtualAxis(axis);
        }


        public override void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button) {
            touchInput.RegisterVirtualButton(button);
        }


        public override void UnRegisterVirtualAxis(string name) {
            touchInput.UnRegisterVirtualAxis(name);
        }


        public override void UnRegisterVirtualButton(string name) {
            touchInput.UnRegisterVirtualButton(name);
        }


        // returns a reference to a named virtual axis if it exists otherwise null
        public override CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name) {
            return touchInput.VirtualAxisReference(name);
        }


        public override void SetVirtualMousePositionX(float f) {
            touchInput.SetVirtualMousePositionX(f);
        }


        public override void SetVirtualMousePositionY(float f) {
            touchInput.SetVirtualMousePositionY(f);
        }


        public override void SetVirtualMousePositionZ(float f) {
            touchInput.SetVirtualMousePositionZ(f);
        }

        public override float GetAxis(string name, bool raw) {
            return hardwareInput.GetAxis(name, raw) + touchInput.GetAxis(name, raw);
        }


        public override void SetButtonDown(string name) {
            touchInput.SetButtonDown(name);
        }


        public override void SetButtonUp(string name) {
            touchInput.SetButtonUp(name);
        }

        public override void CancelButton(string name) {
            touchInput.CancelButton(name);
        }


        public override void SetAxisPositive(string name) {
            touchInput.SetAxisPositive(name);
        }


        public override void SetAxisNegative(string name) {
            touchInput.SetAxisNegative(name);
        }


        public override void SetAxisZero(string name) {
            touchInput.SetAxisZero(name);
        }


        public override void SetAxis(string name, float value) {
            touchInput.SetAxis(name, value);
        }


        public override bool GetButtonDown(string name) {
            return touchInput.GetButtonDown(name) || hardwareInput.GetButtonDown(name);
        }


        public override bool GetButtonUp(string name) {
            return (touchInput.GetButtonUp(name) || hardwareInput.GetButtonUp(name)) && !GetButton(name);
        }


        public override bool GetButton(string name) {
            return touchInput.GetButton(name) || hardwareInput.GetButton(name);
        }


        public override Vector3 MousePosition() {
            return Input.mousePosition;
        }

        public override float GetPreviousAxis(string name, bool raw) {
            return hardwareInput.GetPreviousAxis(name, raw) + touchInput.GetPreviousAxis(name, raw);
        }

        public override CrossPlatformInputManager.VirtualAxis GetVirtualAxis(string name) {
            return touchInput.GetVirtualAxis(name);
        }

        public override CrossPlatformInputManager.VirtualButton GetVirtualButton(string name) {
            return touchInput.GetVirtualButton(name);
        }

        public override bool GetButtonCancel(string name) {
            return touchInput.GetButtonCancel(name);
        }

        public override bool GetButtonUpRaw(string name) {
            return touchInput.GetButtonUpRaw(name) || hardwareInput.GetButtonUpRaw(name);
        }
    }
}