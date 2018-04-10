using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 30f;
	public float jumpSpeed = 10f;
	public float gravity = 0f;

	CharacterController cc;

	void Start(){
		cc = GetComponent<CharacterController>();
	}

	public Vector3 moveInput = Vector3.zero;
	RaycastHit hit;
	float raycastLength = 10f;


	void Update(){
		cc.Move(MoveApplied(true) * speed * Time.deltaTime);
	}

	Vector3 MoveApplied(bool inputAllowed){

		if(inputAllowed){
			if(cc.isGrounded){
			//onGround
				Physics.Raycast(transform.position, -transform.up, out hit, raycastLength);
				float angle = Vector3.Angle(transform.up, hit.normal);

				Vector3 into = Vector3.Cross(transform.up, hit.normal);
				Vector3 downhill = Vector3.Cross (into, hit.normal);
				downhill = downhill.normalized;

				Debug.DrawRay(transform.position - (Vector3.up), -transform.up);
				moveInput = new Vector3((transform.right.x) * Input.GetAxis("Horizontal"), downhill.y, transform.forward.z * Input.GetAxis("Vertical"));
				moveInput.y = downhill.y;
				if(angle > 45f){
				//sliding
					moveInput = downhill/1.8f;
				}
				else{
				//regular movement factoring in slopes
					if(Input.GetAxis("Jump") > 0f){
						moveInput.y = jumpSpeed;
					}
				}
			}
			else{
			//notOnGround, start faling
				fallDi(8f);

				moveInput.y += Physics.gravity.y * gravity;
			}
		}

		return moveInput;
	}

	void fallDi(float di){
		if(moveInput.x > -1f && moveInput.x < 1f){
					//ur less than  or equal to 1, but greater than zero
					//you can substract from it until it equals 0, but only accept negative input

			moveInput.x += (Vector3.right.x/di) * Input.GetAxis("Horizontal");
			//if(Input.GetAxis("Horizontal") < 0f){
				//if ur holding opposite of the last direction...

			//}
		}
		else if(moveInput.x >= 1f){
			//if ur on the right greatest boundary, only remove
			if(Input.GetAxis("Horizontal") < 0f){
				//holding left
				moveInput.x += (Vector3.right.x/di) * Input.GetAxis("Horizontal");
			}
		}
		else if(moveInput.x <= -1f){
			//on the lft most boaundier, only reomve
			if(Input.GetAxis("Horizontal") > 0f){
				//only holding right has an effect here
				moveInput.x += (Vector3.right.x/di) * Input.GetAxis("Horizontal");
			}
		}
		else{
			Debug.Log("moveInput.x is not greater than -1 AND less than 1f, not greater than or equal to 1, not less than or equal to -1");
		}

		if(moveInput.z > -1f && moveInput.z < 1f){
					//ur less than  or equal to 1, but greater than zero
					//you can substract from it until it equals 0, but only accept negative input

					moveInput.z += (Vector3.forward.z/di) * Input.GetAxis("Vertical");
					//if(Input.GetAxis("Horizontal") < 0f){
						//if ur holding opposite of the last direction...

					//}
				}
				else if(moveInput.z >= 1f){
					//if ur on the right greatest boundary, only remove
					if(Input.GetAxis("Vertical") < 0f){
						//holding left
						moveInput.z += (Vector3.forward.z/di) * Input.GetAxis("Horizontal");
					}
				}
				else if(moveInput.z <= -1f){
					//on the lft most boaundier, only reomve
					if(Input.GetAxis("Vertical") > 0f){
						//only holding right has an effect here
						moveInput.z += (Vector3.forward.z/di) * Input.GetAxis("Horizontal");
					}
				}
				else{
					Debug.Log("moveInput.x is not greater than -1 AND less than 1f, not greater than or equal to 1, not less than or equal to -1");
				}
	}

}

