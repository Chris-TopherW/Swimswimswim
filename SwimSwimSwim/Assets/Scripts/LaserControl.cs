using UnityEngine;
using System.Collections;

public class LaserControl : MonoBehaviour {

    public Camera cam;
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
	}
}
