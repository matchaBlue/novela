using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextAtLine : MonoBehaviour {

	public TextAsset theText;

	public int startLine;
	public int endLine;

	public TextBoxManager theTextBox;
	public bool destroyWhenActivated;


	// Use this for initialization
	void Start () {
		theTextBox = FindObjectOfType<TextBoxManager> ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnterCollider (Object other)
	{
		if (other.name == "ROBOT") {
			theTextBox.ReloadScript (theText);
			theTextBox.currentLine = startLine;
			theTextBox.endAtLine = endLine;
			theTextBox.EnableTextBox ();

			if (destroyWhenActivated) {
				Destroy (gameObject);
			}
		}
	}
}
