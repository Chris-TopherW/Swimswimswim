using UnityEngine;
using System.Collections;

public class CrossHair : MonoBehaviour 
{
	public Texture2D crosshairTexture;
    public float scale;
	private Rect position;
    private float size;
	static bool OriginalOn = true;

	void Start() 
	{
        size = Screen.height / scale;
        position = new Rect(Input.mousePosition.x - size / 2, -Input.mousePosition.y - size / 1.54f, size, size);
	}

	void OnGUI() 
	{
        size = Screen.height / scale;

        position.Set (Input.mousePosition.x - size/2, -Input.mousePosition.y + (Screen.height) - size/ 1.54f, size, size);
		Cursor.visible = false; 

		if(OriginalOn == true) 
			GUI.DrawTexture(position, crosshairTexture); 
	}
}