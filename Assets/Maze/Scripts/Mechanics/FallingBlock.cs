using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics {
    public class FallingBlock : MonoBehaviour {
        public Rigidbody rb;
        // Use this for initialization
        void Start() {
            if (rb == null) {
                rb = GetComponent<Rigidbody>();
            }
        }

        public void Move() {
            rb.useGravity = true;
        }
    }
}