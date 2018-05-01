using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
	public void LoadScene(int n){
		if(n < 0){
			EditorApplication.isPlaying = false;
			//placeholder for closing application
		}
		//Debug.Log("Load Request: " + n);
		SceneManager.LoadScene(n);
	}
}


