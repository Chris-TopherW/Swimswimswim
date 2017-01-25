using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    
    public GameObject[] 			obstacles;
	public float 					startDelay;
	public float 					randomWidth;
	public float					randomHeight;
	public bool 					spawningToggle;
    private Vector3 				spawnPoint;

	IEnumerator Start(){
		while ( true ) {
			yield return StartCoroutine (spawnTimer (startDelay));
		}
	}

	IEnumerator spawnTimer( float delay ){
		if ( spawningToggle ) {
            Vector3 spawnOffset = transform.rotation * Random.insideUnitCircle * 6;
            spawnPoint = gameObject.transform.position + spawnOffset;
			GameObject spawned;
			spawned = ( GameObject )Instantiate ( obstacles [Random.Range ( 0, obstacles.Length - 1 )], spawnPoint, transform.rotation );
		}
		yield return new WaitForSeconds( delay );
	}

	void Update () {
	}
}
