using System.Collections.Generic;
using UnityEngine;
using Maze.Mechanics.Obstacle;
using Maze.Database;
using Maze.Mechanics;
using Maze.Mechanics.Obstacle.ObstacleItems;
using Maze.Control.Controllers;
using Maze.Level;
using UnityEngine.SceneManagement;

namespace Maze{
    public class LevelManager : MonoBehaviour{
        public class PortalData {
            public int from;
            public int to;
            public int answerId;
            public Door door;
            public Room room;
        }
        public enum SpawnEntryExitType{
            dontSpawn,
            spawn,
            spawnReverse,
            spawnReverseExitOnly
        }

        public enum SpawnObstacleType{
            dontSpawn,
            spawnNoRestore,
            spawnRestore
        }

        public static LevelData currentLevel = null;
        public static bool replay = false;

        public static int problemDifficulty = 0;
        public static Category[] problemCategories =null;

        public Transform[] _obstaclePositions;
        Pool<Transform> obstaclePositions;

        public Transform[] _playerRespawnPositions;
        Pool<Transform> playerRespawnPositions;

        public Transform[] _portalPositions;
        Pool<Transform> portalPositions;
        public Room[] _rooms;
        Pool<Room> rooms;

        public Room keyRoom;
        public Obstacle[] _obstaclePool;
        Pool<Obstacle> obstaclePool;

        public Transform[] _directionPositions;
        Pool<Transform> directionPositions;
        
        public PlayerController playerPrefab;
        public Transform entryDoorPrefab;
        public Door exitDoorPrefab;

        public Spray sprayPrefab;

        public Door doorPrefab;

        public DirectionSign directionSignPrefab;

        public Transform fallingBlockSpawnerPrefab;

        Door exitDoor;
        Transform entryDoor;

        PlayerController player;


        public Dictionary<string, Obstacle> obstacleByName = new Dictionary<string, Obstacle>();

        int entryPos;
        int exitPos;

        List<PortalData> doors = new List<PortalData>();



        Dictionary<int, Obstacle> obstacles = new Dictionary<int, Obstacle>();


        static LevelManager instance;

        public List<Spray> sprays = new List<Spray>();

        List<DirectionSign> directionSigns = new List<DirectionSign>();

        

        public static bool randomInited = false;
        public static void InitRandom(){
            if(!randomInited){
                Random.InitState((int)(((long)(Time.realtimeSinceStartup * 1000))%int.MaxValue));
            }
        }

        private void Start() {
            instance = this;

            obstaclePositions = new Pool<Transform>(_obstaclePositions);
            playerRespawnPositions = new Pool<Transform>(_playerRespawnPositions);
            portalPositions = new Pool<Transform>(_portalPositions);
            rooms = new Pool<Room>(_rooms);
            obstaclePool = new Pool<Obstacle>(_obstaclePool);
            directionPositions = new Pool<Transform>(_directionPositions);

            for (int i = 0; i < _obstaclePool.Length; ++i) {
                Obstacle obs = _obstaclePool[i];
                obstacleByName[obs.name] = obs;
            }
            for(int i = 0; i < _rooms.Length; ++i){
                _rooms[i].Init();
            }
            InitRandom();

            InitLevel();
        }

        public void InitLevel(){
            if(replay){
                if(currentLevel.type == LevelType.postLevel){
                    Load(currentLevel.preLevel.id, SpawnEntryExitType.dontSpawn, false, true, SpawnObstacleType.spawnRestore);
                    Load(currentLevel.id, SpawnEntryExitType.spawn, true, false, SpawnObstacleType.dontSpawn);
                    exitDoor.Open();
                }else{
                    Load(currentLevel.id, SpawnEntryExitType.spawn, true, false, SpawnObstacleType.spawnNoRestore);
                }
            }else{
                if(currentLevel.type == LevelType.postLevel){
                    Load(currentLevel.preLevel.id, SpawnEntryExitType.spawnReverse, true, true, SpawnObstacleType.spawnRestore);
                    exitDoor.Open();
                }else{
                    Generate();
                }
            }
            if(currentLevel.type == LevelType.postLevel){
                OnTreasureGet();
            }
        }
        
        public static void LoadLevel(LevelData data, bool replay=false){
            currentLevel = data;
            LevelManager.replay = replay;
            SceneManager.LoadScene("Maze/Scenes/" + data.scene);
        }

        public static Transform GetRandomPlayerRespawn() {
            return instance.playerRespawnPositions.Get();
        }

        public static Transform GetObstaclePosition(int id) {
            if(id < 100) return instance.obstaclePositions[id];
            else return instance.rooms[id-100].obstaclePosition;
        }

