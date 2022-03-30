using Maze.Inputs.GUI;
using Maze.Inputs;
using Maze.Mechanics.Obstacle;
using System.Collections;
using System.Collections.Generic;
using Maze.Database;
using UnityEngine;
using System.Data;
using System.IO;
using Mono.Data.SqliteClient;

namespace Maze {
    public class Debugger : MonoBehaviour {
        public float horizontalAxis = 0;
        public float verticalAxis = 0;
        public float lookX = 0;
        public float lookY = 0;


        private string connection;
        public IDbConnection dbcon;
        public IDbCommand dbcmd;
        public IDataReader reader;

        public string packageName = "com.wp.maze";
        string dir = null;
        string dbName = null;

        string filepath = null;

        public bool execute = false;
        

        // Use this for initialization
        void Start() {

        }

        void Update(){
            if(execute){
                GUIManager.SpawnToast("TEXT", "TITLE");

                execute=false;
            }
        }

        // Update is called once per frame
        /*void Update() {
            horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");
            verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");
            lookX = CrossPlatformInputManager.GetAxis("Look X");
            lookY = CrossPlatformInputManager.GetAxis("Look Y");
        }*/

        public void TestEvent(ButtonEventData data) {
            Debug.Log("Receive button event\nFrom:" + data.source.name + "\nId:" + data.sourceId);
        }
        public void TestEvent(ToggleEventData data) {
            Debug.Log("Receive toggle event\nFrom:" + data.source.name + "\nId:" + data.sourceId + "\nEnabled:" + data.enabled);
        }

    }
}