using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class CubeHandler : MonoBehaviour
{
    public static CubeHandler handler;
    public static List<CubeThumper> cubes;
    public static List<CubeThumper> targetedCubes;
	public int maxLock = 8;
	private int numberOfLocks = 0;

    private bool lockedThisTick = false;
    private NotationTime currentTickTime;

    private double timeUntilCanFireAgain;


    private bool firing = false;

    void Awake()
    {

        handler = this;

        cubes = new List<CubeThumper>();
        targetedCubes = new List<CubeThumper>();
    }
   

    // Use this for initialization
    void Start()
    {
        currentTickTime = new NotationTime(Metronome.Instance.currentTime);
    }

    internal void AddCube(CubeThumper cubeThumper)
    {
        cubes.Add(cubeThumper);
    }

    // Update is called once per frame
    void Update()
    {
        //Otherwise we check if the tick has advanced then release the lock
        if (!currentTickTime.Equals(Metronome.Instance.currentTime))
        {
            lockedThisTick = false;
            currentTickTime = new NotationTime((Metronome.Instance.currentTime));
        }

        if (firing && AudioSettings.dspTime >= timeUntilCanFireAgain)
        {
            firing = false;
        }

        //If an object hasn't been locked on this tick do raycasting

        //Cannot rely on list count anymore, will have to keep track in method
		if (numberOfLocks < maxLock && !lockedThisTick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray.origin, 1, ray.direction, 1000.0f);
            for (int i = 0; i < hits.Length; i++)
            {
                GameObject obj = hits[i].transform.gameObject;
                if (obj.CompareTag("Thumper"))
                {

                    CubeThumper thump = obj.GetComponent<CubeThumper>();
                    if (thump.Lockable() && !lockedThisTick )
                    {
                        thump.HitCube();
                        if (!targetedCubes.Contains(thump))
                        {
                            targetedCubes.Add(thump);
                        }
                        lockedThisTick = true;
						numberOfLocks++;
                    }
                }
                if (!obj.CompareTag("Cleanup") && obj.CompareTag("Player") && !firing)
                {
                    Fire();
                }
            }
        }



    }

    void Fire()
    {
        firing = true;
        NotationTime firingStart = new NotationTime(Metronome.Instance.currentTime);
        firingStart.Add(new NotationTime(0, 0, 1));
        int pointsGained = 0;
        int multiplier = 0;
        foreach (CubeThumper thump in targetedCubes)
        {
            if (thump != null)
            {
                int lockLength = thump.GetLockLength();
                int fireResult = thump.FireCube(firingStart);
                if (fireResult > 0)
                {
                    pointsGained += fireResult;
                    multiplier++;
                }
                firingStart.Add(new NotationTime(0, 0, lockLength));
            }
        }
        GameManager.gameScore += pointsGained * multiplier;
        timeUntilCanFireAgain = Metronome.Instance.GetFutureTime(firingStart.bar, firingStart.quarter, firingStart.tick);
        targetedCubes.Clear();
        numberOfLocks = 0;
    }
}
