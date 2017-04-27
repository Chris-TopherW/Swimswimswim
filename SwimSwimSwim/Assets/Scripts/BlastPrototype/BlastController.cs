using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastController : MonoBehaviour {
    public bool activated;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (BlastManager.Instance.state == GameState.Playing && activated)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchPos = Input.GetTouch(0).position;
                if (touchPos.y > 200)
                {
                    BlastManager.Instance.HandleBlastZoneInput(touchPos);
                }
            }
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
