using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelScript : MonoBehaviour {
    public Transform[] itemsPositions;
    public GameObject itemPrefab;
    private Transform itemParent;

    public float itemCooldown = 4f;
    public float longCooldown = 10f;
    private float itemTimer = 0f;
    private int lastIndex;

    public float itemProb = 5f;
    private float itemProbAcum = 0f;

	// Use this for initialization
	void Start () {
        itemTimer = Time.time + itemCooldown;
        itemParent = new GameObject().transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (itemTimer <= Time.time) {
            float rand = Random.Range(0 , 100);
            if (rand + itemProbAcum > 100 - itemProb)
            {
                itemProbAcum = 0f;
                PlaceItemAtRandom();
                itemTimer = Time.time + longCooldown;
            }
            else
            {
                itemTimer = Time.time + itemCooldown;
                itemProbAcum += rand;
            }
        }
	}

    void PlaceItemAtRandom()
    {
        if (itemParent.childCount >= itemsPositions.Length)
        {
            return;
        }
        int rand = Random.Range(0, itemsPositions.Length);
        while (rand == lastIndex)
        {
            rand = Random.Range(0, itemsPositions.Length);
        }
        GameObject go = (GameObject) Instantiate(itemPrefab, itemsPositions[rand].position, Quaternion.identity);
        go.transform.SetParent(itemParent);
    }
}
