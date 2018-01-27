using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {

    public Image fadeImage;
    public Animator anim;

    private void Start()
    {
        anim = transform.Find("FadeImage").GetComponent<Animator>();
    }
    public void OnStartClick()
    {
        StartCoroutine(FadeInOut());
    }
    IEnumerator FadeInOut()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        SceneManager.LoadScene(1);
    }

    public void OnExitClick()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        else
            Application.Quit();
    }
}
