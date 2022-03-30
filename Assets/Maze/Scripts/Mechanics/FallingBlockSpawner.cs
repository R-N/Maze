using UnityEngine;
using Maze;
using Maze.Control.Controllers;
namespace Maze.Mechanics
{
    public class FallingBlockSpawner : MonoBehaviour
    {
        public float emissionRate = 20;
        public float radius = 35;

        float timer = 0;

        public Transform prefab;
        void Start(){
            /* 
            transform.parent = PlayerController.instance.transform;
            transform.localPosition = new Vector3(0, 30, 0);
            */
        }

        void FixedUpdate(){
            float delay = 1/emissionRate;
            timer += Time.fixedDeltaTime;
            while(timer > delay){
                timer -= delay;
                Vector2 localPos = Random.insideUnitCircle * radius;
                Transform obj = Instantiate<Transform>(prefab, transform.position + localPos.ToVector3XZ(), Random.rotation);
            }
        }
    }
}