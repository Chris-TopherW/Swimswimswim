using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

    public BezierCurve path;
    public GameObject[] obstacles;
	public float startDelay;
	public float randomWidth, randomHeight;
	public bool spawningToggle;

    private Vector3 spawnPoint;

	IEnumerator Start()
	{
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<TestModel>().bezier;
        while (true)
				yield return StartCoroutine (spawnTimer (startDelay));
	}

	IEnumerator spawnTimer(float delay){
		if (spawningToggle) 
		{
            Vector3 spawnOffset = transform.rotation * Random.insideUnitCircle * 6;
            spawnPoint = gameObject.transform.position + spawnOffset;

            Instantiate (obstacles [Random.Range(0,obstacles.Length)], spawnPoint, Quaternion.identity);
		}
		yield return new WaitForSeconds(delay);
	}

	void Update () 
	{
        OrientedPoint p = new OrientedPoint();
        p = path.GetOrientedPoint(GameManager.splinePos + 0.02f);
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
