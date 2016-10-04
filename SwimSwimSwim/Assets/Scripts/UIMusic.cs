﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class UIMusic : MonoBehaviour {

	public GameObject[] samples; //0 and 1 are A and D bass, then odds are A melody, evens are D melody
	public int[] weightedValues;

	private SetLevels setLevels;

	void Start()
	{
		setLevels = GetComponent<SetLevels> ();
		StartCoroutine (playSample (0));
		StartCoroutine (playSample (2 + (Random.Range(0,3)*2)));
		//Debug.Log(WeightedRandom (weightedValues));
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.W))
			TurnOnEffects ();
		if (Input.GetKeyDown (KeyCode.S))
			TurnOffEffects ();
		//Debug.Log(WeightedRandom (weightedValues));
	}

	private IEnumerator playSample(int index)
	{
		Instantiate (samples [index], transform.position, transform.rotation);
		//Instantiate (bassNotes [index], transform.position, transform.rotation);
		yield return new WaitForSeconds (samples[index].GetComponent<AudioSource>().clip.length - 2.0f);
		if (index == 0) 
		{
			StartCoroutine (playSample (1));
			StartCoroutine (playSample (1 + (Random.Range(0,3)*2)));
		} 
		else if (index == 1) 
		{
			StartCoroutine (playSample (0));
			StartCoroutine (playSample (2 + (Random.Range(0,3) *2)));
		}
		
		yield break;
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

	private int WeightedRandom(int[] weights)
	{
		int sum = 0;
		int randomisedSum = 0;
		for (int i = 0; i < weights.Length; i++) 
		{
			sum += weights[i];
		}
		randomisedSum = Random.Range (0, sum + 1);
		sum = 0;
		for (int i = 0; i < weights.Length; i++) 
		{
			sum += weights [i];
			if (sum >= randomisedSum)
				return i;
		}
		//error!
		return -1;
	}
}