using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFB_LightColorChange : MonoBehaviour {

	public Color[] colors;
	public float minTime = 0.1f;
	public float maxTime = 0.25f;
	float curTime = 0.0f;
	Color curColor;
	public bool everyFrame = false;
	
	// Update is called once per frame
	void Update () {
		if (colors.Length != 0) {
			curTime = Mathf.Clamp (curTime - Time.deltaTime, 0, maxTime);
			if (curTime == 0 || everyFrame) {
				curTime = Random.Range (minTime, maxTime);							// Reset curTime
				GetComponent<Light> ().color = colors [Random.Range (0, colors.Length)];
			}
		}
	}
}
