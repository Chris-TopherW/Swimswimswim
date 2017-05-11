using UnityEngine;
using System.Collections;

public enum Loops { Bb7Bossa, Bb7BossaBreakdown, F7Bossa, numLoops };

public class BackgroundMusic : Singleton<BackgroundMusic>
{
	public static BackgroundMusic backgroundMusic;

	public AudioClip[] clips;
	public string[] clipKeys;
	public int[] clipLengths;
    private BackgroundClip[] backgroundClips;
    private AudioSource[] sources;
    private bool startedPlaying;
    private int nextSource;
	public int currentClip;
	//this should defo not be editable! Just for debugging rn
	[SerializeField]
	private int nextClip = (int)Loops.Bb7Bossa;
	private int nextClipZ_1;
	bool clipHasChanged = false;
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
    
	public void Init(int loop)
	{
		if(Metronome.Instance.ready) 
		{
			Debug.Log("Error: cannot call StartMusic when metro is already ticking, call StopMusic first");
			return;
		}
		Metronome.Instance.SetBPM(126.0f);
		Metronome.Instance.ready = true;
		currentClip = nextClip;
		nextClip = loop;
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
			sources[nextSource].clip = backgroundClips[nextClip].clip;
            sources[nextSource].volume = 1;
            sources[nextSource].PlayScheduled(nextPlayTime);
            //Set next time to play
			nextPlay.Add(backgroundClips[nextSource].length);
            nextSource = (nextSource + 1) % sources.Length;
			if(nextClip != currentClip) {
				nextClipZ_1 = nextClip;
				clipHasChanged = true;
			}
        }
		else if(clipHasChanged) 
		{
			currentClip = nextClipZ_1; //next clip delayed 1 tick
			clipHasChanged = false;
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