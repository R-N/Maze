using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze{
public class CamHolder : MonoBehaviour {

    public new Camera camera;
    Transform camTrans;
    float maxZ = 5;
    public static LayerMask mask;
    public LayerMask _mask;
    // Use this for initialization
    void Awake () {
        camTrans = camera.transform;
        maxZ = -camTrans.localPosition.z;
        mask = _mask;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        float z;
        float ay = Mathf.Deg2Rad * camera.fieldOfView * 0.5f;
        float near = camera.nearClipPlane;
        float y = Mathf.Tan(ay) * near;
        //float ax = Mathf.Atan(y * camera.aspect);
        float x = y * camera.aspect;
        Vector3 extend = new Vector3(x, y, camera.nearClipPlane);
        RaycastHit hit;
        if (Physics.BoxCast(transform.position, extend, -transform.forward, out hit, transform.rotation, maxZ, mask)) {
            z = Mathf.Max(1f, hit.distance);// Mathf.Max(0, hit.distance - near / Vector3.Dot(hit.normal, transform.forward));
            //Debug.Log("Hit : " + hit.transform.gameObject.name);
        } else {
            z = maxZ;
        }
        camTrans.localPosition = new Vector3(0, 0, -z);
	}
}
}