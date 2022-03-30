using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics {
    public enum MovingBlockState {
        ready,
        moving,
        done,
        returning
    }
    public class MovingBlock : MyBehaviour {
        public Transform blockTransform;
        public Transform startTransform;
        public Transform targetTransform;

        public float doneTime = 2;
        public float moveSpeed = 10;
        public float angularSpeed = 10;
        //public float scaleSpeed = 0.2f;
        public float returnSpeedMultiplier = 0.2f;

        float timer = 0;

        public bool autoReturn = true;

        public MovingBlockState state;

        public bool lateUpdate = false;

        public void ChangeState(MovingBlockState target) {
            switch (state) {
                case MovingBlockState.ready: {
                    blockTransform.position = startTransform.position;
                    blockTransform.rotation = startTransform.rotation;
                    //blockTransform.localScale = startTransform.localScale;
                    break;
                }
                case MovingBlockState.done: {
                    blockTransform.position = targetTransform.position;
                    blockTransform.rotation = targetTransform.rotation;
                    //blockTransform.localScale = targetTransform.localScale;
                    break;
                }
            }
            state = target;
        }

        public void Move (){
            ChangeState(MovingBlockState.moving);
        }

        public void Update(){
            if(!lateUpdate) MyUpdate();
        }

        public void LateUpdate(){
            if(lateUpdate) MyUpdate();
        }

        public void MyUpdate() {
            float dt = Time.deltaTime;
            switch (state) {
                case MovingBlockState.moving: {
                    blockTransform.position = Vector3.MoveTowards(blockTransform.position, targetTransform.position, moveSpeed * dt);
                    blockTransform.rotation = Quaternion.RotateTowards(blockTransform.rotation, targetTransform.rotation, angularSpeed * dt);
                    //blockTransform.localScale = Vector3.MoveTowards(blockTransform.localScale, targetTransform.localScale, scaleSpeed * dt);

                    if (blockTransform.position == targetTransform.position
                        && blockTransform.rotation == targetTransform.rotation
                        //&& blockTransform.localScale == targetTransform.localScale
                        ) {
                        if (autoReturn) {
                            timer = doneTime;
                        }
                        ChangeState(MovingBlockState.done);
                    }
                    break;
                }
                case MovingBlockState.returning: {
                    blockTransform.position = Vector3.MoveTowards(blockTransform.position, startTransform.position, moveSpeed * dt);
                    blockTransform.rotation = Quaternion.RotateTowards(blockTransform.rotation, startTransform.rotation, angularSpeed * dt);
                    //blockTransform.localScale = Vector3.MoveTowards(blockTransform.localScale, startTransform.localScale, scaleSpeed * dt);

                    if (blockTransform.position == startTransform.position
                        && blockTransform.rotation == startTransform.rotation
                        //&& blockTransform.localScale == startTransform.localScale
                        ) {
                        ChangeState(MovingBlockState.ready);
                    }
                    break;
                }
                case MovingBlockState.done:{
                    if (autoReturn) {
                        timer -= dt;
                        if (timer <= 0) {
                            timer = 0;
                            ChangeState(MovingBlockState.returning);
                        }
                    }
                    break;
                }
            }
        }
    }
}
