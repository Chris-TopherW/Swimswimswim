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

    public Collider blastCollider;

    private List<BlastEnemy> Enemies;
    public List<BlastZone> BlastZones
    {
        get
        {
            return blastZones;
        }

        set
        {
            blastZones = value;
        }
    }

    protected BlastManager() { }

    void Awake()
    {
        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 60;
    }

    // Use this for initialization
    void Start () {
        BlastZones = new List<BlastZone>();
        Enemies = new List<BlastEnemy>();
        controller = GetComponent<BlastController>();
        UpdateText();
        spawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        blastZoneParent = GameObject.Find("BlastZones").GetComponent<Transform>();
    }

    public void BeginGame()
    {
		backgroundLoop.GetComponent<BackgroundMusic>().Init("ChipBoss");
        //Delay before activating game controller
        StartCoroutine(StartController(0.5f)); 
        ChangeState(GameState.Playing);
        tickUnlockTime = new NotationTime(Metronome.Instance.currentTime);
        tickUnlockTime.Add(new NotationTime(0, 0, 1));
        StartCoroutine(StartSpawn(4f));
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
                RaycastHit hit;
                if (blastCollider.Raycast(ray, out hit, 1000.0F))
                {
                    GameObject zoneMade = GameObject.Instantiate(blastZone, hit.point, Quaternion.identity);
                    BlastZones.Add(zoneMade.GetComponent<BlastZone>());
                    zoneMade.transform.parent = blastZoneParent;
                    zoneMade.GetComponent<BlastZone>().CreateZone(Metronome.Instance.currentTime);
                    ticksFired++;
                    zoneCount++;
                }
            }
            tickUnlockTime = new NotationTime(Metronome.Instance.currentTime);
            tickUnlockTime.Add(new NotationTime(0, 0, 1));
            tickLock = true;
        }
    }

    public BlastZone CheckForBlastZones(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(touchPosition.x, touchPosition.y, 0));
        RaycastHit hit;
        BlastZone zone = null;
        float zDistance = float.PositiveInfinity;
        if (blastCollider.Raycast(ray, out hit , 1000.0f)) {
            foreach (BlastZone z in BlastZones)
            {
                float d = Vector2.Distance(hit.point.xz(), z.transform.position.xz()) - z.zoneScale/2;
                if (d <= 0 && d < zDistance)
                {
                    zDistance = d;
                    zone = z;
                }
            }
        }
        return zone;
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
        foreach (BlastZone zone in BlastZones)
        {
            zone.DestroyZone();
        }
        foreach (BlastEnemy enemy in Enemies)
        {
            enemy.Hit();
        }
        BlastZones.Clear();
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
            Vector3 position = spawnPoint.position; //+ new Vector3(0, Random.Range(-3, 7), 0);
            GameObject.Instantiate(toSpawn, position, Quaternion.identity);
            Enemies.Clear();
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Destroyable");
            for (int i = 0; i < enemies.Length; i++)
            {
                Enemies.Add(enemies[i].GetComponent<BlastEnemy>());
            }

        }
    }
}
