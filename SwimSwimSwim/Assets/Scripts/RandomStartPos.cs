using UnityEngine;
using System.Collections;

//randomise the start position of the ambience

public class RandomStartPos : MonoBehaviour {

	private AudioSource audioSource;
	private float length;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
		length = audioSource.clip.length;
		audioSource.time = Random.Range (0.0f, length);
		audioSource.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
