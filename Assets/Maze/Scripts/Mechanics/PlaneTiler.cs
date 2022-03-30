using UnityEngine;

namespace Maze.Mechanics
{
    public class PlaneTiler : MonoBehaviour
    {
         Renderer renderer;

        public float multiplier = 0.5f;
        
        void Start(){
            renderer = GetComponent<Renderer>();
            Vector3 scale = transform.localScale;
            Vector2 scale2 =  new Vector2(scale.x, scale.z) * multiplier;
            renderer.material.SetTextureScale("_MainTex", scale2);
            renderer.material.SetTextureScale("_BumpMap", scale2);
        }
    }
}