        public void Generate() {


            entryPos = portalPositions.GetIndex();

            if(currentLevel.type == LevelType.bossLevel){
                SpawnExit(entryPos);
            }else{
                exitPos = portalPositions.GetIndex();
                SpawnEntryExit(entryPos, exitPos);
            }
            SpawnPlayer();


            int obsCount = Mathf.Min(obstaclePool.Length, obstaclePositions.Length);
            Debug.Log("OBSTACLE POSITION LENGTH: " + obstaclePositions.Length);
            int problemCount = 0;
            for (int i = 0; i < obsCount; ++i) {

                Obstacle obs = GameObject.Instantiate<Obstacle>(obstaclePool.Get());

                int pos = obstaclePositions.GetIndex();
                UnityEngine.Debug.Log("SET OBSTACLE POSITION TO " + pos);
                obs.position = pos;
                UnityEngine.Debug.Log("SET OBSTACLE POSITION RESULT: " + obs.position);

                //Util.ParentAndCenter(obs.transform, obstaclePositions[obs.position]);

                obs.obstacleId = i;
                instance.obstacles[obs.obstacleId] = obs;
                obs.Init();

                problemCount += obs.preferredProblemCount;
            }



            int ppc = portalPositions.Length - 2; //-2 for entry and exit

            int portalCount = rooms.Length;
            problemCount += ppc;
            
            for (int i = 0; i < ppc; ++i) {
                int doorPosIndex = portalPositions.GetIndex();
                if(rooms.recycleCount >= 2){
                    MakeFakePortal(doorPosIndex);
                    continue;
                }

                int roomIndex = rooms.GetIndex();


                PortalData data = MakePortal( doorPosIndex, roomIndex);
                
                //obstacle in room
                Obstacle obs = GameObject.Instantiate<Obstacle>(obstaclePool.Get());

                obs.position = roomIndex + 100;

                Util.ParentAndCenter(obs.transform, data.room.obstaclePosition);

                obs.obstacleId = obsCount++;
                instance.obstacles[obs.obstacleId] = obs;
                obs.Init();
            }
            
            int dirC = (int)(0.8f * directionPositions.Length);
            problemCount += dirC;
            for(int i = 0; i < dirC; ++i){
                int pos = directionPositions.GetIndex();
                DirectionSign dir = Instantiate<DirectionSign>(directionSignPrefab);
                dir.SetTarget(exitDoor.transform, 0);
                dir.position = pos;
                Util.ParentAndCenter(dir.transform, directionPositions[pos]);
            }

            Problem[] problems = Problem.GetRandom(problemCount, problemDifficulty, problemCategories);

            Debug.Log("Problem count: " + problems.Length);

            Pool<Problem> problemPool = new Pool<Problem>(problems);

            foreach (Obstacle obs in instance.obstacles.Values) {
                Problem[] probs = problemPool.Get(obs.preferredProblemCount);
                obs.LoadProblems(probs);
            }

            Pool<PortalData> portalPool = new Pool<PortalData>(doors);
            int c = portalPool.Length;
            for(int i = 0; i < c; ++i){
                PortalData portal = portalPool.Get();
                if(keyRoom == null && portal.room != null && portal.room.isKeyRoom) keyRoom = portal.room;
                //Statement stat = problemPool.Get().GetStatements(portal.room.isKeyRoom || i% 2 == 0, 1)[0];
                //portal.answerId = stat.id;
                //portal.door.LoadStatement(stat);
                portal.door.LoadProblem(problemPool.Get(), i% 2 == 0);
                Debug.Log("DOOR LOAD PROBLEM");
            }
            
            Pool<DirectionSign> directionPool = new Pool<DirectionSign>(directionSigns);
            for(int i = 0; i < directionPool.Length; ++i){
                directionPool.Get().LoadStatement(problemPool.Get().GetStatements(i% 2 == 0, 1)[0]);
            }



            Save(false);
        }
        
        public void SpawnPlayer(){
            SpawnPlayer(entryPos);
        }
        public void SpawnPlayer(Database.Cursor cur){
            SpawnPlayer(cur, entryPos);
        }
        public void SpawnPlayer(int entryPos){
            using (Database.Cursor cur = Database.Database.GetCursor()){
                SpawnPlayer(cur, entryPos);
            }
        }

        public void SpawnPlayer(Database.Cursor cur, int entryPos){
            player = GameObject.Instantiate<PlayerController>(this.playerPrefab);
            Util.CopyTransform(player.transform, portalPositions[entryPos].GetChild(0));

            player.transform.position += player.transform.forward * 7.5f + Vector3.up * 5;
            //Get previous level HP
            if(currentLevel.number == 1){
                player.status.hpCur = player.status.hpMax;
            }else{
                cur.commandText =string.Format("SELECT PLAYER_HP FROM LEVEL_COMPLETION WHERE LEVEL_ID={0}", currentLevel.id - 1);
                using (Reader reader = cur.ExecuteReader()){
                    reader.Read();
                    player.status.hpCur = reader.GetFloat("PLAYER_HP");
                }
            }
        }

