using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : Singleton<SFXPlayer> 
{
	public static SFXPlayer sfxPlayer;
	public AudioClip[] BbMixolydian;
	public AudioClip[] fMixolydian;
	public AudioClip[] drumsSamples;
	public int numSources = 2;

	private AudioSource[] sources;
	private int currentSource;
	private int nextSource;

	void Awake() 
	{
		sources = new AudioSource[numSources];
		for(int i = 0; i < sources.Length; i++) {
			sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
		}
	}

	public void PlayCircleStart() 
	{
		NotationTime nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
		NoteChoice();
		sources[currentSource].volume = 0.4f;
		sources[currentSource].PlayScheduled(nextPlayTime);
		currentSource++;
		if(currentSource == sources.Length) {
			currentSource = 0;
		}
	}

	void PlayCircleExpand() {

	}
	void PlayCircleEnd() {

	}

	void NoteChoice() {
		switch(BackgroundMusic.Instance.currentClip) {
		case (int)Loops.Bb7Bossa:
			sources[currentSource].clip = BbMixolydian[Random.Range(0, fMixolydian.Length)];
			break;
		case (int)Loops.Bb7BossaBreakdown:
			sources[currentSource].clip = BbMixolydian[Random.Range(0, fMixolydian.Length)];
			break;
		case (int)Loops.F7Bossa:
			sources[currentSource].clip = fMixolydian[Random.Range(0, fMixolydian.Length)];
			break;
		}
	}
}