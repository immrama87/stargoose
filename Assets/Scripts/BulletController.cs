using UnityEngine;
using System.Collections;

public class BulletController : ProjectileController {
	
	public GameObject player;

	// Use this for initialization
	void Start () {
		initComponents ();
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyController ec = other.gameObject.GetComponent<EnemyController> ();
			ec.updateHealth (-5);
		}

		if (other.gameObject.CompareTag ("Player")) {
			PlayerController pc = other.gameObject.GetComponent<PlayerController> ();
			pc.updateShields (-5.0f);
		}

		if (!other.gameObject.CompareTag ("Missile Refill")) {
			Destroy (this.gameObject);
		}
	}

	protected override void gracefulDestroy(){
		Destroy (this.gameObject);
	}
}
