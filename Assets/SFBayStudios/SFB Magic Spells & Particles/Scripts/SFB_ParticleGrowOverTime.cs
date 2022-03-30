using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_ParticleGrowOverTime : MonoBehaviour {

	public Vector3 growth;

	// Update is called once per frame
	void Update () {
		transform.localScale += growth * Time.deltaTime;
	}
}
