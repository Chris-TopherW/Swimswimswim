using UnityEngine;
using System.Collections;
using System;
using GAudio;

    public class CubeThumper : MonoBehaviour
    {
    public PulseModule pulse;
    public PulseModule firePulse;
    public int hitIndex;
    public bool isTriggered, isFiring, hasFired;

        void OnEnable()
        {
        pulse = GameObject.Find("Sub Pulse").GetComponent<SubPulseModule>();
        firePulse = GameObject.Find("FirePulse").GetComponent<SubPulseModule>();
        pulse.onWillPulse += OnTriggerPulse;
        firePulse.onWillPulse += OnFirePulse;
        }
   
    void OnDisable()
        {
        pulse.onWillPulse -= OnTriggerPulse;
        firePulse.onWillPulse -= OnFirePulse;
    }

    public bool IsTriggered()
    {
        return isTriggered;
    }


   private void OnTriggerPulse(IGATPulseInfo pulseInfo)
    {
        if (isTriggered && !hasFired) {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        if (hasFired && pulseInfo.StepIndex == pulseInfo.NbOfSteps - 1)
        {
            Destroy(gameObject);
        }
        if (isFiring && pulseInfo.PulseSender.MasterPulseInfo.StepIndex == hitIndex)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            isFiring = false;
            hasFired = true; //TODO: This is gross -- change to enums.
        }
    }

    private void OnFirePulse(IGATPulseInfo pulseInfo)
    {
        if (hasFired && pulseInfo.StepIndex == (hitIndex + 1) % pulseInfo.NbOfSteps)
        {
            Destroy(gameObject);
        }
        if (isFiring && pulseInfo.StepIndex == hitIndex)
        {
            GetComponent<Renderer>().material.SetColor("_Color", Color.green);
            isFiring = false;
            hasFired = true; //TODO: This is gross -- change to enums.
        }
    }



    // Use this for initialization
    void Start()
        {
        }
        // Update is called once per frame
        void Update()
        {

        }
    }