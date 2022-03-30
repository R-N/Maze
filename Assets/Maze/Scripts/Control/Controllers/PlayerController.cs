using Maze.Control.Buffs;
using Maze.Inputs.Sources;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control.States;
using Maze.Control.States.Player;

namespace Maze.Control.Controllers {
    public class PlayerController : CCController<PlayerController> {
        public static PlayerController instance = null;



        public float camRotXMax = 30;
        public float camRotXMin = -16;
        float camRot = 0;






        public override InputSource inputSource {
            get {
                return UserInput.instance;
            }
        }


        private void OnDestroy() {
            if (instance == this) instance = null;
        }

        // Use this for initialization
        void Start() {
            instance = this;

            camRot = camHolder.localEulerAngles.x;

            ChangeState(new States.Player.Jumping(), true);

            status.AddBuff(new Spray());
        }

        public bool UpdateMoveDirection(float dt) {
            moveDirection = transform.rotation * new Vector3(
                CrossPlatformInputManager.GetAxis("Horizontal") + Input.GetAxis("Horizontal"),
                0,
                CrossPlatformInputManager.GetAxis("Vertical") + Input.GetAxis("Vertical")
                );
            moveDirection = moveDirection.NullifyY().normalized;

            if (moveDirection.sqrMagnitude == 0) {
                return false;
            }
            return true;
        }



        public bool UpdateLookDirection(float dt) {
            float mouseX = GameSettings.lookSensitivityX * CrossPlatformInputManager.GetAxis("Look X");
            float mouseY = -GameSettings.lookSensitivityY * CrossPlatformInputManager.GetAxis("Look Y");

            if (mouseX == 0 && mouseY == 0) {
                return false;
            }

            if (GameSettings.invertLookAxes) {
                mouseX = -mouseX;
                mouseY = -mouseY;
            }


            Quaternion yRotDelta = Quaternion.AngleAxis(mouseX, Vector3.up);

            transform.localRotation = yRotDelta * transform.localRotation;

            float prevCamRot = camRot;

            camRot = Mathf.Clamp(camRot + mouseY, camRotXMin, camRotXMax);
            float deltaCamRot = camRot - prevCamRot;
            if (mouseX == 0 && deltaCamRot == 0) {
                return false;
            }

            Quaternion xRotDelta = Quaternion.AngleAxis(deltaCamRot, Vector3.right);

            camHolder.localRotation = xRotDelta * camHolder.localRotation;
            head.localRotation = xRotDelta * head.localRotation;

            //localLookDirection = yRotDelta * xRotDelta * localLookDirection;

            return true;
        }


        public void UpdateWalk(float dt) {
            movement += moveDirection * status.moveSpeed * dt;
        }


    }
}