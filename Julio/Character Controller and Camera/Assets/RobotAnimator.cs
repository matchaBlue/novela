using UnityEngine;
using System.Collections;

public class RobotAnimator : MonoBehaviour {

	private Animator anim;

	void Start () {
		anim = GetComponent<Animator> ();
		anim.SetBool("isIdle", true);
		anim.SetBool("isWalking", false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
