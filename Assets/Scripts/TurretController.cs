using UnityEngine;
using System.Collections;

public class TurretController : EnemyController {

	public GameObject player;

	protected GameObject turret;

	protected void initTurret(){
		turret = transform.FindChild ("Turret").gameObject;
	}

	protected float calculateAngle(){
		float xDiff = transform.position.x - player.transform.position.x;
		float zDiff = transform.position.z - player.transform.position.z;

		float tan = xDiff / zDiff;
		float angle = Mathf.Rad2Deg * Mathf.Atan (tan);
		float angleDisplay = 0.0f;

		if (Mathf.Abs (Mathf.RoundToInt (angle)) > 45.0f) {
			angleDisplay = 90.0f;
		} else if (Mathf.Abs (Mathf.RoundToInt (angle)) > 22.5f) {
			angleDisplay = 45.0f;
		}

		if (angle < 0) {
			angleDisplay *= -1;
		}

		turret.transform.localEulerAngles = new Vector3 (0.0f, angleDisplay, 0.0f);

		return angleDisplay;
	}
}
