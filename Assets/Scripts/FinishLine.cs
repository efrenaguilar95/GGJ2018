using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Needed for text

public class FinishLine : MonoBehaviour {
    public Text victoryText;

    private bool funState = false;


    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Player")
        {
            funState = true;
            victoryText.text = "Hurray!";
        }
    }

    public bool GetFunState()
    {
        return funState;
    }
}
