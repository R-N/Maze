using Maze.Database;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Maze.Level {
    public enum LevelType{
        preLevel,
        bossLevel,
        postLevel
    }
    public class LevelData {

        public int id;
        public string scene;
        public int number;
        public Sprite thumbnail;
        public LevelPackData parent;
        
        public LevelType type;

        public bool completed = false;

        public LevelData preLevel = null;

        public LevelData(){}
        public LevelData(Database.Cursor cur, Reader reader){
            Read(cur, reader);
        }

        public void Read(Database.Cursor cur, Reader reader) {
            id = (int)reader.GetInt32("LEVEL_ID");
            scene = reader.GetString("SCENE");
            number = reader.GetInt32("NUMBER");
            //TODO: thumbnail
            type = (LevelType)reader.GetInt32("TYPE");

            completed = reader.GetInt64("COMPLETED") != 0;

            if(type == LevelType.postLevel){
                preLevel = Level.GetLevelData((int)reader.GetInt32("PRE_LEVEL"));
            }

            SetParent(LevelPack.GetLevelPackData((int)reader.GetInt32("LEVEL_PACK_ID")));

            Level.dataCache[id] = this;
        }

        public void SetParent(LevelPackData parent){
            this.parent = parent;
        }

        public LevelData prevLevel{
            get{
                if(!hasPrevLevel) return null;
                return parent.levels[number-2];
            }
        }

        public LevelData nextLevel{
            get{
                if(!hasNextLevel) return null;
                return parent.levels[number];
            }
        }
        public bool hasNextLevel{
            get{
                return parent.levels.Length > number;
            }
        }
        public bool hasPrevLevel{
            get{
                return number > 1;
            }
        }

        public bool unlocked{
            get{
                if(!hasPrevLevel) return parent.unlocked;
                return prevLevel.completed;
            }
        }

    }
    public class Level : MonoBehaviour{
        
        public static Dictionary<int, LevelData> dataCache = new Dictionary<int, LevelData>();
        LevelData data;

        public Text text;
        public Image image;

        public Button button;
        

        public void OnClick(){
            LevelManager.LoadLevel(data);
        }

        public void Init(){
            text.text = data.number.ToString();
            button.interactable = data.unlocked;
            //TODO: levelImage
        }
        public static LevelData ReadStatic(Database.Cursor cur, Reader reader)
        {
            LevelData data;
            int id = (int)reader.GetInt32("LEVEL_ID");
            if(dataCache.ContainsKey(id)){
                data = dataCache[id];
            }else{
                data = new LevelData();
            }
            data.Read(cur, reader);
            return data;
        }

        public void SetData(LevelData data){
            this.data = data;
            Init();
        }
        public void Read(Database.Cursor cur, Reader reader)
        {
            SetData(ReadStatic(cur, reader));
        }

        public static LevelData GetLevelData(int id){
            if(dataCache.ContainsKey(id)){
                return dataCache[id];
            }else{
                throw new System.NotImplementedException();
                //TODO: Get LevelData
            }
        }
        public void SetParent(LevelPackData parent){
            data.parent = parent;
        }

        public void SetParent(Transform parent, bool worldPostionStays = false){
            transform.SetParent(parent, worldPostionStays);
        }

    
    }
}