using UnityEngine;
using System.Collections;

public class LaserControl : MonoBehaviour {

    public Camera cam;

	private LineRenderer line;
	private AudioSource audioSource;
	private float currentStep;
	private float increment = 0.00392156862f;

	void Start () 
	{
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		//audio setup
		currentStep = 0;
		audioSource = GetComponent<AudioSource> ();

	}

	void Update () 
	{
		if (Input.GetButtonDown ("Fire1"))
		{
			Debug.Log ("Fire!");
			StopCoroutine ("FireLaser");
			StartCoroutine ("FireLaser");
		}
	}

	IEnumerator FireLaser()
	{
		line.enabled = true;
		while (Input.GetButton ("Fire1")) 
		{
			audioSource.volume = 0.1f;
            RaycastHit vHit = new RaycastHit();
            Ray vRay = cam.ScreenPointToRay(Input.mousePosition);
            line.SetPosition(0, transform.position);
			if (Physics.Raycast(vRay, out vHit, 1000) && vHit.transform.gameObject.tag == "Destroyable")
            {
                line.SetPosition(1, vHit.point);
                Destroy(vHit.transform.gameObject);
            }
			else
				line.SetPosition (1, vRay.GetPoint (1000));

			yield return null;
		}
		line.enabled = false;
		audioSource.volume = 0.0f;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		for (int i = 0; i < 1024; i++) {
			currentStep += increment;
			if (currentStep >= 1.0f) {
				currentStep = 0.0f;
			}
			data [i] = currentStep;
		}
	}
}
