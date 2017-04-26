using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class 
/// </summary>

public class UIController : Singleton<UIController> {

    protected UIController() { }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void BeginButtonClick()
    {
        BlastManager.Instance.BeginGame();
    }

    public void Pause()
    {
        
    }

}
