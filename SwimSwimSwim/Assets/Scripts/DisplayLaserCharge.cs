using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayLaserCharge : MonoBehaviour {

	private Text		myText;
	private int 		pollutionAsInt;

	void Start () {
		myText = GetComponent<Text> ();
	}
		
	void Update () {
		pollutionAsInt = ( int )LaserControl.laserCharge;
		myText.text = "Laser charge = " + pollutionAsInt.ToString();
		if (LaserControl.laserCharge < LaserControl.laserChargeThreshold) {
			myText.color = Color.red;
		} else {
			myText.color = Color.black;
		}
	}
}
