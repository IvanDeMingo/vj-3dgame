using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	private static string UP = "up";
	private static string DOWN = "down";
	private static string LEFT = "left";
	private static string RIGHT = "right";

	public float moveDistance 			= 1;
	public float moveTime 				= 0.4f;
	public float colliderDistCheck 		= 1;
	public bool  isIdle 				= true;
	public bool  isDead 				= false;
	public bool  isMoving 				= false;
	public bool  isJumping 				= false;
	public bool  jumpStart 				= false;
	public bool  enableAngleCheckOnMove = false;
	public float angleCheck 			= 1;
	public float angleCheckDist 		= 0.5f;
	public double boxLimit = 1;

	public bool mouseControl = false;
	public bool keyboardControl = true;

	public ParticleSystem 	particle 		= null;
	public GameObject 		chick 			= null;
	private Renderer 		renderer 		= null;
	private bool 			isVisible 		= false;
	private string 			moveDirection 	= null;

	public AudioClip audioIdle1  = null;
	public AudioClip audioIdle2  = null;
	public AudioClip audioHop    = null;
	public AudioClip audioHit    = null;
	public AudioClip audioSplash = null;

	public ParticleSystem 	splash 				= null;
	public bool 			parentedToObject 	= false;

	void Start () {
		renderer = chick.GetComponent<Renderer> ();
	}

	void Update () {
		if (!Manager.instance.CanPlay ())
			return;

		if (isDead)
			return;
		
		CanIdle ();
		CanMove ();
		IsVisible ();
	}

	void CanIdle () {
		if (isIdle) {
			if (keyboardControl) {
				if (Input.GetKey (KeyCode.UpArrow))
					CheckIfIdle (270, 0, 0);
				if (Input.GetKey (KeyCode.DownArrow))
					CheckIfIdle (270, 180, 0);
				if (Input.GetKey (KeyCode.LeftArrow))
					CheckIfIdle (270, -90, 0);
				if (Input.GetKey (KeyCode.RightArrow))
					CheckIfIdle (270, 90, 0);
			}
			if (mouseControl) {
				if (Input.GetMouseButtonDown (0)) {
					Ray rayMouse = Camera.main.ScreenPointToRay (Input.mousePosition);
					Ray rayChicken = Camera.main.ScreenPointToRay (this.transform.position);
					Vector3 pointClick = rayMouse.origin;
					Vector3 pointPlayer = rayChicken.origin;
					if (pointClick.x < (pointPlayer.x - boxLimit) ) {
						Debug.Log("LEFT");
						moveDirection = LEFT;
						CheckIfIdle (270, -90, 0);
					} else if (pointClick.x > (pointPlayer.x + boxLimit) ) {
						Debug.Log("RIGHT");
						moveDirection = RIGHT;
						CheckIfIdle (270, 90, 0);
					} else if (pointClick.z > (pointPlayer.z + boxLimit) ) {
						Debug.Log("UP");
						moveDirection = UP;
						CheckIfIdle (270, 0, 0);
					} else if (pointClick.z < (pointPlayer.z - boxLimit)) {
						Debug.Log("DOWN");
						moveDirection = DOWN;
						CheckIfIdle (270, 180, 0);
					}
				
				}
			}
		}
	}

	void CheckIfIdle (float x, float y, float z) {
		chick.transform.rotation = Quaternion.Euler (x, y, z);

		if (enableAngleCheckOnMove) CheckIfCanMoveAngles ();
		else CheckIfCanMoveSingleRay ();

		int a = Random.Range (0, 12);
		if (a < 4) PlayAudioClip (audioIdle1);
	}

	void CheckIfCanMoveSingleRay () {
		// raycast - find if there's any collider box in front of player
		RaycastHit hit;
		Physics.Raycast (this.transform.position, -chick.transform.up, out hit, colliderDistCheck);
		//Debug.DrawRay (this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2);

		if (hit.collider == null) {
			SetMove ();
		} else {
			if (hit.collider.tag == "collider") {
				//Debug.Log ("Hit something with collider tag.");
				isIdle = true;
			} else {
				SetMove ();
			}
		}
	}

	void CheckIfCanMoveAngles () {
		RaycastHit hit, hitLeft, hitRight;

		Physics.Raycast (this.transform.position, -chick.transform.up, out hit, colliderDistCheck);
		Physics.Raycast (this.transform.position, -chick.transform.up + new Vector3 (angleCheck, 0, 0), out hitLeft, colliderDistCheck + angleCheckDist);
		Physics.Raycast (this.transform.position, -chick.transform.up + new Vector3 (-angleCheck, 0, 0), out hitRight, colliderDistCheck + angleCheckDist);

		Debug.DrawRay (this.transform.position, -chick.transform.up * colliderDistCheck, Color.red, 2);
		Debug.DrawRay (this.transform.position, (-chick.transform.up + new Vector3 (angleCheck, 0, 0)) * (colliderDistCheck + angleCheckDist), Color.green, 2);
		Debug.DrawRay (this.transform.position, (-chick.transform.up + new Vector3 (-angleCheck, 0, 0)) * (colliderDistCheck + angleCheckDist), Color.blue, 2);

		if (hit.collider == null && hitLeft.collider == null && hitRight.collider == null) {
			SetMove ();
		} else {
			if (hit.collider != null && hit.collider.tag == "collider") {
				//Debug.Log ("Hit something with collider tag.");
				isIdle = true;
			} else if (hitLeft.collider != null && hitLeft.collider.tag == "collider") {
				//Debug.Log ("Hit left something with collider tag.");
				isIdle = true;
			} else if (hitRight.collider != null && hitRight.collider.tag == "collider") {
				//Debug.Log ("Hit right something with collider tag.");
				isIdle = true;
			} else {
				SetMove ();
			}
		}
	}

	void SetMove () {
		//Debug.Log ("Hit nothing. Keep moving.");

		isIdle = false;
		isMoving = true;
		jumpStart = true;
	}

	void CanMove () {
		Debug.Log ("CanMove");
		if (isMoving) {
			if (keyboardControl) {
				if (Input.GetKeyUp (KeyCode.UpArrow)) {
					Moving (new Vector3 (transform.position.x, transform.position.y, transform.position.z + moveDistance));
					SetMoveForwardState ();
				} else if (Input.GetKeyUp (KeyCode.DownArrow)) {
					Moving (new Vector3 (transform.position.x, transform.position.y, transform.position.z - moveDistance));
				} else if (Input.GetKeyUp (KeyCode.LeftArrow)) {
					Moving (new Vector3 (transform.position.x - moveDistance, transform.position.y, transform.position.z));
				} else if (Input.GetKeyUp (KeyCode.RightArrow)) {
					Moving (new Vector3 (transform.position.x + moveDistance, transform.position.y, transform.position.z));
				}
			}
			if (mouseControl) {
				if (moveDirection == UP) {
					Moving (new Vector3 (transform.position.x, transform.position.y, transform.position.z + moveDistance));
					SetMoveForwardState ();
				} else if (moveDirection == DOWN) {
					Moving (new Vector3 (transform.position.x, transform.position.y, transform.position.z - moveDistance));
				} else if (moveDirection == LEFT) {
					Moving (new Vector3 (transform.position.x - moveDistance, transform.position.y, transform.position.z));
				} else if (moveDirection == RIGHT) {
					Moving (new Vector3 (transform.position.x + moveDistance, transform.position.y, transform.position.z));
				}
			}
		}
	}

	void Moving (Vector3 pos) {
		Debug.Log ("Moving");
		isIdle = false;
		isMoving = false;
		isJumping = true;
		jumpStart = false;
		PlayAudioClip (audioHop);
		LeanTween.move (this.gameObject, pos, moveTime).setOnComplete (MoveComplete);
	}

	void MoveComplete () {
		Debug.Log ("MoveComplete");
		isJumping = false;
		isIdle = true;

		int a = Random.Range (0, 12);
		if (a < 4) PlayAudioClip (audioIdle2);
	}

	void SetMoveForwardState () {
		Manager.instance.UpdateDistanceCount ();
	}

	void IsVisible () {
		if (renderer.isVisible) {
			isVisible = true;
		}
		if (!renderer.isVisible && isVisible) {
			//Debug.Log ("Player off screen. Apply GotHit()");
			GotHit ();
		}
	}

	public void GotHit () {
		isDead = true;
		ParticleSystem.EmissionModule em = particle.emission;
		em.enabled = true;

		PlayAudioClip (audioHit);

		Manager.instance.GameOver ();
	}

	public void GotSoaked () {
		isDead = true;
		ParticleSystem.EmissionModule em = splash.emission;
		em.enabled = true;

		PlayAudioClip (audioSplash);

		chick.SetActive (false);

		Manager.instance.GameOver ();
	}

	void PlayAudioClip (AudioClip clip) {
		this.GetComponent<AudioSource> ().PlayOneShot (clip);
	}
}
