using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;

	private Rigidbody rb;
	private bool win;
	private bool lose;

	public float time = 20f;

	void Start ()
	{
		rb = GetComponent<Rigidbody>();
		winText.text = "";
		win = false;
		lose = false;
	}

	void Update(){
		if (!win) {
			time -= Time.deltaTime;
			countText.text = "Time: " + time.ToString ("0.0");
			if (time <= 0) {
				lose = true;
				winText.text = "You Lose...";
				countText.text = "";
			}
		} 
	}

	#region
	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}
	#endregion //Ball Movement

	void OnTriggerEnter(Collider other) 
	{
		if (other.gameObject.CompareTag ("PickUp"))
		{
			other.gameObject.SetActive (false);
			time += 5f;
		}

		if (other.gameObject.CompareTag ("Finish")) {
			if (!lose) {
				win = true;
				winText.text = "You win! Total Time: " + Time.realtimeSinceStartup.ToString ("0.00");
				countText.text = "";
			}
		}
	}
			
}
