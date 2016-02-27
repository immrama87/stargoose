using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour {

	protected Rigidbody rb;
	protected Renderer re;

	protected bool fired;

	private Vector3 velocity;
	private Vector3 force;

	protected void initComponents(){
		rb = GetComponent<Rigidbody> ();
		re = GetComponent<Renderer> ();
	}

	void Update(){
		if (velocity != Vector3.zero) {
			rb.velocity = velocity;
			customOnUpdate ();
		}
		if (!re.IsVisibleFrom (Camera.main)) {
			gracefulDestroy ();
		}
	}

	void FixedUpdate(){
		if (force != Vector3.zero && fired) {
			rb.AddForce (force);
			customOnFixedUpdate ();
		}

		if (!re.IsVisibleFrom (Camera.main)) {
			gracefulDestroy ();
		}
	}

	protected virtual void customOnUpdate (){}

	protected virtual void customOnFixedUpdate(){}

	protected virtual void gracefulDestroy (){}

	public void setVelocity(Vector3 _velocity){
		velocity = _velocity;
		force = Vector3.zero;
	}

	public void setVelocity(float x, float y, float z){
		velocity = new Vector3 (x, y, z);
		force = Vector3.zero;
	}

	public void setVelocity(float speed, float angle){
		float sin = Mathf.Sin (Mathf.Deg2Rad * angle);
		float x = speed * sin;

		float cos = Mathf.Cos (Mathf.Deg2Rad * angle);
		float z = speed * cos;

		velocity = new Vector3 (x, 0.0f, z);
		force = Vector3.zero;
	}

	public void setForce(Vector3 _force){
		force = _force;
		velocity = Vector3.zero;
	}

	public void setForce(float x, float y, float z){
		force = new Vector3 (x, y, z);
		velocity = Vector3.zero;
	}

	public void setForce(float change, float angle){
		float sin = Mathf.Sin (Mathf.Deg2Rad * angle);
		float x = change * sin;

		float cos = Mathf.Cos (Mathf.Deg2Rad * angle);
		float z = change * cos;

		force = new Vector3 (x, 0.0f, z);
		velocity = Vector3.zero;
	}
}
