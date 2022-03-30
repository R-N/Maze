using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control {
    public enum BuffId {
        spray=1,
        moveSpeed,
        key,
        heal,
        burn
    }
    public abstract class Buff {

        public bool continuous;
        public float duration;
        private int _stack = 1;
        public int stack {
            get {
                return _stack;
            }
            set {
                _stack = value;
            }
        }

        public abstract BuffId id {
            get;
        }

        protected Status status = null;
        
        
        public Buff(float duration, int stack=1) {
            if (duration <= 0) {
                continuous = true;
            } else {
                continuous = false;
                this.duration = duration;
            }
            this._stack = stack;
        }

        public void SetStatus(Status status) {
            this.status = status;
        }
        public abstract void Start();
        public virtual void Update(float dt) {
            if(!continuous) duration -= dt;
            _Update(dt);
        }
        public virtual bool shouldBeRemoved {
            get {
                return !continuous && (duration <= 0 || stack == 0);
            }
        }
        protected abstract void _Update(float dt);
        public abstract bool End();
        public abstract void Stack(Buff buff);
        public bool TakeStack(int stack) {
            bool ret = _TakeStack(stack);
            if (shouldBeRemoved) {
                status.RemoveBuff(id);
            }
            return ret;
        }
        protected virtual bool _TakeStack(int stack) {
            bool ret = this.stack >= stack;
            if(ret) this.stack -= stack;
            return ret;
        }
        public virtual int CheckStack(int arg = 0) {
            return stack;
        }
    }
}