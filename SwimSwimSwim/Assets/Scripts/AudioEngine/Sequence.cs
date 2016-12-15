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
        NotationTime n1, barLoop, quarterLoop, noLoop;
        n1 = new NotationTime(0,0,1);
        //n2 = new NotationTime(0, 1, 1);
        //n3 = new NotationTime(0, 1, 3);
        //n4 = new NotationTime(0, 1, 5);
        //n5 = new NotationTime(0, 0, 1);
        noLoop = new NotationTime(0, 0, 0);
        barLoop = new NotationTime(1, 0, 0);
        quarterLoop = new NotationTime(0, 1, 0);

        //TODO: Parameters for volume/pan/outputgroup for each note.
        //Parsing for each note
        //Restructure this so each note has an array of times to play?

        //TODO: Connect sequence to gameplay with visual
        //-- A way of triggering a sequence to play at a particular future time that makes musical sense
        //-- A way of tying gameplay behaviours and animations to a particular note. 

        notes[0] = new ScheduledClip(metro, n1, barLoop, clipsToPlay[0], gameObject);
        //notes[1] = new ScheduledClip(metro, n2, barLoop, clipsToPlay[1], gameObject);
        //notes[2] = new ScheduledClip(metro, n3, barLoop, clipsToPlay[2], gameObject);
        //notes[3] = new ScheduledClip(metro, n4, barLoop, clipsToPlay[3], gameObject);

        
        //notes[4] = new ScheduledClip(metro, n5, barLoop, clipsToPlay[4], gameObject);
        /*
        notes[5] = new Note(metro, new NotationTime(0, 0, 1), quarterLoop, clipsToPlay[5], gameObject);
        notes[6] = new Note(metro, new NotationTime(0, 0, 5), quarterLoop, clipsToPlay[5], gameObject);
        notes[7] = new Note(metro, new NotationTime(0, 3, 7), new NotationTime(0,0,1), clipsToPlay[5], gameObject);

        notes[8] = new Note(metro, new NotationTime(0, 0,1), barLoop, clipsToPlay[6], gameObject);
        notes[9] = new Note(metro, new NotationTime(0, 0, 5), barLoop, clipsToPlay[6], gameObject);
        notes[10] = new Note(metro, new NotationTime(0, 1, 7), barLoop, clipsToPlay[6], gameObject);
        notes[11] = new Note(metro, new NotationTime(0, 2, 5), barLoop, clipsToPlay[6], gameObject);

        notes[12] = new Note(metro, new NotationTime(0, 1, 1), barLoop, clipsToPlay[7], gameObject);
        notes[13] = new Note(metro, new NotationTime(0, 1, 3), barLoop, clipsToPlay[7], gameObject);
        notes[14] = new Note(metro, new NotationTime(0, 2, 3), barLoop, clipsToPlay[7], gameObject);
        notes[15] = new Note(metro, new NotationTime(0, 3, 1), barLoop, clipsToPlay[7], gameObject);
        */
        


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

