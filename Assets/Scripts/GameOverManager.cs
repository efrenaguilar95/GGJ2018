using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour {

    public float restartDelay = 5f;
    public bool failState = false;

    private Animator anim;
    private float restartTimer;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {

        if(failState == true)
        {
            anim.SetTrigger("GameOver");
            restartTimer += Time.deltaTime;

            if(restartTimer >= restartDelay)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void SetFailState(bool state)
    {
        failState = state;
    }

}
