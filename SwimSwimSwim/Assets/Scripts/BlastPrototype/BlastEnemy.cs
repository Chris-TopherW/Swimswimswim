using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastEnemy : MonoBehaviour {
    public int hitPoints = 1;
    public int scorePoints = 500;
    public float speed = 0.025f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        UpdatePosition();
        if (hitPoints <= 0)
        {

            BlastManager.Instance.IncreaseScore(scorePoints);

            Destroy(gameObject);
        }
        if (gameObject.transform.position.z <= -6.0f)
        {
            BlastManager.Instance.IncreasePollution(1);

            Destroy(gameObject);
        }
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
    }
}
