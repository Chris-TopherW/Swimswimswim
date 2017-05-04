using UnityEngine;
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

    public ButtonAnimation gameButton;
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
        player.GetComponentInChildren<Spawner>().spawningToggle = true;
		backgroundLoop.GetComponent<BackgroundMusic>().Init(null);
        gamePlaying = true;
    }

    public void GameOver()
    {
        player.GetComponent<MoveCube>().enabled = false;
        player.GetComponentInChildren<Spawner>().spawningToggle = false;
        foreach (CubeThumper t in FindObjectsOfType<CubeThumper>())
        {
            Destroy(t.gameObject);
        }
        gamePlaying = false;
        gameScore = 0;
        currentPollutionLevel = 0;
        gameButton.Enable();
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
		if ( currentPollutionLevel >= maxPollutionLevel ) {
            GameOver();
		}
	    if (textPane != null && gamePlaying)
	    {
	        textPane.text = "Pollution: " + currentPollutionLevel + "\n Score: " + gameScore;
	    }
	    else
	    {
	        textPane.text = "";
	    }
	}
}
