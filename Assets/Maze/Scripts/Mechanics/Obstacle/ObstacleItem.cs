using Maze.Database;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Maze.Mechanics.Obstacle {
    public abstract class ObstacleItem : MyBehaviour, IGameObject {
        public ObstacleItemGroup parent;
        public Statement statement;
        public Dialog dialog;

        public int obstacleItemId;

        public int hashCode;

        public void LoadStatement(Statement statement) {
            this.statement = statement;
            _LoadStatement(statement);
        }

        protected abstract void _LoadStatement(Statement statement);

        public abstract int state { get; set; }

        public void Save(bool complete) {

            using (Database.Cursor cur = Database.Database.GetCursor()) {
                Save(cur, complete);
            }
        }

        void Start(){
            hashCode = GetHashCode();
        }

        public void Read(Database.Cursor cur, Reader reader){
            Read(cur, reader, LevelManager.currentLevel.id, true);
        }
        public virtual void Read(Database.Cursor cur, Reader reader, int levelId, bool restoreState){
            Statement s = Problem.GetStatement(reader.GetInt32("ANSWER_ID"));
            LoadStatement(s);
            if(restoreState) state = reader.GetInt32("STATE");
            else state = reader.GetInt32("STATE_0");
        }
        public virtual void Save(Database.Cursor cur, bool complete) {
            string cmd = "INSERT  INTO OBSTACLE_ITEM (LEVEL_ID, OBSTACLE_ID_LEVEL, OBSTACLE_ITEM_GROUP_OBSTACLE, OBSTACLE_ITEM_ID_GROUP, ANSWER_ID, STATE) VALUES({0}, {1}, {2}, {3}, {4}, {5});";
            if(!complete) cmd = cmd.Replace("STATE", "STATE_0");
            try{
                if(cur == null) throw new Exception("CURSOR IS NULL");
                if(LevelManager.currentLevel == null) throw new Exception("LevelManager.currentLevel IS NULL");
                if(parent == null) throw new Exception("parent IS NULL");
                if(parent.parent == null) throw new Exception("parent.parent IS NULL");
                if(statement == null) throw new Exception("statement IS NULL");
                cur.commandText = string.Format(cmd,
                    LevelManager.currentLevel.id,
                    parent.parent.obstacleId,
                    parent.groupId,
                    obstacleItemId,
                    statement.id,
                    state
                    );
            }catch (Exception ex){
                throw new Exception(gameObject.name + " - " + this.GetHashCode() + " : " + ex.Message);
            }
            int ret = cur.ExecuteNonQuery();
        }
        public void SetDialogActiveTimed(bool active){
            dialog.SetActiveTimed(active);
        }
    }
}
