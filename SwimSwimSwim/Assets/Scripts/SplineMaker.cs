using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineMaker : MonoBehaviour {

    public GameObject 			pointPrefab;
    public List<GameObject> 	points = new List<GameObject>();
    [ExecuteInEditMode()]
	void Awake () {
        CurveImplementation script = GetComponent<CurveImplementation>();
        script.enabled = false;
        points.Clear();
        float zPos = 0;
        for (int i = 0; i < 512; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-100, 100), 0, zPos);
            GameObject newPoint = (GameObject)Instantiate(pointPrefab, pos, Quaternion.identity);
            newPoint.GetComponent<PointControl>().width = Random.Range(20, 200);
            points.Add(newPoint);
            zPos += Random.Range(150, 200);
        }
        script.enabled = true;

    }
}
