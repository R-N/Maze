using Maze.Inputs;
using UnityEngine;
namespace Maze.Mechanics.Obstacle.ObstacleItems {
    public class Room : ObstacleItem {
        public Chest chest;
        public Door door;
        public bool isKeyRoom = false;

        public Transform obstaclePosition = null;

        public void Init(){
            door.Init();
        }

        public override int state {
            get {
                return chest.state;
            }
            set {
                chest.state = value;
            }
        }

        protected override void _LoadStatement(Statement statement) {
            chest.LoadStatement(statement);
        }

        public override void Save(Database.Cursor cur, bool complete){
            if(chest) chest.Save(cur, complete);
        }

        public override void Read(Database.Cursor cur, Database.Reader reader, int levelId, bool restoreState){
            if(chest != null){
                chest.Read(cur, reader, levelId, restoreState);
            }
        }
    }
}
