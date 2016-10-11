using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayLaserCharge : MonoBehaviour {

	private Text		 myText;

	// Use this for initialization
	void Start () {
		myText = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		myText.text = "Laser charge = " + LaserControl.laserCharge.ToString();
	}
}
