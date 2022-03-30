using UnityEngine;
using Maze.Control;
using System.Collections.Generic;

namespace Maze.Mechanics {
    public abstract class ActionOnControllerContact : MyBehaviour {
        public float hitDelay = 0;

        public Dictionary<Controller, float> hits = new Dictionary<Controller, float>();

        protected void Update(){}

        public void CheckActionEnter(GameObject go) {
            if(enabled){
                Controller controller = go.GetComponent<Controller>();
                if (controller == null) return;
                if (hitDelay <= 0 || !hits.ContainsKey(controller) || (Time.time - hits[controller]) > hitDelay){
                    Ignore(controller);
                    Action(controller);
                }
            }
        }
        public void CheckActionExit(GameObject go) {
            if(enabled){
                Controller controller = go.GetComponent<Controller>();
                if (controller == null) return;
                if (hits.ContainsKey(controller)) hits.Remove(controller);
            }
        }

        public void Ignore(Controller controller){
            hits[controller] = hitDelay;
        }

        public abstract void Action(Controller controller);

        public override void OnControllerColliderHit(ControllerColliderHit hit) {
            CheckActionEnter(hit.controller.gameObject);
        }
        public void OnCollisionEnter(Collision collision) {
            CheckActionEnter(collision.collider.gameObject);
        }

        public void OnCollisionExit(Collision collision) {
            CheckActionExit(collision.collider.gameObject);
        }

        public void OnTriggerEnter(Collider other) {
            CheckActionEnter(other.gameObject);
        }

        public void OnTriggerExit(Collider other) {
            CheckActionExit(other.gameObject);
        }
    }
}
