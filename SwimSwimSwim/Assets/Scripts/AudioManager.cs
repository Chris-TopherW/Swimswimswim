using UnityEngine;
using System.Collections;
using GAudio;

public class AudioManager : MonoBehaviour, IGATPulseClient {
	
	public MasterPulseModule 		pulse;
	public GameObject 				patternModuleObjectLeft;
	public GameObject 				patternModuleObjectRight;
	private PulsedPatternModule 	pulsedPatternLeft;
	private PulsedPatternModule 	pulsedPatternRight;
	private SetLevels 				setLevels;
	private int 					loopNumber = 0;

	void Start() {
		pulse.Period = 60.0f / 81.0f;
	}

	void Update() {
		if ( Time.time > 2.0f && !pulse.IsPulsing ) {
			pulse.StartPulsing ( 0 );
		}
		setLevels.CreateFade ( "UIVolume", -80.0f, 15.0f );
	}

	public void OnEnable() {
		pulse.SubscribeToPulse ( this );
		pulsedPatternLeft = patternModuleObjectLeft.GetComponent<PulsedPatternModule> ();
		pulsedPatternRight = patternModuleObjectRight.GetComponent<PulsedPatternModule> ();
		setLevels = GetComponent<SetLevels> ();
	}

	public void OnDisable(){
		pulse.UnsubscribeToPulse ( this );
	}

	public void OnPulse( IGATPulseInfo pulseInfo ) {
		if ( loopNumber < 10 ) {
			if ( pulseInfo.StepIndex == 6 ) {
				switch ( loopNumber ) {
				case 0:
					changeLoop ( "DroneTransition" );
					break;
				case 1:
					changeLoop ( "VaporVaseA" );
					break;
				case 2:
					changeLoop ( "VaporVaseTransition" );
					break;
				case 3:
					changeLoop ( "FastVapeA1" );
					break;
				case 4:
					changeLoop ( "FastVapeA2" );
					break;
				case 5:
					changeLoop ( "FastVapeTransition" );
					break;
				default:
					changeLoop ( "VaporVaseA" );
					break;
				}
			}
			if ( pulseInfo.StepIndex == 0 && loopNumber == 2 ) {
				pulse.Period = 60.0f / 76.0f;
				Debug.Log ("Tempo shift to 76 bpm");
			}
			if( pulseInfo.StepIndex == 0 && loopNumber == 5 ) {
				pulse.Period = 60.0f / 81.0f;
				Debug.Log ("Tempo shift to 81 bpm");
			}
				
			if ( pulseInfo.StepIndex == 0 ) {
				loopNumber++;
			}
			if ( loopNumber == 6 )
				loopNumber = 1;
		}
	}

	public void changeLoop ( string fname ) {
		pulsedPatternLeft.AddSample ( fname + "_0" );
		pulsedPatternRight.AddSample ( fname + "_1" );
		removeSample (0);
	}

	public void removeSample( int index ) {
		pulsedPatternLeft.RemoveSampleAt ( index );
		pulsedPatternRight.RemoveSampleAt ( index );
	}

	public void PulseStepsDidChange( bool[] newSteps ) {
	}
		
	public void TurnOnEffects() {
		setLevels.CreateFade( "CrusherMix", 0.4f, 1.0f );
		setLevels.CreateFade( "SmasherMix", 0.1f, 1.0f );
		setLevels.CreateFade ( "DecimateMix", 0.1f, 1.0f );
		setLevels.CreateFade( "LowPassFreq", 3000.0f, 1.0f );
	}

	public void TurnOffEffects() {
		setLevels.CreateFade( "CrusherMix", 1.0f, 2.0f );
		setLevels.CreateFade( "SmasherMix", 1.0f, 1.0f );
		setLevels.CreateFade ( "DecimateMix", 1.0f, 2.0f );
		setLevels.CreateFade( "LowPassFreq", 20000.0f, 2.0f );
	}
}
