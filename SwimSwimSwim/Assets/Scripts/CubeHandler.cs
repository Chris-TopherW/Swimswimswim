using UnityEngine;
using System.Collections;
using GAudio;
using System;
using System.Collections.Generic;
using System.Linq;

public class CubeHandler : MonoBehaviour, IGATPulseController
{
    public PulseModule                           pulse;
    public PulsedPatternModule                   pattern;
    public PulsedPatternModule                   firePattern;
    public static List<CubeThumper>              hitCubes;
    public static List<CubeThumper>              cubesToDestroy;
    private int numCubesHit = 0;
    private int subPulseStepIndex = 0;
    private int masterStepIndex = 0;
    private int masterTotalSteps = 0;
    private int subPulseTotalSteps = 8;

    private bool firing = false;
    private bool fireInit = false;


    public int[] fireIndexes;

    void OnEnable()
    {
        //Subscribe and register, we need the info! Always in OnEnable
        pulse.RegisterPulseController(this);
    }
    void OnDisable()
    {
        //Always unregister in OnDisable
        pulse.UnregisterPulseController(this);
    }

    public void OnPulseControl( IGATPulseInfo prevPulseInfo )
    {

        //This info is needed to enable the next pulse -- even if the previous pulse was the last
        subPulseStepIndex = prevPulseInfo.StepIndex;
        subPulseTotalSteps = prevPulseInfo.NbOfSteps;
        masterStepIndex = prevPulseInfo.PulseSender.MasterPulseInfo.StepIndex;
        masterTotalSteps = prevPulseInfo.PulseSender.MasterPulseInfo.NbOfSteps;

        //This block disables the previous pulse step, 
        //then sets the number of samples to play in the upcoming pulse to the number of cubes locked on since the last pulse
        if (true) {
            pattern.SubscribedSteps[subPulseStepIndex] = false;
            if (numCubesHit > 0)
            {
                pattern.randTogetherLimit = numCubesHit;
                pattern.SubscribedSteps[(subPulseStepIndex + 1) % subPulseTotalSteps] = true;
            }
            numCubesHit = 0;


            if (subPulseStepIndex == 0)
            {
                firePattern.SubscribedSteps[(masterStepIndex)] = false;
            }
        }
        /*
         * We're firing here -- we're using the master pulse currently, so we only want this code to run on the first sub pulse
         * Again, the previous pulse is disabled for the fire pattern.
         * The first cube in the list gets passed the pulseIndex that the destroy animation should trigger on, and is then removed.
         * This ensures that the cubes are destroyed in the order that they were originally selected.
         * 
         * TODO: A more complicated destroy pattern. Probably move this to a second script with another master pulse at 4X BPM to manually
         *       craft a destroy rhythm that isn't just on every beat and allows for more variety than just another subPulse.
         */
        if (firing && cubesToDestroy.Count != 0 && subPulseStepIndex == 0)
        {
            firePattern.SubscribedSteps[masterStepIndex] = false;
            firePattern.SubscribedSteps[(masterStepIndex + 1) % masterTotalSteps] = true;
            cubesToDestroy[0].hitIndex = (masterStepIndex + 1) % masterTotalSteps;
            cubesToDestroy[0].isFiring = true;
            cubesToDestroy.Remove(cubesToDestroy[0]);
        } else
        {
            if (cubesToDestroy.Count == 0)
            {
                firing = false;
            }
        }


    }

    // Use this for initialization
    void Awake () {
        hitCubes = new List<CubeThumper>();

        cubesToDestroy = new List<CubeThumper>();
    }
	
	// Update is called once per frame
	void Update () {

        Ray ray = Camera.main.ScreenPointToRay( Input.mousePosition );
        RaycastHit hit;
        if ( Physics.Raycast( ray, out hit, 1000.0f ) ) {
            GameObject obj = hit.transform.gameObject;
            if ( obj.CompareTag ("Thumper") ) {

                CubeThumper thump = obj.GetComponent<CubeThumper>();
                if ( !thump.IsTriggered() && hitCubes.Count < 8 && !cubesToDestroy.Contains(thump)){
                    thump.isTriggered = true;
                    hitCubes.Add(thump);
                    numCubesHit++;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && !firing) {
            firing = true;
            for (int i = 0; i < hitCubes.Count; i++)
            {
                cubesToDestroy.Add(hitCubes[i]);
            }
            hitCubes.Clear();

        }
	}
}
