using UnityEngine;
using System.Collections;
using System;

public enum CubeState
{
    IDLE,
    LOCKED,
    DESTROYED
}

public class CubeThumper : MonoBehaviour
{
    public CubeState state = CubeState.IDLE;
    private Metronome metro;

    private bool hasHit = false;
    private double timeToLock;

    private Material material;

    public AudioClip lockClip;


    // Use this for initialization
    void Start()
    {
        metro = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        material = gameObject.GetComponent<Renderer>().material;
        
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case (CubeState.IDLE):
                HandleIdle();
                break;
            case (CubeState.LOCKED):
                break;
            case (CubeState.DESTROYED):
                break;
            default :
                break;
        }
    }

    public bool HasHit()
    {
        return hasHit;
    }

    public void DestroyCube()
    {
        GameObject.Destroy(gameObject);
    }

    public void HitCube()
    {
        hasHit = true;
        ScheduledClip lockSound = new ScheduledClip(metro,
                                                           new NotationTime(metro.currentBar, metro.currentQuarter, metro.currentTick + 1),
                                                           new NotationTime(0, 0, 0),
                                                           lockClip,
                                                           gameObject);

        timeToLock = metro.GetFutureTime(metro.currentBar, metro.currentQuarter, metro.currentTick + 1);
    }

    void HandleIdle()
    {
        //If hit wait until quantized time -- then Lock
        if (hasHit && AudioSettings.dspTime >= timeToLock)
        {
            state = CubeState.LOCKED;
            material.SetColor("_Color", Color.red);
        }
        
    }
}