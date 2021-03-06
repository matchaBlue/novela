﻿using UnityEngine;
using System.Collections;

public class chase : MonoBehaviour {

	private CharacterController cc;

	public Transform player;
	public Transform head;
	Animator anim;

	private HitboxScript hbs;

	string state = "patrol";
	public GameObject[] waypoints;
	int currentWP = 0;
	public float rotSpeed = 0.2f;
	public float speed = 1.5f;
	float accuracyWP = 0.5f;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		hbs = GetComponentInChildren<HitboxScript> ();

		cc = GetComponent<CharacterController> ();
		state = "patrol";
	}
	
	// Update is called once per frame
	void Update () {
		
		Vector3 direction = player.position - this.transform.position;
		direction.y = 0;
		float angle = Vector3.Angle (direction, head.transform.up);

		if (state == "patrol" && (waypoints.Length > 0)) {
			//Debug.Log ("Should be walking");
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isWalking", true);
			if (Vector3.Distance (waypoints [currentWP].transform.position, transform.position) < accuracyWP) {
				currentWP++;
				if (currentWP >= waypoints.Length)
					currentWP = 0;
			}

			//rotate towards waypoint
			direction = waypoints[currentWP].transform.position - transform.position;
			this.transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (direction),
				rotSpeed * Time.deltaTime);
			this.transform.Translate (0,0, Time.deltaTime * speed);
		}

		if (Vector3.Distance (player.position, this.transform.position) < 10 && (angle < 30 || state == "pursuing")) {
			state = "pursuing";
			this.transform.rotation = Quaternion.Slerp (this.transform.rotation, Quaternion.LookRotation (direction), 0.1f);

			if (direction.magnitude > 5) {
				this.transform.Translate (0, 0, Time.deltaTime * speed);
				anim.SetBool ("isWalking", true);
				anim.SetBool ("isAttacking", false);
			} else {
				anim.SetBool ("isAttacking", true);
				anim.SetBool ("isWalking", false);
			}
				
		} else {
			anim.SetBool ("isIdle", false);
			anim.SetBool ("isWalking", true);
			state = "patrol";
		}

	}

	void ActivateSwordCollider(){
		hbs.ActivateSwordCollider ();
	}

	void DeactivateSwordCollider(){
		hbs.DeactivateSwordCollider ();
	}
}
