using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEnemy : MonoBehaviour {
    public int hitPoints;
    public float speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();
	}

    void UpdatePosition()
    {
        Vector3 newPosition = transform.position;
        newPosition.z -= speed;
        this.transform.position = newPosition;
    }

    //Damage to enemy
    public void DoDamage(int damage)
    {
        hitPoints -= damage;
    }

    IEnumerator Cleanup(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        Destroy(gameObject);
    }
}
