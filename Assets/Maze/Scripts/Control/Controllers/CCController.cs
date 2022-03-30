using System;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.States;

namespace Maze.Control.Controllers {
    public abstract class CCController<T> : Controller<T> where T : CCController<T> {

        public CharacterController cc = null;

        protected float gravityVel = 0;
        protected Vector3 movement;
        protected float prevGrav = 0;

        public override float stepOffset {
            get {
                return cc.stepOffset;
            }
        }

        public override bool isGrounded {
            get {
                return cc.isGrounded;
            }
        }
        
        public override float height {
            get {
                return cc.height;
            }
        }
        public override float radius {
            get {
                return cc.radius;
            }
        }

        public override void AddGravityVelocity(float gravVel) {
            this.gravityVel += gravVel;
        }
        public override void SetGravityVelocity(float gravVel) {
            this.gravityVel = gravVel;
        }

        public override void PrepareUpdate() {
            prevGrav = gravityVel;
            movement = Vector3.zero;
        }

        public override void Move(Vector3 movement) {
            cc.Move(movement);
        }

        public override void PostUpdate() {
            Move(movement);
        }

        public override void UpdateGravity(float dt) {
            bool grounded = cc.isGrounded;

            if (grounded && gravityVel <= 0) {
                gravityVel = -2 * cc.skinWidth;
            } else {
                gravityVel += Physics.gravity.y * dt;
            }

            movement += Vector3.up * 0.5f * (prevGrav + gravityVel) * dt;
        }
        public void OnControllerColliderHit(ControllerColliderHit hit) {
            MyBehaviour[] b = hit.collider.GetComponents<MyBehaviour>();
            foreach (MyBehaviour bi in b) {
                bi.OnControllerColliderHit(hit);
            }
        }
    }
}
