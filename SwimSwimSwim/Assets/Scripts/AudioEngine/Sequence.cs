using UnityEngine;
using System.Collections;

public class Sequence : MonoBehaviour
{
    private ScheduledClip note;
    public AudioClip[] clipsToPlay;
    public Metronome metro;
    // Use this for initialization
    void Start()
    {
        metro = Metronome.metro;
        
        //TODO: Cleanup + Dynamic handling of background music changes


    }

    // Update is called once per frame
    void Update()
    {
           if (note != null) note.Update();
    }

    public void Kill()
    {
            note.Kill();
    }

    public void BeginPlay()
    {
        NotationTime n1, barLoop, quarterLoop, noLoop, initialTime;
        n1 = new NotationTime(0, 0, 1);
        noLoop = new NotationTime(0, 0, 0);
        barLoop = new NotationTime(1, 0, 0);
        quarterLoop = new NotationTime(0, 1, 0);
        initialTime = new NotationTime(metro.currentTime);
        initialTime.Add(n1);

		note = gameObject.AddComponent < ScheduledClip >() as ScheduledClip;
        note.Init(metro, initialTime, barLoop, clipsToPlay[0]);
    }
}

