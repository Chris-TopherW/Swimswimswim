using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class DolphinMovement : MonoBehaviour {

    public CurveImplementation 	path;
    public float 				dolphinSpeed;
    public float 				defaultPathSpeed = 10;
	public float 				sprintSpeed;
	public GameObject 			audioManagerObject;
	private float 				pathSpeed;
    private Rigidbody 			body;
	private Vector3 			dolphinPosition;
	//private AudioManager		audioManager;
	private bool 				fadeToggle = false;
    private MeshCollider 		leftWallCollider;
    private MeshCollider 		rightWallCollider;
    private float 				maxLeft = 0;
    private float 				maxRight = 0;

	void Start () {
		dolphinPosition = transform.position;
        path = GameObject.FindGameObjectWithTag("Spline").GetComponent<CurveImplementation>();
        leftWallCollider = GameObject.Find("LeftWall").GetComponent<MeshCollider>();
        rightWallCollider = GameObject.Find("RightWall").GetComponent<MeshCollider>();
        //audioManager = audioManagerObject.GetComponent<AudioManager> ();
        body = GetComponent<Rigidbody>();
		pathSpeed = defaultPathSpeed;
    }

	void Update () 
	{
        float distanceToMove = pathSpeed * Time.deltaTime;
		if (Input.GetKey (KeyCode.LeftShift)) {
			pathSpeed = sprintSpeed;
		} else{
			pathSpeed = defaultPathSpeed;
		}
        int segPos = GameManager.segmentPos;
        float tPos = GameManager.splinePos;
        OrientedPoint ptToCheck = path.GetPos(segPos, tPos);
        Vector3 previousPosition = ptToCheck.position;
        float distanceMoved = 0;
        while ( distanceMoved < distanceToMove ){
            tPos += 0.0001f;
            if ( tPos >= 1 ){
                tPos = 0;
                segPos++;
                path.Generate();
            }
            ptToCheck = path.GetPos( segPos, tPos );
            distanceMoved += Vector3.Distance( previousPosition, ptToCheck.position );
            previousPosition = ptToCheck.position;
        }
        GameManager.segmentPos = segPos;
        GameManager.splinePos = tPos;
        OrientedPoint p = path.GetPos(segPos, tPos);
        Ray leftRay = new Ray( p.position, p.rotation*( Vector3.left ) );
        Ray rightRay = new Ray( p.position, p.rotation * ( Vector3.right ) );
        // body.AddForce(p.position - transform.position);
        transform.rotation = p.rotation;
        RaycastHit leftHit;
        RaycastHit rightHit;
        if ( leftWallCollider.Raycast( leftRay, out leftHit, 1000.0f ) && rightWallCollider.Raycast( rightRay, out rightHit, 1000.0f ) ){
            maxLeft = leftHit.distance - 2;
                maxRight = rightHit.distance -2;
        }
        Vector3 rotatedDolphinPos = (p.rotation * dolphinPosition);
        if ( Input.GetKey ( KeyCode.A ) && dolphinPosition.x > -maxLeft ) {
			dolphinPosition.x -= dolphinSpeed * Time.deltaTime;
        }
		if ( Input.GetKey ( KeyCode.D ) && dolphinPosition.x < maxRight ) {
			dolphinPosition.x += dolphinSpeed * Time.deltaTime;
        }
		if ( Input.GetKey ( KeyCode.S ) && dolphinPosition.y > -20.0f ) {
			dolphinPosition.y -= dolphinSpeed * Time.deltaTime;
        }
		if ( Input.GetKey ( KeyCode.W ) && dolphinPosition.y < 20.0f ) {
			dolphinPosition.y += dolphinSpeed * Time.deltaTime;
        }

        if ( dolphinPosition.x < -maxLeft ){
            dolphinPosition.x = -maxLeft;
        }
        if ( dolphinPosition.x > maxRight ){
            dolphinPosition.x = maxRight;
        }
        Vector3 goal = ( p.position + ( p.rotation * dolphinPosition ) );
        body.MovePosition( goal );

		/*if ( dolphinPosition.y < -2.5f && !fadeToggle ) {
			audioManager.TurnOnEffects ();
			fadeToggle = true;
		} else if( dolphinPosition.y >= -2.5f && fadeToggle ) {
			audioManager.TurnOffEffects ();
			fadeToggle = false;
		}*/
	}
}
