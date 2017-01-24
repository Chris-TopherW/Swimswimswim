using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class MoveCube : MonoBehaviour
{
    private Rigidbody body;
    private Vector3 position;
    public float speed;
	// Use this for initialization
	void Start ()
	{
	    body = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	    position = gameObject.transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            position.x -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            position.x += speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            position.z -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            position.z += speed * Time.deltaTime;
        }
	    if (position.z >= 28)
	    {
	        position.x += 100;
	        position.z = 0;
	    }
	    body.MovePosition(position);
	}
}
