using UnityEngine;
using System.Collections;

public class ObstacleBehaviour : MonoBehaviour {

	public float 			health = 5.0f;
	public GameObject 		explosionSound;
    private GameObject 		player;

	void Start () {
		player = GameObject.Find ( "Player" );
	}

    public void TakeDamage( float damage ) {
        health -= damage;
    }

	void Update () {
		if ( health <= 0  ){
			Instantiate ( explosionSound, transform.position, transform.rotation );
			Destroy ( gameObject );
		}
	}
}
