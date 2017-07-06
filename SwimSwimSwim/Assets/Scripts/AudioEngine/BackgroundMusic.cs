using UnityEngine;
using System.Collections;

//public enum Keys { C, CS, D, DS, E, F, FS, G, GS, A, AS, B };

public class BackgroundMusic : Singleton<BackgroundMusic>
{
	public static BackgroundMusic backgroundMusic;

	public AudioClip[] clips;
	public string[] clipKeys;
	public int[] clipLengths;
    private BackgroundClip[] backgroundClips;
	public BackgroundClip currentClip;
	public BackgroundClip nextClip;
    private AudioSource[] sources;
    private int nextSource;
    private NotationTime nextPlay;

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
		}
    }
    
	public void Init(string p_clip)
	{
		SetNextLoop(p_clip);
		if(Metronome.Instance.ready) 
		{
			Debug.Log("Error: cannot call StartMusic when metro is already ticking, call StopMusic first");
			return;
		}
		Metronome.Instance.SetBPM(100.0f);
		Metronome.Instance.ready = true;
		nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		HandleTickChange(Metronome.Instance.currentTime);
		Metronome.tickChangeDelegate += HandleTickChange;
	}
		
    public void HandleTickChange(NotationTime currentTime) 
	{
        if (currentTime.TimeAsTicks() == nextPlay.TimeAsTicks() - 1) 
		{
            double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
			//sources[nextSource].clip = nextClip.clip;
			sources[nextSource].clip = nextClip.clip;
            sources[nextSource].volume = 0.4f;
            sources[nextSource].PlayScheduled(nextPlayTime);
            //Set next time to play
			currentClip = nextClip;
			nextPlay.Add(currentClip.length);

            nextSource = (nextSource + 1) % sources.Length;
        }
    }

	private void SetNextLoop(string p_clip) 
	{
		for(int i = 0; i < backgroundClips.Length; i++)
		{
			string name = backgroundClips[i].clipName;
			if(name == p_clip)
				nextClip = backgroundClips[i];
		}
	}
}