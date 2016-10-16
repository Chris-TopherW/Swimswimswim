using UnityEngine;
using System.Collections;
using GAudio;
using System;

public class CubeFire : MonoBehaviour, IGATPulseController
{
    public PulseModule fireModule;
    public PulsedPatternModule pattern;

    private int subPulseStepIndex = 0;
    private int masterStepIndex = 0;
    private int masterTotalSteps = 0;
    private int subPulseTotalSteps = 8;

    public void OnEnable()
    {
        fireModule.RegisterPulseController(this);
    }

    public void OnDisable()
    {
        fireModule.UnregisterPulseController(this);
    }

    public void OnPulseControl(IGATPulseInfo prevPulseInfo)
    {
        subPulseStepIndex = prevPulseInfo.StepIndex;
        subPulseTotalSteps = prevPulseInfo.NbOfSteps;
        /*
        * We're firing here -- we're using the master pulse currently, so we only want this code to run on the first sub pulse
        * Again, the previous pulse is disabled for the fire pattern.
        * The first cube in the list gets passed the pulseIndex that the destroy animation should trigger on, and is then removed.
        * This ensures that the cubes are destroyed in the order that they were originally selected.
        * 
        * TODO: A more complicated destroy pattern.
        */
        if (CubeHandler.firing && CubeHandler.cubesToDestroy.Count != 0)
        {
            pattern.SubscribedSteps[subPulseStepIndex] = false;
            pattern.SubscribedSteps[(subPulseStepIndex + 1) % subPulseTotalSteps] = true;
            CubeHandler.cubesToDestroy[0].hitIndex = (subPulseStepIndex + 1) % subPulseTotalSteps;
            CubeHandler.cubesToDestroy[0].isFiring = true;
            CubeHandler.cubesToDestroy.Remove(CubeHandler.cubesToDestroy[0]);
        }
        else
        {
            pattern.SubscribedSteps[subPulseStepIndex] = false;
            if (CubeHandler.cubesToDestroy.Count == 0)
            {
                CubeHandler.firing = false;
            }
        }
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
