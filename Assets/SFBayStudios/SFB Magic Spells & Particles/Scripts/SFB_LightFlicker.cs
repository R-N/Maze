using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This will flicker a light, changing it's intensity randomly on a randomly generated time interval.
/// </summary>
public class SFB_LightFlicker : MonoBehaviour {

	public float flickerMin = 0.1f;
	public float flickerMax = 0.3f;
	public float flickerCount = 0.3f;
	public float intensityMin = 0.9f;
	public float intensityMax = 1.1f;

	// Update is called once per frame
	void Update () {
		flickerCount = Mathf.Clamp (flickerCount - Time.deltaTime, 0, flickerMax);
		if (flickerCount == 0) {
			GetComponent<Light> ().intensity = Random.Range (intensityMin, intensityMax);
			flickerCount = Random.Range (flickerMin, flickerMax);
		}
	}
}
