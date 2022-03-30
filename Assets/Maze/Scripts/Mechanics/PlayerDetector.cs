using Maze.Inputs;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Maze.Mechanics {
    public class PlayerDetector : MonoBehaviour {
        public PlayerDetectedEvent onEnter;
        public PlayerDetectedEvent onExit;

        public bool playerInside = false;

        // Update is called once per frame
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                playerInside = true;
                PlayerDetectedEventData data = new PlayerDetectedEventData();
                data.source = this;
                data.playerInside = playerInside;
                onEnter.Invoke(data);
            }
        }

        private void OnTriggerExit(Collider other) {
            if (other.tag == "Player") {
                playerInside = false;
                PlayerDetectedEventData data = new PlayerDetectedEventData();
                data.source = this;
                data.playerInside = playerInside;
                onExit.Invoke(data);
            }
        }
    }
}
