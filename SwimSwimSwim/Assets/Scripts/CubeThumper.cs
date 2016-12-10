using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

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
    private bool hasFired = false;
    private double timeToLock;
    private double timeToFire;
    private double timeAlive;

    private Material material;

    public AudioClip lockClip;
    public AudioClip fireClip;


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
                HandleLocked();
                break;
            case (CubeState.DESTROYED):
                HandleDestroyed();
                break;
            default :
                break;
        }
    }

    public bool HasHit()
    {
        return hasHit;
    }

    public void DestroyCube(NotationTime toFire)
    {
        hasFired = true;
        ScheduledClip fireSound = new ScheduledClip(metro,
                                                           toFire,
                                                           new NotationTime(0, 0, 0),
                                                           fireClip,
                                                           gameObject);

        timeToFire = metro.GetFutureTime(toFire.bar, toFire.quarter, toFire.tick);
        NotationTime timeLeft = new NotationTime(toFire);
        timeLeft.Add(new NotationTime(0,0,1));
        timeAlive = metro.GetFutureTime(timeLeft.bar, timeLeft.quarter, timeLeft.tick); 
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

        state = CubeState.LOCKED;
        material.SetColor("_Color", Color.red);
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

    void HandleLocked()
    {
        if (hasFired && AudioSettings.dspTime >= timeToFire)
        {
            state = CubeState.DESTROYED;
            material.SetColor("_Color", Color.green);
        }
    }

    void HandleDestroyed()
    {
        if (AudioSettings.dspTime >= timeAlive)
        {
            GameObject.Destroy(gameObject);
        }
    }
}