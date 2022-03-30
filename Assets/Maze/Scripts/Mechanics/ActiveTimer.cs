using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTimer : MonoBehaviour {
    public float activeTime = 2;
    public bool autoActive = false;

    public bool destroy = false;
    bool active = false;
    
    float timer = 0;
	// Use this for initialization
	void OnEnable () {
        SetActive(isActiveAndEnabled);
	}
	
	// Update is called once per frame
	void Update () {
        if (timer > 0) {
            timer -= Time.deltaTime;
            if (timer <= 0) {
                timer = 0;
                SetActive(!active);
            }
        }
	}

    public virtual void OnActive() {
        gameObject.SetActive(true);
    }

    public void OnInactive() {
        gameObject.SetActive(false);
        if(destroy) Destroy(gameObject);
    }

    public void SetActive(bool active) {
        this.active = active;
        timer = activeTime;
        if (active) {
            OnActive();
        } else {
            OnInactive();
        }
    }
}
