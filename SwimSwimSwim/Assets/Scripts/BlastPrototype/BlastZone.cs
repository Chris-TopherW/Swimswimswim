using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlastZoneStates
{
    Hidden, Visible
}

public class BlastZone : MonoBehaviour {
    public const int MAX_SIZE = 8;

    private MeshRenderer mesh;
    private float zoneScale = 3.0f;
    private bool growNextTick = false;

    private List<BlastEnemy> enemiesInZone;


    private int currentSize;

    //NotationTime timeToCreate;
    // Use this for initialization
    void Awake () {
        enemiesInZone = new List<BlastEnemy>();
        Metronome.tickChangeDelegate += HandleTickChange;

    }

    public void HandleTickChange(NotationTime currentTime)
    {
        if (growNextTick)
        {
            GrowZone();
            growNextTick = false;
        }
    }

    public void CreateZone(NotationTime timeToCreate)
    {
        //this.timeToCreate = timeToCreate;
        gameObject.GetComponent<MeshRenderer>().enabled = true;
        currentSize++;
		SFXPlayer.Instance.PlayCircleStart();
    }
    
    public void DestroyZone()
    {
        //Shallow clone
        List<BlastEnemy> localEnemies = enemiesInZone.GetRange(0, enemiesInZone.Count);
        foreach (BlastEnemy enemy in localEnemies)
        {
            enemy.DoDamage(1);
        }
        Metronome.tickChangeDelegate -= HandleTickChange;
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Destroyable"))
        {
            enemiesInZone.Add(other.gameObject.GetComponent<BlastEnemy>());
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyable"))
        {
            enemiesInZone.Remove(other.gameObject.GetComponent<BlastEnemy>());
        }
    }


    public void GrowNextTick()
    {
        growNextTick = true;
		SFXPlayer.Instance.PlayCircleStart();
        currentSize++;
    }

    public bool CanGrow()
    {
        return currentSize < MAX_SIZE;
    }

    public void GrowZone()
    {
        zoneScale += 0.5f;
        if (gameObject != null)
        {
            this.gameObject.transform.localScale = new Vector3(zoneScale, zoneScale, 1f);
        }

    }
}
