using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Maze.Level {
    public class LevelMenu : MonoBehaviour {
        public Text title;
        public Transform topRow;
        public Transform bottomRow;
        public Transform mainRow;

        public Level levelPrefab;
        public LevelPackData levelPack;
        

        public void LoadLevelPack(LevelPackData levelPack) {
            this.levelPack = levelPack;
            LoadLevelPack();
        }

        public Level CreateLevelItem(LevelData level) {

            Level t = GameObject.Instantiate<Level>(levelPrefab);

            t.SetData(level);

            return t;
        }

        public void LoadLevelPack() {
            title.text = levelPack.name;

            int c;
            GameObject go;
            Transform t;

            Util.DestroyAllChildren(topRow);
            for (int i = 0; i < levelPack.preLevels.Length; ++i) {
                CreateLevelItem(levelPack.preLevels[i]).SetParent(topRow, false);
            }

            if (mainRow.childCount > 1) {
                Util.DestroyChild(mainRow.GetChild(1));
            }

            CreateLevelItem(levelPack.treasure).SetParent(mainRow, false);

            Util.DestroyAllChildren(bottomRow);
            for (int i = levelPack.postLevels.Length - 1; i >= 0; --i) {
                CreateLevelItem(levelPack.postLevels[i]).SetParent(bottomRow, false);
            }
        }
    }
}
