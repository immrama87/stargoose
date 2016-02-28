using UnityEngine;
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

	public float maxZSpeed;
	public float minZSpeed;
	public float baseZSpeed;

	public Camera mainCamera;
	public GameObject walls;

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

	private float zSpeed;
	private Vector3 cameraOffset;

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
		updateShields (0.0f);
		maxAmmo = ammo = 800.0f;
		updateAmmoText ();
		zSpeed = baseZSpeed;

		cameraOffset = new Vector3 (0.0f, 
			this.transform.position.y - mainCamera.transform.position.y,
			this.transform.position.z - mainCamera.transform.position.z);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("left ctrl")) {
			if (!left) {
				if (missiles > 0) {
					lMissile = GameObject.Instantiate (missileParent);
					MissileController mc = lMissile.GetComponentInChildren<MissileController> ();
					mc.player = this.gameObject;
					mc.setForce (0.0f, 0.0f, 1.0f * mc.speed);
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
					MissileController mc = rMissile.GetComponentInChildren<MissileController> ();
					mc.player = this.gameObject;
					mc.setForce (0.0f, 0.0f, 1.0f * mc.speed);
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

		float speedChange = Input.GetAxis ("Vertical");
		if (speedChange > 0) {
			zSpeed += 0.002f;
			if (zSpeed >= maxZSpeed) {
				zSpeed = maxZSpeed;
			}
		} else if (speedChange < 0) {
			zSpeed -= 0.002f;
			if (zSpeed <= minZSpeed) {
				zSpeed = minZSpeed;
			}
		} else {
			if (zSpeed > baseZSpeed) {
				zSpeed -= 0.002f;
			} else if (zSpeed < baseZSpeed) {
				zSpeed += 0.002f;
			}
		}

		score += Mathf.RoundToInt(10 * (zSpeed / baseZSpeed));
		updateScoreText ();

		fuel-= Mathf.RoundToInt(1 * (zSpeed / baseZSpeed));
		updateFuelText ();

		this.transform.position = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + zSpeed);

		mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, this.transform.position.z - cameraOffset.z);;
		walls.transform.position = new Vector3(walls.transform.position.x, walls.transform.position.y, this.transform.position.z);
	}

	void FixedUpdate(){
		float xSpeed = Input.GetAxis ("Horizontal");
		Vector3 movement = new Vector3 (xSpeed * speed, 0.0f, 0.0f);
		rb.AddForce (movement, ForceMode.Impulse);

		if (lMissile != null) {
			lMissile.transform.position = new Vector3 (this.transform.position.x-0.2f, 0.2f, this.transform.position.z);
		}
		if (rMissile != null) {
			rMissile.transform.position = new Vector3 (this.transform.position.x + 0.2f, 0.2f, this.transform.position.z);
		}

		spotLight.transform.position = this.transform.position - lightOffset;
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Missile Refill")) {
			missiles = 6;
			writeMissileText ();
		}

		if (other.gameObject.CompareTag ("Missile Refill Wall")) {
			updateShields (maxShields * -1); 
		}

		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyController ec = other.GetComponent(typeof(EnemyController)) as EnemyController;

			updateShields (ec.health * -1.0f);
			addScore (ec.score);
			ec.updateHealth (ec.health * -1);
		}
	}

	public float getZSpeed(){
		return zSpeed;
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

	public void updateShields(float update){
		shields += update;
		if (shields <= 0) {
			shields = 0;
			explode ();
		}
		shieldText.text = "Shields: " + Mathf.Round ((shields / maxShields) * 100).ToString () + "%";
	}

	public void kill(){
		updateShields (maxShields * -1);
	}

	void updateAmmoText(){
		ammoText.text = "Ammo: " + Mathf.Round ((ammo / maxAmmo) * 100).ToString () + "%";
	}

	void fireBullets(){
		if (!firing) {
			if (ammo >= 2) {
				GameObject lBullet = GameObject.Instantiate (bullet);
				lBullet.transform.position = new Vector3 (this.transform.position.x - 0.1f, this.transform.position.y, this.transform.position.z + 0.5f);
				lBullet.GetComponent<BulletController> ().player = this.gameObject;
				lBullet.GetComponent<BulletController> ().setVelocity (0.0f, 0.0f, 10.0f);
				GameObject rBullet = GameObject.Instantiate (bullet);
				rBullet.transform.position = new Vector3 (this.transform.position.x + 0.1f, this.transform.position.y, this.transform.position.z + 0.5f);
				rBullet.GetComponent<BulletController> ().player = this.gameObject;
				rBullet.GetComponent<BulletController> ().setVelocity (0.0f, 0.0f, 10.0f);
				ammo -= 2;
				updateAmmoText ();
				firing = true;
				StartCoroutine ("reload");
			}
		}
	}

	void explode(){
		this.gameObject.SetActive (false);
		spotLight.gameObject.SetActive (false);
	}

	IEnumerator reload(){
		yield return new WaitForSeconds (0.05f);
		firing = false;
	}
}
