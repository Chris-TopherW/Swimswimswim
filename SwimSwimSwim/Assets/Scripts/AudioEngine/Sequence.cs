using UnityEngine;
using System.Collections;

public class Sequence : MonoBehaviour
{
    public ScheduledClip[] notes;
    public AudioClip[] clipsToPlay;
    public Metronome metro;
    // Use this for initialization
    void Start()
    {
        metro = Metronome.metro;
        notes = new ScheduledClip[1];
        NotationTime n1, barLoop, quarterLoop, noLoop, initialTime;
        n1 = new NotationTime(0,0,1);
        noLoop = new NotationTime(0, 0, 0);
        barLoop = new NotationTime(1, 0, 0);
        quarterLoop = new NotationTime(0, 1, 0);
        initialTime = new NotationTime(metro.currentTime);
        initialTime.Add(n1);
        
        //TODO: Cleanup + Dynamic handling of background music changes

        notes[0] = new ScheduledClip(metro, initialTime, barLoop, clipsToPlay[0], gameObject);
        


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

