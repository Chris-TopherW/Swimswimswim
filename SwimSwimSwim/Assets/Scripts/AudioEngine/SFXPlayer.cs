using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SFXPlayer : Singleton<SFXPlayer> 
{
	public static SFXPlayer sfxPlayer;
	public AudioClip[] guitarSynth;
	public AudioClip[] drumsSamples;
	public AudioClip[] explosions;
	public int numSources = 2;
	public AudioMixerGroup MelodyMixGroup;

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
			sources[i].outputAudioMixerGroup = MelodyMixGroup;
		}
	}

	public void PlayCircleStart() 
	{
		NotationTime nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
		NoteChoice();	 
		sources[currentSource].volume = 0.4f;
		sources[currentSource].pitch = 1.0f;
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
		sources[currentSource].pitch = 1.122462f; //up one tone
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
		case "GM":
			ofset = -7;
			break;
		}

        //Debug.Log("Previous note: " + currentNote);
		nextNote = markovGenerator.NextNote(currentNote) - ofset;
		if(nextNote < 0) nextNote += 12;

		if(nextNote >= 12) nextNote -= 12;

		sources[currentSource].clip = guitarSynth[nextNote];
		currentNote = nextNote + ofset;
        if (currentNote < 0) currentNote += 12;

        if (currentNote >= 12) currentNote -= 12;

        //Debug.Log("current note: " + currentNote);
    }
}