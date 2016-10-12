//Chris Wratt 2016
//Audio fader control

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SetLevels : MonoBehaviour {

	public AudioMixer 					masterMixer;
	private Dictionary<string, bool> 	changingBools;

	void Awake() {
		changingBools = new Dictionary<string, bool>();
		changingBools.Add( "CrusherMix", false );
		changingBools.Add( "SmasherMix", false );
		changingBools.Add( "DecimateMix", false );
		changingBools.Add( "DECIMATION", false );
		changingBools.Add( "LowPassFreq", false );
		changingBools.Add( "UIVolume", false );
		changingBools.Add( "Smasher", false );
	}

	public void CreateFade( string loopName, float endValue, float length ) {
		if ( changingBools.ContainsKey ( loopName ) ) {
			StartCoroutine ( VolumeFade ( loopName, endValue, length ) );
		} else {
			Debug.Log ( "Invalid loop name provided" );
		}
	}

	private IEnumerator VolumeFade( string loopName, float endValue, float length ) {

		float 			fadeStart = Time.time;
		float 			timeSinceStart = 0.0f;
		float 			myVolume;
		float 			startValue;

		masterMixer.GetFloat( loopName, out startValue );
		changingBools[loopName] = false;
		yield return new WaitForSeconds ( 0.05f );
		changingBools[loopName] = true;
		while( timeSinceStart < length ){
			timeSinceStart = Mathf.Abs ( Time.time - fadeStart );
			if ( changingBools[loopName] ) {
				yield return new WaitForSeconds ( 0.05f );
			} else {
				yield break;
			}
			myVolume = Mathf.Lerp ( startValue, endValue, timeSinceStart / length );
			masterMixer.SetFloat( loopName, myVolume );
		}
		yield break;
	}
}
