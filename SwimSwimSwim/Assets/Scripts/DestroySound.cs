using UnityEngine;
using System.Collections;

public class DestroySound : MonoBehaviour {

	AudioSource 		audioSource;

	void Start () {
		DontDestroyOnLoad( this.gameObject );
		audioSource = GetComponent<AudioSource> ();
	}

	void Update () {
	
		if ( !audioSource.isPlaying ) {
			Destroy ( gameObject );
		}

	}
}
