using UnityEngine;
using System.Collections;

public class HitboxScript : MonoBehaviour {

	public Animator anim;
	private BoxCollider sword;

	// Use this for initialization
	void Start () {
		anim = GetComponentInParent<Animator> ();
		sword = GetComponent<BoxCollider> ();
		sword.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.CompareTag ("Player")) {
			Debug.Log ("Hit");
		}
	}

	public void ActivateSwordCollider(){
		sword.enabled = true;
	}

	public void DeactivateSwordCollider(){
		sword.enabled = false;
	}
}
