using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour{
	public void LoadScene(int n){
		Debug.Log("Load Request: " + n);
		SceneManager.LoadScene(n);
	}
}


