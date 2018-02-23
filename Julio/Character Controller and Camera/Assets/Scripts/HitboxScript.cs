using UnityEngine;
using System.Collections;

public class HitboxScript : MonoBehaviour {

	Transform parent;
	// Use this for initialization
	void Start () {
		parent = GetComponentInParent<Transform> ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.transform.position;
	}
}
