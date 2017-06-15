using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class Button : MonoBehaviour {
    public GameObject trigger;
    public float cooldown; //in seconds
    private float time;


    private bool active;
	// Use this for initialization
	
	void OnTrigger() {
        if (time >= cooldown) {
			GenericTriggers bt = trigger.GetComponent<GenericTriggers> ();
            bt.OnTrigger();
            time = 0;
        }
    }

    void Update() {
        if (time < cooldown) {
            time += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.tag.Equals("Bullet")) {
            OnTrigger();
        }
    }
}
