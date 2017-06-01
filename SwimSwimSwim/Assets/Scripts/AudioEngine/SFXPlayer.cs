using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : Singleton<SFXPlayer> 
{
	public static SFXPlayer sfxPlayer;
	public AudioClip[] BbMixolydian;
	public AudioClip[] fMixolydian;
	public AudioClip[] drumsSamples;
	public int[] noteProb;
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

	public void PlayCircleExpand() {

	}
	public void PlayCircleEnd() {

	}
		
	private void NoteChoice() {
		switch(BackgroundMusic.Instance.currentClip.key) {
		case "Bbm":
			sources[currentSource].clip = BbMixolydian[markovMelody(BbMixolydian.Length)];
			break;
		case "F7":
			sources[currentSource].clip = BbMixolydian[markovMelody(fMixolydian.Length)];
			break;
		}
	}

	private void MixolImprov(int currentRoot, int newRoot) 
	{
		int transposition = newRoot - currentRoot;
	}

	private int markovMelody(int p_numChoices)
	{
		int sum = 0;
		int noteIndex = 0;
		for(int i = 0; i < noteProb.Length; i++)
		{
			sum += noteProb[i];
		}
		int randomProbChoice = Random.Range(0, sum);

		sum = 0;
		for(int i = 0; i < noteProb.Length; i++)
		{
			if(sum < randomProbChoice) sum += noteProb[i];
			else
			{
				noteIndex = i;
				break;
			}
			noteIndex = i;
		}
		return noteIndex;
	}
}