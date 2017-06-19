using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour {
    public Renderer rend;
    // Use this for initialization
    void Start () {
        rend = GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, 360/10 * Time.deltaTime,0); //rotates 50 degrees per second around z axis
        float glitch = Mathf.PingPong(Time.time / 5, 0.5f);
        rend.material.SetFloat("_GlitchAmount", glitch);
    }
}
