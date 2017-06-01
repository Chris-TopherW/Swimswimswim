using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Begin, Playing, Paused, GameOver
}

public class BlastManager : Singleton<BlastManager>
{

    public const int MAX_FIREPOWER = 24;
    public const int MAX_ZONES = 4;

    public int ticksFired = 0;
    public int zoneCount = 0;

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

    public bool tickLock = true;
    private NotationTime tickUnlockTime;


    public GameObject toSpawn;
    public GameObject blastZone;

    private Transform spawnPoint;
    private Transform blastZoneParent;

    public Text debugScoreDisplay;

    protected BlastManager() { }

    // Use this for initialization
    void Start () {
        blastZones = new List<BlastZone>();
        controller = GetComponent<BlastController>();
        UpdateText();
        spawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        blastZoneParent = GameObject.Find("BlastZones").GetComponent<Transform>();
    }

    public void BeginGame()
    {
		backgroundLoop.GetComponent<BackgroundMusic>().Init((int)Loops.Bb7_BOSSA);
        //Delay before activating game controller
        StartCoroutine(StartController(0.5f)); 
        ChangeState(GameState.Playing);
        tickUnlockTime = new NotationTime(Metronome.Instance.currentTime);
        tickUnlockTime.Add(new NotationTime(0, 0, 2));
        StartCoroutine(StartSpawn(2f));
        Metronome.tickChangeDelegate += HandleTickChange;
    }

    public void HandleTickChange(NotationTime currentTime)
    {
        if (tickUnlockTime.TimeAsTicks() == Metronome.Instance.currentTime.TimeAsTicks())
        {
            tickLock = false;
        }
    }

    void ChangeState(GameState newState)
    {
        state = newState;
        stateChangeDelegate();
    }

    public bool HasFirepower()
    {
        return ticksFired < MAX_FIREPOWER;
    }

	private void GameOver() {
		Debug.Log("lol, u lose :P");
        ChangeState(GameState.GameOver);
		controller.Deactivate();
	}
    
    public void PauseGame()
    {
        //Time.timeScale = 0; Will need to be smarter than this.
        //TODO: Manage pausing/resuming metro and audio and gameplay in sync :D
        controller.Deactivate();
        ChangeState(GameState.Paused);

    }

    public void HandleDolphinInput()
    {
        DestroyZones();
    }

    public void HandleBlastZoneInput(Vector2 touchPosition)
    {
        if (!tickLock && HasFirepower())
        {
            BlastZone touchedBlastZone = CheckForBlastZones(touchPosition);
            if (touchedBlastZone != null)
            {
                if (touchedBlastZone.CanGrow())
                {
                    touchedBlastZone.GrowNextTick();
                    ticksFired++;
                }
            } else if (zoneCount < MAX_ZONES) {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
                Vector3 spawnLocation = ray.origin + (ray.direction * (ray.origin.y +2));
                GameObject zoneMade = GameObject.Instantiate(blastZone, spawnLocation, Quaternion.identity);
                blastZones.Add(zoneMade.GetComponent<BlastZone>());
                zoneMade.transform.parent = blastZoneParent;
                zoneMade.GetComponent<BlastZone>().CreateZone(Metronome.Instance.currentTime);
                ticksFired++;
                zoneCount++;
            }
            tickUnlockTime = new NotationTime(Metronome.Instance.currentTime);
            tickUnlockTime.Add(new NotationTime(0, 0, 2));
            tickLock = true;
        }
    }

    public BlastZone CheckForBlastZones(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
        RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, 1000.0f);
        for (int i = 0; i < hits.Length; i++)
        {

            GameObject obj = hits[i].transform.gameObject;
            if (obj.CompareTag("Blast Zone"))
            {
                return obj.GetComponent<BlastZone>();
            }
        }
        return null;
    }


    public void ResumeGame()
    {
       // Time.timeScale = 1;
        ChangeState(GameState.Playing);
        StartCoroutine(StartController(1f));

    }

    //Logic for destruction of zones goes here.
    public void DestroyZones()
    {
        foreach (BlastZone zone in blastZones)
        {
            zone.DestroyZone();
        }
        blastZones.Clear();
        zoneCount = 0;
        ticksFired = 0;
    }

    public void IncreaseScore (int points)
    {
		gameScore += points;
        UpdateText();
    }

    public void IncreasePollution (int damage)
    {
		currentPollutionLevel += damage;
		if(currentPollutionLevel >= maxPollutionLevel) {
		//	GameOver();
		}
        UpdateText();
    }

    public void UpdateText()
    {
        debugScoreDisplay.text = "Score: " + gameScore + "\nPollution: " + currentPollutionLevel;
    }

    IEnumerator StartController(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        controller.Activate();
    }

    IEnumerator StartSpawn(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GameObject.Instantiate(toSpawn, spawnPoint.position, Quaternion.identity);
        }
    }
}
