using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int score;
	public int health;

	private bool exploded;

	void Start(){
		exploded = false;
	}

	void Update(){
		if (exploded) {
			Destroy (this.gameObject);
		}
	}

	public void explode(){
		exploded = true;
	}
}
