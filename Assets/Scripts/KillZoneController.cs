using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZoneController : MonoBehaviour
{
	public GameObject m_Player;
    public GameOverManager gameOver;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
            gameOver.SetFailState(true);
		}
		else if (other.tag == "Human")
		{
			// remove human
			GameObject human = other.transform.parent.gameObject;
			Destroy(human);

			// take away happiness
			PlayerController playerControllerScript = m_Player.GetComponentInChildren<PlayerController>();
			playerControllerScript.ChangeHappiness(-100);
		}
	}
}
