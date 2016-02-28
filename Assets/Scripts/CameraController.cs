using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public GameObject player;
	public float animationLength;

	public AnimationCurve offsetCurve;
	public AnimationCurve pitchCurve;

	private Vector3 offset;
	private Vector3 baseOffset;
	private Vector3 tightOffset;
	private Vector3 wideOffset;

	private Vector3 tightRotation;
	private Vector3 wideRotation;
	private Vector3 baseRotation;

	private Renderer playerRenderer;

	private float animationTimeStamp;
	private bool animating;
	private bool reverse;

	// Use this for initialization
	void Start () {
		offset = player.transform.position - transform.position;
		baseOffset = offset;
		wideOffset = offset;
		tightOffset = new Vector3 (offset.x, offset.y + 7.5f, offset.z + 3.5f);

		baseRotation = wideRotation = transform.localEulerAngles;
		tightRotation = new Vector3 (wideRotation.x - 90.0f, wideRotation.y, wideRotation.z);

		playerRenderer = player.GetComponent<Renderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (animating) {
			animationTimeStamp += Time.deltaTime;
			animate ();
		}
		if (playerRenderer.enabled) {
			transform.position = new Vector3 (transform.position.x, player.transform.position.y - offset.y, player.transform.position.z - offset.z);
		} else {
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + player.GetComponent<PlayerController> ().getZSpeed ());
		}
	}

	public void startAnimation(bool _reverse = false){
		animationTimeStamp = 0.0f;
		animating = true;
		reverse = _reverse;
	}

	private void animate(){
		float timePercent = animationTimeStamp / animationLength;
		if (reverse) {
			timePercent = 1 - timePercent;
		}
		float offsetPercent = offsetCurve.Evaluate (timePercent);
		float rotation = pitchCurve.Evaluate (timePercent);

		offset = baseOffset - ((baseOffset - tightOffset) * offsetPercent);
		transform.localEulerAngles = new Vector3 (rotation, 0.0f, 0.0f);
	}
}
