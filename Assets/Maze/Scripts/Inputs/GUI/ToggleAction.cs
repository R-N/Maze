using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;
using Maze.Control.Controllers;

namespace Maze.Inputs.GUI {
    public class ToggleAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IToggle {


        public int id;
        public bool active = false;
        public ToggleEvent onToggle;
        public ToggleEvent onToggleOn;
        public ToggleEvent onToggleOff;

        public ToggleView view;


        void OnEnable() {
        }
        void OnDisable() {
        }

        public void OnPointerUp(PointerEventData eventData) {
            active = !active;
            ToggleEventData data = new ToggleEventData();
            data.actor = PlayerController.instance;
            data.source = gameObject;
            data.sourceId = id;
            data.enabled = active;
            onToggle.Invoke(data);
            if (active) {
                onToggleOn.Invoke(data);
            } else {
                onToggleOff.Invoke(data);
            }
        }
        public void OnPointerDown(PointerEventData eventData) {
        }

        public void SetViewState(bool on) {
            view.active = on;
            view.RefreshActiveState();
        }

        public void SetToggleEventHandler(ToggleEvent handler) {
            this.onToggle = handler;
        }

        public void SetToggleOnHandler(ToggleEvent handler) {
            this.onToggleOn = handler;
        }

        public void SetToggleOffHandler(ToggleEvent handler) {
            this.onToggleOff = handler;
        }
    }

}