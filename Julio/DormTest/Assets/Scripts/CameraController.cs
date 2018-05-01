using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 offset;
	public PlayerController player;
	public Transform pivot;

	float mouseY;
	public float minView = -10f;
	public float maxView = 30f;
	public float rotSpeed = 20f;

	void Start () {
		offset = player.transform.position - transform.position;
		//Cursor.lockState = CursorLockMode.Locked;

	}

	void LateUpdate () {
		//rotate cam and keep it locked to player
		pivot.position = player.transform.position;

		float horizontal = Input.GetAxis("Mouse X") * rotSpeed * Time.deltaTime;
		pivot.Rotate(0, horizontal, 0);

		float vertical = Input.GetAxis("Mouse Y") * rotSpeed * Time.deltaTime;
		pivot.Rotate(-vertical, 0, 0);

		if(pivot.rotation.eulerAngles.x > maxView && pivot.rotation.eulerAngles.x < 180f)
			pivot.rotation = Quaternion.Euler(maxView, pivot.eulerAngles.y, pivot.eulerAngles.z);

		if(pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minView)
			pivot.rotation = Quaternion.Euler(360f + minView, pivot.eulerAngles.y, pivot.eulerAngles.z);

		//rotate cam based on current rotation of pivot & original offset
		float desiredYAngle = pivot.eulerAngles.y;
		float desiredXAngle = pivot.eulerAngles.x;

		//camera should rotate around pivot, but face player
		Quaternion rot = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
		transform.position = player.transform.position - (rot * offset);

		//player.transform.rotation = Quaternion.Euler(new Vector3(0f, pivot.eulerAngles.y, 0f));


		//if(transform.position.y < pivot.position.y)
			//transform.position = new Vector3(transform.position.x, pivot.position.y - .5f, pivot.position.z);


		
		transform.LookAt(pivot);
		/*
		transform.localEulerAngles = new Vector3(-rotLimiter, transform.localEulerAngles.y, 0f);

		transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * rotSpeed * Time.deltaTime);
		transform.LookAt(pivot.transform.forward);
		*/
	}

}
