using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicPlayer : MonoBehaviour {

    public PlayerController player;
    public AudioMixerGroup playerSongGroup;
    public AudioMixerGroup friendSongGroup;
    public float fadeInRate = 1;
    public float fadeOutRate = 1;

    IEnumerator fadeInMethod;
    IEnumerator fadeOutMethod;

	// Use this for initialization
	void Start () {
        friendSongGroup.audioMixer.SetFloat("FriendSongVol", -80);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void applyFade()
    {
        if(player.IsCarrying())
        {
            fadeInMethod = fadeIn(friendSongGroup, fadeInRate);
            if (fadeOutMethod != null)
                StopCoroutine(fadeOutMethod);
            StartCoroutine(fadeInMethod);
        }
        else
        {
            fadeOutMethod = fadeOut(friendSongGroup, fadeOutRate);
            if (fadeInMethod != null)
                StopCoroutine(fadeInMethod);
            StartCoroutine(fadeOutMethod);
        }

    }

    public IEnumerator fadeIn(AudioMixerGroup trackToFade, float fadeRate)
    {
        Debug.Log("WTF");
        float currentAudioVolume;
        playerSongGroup.audioMixer.GetFloat("FriendSongVol", out currentAudioVolume);
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
        Debug.Log("WTF2");
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
