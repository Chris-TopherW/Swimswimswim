using UnityEngine;
using System.Collections;

public class LaserControl : MonoBehaviour {

    public Camera 				cam;
	public bool					inverse;
	public static int 			laserChargeMax = 150;
	public static int			laserChargeThreshold = 50;
	public static float		 	laserCharge;
	private LineRenderer		line;
	private AudioSource 		audioSource;
	private Material 			material;
	private float 				currentStep;
	private float 				increment = 0.00392156862f;
    private float 				increment2 = 0.00392156862f * 5;
    private bool 				damaging;
	private bool 				laserIsFiring = false;

	void Start () {
		line = GetComponent<LineRenderer> ();
		line.enabled = false;
		currentStep = 0;
		laserCharge = laserChargeMax;
		audioSource = GetComponent<AudioSource> ();
        material = GetComponent<Renderer>().material;
	}

	void Update () {
		if ( Input.GetButtonDown ( "Fire1" ) && laserCharge > laserChargeThreshold && !laserIsFiring ) {
			StopCoroutine ( "FireLaser" );
			StartCoroutine ( "FireLaser" );
		}
		if ( !laserIsFiring && laserCharge < laserChargeMax ) {
			laserCharge += 10.0f * Time.deltaTime;
		}
	}

	IEnumerator FireLaser(){
		laserIsFiring = true;
		line.enabled = true;
		while ( Input.GetButton ( "Fire1" ) && laserCharge > 0 ) {
			laserCharge -= 10.0f * Time.deltaTime;
			audioSource.volume = 0.05f;
            RaycastHit vHit = new RaycastHit();
            Vector3 InverseMouse = new Vector3( Screen.width -  Input.mousePosition.x, Input.mousePosition.y, 0 );
            Ray vRay;
            if ( inverse ){
                vRay = cam.ScreenPointToRay( InverseMouse );
            } else {
                vRay = cam.ScreenPointToRay( Input.mousePosition );
            }
            line.SetPosition( 0, transform.position );
            if ( Physics.Raycast( vRay, out vHit, 1000 ) ){
                line.SetPosition( 1, vHit.point );
                Ray dolphinRay = new Ray( transform.position, ( vHit.point - transform.position ) );
                if ( Physics.Raycast( dolphinRay, out vHit, 1000 ) && vHit.transform.gameObject.tag == "Destroyable" ) {
                    damaging = true;
                    Color finalColor = Color.white * Mathf.LinearToGammaSpace( 50.0f );
                    material.SetColor( "_EmissionColor", finalColor );
                    line.SetPosition( 1, vHit.point );
                    ObstacleBehaviour targetToDamage = vHit.transform.gameObject.GetComponent<ObstacleBehaviour>();
                    targetToDamage.TakeDamage( ( float )( 10.0f * Time.deltaTime ) );
                } else {
                    damaging = false;
                    Color finalColor = Color.blue * Mathf.LinearToGammaSpace( 5.0f );
                    material.SetColor( "_EmissionColor", finalColor );
                }
            } else {
                damaging = false;
                Color finalColor = Color.blue * Mathf.LinearToGammaSpace( 5.0f );
                material.SetColor( "_EmissionColor", finalColor );
                line.SetPosition( 1, vRay.GetPoint( 1000 ) );
            }
			yield return null;
		}
		line.enabled = false;
		audioSource.volume = 0.0f;
		laserIsFiring = false;
	}

	void OnAudioFilterRead( float[] data, int channels ){
		for ( int i = 0; i < 1024; i++ ) {
            if (damaging) currentStep += increment2;
            else currentStep += increment;
			if ( currentStep >= 1.0f ) {
				currentStep = 0.0f;
			}
			data [i] = currentStep;
		}
	}
}
