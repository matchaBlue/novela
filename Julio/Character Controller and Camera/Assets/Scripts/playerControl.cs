using UnityEngine;
using System.Collections;

public class playerControl : MonoBehaviour {

	private CharacterController controller;
	private Camera camera;

	public float speed;
	public float jumpSpeed;
	public float sensitivity;
	public float cameraHeight;
	public float gravity;
	public float minViewAngle = -45f;
	public float maxViewAngle = 10f;

	private float rotationY = 0f;

	Vector3 offset;
	public float foffset;

	private Vector3 direction = Vector3.zero; // (0, 0, 0)

	void Start () {
		controller = GetComponent<CharacterController> ();
		camera = GetComponentInChildren<Camera> ();


		//use first transform
		camera.transform.localPosition = new Vector3 (0, cameraHeight, 0);
		

		camera.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up);
	}
	

	void Update () {

		// get input
		Vector3 moveInput = (transform.forward * Input.GetAxis ("Vertical") +
		                    transform.right * Input.GetAxis ("Horizontal")) * speed;
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");

		transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0f);

		//move player
		direction.x = moveInput.x;
		direction.z = moveInput.z;

		if (controller.isGrounded) {
			//if grounded
			if (Input.GetKey (KeyCode.Space)) {
				direction.y = jumpSpeed;
			} else {
				direction.y = 0;
			}
		}
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
