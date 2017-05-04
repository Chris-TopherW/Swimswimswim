using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClip : MonoBehaviour {

	public string key;
	public AudioClip clip;
	public int barsLength;

	public BackgroundClip(AudioClip clip, string key, int barsLength) {
		this.key = key;
		this.clip = clip;
		this.barsLength = barsLength;
	}
}
