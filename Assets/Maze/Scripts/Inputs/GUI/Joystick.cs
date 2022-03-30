using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {
        public enum AutoHideType {
            alwaysVisible,
            hideBackground,
            hideAll
        }
        
        public float range = 100;
        public float scaledRange {
            get {
                return range * canvas.scaleFactor;
            }
        }
        public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

        public GameObject background = null;
        Transform backgroundTrans;

        public GameObject child = null;
        Transform childTrans;


        Vector3 anchor;
        Vector2 startPos;
        Vector2 prevPos;

        public bool staticAnchor = true;
        public bool useDelta = false;

        public float sensitivity = 1;

        public AutoHideType autoHide = AutoHideType.alwaysVisible;


        float rawX = 0;
        float rawY = 0;

        public Canvas canvas;

        public virtual void Start() {
            
            backgroundTrans = background.transform;
            childTrans = child.transform;
            anchor = backgroundTrans.position;
        }

        long lastUpdate;

        void Update() {

            if (lastUpdate - Time.frameCount == -2 && useDelta) {
                UpdateVirtualAxes(Vector2.zero);
            }
        }

        Vector3 GetWorldPoint(Vector2 screenPoint) {
            switch (canvas.renderMode) {
                case RenderMode.ScreenSpaceOverlay: {
                    return screenPoint.ToVector3XY();
                }
                case RenderMode.ScreenSpaceCamera: {
                    return canvas.worldCamera.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, canvas.planeDistance));
                }
            }
            return Vector3.zero;
        }

        void UpdateVirtualAxes(Vector2 delta) {
            delta = delta * sensitivity / scaledRange;
            CrossPlatformInputManager.SetAxis(horizontalAxisName, delta.x);
            CrossPlatformInputManager.SetAxis(verticalAxisName, delta.y);
        }


        public virtual void OnDrag(PointerEventData data) {
            var delta = Vector2.ClampMagnitude(data.position - startPos, scaledRange);
            var newPos = startPos + delta;

            childTrans.position = GetWorldPoint(newPos);
            if (useDelta) {
                UpdateVirtualAxes(newPos - prevPos);
                prevPos = newPos;
                lastUpdate = Time.frameCount;
            } else {
                UpdateVirtualAxes(delta);
            }
        }


        public virtual void OnPointerUp(PointerEventData data) {
            UpdateVirtualAxes(Vector2.zero);

            switch (autoHide) {
                case AutoHideType.alwaysVisible: {
                    backgroundTrans.position = anchor;
                    childTrans.position = anchor;
                    break;
                }
                case AutoHideType.hideBackground: {
                    background.SetActive(false);
                    childTrans.position = anchor;
                    break;
                }
                case AutoHideType.hideAll: {
                    background.SetActive(false);
                    child.SetActive(false);
                    break;
                }
            }
        }


        public virtual void OnPointerDown(PointerEventData data) {
            Vector2 newPos = data.position;
            if (staticAnchor) {
                startPos = anchor.ToVector2XY();
            } else {
                startPos = newPos;
            }
            prevPos = startPos;
            backgroundTrans.position = GetWorldPoint(startPos);
            childTrans.position = GetWorldPoint(newPos);

            background.SetActive(true);
            child.SetActive(true);
        }

        void OnDisable() {
        }
    }
}