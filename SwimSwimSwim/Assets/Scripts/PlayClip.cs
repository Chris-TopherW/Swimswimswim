﻿using UnityEngine;
using System.Collections;

public class PlayClip : MonoBehaviour 
{
	AudioSource audioSource;
	AudioClip audioClip;
	private float[] buffer;
	private int playHead = 0;
	private bool ready = false;

	void Start() {
		audioSource = GetComponent<AudioSource>();
		audioClip = audioSource.clip;
		buffer = new float[audioClip.samples];
		audioClip.GetData(buffer, 0);
		ready = true;
	}

	void OnAudioFilterRead(float[] samples, int channels) {
		if(!ready)
			return;
		for(int i= 0; i < samples.Length; i++) {
			if(playHead <= buffer.Length) {
				samples[i] = buffer[playHead];
			} else {
				samples[i] = 0.0f;
			}
			playHead++;
		}
	}
}
