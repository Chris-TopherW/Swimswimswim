using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Begin, Playing, Paused
}

public class BlastManager : Singleton<BlastManager>
{

    public GameState state = GameState.Begin;

    public int maxPollutionLevel;
    public static int currentPollutionLevel;
    public static int gameScore;
    
    public GameObject player;
    private float startTime;
    private float timeSinceSceneStart;
    public Text textPane;

    public ButtonAnimation gameButton;
    public GameObject backgroundLoop;

    public delegate void OnStateChangeDelegate();
    public static OnStateChangeDelegate stateChangeDelegate;

    protected BlastManager() { }


    // Use this for initialization
    void Start () {
		
	}

    public void BeginGame()
    {
       // player.GetComponent<MoveCube>().enabled = true;
        //player.GetComponentInChildren<Spawner>().spawningToggle = true;
        backgroundLoop.GetComponent<Sequence>().BeginPlay();
        ChangeState(GameState.Playing);
    }

    void ChangeState(GameState newState)
    {
        state = newState;
        stateChangeDelegate();
    }
    
    public void PauseGame()
    {
        //Time.timeScale = 0; Will need to be smarter than this.
        //TODO: Manage pausing/resuming metro and audio and gameplay in sync :D

        ChangeState(GameState.Paused);

    }

    public void ResumeGame()
    {
       // Time.timeScale = 1;
        ChangeState(GameState.Playing);

    }

    // Update is called once per frame
    void Update () {
		
	}
}
