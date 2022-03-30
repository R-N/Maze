using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlphaGraphic : MonoBehaviour {

	public bool fadeIn = true;
	public Graphic graphic;
	public float speed = 0.5f;
	public float minAlpha = 0.5f;
	public float maxAlpha = 1;
	float alpha = 1;
	void Start () {
		alpha = graphic.color.a;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		float dt = Time.deltaTime;
		if(fadeIn){
			alpha += speed * dt;
			if(alpha >= maxAlpha){
				alpha = maxAlpha - (maxAlpha-alpha);
				fadeIn = false;
			}
		}else{
			alpha -= speed*dt;
			if(alpha <= minAlpha){
				alpha = minAlpha + (minAlpha - alpha);
				fadeIn=true;
			}
		}
		Color col = graphic.color;
		col.a = alpha;
		graphic.color = col;
	}
}
