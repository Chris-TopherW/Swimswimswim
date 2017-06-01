using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDrawing : MonoBehaviour {

    public Color col;
    public int cx;
    public int cy;
    public int r;
	// Use this for initialization
	void Start () {
       // Circle(tex, cx, cy, r, col);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //http://answers.unity3d.com/questions/590469/drawing-a-solid-circle-onto-texture.html
    public void Circle(Texture2D tex, int cx, int cy, int r, Color col)
    {
        int x, y, px, nx, py, ny, d;
        Color[] tempArray = tex.GetPixels();

        for (x = 0; x <= r; x++)
        {
            d = (int)Mathf.Ceil(Mathf.Sqrt(r * r - x * x));
            for (y = 0; y <= d; y++)
            {
                px = cx + x;
                nx = cx - x;
                py = cy + y;
                ny = cy - y;

                tempArray[py * 1024 + px] = col;
                tempArray[py * 1024 + nx] = col;
                tempArray[ny * 1024 + px] = col;
                tempArray[ny * 1024 + nx] = col;
            }
        }
        tex.SetPixels(tempArray);
        tex.Apply();
    }
}
