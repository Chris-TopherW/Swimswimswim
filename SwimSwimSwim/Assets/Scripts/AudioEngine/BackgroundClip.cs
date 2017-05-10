using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClip 
{
	public string key;
	public AudioClip clip;
	private int numBars;
	public NotationTime length;

	public BackgroundClip(AudioClip clip, string key, int numBars) 
	{
		this.key = key;
		this.clip = clip;
		this.numBars = numBars;
		length = new NotationTime(numBars, 0,0);
	}
}
