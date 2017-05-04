using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClip : MonoBehaviour 
{
	public string key;
	public AudioClip clip;
	private int numBars;
	public NotationTime bars;

	public BackgroundClip(AudioClip clip, string key, int numBars) 
	{
		this.key = key;
		this.clip = clip;
		this.numBars = numBars;
		bars = new NotationTime(numBars, 0,0);
	}
}
