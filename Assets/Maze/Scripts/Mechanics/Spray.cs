using Maze.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maze.Mechanics {
    public class Spray : MonoBehaviour, IGameObject {
        public Decal decal;

        void Start() {
            if (decal == null) {
                decal = GetComponent<Decal>();
            }
            Debug.Log("Register spray from spray");
            LevelManager.RegisterSpray(this);
        }

        public void Read(Database.Cursor cur, Reader reader) {
            transform.position = new Vector3(
                reader.GetFloat("POS_X"),
                reader.GetFloat("POS_Y"),
                reader.GetFloat("POS_Z")
                );
            transform.rotation = new Quaternion(
                reader.GetFloat("ROT_X"),
                reader.GetFloat("ROT_Y"),
                reader.GetFloat("ROT_Z"),
                reader.GetFloat("ROT_W")
                );
        }

        public void Save() {
            using (Database.Cursor cur = Database.Database.GetCursor()){
                Save(cur);
            }
        }
        public void Save(Database.Cursor cur) {
            Vector3 pos = transform.position;
            Quaternion rot = transform.rotation;
            cur.commandText = string.Format("INSERT INTO TRANSFORM(POS_X, POS_Y, POS_Z, ROT_X, ROT_Y, ROT_Z, ROT_W) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})",
                pos.x, pos.y, pos.z, rot.x, rot.y, rot.z, rot.w
            );
            cur.ExecuteNonQuery();
            int transformId = (int)cur.lastInsertId;
            cur.commandText = string.Format("INSERT INTO SPRAY(LEVEL_ID, TRANSFORM_ID) VALUES({0}, {1})", LevelManager.currentLevel.id, transformId);
            cur.ExecuteNonQuery();
        }

        public GameObject GetGameObject() {
            return gameObject;
        }
    }
}
