using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDialogue : MonoBehaviour {

    public GameObject screenBox;
    public GameObject speechBubble;

    public float hearingDistance;
    public bool dialogActive;
    public bool visible;
    private float distance;
    private void Update()
    {
        visible = GetComponentInChildren<Renderer>().isVisible;
        distance = Vector2.Distance(gameObject.transform.position, GameObject.FindGameObjectWithTag("Player").transform.position);
        if (distance < hearingDistance)
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
            if (visible)
            {
                speechBubble.SetActive(true);
                screenBox.SetActive(false);
            }
            else
                screenBox.SetActive(true);
        }
    }
}
