using UnityEngine;
using System.Collections;

public class DestroySound : MonoBehaviour {

	AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (!audioSource.isPlaying)
			Destroy (gameObject);

	}
}
