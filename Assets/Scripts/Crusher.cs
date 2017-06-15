using UnityEngine;
using System.Collections;

public class Crusher : GenericTriggers {
    public float displacementTime; //up
    public float yDisplace;
    private bool moving;
    
    // Use this for initialization
    void Start () {
    }


    public override void OnTrigger() {
        Vector3 position = gameObject.transform.position;
        Vector3 endPosition = new Vector3(position.x, position.y + yDisplace, position.z);
        moving = true;
        StartCoroutine(MoveToPosition(position, endPosition, displacementTime));
    }


    public IEnumerator MoveToPosition(Vector3 currentPos, Vector3 position, float timeToMove) {
        var t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
        t = 0f;
        while (t < 1) {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(position, currentPos, t);
            yield return null;
        }
        moving = false;
    }



    public bool Moving {
        get {
            return moving;
        }

        set {
            moving = value;
        }
    }

}
