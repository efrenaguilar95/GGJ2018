using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemySpike : MonoBehaviour {
    public GameOverManager gameOver;
    

    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject hitObj = collider.gameObject;
        if (hitObj.tag == "Player")
        {
             gameOver.SetFailState(true);
        }
    }
}
