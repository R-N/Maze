using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_ParticleTrailRotation : MonoBehaviour {

	public Vector3 rotationSpeed;
	public bool local = true;
	
	// Update is called once per frame
	void Update () {
		if (local) {
			transform.Rotate (rotationSpeed * Time.deltaTime);
		} else {
			transform.Rotate (rotationSpeed * Time.deltaTime, Space.World);
		}
	}
}
	