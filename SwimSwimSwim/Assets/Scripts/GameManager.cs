﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int 						maxPollutionLevel;
	public static int 				currentPollutionLevel;
    public static int gameScore;
    public static float 			splinePos = 0;
    public static int 				segmentPos = 0;
	public static string 			gameState = "Normal";
	public float 					timeUntilBossFight, lengthOfBossFight;
    public GameObject player;
    private CurveImplementation 	meshGen;
	private float					startTime;
	private float 					timeSinceSceneStart;
	//private AudioManager 			audioManager;
	private bool 					endOfGame = false;
    private bool gamePlaying;
    public Text textPane;
    public GameObject backgroundLoop;

    //TODO -- Massive cleanup

	void Start () {
        player = GameObject.FindWithTag("Player");
		//Cursor.visible = false;
		currentPollutionLevel = 0;
		//audioManager = audioManagerObject.GetComponent< AudioManager > ();
		startTime = Time.time;
		timeSinceSceneStart = 0.0f;
	    gamePlaying = false;

	}

    public void ClickBeginGame()
    {

        player.GetComponent<MoveCube>().enabled = true;
        player.GetComponentInChildren<Spawner>().enabled = true;
        backgroundLoop.GetComponent<Sequence>().enabled = true;
        gamePlaying = true;
    }

	void Update () {
		/*timeSinceSceneStart = Time.time - startTime;
		if ( timeSinceSceneStart >= timeUntilBossFight && timeSinceSceneStart <= ( timeUntilBossFight + lengthOfBossFight ) && gameState == "Normal" ) {
			gameState = "BossFight";
			Debug.Log ( "Boss fight!" );
		}
		if ( timeSinceSceneStart >= ( timeUntilBossFight + lengthOfBossFight ) && gameState == "BossFight" ) {
			gameState = "Normal";
			Debug.Log ( "Back to Normal" );
		}
        if ( splinePos >= 1 ){
            splinePos = 0;
            segmentPos++;
            meshGen.Generate();
        }*/
//		if ( currentPollutionLevel >= maxPollutionLevel && !endOfGame) {
//			audioManager.EndOfGameAudio ();
//			endOfGame = true;
//			//SceneManager.LoadScene ("LoseScreen");
//			//Debug.Log("You Lose!!");
//		}
	    if (textPane != null && gamePlaying)
	    {
	        textPane.text = "Pollution: " + currentPollutionLevel + "\n Score: " + gameScore;
	    }
	}
}
