using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Maze.Database;

namespace Maze.Mechanics.Obstacle {
    public class ObstacleItemGroup : MyBehaviour {
        public Obstacle parent;
        public ObstacleItem[] obstacleItems;
        public int groupId;
        public int correctAnswerCount = 1;
        public int preferredProblemCount = 1;

        public void Init() {
            for (int i = 0; i < obstacleItems.Length; ++i) {
                obstacleItems[i].parent = this;
                obstacleItems[i].obstacleItemId = i;
            }
        }

        public void LoadProblems(Problem[] problems) {
            problems = Util.RandomFilter(problems, preferredProblemCount);
            List<Statement> correctStatements = new List<Statement>();
            List<Statement> wrongStatements = new List<Statement>();
            for (int i = 0; i < problems.Length; ++i) {
                Util.AddToList(correctStatements, problems[i].GetStatements(true, correctAnswerCount));
                Util.AddToList(wrongStatements, problems[i].GetStatements(false, 5));
            }

            Pool<Statement> correctPool = new Pool<Statement>(correctStatements);
            Pool<Statement> wrongPool = new Pool<Statement>(wrongStatements);
            int maxCorrect = Mathf.Min(correctPool.Length + correctAnswerCount);

            List<Statement> statements = new List<Statement>();

            for (int i = 0; i < maxCorrect; ++i) {
                statements.Add(correctPool.Get());
            }
            for (int i = maxCorrect; i < obstacleItems.Length; ++i) {
                statements.Add(wrongPool.Get());
            }

            Pool<Statement> pool = new Pool<Statement>(statements);
            
            for (int i = 0; i < obstacleItems.Length; ++i) {
                obstacleItems[i].LoadStatement(pool.Get());
            }
        }
        public void RetrieveItems(bool restoreState=true) {
            using (Database.Cursor cur = Database.Database.GetCursor()) {
                RetrieveItems(cur, LevelManager.currentLevel.id, restoreState);
            }
        }

        public void RetrieveItems(Database.Cursor cur, int levelId, bool restoreState=true) {
            cur.commandText = String.Format("SELECT * FROM OBSTACLE_ITEM WHERE LEVEL_ID={0} AND OBSTACLE_ID_LEVEL={1} AND OBSTACLE_ITEM_GROUP_OBSTACLE={2} ORDER BY OBSTACLE_ITEM_ID_GROUP", levelId, parent.obstacleId, groupId);
            using (Reader reader = cur.ExecuteReader()) {
                Read(cur, reader, levelId, restoreState);
            }
        }

        public void Read(Database.Cursor cur, Reader reader){
            Read(cur, reader, LevelManager.currentLevel.id, true);
        }
        public void Read(Database.Cursor cur, Reader reader, int levelId, bool restoreState){
            for(int i = 0; i < obstacleItems.Length; ++i){
                if(!reader.Read()) throw new Exception(string.Format("OUT OF RANGE. Level: {0}, Obstacle: {1}, ObstacleGroup: {2}, ObstacleItem: {3}",levelId, parent.obstacleId, groupId, i));
                obstacleItems[i].Read(cur, reader, levelId, restoreState);
            }
        }

        public void Save(bool complete) {

            using (Database.Cursor cur = Database.Database.GetCursor()) {
                Save(cur, complete);
            }
        }
        public void Save(Database.Cursor cur, bool complete) {
            for (int i = 0; i < obstacleItems.Length; ++i) {
                obstacleItems[i].Save(cur, complete);
            }
        }

        public void SetDialogsActiveTimed(bool active){
            for (int i = 0; i < obstacleItems.Length; ++i) {
                obstacleItems[i].SetDialogActiveTimed(active);
            }
        }
    }
}
