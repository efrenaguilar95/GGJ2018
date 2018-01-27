using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//For this component, create a text UI object in the scene and attach it to Timer Text;

public class Timer : MonoBehaviour {

    public Text timerText;
    private float startTime;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        float t = Time.time - startTime;

        string minutes = ((int) t / 60).ToString();
        string seconds = (t % 60).ToString("f2");

        if (t % 60 < 10)
            timerText.text = minutes + ":0" + seconds;
        else
            timerText.text = minutes + ":" + seconds;
	}

    public float GetTime()
    {
        return Time.time - startTime;
    }
}
