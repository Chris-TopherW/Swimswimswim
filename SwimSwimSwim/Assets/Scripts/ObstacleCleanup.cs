using UnityEngine;
using System.Collections;

public class ObstacleCleanup : MonoBehaviour {
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destroyable"))
        {
            GameManager.currentPollutionLevel++;
            Destroy(other.gameObject);
        }
    }
}
