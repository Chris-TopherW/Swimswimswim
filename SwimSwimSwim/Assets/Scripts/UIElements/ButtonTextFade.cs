﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextFade : MonoBehaviour
{
    private bool fadingOut, fadingIn;
    private float fadeRate;
    private Text textComponent;

    //TODO: Implement state machine to avoid bool grossness.
	// Use this for initialization
	void Awake ()
	{
	    fadeRate = 1/0.1f;
	    textComponent = gameObject.GetComponent<Text>();
	    fadingOut = false;
        fadingIn = false;
    }
	
	// Update is called once per frame
	void Update ()
	{
	    Color textCol = textComponent.color;

        if (fadingOut && textCol.a >= 0)
	    {
	        textCol.a -= fadeRate*Time.unscaledDeltaTime;
	        if (textCol.a < 0) { textCol.a = 0;
	            fadingOut = false;
	        }
	    }

        if (fadingIn && textCol.a <= 1)
        {
            textCol.a += fadeRate * Time.unscaledDeltaTime;
            if (textCol.a > 1)
            {
                textCol.a = 1;
                fadingIn = false;
            }
        }
        textComponent.color = textCol;

	}

    public void TextFadeOut()
    {
        fadingIn = false;
        fadingOut = true;
    }

    public void TextFadeIn()
    {
        fadingOut = false;
        fadingIn = true;
    }
}
