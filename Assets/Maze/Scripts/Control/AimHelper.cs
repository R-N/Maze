using Maze.Control.Controllers;
using Maze.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Control {
    public class AimHelper : MonoBehaviour {

        public static AimHelper instance = null;
        public Controller controller;
        public Decal sprayAim = null;
        Transform sprayTrans = null;
        public bool _sprayAiming = false;

        public static LayerMask mask;
        public LayerMask _mask;
        bool sprayAiming {
            get {
                return _sprayAiming;
            }
            set {
                _sprayAiming = value;
                if (sprayActive != value) {
                    sprayActive = value;
                }
            }
        }
        bool _sprayActive = false;
        bool sprayActive {
            get {
                return _sprayActive;
            }
            set {
                _sprayActive = value;
                sprayAim.enabled = true;
                sprayAim.gameObject.SetActive(value);
            }
        }

        private void Awake(){
            mask = _mask;
            instance = this;
            sprayTrans = sprayAim.transform;
        }

        private void LateUpdate() {
            if (sprayAiming) {
                RaycastHit hit;
                if (Physics.Raycast(controller.camHolder.position, controller.camHolder.forward, out hit, controller.status.attackRange, mask)) {
                    if (!sprayActive) sprayActive = true;
                    sprayTrans.position = hit.point + hit.normal * 0.05f;
                    sprayTrans.rotation = Quaternion.LookRotation(-hit.normal, controller.camHolder.up);
                    sprayAim.BuildDecal();
                } else {
                    if (sprayActive) sprayActive = false;
                }
            }
        }

        public static void AimSpray(bool aim) {
            instance.sprayAiming = aim;
        }
    }
}