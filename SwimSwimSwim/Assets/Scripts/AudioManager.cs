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

	void Start() {
		pulse.Period = 60.0f / 81.0f;
		//addSample ("VaporVaseA");
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

//		if (pulseInfo.StepIndex == 6) {
//		}
	}

	public void addSample ( string fname ) {
		pulsedPatternLeft.AddSample ( fname + "_0" );
		pulsedPatternRight.AddSample ( fname + "_1" );
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
