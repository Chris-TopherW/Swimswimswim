using UnityEngine;
using System.Collections;

public class Note
{
    public int bar;
    public int quarter;
    public int tick;
    public AudioClip clip;
    private AudioSource[] sources;
    private Metronome metro;
    private NotationTime loop;
    private NotationTime timeToPlay;
    private double nextPlay;
    public GameObject sequencer;
    public int nextSource = 0;
    public int lastSource = 0;

    public Note(Metronome metro, NotationTime time, NotationTime loop, AudioClip clip, GameObject sequencer)
    {
        sources = new AudioSource[2];
        for (int i =0; i < sources.Length; i++)
        {
            sources[i] = sequencer.AddComponent < AudioSource >() as AudioSource;
        }
        this.metro = metro;
        this.bar = time.bar;
        this.quarter = time.quarter;
        this.tick = time.tick;
        this.clip = clip;
        this.timeToPlay = time;
        this.loop = loop;
        this.sequencer = sequencer;
        sources[nextSource].clip = clip;
        sources[nextSource].volume = 1;
        nextPlay = metro.GetFutureTime(bar, quarter, tick);
        sources[nextSource].PlayScheduled(nextPlay);
        nextSource = (lastSource + 1) % sources.Length;
    }

    public void Update()
    {
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
