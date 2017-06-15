using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ItemType
{
    Multi,
    Strong
}

public class ItemScript : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            string it = ((ItemType) Random.Range(0,2)).ToString();
            other.GetComponent<CharacterController>().Item = it;
            Destroy(gameObject);
        }
    }
}
