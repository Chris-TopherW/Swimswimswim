using UnityEngine;
using System.Collections;
using GAudio;

public class AudioManager : MonoBehaviour, IGATPulseClient 
{
	public PulseModule pulse;
	public GameObject patterModuleObjectLeft;
	public GameObject patterModuleObjectRight;

	private PulsedPatternModule pulsedPatternLeft;
	private PulsedPatternModule pulsedPatternRight;
	private SetLevels setLevels;
	private int loopNumber, testNumber = 0;

	//G-Audio management

	public void OnEnable()
	{
		pulse.SubscribeToPulse (this);
		pulsedPatternLeft = patterModuleObjectLeft.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight = patterModuleObjectRight.GetComponent<PulsedPatternModule> ();
		setLevels = GetComponent<SetLevels> ();
	}

	public void OnDisable()
	{
		pulse.UnsubscribeToPulse (this);
	}

	//public void OnPulse(IGATPulseInfo pulseInfo){}

	public void OnPulse(IGATPulseInfo pulseInfo)
	{
	//	print (pulseInfo.StepIndex + "Pulse!");
		if (loopNumber < 10) {
			if (pulseInfo.StepIndex == 8) {

				switch (loopNumber) {
				case 1:
					changeLoop ("AMelodyLoopVapor");
					break;
				case 2:
					changeLoop("ALoopVapor");
					break;
				case 3:
					changeLoop("BreakdownVapor");
					break;
				case 4:
					changeLoop("BMelodyVapor");
					break;
				default:
					changeLoop("ALoopVapor");
					break;
				}
			}
			if (pulseInfo.StepIndex == 0) 
			{
				//removeSample (0);
			}
		}
		if (pulseInfo.StepIndex == 0)
			loopNumber++;
        if (loopNumber == 5) loopNumber = 0;
	}

	public void changeLoop (string fname)
	{
		pulsedPatternLeft.AddSample (fname + "_0");
		pulsedPatternRight.AddSample (fname + "_1");
	}

	public void removeSample(int index){
		pulsedPatternLeft.RemoveSampleAt (index);
		pulsedPatternRight.RemoveSampleAt (index);
	}


	public void PulseStepsDidChange(bool[] newSteps)
	{

	}

	//vertical mixing management
	public void TurnOnEffects()
	{
		setLevels.CreateFade("CrusherMix", 0.4f, 1.0f);
		setLevels.CreateFade ("DecimateMix", 0.1f, 1.0f);
		setLevels.CreateFade("LowPassFreq", 5000.0f, 1.0f);
	}

	public void TurnOffEffects()
	{
		setLevels.CreateFade("CrusherMix", 1.0f, 2.0f);
		setLevels.CreateFade ("DecimateMix", 1.0f, 2.0f);
		setLevels.CreateFade("LowPassFreq", 20000.0f, 2.0f);
	}

}
