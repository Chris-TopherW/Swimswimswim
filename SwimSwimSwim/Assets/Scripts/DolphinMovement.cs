using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

    public CurveImplementation path;
    public float dolphinSpeed;
	public GameObject audioManagerObject;
    private Rigidbody body;
    public float areaWidth;
	private Vector3 dolphinPosition;
	private AudioManager audioManager;
	private bool fadeToggle = false;

	void Start () {
		dolphinPosition = transform.position;
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
        audioManager = audioManagerObject.GetComponent<AudioManager> ();
        body = GetComponent<Rigidbody>();

    }

	void FixedUpdate () 
	{
		//Movement section
        GameManager.splinePos += 0.01f * Time.deltaTime;
        if (GameManager.splinePos >= 1) GameManager.splinePos = 0;
        int segPos = (int)(GameManager.splinePos * (path.Points.Count -1));
        float tPos = GameManager.splinePos * (path.Points.Count - 1) - segPos;
        OrientedPoint p = path.GetPos(segPos, tPos);
        // body.AddForce(p.position - transform.position);
        transform.rotation = p.rotation;
        Vector3 rotatedDolphinPos = (p.rotation * dolphinPosition);


        if (Input.GetKey (KeyCode.A) && dolphinPosition.x > -areaWidth / 2.0f) 
		{
			dolphinPosition.x -= dolphinSpeed * Time.deltaTime;
        }
		if (Input.GetKey (KeyCode.D) && dolphinPosition.x < areaWidth / 2.0f) 
		{
			dolphinPosition.x += dolphinSpeed * Time.deltaTime;
        }
		if (Input.GetKey (KeyCode.S) && dolphinPosition.y > -areaWidth / 2.0f) 
		{
			dolphinPosition.y -= dolphinSpeed * Time.deltaTime;
        }
		if (Input.GetKey (KeyCode.W) && dolphinPosition.y < areaWidth/2.0f) 
		{
			dolphinPosition.y += dolphinSpeed * Time.deltaTime;
        }

       Vector3 goal = (p.position + (p.rotation * dolphinPosition));
        body.MovePosition(goal);

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
