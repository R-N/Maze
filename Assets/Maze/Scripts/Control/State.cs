using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Maze.Control.Controllers;
namespace Maze.Control{
    public enum StateId {
        none = -1,
        normal = 0,
        dodging = 1,
        running = 2,
        jumpingStart = 3,
        jumping = 4,
        jumpingRun = 5,
        jumpingEnd = 6,
        action = 7,
        spraying = 8,
        falling = 9,
        lying = 10, 
        gettingUp = 11,
        dropping = 12,
        droppingEnd = 13
    }
    public abstract class State<T> where T : Controller<T> {
        private bool prevGround = false;
        private float timer = 0;
        protected T controller = null;
        public abstract bool modelFollowMoveDir {
            get;
        }
        public abstract StateId id {
            get;
        }
        public virtual int animatorStateId {
            get {
                return (int)id;
            }
        }
        public State() { }
        public void SetController(T controller) {
            this.controller = controller;
        }
        public void SetTimer(float timer) {
            this.timer = timer;
        }
        public float remainingDT {
            get {
                return -timer;
            }
        }
        public bool IsTimerUp() {
            return timer <= 0;
        }
        public bool Prepare(State<T> previousState, object arg = null) {
            prevGround = controller.isGrounded;
            if (prevGround) {
                if (StartGrounded()) return true;
            } else {
                if (StartUngrounded()) return true;
            }
            SetAnimation();
            _Prepare(previousState, arg);
            return false;
        }
        public virtual void SetAnimation() {
            controller.anim.ResetTrigger("changeState");
            controller.anim.SetInteger("nextState", animatorStateId);
            controller.anim.SetTrigger("changeState");
        }
        public virtual bool CheckTransition() {
            return _CheckTransition();
        }
        public virtual void Update(float dt) {
            if (prevGround != controller.isGrounded) {
                prevGround = controller.isGrounded;
                if (prevGround) {
                    if (OnGrounded()) {
                        return;
                    }
                } else {
                    if (OnUngrounded()) {
                        return;
                    }
                }
            }
            if (timer > 0) {
                timer -= dt;
                if (timer <= 0) {
                    if (OnTimerEnd(-timer)) {
                        return;
                    }
                }
            }
            _Update(dt);
        }
        public virtual State<T> Change(StateId nextState, bool force=false, object arg = null) {
            return _Change(nextState, force);
        }
        protected abstract void _Prepare(State<T> previousState, object arg = null);
        protected abstract bool _CheckTransition();
        protected abstract void _Update(float dt);
        protected abstract State<T> _Change(StateId nextState, bool force=false, object arg = null);

        protected abstract void _End(StateId nextState, bool force=false, object arg = null);

        public void End(StateId nextState, bool force=false, object arg = null){
            _End(nextState, force, arg);
        }
        //return true to return
        protected virtual bool StartGrounded() {
            return false;
        }
        protected virtual bool StartUngrounded() {
            return false;
        }
        protected virtual bool OnGrounded() {
            return false;
        }
        protected virtual bool OnUngrounded() {
            return false;
        }
        protected virtual bool OnTimerEnd(float remainingDT) {
            return false;
        }
    }
}