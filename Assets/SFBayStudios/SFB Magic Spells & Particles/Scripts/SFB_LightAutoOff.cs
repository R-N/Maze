using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_LightAutoOff : MonoBehaviour {

	public float fadeLimit = 1.0f;
	public float fadePassed = 0.0f;
	public float fadeSpeed = 1.0f;

	// Update is called once per frame
	void Update () {
		fadePassed += Time.deltaTime;
		if (fadePassed >= fadeLimit) {
			FadeOut ();
		}
	}

	void FadeOut(){
		GetComponent<Light> ().intensity = GetComponent<Light> ().intensity - (Time.deltaTime * fadeSpeed);
		if (GetComponent<Light> ().intensity <= 0) {
			Destroy (this.gameObject);
		}
	}
}
