using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingGraphic : MonoBehaviour {
	public float rotDelta = 30;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void LateUpdate () {
		transform.Rotate(Vector3.forward, rotDelta*Time.deltaTime, Space.Self);
	}
}
