using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour {
    public GameObject teleportEnd;
    public Vector2 invertX;
    public Vector2 displacement;

	// Use this for initialization
	void Start () {
        if (teleportEnd == null) {
            Debug.LogWarning("Teleporter doesn't have an end attached, disabling.");
            enabled = false;
        }
	}

    internal void Teleport(GameObject character) {
        character.transform.position = teleportEnd.transform.position + new Vector3(displacement.x, displacement.y);

    }
}
