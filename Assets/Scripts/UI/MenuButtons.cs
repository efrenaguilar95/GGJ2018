using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuButtons : MonoBehaviour {

    public Image fadeImage;
    public Animator anim;
    GameObject primaryMenu;
    GameObject creditsScreen;
    GameObject optionsBox;
    Slider volumeSlider;

    public AudioMixerGroup masterAudioMixer;

    private void Start()
    {
        anim = transform.Find("FadeImage").GetComponent<Animator>();
        primaryMenu = transform.Find("PrimaryMenu").gameObject;
        creditsScreen = transform.Find("CreditsPanel").gameObject;
        optionsBox = transform.Find("VolumePanel").gameObject;
        volumeSlider = optionsBox.transform.Find("VolumeSlider").GetComponent<Slider>();

        //Needed for initial volume setting
        VolumeSlideControl();
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

    public void OnCreditsClick()
    {
        primaryMenu.SetActive(!primaryMenu.activeSelf);
        creditsScreen.SetActive(!creditsScreen.activeSelf);
    }

    public void OnOptionsClick()
    {
        optionsBox.SetActive(!optionsBox.activeSelf);
    }

    public void VolumeSlideControl()
    {
        masterAudioMixer.audioMixer.SetFloat("MasterVolume", volumeSlider.value);
    }
}