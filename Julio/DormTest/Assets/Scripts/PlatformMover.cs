using UnityEngine;
using System.Collections;

public class PlatformMover : MonoBehaviour {

	float minHeight = 0f, maxHeight = 10f, moveHeight = 0f;

	void Start(){
		//sets it to 0
		transform.position = new Vector3(transform.position.x, minHeight, transform.position.z);
	}

	void Update(){
		transform.position = new Vector3(transform.position.x, Vector3.Lerp(Vector3.zero, Vector3.up * maxHeight, Time.deltaTime).y, transform.position.z);
	}
}
