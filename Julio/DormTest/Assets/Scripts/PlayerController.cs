using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 30f;
	public float jumpSpeed = 10f;
	public float gravity = 0f;
	public float directionalInfluence = 0f;

	enum State {movement, grab, getUp};
	State currentState; 
	public Vector3 moveInput = Vector3.zero;
	RaycastHit hit;
	RaycastHit hHit;
	public float raycastLength = 5f;

	private float angle;
	CharacterController cc;
	public Transform head;

	void Start(){
		cc = GetComponent<CharacterController>();
		currentState = State.movement;
	}

	void Update(){
		stateControl ();
		//cc.Move(MoveApplied(true) * speed * Time.deltaTime);
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
				break;
		}
	}

	void MoveApplied(){
		
		if(cc.isGrounded){
			cc.height = 2f;
			cc.center.Set(0, 0, 0);
			//cc.center.y = 0;
		//onGround
			Physics.Raycast(transform.position, -transform.up, out hit, raycastLength);
			angle = Vector3.Angle(transform.up, hit.normal);

			Vector3 into = Vector3.Cross(transform.up, hit.normal);
			Vector3 downhill = Vector3.Cross (into, hit.normal);
			downhill = downhill.normalized;

			//Debug.DrawRay(transform.position - (Vector3.up), -transform.up);
			moveInput = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
			//Debug.Log(downhill.y);
			if(downhill.y == 0){
				moveInput.y = 0;
			}
			else{
				moveInput.y = downhill.y;

			}
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
		}
		else{
			fallDi(directionalInfluence);
		}
		moveInput.y += Physics.gravity.y * gravity;
	
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

	void grabCheck(){
		RaycastHit headHit;
		Ray headRay = new Ray();
		headRay.origin = transform.position + (Vector3.up * 0.5f);
		headRay.direction = head.forward;
		Debug.DrawRay(headRay.origin, headRay.direction);
		float headcastLength = 1f;
		if(Physics.Raycast(headRay, out headHit, headcastLength)){
			Debug.Log(headHit.collider.ToString());
			if(headHit.collider.CompareTag("Player")){
				if(headHit.collider.bounds.max.y - head.position.y < 0.5f){
					Vector3 wallOffset = headHit.collider.GetComponent<MeshRenderer>().bounds.ClosestPoint(transform.position);
					wallOffset = wallOffset + headHit.normal;
					transform.position = new Vector3(wallOffset.x, headHit.collider.GetComponent<MeshRenderer>().bounds.max.y, wallOffset.z);
					currentState = State.grab;
				}

			}
		}
	}
		
	void Grab(){
		Debug.Log ("Grabbing");
	}

}

