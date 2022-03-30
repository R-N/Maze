using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Inputs;
using Maze.Mechanics.Obstacle;
using Maze.Database;
using Maze.Control;
using UnityEngine.Events;

namespace Maze.Mechanics.Obstacle.ObstacleItems {
    public class Chest : ObstacleItem {
        public PowerUp[] poolCorrect;
        public PowerUp[] poolWrong;

        public UnityEvent onOpenCorrect;
        public UnityEvent onOpenWrong;



        public int spawnCount = 0;

        public KeyCheckerState _state = KeyCheckerState.unlocked;

        public int keyId = 0;

        public Animator animator;

        public GameObject trap = null;

        public override int state {
            get {
                return (int)_state;
            }

            set {
                _state = (KeyCheckerState)value;
            }
        }

        protected override void _LoadStatement(Statement statement) {
            this.dialog.SetText(statement.text);
            this.dialog.SetTitle("Chest");
        }
         void Start(){
        }
        public void Open(ButtonEventData data) {
            if(_state == KeyCheckerState.opened){
                return;
            }
            if(keyId != 0 && _state == KeyCheckerState.locked){
                if(data.actor.status.CheckStack(BuffId.key, keyId) != keyId){
                    return;
                }
            }
            Pool<PowerUp> pool = new Pool<PowerUp>(statement.correct ? poolCorrect : poolWrong);
            if (spawnCount == 0) spawnCount = pool.Length;
            for (int i = 0; i < spawnCount; ++i) {
                PowerUp p0 = pool.Get();
                Vector3 dir = new Vector3(Random.Range(-1, 1), 2, Random.Range(0.25f, 1)).normalized;
                dir = transform.TransformDirection(dir);
                //Debug.Log("dir " + dir);
                PowerUp p = GameObject.Instantiate<PowerUp>(
                    p0,
                    transform.position + dir * 0.25f,
                    transform.rotation
                );
                p.GetRigidbody().AddForce(dir * 5f, ForceMode.Impulse);
            }
            if(statement.correct){
                onOpenCorrect.Invoke();
            }else{
                onOpenWrong.Invoke();
                if(trap != null) trap.SetActive(true);
            }
            animator.SetBool("opened", true);
            _state = KeyCheckerState.opened;
        }
        
        
        public override void Save(Database.Cursor cur, bool complete){
            
        }

        public override void Read(Database.Cursor cur, Database.Reader reader, int levelId, bool restoreState){
            
            if(restoreState) this.state = reader.GetInt32("CHEST_STATE");
            else this.state = reader.GetInt32("CHEST_STATE_0");
            if(_state == KeyCheckerState.opened){
                if(statement.correct){
                    onOpenCorrect.Invoke();
                }else{
                    onOpenWrong.Invoke();
                    if(trap != null) trap.SetActive(true);
                }
            }
            
        }
    }
}