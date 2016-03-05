using UnityEngine;
using System.Collections;

public class MissileController : ProjectileController {

	public float speed;
	public Light engine;
	public GameObject player;

	private Vector3 engineOffset;

	// Use this for initialization
	void Start () {
		initComponents ();
		engineOffset = this.transform.position - engine.transform.position;
		engine.gameObject.SetActive (false);
	}

	protected override void customOnFixedUpdate(){
		engine.transform.position = this.transform.position - engineOffset;
	}

	protected override void gracefulDestroy(){
		StartCoroutine ("waitToDestroy");
	}

	void OnTriggerEnter(Collider other){
		if (fired) {
			bool stop = false;
			if (other.gameObject.CompareTag ("Enemy")) {
				if (player != null) {
					EnemyController ec = other.gameObject.GetComponent<EnemyController> ();
					player.GetComponent<PlayerController> ().addScore (ec.score);
					ec.updateHealth (ec.health * -1);
				}
				stop = true;
			}

			if (other.gameObject.CompareTag ("Player")) {
				if (player == null) {
					other.gameObject.GetComponent<PlayerController> ().kill ();
					stop = true;
				}
			}

			if (stop) {
				Destroy (this.transform.parent.gameObject);
			}
		}
	}

	IEnumerator waitToDestroy(){
		re.enabled = false;
		yield return new WaitForSeconds(0.4f);
		Destroy (this.transform.parent.gameObject);
	}

	public void fire(){
		fired = true;
		engine.gameObject.SetActive (true);
	}

	public float getMass(){
		rb = GetComponentInChildren<Rigidbody> ();
		return rb.mass;
	}
}
