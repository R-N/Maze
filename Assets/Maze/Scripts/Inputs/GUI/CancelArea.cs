using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maze.Inputs.GUI {
    public class CancelArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IDropHandler, IPointerDownHandler, IPointerUpHandler{
        public ButtonView view;
        public CanvasGroup canvasGroup;
        public float range = 40;
        public ICancelable user = null;

        bool pointerInCancelArea = false;
        public void SetEnterState() {
            if (user != null) {
                view.SetDownState();
                pointerInCancelArea = true;
                user.SetCancel(pointerInCancelArea);
            }
        }
        public void SetExitState() {
            if (user != null) {
                view.SetUpState();
                pointerInCancelArea = false;
                user.SetCancel(pointerInCancelArea);
            }
        }
        public void SetUpState(Vector2 point) {
            IsPointInArea(point);
            SetUpState();
        }
        public void SetUpState() {
            if (user != null && pointerInCancelArea) {
                user.OnCancel();
            }
            user = null;
            gameObject.SetActive(false);
            //canvasGroup.blocksRaycasts = true;
        }
        public bool? Release(ICancelable user, Vector2 point) {
            if (this.user == user) {
                IsPointInArea(point);
                SetUpState();
                return pointerInCancelArea;
            }
            return null;
        }
        public bool Use(ICancelable user) {
            if (this.user == null) {
                this.user = user;
                //canvasGroup.blocksRaycasts = false;
                gameObject.SetActive(true);
                SetExitState();
                return true;
            } else if (this.user == user) {
                return true;
            }
            return false;
        }
        private bool IsPointInArea(Vector2 point) {
            if (Vector2.Distance(transform.position.ToVector2XY(), point) < range) {
                if(!pointerInCancelArea) SetEnterState();
                return true;
            } else {
                if (pointerInCancelArea) SetExitState();
                return false;
            }
        }
        public bool? IsPointInArea(ICancelable user, Vector2 point) {
            if (user == this.user) {
                return IsPointInArea(point);
            }
            return null;
        }
        public void OnPointerEnter(PointerEventData eventData) {
            SetEnterState();
        }

        public void OnPointerExit(PointerEventData eventData) {
            SetExitState();
        }

        public void OnPointerDown(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
            SetUpState(eventData.position);
        }

        public void OnDrag(PointerEventData eventData) {
            IsPointInArea(eventData.position);
        }

        public void OnDrop(PointerEventData eventData) {
            SetUpState(eventData.position);
        }
    }
}
