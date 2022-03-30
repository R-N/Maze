using UnityEngine;
using Maze.Control;
namespace Maze.Mechanics
{
    public class ObjectProjector : MonoBehaviour
    {
        public Transform child;
        public float range = 15;

        bool childActive = false;


        void Start(){
            childActive = child.gameObject.activeSelf;
            prevPos = transform.position;
        }

        void SetChildActive(bool active){
            child.gameObject.SetActive(active);
            childActive = active;
        }

        Vector3 prevPos;
        public void LateUpdate(){
            Vector3 pos = transform.position;
            Vector3 deltaPos = pos-prevPos;
            RaycastHit hit;
            if(deltaPos != Vector3.zero && Physics.Raycast(transform.position, deltaPos.normalized, out hit, range, CamHolder.mask)){
                child.position = hit.point + hit.normal * 0.01f;
                child.rotation = Quaternion.LookRotation(hit.normal, transform.up);
                if(!childActive) SetChildActive(true);
            }else{
                if(childActive) SetChildActive(false);
            }
            prevPos = pos;
        }
    }
}