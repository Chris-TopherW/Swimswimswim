//Chris Wratt 2016
//Audio fader control

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SetLevels : MonoBehaviour {

	// This is access to whole mixer
	public AudioMixer masterMixer;

	private Dictionary<string, bool> changingBools;

	void Awake() {
		changingBools = new Dictionary<string, bool>();
		changingBools.Add("CrusherMix", false);
		changingBools.Add("DecimateMix", false);
		changingBools.Add("DECIMATION", false);
		changingBools.Add("LowPassFreq", false);
		changingBools.Add ("UIVolume", false);
	}

	public void CreateFade(string loopName, float endValue, float length) {
		if (changingBools.ContainsKey (loopName)) {
			StartCoroutine (VolumeFade (loopName, endValue, length));
		} else {
			Debug.Log ("Invalid loop name provided");
		}
	}

	//Fade in or out function. This creates and destroys new routines...
	private IEnumerator VolumeFade(string loopName, float endValue, float length) {

		float fadeStart = Time.time;
		float timeSinceStart = 0.0f;
		float myVolume;
		float startValue;

		//set starting value to current fader position
		masterMixer.GetFloat(loopName, out startValue);

		//this will cancel any fades occuring on this loop
		changingBools[loopName] = false;

		//delay
		yield return new WaitForSeconds (0.05f);

		changingBools[loopName] = true;

		//lerp fade loop with delay
		while(timeSinceStart < length){
			timeSinceStart = Mathf.Abs (Time.time - fadeStart);

			//this checks to see if a new fade has been called
			if (changingBools[loopName]) {
				yield return new WaitForSeconds (0.05f);
			} else {
				yield break;
			}

			myVolume = Mathf.Lerp (startValue, endValue, timeSinceStart / length);
			masterMixer.SetFloat(loopName, myVolume);

		}
		yield break;
	}
}