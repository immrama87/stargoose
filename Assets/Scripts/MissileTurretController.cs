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

			fireMissile (angle);
		} else {
			fired = false;
		}
	}

	private void fireMissile(float angle){
		if (!fired) {
			GameObject _missile = GameObject.Instantiate (missile);

			float zChange = 0.5f;
			float xChange = 0.0f;
			if (angle != 0.0f) {
				xChange = 0.5f;
				if (Mathf.Abs (angle) == 90.0f) {
					zChange = 0.0f;
				}
			}

			if (angle < 0) {
				xChange *= -1;
			}

			_missile.transform.position = new Vector3 (transform.position.x - xChange, transform.position.y, transform.position.z - zChange);
			_missile.transform.localEulerAngles = new Vector3 (0.0f, angle, 0.0f);
			MissileController mc = _missile.GetComponentInChildren<MissileController> ();
			mc.setForce (-10.0f, angle);
			mc.fire ();

			fired = true;
		}
	}
}
