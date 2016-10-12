using UnityEngine;
using System.Collections;

public class ObstacleCleanup : MonoBehaviour {
    
	public GameObject 			audioManagerObject;
	private AudioManager 		audioManager;

	void Start() {
		audioManager = audioManagerObject.GetComponent< AudioManager > ();
	}

    void OnTriggerExit( Collider other ) {
		if ( other.gameObject.CompareTag ( "Destroyable" ) ) {
			GameManager.currentPollutionLevel++;
			Destroy (other.gameObject);
			audioManager.DamageEffectOn ();
		} 
		else if ( other.gameObject.CompareTag ( "Friend" ) ) 
		{
			Destroy ( other.gameObject );
		}
    }
}
