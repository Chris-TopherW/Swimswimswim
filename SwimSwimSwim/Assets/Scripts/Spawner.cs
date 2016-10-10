using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public CurveImplementation path;
    public GameObject[] obstacles;
	public float startDelay;
	public float randomWidth, randomHeight;
	public bool spawningToggle;

    private Vector3 spawnPoint;

	IEnumerator Start()
	{
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
        while (true)
				yield return StartCoroutine (spawnTimer (startDelay));
	}

	IEnumerator spawnTimer(float delay){
		if (spawningToggle) 
		{
            Vector3 spawnOffset = transform.rotation * Random.insideUnitCircle * 6;
            spawnPoint = gameObject.transform.position + spawnOffset;

			//Spawn boss for fight and stop spawning other objects
			GameObject spawned;
			if (GameManager.gameState == "BossFight") 
			{
				spawned = (GameObject)Instantiate (obstacles [obstacles.Length - 1], spawnPoint, transform.rotation);
				spawningToggle = false;
			} 
			else 
			{
				spawned = (GameObject)Instantiate (obstacles [Random.Range (0, obstacles.Length - 1)], spawnPoint, transform.rotation);
			}

            if (spawned.GetComponent<FriendlyFishMovement>())
            {
                spawned.GetComponent<FriendlyFishMovement>().fishOffset = spawnOffset;
            }
		}
		yield return new WaitForSeconds(delay);
	}

	void Update () 
	{
        float t = GameManager.splinePos + 0.2f;
        if (t >= 1) t = 1;
        int segPos = GameManager.segmentPos + 2;
        float tPos = t;
        OrientedPoint p = path.GetPos(segPos, tPos);
        // body.AddForce(p.position - transform.position);
        transform.rotation = p.rotation;
        transform.position = p.position;
        if (Input.GetKeyDown (KeyCode.Space)) 
		{
			spawnPoint = gameObject.transform.position;
			spawnPoint.x = Random.Range (-randomWidth, randomWidth);
			spawnPoint.y = Random.Range (1.0f, randomHeight);

			Instantiate (obstacles [Random.Range(0,obstacles.Length)], spawnPoint, Quaternion.identity);
		}
	}
}
