using UnityEngine;
using System.Collections;

public class ScheduledMusicTest : MonoBehaviour {

    private AudioSource[] sources;
    public AudioClip[] clips;
    private double audioStartTime, lastClipStart, lastClipLength;
    private int clipsLoaded;
    private int currentPlayingSource;
    // Use this for initialization
    void Start () {
        sources = new AudioSource[2];
        for (int i = 0; i < sources.Length; i++){
            sources[i] = new AudioSource();
        }
        sources[0].clip = clips[0];
        clipsLoaded++;
        sources[0].Play();
        lastClipStart = AudioSettings.dspTime;
        lastClipLength = sources[0].clip.length;
	}
	
	// Update is called once per frame
	void Update () {
	    for (int i = 0; i < sources.Length; i++)
        {
           if (!sources[i].isPlaying)
            {

            }
        }
	}
}
