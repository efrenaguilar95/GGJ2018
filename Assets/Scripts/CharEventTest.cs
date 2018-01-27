using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharEventTest : MonoBehaviour {
	private GameObject dialogManager;
	public TextAsset textFile;
	Quaternion startRotation;
	// Use this for initialization
	void Start () {
		dialogManager = gameObject.transform.Find ("Canvas/DialogueManager").gameObject;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Semicolon))
			dialogManager.GetComponent<CharacterDialogue> ().ToggleBox (textFile.text);
	}

}
