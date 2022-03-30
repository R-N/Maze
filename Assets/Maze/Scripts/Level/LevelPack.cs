using Maze.Database;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Level {
    public class LevelPackData {

        public int id;
        public string name;
        public Sprite background;
        public Sprite thumbnail;
        public LevelData[] preLevels;
        public LevelData treasure;
        public LevelData[] postLevels;

        public LevelData[] levels;
        
        public bool unlocked = false;

        public LevelPackData(){}

        public LevelPackData(Database.Cursor cur, Reader reader){
            Read(cur, reader);
        }

        public void Read(Database.Cursor cur, Reader reader) {
            id = (int)reader.GetInt32("LEVEL_PACK_ID");
            name = reader.GetString("NAME");
            unlocked = reader.GetBool("UNLOCKED");

            LevelPack.dataCache[id] = this;
            //TODO: background
            //TODO: thumbnail

            RetrieveItems(cur);
        }

        public void RetrieveItems(){

            using(Database.Cursor cur = Database.Database.GetCursor()){
                RetrieveItems(cur);
            }
        }
        public void RetrieveItems(Database.Cursor cur){
            List<LevelData> levels= new List<LevelData>();

            List<LevelData> preLevels = new List<LevelData>();
            cur.commandText = string.Format("SELECT *, (SELECT COUNT(*) FROM LEVEL_COMPLETION WHERE LEVEL_COMPLETION.LEVEL_ID=LEVEL.LEVEL_ID) AS COMPLETED FROM LEVEL WHERE LEVEL_PACK_ID={0} AND TYPE={1} ORDER BY NUMBER", id, (int)LevelType.preLevel);
            using(Reader reader = cur.ExecuteReader()){
                while(reader.Read()){
                    LevelData data = Level.ReadStatic(cur, reader);
                    data.SetParent(this);
                    preLevels.Add(data);
                    levels.Add(data);
                }
            }
            this.preLevels = preLevels.ToArray();

            cur.commandText = string.Format("SELECT *, (SELECT COUNT(*) FROM LEVEL_COMPLETION WHERE LEVEL_COMPLETION.LEVEL_ID=LEVEL.LEVEL_ID) AS COMPLETED FROM LEVEL WHERE LEVEL_PACK_ID={0} AND TYPE={1} LIMIT 1", id, (int)LevelType.bossLevel);
            using(Reader reader = cur.ExecuteReader()){
                while(reader.Read()){
                    treasure = Level.ReadStatic(cur, reader);
                    treasure.SetParent(this);
                    levels.Add(treasure);
                }
            }
            
            List<LevelData> postLevels = new List<LevelData>();
            cur.commandText = string.Format("SELECT *, (SELECT COUNT(*) FROM LEVEL_COMPLETION WHERE LEVEL_COMPLETION.LEVEL_ID=LEVEL.LEVEL_ID) AS COMPLETED FROM LEVEL WHERE LEVEL_PACK_ID={0} AND TYPE={1} ORDER BY NUMBER", id, (int)LevelType.postLevel);
            using(Reader reader = cur.ExecuteReader()){
                while(reader.Read()){
                    LevelData data = Level.ReadStatic(cur, reader);
                    data.SetParent(this);
                    postLevels.Add(data);
                    levels.Add(data);
                }
            }
            this.postLevels = postLevels.ToArray();

            this.levels = levels.ToArray();
        }
        public void CheckUnlock(){
            using(Database.Cursor cur = Database.Database.GetCursor()){
                CheckUnlock(cur);
            }
        }
        public void CheckUnlock(Database.Cursor cur){
            throw new System.NotImplementedException();
            //TODO
        }
    }
    public class LevelPack : MonoBehaviour{

        public static Dictionary<int, LevelPackData> dataCache = new Dictionary<int, LevelPackData>();
        public Text text;
        public Image image;
        public Button button;
        LevelPackData data;

        public LevelPackMenu menu;

        public void SetData(LevelPackData data){
            this.data =data;
        }

        public void Init(){
            text.text = data.name;
            button.interactable = data.unlocked;
            //TODO: levelImage
        }
        public void OnClick(){
            menu.LoadLevelPack(data);
        }

        public void Read(Database.Cursor cur, Reader reader)
        {
            int id = (int)reader.GetInt32("LEVEL_PACK_ID");
            LevelPackData data;
            if(dataCache.ContainsKey(id)){
                data = dataCache[id];
            }else{
                data = new LevelPackData();
            }
            data.Read(cur, reader);
            SetData(data);

            RetrieveItems(cur);
        }
        public void RetrieveItems(){

            using(Database.Cursor cur = Database.Database.GetCursor()){
                RetrieveItems(cur);
            }
        }
        public void RetrieveItems(Database.Cursor cur){
            data.RetrieveItems(cur);
        }

        public void CheckUnlock(){
            using(Database.Cursor cur = Database.Database.GetCursor()){
                CheckUnlock(cur);
            }
        }
        public void CheckUnlock(Database.Cursor cur){
            data.CheckUnlock(cur);
        }
        public static LevelPackData GetLevelPackData(int id){
            if(dataCache.ContainsKey(id)){
                return dataCache[id];
            }else{
                throw new System.NotImplementedException();
                //TODO
            }
        }
        public void SetParent(Transform parent, bool worldPostionStays = false){
            transform.SetParent(parent, worldPostionStays);
        }
    }
}
