using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour 
{
	public Texture2D crosshairTexture; 

	private Rect position; 
	static bool OriginalOn = true;

	void Start() 
	{
		position = new Rect(Input.mousePosition.x, Input.mousePosition.y, crosshairTexture.width/2, crosshairTexture.height/2);
	}

	void OnGUI() 
	{
		Cursor.visible = false; 

		if(OriginalOn == true) 
			GUI.DrawTexture(position, crosshairTexture); 
	}
}