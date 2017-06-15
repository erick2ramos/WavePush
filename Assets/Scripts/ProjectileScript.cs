using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour {
    public float velocityMagnitude;
    public GameObject boomUpgrade;
    public float lifeSpan;
    private float timedLife;
    public bool canKill;
    public GameObject parent;
    public GameObject[] hitpoints;
	// Use this for initialization
	void Awake () {
        timedLife = Time.time + lifeSpan;
	}
	
	// Update is called once per frame
	void Update () {

        if (timedLife <= Time.time)
        {
           Destroy(gameObject);
        }
	}

    public void Shoot(Vector3 dir)
    {
        GetComponent<Rigidbody2D>().velocity = dir * velocityMagnitude;
        transform.rotation = Quaternion.Euler(0, 0, -Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "Bullet") {
			Vector3 position = new Vector3 ((other.transform.position.x + transform.position.x) / 2, transform.position.y, transform.position.z);
			Vector3 dir = Vector3.zero;
			var otherDir = other.gameObject.GetComponent<Rigidbody2D> ().velocity.normalized;
			var thisDir = GetComponent<Rigidbody2D> ().velocity.normalized;
			if ((otherDir.x > 0 && thisDir.x < 0)) {
				dir = Vector3.up;
			} else if ((otherDir.y > 0 || otherDir.y < 0) && (thisDir.x > 0 || thisDir.x < 0)) {
				dir = new Vector3 (thisDir.x, thisDir.y);
			}
			if (dir.magnitude != 0) {
				GameObject go = (GameObject)Instantiate (boomUpgrade, position, Quaternion.identity);
				go.GetComponent<ProjectileScript> ().Shoot (dir);
			}

			Destroy (other.gameObject);
		} else if (other.gameObject.tag == "Player") {
			//Do nothing, it is done in player side
		}else if( other.gameObject.tag == "Item" || other.gameObject.tag=="SawBlade")
        {
            
        }
		else if(parent != other.gameObject){
			Destroy(gameObject);
		}
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(Physics2D.Linecast(hitpoints[0].transform.position, hitpoints[1].transform.position))
        {
            //Destroy(gameObject);
        }
    }
}
