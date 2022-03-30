using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will move the object over time
/// </summary>
public class SFB_MoveOverTime : MonoBehaviour {

	public Vector3 travelDirection;		// Direction of travel
	public bool local = false;			// True if yo uwant to move on local axis
	public float speed = 1.0f;			// Speed of movement
	public float delay = 0.0f;			// Initial delay before movement starts
	public bool isMoving = false;		// True if moving

	// Use this for initialization
	void Start () {
		Invoke("StartMoving", delay);
	}

	public void StartMoving(){
		isMoving = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMoving) {
			if (local) {
				transform.localPosition += travelDirection * Time.deltaTime * speed;
			} else {
				transform.position += travelDirection * Time.deltaTime * speed;
			}
		}
	}
}
