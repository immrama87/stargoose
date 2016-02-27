using UnityEngine;
using System.Collections;

public class RocketTurretController : TurretController {

	// Use this for initialization
	void Start () {
		initTurret ();
	}
	
	// Update is called once per frame
	void Update () {
		float angle = calculateAngle ();
	}
}
