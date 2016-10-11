using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayPollution : MonoBehaviour {

	private Text		 myText;

	void Start () {
		myText = GetComponent<Text> ();
	}

	void Update () {
		myText.text = "Current pollution = " + GameManager.currentPollutionLevel.ToString();
	}
}
