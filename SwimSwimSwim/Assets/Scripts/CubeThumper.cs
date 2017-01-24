using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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

	public AudioClip[] lockClips;
	public AudioClip[] fireClips;


    public int MaxHealth;
    private int currentHealth;
    private int lockNum;

    public float vol;

    public Sprite[] lockSprites;
    public SpriteRenderer spriteRenderer;
    private ScheduledClip fireSound;


    // Use this for initialization
    void Start()
    {
        metro = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        material = gameObject.GetComponent<Renderer>().material;

        currentHealth = MaxHealth;

    }
    // Update is called once per frame
    void Update()
    {
        if (fireSound !=null)
        {
            fireSound.Update();
        }

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

    public bool FullyLocked()
    {
        return (currentHealth == lockNum);
    }

    public bool Lockable()
    {
        return (currentHealth > lockNum && hasFired == false);
    }

    public int GetLockLength()
    {
        return lockNum;
    }

    public void FireCube(NotationTime toFire)
    {
        hasFired = true;
        currentHealth -= lockNum;
        spriteRenderer.enabled = false;
        fireSound = new ScheduledClip(metro,
                                                           toFire,
                                                           new NotationTime(0, 0, 0),
														   fireClips[UnityEngine.Random.Range(0, fireClips.Length)],
                                                           gameObject);
        fireSound.SetClipLength(new NotationTime(0,1,0), 0.01f);
        
        timeToFire = metro.GetFutureTime(toFire.bar, toFire.quarter, toFire.tick);

        if (Destroyed())
        {
            NotationTime timeLeft = new NotationTime(toFire);
            timeLeft.Add(new NotationTime(0, 1, 0));
            timeAlive = metro.GetFutureTime(timeLeft.bar, timeLeft.quarter, timeLeft.tick);
        }

        lockNum = 0;
    }

    public void HitCube()
    {

        lockNum++;
        ScheduledClip lockSound = new ScheduledClip(metro,
                                                           new NotationTime(metro.currentBar, metro.currentQuarter, metro.currentTick + 1),
                                                           new NotationTime(0, 0, 0),
														   lockClips[UnityEngine.Random.Range(0, lockClips.Length)],
                                                           gameObject);

        lockSound.Randomizer();
        lockSound.setVolume(vol);

        timeToLock = metro.GetFutureTime(metro.currentBar, metro.currentQuarter, metro.currentTick + 1);

        state = CubeState.LOCKED;
        if (lockNum == 0)
        {
            spriteRenderer.enabled = false;
        }
        else if (FullyLocked())
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = lockSprites[7];
        }
        else
        {
            spriteRenderer.enabled = true;
            spriteRenderer.sprite = lockSprites[lockNum-1];
        }
    }

    void HandleIdle()
    {
        Color health = Color.Lerp(Color.red, Color.green, (float)currentHealth / (float)MaxHealth);
        material.SetColor("_Color", health);
    }

    bool Destroyed()
    {
        return (currentHealth <= 0);
    }

    void HandleLocked()
    {
        if (hasFired && AudioSettings.dspTime >= timeToFire)
        {
            if (Destroyed())
            {
                state = CubeState.DESTROYED;
                material.SetColor("_Color", Color.red);
            }
            else
            {

                Color health = Color.Lerp(Color.red, Color.green, (float)currentHealth / (float)MaxHealth);
                material.SetColor("_Color", health);
                state = CubeState.IDLE;
                hasHit = false;
                hasFired = false;
            }

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