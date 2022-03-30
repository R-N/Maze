using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class ButtonInput : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, ICancelable, IDragHandler {


        public string name;

        public bool permitted = false;
        public bool canceled = false;

        void OnEnable() {
        }
        void OnDisable() {
        }

        public void SetDownState() {
            CrossPlatformInputManager.SetButtonDown(name);
        }


        public void SetUpState() {
            if (canceled)
                CrossPlatformInputManager.CancelButton(name);
            else
                CrossPlatformInputManager.SetButtonUp(name);
        }



        public void OnPointerDown(PointerEventData eventData) {
            permitted = GUIManager.UseCancelArea(this);
            canceled = false;
            if(permitted) SetDownState();
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (permitted) {
                GUIManager.ReleaseCancelArea(this, eventData.position);
                SetUpState();
            }
            permitted = false;
        }

        public void OnCancel() {
            canceled = true;
        }

        public void OnDrag(PointerEventData eventData) {
            if(permitted) canceled = GUIManager.IsInCancelArea(this, eventData.position).Value;
        }

        public void SetCancel(bool cancel) {
            canceled = cancel;
        }
    }
}