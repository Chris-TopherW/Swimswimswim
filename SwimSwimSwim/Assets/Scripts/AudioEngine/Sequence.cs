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
        notes = new Note[5];
        NotationTime n1, n2, n3, n4, n5, barLoop, noLoop;
        n1 = new NotationTime(0,0,7);
        n2 = new NotationTime(0, 1, 1);
        n3 = new NotationTime(0, 1, 3);
        n4 = new NotationTime(0, 1, 5);
        n5 = new NotationTime(0, 0, 1);
        noLoop = new NotationTime(0, 0, 0);
        barLoop = new NotationTime(1, 0, 0);
       
        notes[0] = new Note(metro, n1, barLoop, clipsToPlay[0], gameObject);
        notes[1] = new Note(metro, n2, barLoop, clipsToPlay[1], gameObject);
        notes[2] = new Note(metro, n3, barLoop, clipsToPlay[2], gameObject);
        notes[3] = new Note(metro, n4, barLoop, clipsToPlay[3], gameObject);
        notes[4] = new Note(metro, n5, barLoop, clipsToPlay[4], gameObject);
 }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < notes.Length; i++)
        {
            notes[i].Update();
        }
    }
}

public struct NotationTime
{
    public int bar;
    public int quarter;
    public int tick;

    public NotationTime(int bar, int quarter, int tick)
    {
        this.bar = bar;
        this.quarter = quarter;
        this.tick = tick;
    }

    public bool loops()
    {
        return (bar != 0 || quarter != 0 || tick != 0);
    }

    public void Add(NotationTime other)
    {
        tick += other.tick;
        if (tick >=8 )
        {
            tick -= 8;
            quarter++;
        }
        quarter += other.quarter;
        if (quarter >= 4)
        {
            quarter -= 4;
            bar++;
        }
        bar += other.bar;

    }
}
