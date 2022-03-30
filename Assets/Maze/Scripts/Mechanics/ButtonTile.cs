using Maze.Control;
using Maze.Inputs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics {
    public enum ButtonState {
        unpressed,
        pressed
    }
    public class ButtonTile : ActionOnControllerContact, IButton {
        public float pushDistance = 0.05f;
        public float reactivateDelay = 0;
        float timer = 0;
        public int id;
        public ButtonState state = ButtonState.unpressed;
        public ButtonEvent onPress;
        // Use this for initialization
        void Start() {

        }

        public void Update() {
            if (timer > 0) {
                timer -= Time.deltaTime;
                if (timer <= 0) {
                    StateUp();
                }
            }
        }

        public void StateUp() {
            state = ButtonState.unpressed;
            timer = 0;
            transform.position = transform.position + transform.up * pushDistance;
        }

        public void StateDown() {
            state = ButtonState.pressed;
            timer = reactivateDelay;
            transform.position = transform.position - transform.up * pushDistance;
        }

        public override void Action(Controller ctrl) {
            if (state == ButtonState.unpressed) {
                if (ctrl != null) {
                    StateDown();
                    Debug.Log("Pressed by: " + ctrl.name);
                    ButtonEventData data = new ButtonEventData();
                    data.actor = ctrl;
                    data.source = gameObject;
                    data.sourceId = id;
                    onPress.Invoke(data);
                }
            }
        }

        public void SetButtonEventHandler(ButtonEvent onPress) {
            this.onPress = onPress;
        }
    }
}