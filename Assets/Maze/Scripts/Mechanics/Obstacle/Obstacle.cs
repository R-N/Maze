using Maze.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze.Mechanics.Obstacle {
    public class Obstacle : MyBehaviour, IGameObject{
        public int obstacleId;
        public bool groupsShareProblems = false;

        public int preferredProblemCount {
            get {
                int c = groupsShareProblems ? obstacleItemGroups.Length : 0;
                for (int i = 0; i < obstacleItemGroups.Length; ++i) {
                    if (groupsShareProblems) {
                        c = Mathf.Max(c, obstacleItemGroups[i].preferredProblemCount);
                    } else {
                        c += obstacleItemGroups[i].preferredProblemCount;
                    }
                }
                return c;
            }
        }
        [SerializeField]
        private int _position;
        public int position {
            get {
                return _position;
            }
            set {
                SetPosition(value);
            }
        }
        


        public void SetPosition(int position) {
            _position = position;
            Util.ParentAndCenter(transform, LevelManager.GetObstaclePosition(position));
        }

        public ObstacleItemGroup[] obstacleItemGroups;

        public virtual void Init() {
            for (int i = 0; i < obstacleItemGroups.Length; ++i) {
                obstacleItemGroups[i].parent = this;
                obstacleItemGroups[i].groupId = i;
                obstacleItemGroups[i].Init();
            }
        }

        public void LoadProblems(Problem[] probs) {
            Pool<Problem> pool = new Pool<Problem>(probs);
            for (int i = 0; i < obstacleItemGroups.Length; ++i) {
                obstacleItemGroups[i].LoadProblems(pool.Get(obstacleItemGroups[i].preferredProblemCount));
            }
        }

        public void Read(Database.Cursor cur, Reader reader){
            Read(cur, reader, LevelManager.currentLevel.id, true);
        }

        public void Read(Database.Cursor cur, Reader reader, int levelId, bool restoreState) {
            position = reader.GetInt32("OBSTACLE_POS");
            RetrieveItems(cur, levelId,restoreState);
        }
        public void RetrieveItems(int levelId, bool restoreState = true){
            using (Database.Cursor cur = Database.Database.GetCursor()){
                RetrieveItems(cur, levelId, restoreState);
            }
        }
        public void RetrieveItems(Database.Cursor cur, int levelId, bool restoreState = true){
            
            cur.commandText = string.Format("SELECT * FROM OBSTACLE_ITEM WHERE LEVEL_ID={0} AND OBSTACLE_ID_LEVEL={1} ORDER BY OBSTACLE_ITEM_GROUP_OBSTACLE, OBSTACLE_ITEM_ID_GROUP", levelId, obstacleId);
            using(Reader reader2 = cur.ExecuteReader()){
                for(int i = 0; i < obstacleItemGroups.Length; ++i){
                    //reader.Read();
                    obstacleItemGroups[i].Read(cur, reader2, levelId, restoreState);
                }
            }
        }

        public void Save(bool complete) {
            using (Database.Cursor cur = Database.Database.GetCursor()) {
                Save(cur, complete);
            }
        }

        public void Save(Database.Cursor cur, bool complete) {
            cur.commandText = String.Format("INSERT INTO OBSTACLE (LEVEL_ID, OBSTACLE_ID_LEVEL, NAME, OBSTACLE_POS) VALUES({0}, {1}, \"{2}\", {3});", LevelManager.currentLevel.id, obstacleId, gameObject.name.Replace("(Clone)", ""), position);
            
            int ret = cur.ExecuteNonQuery();

            for (int i = 0; i < obstacleItemGroups.Length; ++i) {
                obstacleItemGroups[i].Save(cur, complete);
            }
        }
    }
}
