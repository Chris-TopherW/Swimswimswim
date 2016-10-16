using UnityEngine;
using System.Collections;
using GAudio;
using System;
using System.Collections.Generic;
using System.Linq;

public class CubeHandler : MonoBehaviour, IGATPulseController
{
    public static CubeHandler handler;
    public PulseModule pulse;
    public PulsedPatternModule pattern;
    public PulsedPatternModule firePattern;
    public static List<CubeThumper> hitCubes;
    public static List<CubeThumper> cubesToDestroy;
    private int numCubesHit = 0;
    private int subPulseStepIndex = 0;
    private int masterStepIndex = 0;
    private int masterTotalSteps = 0;
    private int subPulseTotalSteps = 8;

    public static bool firing = false;
    private bool fireInit = false;


    public int[] fireIndexes;

    void OnEnable()
    {
        handler = this;
        //Subscribe and register, we need the info! Always in OnEnable
        pulse.RegisterPulseController(this);
    }
    void OnDisable()
    {
        //Always unregister in OnDisable
        pulse.UnregisterPulseController(this);
    }

    public void OnPulseControl(IGATPulseInfo prevPulseInfo)
    {

        //This info is needed to enable the next pulse -- even if the previous pulse was the last
        subPulseStepIndex = prevPulseInfo.StepIndex;
        subPulseTotalSteps = prevPulseInfo.NbOfSteps;
        masterStepIndex = prevPulseInfo.PulseSender.MasterPulseInfo.StepIndex;
        masterTotalSteps = prevPulseInfo.PulseSender.MasterPulseInfo.NbOfSteps;

        //This block disables the previous pulse step, 
        //then sets the number of samples to play in the upcoming pulse to the number of cubes locked on since the last pulse
        if (true)
        {
            pattern.SubscribedSteps[subPulseStepIndex] = false;
            if (numCubesHit > 0)
            {
                pattern.randTogetherLimit = numCubesHit;
                pattern.SubscribedSteps[(subPulseStepIndex + 1) % subPulseTotalSteps] = true;
            }
            numCubesHit = 0;
        }



    }

    // Use this for initialization
    void Awake()
    {
        hitCubes = new List<CubeThumper>();

        cubesToDestroy = new List<CubeThumper>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.SphereCastAll(ray.origin, 1, ray.direction, 1000.0f);
        for (int i = 0; i < hits.Length && i < 1; i++)
        {
            GameObject obj = hits[i].transform.gameObject;
            if (obj.CompareTag("Thumper"))
            {

                CubeThumper thump = obj.GetComponent<CubeThumper>();
                if (!thump.IsTriggered() && hitCubes.Count < 8 && !cubesToDestroy.Contains(thump))
                {
                    thump.isTriggered = true;
                    hitCubes.Add(thump);
                    numCubesHit++;
                }
            }
        }

        if ((Input.touchCount > 1 || Input.GetKeyDown(KeyCode.Space)) && !firing)
        {
            firing = true;
            for (int i = 0; i < hitCubes.Count; i++)
            {
                cubesToDestroy.Add(hitCubes[i]);
            }
            hitCubes.Clear();

        }
    }
}
