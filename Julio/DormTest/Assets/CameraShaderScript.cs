using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//(ExecuteInEditMode)
public class CameraShaderScript : MonoBehaviour {

	public Material mat;
	private bool is_hit = false;
	public float recover_time = 5f;
	private float end_time;

	void OnRenderImage(RenderTexture src, RenderTexture dest){

		if (is_hit && Time.time > end_time) {
			is_hit = false;
		}
		if (is_hit) {
			Graphics.Blit (src, dest, mat);
		} else {
			Graphics.Blit (src, dest);
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.Space)) {
			mat.SetFloat ("_StartTime", Time.time);
			is_hit = true;
			end_time = Time.time + recover_time;
		}
	}
}
