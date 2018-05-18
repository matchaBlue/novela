using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 30f;
	public float initSpeed;
	public float sprintSpeed;
	public float jumpSpeed = 10f;
	public float gravity = 0f;
	public float directionalInfluence = 0f;

	public enum State {movement, grab, getUp};
	State currentState; 
	public Vector3 moveInput = Vector3.zero;
	RaycastHit hit;
	RaycastHit hHit;
	public float raycastLength = 5f;
	public Transform pivot;

	public LevelManager lv;

	private float angle;
	CharacterController cc;
	public Transform head;

	void Start(){
		cc = GetComponent<CharacterController>();
		currentState = State.movement;
		sprintSpeed = speed * 1.5f;
		initSpeed = speed;
	}

	void FixedUpdate(){
		stateControl ();
		Debug.Log(cc.isGrounded);
		//cc.Move(MoveApplied(true) * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other){
		if(other.gameObject.CompareTag("PickUp")){
			HealthBar.health += (int)(HealthBar.maxHealth * 0.15);
			other.gameObject.SetActive(false);
		}
		if(other.gameObject.CompareTag("Finish")){
			lv.LoadScene(3);
		}
	}

	void stateControl(){
		//check each fram what state your in
		switch(currentState){
			case State.movement:
				MoveApplied ();
				grabCheck ();
				break;
			case State.grab:
				Grab ();
				break;
			case State.getUp:
				GetUp();
				break;
		}
	}

	void MoveApplied(){
		
		if(cc.isGrounded){
			speed = initSpeed;
			cc.height = 2f;
			cc.center.Set(0, 0, 0);
			//cc.center.y = 0;
		//onGround
			Physics.Raycast(transform.position, -transform.up, out hit, raycastLength);
			angle = Vector3.Angle(transform.up, hit.normal);

			Vector3 into = Vector3.Cross(transform.up, hit.normal);
			Vector3 downhill = Vector3.Cross (into, hit.normal);
			downhill = downhill.normalized;

			moveInput = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
			moveInput.y = downhill.y;

			if(angle > 45f){
			//sliding
				moveInput = downhill;
			}
			else{
			//regular movement factoring in slopes
				if(Input.GetAxis("Jump") > 0f){
					cc.height = 1.5f;
					cc.center.Set(0, 0.4f, 0);
					moveInput.y = jumpSpeed;
				}
			}

			if (Input.GetAxis ("Fire3") > 0) {
				speed = sprintSpeed;
				HealthBar.health -= HealthBar.maxHealth * 0.02f * Time.deltaTime;
			} else {
				speed = initSpeed;
			}
		}
		else{
			fallDi(directionalInfluence);
		}

		//apply gravity
		moveInput.y += Physics.gravity.y * gravity;
		//rotate here based on pivot
		if(Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f){
				transform.rotation = Quaternion.Euler(new Vector3(0f, pivot.eulerAngles.y, 0f));
			}

		//apply movement as a vector
		cc.Move (moveInput * speed * Time.deltaTime);
	}

	void fallDi(float di){
		Vector3 currentVector = (transform.right * Input.GetAxisRaw("Horizontal")) + (transform.forward * Input.GetAxisRaw("Vertical"));
		if(moveInput.x <= 1f && moveInput.x > 0f){
			//going in positive x, holding -x
			if(currentVector.x < 0f){
				
				moveInput.x += currentVector.x/di;
			}
		}
		else if(moveInput.x >= -1f && moveInput.x < 0f){
			//moving in negative X
			if(currentVector.x > 0f){
				
				moveInput.x += currentVector.x/di;
			}
		}
		else{
			//zero
			moveInput.x = currentVector.x;
		}

		if(moveInput.z <= 1f && moveInput.z > 0f){
			if(currentVector.z < 0f){
				
				moveInput.z += currentVector.z/di;
			}
		}
		else if(moveInput.z >= -1f && moveInput.z < 0f){
			//moving in negative X
			if(currentVector.z > 0f){
				
				moveInput.z += currentVector.z/di;
			}
		}
		else{
			//zero
			moveInput.z = currentVector.z;
		}
	}

	public CharacterController getCC(){
		return cc;
	}

	public float getAngle(){
		return angle;
	}

	public State getCurrState(){return currentState;}

	void grabCheck(){
		RaycastHit headHit;
		Ray headRay = new Ray();
		headRay.origin = transform.position + (Vector3.up * 0.5f);
		headRay.direction = head.forward;
		//Debug.DrawRay(headRay.origin, headRay.direction);
		float headcastLength = 1f;
		if(Physics.Raycast(headRay, out headHit, headcastLength)){
			if(headHit.collider.CompareTag("Ledge")){
				if(headHit.collider.bounds.max.y - head.position.y < 0.5f){
					Vector3 wallOffset = headHit.collider.GetComponent<MeshRenderer>().bounds.ClosestPoint(transform.position);
					//wallOffset = wallOffset + headHit.normal;
					transform.position = new Vector3(wallOffset.x, headHit.collider.GetComponent<MeshRenderer>().bounds.max.y - 0.8f, wallOffset.z);
					//transform.rotation = Quaternion.Euler(new Vector3(0f, , 0f));
					currentState = State.grab;
					setGrab(false);
					cc.height = 2f;
					cc.center.Set(0, 0, 0);
					hHit = headHit;
					moveInput = Vector3.zero;
				}

			}
		}
	}

	bool grabDone = false;
	public void setGrab(bool isDone){grabDone = isDone;}
		
	void Grab(){
		if(grabDone){
			//the anim is done, now check input
			if(Input.GetAxis("Jump") > 0){
				currentState = State.getUp;
				setGrab(false);
				setGetUp(false);
			}
		}
	}

	bool getUp = false;
	public void setGetUp(bool isDone){getUp = isDone;}
	void GetUp(){
		if(getUp){
			//shift the box to be ontop
			transform.position = new Vector3(-hHit.normal.x + transform.position.x, transform.position.y + 1.8f, -hHit.normal.z + transform.position.z);
			setGetUp(false);
			currentState = State.movement;
		}
	}

}

