using Maze.Mechanics.Obstacle;
using Maze.Inputs.GUI;

namespace Maze
{
    public static class GameSettings
    {
        public static int problemDifficulty = 0;
        public static Category[] problemCategories =null;
        public static bool invertLookAxes = false;

        public static float lookSensitivityX = 40;
        public static float lookSensitivityY = 10;

        private static Joystick.AutoHideType _moveJoystickAutoHide = Joystick.AutoHideType.alwaysVisible;
        private static bool _moveJoystickStatic = true;
        public static Joystick.AutoHideType moveJoystickAutoHide{
            get{
                return _moveJoystickAutoHide;
            }
            set{
                _moveJoystickAutoHide = value;
                GUIManager.moveJoystickAutoHide = value;
            }
        }

        public static bool moveJoystickStatic{
            get{
                return _moveJoystickStatic;
            }
            set{
                _moveJoystickStatic = value;
                GUIManager.moveJoystickStatic = value;
            }
        }
    }
}