using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class displayMetro : MonoBehaviour {

    private Metronome metro;
    private Text text;

	// Use this for initialization
	void Start () {
        metro = GameObject.FindGameObjectWithTag("Metronome").GetComponent<Metronome>();
        text = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        text.text = string.Format("Current Bar: {0} \n Current Quarter: {1} \n Current Tick: {2} \n Last Tick Time: {3}", metro.currentBar, metro.currentQuarter, metro.currentTick, metro.lastTickTime);
	}
}
