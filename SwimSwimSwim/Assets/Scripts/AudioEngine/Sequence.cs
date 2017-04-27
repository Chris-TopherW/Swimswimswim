using UnityEngine;
using System.Collections;

public class Sequence : MonoBehaviour
{
    private ScheduledClip note;
    public AudioClip[] clipsToPlay;
    private AudioSource[] sources;
    private bool startedPlaying;
    private int clipPlaying;
    private int nextSource;
    // Use this for initialization
    void Start()
    {
        //TODO: Make sure to set the BPM of the metro before playing clips as this is now a singleton.

        Metronome.barChangeDelegate += ChangeClip;

        //Init AudioSources 
        sources = new AudioSource[2];
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Kill()
    {
            note.Kill();
    }

    public void BeginPlay()
    {
      /*  NotationTime n1, barLoop, quarterLoop, noLoop, initialTime;
        n1 = new NotationTime(0, 0, 1);
        noLoop = new NotationTime(0, 0, 0);
        barLoop = new NotationTime(1, 0, 0);
        quarterLoop = new NotationTime(0, 1, 0);
        initialTime = new NotationTime(Metronome.Instance.currentTime);
        initialTime.Add(n1);

		note = gameObject.AddComponent < ScheduledClip >() as ScheduledClip;
        note.Init(initialTime, noLoop, clipsToPlay[0]);
        clipPlaying = 0;*/
    }

    public void ChangeClip(NotationTime currentTime)
    {
        if
            (currentTime.bar % 8 == 1)
        {
            NotationTime initialTime = new NotationTime(Metronome.Instance.currentTime);
            initialTime.Add(new NotationTime(0, 0, 1));
            double nextPlay = Metronome.Instance.GetFutureTime(initialTime);
            sources[nextSource].clip = clipsToPlay[clipPlaying];
            sources[nextSource].volume = 1;
            sources[nextSource].PlayScheduled(nextPlay);
            nextSource = (nextSource + 1) % sources.Length;
            clipPlaying++;
            clipPlaying = clipPlaying % clipsToPlay.Length;
        }
    }
}

