using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof (CharacterController))]
public class InputManager : MonoBehaviour
{
	private PlayerController m_Character;
	private bool m_Jump;

	private void Awake()
	{
		m_Character = GetComponent<PlayerController>();
	}


	private void Update()
	{
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
		float h = CrossPlatformInputManager.GetAxis("Horizontal");

		// Pass all parameters to the character control script
		m_Character.Move(h, m_Jump, pickup);
		m_Jump = false;
	}
}
