using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialogue : MonoBehaviour {

	public GameObject dBox;
	public Text dText;
	public bool dialogueActive = false;
	public TextAsset textFile;
	public string lines;


	// Use this for initialization
	void Start () {
		lines = ImportTextfromFile ();
		ShowBox (lines);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ShowBox (string dialogue)
	{
		dialogueActive = true;
		dBox.SetActive (true);
		dText.text = dialogue;
	}
	public string ImportTextfromFile(){
		Debug.Log (textFile.text);
		return textFile.text;
	}
}
