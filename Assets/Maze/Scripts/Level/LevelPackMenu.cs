using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Database;

namespace Maze.Level {
    public class LevelPackMenu : MonoBehaviour {

        LevelPackData[] levelPacks;
        public Transform mainRow;

        public LevelPack levelPackPrefab;

        public LevelMenu levelMenu;

        void Start(){
            Debug.Log("HELLO");
            RetrieveLevelPacks();
        }

        public void RetrieveLevelPacks(){
            using (Database.Cursor cur = Database.Database.GetCursor()){
                cur.commandText = string.Format("SELECT * FROM LEVEL_PACK");
                using (Reader reader = cur.ExecuteReader()){
                    List<LevelPackData> levelPacks= new List<LevelPackData>();
                    while(reader.Read()){
                        levelPacks.Add(new LevelPackData(cur, reader));
                    }
                    this.levelPacks = levelPacks.ToArray();
                }
            }
            /*for(int i = 0; i < levelPacks.Length; ++i){
                levelPacks[i].RetrieveItems();
            }*/
            LoadLevelPacks();
        }

        public void LoadLevelPack(LevelPackData levelPack){
            gameObject.SetActive(false);
            levelMenu.gameObject.SetActive(true);
            levelMenu.LoadLevelPack(levelPack);
        }

        public LevelPack CreateLevelPackItem(LevelPackData levelPack) {
            LevelPack t = GameObject.Instantiate<LevelPack>(levelPackPrefab);

            t.menu = this;
            t.SetData(levelPack);
            //t.RetrieveItems();
            //TODO

            return t;
        }

        public void LoadLevelPacks() {
            Util.DestroyAllChildren(mainRow);

            for (int i = 0; i < levelPacks.Length; ++i) {
                CreateLevelPackItem(levelPacks[i]).SetParent(mainRow, false);
            }
        }
    }
}