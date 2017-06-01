using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject mainMenuButtons;
    public GameObject optionsButtons;
    public GameObject creditsView;
    public GameObject highScoreView;

	// Use this for initialization
	void Start () {

    }
	
    public void loadGame() {
        SceneManager.LoadScene(1);
    }
    public void loadOptions() {
        mainMenuButtons.SetActive(false);
        optionsButtons.SetActive(true);
    }
    public void loadHighScore() {
        mainMenuButtons.SetActive(false);
        highScoreView.SetActive(true);
    }
    public void loadCredits() {
        mainMenuButtons.SetActive(false);
        creditsView.SetActive(true);
    }
    public void loadExit() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void loadMainMenu()
    {
        mainMenuButtons.SetActive(true);
        optionsButtons.SetActive(false);
        creditsView.SetActive(false);
        highScoreView.SetActive(false);
    }

}
