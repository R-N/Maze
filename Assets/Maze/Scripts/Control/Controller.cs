using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.States;

namespace Maze.Control {
    public abstract class Controller<T> : Controller where T : Controller<T> {
        private State<T> _state;
        protected State<T> state {
            get {
                return _state;
            }
            set {
                _state = value;
            }
        }

        public override StateId stateId {
            get {
                return state.id;
            }
        }

        public override bool ChangeState(StateId nextState, bool force=false, float dt=0) {
            Debug.Log("Change state to " + nextState);
            State<T> newState = state.Change(nextState, force);
            if (newState == null) {
                Debug.Log("Fail to change state state to " + nextState + ".\nCurrent state: " + state.id);
                return false;
            }
            return ChangeState(newState, force, dt);
        }
        public bool ChangeState(State<T> newState, bool force = false, float dt = 0) {
            State<T> oldState = state;
            newState.SetController((T)this);
            state = newState;
            if(oldState != null) oldState.End(newState.id, force);
            if (newState.Prepare(oldState)) {
                return true;
            }
            modelFollowMoveDir = state.modelFollowMoveDir;
            if (dt > 0) state.Update(dt);
            return true;
        }

        protected sealed override void Update() {

            float dt = Time.deltaTime;

            status.Reset();
            status.UpdateBuffs(dt);

            PrepareUpdate();

            state.CheckTransition();

            state.Update(dt);

            model.localRotation = Quaternion.RotateTowards(model.localRotation, modelTargetRotation, maxRotationSpeedDeg * dt);

            UpdateGravity(dt);

            PostUpdate();

        }
    }
    public abstract class Controller : MonoBehaviour {
        public Status status;
        public Animator anim;
        public Transform model;
        public Transform camHolder;
        public Transform head;

        public abstract StateId stateId {
            get;
        }

        public void ThrowOff(Vector3 velocity) {
            moveDirection = velocity.NullifyY();
            AddGravityVelocity(velocity.y);
            ChangeState(StateId.falling, true);
        }

        public abstract float stepOffset {get;
        }

        Vector3 _moveDirection;
        public Vector3 moveDirection {
            get {
                return _moveDirection;
            }
            set {
                //Debug.Log("Set moveDir to: " + value);
                _moveDirection = value;
                Vector3 dir = transform.InverseTransformDirection(value).NullifyY().normalized;
                anim.SetFloat("dirMag", dir.magnitude);
               // anim.SetFloat("dirX", dir.x);
                //anim.SetFloat("dirY", dir.z);
                //anim.SetBool("dirZero", dir.x == 0 && dir.z == 0);
                if (modelFollowMoveDir) {
                    ModelFollowMoveDir(dir.x, dir.z);
                }
            }
        }

        public void NullifyDir() {
            moveDirection = Vector3.zero;
        }

        public Quaternion modelTargetRotation;
        public float maxRotationSpeedDeg = 30;

        public void ModelFollowMoveDir(float x, float z) {
            if (x == 0 && z == 0) {
                modelTargetRotation = Quaternion.identity;
            } else {
                modelTargetRotation = Quaternion.AngleAxis(90 - Mathf.Rad2Deg * Mathf.Atan2(z, x), Vector3.up);
            }
        }

        [SerializeField]
        bool _modelFollowMoveDir = false;
        public bool modelFollowMoveDir {
            get {
                return _modelFollowMoveDir;
            }
            set {
                if (_modelFollowMoveDir == value) return;
                _modelFollowMoveDir = value;
                if (value) {
                    Vector3 dir = transform.InverseTransformDirection(moveDirection).normalized;
                    ModelFollowMoveDir(dir.x, dir.z);
                } else {
                    model.localRotation = Quaternion.identity;
                }
            }
        }

        public abstract float height { get; }
        public abstract float radius { get; }
        

        public bool ChangeState(StateId nextState, float dt) {
            return ChangeState(nextState, false, dt);
        }
        public abstract bool ChangeState(StateId nextState, bool force = false, float dt = 0);

        public Vector3 lookDirection {
            get {
                return transform.localToWorldMatrix * localLookDirection;
            }
            set {
                localLookDirection = transform.worldToLocalMatrix * value;
            }
        }
        //dont use this yet
        public Vector3 localLookDirection {
            get {
                if (camHolder == transform) {
                    return transform.forward;
                } else {
                    return bodyLocalRotation * headLocalRotation * Vector3.forward;
                }
            }
            protected set {
                if (camHolder == transform) {
                    bodyLocalRotation = Quaternion.FromToRotation(Vector3.forward, value);
                } else {
                    Quaternion rot = Quaternion.LookRotation(value, Vector3.up);
                    //bodyLocalRotation = new Quaternion(0, rot.y, 0, rot.w).normalized;
                    //headLocalRotation = new Quaternion(rot.x, 0, 0, rot.w).normalized;
                    //Vector3 bodyLook = value.NullifyY().normalized;
                    //bodyLocalRotation = Quaternion.LookRotation(bodyLook);
                    //headLocalRotation = Quaternion.Inverse(bodyLocalRotation) * Quaternion.LookRotation(value);
                    //Quaternion headRot = Quaternion.FromToRotation(bodyLook, value);
                    //headLocalRotation = Quaternion.Euler(headRot.ToEuler().NullifyZ());
                    Vector3 eul = rot.eulerAngles;
                    bodyLocalRotation = Quaternion.Euler(0, eul.y, eul.z);
                    headLocalRotation = Quaternion.Euler(eul.x, 0, 0);
                }
            }
        }
        public Quaternion bodyRotation {
            get {
                return transform.rotation;
            }
            set {
                transform.rotation = value;
            }
        }
        public Quaternion headRotation {
            get {
                return camHolder.rotation;
            }
            set {
                camHolder.rotation = value;
            }
        }
        public Quaternion bodyLocalRotation {
            get {
                return transform.localRotation;
            }
            private set {
                transform.localRotation = value;
            }
        }
        public Quaternion headLocalRotation {
            get {
                return camHolder.localRotation;
            }
            set {
                camHolder.localRotation = value;
            }
        }

        public abstract InputSource inputSource {
            get;
        }

        public abstract bool isGrounded {
            get;
        }


        public abstract void AddGravityVelocity(float gravVel);
        public abstract void SetGravityVelocity(float gravVel);
        public abstract void PrepareUpdate();
        public abstract void UpdateGravity(float dt);
        public abstract void Move(Vector3 movement);

        public abstract void PostUpdate();

        protected abstract void Update();
    }
}