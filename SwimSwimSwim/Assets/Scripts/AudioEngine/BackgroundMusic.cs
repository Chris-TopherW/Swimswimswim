using UnityEngine;
using System.Collections;

public class BackgroundMusic : MonoBehaviour
{
    private ScheduledClip note;
    public BackgroundClip backgroundClip;
    private AudioSource[] sources;
    private bool startedPlaying;
    private int clipPlaying;
    private int nextSource;
    private int nextClip;
    private NotationTime nextPlay;

	void Start() 
	{
        sources = new AudioSource[2];
        for (int i = 0; i < sources.Length; i++) {
            sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
        }
    }
    
	public void Init(BackgroundClip bgClip)
	{
		if(Metronome.Instance.ready) 
		{
			Debug.Log("Error: cannot call StartMusic when metro is already ticking, call StopMusic first");
			return;
		}
		Metronome.Instance.SetBPM(126.0f);
		Metronome.Instance.ready = true;
		nextPlay = new NotationTime(Metronome.Instance.currentTime);
		nextPlay.AddTick();
		//Call the tick change once to schedule the first time, then subscribe to the tick change delegate to handle all future scheduling.
		HandleTickChange(Metronome.Instance.currentTime);
		Metronome.tickChangeDelegate += HandleTickChange;
	}

	public NotationTime PlayNext(BackgroundClip bgClip) 
	{
		nextPlay = new NotationTime(Metronome.Instance.currentBar);
		nextPlay.AddTime(Metronome.Instance.currentTick);
		nextPlay.AddTick();
		//Call the tick change once to schedule the first time, then subscribe to the tick change delegate to handle all future scheduling.
		HandleTickChange(Metronome.Instance.currentTime);
		Metronome.tickChangeDelegate += HandleTickChange;
		return null;
	}

    /**
     * This will call on a tick change, we do scheduling on the tick before, this leaves it as late as possible
     * so music can be as D Y N A M I C as.
     */
    public void HandleTickChange(NotationTime currentTime) 
	{
        if (currentTime.TimeAsTicks() == nextPlay.TimeAsTicks() - 1) 
		{
            double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
            sources[nextSource].clip = backgroundClip.clip;
            sources[nextSource].volume = 1;
            sources[nextSource].PlayScheduled(nextPlayTime);
            //Set next time to play
            nextPlay.Add(new NotationTime(backgroundClip.barsLength, 0, 0));
            //Set the next source to play
            nextSource = (nextSource + 1) % sources.Length;
            //Set the next clip to play -- this logic could go into another method
//            nextClip++;
//            nextClip = nextClip % clipsToPlay.Length;
        }
    }
}