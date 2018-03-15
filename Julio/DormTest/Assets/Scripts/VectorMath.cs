using UnityEngine;
using System.Collections;

public class VectorMath : MonoBehaviour {

	public Transform pivot;

	CharacterController cc;
	Vector3 moveInput;
	float jumpPower = 1f;
	float lastY;
	float speed = 10f;

	public Transform playerModel;
	Animator anim;

	void Start(){
		lastY = 0;
		cc = GetComponent<CharacterController>();
		//initial anim values
		anim = GetComponentInChildren<Animator>();
		anim.SetBool("isWalking", false);
		anim.SetBool("isIdle", true);
		anim.SetBool("isRunning", false);
		anim.SetBool("isJumping", false);
	}

	void Update () {
	
		lastY = moveInput.y;
		moveInput = (transform.forward * Input.GetAxisRaw("Vertical")) + 
		(transform.right * Input.GetAxisRaw("Horizontal"));
		moveInput.y = lastY;

		if(cc.isGrounded){
			Debug.Log("isGrounded");
			moveInput.y = 0f;
			//when grounded, apply no Y movement
			if(Input.GetKeyDown(KeyCode.Space)){
				moveInput.y = jumpPower;
			}
		}
		else{
			Debug.Log("notGrounded");
		}

		//rotate it to face the same as pivot;
		if(moveInput.magnitude > 0f){
			//if moving, rotate it bruh
			transform.rotation = Quaternion.Euler(new Vector3(0f, pivot.eulerAngles.y, 0f));
			Quaternion lookRot = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.z));
			playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, lookRot, speed * Time.deltaTime);
		}

		//update animations. too clunky so I moved it to a seperate method
		animControl();

		//drop this much y each frame. y is set to 0 each frame ur grounded, but then is dropped a little bit after, only to be set to 0 the next frame
		moveInput.y += Physics.gravity.y * 0.01f;

		//Applies a vector3 as movement each frame
		cc.Move(moveInput * speed * Time.deltaTime);
		
	}

	void animControl(){
		//create new public floats called runSpeed walkSpeed, set speed accordingly (just for clean up)
		if(cc.isGrounded){
			anim.SetBool("isJumping", false);
			if(moveInput.magnitude > 0f && !(Input.GetKey(KeyCode.LeftShift))){
				speed = 3f;
				//walking
				anim.SetBool("isWalking", true);
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
			}
			else if(moveInput.magnitude > 0f && (Input.GetKey(KeyCode.LeftShift))){
				speed = 10f;
				//running
				anim.SetBool("isWalking", false);
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", true);
			}
			else{
				//idle
				anim.SetBool("isWalking", false);
				anim.SetBool("isIdle", true);
				anim.SetBool("isRunning", false);
			}
		}
		else{
			//not touching ground
			anim.SetBool("isJumping", true);
			anim.SetBool("isWalking", false);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
		}
	}

}
