using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class ToggleInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ICancelable, IDragHandler {

        public ToggleView view;

        public new string name;

        public bool active = false;

        public bool actAsHold = true;

        bool permitted = false;
        bool canceled = false;

        void OnEnable() {
        }
        void OnDisable() {
        }

        public void SetDownState() {
            CrossPlatformInputManager.SetButtonDown(name);
        }

        public void SetUpState() {
            CrossPlatformInputManager.SetButtonUp(name);
        }


        public void OnPointerUp(PointerEventData eventData) {
            if (permitted) {
                GUIManager.ReleaseCancelArea(this, eventData.position);
                if (!canceled) {
                    active = !active;
                    if (actAsHold) {
                        if (active) {
                            SetDownState();
                        } else {
                            SetUpState();
                        }
                    } else {
                        SetUpState();
                    }
                } else {
                    view.active = !view.active;
                    if (!actAsHold) {
                        CrossPlatformInputManager.CancelButton(name);
                    }
                }
            }
        }

        public void OnPointerDown(PointerEventData eventData) {
            permitted = GUIManager.UseCancelArea(this);
            canceled = false;
            if (permitted) {
                if (!actAsHold) {
                    SetDownState();
                }
            }
        }

        public void OnCancel() {
            canceled = true;
        }

        public void OnDrag(PointerEventData eventData) {
            if (permitted) canceled = GUIManager.IsInCancelArea(this, eventData.position).Value;
        }

        public void SetCancel(bool cancel) {
            canceled = cancel;
        }
    }
}