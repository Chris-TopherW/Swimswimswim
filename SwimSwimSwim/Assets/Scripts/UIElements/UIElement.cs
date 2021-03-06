﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// This class is responsible for making sure a particular UI element is visible and working on a particular gameState.
/// </summary>
/// 
public enum UIElementState
{
    Inactive, Active
}

public class UIElement : MonoBehaviour {
    public UIElementState elemState = UIElementState.Inactive;
    public List<GameState> activeStates;
    private ButtonTextFade fade;

    // Use this for initialization
    void Awake () {
        fade = GetComponentInChildren<ButtonTextFade>();
        //Subscribes to state change delegate of the game manager, whenever the state changes this gets called.
        BlastManager.stateChangeDelegate += UpdateState;
        UpdateState();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateState()
    {
        if (activeStates.Contains(BlastManager.Instance.state) && elemState == UIElementState.Inactive)
        {
            Enable();
        }
        else if (!activeStates.Contains(BlastManager.Instance.state) && elemState == UIElementState.Active)
        {
            Disable();
        }
    }


    public void Disable()
    {
        gameObject.GetComponent<Image>().raycastTarget = false;
        gameObject.GetComponent<Button>().interactable = false;
        if (fade != null)
        {
            fade.TextFadeOut();
        }
        elemState = UIElementState.Inactive;
    }

    public void Enable()
    {
        gameObject.GetComponent<Image>().raycastTarget = true;
        gameObject.GetComponent<Button>().interactable = true;
        if (fade != null)
        {
            fade.TextFadeIn();
        }
        elemState = UIElementState.Active;
    }

}
