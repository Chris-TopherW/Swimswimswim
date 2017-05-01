using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlastZoneStates
{
    Hidden, Visible
}

public class BlastZone : MonoBehaviour {

    public AudioClip[] lockClips;
    public AudioClip[] fireClips;
    public AudioClip[] destroyClips;

    private MeshRenderer mesh;

    NotationTime timeToCreate;
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CreateZone(NotationTime timeToCreate)
    {
        this.timeToCreate = timeToCreate;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        ScheduledClip lockSound = gameObject.AddComponent<ScheduledClip>() as ScheduledClip;
        lockSound.Init(new NotationTime(Metronome.Instance.currentBar, Metronome.Instance.currentQuarter, Metronome.Instance.currentTick + 1),
                                                           new NotationTime(0, 0, 0),
                                                           lockClips[UnityEngine.Random.Range(0, lockClips.Length)]);
        lockSound.setVolume(0.4f);
    }
}
