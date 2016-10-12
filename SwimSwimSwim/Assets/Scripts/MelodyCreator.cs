using UnityEngine;
using System.Collections;
using GAudio;

public class MelodyCreator : MonoBehaviour, IGATPulseClient {

	public PulseModule 				pulse;
	public GameObject 				patternModuleObject;
	public int[] 					sampleWeighting;
	public bool 					samplePlayed = false;
	private PulsedPatternModule 	pulsedPattern;
	private string[] 				notes;
	private string[] 				sampleNames;

	void Start() {
		sampleNames = new string[ pulsedPattern.SampleBank.AllSampleNames.Length ];
		notes = new string[ sampleNames.Length ];
		for ( int i = 0; i < sampleNames.Length; i++ ) {
			sampleNames[ i ] = pulsedPattern.SampleBank.AllSampleNames[ i ];
			notes[ i ] = sampleNames[ i ].Substring( 0, 2 );
		}
	}

	public void OnEnable() {
		pulse.SubscribeToPulse ( this );
		pulsedPattern = patternModuleObject.GetComponent< PulsedPatternModule > ();
	}

	public void OnDisable() {
		pulse.UnsubscribeToPulse ( this );
	}

	public void OnPulse( IGATPulseInfo pulseInfo ) {
		if (samplePlayed ) {
			pulsedPattern.RemoveSampleAt ( 0 );
			samplePlayed = false;
		}
		if (pulsedPattern.Samples.Length >= 1) {
			samplePlayed = true;
		}
	}

	public void PulseStepsDidChange( bool[] newSteps ) {
	}

	public void PlayRandomNote(){
		pulsedPattern.AddSample ( sampleNames [ WeightedRandom (sampleWeighting) ] );
	}

	private int WeightedRandom(int[] weights)
	{
		int sum = 0;
		int randomisedSum = 0;
		for ( int i = 0; i < weights.Length; i++ ) {
			sum += weights[i];
		}
		randomisedSum = Random.Range (1, sum + 1);
		sum = 0;
		for ( int i = 0; i < weights.Length; i++ ) {
			sum += weights [ i ];
			if ( sum >= randomisedSum )
				return i;
		}
		//error!
		return -1;
	}
}
