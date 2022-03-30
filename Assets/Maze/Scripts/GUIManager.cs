using System;
using System.Collections.Generic;
using Maze.Inputs.GUI;
using UnityEngine;
using UnityEngine.UI;

namespace Maze {
    public  class GUIManager : MonoBehaviour{
        public static  GUIManager instance;

        public ButtonInput sprayButton;
        public Joystick sprayAim;
        public ButtonInput jumpButton;
        public ToggleInput runToggle;
        public Joystick moveJoystick;
        public SwipePanel lookPanel;

        public CancelArea cancelArea;

        public Image hpBarFill;
        public Text sprayCharge;
        public Image sprayRecharge;

        public RectTransform center;
        public RectTransform toastSpawn;

        public Toast toastPrefab;

        public static bool invertLookAxis = false;

        public static void SpawnToast(string text, string title=null){
            Toast toast = Instantiate<Toast>(instance.toastPrefab, instance.toastSpawn.position, instance.toastSpawn.rotation);
            //Util.CopyTransform(instance.toastSpawn, toast.transform);
            toast.SetMovement(instance.toastSpawn, instance.center);
            Util.ParentAndCenter(toast.transform, instance.toastSpawn, false);
            toast.SetText(text);
            if(title != null) toast.SetTitle(title);
        }

        private void Awake() {
            GUIManager.instance = this;
            
            moveJoystickAutoHide = GameSettings.moveJoystickAutoHide;
            moveJoystickStatic = GameSettings.moveJoystickStatic;
        }

        public static Joystick.AutoHideType moveJoystickAutoHide{
            get{
                return instance.moveJoystick.autoHide;
            }
            set{
                if(instance) instance.moveJoystick.autoHide = value;
            }
        }

        public static bool moveJoystickStatic{
            get{
                return instance.moveJoystick.staticAnchor;
            }
            set{
                if(instance) instance.moveJoystick.staticAnchor = value;
            }
        }

        public static bool UseCancelArea(ICancelable user) {
            return instance.cancelArea.Use(user);
        }
        public static bool? ReleaseCancelArea(ICancelable user, Vector2 point) {
            return instance.cancelArea.Release(user, point);
        }

        public static bool? IsInCancelArea(ICancelable user, Vector2 point) {
            return instance.cancelArea.IsPointInArea(user, point);
        }

        public static void SetHPFill(float percent) {
            instance.hpBarFill.fillAmount = percent;
        }

        public static void SetSprayCharge(int charge) {
            instance.sprayCharge.text = charge.ToString();
        }

        static float lastSprayRecharge = 1;
        public static void SetSprayRecharge(float percent) {
            if (lastSprayRecharge == percent) {
                return;
            }
            instance.sprayRecharge.fillAmount = percent;
        }
    }
}
