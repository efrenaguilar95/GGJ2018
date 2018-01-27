using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	public float smoothing = 0.3f;
	public float lookAheadFactor = 2;
	public float lookAheadReturnSpeed = 0.1f;
	public float lookAheadMoveThreshold = 0.1f;
	public float lookAboveFactor = 2;
	public float cameraFloor = 0;

	private float m_OffsetZ;
	private Vector3 m_LastPlayerPosition;
	private Vector3 m_CurrentVelocity;
	private Vector3 m_LookAheadPos;
	private Vector3 m_LookAbovePos;
	private float m_yCameraPos = 0;

	// Use this for initialization
	private void Start()
	{
		m_LastPlayerPosition = player.transform.position;
		m_OffsetZ = (transform.position - player.transform.position).z;
		transform.parent = null;
	}


	// Update is called once per frame
	private void Update()
	{
		// update lookahead pos if accelerating or changed direction
		float xMoveDelta = (player.transform.position - m_LastPlayerPosition).x;
		bool updateLookAheadPlayer = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

		if (updateLookAheadPlayer)
			m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		else
			m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * lookAheadReturnSpeed);

		// update lookabove pos if player is on ground
		CharacterController characterControllerScript = player.GetComponent<CharacterController>();
		bool playerIsGrounded = characterControllerScript.IsGrounded();

		float yMoveDelta = m_yCameraPos - player.transform.position.y;
		if (playerIsGrounded || yMoveDelta > 0)
			m_yCameraPos = player.transform.position.y;

		if (m_yCameraPos < cameraFloor)
			m_yCameraPos = cameraFloor;

		m_LookAbovePos = lookAheadFactor * Vector3.up;

		// smooth camera movement
		Vector3 cameraPos = new Vector3(player.transform.position.x + m_LookAheadPos.x, m_yCameraPos + m_LookAbovePos.y, player.transform.position.z);
		Vector3 targetPos = cameraPos + Vector3.forward * m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref m_CurrentVelocity, smoothing);
		transform.position = newPos;

		// update last player position
		m_LastPlayerPosition = player.transform.position;
	}
}
