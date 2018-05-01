using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

	PlayerController player;
	private Animator anim;
	public float rotSpeed = 15f;
	Vector3 modelRot;
	float inX, inZ;

	public enum State {movement, grab, getUp};

	void Start(){
		player = GetComponentInParent<PlayerController>();
		anim = GetComponentInParent<Animator>();
		inX = inZ = 0;
	}

	void Update(){

		switch((State)player.getCurrState()){
			case State.movement:
				movementAnim();
				break;
			case State.grab:
				grabAnim();
				break;
			case State.getUp:
				getUpAnim();
				break;
		}
	}

	void movementAnim(){
		Quaternion lookRot;
		if(player.moveInput.x != 0){
			inX = player.moveInput.x;
			}
		if(player.moveInput.z != 0){
			inZ = player.moveInput.z;
		}
		lookRot = Quaternion.LookRotation(new Vector3(inX, 0f, inZ));

		if((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)){
			transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, rotSpeed * Time.deltaTime);
		}
		if(player.getAngle() > 45){
			transform.rotation = lookRot;
		}

		
		if(player.getCC().isGrounded){
			//handle all ground shit
			if(player.getAngle() > 45f){
				setAnim("isSliding");
			}
			else{
				//not sliding
				if(player.moveInput.x != 0f || player.moveInput.z != 0f){
					setAnim("isRunning");
				}
				else{
					setAnim("isIdle");
				}
			}
		}
		else{
			//must be in falling anim
			if(!Physics.Raycast(transform.position, -transform.up, 0.5f)){
				setAnim("isJumping");
			}

		}
	}

	float animTime = 0f;
	void grabAnim(){
		setAnim("isGrabbing");
		animTime += Time.deltaTime;
		if(animTime >= anim.GetCurrentAnimatorStateInfo(0).length/2f){
			//enough time has passed for anim to finish
			//if its done
			player.setGrab(true);
			animTime = 0f;
		}
	}

	void getUpAnim(){
		setAnim("isClimbing");
		animTime += Time.deltaTime;
		if(animTime >= anim.GetCurrentAnimatorStateInfo(0).length/1.2f){
			//enough time has passed for anim to finish
			//if its done
			player.setGetUp(true);
			animTime = 0f;
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
				anim.SetBool("isSliding", false);
				break;
			case "isRunning":
				//set running true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", true);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				anim.SetBool("isSliding", false);
				break;
			case "isWalking":
				//set walking to true, all else to false (there has to be a better way -_-)
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", true);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				anim.SetBool("isSliding", false);
				break;
			case "isJumping":
				//set jumping true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", true);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				anim.SetBool("isSliding", false);
				break;
			case "isGrabbing":
				//set grabbing true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", true);
				anim.SetBool("isClimbing", false);
				anim.SetBool("isSliding", false);
				break;
			case "isClimbing":
				//set getUp true, all else false
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", true);
				anim.SetBool("isSliding", false);
				break;
			case "isSliding":
				//set sliding anim to be true
				//no slide anim yet
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
				anim.SetBool("isWalking", false);
				anim.SetBool("isJumping", false);
				anim.SetBool("isGrabbing", false);
				anim.SetBool("isClimbing", false);
				anim.SetBool("isSliding", true);
				break;
			default:
				Debug.Log("Wrong input");
				break;
		}
	}
}
