using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : Singleton<SFXPlayer> 
{
	public static SFXPlayer sfxPlayer;
	public AudioClip[] guitarSynth;
	public AudioClip[] drumsSamples;
	public AudioClip[] explosions;
	public int numSources = 2;

	private AudioSource[] sources;
	private int currentSource;
	private int nextSource;
	private int currentNote;
	private int previousNote;
	private MarkovGenerator markovGenerator = new MarkovGenerator();

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

	public void PlayCircleExpand() {

	}
	public void PlayCircleDestroy() {
		NotationTime nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
		sources[currentSource].clip = explosions[0];
		sources[currentSource].volume = 0.1f;
		sources[currentSource].PlayScheduled(nextPlayTime);
		currentSource++;
		if(currentSource == sources.Length) {
			currentSource = 0;
		}
	}
		
	private void NoteChoice() {
		int nextNote;
		int ofset = 0;

		switch(BackgroundMusic.Instance.currentClip.key) {
		case "Bbm":
			ofset = -2;
			break;
		case "F7":
			ofset = 5;
			break;
		}

		nextNote = markovGenerator.NextNote(currentNote) - ofset;
		if(nextNote < 0)
		{
			nextNote += 12;
		}
		if(nextNote >= 12)
		{
			nextNote -= 12;
		}
		sources[currentSource].clip = guitarSynth[nextNote];
		currentNote = nextNote;
	}
}