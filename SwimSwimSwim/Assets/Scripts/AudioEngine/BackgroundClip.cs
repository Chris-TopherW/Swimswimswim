using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundClip 
{
	public string key;
	public AudioClip clip;
	private int numBars;
	public NotationTime length;
	public string clipName;

	public BackgroundClip(AudioClip p_clip, string p_key, int p_numBars) 
	{
		key = p_key;
		clip = p_clip;
		numBars = p_numBars;
		length = new NotationTime(numBars, 0,0);
		clipName = clip.name;
	}
}
