using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	Vector3 offset;
	public Transform player;
	public Transform pivot;

	float mouseY;
	float rotLimiter = 0f;
	public float minView = -45f;
	public float maxView = 30f;
	public float rotSpeed = 10f;

	void Start () {
		offset = player.position - transform.position;
	}

	void LateUpdate () {
		//rotate cam and keep it locked to player
		pivot.position = player.position;
		pivot.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * rotSpeed * Time.deltaTime);

		//camera should rotate around pivot, but face player
		Quaternion rot = Quaternion.Euler(new Vector3(0f, pivot.eulerAngles.y, 0f));
		transform.position = player.position - (rot * offset);

		//grabs Y value
		mouseY = Input.GetAxis("Mouse Y");
		//then stores as rotLim, but allowes us to lmit rotLim w/o changing Input val
		rotLimiter -= mouseY * rotSpeed * Time.deltaTime;
		rotLimiter = Mathf.Clamp(rotLimiter, minView, maxView);

		transform.localEulerAngles = new Vector3(-rotLimiter, transform.localEulerAngles.y, 0f);

		transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * rotSpeed * Time.deltaTime);
	}
}
