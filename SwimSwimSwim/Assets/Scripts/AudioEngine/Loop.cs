using UnityEngine;
using System.Collections;

public class Loop : MonoBehaviour {
    private AudioSource[] sources;
    public AudioClip[] clipsToPlay;
    private double startTime;
    private double lastPlayed;
    private double nextPlay;
    private double interval;
    [Range(1, 480)]
    public double BPM = 120.0;
    public int lastSourceTriggered = 0;
	// Use this for initialization
	void Awake () {
        sources = new AudioSource[clipsToPlay.Length * 4];
        for (int i = 0; i < sources.Length/clipsToPlay.Length; i++)
        {
            for (int j = 0; j < clipsToPlay.Length; j++)
            {
                Debug.Log("Fuuckin i: " + i + " and j: " + j);
                sources[j * clipsToPlay.Length + i] = gameObject.AddComponent<AudioSource>() as AudioSource;
                sources[j * clipsToPlay.Length + i].clip = clipsToPlay[i];
            }
        }
        //sources[3].volume = 0;
        lastPlayed = Metronome.startTime;
        interval = 60 / BPM;
        nextPlay = lastPlayed + interval;

        for (int i = 0; i < clipsToPlay.Length; i++)
        {
            sources[lastSourceTriggered].PlayScheduled(nextPlay);
            int sourceToPlay = (lastSourceTriggered + 1) % sources.Length;
            lastSourceTriggered = sourceToPlay;
        }
    }

    // Update is called once per frame
    void Update() {
        interval = 60 / BPM;
        if (AudioSettings.dspTime > nextPlay)
        {
            lastPlayed = nextPlay;
            nextPlay = lastPlayed + interval;
            for (int j = 0; j < clipsToPlay.Length; j++)
            {
                int sourceToPlay = (lastSourceTriggered + 1) % sources.Length;
                sources[sourceToPlay].PlayScheduled(nextPlay);
                lastSourceTriggered = sourceToPlay;
            }
            Debug.Log(nextPlay);
        }

    }
}
