using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //Needed for text

public class FinishLine : MonoBehaviour {
    public ResultsScreen win;

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Player")
        {
            win.SetWinState(true);
        }
    }
}
