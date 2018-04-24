using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VectorMath : MonoBehaviour{

	int health = 100;
	private Animator anim;
	CharacterController cc;


	//public RectTransform healthBar;
	
	public Transform pivot;


	Vector3 moveInput;
	public float jumpPower = 1f;
	float lastY;
	private float speed = 10f;
	public float rotSpeed = 20f;
	public float slideSpeed = 10f;

	public float walkSpeed = 3f;
	public float runSpeed = 10f;

	public Transform playerModel;

	void Start(){
		//health = (int)healthBar.sizeDelta.x;
		lastY = 0f;
		cc = GetComponent<CharacterController>();
		//initial anim values
		anim = GetComponentInChildren<Animator>();
		anim.SetBool("isWalking", false);
		anim.SetBool("isIdle", true);
		anim.SetBool("isRunning", false);
		anim.SetBool("isJumping", false);
	}

	RaycastHit hit, headHit;
	public float distToGround = 5f;

	//handle climbing and sliding
	public Transform head;
	public float distToHead = 6f;
	public float climbHeight = 2f;
	float theta;
	float dist;
	bool sliding = false;
	bool onWall = false;
	bool grabbed = false;
	bool gettingUp = false;

	public float gravMult = 0.01f;

	float slideAngle = 45f;

	void Update () {

		//check if sliding, or on wall first? it shouldnt be checked if ur on wall i feel
		//always raycast tho, then check outside the raycast
		if(Physics.Raycast(transform.position, -transform.up, out hit, distToGround)){
			theta = Vector3.Angle(hit.normal, transform.up);
			if(theta > slideAngle){sliding = true;} else{sliding = false;}
		}

		//check if grabbing wall. should turn off while already grabbed so you dont turn onWall->false when animating.
		//onWall is global so mine aswell only use this if onWall is false
		if(!onWall && Physics.Raycast(head.position, head.forward, out headHit, distToHead) && moveInput.y < 0f){
			dist = head.position.y - headHit.collider.GetComponent<MeshRenderer>().bounds.max.y;
			//using Abs causes it to measure the distance from the bottom aswell, applying a negative height
			if(Mathf.Abs(dist) <= climbHeight){onWall = true; grabbed = true;}
		}

		improvedMovement();
		//healthControl();
	}


	void improvedMovement(){

		if(sliding && !onWall){
			//sliding
			setAnim("isSliding");
			Vector3 into = Vector3.Cross(transform.up, hit.normal);
			Vector3 downhill = Vector3.Cross (into, hit.normal);
			downhill = downhill.normalized;
			//should allow left and right movement down hill in theory
			moveInput = (downhill * slideSpeed) + (transform.right * Input.GetAxis("Horizontal"));
		}
		else if(!sliding && onWall){
			//not sliding, grabbing wall
			if(grabbed && !gettingUp){
				if(headHit.normal.y > 0f){
					Debug.Log("Scanned top of wall, ur gonna clip in. DO NOTHING");
					sliding = false;
					onWall = false;
					//just keep falling
				}
				else{
					Vector3 wallOffset = headHit.collider.GetComponent<MeshRenderer>().bounds.ClosestPoint(transform.position);
					Debug.Log("Closest point: " + wallOffset);
					wallOffset = wallOffset + headHit.normal;
					Debug.Log("Actual position of player: " + wallOffset);
					Debug.Log("Headhit's Normal: " + headHit.normal);
					transform.position = new Vector3(wallOffset.x, headHit.collider.GetComponent<MeshRenderer>().bounds.max.y, wallOffset.z);
					setAnim("isGrabbing");
					moveInput = Vector3.zero;
					grabbed = false;
					gettingUp = true;
				}
			}
			else if(!grabbed && gettingUp){
				//if the grab anim is done, you can start gettingUp
				if(anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LedgeGrab") && (Input.GetAxis("Jump") > 0f)){
					setAnim("isClimbing");
					gettingUp = false;
				}
			}
			else{
				//climb here
				if(anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LedgeGetUp")){
					transform.position = new Vector3(-headHit.normal.x + transform.position.x, transform.position.y + 1f, -headHit.normal.z + transform.position.z);
					onWall = false;
				}
		
			}

		}
		else if(!sliding && !onWall){
			lastY = moveInput.y;
			moveInput = (transform.forward * Input.GetAxis("Vertical")) + 
			(transform.right * Input.GetAxis("Horizontal"));
			moveInput.y = lastY; //at the start of this frame, the moveInput.y is set to last frames y

			if(cc.isGrounded){
				moveInput.y = 0f;
				if(moveInput.magnitude > 0f && !(Input.GetKey(KeyCode.LeftShift))){
					setAnim("isWalking");
					speed = walkSpeed;
				}
				else if(moveInput.magnitude > 0f && (Input.GetKey(KeyCode.LeftShift))){
					setAnim("isRunning");
					speed = runSpeed;
				}
				else{
					setAnim("isIdle");
				}

				if(Input.GetButton("Jump")){
					moveInput.y = jumpPower;

				}
			}
			else{
				setAnim("isJumping");
				Debug.Log("not grounded");
				speed = runSpeed;
			}

			moveInput.y += Physics.gravity.y * gravMult;


			//if moving, basically. but doesnt handle animation. should only rotate model
			if(Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f){
				transform.rotation = Quaternion.Euler(new Vector3(0f, pivot.eulerAngles.y, 0f));
				Quaternion lookRot = Quaternion.LookRotation(new Vector3(moveInput.x, 0f, moveInput.z));
				playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, lookRot, rotSpeed * Time.deltaTime);
			}
		}
		else{
			onWall = false;
			//sliding and trying to grab wall
		}

		Debug.Log("Forward Vector: " + transform.forward);

		cc.Move(moveInput * speed * Time.deltaTime);
	}


	//i wanna move this vvv to a different script. just say ifPlayer is true, display on screen. else, just keep track
	/*
	int count = 0;
	void healthControl(){
		healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
		if(Time.frameCount%12 == 0){
			count += 1;
			if(count%6 == 0)
				health -= 6;
		}
	}
	*/
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

	void animControl(){
		//create new public floats called runSpeed walkSpeed, set speed accordingly (just for clean up)
		if(cc.isGrounded){
			anim.SetBool("isJumping", false);
			if(moveInput.magnitude > 0f && !(Input.GetKey(KeyCode.LeftShift))){
				speed = walkSpeed;
				//walking
				anim.SetBool("isWalking", true);
				anim.SetBool("isIdle", false);
				anim.SetBool("isRunning", false);
			}
			else if(moveInput.magnitude > 0f && (Input.GetKey(KeyCode.LeftShift))){
				speed = runSpeed;
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
			speed = runSpeed;
			anim.SetBool("isJumping", true);
			anim.SetBool("isWalking", false);
			anim.SetBool("isIdle", false);
			anim.SetBool("isRunning", false);
		}
	}

}
