using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (CharacterController))]
public class InputManager : MonoBehaviour
{
	private PlayerController m_Character;
	private bool m_Jump;
	public GameObject canvas;
	private void Awake()
	{
		m_Character = GetComponent<PlayerController>();
	}


	private void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			canvas.SetActive(!canvas.activeSelf);
		}
		if (!m_Jump)
		{
			// Read the jump input in Update so button presses aren't missed
			m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
	}


	private void FixedUpdate()
	{
		// Read the inputs
		bool pickup = Input.GetKey(KeyCode.S);
		bool travel = Input.GetKey(KeyCode.LeftShift);
		float h = CrossPlatformInputManager.GetAxis("Horizontal");

		// Pass all parameters to the character control script
		m_Character.Move(h, m_Jump, pickup, travel);
		m_Jump = false;
	}
}
