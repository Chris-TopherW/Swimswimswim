using UnityEngine;
using System.Collections;
using GAudio;

public class AudioManager : MonoBehaviour, IGATPulseClient {
	
	public MasterPulseModule 		pulse;
	public GameObject 				patternModuleObjectLeft;
	public GameObject 				patternModuleObjectRight;
	public static string 			musicState;
	private PulsedPatternModule 	pulsedPatternLeft;
	private PulsedPatternModule 	pulsedPatternRight;
	private SetLevels 				setLevels;

	void Start() {
		pulse.Period = 60.0f / 81.0f;
		musicState = "VaporVase";
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
		if (pulseInfo.StepIndex == 0 && pulsedPatternLeft.Samples [0].SampleName == "VaporVaseTransition_0") {
			pulse.Period = 60.0f / 76.0f;
			musicState = "FastVape";
		}
		if ( GameManager.gameState == "BossFight" && pulsedPatternLeft.Samples[ 0 ].SampleName == "VaporVaseA_0" ) {
			addSample ("VaporVaseTransition");
			addSample ("FastVapeA1");
			removeSample (0);
		}
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
		
	public void UnderwaterEffectsOn() {
		setLevels.CreateFade( "CrusherMix", 0.4f, 1.0f );
		setLevels.CreateFade( "SmasherMix", 0.1f, 1.0f );
		setLevels.CreateFade ( "DecimateMix", 0.1f, 1.0f );
		setLevels.CreateFade( "LowPassFreq", 3000.0f, 1.0f );
		setLevels.CreateFade ( "DECIMATION", Random.Range( 2.0f, 100.0f ), 1.0f );
	}

	public void UnderwaterEffectsOff() {
		setLevels.CreateFade( "CrusherMix", 1.0f, 2.0f );
		setLevels.CreateFade( "SmasherMix", 1.0f, 1.0f );
		setLevels.CreateFade ( "DecimateMix", 1.0f, 2.0f );
		setLevels.CreateFade( "LowPassFreq", 20000.0f, 2.0f );
		setLevels.CreateFade ( "DECIMATION", 2.0f, 1.0f );
	}

	public void DamageEffectOn() {
		StartCoroutine ( DamageEffect() );
	}

	private IEnumerator DamageEffect() {
		setLevels.CreateFade ( "CrusherMix", 0.4f, 0.2f );
		yield return new WaitForSeconds (0.2f);
		setLevels.CreateFade ( "CrusherMix", 1.0f, 2.0f );
		yield break;
	}

	public void EndOfGameAudio(){
		setLevels.CreateFade ("Smasher", 1250.0f, 15.0f);
	}
}
