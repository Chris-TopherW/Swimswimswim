using UnityEngine;
using System.Collections;

public class Note
{
    public int bar;
    public int quarter;
    public int tick;
    public AudioClip clip;
    private AudioSource source;
    private Metronome metro;
    

    public Note(Metronome metro, NotationTime time, AudioClip clip, AudioSource source)
    {
        this.metro = metro;
        this.bar = time.bar;
        this.quarter = time.quarter;
        this.tick = time.tick;
        this.clip = clip;
        this.source = source;
        source.clip = clip;
        source.volume = 1;
        double nextPlay = metro.GetFutureTime(bar, quarter, tick);
        source.PlayScheduled(nextPlay);
    }

}
