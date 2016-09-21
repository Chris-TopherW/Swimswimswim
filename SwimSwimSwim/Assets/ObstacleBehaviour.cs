using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	public GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < player.transform.position.z - 5.0f) 
		{
			Destroy (gameObject);
			GameManager.currentPollutionLevel += 1;
		}
	}
}
