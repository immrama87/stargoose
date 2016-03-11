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

		bool kill = true;

		if (other.gameObject.CompareTag ("Missile Refill") ||
				other.gameObject.CompareTag("Tunnel Entrance") ||
				other.gameObject.CompareTag("Tunnel Exit")) {
			kill = false;
		}

		if (kill) {
			Destroy (this.gameObject);
		}
	}

	protected override void gracefulDestroy(){
		Destroy (this.gameObject);
	}
}
