using Maze.Database;
using Maze.Inputs;
using UnityEngine;
using System.Text;
using BLINDED_AM_ME;
namespace Maze.Mechanics.Obstacle.ObstacleItems {
    public class Door : ObstacleItem {
        public int keyId = 0;
        public Rigidbody door;

        public Room target;

        public int from;
        public int to;

        public Portal portal;
        //public PortalView portalView;
        public Transform playerSpawnPosition;

        public KeyCheckerState _state = KeyCheckerState.unlocked;

        public bool isKeyDoor {
            get {
                return target.isKeyRoom;
            }
        }

        public override int state {
            get {
                return (int)_state;
            }
            set {
                _state = (KeyCheckerState)value;
            }
        }

        public void Init() {
            try{
                door.useGravity = false;
                door.GetComponent<ActiveTimer>().enabled = false;
                door.GetComponent<Damager>().enabled = false;
                door.GetComponent<Portal>().enabled = false;
            }catch(System.Exception ex){

            }
            //if(portalView == null && portal != null) portalView = portal.GetComponent<PortalView>();
        }

        protected override void _LoadStatement(Statement statement) {
            Debug.Log("DOOR LOAD STATEMENT");
            dialog.SetTitle("Door");
            dialog.SetText(statement.text);
            if(target != null) target.LoadStatement(statement);
        }
        public void LoadProblem(Problem problem, bool randomCorrect) {
            Debug.Log("DOOR LOAD PROBLEM IN");
            LoadStatement(problem.GetStatements(Random.Range(0, 4) < 1 || target != null, 1)[0]);
            if(target != null) target.LoadStatement(problem.GetStatements(isKeyDoor || Random.Range(0,4) < 1, 1)[0]);
        }

        public void Open() {
            if(statement.correct){
                door.GetComponent<MovingBlock>().Move();
            }else{
                door.gameObject.layer = LayerMask.NameToLayer("PowerUp");
                door.isKinematic = false;
                door.useGravity = true;
                door.GetComponent<ActiveTimer>().enabled = true;
                door.GetComponent<Damager>().enabled = true;
                door.GetComponent<Portal>().enabled = true;
                door.AddForceAtPosition(
                    (transform.forward -transform.up).normalized * 10,
                    transform.position + transform.up * 10 - transform.forward * 4, 
                    ForceMode.Impulse);
            }
            _state = KeyCheckerState.opened;
        }

        public void SetTarget(Room room) {
            target = room;
            this.SetTarget(room.door);
            room.door.SetTarget(this);
        }

        public void SetTarget(Door door) {
            portal.target = door.playerSpawnPosition;
            //portalView.pointB = door.portalView.transform;
            //TODO
        }

        public void OnTryOpen(ButtonEventData data) {
            if (keyId == 0 || _state == KeyCheckerState.unlocked || data.actor.status.CheckStack(Control.BuffId.key, keyId) == keyId) {
                Open();
            }else{
                GUIManager.SpawnToast("Door locked.");
            }
        }
        
        public void SetPortal(int from, int to){
            this.from = from;
            this.to = to;

            Transform doorPos = LevelManager.GetPortalPosition(from);
            Room room = LevelManager.GetRoom(to);

            if(doorPos.childCount > 0) playerSpawnPosition = doorPos.GetChild(0);
            Util.ParentAndCenter(transform, doorPos);

            SetTarget(room);
            transform.localPosition += new Vector3(0, 0, 0.05f);

        }

        public override void Save(Database.Cursor cur, bool complete){
            string cmd = "INSERT OR REPLACE INTO PORTAL(LEVEL_ID, FROM_POS, TO_POS, DOOR_ANSWER_ID, DOOR_STATE, CHEST_ANSWER_ID, CHEST_STATE) VALUES({0}, {1}, {2}, {3}, {4}, {5}, {6})";
            if(!complete) cmd = cmd.Replace("STATE", "STATE_0");
            try{
                if(cur == null) throw new System.Exception("CURSOR IS NULL");
                if(statement == null) throw new System.Exception("statement IS NULL");
                if(target != null && target.chest == null) throw new System.Exception("target.chest IS NULL");
                if(target != null && target.chest.statement == null) throw new System.Exception("target.chest.statement IS NULL");
                if(LevelManager.currentLevel == null) throw new System.Exception("LevelManager.currentLevel IS NULL");
                cur.commandText = string.Format(cmd,
                LevelManager.currentLevel.id, from, to, statement.id, state, target == null ? 0 : target.chest.statement.id, target==null ? 0 : target.state);
                
                cur.ExecuteNonQuery();
            }catch(System.Exception ex){
                if(target == null) throw new System.Exception(gameObject.name + " : target IS NULL");
                throw new System.Exception(gameObject.name + " : " + ex.Message);
            }
            if(target) target.Save(cur, complete);
        }

        public override void Read(Database.Cursor cur, Database.Reader reader, int levelId, bool restoreState){
            Statement statement = Problem.GetStatement(reader.GetInt32("ANSWER_ID"));
            LoadStatement(statement);
            if(restoreState) state = reader.GetInt32("DOOR_STATE");
            else state = reader.GetInt32("DOOR_STATE_0");
            if(target != null) target.Read(cur, reader, levelId, restoreState);
        }
    }
}
