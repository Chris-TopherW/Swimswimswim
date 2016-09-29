using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

	public AudioMixer audioMixer;
    public TestModel path;
    public float dolphinSpeed;
	public GameObject audioManagerObject;

	private AudioManager audioManager;
	private Vector3 dolphinPosition;
    private float t = 0;
	private bool fadeToggle = false;

	// Use this for initialization
	void Start () {
		dolphinPosition = transform.position;
		audioManager = audioManagerObject.GetComponent<AudioManager> ();
	}
	
	// Update is called once per frame
	void Update () 
	{

        t += 0.01f * Time.deltaTime;
        if (t >= 1) t = 0;
        OrientedPoint p = new OrientedPoint();
        p = path.bezier.GetOrientedPoint(t);
        transform.rotation = p.rotation;
        transform.position = p.position;

		if (Input.GetKey (KeyCode.A)) 
		{
			dolphinPosition.x -= dolphinSpeed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.D)) 
		{
			dolphinPosition.x += dolphinSpeed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.S)) 
		{
			dolphinPosition.y -= dolphinSpeed * Time.deltaTime;
		}
		if (Input.GetKey (KeyCode.W)) 
		{
			dolphinPosition.y += dolphinSpeed * Time.deltaTime;
		}

        
        transform.position = p.position + (p.rotation * dolphinPosition);
		if (dolphinPosition.y < -2.5f && !fadeToggle) {
			audioManager.TurnOnEffects ();
			fadeToggle = true;
		} else if(dolphinPosition.y >= -2.5f && fadeToggle) {
			audioManager.TurnOffEffects ();
			fadeToggle = false;
		}
	}
}
