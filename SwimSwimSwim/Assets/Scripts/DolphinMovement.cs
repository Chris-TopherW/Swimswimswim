using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

	public AudioMixer audioMixer;
    public TestModel path;
    public float dolphinSpeed;
	private Vector3 dolphinPosition;
    private float t = 0;

	// Use this for initialization
	void Start () {
		dolphinPosition = transform.position;
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

        if (dolphinPosition.y < -2.5f) {
			audioMixer.SetFloat ("CrusherMix", 0.4f);
            audioMixer.SetFloat("DECIMATION", Random.Range(1,100));
            audioMixer.SetFloat ("DecimateMix", 0.1f);
			audioMixer.SetFloat ("LowPassFreq", 5000.0f);
		} else {
            audioMixer.SetFloat("DECIMATION", 1);
            audioMixer.SetFloat ("CrusherMix", 1.0f);
			audioMixer.SetFloat ("DecimateMix", 1.0f);
			audioMixer.SetFloat ("LowPassFreq", 22000.0f);
		}

	}
}
