using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Maze.Mechanics.Obstacle;
namespace Maze.Mechanics.Obstacle.ObstacleItems
{
    public class DirectionSign : ObstacleItem
    {
        public Transform _target;
        public int targetId;
        public int position;
        public Transform target{
            get{
                return _target;
            }
        }
        public override int state { 
            get{
                return 0;
            }
            set{

            }
        }

        void Start(){
            if(target != null) _SetTarget(target);
        }

        public void SetTarget(Transform target, int targetId){
            
            this.targetId = targetId;
            StartCoroutine("_SetTarget", target);
        }
        private IEnumerator _SetTarget(Transform target){
            this._target = target;
            NavMeshPath path= new NavMeshPath();
            NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, path);
            
            while(path.corners.Length < 2){
                yield return new WaitForEndOfFrame();
            }
            transform.rotation = Quaternion.LookRotation(
                (path.corners[1]-transform.position).NullifyY().normalized,
                Vector3.up);
            Vector3 eul = transform.localEulerAngles;
            float yRot = eul.y;
            yRot = 90 * ((int)((yRot/90) + 0.5f));
            transform.localEulerAngles = new Vector3(eul.x, yRot, eul.z);
            yield return null;
        }

        protected override void _LoadStatement(Statement statement)
        {
            dialog.SetText(statement.text);
            if(!statement.correct){
                transform.Rotate(Vector3.up, Random.Range(0, 1) == 0 ? -90 : 90, Space.Self);
            }
        }
        public override void Save(Database.Cursor cur, bool complete){
            cur.commandText = string.Format("INSERT INTO DIRECTION VALUES({0}, {1}, {2}, {3})",
            LevelManager.currentLevel.id, position, statement.id, targetId);
            cur.ExecuteNonQuery();
        }

        public override void Read(Database.Cursor cur, Database.Reader reader, int levelId, bool restoreState){
            position = reader.GetInt32("POS");
            Statement statement = Problem.GetStatement(reader.GetInt32("ANSWER_ID"));
            LoadStatement(statement);
        }
    }
}