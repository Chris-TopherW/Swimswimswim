using UnityEngine;
using System.Collections;
using System;
using GAudio;

    public class CubeThumper : MonoBehaviour
    {
    public PulseModule pulse;
    public int hitIndex;
    public bool isTriggered, isFiring, hasFired;

        void OnEnable()
        {
        pulse = GameObject.Find("Sub Pulse").GetComponent<SubPulseModule>();
        pulse.onWillPulse += OnPulse;
        }
   
    void OnDisable()
        {
        pulse.onWillPulse -= OnPulse;
        }

    public bool IsTriggered()
    {
        return isTriggered;
    }


   private void OnPulse(IGATPulseInfo pulseInfo)
    {
        if (hasFired && pulseInfo.StepIndex == pulseInfo.NbOfSteps-1)
        {
            Destroy(gameObject);
        }
        if (isTriggered && !hasFired) {
            GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
        if (isFiring && pulseInfo.PulseSender.MasterPulseInfo.StepIndex == hitIndex)
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