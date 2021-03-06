﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour
{
    private ButtonTextFade fade;

    void Start()
    {
        fade = GetComponentInChildren<ButtonTextFade>();
    }
    
    public void Disable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameObject.GetComponent<Button>().interactable = false;
        fade.TextFadeOut();
    }

    public void Enable()
    {
        
        gameObject.GetComponent<Button>().interactable = true;
        fade.TextFadeIn();
    }

}
