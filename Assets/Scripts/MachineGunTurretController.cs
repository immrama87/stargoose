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

			if (re.IsVisibleFrom(Camera.main) && (angle != 0.0f || player.transform.position.x == transform.position.x)) {
				fireBullets (angle);
			}
		}
	}

	void fireBullets(float angle){
		if (!firing) {
			GameObject _bullet = GameObject.Instantiate (bullet);
			_bullet.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 0.5f);
			_bullet.GetComponent<BulletController> ().setSpeed (-10.0f);
			firing = true;

			StartCoroutine ("reload");
		}
	}

	private IEnumerator reload(){
		yield return new WaitForSeconds (0.05f);
		firing = false;
	}
}
