using Maze.Mechanics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Inputs.GUI {
    public class WorldCanvasHelper : MonoBehaviour {
        public static Quaternion y180 = Quaternion.AngleAxis(180, Vector3.up);
        public GameObject canvas;
        Transform canvasTrans;
        CanvasGroup canvasGroup;
        GraphicRaycaster raycaster;

        public enum FadeState {
            inactive,
            fadeIn,
            active,
            fadeOut
        }

        public FadeState fadeState = FadeState.inactive;
        public bool shouldBeActive = false;
        public float timer = 0;
        public float fadeTimer = 0.5f;

        public float activeTimer = 0;

        // Use this for initialization
        void Start() {
            canvasTrans = canvas.transform;
            canvasGroup = canvas.GetComponent<CanvasGroup>();
            raycaster = canvas.GetComponent<GraphicRaycaster>();
            SetCanvasActive(false);
        }

        public void SetCanvasActive(bool active) {
            StartCoroutine("_SetCanvasActive", active);
        }

        IEnumerator _SetCanvasActive(bool active) {
            yield return new WaitForEndOfFrame();
            canvas.SetActive(active);
            //EnableInteraction(active);
            //timer = 0;
            yield return null;
        }

        private void Update() {
            switch (fadeState) {
                case FadeState.fadeIn: {
                    timer -= Time.deltaTime;
                    if (timer <= 0) {
                        EnableInteraction(true);
                        timer = 0;
                    } else {
                        canvasGroup.alpha = 1 - (timer / fadeTimer);
                    }
                    break;
                }
                case FadeState.fadeOut: {
                    timer -= Time.deltaTime;
                    if (timer <= 0) {
                        SetCanvasActive(false);
                        timer = 0;
                    } else {
                        canvasGroup.alpha = timer / fadeTimer;
                    }
                    break;
                }
                case FadeState.active: {
                    if (nearCamera) {
                        _SetActive(false);
                    }
                    break;
                }
                case FadeState.inactive: {
                    if (shouldBeActive && !nearCamera) {
                        _SetActive(true);
                    }
                    break;
                }
            }
            if (activeTimer > 0) {
                activeTimer -= Time.deltaTime;
                if (activeTimer < 0) {
                    activeTimer = 0;
                    SetActive(!shouldBeActive);
                }
            }
        }

        bool nearCamera {
            get {
                return Vector3.Project(canvasTrans.position - Camera.main.transform.position, canvasTrans.forward).magnitude < 2;
            }
        }

        // Update is called once per frame
        void LateUpdate() {
            //trans.LookAt(Camera.main.transform);
            //trans.Rotate(Vector3.up, 180, Space.Self);
            Quaternion camRot = Camera.main.transform.rotation;
            canvasTrans.LookAt(canvasTrans.position + camRot * Vector3.forward, camRot * Vector3.up);
        }

        void EnableInteraction(bool enable) {
            raycaster.enabled = enable;
            canvasGroup.alpha = enable ? 1 : 0;
            canvasGroup.interactable = enable;
            canvasGroup.blocksRaycasts = enable;
            fadeState = enable ? FadeState.active : FadeState.inactive;
        }

        void _SetActive(bool active) {
            canvas.SetActive(true);
            EnableInteraction(false);
            fadeState = active ? FadeState.fadeIn : FadeState.fadeOut;
            timer = fadeTimer - timer;
        }

        public void SetActive(bool active) {
            if (shouldBeActive == active) {
                return;
            }
            shouldBeActive = active;
            activeTimer = 0;
            _SetActive(active);
        }
        
        public void SetActiveTimed(bool active) {
            SetActive(active);
            this.activeTimer = 5;
        }

        HashSet<PlayerDetector> detectors = new HashSet<PlayerDetector>();

        public void SetActive(PlayerDetectedEventData data) {
            if (data.playerInside) {
                detectors.Add(data.source);
            } else {
                detectors.Remove(data.source);
            }
            SetActive(data.playerInside);
        }
    }
}