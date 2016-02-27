using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int score;
	public int health;

	void Start(){}

	void Update(){}

	protected void explode(){
		animateExplosion ();
		Destroy (this.gameObject);
	}

	protected virtual void animateExplosion(){}

	public void updateHealth(int update){
		health += update;

		if (health <= 0) {
			explode ();
		}
	}
}
