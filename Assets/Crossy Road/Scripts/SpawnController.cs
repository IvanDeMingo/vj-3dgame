using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour {
	public bool goLeft = false;
	public bool goRight = false;
	public bool both = false;

	public List<GameObject> items = new List<GameObject> ();
	public List<Spawner> spawnersLeft = new List<Spawner> ();
	public List<Spawner> spawnersRight = new List<Spawner> ();

	void Start () {
		int itemId = Random.Range (0, items.Count);
		GameObject item = items [itemId];

		int direction = Random.Range (0, 2);

		if (both) {
			goLeft = true;
			goRight = true;
		} else if (direction > 0) {
			goLeft = false;
			goRight = true;
		} else {
			goLeft = true;
			goRight = false;
		}

		for (int i = 0; i < spawnersLeft.Count; i++) {
			spawnersLeft [i].item = item;
			spawnersLeft [i].goLeft = goLeft;
			spawnersLeft [i].gameObject.SetActive (goRight);
			spawnersLeft [i].spawnLeftPos = spawnersLeft [i].transform.position.x;
		}

		for (int i = 0; i < spawnersRight.Count; i++) {
			spawnersRight [i].item = item;
			spawnersRight [i].goLeft = goLeft;
			spawnersRight [i].gameObject.SetActive (goLeft);
			spawnersRight [i].spawnRightPos = spawnersRight [i].transform.position.x;
		}
	}
}
