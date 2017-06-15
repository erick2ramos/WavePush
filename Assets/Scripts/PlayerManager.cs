using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {
    public bool playerActive;
    public KeyCode left, right, jump, shoot;
    private bool change;
    private int changeVar;
    private UnityEngine.UI.Button textChange;

    void Start() {
        PlayerActive = playerActive;
    }

    void Update() {
        if (change) {
            DetectInput();
        }
    }

    public void SetGameObject(GameObject source) {
        textChange = source.GetComponent<UnityEngine.UI.Button>();
        textChange.interactable = false;
    }


    public bool PlayerActive {
        get {
            return playerActive;
        }

        set {
            playerActive = value;
            foreach (UnityEngine.UI.Button button in GetComponentsInChildren<UnityEngine.UI.Button>()) {
                if (!button.gameObject.tag.Equals("Player")) {
                    button.interactable = value;
                }
            }
        }
    }

    public KeyCode Left {
        get {
            return left;
        }

        set {
            left = value;
        }
    }

    public KeyCode Right {
        get {
            return right;
        }

        set {
            right = value;
        }
    }

    public KeyCode Jump {
        get {
            return jump;
        }

        set {
            jump = value;
        }
    }

    public KeyCode Shoot {
        get {
            return shoot;
        }

        set {
            shoot = value;
        }
    }

    public void ChangeActive() {
        PlayerActive = !PlayerActive;
    }


    public void ChangeKeycode(int variable) {
        change = true;
        changeVar = variable;
        Cursor.lockState = CursorLockMode.Locked;
    }


    private void DetectInput() {
        foreach (KeyCode vkey in System.Enum.GetValues(typeof(KeyCode))) {
            if (Input.GetKey(vkey)) {
                if (vkey != KeyCode.Return) {
                    switch (changeVar) {
                        case 0:
                            left = vkey;
                            break;
                        case 1:
                            right = vkey;
                            break;
                        case 2:
                            jump = vkey;
                            break;
                        case 3:
                            shoot = vkey;
                            break;
                    }
                    textChange.interactable = true;
                    Text txt = textChange.GetComponentInChildren<Text>();
                    txt.text = vkey.ToString();
                    change = false;
                    Cursor.lockState = CursorLockMode.None;
                }
            }
        }
    }


}
