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

    protected BlastManager() { }

    // Use this for initialization
    void Start () {
        blastZones = new List<BlastZone>();
        controller = GetComponent<BlastController>();
		
	}

    public void BeginGame()
    {
		backgroundLoop.GetComponent<BackgroundMusic>().Init((int)Loops.Bb7_BOSSA);
        //Delay before activating game controller
        StartCoroutine(StartController(1f)); 
        ChangeState(GameState.Playing);
        tickUnlockTime = new NotationTime(Metronome.Instance.currentTime);
        tickUnlockTime.Add(new NotationTime(0, 0, 2));

        Metronome.tickChangeDelegate += HandleTickChange;
    }

    public void HandleTickChange(NotationTime currentTime)
    {
        if (tickUnlockTime.TimeAsTicks() - 1 == Metronome.Instance.currentTime.TimeAsTicks())
        {
            tickLock = false;
            tickUnlockTime.Add(new NotationTime(0, 0, 4));
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
    
    public void PauseGame()
    {
        //Time.timeScale = 0; Will need to be smarter than this.
        //TODO: Manage pausing/resuming metro and audio and gameplay in sync :D
        controller.Deactivate();
        ChangeState(GameState.Paused);

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
                Vector3 spawnLocation = ray.origin + (ray.direction * ray.origin.y);
                GameObject zoneMade = GameObject.Instantiate(toSpawn, spawnLocation, Quaternion.identity);
                zoneMade.GetComponent<BlastZone>().CreateZone(Metronome.Instance.currentTime);
                ticksFired++;
                zoneCount++;
            }
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

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator StartController(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        controller.Activate();
    }
}
