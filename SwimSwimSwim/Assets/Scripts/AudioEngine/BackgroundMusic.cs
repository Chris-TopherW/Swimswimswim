﻿using UnityEngine;
using System.Collections;

public enum Loops { loop1, loop2, loop3, numLoops };

public class BackgroundMusic : MonoBehaviour
{
	public AudioClip[] clips;
	public string[] clipKeys;
	public int[] clipLengths;
    private BackgroundClip[] backgroundClips;
    private AudioSource[] sources;
    private bool startedPlaying;
    private int nextSource;
	private int clipPlaying;
	//this should defo not be public! Just for debugging rn
	public int nextClip = (int)Loops.loop1;
    private NotationTime nextPlay;

	private string[] keys = {"BbMinor", "F7"};

	void Start() 
	{
        sources = new AudioSource[2];
        for (int i = 0; i < sources.Length; i++) {
            sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
        }
		//setup background loop meta data
		backgroundClips = new BackgroundClip[clips.Length];
		for(int i = 0; i < clips.Length; i++) 
		{
			backgroundClips[i] = new BackgroundClip(clips[i], clipKeys[i], clipLengths[i]);
			Debug.Log(backgroundClips[i].length.bar);
		}
    }
    
	public void Init(int loop)
	{
		if(Metronome.Instance.ready) 
		{
			Debug.Log("Error: cannot call StartMusic when metro is already ticking, call StopMusic first");
			return;
		}
		Metronome.Instance.SetBPM(126.0f);
		Metronome.Instance.ready = true;
		clipPlaying = nextClip;
		nextClip = loop;
		nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		//Call the tick change once to schedule the first time, then subscribe to the tick change delegate to handle all future scheduling.
		HandleTickChange(Metronome.Instance.currentTime);
		Metronome.tickChangeDelegate += HandleTickChange;
	}

//	public NotationTime PlayNext(BackgroundClip bgClip) 
//	{
//		nextPlay = new NotationTime(Metronome.Instance.currentBar);
//		nextPlay.AddTime(Metronome.Instance.currentTick);
//		nextPlay.AddTick();
//		//Call the tick change once to schedule the first time, then subscribe to the tick change delegate to handle all future scheduling.
//		HandleTickChange(Metronome.Instance.currentTime);
//		Metronome.tickChangeDelegate += HandleTickChange;
//		return null;
//	}

    /**
     * This will call on a tick change, we do scheduling on the tick before, this leaves it as late as possible
     * so music can be as D Y N A M I C as.
     */
    public void HandleTickChange(NotationTime currentTime) 
	{
        if (currentTime.TimeAsTicks() == nextPlay.TimeAsTicks() - 1) 
		{
            double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
			sources[nextSource].clip = backgroundClips[nextClip].clip;
            sources[nextSource].volume = 1;
            sources[nextSource].PlayScheduled(nextPlayTime);
            //Set next time to play
			nextPlay.Add(backgroundClips[nextSource].length);
			//Debug.Log(backgroundClips[nextSource].length.bar);
            nextSource = (nextSource + 1) % sources.Length;
        }
    }
	public void SetNextLoop(int loop) 
	{
		if(loop < (int)Loops.numLoops) {
		nextClip = loop;
		}
		else
		{
			Debug.Log("Error, next loop set to non-existent loop!");
			return;
		}
	}
}