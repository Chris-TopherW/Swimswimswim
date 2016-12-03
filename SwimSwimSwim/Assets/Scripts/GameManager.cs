using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int 						maxPollutionLevel;
	public static int 				currentPollutionLevel;
    public static float 			splinePos = 0;
    public static int 				segmentPos = 0;
	public static string 			gameState = "Normal";
	public float 					timeUntilBossFight, lengthOfBossFight;
	public GameObject 				audioManagerObject;
    private CurveImplementation 	meshGen;
	private float					startTime;
	private float 					timeSinceSceneStart;
	//private AudioManager 			audioManager;
	private bool 					endOfGame = false;

	void Start () {
		Cursor.visible = false;
		currentPollutionLevel = 0;
        meshGen = GameObject.FindGameObjectWithTag( "Spline" ).GetComponent<CurveImplementation>();
		//audioManager = audioManagerObject.GetComponent< AudioManager > ();
		startTime = Time.time;
		timeSinceSceneStart = 0.0f;
	}

	void Update () {
		timeSinceSceneStart = Time.time - startTime;
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
        }
//		if ( currentPollutionLevel >= maxPollutionLevel && !endOfGame) {
//			audioManager.EndOfGameAudio ();
//			endOfGame = true;
//			//SceneManager.LoadScene ("LoseScreen");
//			//Debug.Log("You Lose!!");
//		}
	}
}
