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

	public bool firstPerson = false;

	Vector3 offset;
	public float foffset;

	private Vector3 direction = Vector3.zero; // (0, 0, 0)

	void Start () {
		controller = GetComponent<CharacterController> ();
		camera = GetComponentInChildren<Camera> ();

		if (firstPerson) {
			//use first transform
			camera.transform.localPosition = new Vector3 (0, cameraHeight, 0);
		} else {
			//use third
			offset = transform.position - camera.transform.position;
			camera.transform.position -= offset;
		}

		camera.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up);
	}
	

	void Update () {

		// get input
		Vector3 moveInput = (transform.forward * Input.GetAxis ("Vertical") +
		                    transform.right * Input.GetAxis ("Horizontal")) * speed;
		float mouseX = Input.GetAxis ("Mouse X");
		float mouseY = -Input.GetAxis ("Mouse Y");

		//move player
		direction.x = moveInput.x;
		direction.z = moveInput.z;
		if (moveInput.magnitude > 0) {
			//transform.TransformDirection(camera.transform.forward);
			transform.rotation = Quaternion.LookRotation (camera.transform.forward);
			camera.transform.rotation = Quaternion.LookRotation (transform.forward, transform.up);

		}
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
	    camera.transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0);
		camera.transform.Rotate(mouseY * sensitivity * Time.deltaTime, 0, 0);

		camera.transform.position = transform.position - (camera.transform.forward * foffset); 

	}
}
