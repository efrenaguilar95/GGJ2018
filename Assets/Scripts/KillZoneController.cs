using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZoneController : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}
