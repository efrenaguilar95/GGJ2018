using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis
	[SerializeField] private float m_JumpForce = 600f;                  // Amount of force added when the player jumps
	[Range(0, 1)] [SerializeField] private float m_CarrySpeed = .5f;    // Amount of maxSpeed applied to carrying movement (1 = 100%)
	[SerializeField] private bool m_AirControl = true;                  // Whether or not a player can steer while jumping
	[SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character
	[SerializeField] private bool m_Carrying;            				// Whether or not the player is carrying a person
	[SerializeField] private bool m_Electric;
	public Transform m_CarryPos;
	public Sprite m_HappyHead;
	public Sprite m_NormalHead;
	public Sprite m_SadHead;

    private CurveNode mEnteredCurveNode;
    private PlayerSplineWalker m_SplineMover;

    private Transform m_GroundCheck;    // A position marking where to check if the player is grounded
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded
	private bool m_CarryToggle;         // Restricts picking up and placing down with the same keypress
	[SerializeField] private int m_Happiness;


	private bool m_OnHuman;
	private GameObject m_Human;
	private Animator m_HumanAnim;
	private Vector3 m_HumanCarryingPos;
	private bool m_HumanFacingRight;

	private Animator m_Anim;            // Reference to the player's animator component
	private Rigidbody2D m_Rigidbody2D;  // Reference to the player's rigidbody2d component
	private bool m_FacingRight = true;  // For determining which way the player is currently facing

    public UnityEvent helpToggleEvent; //Event called when the player m_CarryTogggle is changed

	private void Awake()
	{
		// Setting up references.
		m_GroundCheck = transform.Find("GroundCheck");
		m_Grounded = false;
		m_Carrying = false;
		m_CarryToggle = false;
		m_Electric = false;
		m_Happiness = 0;
		m_Anim = GetComponentInChildren<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //Setting up spline mover reference
        m_SplineMover = GetComponent<PlayerSplineWalker>();
        if (m_SplineMover == null)
        {
            m_SplineMover = this.gameObject.AddComponent<PlayerSplineWalker>();
            m_SplineMover.duration = 3f;
        }
    }


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Human") && m_Carrying == false)
		{
			m_OnHuman = true;

			m_Human = other.transform.parent.gameObject;
			m_HumanAnim = m_Human.GetComponentInChildren<Animator>();
			CheckHumanFacingRight();
		}
        if (other.CompareTag("Curve"))
        {
            this.mEnteredCurveNode = other.GetComponent<CurveNode>();
        }
    }
	

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Human") && m_Carrying == false)
		{
			m_OnHuman = false;
		}
        if (other.CompareTag("Curve"))
        {
            this.mEnteredCurveNode = null;
        }
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


	private void Update()
	{
		if (m_Human != null && m_Carrying)
			m_Human.transform.position = m_CarryPos.position;
	}


	private void CheckHumanFacingRight()
	{
		if (m_Human.transform.localScale.x > 0)
			m_HumanFacingRight = true;
		else
			m_HumanFacingRight = false;
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1
		Vector3 playerScale = transform.localScale;
		playerScale.x *= -1;
		transform.localScale = playerScale;

		FlipHuman();
	}


	private void FlipHuman()
	{
		// Multiply the human's x scale by -1
		if (m_Carrying)
		{
			Vector3 humanScale = m_Human.transform.localScale;
			Quaternion humanRot = m_Human.transform.localRotation;
			humanScale.x *= -1;
			humanRot.z *= -1;
			m_Human.transform.localScale = humanScale;
			m_Human.transform.localRotation = humanRot;
		}
	}


	private void AttemptPickup(bool pickup)
	{
		if (m_OnHuman)
		{
			if (pickup && !m_CarryToggle)
			{
				m_CarryToggle = true;
				m_Carrying = !m_Carrying;
                helpToggleEvent.Invoke();
                m_HumanAnim.SetBool("Hanging", m_Carrying);

				CheckHumanFacingRight();
				if (m_Carrying && (m_FacingRight != m_HumanFacingRight))
					FlipHuman ();
			}
			else if (!pickup && m_CarryToggle)
			{
				m_CarryToggle = false;

			}

		}
				
		m_Anim.SetBool ("Carry", m_Carrying);
	}


    private void AttemptTravel()
    {
        // test is on start node
        switch (m_Electric)
        {
            case (true): //is currently moving
                m_SplineMover.disableSplineMovement();
                m_Electric = false;
                break;
            case (false):
                if (mEnteredCurveNode != null)
                {
                    var spline = mEnteredCurveNode.GetComponentInParent<BezierSpline>();
                    if (spline != null)
                    {
                        //Begin wire travel
                        if (spline.GetIndexOfNode(mEnteredCurveNode) == 0) //If start of curve
                        {
                            m_SplineMover.enableSplineMovement(PlayerSplineWalker.TravelDirection.Forward, spline);
                        }
                        else //if end of curve
                        {
                            m_SplineMover.enableSplineMovement(PlayerSplineWalker.TravelDirection.Backward, spline);
                        }
                        m_Electric = true;
                        Debug.Log("Attempted to Travel");
                    }
                    else
                    {
                        Debug.LogError("Could not find parent spline of curve node " + mEnteredCurveNode.index);
                    }
                    // travel through wire

                    //
                    // change animation
                }
                break;

        }
        if (mEnteredCurveNode != null)
        {
            // travel through wire
            m_Electric = !m_Electric;
            Debug.Log("Attempted to Travel");

            //
            // change animation
        }
    }


    public void Move(float move, bool jump, bool pickup, bool travel)
	{
		// attempt to pickup a person if s is pressed
		AttemptPickup(pickup);

		// attempt to travel through a wire if shift is pressed
		if (travel)
			AttemptTravel();
		
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

	public bool IsElectric()
	{
		return m_Electric;
	}

	public void ChangeHappiness(int change)
	{
		m_Happiness += change;
		GameObject headSprite = GameObject.FindGameObjectWithTag ("Head");
		SpriteRenderer headSpriteRenderer = headSprite.GetComponent<SpriteRenderer> ();
		if (m_Happiness >= 10) {
			headSpriteRenderer.sprite = m_HappyHead;
		} else if (m_Happiness <= -10) {
			headSpriteRenderer.sprite = m_SadHead;
		} else {
			headSpriteRenderer.sprite = m_NormalHead;
		}
	}

	public int GetHappiness()
	{
		return m_Happiness;
	}
}
