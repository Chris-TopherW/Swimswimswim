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
    private AudioSource[] sources;
    private MeshRenderer mesh;
    private float zoneScale = 3.0f;
    private bool growNextTick = false;

    NotationTime timeToCreate;
    // Use this for initialization
    void Start () {
        Metronome.tickChangeDelegate += HandleTickChange;

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HandleTickChange(NotationTime currentTime)
    {
        if (growNextTick)
        {
            GrowZone();
            growNextTick = false;
        }
    }

    public void CreateZone(NotationTime timeToCreate)
    {

        sources = new AudioSource[2];
        for (int i = 0; i < sources.Length; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>() as AudioSource;
        }
        this.timeToCreate = timeToCreate;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        PlayLockSound();
    }

    public void PlayLockSound()
    {
        NotationTime nextPlay = new NotationTime(Metronome.Instance.currentTime);
        nextPlay.AddTick();
        double nextPlayTime = Metronome.Instance.GetFutureTime(nextPlay);
        sources[0].clip = lockClips[Random.Range(0, lockClips.Length)];
        sources[0].volume = 0.4f;
        sources[0].PlayScheduled(nextPlayTime);
    }

    public void GrowNextTick()
    {
        growNextTick = true;
        PlayLockSound();
    }

    public void GrowZone()
    {
        zoneScale += 0.5f;
        this.gameObject.transform.localScale = new Vector3(zoneScale, 0.05f, zoneScale);

    }
}
