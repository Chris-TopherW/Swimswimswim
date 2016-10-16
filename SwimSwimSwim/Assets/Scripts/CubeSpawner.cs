using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    public GameObject CubePrefab;
    public float delay;

    IEnumerator Start()
    {
        while (true)
        {
            yield return StartCoroutine(spawnTimer(delay));
        }
    }

    IEnumerator spawnTimer(float delay)
    {
        Vector3 spawnPos = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10));
        GameObject.Instantiate(CubePrefab, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(delay);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
