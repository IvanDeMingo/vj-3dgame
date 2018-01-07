using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
	public List<GameObject> platform0 = new List<GameObject> ();
	public List<GameObject> platform1 = new List<GameObject> ();
	public List<GameObject> platform2 = new List<GameObject> ();
	public List<float> height0 = new List<float> ();
	public List<float> height1 = new List<float> ();
	public List<float> height2 = new List<float> ();

	private float lastPos = 0;
	private float lastScale = 0;

	public void RandomGenerator (int level) {
		int i;
		GameObject obj;
		float h;

		switch (level) {
		case 0:
			i = Random.Range (0, platform0.Count);
			obj = platform0 [i];
			h = height0 [i];
			break;
		case 1:
			i = Random.Range (0, platform1.Count);
			obj = platform1 [i];
			h = height1 [i];
			break;
		default:
			i = Random.Range (0, platform2.Count);
			obj = platform2 [i];
			h = height2 [i];
			break;
		}

		CreateLevelObject (obj, h);
	}

	public void CreateLevelObject (GameObject obj, float height) {
		GameObject go = Instantiate (obj) as GameObject;

		float offset = lastPos + (lastScale * 0.5f);
		offset += (go.transform.localScale.z) * 0.5f;
		Vector3 pos = new Vector3 (0, height, offset);

		go.transform.position = pos;

		lastPos = go.transform.position.z;
		lastScale = go.transform.localScale.z;

		go.transform.parent = this.transform;
	}
}
