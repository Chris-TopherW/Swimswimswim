using UnityEngine;
using System.Collections;

public class RandomStartPos : MonoBehaviour {

	private AudioSource 	audioSource;
	private float 			length;

	void Start () {
		audioSource = GetComponent<AudioSource> ();
		length = audioSource.clip.length;
		audioSource.time = Random.Range ( 0.0f, length );
		audioSource.Play ();
	}
}
