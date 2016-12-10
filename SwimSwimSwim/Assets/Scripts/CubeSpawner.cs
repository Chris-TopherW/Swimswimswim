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
        Collider[] checkResult = Physics.OverlapSphere(spawnPos, 0.5f);

        if (checkResult.Length == 0)
        {
            // all clear!
            GameObject newCube = (GameObject)GameObject.Instantiate(CubePrefab, spawnPos, Quaternion.identity);
            CubeHandler.handler.AddCube(newCube.GetComponent<CubeThumper>());
        }
        yield return new WaitForSeconds(delay);
    }

    // Update is called once per frame
    void Update () {
	
	}
}
