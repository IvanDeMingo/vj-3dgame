using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killbox : MonoBehaviour {
	public bool enableKillbox = true;

	void OnTriggerEnter (Collider other) {
		if (enableKillbox) {
			if (other.tag == "Player") {
				other.GetComponent<PlayerController> ().GotHit ();
			}
		}
	}
}
