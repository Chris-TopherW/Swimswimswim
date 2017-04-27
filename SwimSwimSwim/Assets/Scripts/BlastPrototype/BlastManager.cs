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

    private List<BlastZone> blastZones;

    private BlastController controller;

    public bool tickLock;


    public GameObject toSpawn;

    protected BlastManager() { }


    // Use this for initialization
    void Start () {
        blastZones = new List<BlastZone>();
        controller = GetComponent<BlastController>();
        Metronome.quarterChangeDelegate += HandleTickChange;
		
	}

    public void BeginGame()
    {
        backgroundLoop.GetComponent<Sequence>().BeginPlay();
        //Delay before activating game controller
        StartCoroutine(StartController(1f)); 
        ChangeState(GameState.Playing);
    }

    public void HandleTickChange(NotationTime currentTime)
    {
        tickLock = false;
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
        controller.Deactivate();
        ChangeState(GameState.Paused);

    }

    public void HandleBlastZoneInput(Vector2 touchPosition)
    {
        if (!tickLock)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
            Vector3 spawnLocation = ray.origin + (ray.direction * ray.origin.y);
            GameObject zoneMade = GameObject.Instantiate(toSpawn, spawnLocation, Quaternion.identity);
            zoneMade.GetComponent<BlastZone>().CreateZone(Metronome.Instance.currentTime);
            tickLock = true;
        }
    }
    

    public void ResumeGame()
    {
       // Time.timeScale = 1;
        ChangeState(GameState.Playing);
        StartCoroutine(StartController(1f));

    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator StartController(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        controller.Activate();
    }
}
