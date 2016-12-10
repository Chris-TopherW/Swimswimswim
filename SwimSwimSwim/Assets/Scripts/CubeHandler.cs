using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class CubeHandler : MonoBehaviour
{
    public static CubeHandler handler;
    public Metronome metro;
    public static List<CubeThumper> cubes;
    public static List<CubeThumper> targetedCubes;
    private int numCubesHit = 0;

    private bool lockedThisTick = false;
    private NotationTime currentTickTime;


    private bool firing = false;
    
   

    // Use this for initialization
    void Start()
    {
        handler = this;
        metro = Metronome.metro;
        cubes = new List<CubeThumper>();
        targetedCubes = new List<CubeThumper>();
        currentTickTime = new NotationTime(metro.currentTime);
    }

    internal void AddCube(CubeThumper cubeThumper)
    {
        cubes.Add(cubeThumper);
    }

    // Update is called once per frame
    void Update()
    {
        //Otherwise we check if the tick has advanced then release the lock
        if (!currentTickTime.Equals(metro.currentTime))
        {
            lockedThisTick = false;
            currentTickTime = new NotationTime((metro.currentTime));
        }

        //If an object hasn't been locked on this tick do raycasting
        if (targetedCubes.Count < 8 && !lockedThisTick)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.SphereCastAll(ray.origin, 1, ray.direction, 1000.0f);
            for (int i = 0; i < hits.Length && i < 1; i++)
            {
                GameObject obj = hits[i].transform.gameObject;
                if (obj.CompareTag("Thumper"))
                {

                    CubeThumper thump = obj.GetComponent<CubeThumper>();
                    if (!thump.HasHit() && !lockedThisTick)
                    {
                        thump.HitCube();
                        targetedCubes.Add(thump);
                        lockedThisTick = true;
                    }
                }
            }
        }
        if ((Input.touchCount > 1 || Input.GetKeyDown(KeyCode.Space)) && !firing)
        {
            foreach (CubeThumper thump in targetedCubes)
            {
                thump.DestroyCube();
            }
            targetedCubes.Clear();
        }
    }
}
