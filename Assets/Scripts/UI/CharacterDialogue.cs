using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialogue : MonoBehaviour {

    public GameObject screenBox;
    public GameObject speechBubble;

    public float hearingDistance;
    public bool dialogActive;

    private void Update()
    {
        if (Vector2.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) < hearingDistance)
        {
            dialogActive = true;
        }
        else
        {
            dialogActive = false;
            screenBox.SetActive(false);
            speechBubble.SetActive(false);
        }
        ShowBox();
    }

    void ShowBox()
    {
        if (dialogActive)
        {
            if (GetComponent<Renderer>().isVisible)
                speechBubble.SetActive(true);
            else
                screenBox.SetActive(true);
        }
    }
}
