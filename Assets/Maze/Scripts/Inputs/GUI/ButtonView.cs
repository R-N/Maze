using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class ButtonView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {


        public Image imageComponent = null;
        public Sprite activeSprite = null;
        public Sprite unavailableSprite = null;
        protected Sprite unpressedSprite = null;
        public Sprite pressedSprite = null;

        public Color activeColor = Color.white;
        public Color pressedColor = Color.grey;
        protected Color unpressedColor;
        public Color unavailableColor = Color.grey;

        public bool pressed = false;

        public bool standalone = true;

        

        protected virtual void OnEnable() {
            if (imageComponent == null) {
                imageComponent = GetComponent<Image>();
            }
            if (activeSprite == null) {
                activeSprite = imageComponent.sprite;
            }
            if (unavailableSprite == null) {
                unavailableSprite = activeSprite;
            }
            if (pressedSprite == null) {
                pressedSprite = activeSprite;
            }

            unpressedSprite = activeSprite;
            unpressedColor = activeColor;

            imageComponent.sprite = unpressedSprite;

            SetState();
        }
        protected virtual void OnDisable() {
            unpressedSprite = unavailableSprite;
            imageComponent.sprite = unavailableSprite;
            SetState();
        }

        public virtual void SetState() {
            if (pressed) {
                SetDownState();
            } else {
                SetUpState();
            }
        }

        public virtual void SetDownState() {
            pressed = true;
            imageComponent.color = pressedColor;
            imageComponent.sprite = pressedSprite;
        }


        public virtual void SetUpState() {
            pressed = false;
            imageComponent.sprite = unpressedSprite;
            imageComponent.color = unpressedColor;
        }



        public void OnPointerDown(PointerEventData eventData) {
            if(standalone) SetDownState();
        }

        public void OnPointerUp(PointerEventData eventData) {
            if (standalone) SetUpState();
        }
    }
}