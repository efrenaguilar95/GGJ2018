using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour {

    private AudioSource audioSource;
    public List<AudioClip> footstepSounds;

	// Use this for initialization
	void Start () {
        audioSource = this.GetComponent<AudioSource>();
	}


    void PlayerFootstep()
    {
        audioSource.PlayOneShot(footstepSounds[Random.Range(0, footstepSounds.Count)]);
    }
}
