using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int maxPollutionLevel;
	public static int currentPollutionLevel;
    public static float splinePos = 0;
    public static int segmentPos = 0;
    private CurveImplementation meshGen;

	void Start () {
		Cursor.visible = false;
		currentPollutionLevel = 0;
        meshGen = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
	}

	void Update () {
        if (splinePos >= 1)
        {
            splinePos = 0;
            segmentPos++;
            meshGen.Generate();
        }
		if (currentPollutionLevel >= maxPollutionLevel) {
			//SceneManager.LoadScene ("LoseScreen");
			//Debug.Log("You Lose!!");
		}
	}
}
