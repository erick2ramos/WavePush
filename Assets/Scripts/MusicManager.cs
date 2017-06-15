using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    public AudioClip engineStartClip;
    public AudioClip engineLoopClip;
    private static MusicManager instance;

    private AudioSource audioSource;
    // Use this for initialization
    void Start () {
        if (instance != null) {
            Destroy(gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        
        StartCoroutine(playEngineSound());
    }

    private IEnumerator playEngineSound() {
        audioSource.clip = engineStartClip;
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length-0.6f);
        audioSource.loop = true;
        audioSource.clip = engineLoopClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
