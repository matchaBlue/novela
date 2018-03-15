using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform pivot;
	public Transform player;
	Vector3 offset;

	public float minViewAngle = -45f;
	public float maxViewAngle = 10f;
	float rotSpeed = 10f;
	void Start () {
		offset = player.position - transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//rotate camera AND pivot based on mouseY ONLY for now, set limiters on X later
		pivot.position = player.position;
		float horizon = Input.GetAxis("Mouse X") * rotSpeed;
		float vert = Input.GetAxis("Mouse Y") * rotSpeed;

		pivot.Rotate(0, horizon, 0);
		pivot.Rotate(vert, 0, 0);

		float desiredYAngle = pivot.eulerAngles.y;
		float desiredXAngle = pivot.eulerAngles.x;

		desiredXAngle -= vert * rotSpeed * Time.deltaTime;
		desiredXAngle = Mathf.Clamp (desiredXAngle, minViewAngle, maxViewAngle);

		Quaternion camRot = Quaternion.Euler(desiredXAngle, desiredYAngle, 0f);

		transform.position = pivot.transform.position - (camRot * offset);

		transform.LookAt(pivot.transform.position);
	}
}
