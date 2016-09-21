using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayPollution : MonoBehaviour {

	private Text myText;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		myText.text = "Current pollution = " + GameManager.currentPollutionLevel.ToString();
	}
}
