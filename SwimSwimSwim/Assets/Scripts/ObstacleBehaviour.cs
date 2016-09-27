using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	private GameObject player;
    public float health = 5.0f;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
	}

    public void TakeDamage(float damage)
    {
        health -= damage;
    }
	
	// Update is called once per frame
	void Update () {
        //TODO: REWRITE THIS
		if (health <= 0) 
		{
			Destroy (gameObject);
		}
	}
}
