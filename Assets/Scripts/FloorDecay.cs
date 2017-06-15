using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDecay : MonoBehaviour {

    public float maxHealth;
    public float regenCooldown;

    private float health;
    private float cooldown;

    public float rate;

    private BoxCollider2D coll;
    private SpriteRenderer sprite;

    void Start() {
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

	// Use this for initialization
	void Update () {
        Transform t = gameObject.transform;
        Vector2 position = gameObject.transform.position + new Vector3((coll.size.x/2)-0.1f, (coll.size.y/2)+0.05f);
        Vector2 endPosition = gameObject.transform.position + new Vector3((-coll.size.x / 2)+0.1f, (coll.size.y / 2) + 0.05f);
        RaycastHit2D rh2d = Physics2D.Linecast(position, endPosition);
        if (rh2d.transform!=null && rh2d.transform.tag == "Player") {
            Debug.Log("Touching2");
            cooldown = regenCooldown;
            Decay();
            SetAlpha();
        } else {
            cooldown -= Time.deltaTime;
            if (cooldown <= 0) { 
                cooldown = 0;
                coll.enabled = true;
                if (health < maxHealth) {
                    health += rate;
                } else {
                    health = maxHealth;
                }
                SetAlpha();
            }
        }
	}
	
	// Update is called once per frame
	public void Decay() {
        health -= rate;
        if (health <= 0) {
            health = 0;
            FloorDestroy();
        }

    }
    private void SetAlpha() {

        Color color = sprite.color;
        color.a = health / maxHealth;
        sprite.color = color;
    }


    private void FloorDestroy() {
        coll.enabled = false;
    }
}