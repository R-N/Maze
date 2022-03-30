using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WidthMeasurer : MonoBehaviour {
    public float width = 0;
    public Vector3 hit1Point;
    public Vector3 hit1Normal;
    public Quaternion hit1NormalQ;
    public Quaternion hit1NormalQU;
    public Vector3 hit2Point;

	// Use this for initialization
	void Start () {
		
	}

    [ExecuteInEditMode]
    void Update () {
        RaycastHit hit;
        bool done = false;
        if (Physics.Raycast(new Ray(transform.position, transform.forward), out hit, float.PositiveInfinity)) {
            hit1Point = hit.point;
            hit1Normal = hit.normal;
            hit1NormalQ = Quaternion.LookRotation(hit.normal);
            hit1NormalQU = Quaternion.LookRotation(hit.normal, Vector3.up);
            RaycastHit hit2;
            if (Physics.Raycast(new Ray(hit.point, hit.normal), out hit2, float.PositiveInfinity)) {
                hit2Point = hit2.point;
                width = hit2.distance;
                Debug.DrawLine(hit.point, hit2.point, Color.green);
                done = true;
            } else {
                Debug.DrawRay(hit.point, hit.normal * 10, Color.yellow);
            }
        } else {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        }
        if (!done) {
            hit1Point = Vector3.zero;
            hit1Normal = Vector3.zero;
            hit1NormalQ = Quaternion.identity;
            hit1NormalQU = Quaternion.identity;
            hit2Point = Vector3.zero;
            width = 0;
        }
	}
}
