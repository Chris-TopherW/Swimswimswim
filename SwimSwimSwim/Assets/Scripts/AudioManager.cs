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
	private int loopNumber = 0;

	public void OnEnable()
	{
		pulse.SubscribeToPulse (this);
		pulsedPatternLeft = patterModuleObjectLeft.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight = patterModuleObjectRight.GetComponent<PulsedPatternModule> ();
	}

	public void OnDisable()
	{
		pulse.UnsubscribeToPulse (this);
	}

	public void OnPulse(IGATPulseInfo pulseInfo)
	{
		print (pulseInfo.StepIndex + "Pulse!");

		if (loopNumber < 10) {
			if (pulseInfo.StepIndex == 0) {

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
		}
		if (pulseInfo.StepIndex == 0)
			loopNumber++;
	}

	public void changeLoop (string fname)
	{
		pulsedPatternLeft.AddSample (fname + "_0");
		pulsedPatternRight.AddSample (fname + "_1");
		pulsedPatternLeft.RemoveSampleAt (0);
		pulsedPatternRight.RemoveSampleAt (0);
	}


	public void PulseStepsDidChange(bool[] newSteps)
	{

	}

}
