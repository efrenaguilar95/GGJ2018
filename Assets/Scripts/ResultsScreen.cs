using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour {

    public float transitionDelay = 5f;
    public string nextScene;    //next level to enter
    public FinishLine finisher; //finish line checker
    public Text resultsText;
    public StarCounter stars;   //to check how many stars are given in this level depending on completion time
    public Timer time;          //to get the time the level completed
    
    private float transitionTimer;
    private Animator anim;
    private bool safety = true;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (finisher.GetFunState() == true && safety == true)
        {
            safety = false;
            UpdateResultsText();
            transitionTimer = Time.deltaTime;
            if (transitionTimer >= transitionDelay)
            {
                SceneManager.LoadScene(nextScene); //this thing needs a little bit of work in terms of going to the next scene.
            }
        }
    }

    private void UpdateResultsText()
    {
        float clock = time.GetTime();
        int starCount = 0;
        resultsText.text += "You did it!\n";

        if (clock < stars.starGoal3)
        {
            anim.SetTrigger("3Star");
            starCount = 3;
            resultsText.text += "You earned " + starCount + " stars!";
        }
        else if (clock < stars.starGoal2)
        {
            anim.SetTrigger("2Star");
            starCount = 2;
            resultsText.text += "You earned " + starCount + " stars!";
        }
        else if (clock < stars.starGoal1)
        {
            anim.SetTrigger("1Star");
            starCount = 1;
            resultsText.text += "You earned " + starCount + " stars!";
        }
        else
        {
            resultsText.text += "You earned no stars!";
            anim.SetTrigger("ResultsScreen");
        }
    }
}
