using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	PlayerController player;
	public CameraController cam;
	private Animator anim;
	public float rotSpeed = 15f;

	void Start(){
		player = GetComponentInParent<PlayerController>();
		anim = GetComponentInParent<Animator>();
	}

	void Update(){
		if(Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f){
				Quaternion lookRot = Quaternion.LookRotation(new Vector3(player.moveInput.x, 0f, player.moveInput.z));
				transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotSpeed * Time.deltaTime);
			}
	}

	void setAnim(string state){
		//innefficient, probably should put them in an array and just find the one state to set to true lol
		//but this saves having to parse through an array so 
		switch(state){
			case "isIdle":
				//set idle true, all others false
				anim.SetBool("isIdle", true);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				break;
			case "isRunning":
				//set running true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", true);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				break;
			case "isWalking":
				//set walking to true, all else to false (there has to be a better way -_-)
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", true);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				break;
			case "isJumping":
				//set jumping true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", true);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				break;
			case "isGrabbing":
				//set grabbing true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", true);
				anim.SetBool("isClimbing", false);
				break;
			case "isClimbing":
				//set getUp true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", true);
				break;
			case "isSliding":
				//set sliding anim to be true
				//no slide anim yet
				Debug.Log("isSliding");
				break;
			default:
				Debug.Log("Wrong input");
				break;
		}
	}
}
