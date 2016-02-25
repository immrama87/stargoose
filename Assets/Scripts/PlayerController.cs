﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public GameObject missileParent;
	public GameObject bullet;
	public Light spotLight;
	public Text missileText;
	public Text scoreText;
	public Text fuelText;
	public Text ammoText;
	public Text shieldText;

	private Rigidbody rb;

	private GameObject missile;
	private int missiles;
	private bool left;
	private bool right;
	private GameObject lMissile, rMissile;

	private Vector3 lightOffset;

	private int score;

	private float maxFuel, fuel;
	private float maxShields, shields;

	private float maxAmmo, ammo;
	private bool firing;

	// Use this for initialization
	void Start () {
		lMissile = rMissile = null;
		firing = false;
		rb = GetComponent<Rigidbody> ();
		missiles = 6;
		writeMissileText ();
		lightOffset = this.transform.position - spotLight.transform.position;
		score = 0;
		updateScoreText ();

		maxFuel = fuel = 10000.0f;
		updateFuelText ();
		maxShields = shields = 500.0f;
		updateShieldsText ();
		maxAmmo = ammo = 800.0f;
		updateAmmoText ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("left ctrl")) {
			if (!left) {
				if (missiles > 0) {
					lMissile = GameObject.Instantiate (missileParent);
					lMissile.GetComponentInChildren<MissileController> ().player = this.gameObject;
					lMissile.transform.position = new Vector3 (this.transform.position.x - 0.2f, 0.2f, 0.0f);
					left = true;
					missiles--;
					writeMissileText ();
				}
			} else {
				lMissile.GetComponentInChildren<MissileController> ().fire();
				lMissile = null;
				left = false;
			}
		}
		if (Input.GetKeyDown ("left alt")) {
			if (!right) {
				if (missiles > 0) {
					rMissile = GameObject.Instantiate (missileParent);
					rMissile.GetComponentInChildren<MissileController> ().player = this.gameObject;
					rMissile.transform.position = new Vector3 (this.transform.position.x + 0.2f, 0.2f, 0.0f);
					right = true;
					missiles--;
					writeMissileText ();
				}
			} else {
				rMissile.GetComponentInChildren<MissileController> ().fire();
				rMissile = null;
				right = false;
			}
		}
		if (Input.GetKey ("space")) {
			fireBullets ();
		}

		score += 10;
		updateScoreText ();

		fuel-=1;
		updateFuelText ();
	}

	void FixedUpdate(){
		float xSpeed = Input.GetAxis ("Horizontal");
		Vector3 movement = new Vector3 (xSpeed * speed, 0.0f, 0.0f);
		rb.AddForce (movement, ForceMode.Impulse);

		if (lMissile != null) {
			lMissile.transform.position = new Vector3 (this.transform.position.x-0.2f, 0.2f, 0.0f);
		}
		if (rMissile != null) {
			rMissile.transform.position = new Vector3 (this.transform.position.x + 0.2f, 0.2f, 0.0f);
		}

		spotLight.transform.position = this.transform.position - lightOffset;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Missile Refill")) {
			missiles = 6;
			writeMissileText ();
		}

		if (other.gameObject.CompareTag ("Missile Refill Wall")) {
			missileText.text = "You crashed...";
		}

		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyController ec = other.GetComponent<EnemyController> ();
			shields -= ec.health;
			ec.explode ();
			if (shields <= 0) {
				shields = 0;
			}

			updateShieldsText ();
		}
	}

	void writeMissileText(){
		missileText.text = "Missiles: " + missiles.ToString ();
	}

	public void addScore(int _score){
		score += _score;
		updateScoreText ();
	}

	void updateScoreText(){
		scoreText.text = score.ToString ();
	}

	void updateFuelText(){
		fuelText.text = "Fuel: " + Mathf.Round ((fuel / maxFuel) * 100).ToString () + "%";
	}

	void updateShieldsText(){
		shieldText.text = "Shields: " + Mathf.Round ((shields / maxShields) * 100).ToString () + "%";
	}

	void updateAmmoText(){
		ammoText.text = "Ammo: " + Mathf.Round ((ammo / maxAmmo) * 100).ToString () + "%";
	}

	void fireBullets(){
		if (!firing) {
			if (ammo >= 2) {
				GameObject lBullet = GameObject.Instantiate (bullet);
				lBullet.transform.position = new Vector3 (this.transform.position.x - 0.05f, this.transform.position.y, this.transform.position.z + 0.25f);
				lBullet.GetComponent<BulletController> ().player = this.gameObject;
				GameObject rBullet = GameObject.Instantiate (bullet);
				rBullet.transform.position = new Vector3 (this.transform.position.x + 0.05f, this.transform.position.y, this.transform.position.z + 0.25f);
				rBullet.GetComponent<BulletController> ().player = this.gameObject;
				ammo -= 2;
				updateAmmoText ();
				firing = true;
				StartCoroutine ("reload");
			}
		}
	}

	IEnumerator reload(){
		yield return new WaitForSeconds (0.05f);
		firing = false;
	}
}
