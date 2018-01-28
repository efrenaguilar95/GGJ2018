using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZoneController : MonoBehaviour
{
	public GameObject m_Player;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
		}
		else if (other.tag == "Human")
		{
			// remove human
			GameObject human = collider.gameObject;
			Destroy(human);

			// take away happiness
			PlayerController playerControllerScript = m_Player.GetComponentInChildren<PlayerController>();
			playerControllerScript.ChangeHappiness(-100);
		}
	}
}
