using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using Maze.Control.Controllers;

namespace Maze.Inputs.GUI {
    public class ButtonAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IButton {
        public int id;
        public ButtonEvent onClick;


        public void OnPointerDown(PointerEventData eventData) {
        }

        public void OnPointerUp(PointerEventData eventData) {
            ButtonEventData data = new ButtonEventData();
            data.source = gameObject;
            data.sourceId = id;
            data.actor = PlayerController.instance;

            onClick.Invoke(data);
        }

        public void SetButtonEventHandler(ButtonEvent onPress) {
            this.onClick = onPress;
        }
    }
}