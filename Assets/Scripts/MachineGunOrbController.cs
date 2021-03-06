﻿using UnityEngine;
using System.Collections;

public class MachineGunOrbController : EnemyController {

	public float speed;
	public GameObject player;
	public GameObject bullet;

	private Renderer re;
	private bool firing;

	void Start(){
		re = GetComponent<Renderer> ();
		firing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (re.IsVisibleFrom (Camera.main)) {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - speed);

			if (player.transform.position.x - (player.transform.localScale.x / 2) < transform.position.x &&
				player.transform.position.x + (player.transform.localScale.x / 2) > transform.position.x) {
				fireBullets ();
			}
		}
	}

	private void fireBullets(){
		if (!firing) {
			GameObject _bullet = GameObject.Instantiate (bullet);
			_bullet.transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 0.5f);
			_bullet.GetComponent<BulletController> ().setVelocity(0.0f, 0.0f, -10.0f);
			firing = true;

			StartCoroutine ("reload");
		}
	}

	private IEnumerator reload(){
		yield return new WaitForSeconds (0.05f);
		firing = false;
	}
}
