using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpike : MonoBehaviour {
    public GameOverManager gameOver;
    
    private bool failState = false;

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Player")
        {
            failState = true;
        }
    }

    public bool GetFailState()
    {
        return failState;
    }
}
