using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour {

	public GameObject[] fishes;
	private List<float> iterator;
	public float increment = 0.01f;
	public float amplitude = 0.01f;

	// Use this for initialization
	void Start () {
		iterator = new List<float>();
		foreach(GameObject child in fishes)
		{
			float startVal = Random.Range(0.0f, Mathf.PI);
			iterator.Add(startVal);
			child.GetComponent<Animation>()["Take 001"].time = Random.Range(0.0f, child.GetComponent<Animation>()["Take 001"].length);
			child.GetComponent<Animation>()["Take 001"].speed = Random.Range(0.7f, 1.2f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		int i = 0;
		foreach(GameObject child in fishes)
		{
			Vector3 temp = child.transform.position + new Vector3(Random.Range(amplitude, amplitude * 2) * Mathf.Sin(iterator[i]),0,0);
			child.transform.position = temp;
			iterator[i] += increment + Random.Range(increment, increment * 2);
			i++;
		}

	}
}
