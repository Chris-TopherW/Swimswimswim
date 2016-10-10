using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

<<<<<<< HEAD
	public int 						maxPollutionLevel;
	public static int 				currentPollutionLevel;
    public static float 			splinePos = 0;
    public static int 				segmentPos = 0;
	public static string 			gameState = "Normal";
    private CurveImplementation 	meshGen;
	private float 					startTime, 
	private float 					timeSinceSceneStart;
=======
	public int maxPollutionLevel;
	public static int currentPollutionLevel;
    public static float splinePos = 0;
    public static int segmentPos = 0;
    private CurveImplementation meshGen;
>>>>>>> parent of a70ff34... game state logic

	void Start () {
		Cursor.visible = false;
		currentPollutionLevel = 0;
<<<<<<< HEAD
        meshGen = GameObject.FindGameObjectWithTag( "Spline" ).GetComponent<CurveImplementation>();
		startTime = Time.time;
		timeSinceSceneStart = 0.0f;
	}

	void Update () {
		timeSinceSceneStart = Time.time - startTime;
		if ( timeSinceSceneStart >= 10.0f && gameState == "Normal" ) {
			gameState = "BossFight";
			Debug.Log ( "Boss fight!" );
		}
        if ( splinePos >= 1 ){
=======
        meshGen = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
	}

	void Update () {
        if (splinePos >= 1)
        {
>>>>>>> parent of a70ff34... game state logic
            splinePos = 0;
            segmentPos++;
            meshGen.Generate();
        }
		if ( currentPollutionLevel >= maxPollutionLevel ) {
			//SceneManager.LoadScene ("LoseScreen");
			//Debug.Log("You Lose!!");
		}
	}
}
