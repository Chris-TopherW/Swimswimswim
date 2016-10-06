using UnityEngine;
using System.Collections;
using GAudio;

public class AudioManager : MonoBehaviour, IGATPulseClient 
{
	public PulseModule pulse;
	public GameObject patternModuleObjectLeft1;
	public GameObject patternModuleObjectRight1;
	public GameObject patternModuleObjectLeft2;
	public GameObject patternModuleObjectRight2;

	private PulsedPatternModule pulsedPatternLeft1;
	private PulsedPatternModule pulsedPatternRight1;
	private PulsedPatternModule pulsedPatternLeft2;
	private PulsedPatternModule pulsedPatternRight2;
	private SetLevels setLevels;
	private int loopNumber = 0;

	void Start() 
	{
		setLevels.CreateFade ("UIVolume", -80.0f, 1.0f);
	}

	//G-Audio management

	public void OnEnable()
	{
		pulse.SubscribeToPulse (this);
		pulsedPatternLeft1 = patternModuleObjectLeft1.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight1 = patternModuleObjectRight1.GetComponent<PulsedPatternModule> ();
		pulsedPatternLeft2 = patternModuleObjectLeft2.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight2 = patternModuleObjectRight2.GetComponent<PulsedPatternModule> ();
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
//				case 1:
//					removeSample (0);
//					pulsedPatternLeft2.AddSample ("FasterVapor" + "_0");
//					pulsedPatternRight2.AddSample ("FasterVapor" + "_1");
//					break;
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
		pulsedPatternLeft1.AddSample (fname + "_0");
		pulsedPatternRight1.AddSample (fname + "_1");
	}

	public void removeSample(int index){
		pulsedPatternLeft1.RemoveSampleAt (index);
		pulsedPatternRight1.RemoveSampleAt (index);
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
