using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.05f);
	}
}
