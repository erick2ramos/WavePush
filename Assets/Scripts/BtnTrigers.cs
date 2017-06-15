using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnTrigers : MonoBehaviour {

    public void Restart()
    {
        GameObject.Find("Game Manager").GetComponent<GameManager>().ResetGame();
    }
}
