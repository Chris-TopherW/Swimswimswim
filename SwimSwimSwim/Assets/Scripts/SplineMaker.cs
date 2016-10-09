using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SplineMaker : MonoBehaviour {

    public GameObject pointPrefab;
    public List<GameObject> points = new List<GameObject>();
	// Use this for initialization
    [ExecuteInEditMode()]
	void Awake () {
        points.Clear();
        for (int i = 0; i < 1000; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-300, 300), 0, i * 200);
            GameObject newPoint = (GameObject)Instantiate(pointPrefab, pos, Quaternion.identity);
            newPoint.GetComponent<PointControl>().width = Random.Range(40, 180);
            points.Add(newPoint);
        }
        CurveImplementation script = GetComponent<CurveImplementation>();
        script.enabled = true;

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
