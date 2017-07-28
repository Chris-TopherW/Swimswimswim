using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlastZoneStates
{
    Hidden, Visible
}

public class BlastZone : MonoBehaviour {
    public const int MAX_SIZE = 8;

    private MeshRenderer mesh;
    public float zoneScale = 3.0f;
    public float zoneGrowth = 1f;
    private bool growNextTick = false;
    


    private int currentSize;

    //NotationTime timeToCreate;
    // Use this for initialization
    void Awake () {
        Metronome.tickChangeDelegate += HandleTickChange;
        this.gameObject.transform.localScale = new Vector3(zoneScale, 1f, zoneScale);
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
        //this.timeToCreate = timeToCreate;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        currentSize++;
		SFXPlayer.Instance.PlayCircleStart();
    }
    
    public void DestroyZone()
    {
        Metronome.tickChangeDelegate -= HandleTickChange;
		SFXPlayer.Instance.PlayCircleDestroy();
        Destroy(gameObject);
    }



    public void GrowNextTick()
    {
        growNextTick = true;
		SFXPlayer.Instance.PlayCircleStart();
        currentSize++;
    }

    public bool CanGrow()
    {
        return currentSize < MAX_SIZE;
    }

    public void GrowZone()
    {
        zoneScale += zoneGrowth;
        if (gameObject != null)
        {
            this.gameObject.transform.localScale = new Vector3(zoneScale, 1f, zoneScale);
        }

    }
}
