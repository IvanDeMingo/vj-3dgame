using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
	public float speed = 1.0f;
	public float moveDirection = 0;
	public bool parentOnTrigger = true;
	public bool hitBoxOnTrigger = false;
	public GameObject moverObject = null;

	private Renderer renderer = null;
	private bool isVisible = false;

	void Start () {
		renderer = moverObject.GetComponent<Renderer> ();
	}

	void Update () {
		this.transform.Translate (speed * Time.deltaTime, 0, 0);

		IsVisible ();
	}

	void IsVisible () {
		if (renderer.isVisible) {
			isVisible = true;
		}

		if (!renderer.isVisible && isVisible) {
			//Debug.Log ("Remove object. No longer seen by camera.");
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			//Debug.Log ("Enter.");

			if (parentOnTrigger) {
				//Debug.Log ("Enter: Parent to me");
				other.transform.parent = this.transform;

				PlayerController pc = other.GetComponent<PlayerController> ();
				pc.enableAngleCheckOnMove = true;
				pc.parentedToObject = true;
			}

			if (hitBoxOnTrigger) {
				//Debug.Log ("Enter: Gothit. Game over.");
				other.GetComponent<PlayerController> ().GotHit ();
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			if (parentOnTrigger) {
				//Debug.Log ("Exit.");
				other.transform.parent = null;

				PlayerController pc = other.GetComponent<PlayerController> ();
				pc.enableAngleCheckOnMove = false;
				pc.parentedToObject = false;
			}
		}
	}
}
