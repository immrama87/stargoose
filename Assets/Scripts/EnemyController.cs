using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	public int score;
	public int health;

	void Start(){}

	void Update(){}

	public void explode(){
		animateExplosion ();
		Destroy (this.gameObject);
	}

	protected virtual void animateExplosion(){}
}
