using UnityEngine;
using System.Collections;

public class ScheduledClip : MonoBehaviour { 

    public AudioClip clip;

    private AudioSource[] sources;
    private NotationTime loop;
    private NotationTime timeToPlay;
    private double nextPlay;
    private GameObject sequencer;
    public int nextSource = 0;
    public int lastSource = 0;

    public bool customLength = false;
    private double fadeoutStart, fadeoutLength;

	//TODO: Refactor so that no NotationTime is needed for loop, just a bool
	//		This will require the ScheduledClip to calculate the NotationTime from the length of the AudioClip
	//		Alternatively the loop length could be included in the metadata for the audioclip and retrieved from that.

	public void Init(NotationTime time, NotationTime loop, AudioClip clip)
    {

        //Init fields
        this.clip = clip;
        this.timeToPlay = time;
        this.loop = loop;
		this.sequencer = gameObject;

        //Init AudioSources 
        sources = new AudioSource[2];
        for (int i =0; i < sources.Length; i++)
        {
            sources[i] = sequencer.AddComponent < AudioSource >() as AudioSource;
        }
        
        //Schedule first AudioSource
        sources[nextSource].clip = clip;
        sources[nextSource].volume = 1;
        nextPlay = Metronome.Instance.GetFutureTime(timeToPlay.bar, timeToPlay.quarter, timeToPlay.tick);
        sources[nextSource].PlayScheduled(nextPlay);
        nextSource = (lastSource + 1) % sources.Length;
    }

    public void Randomizer()
    {
        sources[lastSource].volume = Random.Range(0.4f, 1.0f);
        sources[lastSource].pitch = Random.Range(0.1f, 2.1f);
    }

    public void Kill()
    {
        loop = new NotationTime(0,0,0);
        foreach (AudioSource source in sources)
        {
            source.volume = 0;

        }
    }

    public void setVolume(float vol)
    {
        sources[lastSource].volume = vol;
    }

    public void SetClipLength(NotationTime length, float fadeoutLength)
    {
        customLength = true;
        length.Add(timeToPlay);
        fadeoutStart = Metronome.Instance.GetFutureTime(length) - fadeoutLength;
        Debug.Log(fadeoutStart + fadeoutLength - nextPlay);

    }

    public void Update()
    {
        //If this is a looping note, schedule alternate AudioSource to play at the next loop interval.
        if (loop.isLooping() && AudioSettings.dspTime > nextPlay)
        {
            timeToPlay.Add(loop);
            sources[nextSource].clip = clip;
            sources[nextSource].volume = 1;
            nextPlay = Metronome.Instance.GetFutureTime(timeToPlay.bar, timeToPlay.quarter, timeToPlay.tick);
            sources[nextSource].PlayScheduled(nextPlay);
            lastSource = nextSource;
            nextSource = (lastSource + 1) % sources.Length;
        }

		/*if(loop.isLooping()){
			sources[0].loop = true;
		}*/

        if (customLength && AudioSettings.dspTime >= fadeoutStart)
        {
            float t = (float)(AudioSettings.dspTime - fadeoutStart)/(float)fadeoutLength;
            float vol = Mathf.Lerp(1, 0, t);
            sources[lastSource].volume = vol;
        }
    }

}
