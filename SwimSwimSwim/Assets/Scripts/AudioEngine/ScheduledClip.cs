using UnityEngine;
using System.Collections;

public class ScheduledClip { 

    public AudioClip clip;

    private AudioSource[] sources;
    private Metronome metro;
    private NotationTime loop;
    private NotationTime timeToPlay;
    private double nextPlay;
    public GameObject sequencer;
    public int nextSource = 0;
    public int lastSource = 0;

    public ScheduledClip(Metronome metro, NotationTime time, NotationTime loop, AudioClip clip, GameObject sequencer)
    {

        //Init fields
        this.metro = metro;
        this.clip = clip;
        this.timeToPlay = time;
        this.loop = loop;
        this.sequencer = sequencer;

        //Init AudioSources 
        sources = new AudioSource[2];
        for (int i =0; i < sources.Length; i++)
        {
            sources[i] = sequencer.AddComponent < AudioSource >() as AudioSource;
        }
        
        //Schedule first AudioSource
        sources[nextSource].clip = clip;
        sources[nextSource].volume = 1;
        nextPlay = metro.GetFutureTime(timeToPlay.bar, timeToPlay.quarter, timeToPlay.tick);
        sources[nextSource].PlayScheduled(nextPlay);
        nextSource = (lastSource + 1) % sources.Length;
    }

    public void Update()
    {
        //If this is a looping note, schedule alternate AudioSource to play at the next loop interval.
        if (loop.isLooping() && AudioSettings.dspTime > nextPlay)
        {
            timeToPlay.Add(loop);
            sources[nextSource].clip = clip;
            sources[nextSource].volume = 1;
            nextPlay = metro.GetFutureTime(timeToPlay.bar, timeToPlay.quarter, timeToPlay.tick);
            sources[nextSource].PlayScheduled(nextPlay);
            lastSource = nextSource;
            nextSource = (lastSource + 1) % sources.Length;
        }
    }

}
