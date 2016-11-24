using UnityEngine;
using System.Collections;

public class PlaneControl : MonoBehaviour {
    public AudioSource[] sources;
    public AudioClip[] clips;
    private GameObject player;
	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
	
	}
	
	// Update is called once per frame
	void Update () {
        gameObject.transform.position = new Vector3(player.transform.position.x, gameObject.transform.position.y, player.transform.position.z);
	}
}
