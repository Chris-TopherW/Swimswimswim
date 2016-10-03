using UnityEngine;
using System.Collections;

public class ObstacleCleanup : MonoBehaviour {
    
    void OnTriggerExit(Collider other)
    {
		if (other.gameObject.CompareTag ("Destroyable")) {
			GameManager.currentPollutionLevel++;
			Destroy (other.gameObject);
		} 
		else if (other.gameObject.CompareTag ("Friend")) 
		{
			Destroy (other.gameObject);
		}
    }
}
