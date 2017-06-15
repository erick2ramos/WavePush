using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomTriggerScript : GenericTriggers {
    public GameObject boomPrefab;
    public GameObject killerBoomPrefab;
    public float cooldown = 2f;
    private float cooldownCounter = 0f;
    public Vector3 dir;

    public override void OnTrigger()
    {
        Debug.Log("4");
		CharacterController ctrl = GetComponent<CharacterController> ();
		string mode="";			
		if(ctrl!=null){
			mode = ctrl.Item;
		}
        if(cooldownCounter <= Time.time)
        {
            cooldownCounter = Time.time + cooldown;
            if(mode != null && mode != "")
            {
                ShootWithPowerUp(mode, dir);
            } else
            {

                Debug.Log("5");
                AudioSource audio = GameObject.FindGameObjectWithTag("shootfx").GetComponent<AudioSource>();
				audio.Play();
                GameObject go = (GameObject)Instantiate(boomPrefab, this.transform.position + (dir), Quaternion.identity);
                go.GetComponentInChildren<ProjectileScript>().parent = gameObject;
                go.GetComponentInChildren<ProjectileScript>().Shoot(dir);
            }
        }
    }

    public void RefreshCooldowns()
    {
        cooldownCounter = Time.time;
    }

    private void ShootWithPowerUp(string mode, Vector3 dir)
    {
        switch (mode)
        {
            case "Multi":
                Debug.Log("10");
                Vector3[] dirs = new Vector3[] { Vector3.right, Vector3.left, Vector3.up };
                for(int i = 0; i < dirs.Length; i++)
                {
                    GameObject go = (GameObject)Instantiate(boomPrefab, this.transform.position + dirs[i], Quaternion.identity);
                    go.GetComponentInChildren<ProjectileScript>().parent = gameObject;
                    go.GetComponentInChildren<ProjectileScript>().Shoot(dirs[i]);
                }
                break;
            case "Strong":
                Debug.Log(" 11");
				AudioSource audio = GameObject.FindGameObjectWithTag("sonicfx").GetComponent<AudioSource>();
				audio.Play();
				//audio.Play(44100);
                GameObject aux = (GameObject)Instantiate(killerBoomPrefab, this.transform.position + dir, Quaternion.identity);
                aux.GetComponentInChildren<ProjectileScript>().parent = gameObject;
                aux.GetComponentInChildren<ProjectileScript>().Shoot(dir);
                break;

            default:
                Debug.Log("'"+mode+"'");
                break;
        }
    }
}
