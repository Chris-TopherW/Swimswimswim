using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTextFade : MonoBehaviour
{
    private bool fadingOut;
    private float fadeRate;
    private Text textComponent;
	// Use this for initialization
	void Start ()
	{
	    fadeRate = 1/0.2f;
	    textComponent = gameObject.GetComponent<Text>();
	    fadingOut = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
	    Color textCol = textComponent.color;

        if (fadingOut && textCol.a >= 0)
	    {
	        textCol.a -= fadeRate*Time.deltaTime;
	        if (textCol.a < 0) { textCol.a = 0;}

	    }
	    textComponent.color = textCol;

	}

    public void TextFadeOut()
    {
        fadingOut = true;
    }
}
