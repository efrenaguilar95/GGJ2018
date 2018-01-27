using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialogue : MonoBehaviour {

	public GameObject dBox;
	public Text dText;
	public bool dialogueActive = false;
	public string lines;


	void Start () {
		dBox = gameObject.transform.Find ("DialogueBox").gameObject;
		dText = gameObject.transform.Find ("DialogueBox/Text").GetComponent<Text> ();
	}

	public void ToggleBox (string dialogue = null)
	{
		dBox.SetActive (!dialogueActive);
		dialogueActive = !dialogueActive;
		dText.text = dialogue;
	}
}
