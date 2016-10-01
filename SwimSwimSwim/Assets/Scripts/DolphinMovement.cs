using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

    public BezierCurve path;
    public float dolphinSpeed;
	public GameObject audioManagerObject;

	private Vector3 dolphinPosition;
	private AudioManager audioManager;
	private bool fadeToggle = false;

	void Start () {
		dolphinPosition = transform.position;
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<TestModel>().bezier;
        audioManager = audioManagerObject.GetComponent<AudioManager> ();
	}

	void Update () 
	{
		//Movement section
        GameManager.splinePos += 0.01f * Time.deltaTime;
        if (GameManager.splinePos >= 1) GameManager.splinePos = 0;
        OrientedPoint p = new OrientedPoint();
        p = path.GetOrientedPoint(GameManager.splinePos);
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

		//Audio section
		if (dolphinPosition.y < -2.5f && !fadeToggle) {
			audioManager.TurnOnEffects ();
			fadeToggle = true;
		} else if(dolphinPosition.y >= -2.5f && fadeToggle) {
			audioManager.TurnOffEffects ();
			fadeToggle = false;
		}
	}
}
