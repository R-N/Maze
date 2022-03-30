using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class SwipePanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDropHandler, IEndDragHandler{

        public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
        public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input


        Vector2 startPos = Vector2.zero;

        public bool waitRelease = false;
        long lastUpdate = -5;

        float range;
        float scaledRange {
            get {
                return canvas.scaleFactor * range;
            }
        }

        public float sensitivity = 2;

        public Canvas canvas;
        CanvasScaler canvasScaler;

        void OnEnable() {
           
        }

        void UpdateVirtualAxes(Vector2 value) {
            value = value * (sensitivity / scaledRange);
            

            CrossPlatformInputManager.SetAxis(horizontalAxisName, value.x);
            CrossPlatformInputManager.SetAxis(verticalAxisName, value.y);
        }

        // Use this for initialization
        void Start() {
            canvasScaler = canvas.GetComponent<CanvasScaler>();
            RectTransform rect = GetComponent<RectTransform>();
            range = Mathf.Max(rect.rect.width, rect.rect.height);
        }

        void Update() {

            if (lastUpdate - Time.frameCount == -2) {
                UpdateVirtualAxes(Vector2.zero);
            }
        }
        public void OnPointerDown(PointerEventData eventData) {
            startPos = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (waitRelease) {
                UpdateVirtualAxes(eventData.position - startPos);
            } else {
                UpdateVirtualAxes(Vector2.zero);
            }
        }

        public void OnDrag(PointerEventData eventData) {
            if (!waitRelease) {
                UpdateVirtualAxes(eventData.delta);
                lastUpdate = Time.frameCount;
            }
        }

        public void OnDrop(PointerEventData eventData) {
            if (!waitRelease) {
                UpdateVirtualAxes(Vector2.zero);
                lastUpdate = Time.frameCount;
            }
        }

        public void OnEndDrag(PointerEventData eventData) {
            if (!waitRelease) {
                UpdateVirtualAxes(Vector2.zero);
                lastUpdate = Time.frameCount;
            }
        }
    }
}