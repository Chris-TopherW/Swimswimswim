using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {

	public GameObject[] obstacles;
	public float startDelay;
	public float randomWidthX;
	public bool spawningToggle;

	private Vector3 spawnPoint;

	IEnumerator Start()
	{
			while (true)
				yield return StartCoroutine (spawnTimer (startDelay));
	}

	IEnumerator spawnTimer(float delay){
		if (spawningToggle) 
		{
			spawnPoint = gameObject.transform.position;
			spawnPoint.x = Random.Range (-randomWidthX, randomWidthX);
			Instantiate (obstacles [Random.Range(0,3)], spawnPoint, Quaternion.identity);
		}
		yield return new WaitForSeconds(delay);
	}

	void Update () 
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			spawnPoint = gameObject.transform.position;
			spawnPoint.x = Random.Range (-randomWidthX, randomWidthX);
			Instantiate (obstacles [Random.Range(0,3)], spawnPoint, Quaternion.identity);
		}
	}
}
