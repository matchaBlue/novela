using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class unPause : MonoBehaviour {
	public Transform canvas;
	// Use this for initialization
	public void noPause(){
		canvas.gameObject.SetActive (false);
		Time.timeScale = 1;
	}
}
