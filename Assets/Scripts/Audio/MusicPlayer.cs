using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {

    public PlayerController player;
    public AudioMixerGroup playerSongGroup;
    public AudioMixerGroup friendSongGroup;
    public float fadeRate = 1;

    public bool isWithFriend = false;

    IEnumerator fadeInMethod;
    IEnumerator fadeOutMethod;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

        if(isWithFriend)
        {
            fadeInMethod = fadeIn(friendSongGroup, fadeRate);
            if(fadeOutMethod != null)
                StopCoroutine(fadeOutMethod);
            StartCoroutine(fadeInMethod);
        }
        else
        {
            fadeOutMethod = fadeOut(friendSongGroup, fadeRate);
            if(fadeInMethod != null)
                StopCoroutine(fadeInMethod);
            StartCoroutine(fadeOutMethod);
        }
		
	}

    public IEnumerator fadeIn(AudioMixerGroup trackToFade, float fadeRate)
    {
        float currentAudioVolume;
        playerSongGroup.audioMixer.GetFloat("FriendSongVol", out currentAudioVolume);
        //Debug.Log(currentAudioVolume);
        while (currentAudioVolume <= 0)
        {
            playerSongGroup.audioMixer.GetFloat("FriendSongVol", out currentAudioVolume);
            currentAudioVolume += fadeRate * Time.deltaTime;
            trackToFade.audioMixer.SetFloat("FriendSongVol", currentAudioVolume);
            yield return null;
        }
    }

    public IEnumerator fadeOut(AudioMixerGroup trackToFade, float fadeRate)
    {
        float currentAudioVolume;
        playerSongGroup.audioMixer.GetFloat("FriendSongVol", out currentAudioVolume);
        while (currentAudioVolume >= -80)
        {
            playerSongGroup.audioMixer.GetFloat("FriendSongVol", out currentAudioVolume);
            currentAudioVolume -= fadeRate * Time.deltaTime;
            trackToFade.audioMixer.SetFloat("FriendSongVol", currentAudioVolume);
            yield return null;
        }
    }
}
