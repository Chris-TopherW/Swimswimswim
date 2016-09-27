﻿using UnityEngine;
using System.Collections;

public class LaserControl : MonoBehaviour {

    public Camera cam;

	private LineRenderer line;
	private AudioSource audioSource;
	private float currentStep;
	private float increment = 0.00392156862f;
    private float increment2 = 0.00392156862f * 5;
    public bool inverse;
    private bool damaging;

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
			audioSource.volume = 0.05f;
            RaycastHit vHit = new RaycastHit();
            Vector3 InverseMouse = new Vector3(Screen.width -  Input.mousePosition.x, Input.mousePosition.y, 0);
            Ray vRay;
            if (inverse)
            {
                vRay = cam.ScreenPointToRay(InverseMouse);
            } else
            {
                vRay = cam.ScreenPointToRay(Input.mousePosition);
            }
            line.SetPosition(0, transform.position);
            if (Physics.Raycast(vRay, out vHit, 1000) && vHit.transform.gameObject.tag == "Destroyable")
            {
                damaging = true;
                line.SetPosition(1, vHit.point);
                ObstacleBehaviour targetToDamage = vHit.transform.gameObject.GetComponent<ObstacleBehaviour>();
                targetToDamage.TakeDamage((float)(10.0f * Time.deltaTime));
            }
            else
                damaging = false;
				line.SetPosition (1, vRay.GetPoint (1000));

			yield return null;
		}
		line.enabled = false;
		audioSource.volume = 0.0f;
	}

	void OnAudioFilterRead(float[] data, int channels)
	{
		for (int i = 0; i < 1024; i++) {
            if (damaging) currentStep += increment2;
            else currentStep += increment;
			if (currentStep >= 1.0f) {
				currentStep = 0.0f;
			}
			data [i] = currentStep;
		}
	}
}
