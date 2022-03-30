using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maze.Inputs.GUI {
    public class ButtonAim : Joystick, ICancelable {
        public string buttonName = "Aim";
        public bool permitted = false;
        public bool canceled = false;
        public override void Start() {
            autoHide = AutoHideType.hideBackground;
            base.Start();
        }

        public override void OnPointerDown(PointerEventData data) {
            permitted = GUIManager.UseCancelArea(this);
            canceled = false;
            if (permitted) {
                CrossPlatformInputManager.SetButtonDown(buttonName);
                base.OnPointerDown(data);
            }
        }

        public override void OnPointerUp(PointerEventData data) {
            if (permitted) {
                GUIManager.ReleaseCancelArea(this, data.position);
                if (canceled)
                    CrossPlatformInputManager.CancelButton(buttonName);
                else
                    CrossPlatformInputManager.SetButtonUp(buttonName);
                base.OnPointerUp(data);
            }
        }
        public override void OnDrag(PointerEventData eventData) {
            if (permitted) canceled = GUIManager.IsInCancelArea(this, eventData.position).Value;
            base.OnDrag(eventData);
        }

        public void OnCancel() {
            canceled = true;
        }

        public void SetCancel(bool cancel) {
            canceled = cancel;
        }
    }
}