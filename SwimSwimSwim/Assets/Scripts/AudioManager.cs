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
					pulsedPatternLeft.AddSample ("AMelodyLoopVapor_0");
					pulsedPatternRight.AddSample ("AMelodyLoopVapor_1");
					pulsedPatternLeft.RemoveSampleAt (0);
					pulsedPatternRight.RemoveSampleAt (0);
					break;
				case 2:
					pulsedPatternLeft.AddSample ("ALoopVapor_0");
					pulsedPatternRight.AddSample ("ALoopVapor_1");
					pulsedPatternLeft.RemoveSampleAt (0);
					pulsedPatternRight.RemoveSampleAt (0);
					break;
				case 3:
					pulsedPatternLeft.AddSample ("BreakdownVapor_0");
					pulsedPatternRight.AddSample ("BreakdownVapor_1");
					pulsedPatternLeft.RemoveSampleAt (0);
					pulsedPatternRight.RemoveSampleAt (0);
					break;
				case 4:
					pulsedPatternLeft.AddSample ("BMelodyVapor_0");
					pulsedPatternRight.AddSample ("BMelodyVapor_1");
					pulsedPatternLeft.RemoveSampleAt (0);
					pulsedPatternRight.RemoveSampleAt (0);
					break;
				}
			}
		}
		if (pulseInfo.StepIndex == 0)
			loopNumber++;

	}

	public void PulseStepsDidChange(bool[] newSteps)
	{

	}

}
