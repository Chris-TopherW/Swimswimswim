using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

	public AudioMixer audioMixer;

	private Vector3 dolphinPosition;

	// Use this for initialization
	void Start () {
		dolphinPosition = new Vector3(0,0,0);
		dolphinPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{

			dolphinPosition.z += 2.5f * Time.deltaTime;
			transform.position = dolphinPosition;

		if (Input.GetKey (KeyCode.A)) 
		{
			dolphinPosition.x -= 1.5f * Time.deltaTime;
			transform.position = dolphinPosition;
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			dolphinPosition.x += 1.5f * Time.deltaTime;
			transform.position = dolphinPosition;
		}
		if (Input.GetKey (KeyCode.S)) 
		{
			dolphinPosition.y -= 1.5f * Time.deltaTime;
			transform.position = dolphinPosition;
		}
		if (Input.GetKey (KeyCode.W)) 
		{
			dolphinPosition.y += 1.5f * Time.deltaTime;
			transform.position = dolphinPosition;
		}

		if (dolphinPosition.y < -2.5f) {
			audioMixer.SetFloat ("CrusherMix", 0.4f);
            audioMixer.SetFloat("DECIMATION", Random.Range(1,100));
            audioMixer.SetFloat ("DecimateMix", 0.1f);
			audioMixer.SetFloat ("LowPassFreq", 5000.0f);
		} else {
            audioMixer.SetFloat("DECIMATION", 1);
            audioMixer.SetFloat ("CrusherMix", 1.0f);
			audioMixer.SetFloat ("DecimateMix", 1.0f);
			audioMixer.SetFloat ("LowPassFreq", 22000.0f);
		}

	}
}
