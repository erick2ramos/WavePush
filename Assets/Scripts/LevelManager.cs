using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {
    public bool changes;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Quit() {
        Application.Quit();
    }

    public void ChangeScene(string scene) {
        if (changes) {
            if (scene.Equals("Smash")) {
                GameManager gm = FindObjectOfType<GameManager>();
                PlayerManager[] players = FindObjectsOfType<PlayerManager>();
                gm.clearPregame();
                foreach (PlayerManager player in players) {
                    if (player.playerActive) {
                        gm.addPreGamePlayer(player);
                    }
                }
                MusicManager mm = FindObjectOfType<MusicManager>();
                if (mm != null) {
                    Destroy(mm);
                }
            }
        }
        SceneManager.LoadScene(scene);
    }
}