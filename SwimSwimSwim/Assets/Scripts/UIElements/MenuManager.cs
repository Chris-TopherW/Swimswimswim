using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    public void loadGame() {
        SceneManager.LoadScene(1);
    }
    public void loadOptions() {

    }
    public void loadHighScore() {

    }
    public void loadCredits() {

    }
    public void loadExit() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
