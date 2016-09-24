using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.transform.position.z < player.transform.position.z) 
		{
			Destroy (gameObject);
			GameManager.currentPollutionLevel += 1;
		}
	}
}
