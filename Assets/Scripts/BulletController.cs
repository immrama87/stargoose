using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour {

	public float speed;
	public GameObject player;

	private Rigidbody rb;
	private Renderer re;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		re = GetComponent<Renderer> ();
		rb.velocity = new Vector3 (0.0f, 0.0f, speed);
	}

	void Update(){
		if (!re.IsVisibleFrom (Camera.main)) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Enemy")) {
			EnemyController ec = other.gameObject.GetComponent<EnemyController> ();
			ec.health -= 5;

			if (ec.health <= 0) {
				if (player != null) {
					player.GetComponent<PlayerController> ().addScore (ec.score);
				}
				ec.explode ();
			}
		}

		if (other.gameObject.CompareTag ("Player")) {
			PlayerController pc = other.gameObject.GetComponent<PlayerController> ();
			pc.updateShields (-5.0f);
		}

		if (!other.gameObject.CompareTag ("Missile Refill")) {
			Destroy (this.gameObject);
		}
	}

	public void setSpeed(float s){
		this.speed = s;
	}
}
