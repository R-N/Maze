using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Database;
using Maze.Inputs;

namespace Maze.Mechanics.Obstacle.ObstacleItems {
    public class ObstacleButtonTile : ObstacleItem {
        public ButtonTile buttonTile;

        public ButtonEvent onPressCorrect;
        public ButtonEvent onPressWrong;


        public override int state {
            get {
                return (int)buttonTile.state;
            }
            set {
                buttonTile.state = (ButtonState)value;
            }
        }

        protected override void _LoadStatement(Statement statement) {
            dialog.SetText(statement.text);
            dialog.SetTitle("Tile");

            buttonTile.SetButtonEventHandler(statement.correct ? onPressCorrect : onPressWrong);
        }

        public void OnPress(ButtonEventData data) {
            if (statement.correct) {
                onPressCorrect.Invoke(data);
            } else {
                onPressWrong.Invoke(data);
            }
        }
    }
}
