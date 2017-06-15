using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashManager : MonoBehaviour {
    public float splashTime;
    private static int mode = -1; //0 = change to menu

    private float timeEllapsed;
    private LevelManager lm;

	// Use this for initialization
	void Awake () {
        mode++;
        Debug.Log(mode);
        lm = GetComponent<LevelManager>();
        if (mode >= 2) {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
        timeEllapsed += Time.deltaTime;
        if (splashTime < timeEllapsed) {
            if(mode == 0) {
                lm.ChangeScene("Menu");
            } else {
                gameObject.SetActive(false);
            }
        } else if(mode == 1){
                float inverseAlpha = (timeEllapsed / splashTime);
                Image sr = GetComponent<Image>();
                Color color = sr.color;
                color.a = 1 - inverseAlpha;
                sr.color = color;
        }
	}
}