        public void SpawnEntryExit(int entryPos, int exitPos){
            this.entryPos = entryPos;

            Transform entry = portalPositions[entryPos];
            entryDoor = GameObject.Instantiate<Transform>(this.entryDoorPrefab);
            Util.ParentAndCenter(entryDoor, entry);
            entryDoor.transform.localPosition += new Vector3(0, 0, 0.05f);
            
            SpawnExit(exitPos);
        }

        public void SpawnExit(int exitPos){
            this.exitPos = exitPos;
            Transform exit = portalPositions[exitPos];
            exitDoor = GameObject.Instantiate<Door>(this.exitDoorPrefab);
            Util.ParentAndCenter(exitDoor.transform, exit);
            exitDoor.transform.localPosition += new Vector3(0, 0, 0.05f);
        }

        public PortalData MakePortal(int doorPosId, int roomId) {

            if(roomId < 0){
                return MakeFakePortal(doorPosId);
            }

            Door door = GameObject.Instantiate<Door>(this.doorPrefab);
            door.Init();
            door.SetPortal(doorPosId, roomId);

            PortalData data = new PortalData();
            data.from = doorPosId;
            data.to = roomId;
            data.door = door;
            data.room = door.target;

            doors.Add(data);

            return data;
        }
        public PortalData MakeFakePortal(int doorPosId) {

            Door door = GameObject.Instantiate<Door>(this.doorPrefab);
            door.Init();
            Util.ParentAndCenter(door.transform, portalPositions[doorPosId]);

            PortalData data = new PortalData();
            data.from = doorPosId;
            data.to = -1;
            data.door = door;
            data.room = null;

            doors.Add(data);

            return data;
        }

        public static Transform GetPortalPosition(int index){
            return instance.portalPositions[index];
        }
        public static Room GetRoom(int index){
            return instance.rooms[index];
        }

        public void OnTreasureGet(){
            Transform spawner = Instantiate<Transform>(fallingBlockSpawnerPrefab);
            spawner.parent = player.transform;
            spawner.localPosition = new Vector3(0, 30, 0);
        }

        public static void OnLevelClear() {
            Debug.Log("LEVEL CLEAR");
            instance.Save(true);
            currentLevel.completed = true;
        }

        public static void RegisterSpray(Spray spray){
            Debug.Log("REGISTER SPRAY");
            instance.sprays.Add(spray);
        }

        public void Save(bool complete) {
            using (Database.Cursor cur = Database.Database.GetCursor()){

                if(complete){
                    cur.commandText = string.Format("INSERT OR REPLACE INTO LEVEL_COMPLETION VALUES({0}, {1}, {2}, {3})",
                    currentLevel.id, PlayerController.instance.status.hpCur, entryPos, exitPos);
                    cur.ExecuteNonQuery();
                }

                //if(type == LevelType.preLevel){
                    //overwrite

                //cur.commandText = string.Format("DELETE FROM OBSTACLE_ITEM WHERE OBSTACLE_ITEM.OBSTACLE_ID_LEVEL IN (SELECT OBSTACLE.OBSTACLE_ID_LEVEL FROM OBSTACLE WHERE OBSTACLE.LEVEL_ID={0})", levelId);
                cur.commandText = string.Format("DELETE FROM OBSTACLE_ITEM WHERE LEVEL_ID={0}", currentLevel.id);
                cur.ExecuteNonQuery();
                cur.commandText = string.Format("DELETE FROM OBSTACLE WHERE LEVEL_ID={0}", currentLevel.id);
                cur.ExecuteNonQuery();
                
                cur.commandText = string.Format("DELETE FROM PORTAL WHERE LEVEL_ID={0}", currentLevel.id);
                cur.ExecuteNonQuery();

                cur.commandText = string.Format("DELETE FROM TRANSFORM WHERE TRANSFORM.TRANSFORM_ID IN (SELECT SPRAY.TRANSFORM_ID FROM SPRAY WHERE SPRAY.LEVEL_ID={0})", currentLevel.id);
                cur.ExecuteNonQuery();


                cur.commandText = string.Format("DELETE FROM SPRAY WHERE LEVEL_ID={0}", currentLevel.id);
                cur.ExecuteNonQuery();
                cur.commandText = string.Format("DELETE FROM DIRECTION WHERE LEVEL_ID={0}", currentLevel.id);
                cur.ExecuteNonQuery();


                //save
                foreach(Obstacle o in obstacles.Values){
                    o.Save(cur, complete);
                }
                foreach(PortalData portal in doors){
                    portal.door.Save(cur, complete);
                }
                foreach(DirectionSign dir in directionSigns){
                    dir.Save(cur, complete);
                }
                if(complete){
                    foreach(Spray s in sprays){
                        s.Save(cur);
                    }
                }
                //}
            }
        }

