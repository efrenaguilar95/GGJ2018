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

    public GameObject pauseMenu;
    bool isPaused = false;
    bool canPause = false; //Check if player has started the game, because they cannot pause in the main menu

    public AudioMixerGroup masterAudioMixer;

    private void Start()
    {
        anim = transform.Find("FadeImage").GetComponent<Animator>();
        primaryMenu = transform.Find("PrimaryMenu").gameObject;
        creditsScreen = transform.Find("CreditsPanel").gameObject;
        optionsBox = transform.Find("VolumePanel").gameObject;
        volumeSlider = optionsBox.transform.Find("VolumeSlider").GetComponent<Slider>();
        pauseMenu = gameObject.transform.Find("PauseMenu").gameObject;
        //Needed for initial volume setting
        VolumeSlideControl();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (canPause)
            if (Input.GetKeyDown(KeyCode.Escape))
                OnGamePaused(); 
    }

    public void OnStartClick()
    {
        StartCoroutine(FadeInOut());
    }
    IEnumerator FadeInOut()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        canPause = true;
        SceneManager.LoadScene(1);
        primaryMenu.SetActive(!primaryMenu.activeSelf);
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

    public void OnGamePaused()
    {
        
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            isPaused = false;
        }
    }
}