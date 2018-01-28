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

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        float val;
        playerSongGroup.audioMixer.GetFloat("FriendSongVol", out val);
        Debug.Log(val);
        if(isWithFriend)
        {
            StartCoroutine(fadeIn(playerSongGroup, fadeRate));
        }
        else
        {
            StartCoroutine(fadeOut(playerSongGroup, fadeRate));
        }
		
	}

    //TODO: Look into using Mathf.Lerp instead
    public IEnumerator fadeIn(AudioMixerGroup trackToFade, float fadeRate)
    {
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
