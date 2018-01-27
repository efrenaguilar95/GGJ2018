using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis
	[SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps
	[Range(0, 1)] [SerializeField] private float m_CarrySpeed = .5f;    // Amount of maxSpeed applied to carrying movement (1 = 100%)
	[SerializeField] private bool m_AirControl = true;                  // Whether or not a player can steer while jumping
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	[SerializeField] private bool m_Carrying;            				// Whether or not the player is carrying a person

	private Transform m_GroundCheck;    // A position marking where to check if the player is grounded
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded
	private Transform m_CeilingCheck;   // A position marking where to check for ceilings
	private bool m_CarryToggle;         // Restricts picking up and placing down with the same keypress
	private Animator m_Anim;            // Reference to the player's animator component
	private Rigidbody2D m_Rigidbody2D;  // Reference to the player's rigidbody2d component
	private bool m_FacingRight = true;  // For determining which way the player is currently facing

	private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.Find("GroundCheck");
		m_CeilingCheck = transform.Find("CeilingCheck");
		m_Grounded = false;
		m_Carrying = false;
		m_CarryToggle = false;
		m_Anim = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}


	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}
		m_Anim.SetBool("Ground", m_Grounded);

		// Set the vertical animation
		m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	private void AttemptPickup(bool pickup)
	{
		if (pickup && !m_CarryToggle)
		{
			m_CarryToggle = !m_CarryToggle;
			m_Carrying = !m_Carrying;
		}
		else if (!pickup && m_CarryToggle)
		{
			m_CarryToggle = !m_CarryToggle;
		}
			
		m_Anim.SetBool("Carry", m_Carrying);
	}


	public void Move(float move, bool jump, bool pickup)
	{
		// attempt to pickup a person if s is pressed
		AttemptPickup(pickup);
		
		// only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Reduce the speed if carrying by the CarrySpeed multiplier
			move = (m_Carrying ? move * m_CarrySpeed : move);

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			m_Anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (m_Grounded && jump && m_Anim.GetBool("Ground"))
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Anim.SetBool("Ground", false);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	public bool IsGrounded()
	{
		return m_Grounded;
	}


	public bool IsCarrying()
	{
		return m_Carrying;
	}
}
