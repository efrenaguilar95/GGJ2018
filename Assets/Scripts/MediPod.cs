using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediPod : MonoBehaviour
{
	public GameObject m_Player;

	private void OnTriggerStay2D(Collider2D other)
    {
		print (other.tag);
		if (other.tag == "Helpless")
        {
			GameObject human = other.transform.gameObject;
			Animator humanAnimator = human.GetComponentInChildren<Animator>();
			if (!humanAnimator.GetBool("Hanging"))
			{
				// remove human
				Destroy(human);

				// give happiness
				PlayerController playerControllerScript = m_Player.GetComponentInChildren<PlayerController>();
				playerControllerScript.ChangeHappiness(15);
			}
        }
    }
}
