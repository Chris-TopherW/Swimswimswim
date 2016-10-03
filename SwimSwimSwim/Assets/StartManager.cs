using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour {

//This is a placeholder script...
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space))
			SceneManager.LoadScene ("Chris_Sandbox");
	}
}
