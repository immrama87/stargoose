using UnityEngine;
using System.Collections;

public class MachineGunTurretController : TurretController {

	public GameObject bullet;

	private Renderer re;
	private bool firing;

	// Use this for initialization
	void Start () {
		initTurret ();
		re = turret.GetComponent<Renderer> ();
		firing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (player.transform.position.z < transform.position.z) {
			float angle = calculateAngle ();

			if (re.IsVisibleFrom(Camera.main)) {
				fireBullets (angle);
			}
		}
	}

	void fireBullets(float angle){
		if (!firing) {
			GameObject _bullet = GameObject.Instantiate (bullet);
			float xPos = 0.0f;
			float zPos = 0.0f;
			if (angle != 0) {
				xPos = 0.5f;
			}
			if (angle < 0) {
				xPos *= -1;
			}
			if (Mathf.Abs(angle) != 90.0f) {
				zPos = 0.5f;
			}

			_bullet.transform.localEulerAngles = new Vector3 (0.0f, angle, 0.0f);
			_bullet.transform.position = new Vector3 (transform.position.x - xPos, transform.position.y + 0.4f, transform.position.z - zPos);
			_bullet.GetComponent<BulletController> ().setVelocity (-10.0f, angle);
			firing = true;

			StartCoroutine ("reload");
		}
	}

	private IEnumerator reload(){
		yield return new WaitForSeconds (0.05f);
		firing = false;
	}
}
