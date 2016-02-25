using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	public float maxSpeed;
	public float minSpeed;
	public float baseSpeed;

    private float speed;

    void Start()
    {
        speed = baseSpeed;
    }

	// Update is called once per frame
	void Update () {
		float speedChange = Input.GetAxis ("Vertical");
		if (speedChange > 0) {
			speed += 0.002f;
			if (speed >= maxSpeed) {
				speed = maxSpeed;
			}
		} else if (speedChange < 0) {
			speed -= 0.002f;
			if (speed <= minSpeed) {
				speed = minSpeed;
			}
		} else {
			if (speed > baseSpeed) {
				speed -= 0.002f;
			} else if (speed < baseSpeed) {
				speed += 0.002f;
			}
		}
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - speed);
	}
}
