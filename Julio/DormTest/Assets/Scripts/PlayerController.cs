using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 30f;
	public float jumpSpeed = 10f;
	public float gravity = 0f;
	public float directionalInfluence = 0f;

	private float angle;
	CharacterController cc;
	public Transform head;

	void Start(){
		cc = GetComponent<CharacterController>();
	}

	public Vector3 moveInput = Vector3.zero;
	RaycastHit hit;
	public float raycastLength = 5f;
	bool canMove = true;

	void Update(){
		cc.Move(MoveApplied(true) * speed * Time.deltaTime);
	}

	Vector3 MoveApplied(bool inputAllowed){
		
		if(inputAllowed){
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
				//fallDi2();
				//every frame, you should be able to
			}
			moveInput.y += Physics.gravity.y * gravity;
		}
		return moveInput;
	}

	void fallDi2(){
		//every frame, the x and z get cut back i guess
		Vector3 currentVector = (transform.right * Input.GetAxis("Horizontal")) + (transform.forward * Input.GetAxis("Vertical"));
		float y = moveInput.y;
		//influence the moveInput with the current moveInput vector
		if((moveInput.x < 1f && moveInput.x > -1f) && (moveInput.z < 1f && moveInput.z > -1f)){
			//have to be in range to influence
			//float lastX = moveInput.x;
			moveInput += currentVector/directionalInfluence;

		}
		else{
			//only allow DI in opposite of current moveInput
			if(moveInput.x > 1f && currentVector.x < 0f){
				//in pos direction, holding in opposite direction
				moveInput.x += currentVector.x;
			}
			if(moveInput.x < -1f && currentVector.x > 0f){
				//in neg direction, holding positive
				moveInput.x += currentVector.x;
			}
			if(moveInput.z > 1f && currentVector.z < 0f){
				//in pos direction, holding in opposite direction
				moveInput.z += currentVector.z;
			}
			if(moveInput.z < -1f && currentVector.z > 0f){
				//in neg direction, holding positive
				moveInput.z += currentVector.z;
			}
		}
		moveInput.y = y;
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

	void getUp(){
		//if the distance from the head is close and the angle between the raycast and collider is within range
		//then go into the grab animetion. keep looping through until the jump button is pressed or if getAxis is in the same direction as the character
		//if getAxis is opposite of current direction, release(turn him around so he is no longer facing the ledge)
		//when the jump/getAxis is pressed, go into the getUp animation. once the animation is done, move the player to the edge of the collider
		//Physics.Raycast(transform.position, -transform.up, out hit, raycastLength);
		RaycastHit hit;
		if(Physics.Raycast(head.position, head.forward, out hit, 0.5f)){
			//if u hit something
			float distance = hit.collider.GetComponent<MeshRenderer>().bounds.max.y - head.position.y;
			//Debug.Log(distance);
			if(distance < 0.5f){
				Debug.Log("Grabbing");

			}
		}
		else{
			Debug.Log("Not touching anything, mesh renderer shuldnt even be called");
		}
	}

}

