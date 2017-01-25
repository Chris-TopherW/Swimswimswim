using UnityEngine;
using System.Collections;

public class ObstacleCleanup : MonoBehaviour {
    

	void Start() {
	}

    void OnTriggerExit( Collider other ) {
		if ( other.gameObject.CompareTag ( "Thumper" ) ) {
			GameManager.currentPollutionLevel++;
			Destroy (other.gameObject);
		} 
		else if ( other.gameObject.CompareTag ( "Friend" ) ) 
		{
			Destroy ( other.gameObject );
		}
    }
}
