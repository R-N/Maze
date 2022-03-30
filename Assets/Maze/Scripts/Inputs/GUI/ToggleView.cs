using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class ToggleView : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

        public GameObject activeEffect;

        bool _active = false;
        public bool active {
            get {
                return _active;
            }
            set {
                _active = value;
                RefreshActiveState();
            }
        }
        

        public void SetUpState() {
            active = !active;
        }

        public void RefreshActiveState() {
            if (active) {
                 activeEffect.SetActive(true);
            } else {
                activeEffect.SetActive(false);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            SetUpState();
        }

        public void OnPointerDown(PointerEventData eventData) {

        }
    }
}