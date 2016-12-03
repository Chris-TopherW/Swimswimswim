using UnityEngine;
using System.Collections;

public class Sequence : MonoBehaviour
{
    public Note[] notes;
    public AudioClip[] clipsToPlay;
    private AudioSource[] sources;
    private double startTime;
    private double lastPlayed;
    private double nextPlay;
    private double interval;
    public int lastSourceTriggered = 0;
    public Metronome metro;
    // Use this for initialization
    void Start()
    {
        metro = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        sources = new AudioSource[16];
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
        }
        notes = new Note[4];
        NotationTime n1, n2, n3, n4;
        n1 = new NotationTime();
        n1.bar = 0;
        n1.quarter = 0;
        n1.tick = 6;
        n2 = new NotationTime();
        n2.bar = 0;
        n2.quarter = 1;
        n2.tick = 0;
        n3 = new NotationTime();
        n3.bar = 0;
        n3.quarter = 1;
        n3.tick = 2;
        n4 = new NotationTime();
        n4.bar = 0;
        n4.quarter = 1;
        n4.tick = 4;
        notes[0] = new Note(metro, n1, clipsToPlay[0], sources[0]);
        notes[1] = new Note(metro, n2, clipsToPlay[1], sources[1]);
        notes[2] = new Note(metro, n3, clipsToPlay[2], sources[2]);
        notes[3] = new Note(metro, n4, clipsToPlay[3], sources[3]);
    }

    // Update is called once per frame
    void Update()
    {
    }
}

public struct NotationTime
{
    public int bar;
    public int quarter;
    public int tick;

    public void Add(NotationTime other)
    {

    }
}
