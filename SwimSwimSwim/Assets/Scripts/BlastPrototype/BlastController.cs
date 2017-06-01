using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastController : MonoBehaviour {
    public bool activated;
    public Collider playerCollider;
    public Collider blastZoneCollider;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        if (BlastManager.Instance.state == GameState.Playing && activated)
        {
            //Rewrite -- Raycast to dolphin first,
            //Then raycast to blast zone.
            Vector2 touchPos ;
            if (Input.touchCount > 0)
            {
                touchPos = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButton(0))
            {
                touchPos = Input.mousePosition;
            } else
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            if (playerCollider.Raycast(ray, out hit, 1000.0F))
            {
                BlastManager.Instance.HandleDolphinInput();
            } else if (blastZoneCollider.Raycast(ray, out hit, 1000.0F))
            {
                BlastManager.Instance.HandleBlastZoneInput(touchPos);
            }
            return;
        }
   
	}

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
    }
}
