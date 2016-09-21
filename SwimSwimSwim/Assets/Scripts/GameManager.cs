using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public int maxPollutionLevel;
	public static int currentPollutionLevel;

	void Start () {
		Cursor.visible = false;
		currentPollutionLevel = 0;
	}

	void Update () {
		if (currentPollutionLevel >= maxPollutionLevel) {
			//SceneManager.LoadScene ("LoseScreen");
			Debug.Log("You Lose!!");
		}
	}
}
