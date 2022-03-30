using UnityEngine;

namespace Maze.Mechanics
{
    public class MaterialTiler : MonoBehaviour
    {
         Renderer renderer;

        public float multiplier = 0.5f;
        
        void Start(){
            renderer = GetComponent<Renderer>();
            Vector3 scale = transform.localScale;
            float scale1 = Mathf.Max(scale.x, scale.z);
            Vector2 scale2 =  new Vector2(scale1, scale.y) * multiplier;
            renderer.material.SetTextureScale("_MainTex", scale2);
            renderer.material.SetTextureScale("_BumpMap", scale2);
        }
    }
}