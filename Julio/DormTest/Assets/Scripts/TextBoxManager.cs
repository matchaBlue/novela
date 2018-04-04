using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxManager : MonoBehaviour {

	public GameObject textBox;
	public Text actualText;

	public TextAsset textFile;
	public string[] textLines;

	public int currentLine;
	public int endAtLine;

	public CharacterController player;

	public bool isActive;



	// Use this for initialization
	void Start () {
		player = FindObjectOfType<CharacterController>();

		if (textFile != null) 
		{
			textLines = (textFile.text.Split('\n'));
		}

		if (endAtLine == 0) 
		{
			endAtLine = textLines.Length-1;
		}

		if (isActive) {
			EnableTextBox ();
		} else {
			DisableTextBox ();
		}
	}

	void Update(){
		if (!isActive) {
			return;
		}

		actualText.text = textLines [currentLine];

		if (Input.GetKeyDown (KeyCode.Return)) {
			currentLine += 1;
		}

		if (currentLine > endAtLine) {
			DisableTextBox ();

		}
			
	}

	public void EnableTextBox(){
		textBox.SetActive (true);
	}

	public void DisableTextBox(){
		textBox.SetActive (false);
	}

	public void ReloadScript(TextAsset theText){
		if (theText != null) {
			textLines = new string[1];
			textLines = (theText.text.Split('\n'));
		}
		
	}
}