        public void Load(int levelId, SpawnEntryExitType spawnEntryExit = SpawnEntryExitType.spawnReverse, bool spawnPlayer=true, bool spawnSprays=true, SpawnObstacleType obstacleState=SpawnObstacleType.spawnRestore) {
            using (Database.Cursor cur = Database.Database.GetCursor()){
                cur.commandText = string.Format("SELECT * FROM LEVEL_GENERATION WHERE LEVEL_ID={0}", levelId);
                
                using(Reader reader = cur.ExecuteReader()){
                    reader.Read();
                    entryPos = reader.GetInt32("ENTRY_POS");
                    exitPos = reader.GetInt32("EXIT_POS");
                }
                
                if(spawnEntryExit == SpawnEntryExitType.spawnReverse){
                    SpawnEntryExit(exitPos, entryPos);
                }else if(spawnEntryExit == SpawnEntryExitType.spawn){
                    SpawnEntryExit(entryPos, exitPos);
                }else if (spawnEntryExit == SpawnEntryExitType.spawnReverseExitOnly){
                    SpawnExit(entryPos);
                }
                if(spawnPlayer){
                    SpawnPlayer(cur);
                }

                if(obstacleState != SpawnObstacleType.dontSpawn){
                    bool restoreState = obstacleState == SpawnObstacleType.spawnRestore;

                    cur.commandText = string.Format("SELECT * FROM PORTAL WHERE LEVEL_ID={0}", levelId);
                    using(Reader reader = cur.ExecuteReader()){
                        while(reader.Read()){
                            int from = reader.GetInt32("FROM_POS");
                            int to = reader.GetInt32("TO_POS");
                            PortalData data = MakePortal(from, to);
                            data.door.Read(cur, reader, levelId, restoreState);
                            /* 
                            data.door.LoadStatement(Problem.GetStatement(reader.GetInt32("ANSWER_ID")));
                            data.door.state = reader.GetInt32("STATE");
                            data.room.state = reader.GetInt32("CHEST_STATE");
                            */
                        }
                    }

                    cur.commandText = string.Format("SELECT * FROM OBSTACLE WHERE LEVEL_ID={0} ORDER BY OBSTACLE_ID_LEVEL", levelId);
                    using (Reader reader = cur.ExecuteReader()){
                        while(reader.Read()){
                            string name = reader.GetString("NAME");
                            int pos = reader.GetInt32("POS");

                            Obstacle obs = GameObject.Instantiate<Obstacle>(obstacleByName[name]);

                            //Util.ParentAndCenter(obs.transform, GetObstaclePosition(pos));

                            obs.obstacleId = reader.GetInt32("OBSTACLE_ID_LEVEL");
                            instance.obstacles[obs.obstacleId] = obs;

                            obs.Init();

                            obs.Read(cur, reader, levelId, restoreState);
                        }
                    }

                    
                    cur.commandText = string.Format("SELECT * FROM DIRECTION WHERE LEVEL_ID={0}", levelId);
                    using(Reader reader = cur.ExecuteReader()){
                        while(reader.Read()){
                            DirectionSign dir = Instantiate<DirectionSign>(directionSignPrefab);
                            int pos = reader.GetInt32("POS");
                            Util.ParentAndCenter(dir.transform, directionPositions[pos]);
                            int target = reader.GetInt32("TARGET");
                            if(target == 0) dir.SetTarget(exitDoor.transform, target);
                            dir.Read(cur, reader, levelId, restoreState);
                        }
                    }
                }

                if(spawnSprays){
                    cur.commandText = string.Format("SELECT * FROM SPRAY, TRANSFORM WHERE SPRAY.LEVEL_ID={0} AND SPRAY.TRANSFORM_ID=TRANSFORM.TRANSFORM_ID", levelId);
                    using (Reader reader = cur.ExecuteReader()){
                        while(reader.Read()){
                            Spray spray =GameObject.Instantiate<Spray>(this.sprayPrefab);

                            spray.Read(cur, reader);

                            //spray.transform.position = new Vector3(reader.GetFloat("POS_X"), reader.GetFloat("POS_Y"), reader.GetFloat("POS_Z"));
                            //spray.transform.rotation = new Quaternion(reader.GetFloat("ROT_X"), reader.GetFloat("ROT_Y"), reader.GetFloat("ROT_Z"), reader.GetFloat("ROT_W"));
                        }
                    }
                }
            }
        }
    }
}
