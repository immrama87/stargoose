using UnityEngine;
using System.Collections;

public class MissileController : MonoBehaviour {

	public float speed;
	public Light engine;
	public GameObject player;

	private bool fired;
	private bool exploding;
	private Rigidbody rb;
	private Renderer re;
	private Vector3 engineOffset;

	// Use this for initialization
	void Start () {
		fired = false; 
		exploding = false;
		rb = GetComponent<Rigidbody> ();
		re = GetComponent<Renderer> ();
		engineOffset = this.transform.position - engine.transform.position;
		engine.gameObject.SetActive (false);
	}

	void FixedUpdate(){
		if (fired) {
			rb.AddForce (new Vector3 (0.0f, 0.0f, 1.0f * speed));

			engine.transform.position = this.transform.position - engineOffset;

			if (!re.IsVisibleFrom(Camera.main)) {
				StartCoroutine ("waitToDestroy");
			}
		}
	}

	void OnTriggerEnter(Collider other){
		bool stop = false;
		if (other.gameObject.CompareTag ("Enemy")) {
			if (player != null) {
				player.GetComponent<PlayerController> ().addScore (other.gameObject.GetComponent<EnemyController> ().score);
				other.gameObject.GetComponent<EnemyController> ().explode ();
			}
			stop = true;
		}

		if (stop) {
			Destroy (this.transform.parent.gameObject);
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
}
