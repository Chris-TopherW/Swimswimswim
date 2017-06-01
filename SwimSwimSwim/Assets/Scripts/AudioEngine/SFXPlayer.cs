using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : Singleton<SFXPlayer> 
{
	public static SFXPlayer sfxPlayer;
	public AudioClip[] BbMixolydian;
	public AudioClip[] fMixolydian;
	public AudioClip[] drumsSamples;
	public AudioClip[] explosions;
	public int[] noteProb;
	public int numSources = 2;

	private AudioSource[] sources;
	private int currentSource;
	private int nextSource;
	private int currentNote;
	private int previousNote;

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
		switch(BackgroundMusic.Instance.currentClip.key) {
		case "Bbm":
			sources[currentSource].clip = BbMixolydian[MarkovMelody(BbMixolydian.Length)];
			break;
		case "F7":
			sources[currentSource].clip = BbMixolydian[MarkovMelody(fMixolydian.Length)];
			break;
		}
	}

	private void MixolImprov(int currentRoot, int newRoot) 
	{
		int transposition = newRoot - currentRoot;
	}

	private int MarkovMelody(int p_numChoices)
	{
		switch(Random.Range(0, 4))
			{
		case 0:
			return RandomWalk(p_numChoices);
			break;
		case 1:
			return RandomWalk(p_numChoices);
			break;
		case 2:
			return RandomWalk(p_numChoices);
			break;
		case 3:
			return RandomWalk(p_numChoices);
			break;
		default:
			return RandomWalk(p_numChoices);
			break;
			}
	}
	private int RandomWalk(int p_numChoices)
	{
		currentNote = (previousNote + Random.Range(-1, 2));
		if(currentNote < 0)
			currentNote = 0;
		if(currentNote >= p_numChoices)
			currentNote = p_numChoices - 1;
		previousNote = currentNote;
		return currentNote;
	}
	private int RandomJump(int p_numChoices)
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
		currentNote = noteIndex;
		previousNote = noteIndex;
		return currentNote;
	}
}