using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour {
    public List<CharacterController> characters;
    private List<PlayerManager> preGamePlayer;
    public static GameManager instance;
    public GameObject playerPrefab;



    // Use this for initialization
    void Awake() {
        if(instance == null)
        {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        characters = new List<CharacterController>();
        preGamePlayer = new List<PlayerManager>();
        Time.timeScale = 1;
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddCharacter(CharacterController character) {
        characters.Add(character);
    }

    internal void clearPregame() {
        preGamePlayer.Clear();
    }

    public void CheckWin() {
        int alive = 0;
        GameObject winner = null;
        Debug.Log("Vivos " + characters.Count.ToString());
        foreach (CharacterController character in characters) {
            if (character.Alive) {
                alive += 1;
                winner = character.gameObject;
            }
        }
        Debug.Log( "Vivos "  + alive.ToString());
        if (alive == 1) {
            //TODO WIN ROUND, RESET?
            Debug.Log("WIN");
            GameObject go = GameObject.Find("Canvas/VictoryScreen");
            foreach(Transform child in go.transform)
            {
                child.gameObject.SetActive(true);
            }
            go.GetComponentInChildren<Text>(true).text = winner.name + " you are victorious!";

        }
    }

    public void StartGame() {
        Debug.Log("Starting game");
        characters.Clear();
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Respawn");
        int x = 0;
        foreach (PlayerManager pm in preGamePlayer) {
            GameObject go = (GameObject)Instantiate(playerPrefab, null);
            go.name = "Player " + (x + 1).ToString();
            SpriteRenderer[] srs = go.GetComponentsInChildren<SpriteRenderer>();
            int y = 0;
            foreach (SpriteRenderer sr in srs) {
                sr.sprite = go.GetComponent<CharacterController>().sprites[(x * 4) + y++];
            }
            go.transform.position = gos[x++].transform.position;
            CharacterController cc = go.GetComponent<CharacterController>();
            cc.setKeys(pm);
            AddCharacter(cc);
            Debug.Log("Added character");
        }
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode) {
        if (scene.name.Equals("Smash")) {
            StartGame();
            Debug.Log("Entre una sola vez");
        }
    }

    internal void addPreGamePlayer(PlayerManager pm) {
        preGamePlayer.Add(pm);
    }

    public void ResetGame()
    {
        GameObject go = GameObject.Find("Canvas/VictoryScreen");
        foreach (Transform child in go.transform)
        {
            child.gameObject.SetActive(false);
        }
        foreach (CharacterController cc in characters)
        {
            Destroy(cc.gameObject);
        }
        StartGame();
    }
}