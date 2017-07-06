using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastPlayer : MonoBehaviour {
    public float speed;
    public float posStart;
    public Material gridMaterial;
    public Transform blastZonePos;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        gridMaterial.SetFloat(Shader.PropertyToID("_ZonePosition"), posStart + blastZonePos.position.z);
        this.gameObject.transform.position += new Vector3(0,0,speed * Time.deltaTime);
	}
}
