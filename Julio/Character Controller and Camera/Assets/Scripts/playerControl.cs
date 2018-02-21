﻿using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	private CharacterController controller;
	private Camera camera;

	public float speed;
	public float jumpSpeed;
	public float slideSpeed;
	public float sensitivity;
	public float cameraHeight;
	public float gravity;
	public float minViewAngle = -45f;
	public float maxViewAngle = 10f;

	private float rotationY = 0f;

	Vector3 offset;
	public float foffset;
	private RaycastHit hit;
	float distToGround = 5f;
	float theta;

	Vector3 normal;

	private Vector3 direction = Vector3.zero; // (0, 0, 0)

	void Start () {
		controller = GetComponent<CharacterController> ();
		camera = GetComponentInChildren<Camera> ();


		//use first transform
		camera.transform.localPosition = new Vector3 (0, cameraHeight, 0);
		

		camera.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up);

	}
	


	bool isSliding = false;

	void Update () {

		if (Physics.Raycast (transform.position, -transform.up, out hit, distToGround)) {
			normal = hit.normal;
			theta = Vector3.Angle (normal, transform.up);
		}

		if (theta > 45f) {
			isSliding = true;
		} else {
			isSliding = false;
		}
		
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");

		if (!isSliding) {
			// get input
			Vector3 moveInput = (transform.forward * Input.GetAxis ("Vertical") +
				transform.right * Input.GetAxis ("Horizontal")) * speed;
			
			transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0f);

			//move player
			direction.x = moveInput.x;
			direction.z = moveInput.z;

			//on ground
			if (controller.isGrounded) {
				if (Input.GetKey (KeyCode.Space)) {
					direction.y = jumpSpeed;
				} else {
					direction.y = 0;
				}
			}
			
		} else {
			//sliding
			Vector3 into = Vector3.Cross(transform.up, normal);
			Vector3 downhill = Vector3.Cross (into, normal);
			downhill = downhill.normalized;
			direction = downhill * slideSpeed;
		}

		/*
		if (controller.isGrounded) {
			
			if (Physics.Raycast (transform.position, -transform.up, hit, distToGround)) {
				normal = hit.normal;
				theta = Vector3.Angle (normal, transform.up);
				//if the angle of the slope is greater than 45f
				if (theta > 45f) {
					//slide down
					Vector3 into = Vector3.Cross(transform.up, normal);
					Vector3 downhill = Vector3.Cross (into, normal);
					downhill = downhill.normalized;
					direction = downhill * slideSpeed;

				} else {
					//jump

					direction.y = 0;
				}
			}
		}
		*/

		controller.Move (direction * Time.deltaTime);
		direction.y -= gravity * Time.deltaTime;

		//look
	    //camera.transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0);

	    //Rotates camera between given angles
		rotationY -= mouseY * sensitivity * Time.deltaTime;
		rotationY = Mathf.Clamp (rotationY, minViewAngle, maxViewAngle);
			
		camera.transform.localEulerAngles = new Vector3(-rotationY, camera.transform.localEulerAngles.y, 0);


		camera.transform.position = transform.position - (camera.transform.forward * foffset); 

	}
}
