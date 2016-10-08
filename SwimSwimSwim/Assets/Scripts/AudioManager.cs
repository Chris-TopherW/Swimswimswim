using UnityEngine;
using System.Collections;
using GAudio;

public class AudioManager : MonoBehaviour, IGATPulseClient 
{
	public MasterPulseModule pulse;
	public GameObject patternModuleObjectLeft;
	public GameObject patternModuleObjectRight;

	private PulsedPatternModule pulsedPatternLeft;
	private PulsedPatternModule pulsedPatternRight;

	private SetLevels setLevels;
	private int loopNumber = 0;

	void Start() 
	{
		setLevels.CreateFade ("UIVolume", -80.0f, 1.0f);
		pulse.StartPulsing (0);
		//set BPM
		pulse.Period = 60.0f / 81.0f;
	}

	//G-Audio management

	public void OnEnable()
	{
		pulse.SubscribeToPulse (this);
		pulsedPatternLeft = patternModuleObjectLeft.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight = patternModuleObjectRight.GetComponent<PulsedPatternModule> ();

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
			if (pulseInfo.StepIndex == 6) {

				switch (loopNumber) {
				case 0:
					changeLoop ("VaporVaseA");
					break;
//				case 1:
//					changeLoop ("VaporVaseA");
//					break;
//				case 2:
//					changeLoop ("VaporVaseAMelody1");
//					break;
//				case 3:
//					changeLoop ("VaporVaseAMelody2");
//					break;
//				case 4:
//					changeLoop ("VaporVaseA");
//					break;
//				case 5:
//					changeLoop ("VaporVaseA");
//					break;
				case 1:
					changeLoop ("VaporVaseTransition");
					break;
				case 2:
					changeLoop ("FastVapeA1");
					break;
				case 3:
					changeLoop ("FastVapeA2");
					break;
				case 4:
					changeLoop ("VaporVaseA");
					break;
				default:
					changeLoop ("VaporVaseA");
					break;
				}
			}
			if (pulseInfo.StepIndex == 0 && loopNumber == 1) {
				pulse.Period = 60.0f / 76.0f;
				Debug.Log ("Tempo shift to 76 bpm");
			} if(pulseInfo.StepIndex == 0 && loopNumber == 4) {
				pulse.Period = 60.0f / 81.0f;
				Debug.Log ("Tempo shift to 81 bpm");
			}
				
			if (pulseInfo.StepIndex == 0) {
				loopNumber++;
			}
			if (loopNumber == 5)
				loopNumber = 0;
		}
	}

	public void changeLoop (string fname)
	{
		pulsedPatternLeft.AddSample (fname + "_0");
		pulsedPatternRight.AddSample (fname + "_1");
		removeSample (0);
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
