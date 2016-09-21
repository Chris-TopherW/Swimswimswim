using UnityEngine;
using System.Collections;

public class LaserControl : MonoBehaviour {

	LineRenderer line;
	private Vector3 mousePosition;

	void Start () 
	{
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		mousePosition = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.y);

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
			mousePosition.x = Input.mousePosition.x - 100;
			mousePosition.y = Input.mousePosition.y - 100;
			Ray ray = new Ray (transform.position, mousePosition);
			RaycastHit hit;

			line.SetPosition (0, ray.origin);
			if (Physics.Raycast (ray, out hit, 100)) 
			{
				line.SetPosition (1, hit.point);
				Destroy (hit.transform.gameObject);
			}
			else
				line.SetPosition (1, ray.GetPoint (100));

			yield return null;
		}
		line.enabled = false;
	}
}
