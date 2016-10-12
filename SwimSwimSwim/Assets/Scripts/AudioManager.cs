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
	private int 					bossFightLoopIterator = 0;
	private int 					normalLoopIterator = 0;

	void Start() {
		pulse.Period = 60.0f / 81.0f;
		musicState = "VaporVase";
	}

	void Update() {
		if ( Time.time > 2.0f && !pulse.IsPulsing ) {
			pulse.StartPulsing ( 0 );
			setLevels.CreateFade ( "UIVolume", -80.0f, 13.0f );
		}
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
		//BPM changes
		if ( pulseInfo.StepIndex == 0 && pulsedPatternLeft.Samples [ 0 ].SampleName == "VaporVaseTransition_0" ) {
			pulse.Period = 60.0f / 76.0f;
			musicState = "FastVape";
			bossFightLoopIterator = 0;
		}
		if ( pulseInfo.StepIndex == 0 && pulsedPatternLeft.Samples [ 0 ].SampleName == "FastVapeTransition_0" ) {
			pulse.Period = 60.0f / 81.0f;
			musicState = "VaporVase";
			normalLoopIterator = 0;
		}
		//transition sections
		if ( GameManager.gameState == "BossFight" && musicState == "VaporVase" ) {
			removeSample (0);
			addSample ( "VaporVaseTransition" );
		}
		if ( GameManager.gameState == "Normal" && musicState == "FastVape" ) {
			removeSample (0);
			addSample ( "FastVapeTransition" );
		}
		if( pulseInfo.StepIndex == 6 && musicState == "FastVape" ) {
			PlayBossFight();
		}
		if( pulseInfo.StepIndex == 6 && musicState == "VaporVase" ) {
			PlayVaporVase();
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

	public void PlayVaporVase() {
		switch ( normalLoopIterator ) {
		case 0:
			removeSample(0);
			addSample( "VaporVaseA" );
			break;
		case 1:
			removeSample(0);
			addSample( "VaporVaseA" );
			break;
		case 2:
			removeSample(0);
			addSample( "VaporVaseAMelody1" );
			break;
		case 3:
			removeSample(0);
			addSample( "VaporVaseAMelody2" );
			break;
		case 4:
			removeSample(0);
			addSample( "VaporVaseAMelody1" );
			break;
		case 5:
			removeSample(0);
			addSample( "VaporVaseAMelody2" );
			break;
		case 6:
			removeSample(0);
			addSample( "VaporVaseA" );
			break;
		case 7:
			removeSample(0);
			addSample( "VaporVaseA" );
			break;
		case 8:
			removeSample(0);
			addSample( "VaporVaseBridge1" );
			break;
		case 9:
			removeSample(0);
			addSample( "VaporVaseBridge2" );
			break;
		case 10:
			removeSample(0);
			addSample( "VaporVaseBridge1" );
			break;
		case 11:
			removeSample(0);
			addSample( "VaporVaseBridge2" );
			break;
		default:
			print("Error, VaporVase loop " + normalLoopIterator + " does not exist");
			break;
		}
		normalLoopIterator ++;
		if( normalLoopIterator == 12 ) {
			normalLoopIterator = 0;
		}
	}

	public void PlayBossFight() {
		switch ( bossFightLoopIterator ) {
		case 0:
			removeSample(0);
			addSample( "FastVapeA1" );
			break;
		case 1:
			removeSample(0);
			addSample( "FastVapeA2" );
			break;
		default:
			print("Error, FastVape loop " + bossFightLoopIterator + " does not exist");
			break;
		}
		bossFightLoopIterator ++;
		if( bossFightLoopIterator == 2 ) {
			bossFightLoopIterator = 0;
		}
	}
}
