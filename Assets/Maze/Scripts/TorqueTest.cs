using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorqueTest : MonoBehaviour {
	public bool apply = false;
	public Vector3 force;
	public Vector3 torque;

	public new Rigidbody rigidbody;

	// Use this for initialization
	void Start () {
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		if(apply){
			rigidbody.AddForceAtPosition(force, transform.position + transform.up * 10 - transform.forward * 4, ForceMode.Impulse);
			rigidbody.AddRelativeTorque(torque, ForceMode.Impulse);
			apply = false;
		}
	}
}
