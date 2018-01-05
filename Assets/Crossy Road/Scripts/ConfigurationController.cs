using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationController : MonoBehaviour {

	public PlayerController pc = null;
	public Text instructions = null;
	public Text option = null;

	void Update() {
		if (Input.GetKey ("m")) {
			pc.mouseControl = true;
			pc.keyboardControl = false;
			instructions.text = "Click at point to move";
			option.text = "Press 'k' to change control to keyboard";
		}
		if (Input.GetKey ("k")) {
			pc.mouseControl = false;
			pc.keyboardControl = true;
			instructions.text = "Use arrow keys to move";
			option.text = "Press 'm' to change control to mouse";
		}
	}
}
