using UnityEngine;
using System.Collections;

public class MissileTurretController : TurretController {

	public GameObject missile;

	private Renderer re;
	private bool fired;

	// Use this for initialization
	void Start () {
		initTurret ();
		re = turret.GetComponent<Renderer> ();
		fired = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (re.IsVisibleFrom (Camera.main)) {
			float angle = calculateAngle ();

			calculateHitChance (angle);
		}
	}

	private void calculateHitChance(float angle){
		if(angle == 0.0f){
			bool straight = player.transform.position.x - (player.transform.localScale.x / 2) < transform.position.x &&
		                player.transform.position.x + (player.transform.localScale.x / 2) > transform.position.x;

			if (straight) {
				fireMissile ();
			}
		} else {
			float x = player.transform.position.x;
			float slope = 0.0f;
			if (Mathf.Abs (angle) == 45.0f) {
				slope = 1.0f;
			}

			float a = player.transform.position.x - transform.position.x;
			float z = transform.position.z - (Mathf.Abs (a) * slope);

			if (player.transform.position.z < z) {
				float playerZSpeed = player.GetComponent<PlayerController> ().getZSpeed ();
				float drawsToPoint = (z - player.transform.position.z) / playerZSpeed;

				Debug.Log ("Player position: [" + player.transform.position.x + ", " + player.transform.position.z + "]; Collision Position: [" + x + ", " + z + "]; zSpeed: " + playerZSpeed + "; Draws to Collision: " + drawsToPoint);
			}
		}
	}

	private void fireMissile(){
		GameObject _missile = GameObject.Instantiate (missile);
	}
}
