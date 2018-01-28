using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject m_player;
	public float m_smoothing = 0.3f;
	public float m_lookAheadFactor = 3f;
	public float m_lookAheadReturnSpeed = 0f;
	public float m_lookAheadMoveThreshold = 0.1f;
	public float m_lookAboveFactor = 1f;
	public float m_cameraFloor = 1.8f;

	private float m_OffsetZ;
	private Vector3 m_LastPlayerPosition;
	private Vector3 m_CurrentVelocity;
	private Vector3 m_LookAheadPos;
	private Vector3 m_LookAbovePos;
	private float m_yCameraPos = 0;

	// Use this for initialization
	private void Start()
	{
		m_LastPlayerPosition = m_player.transform.position;
		m_OffsetZ = (transform.position - m_player.transform.position).z;
		transform.parent = null;
	}


	// Update is called once per frame
	private void Update()
	{
		// update lookahead pos if accelerating or changed direction
		float xMoveDelta = (m_player.transform.position - m_LastPlayerPosition).x;
		bool updateLookAheadPlayer = Mathf.Abs(xMoveDelta) > m_lookAheadMoveThreshold;

		if (updateLookAheadPlayer)
			m_LookAheadPos = m_lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
		else
			m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime * m_lookAheadReturnSpeed);

		// update lookabove pos if player is on ground
		PlayerController playerControllerScript = m_player.GetComponent<PlayerController>();
		bool playerIsGrounded = playerControllerScript.IsGrounded();

		float yMoveDelta = m_yCameraPos - m_player.transform.position.y;
		if (playerIsGrounded || yMoveDelta > 0)
			m_yCameraPos = m_player.transform.position.y;

		if (m_yCameraPos < m_cameraFloor)
			m_yCameraPos = m_cameraFloor;

		m_LookAbovePos = m_lookAboveFactor * Vector3.up;

		// smooth camera movement
		Vector3 cameraPos = new Vector3(m_player.transform.position.x + m_LookAheadPos.x, m_yCameraPos + m_LookAbovePos.y, m_player.transform.position.z);
		Vector3 targetPos = cameraPos + Vector3.forward * m_OffsetZ;
		Vector3 newPos = Vector3.SmoothDamp(transform.position, targetPos, ref m_CurrentVelocity, m_smoothing);
		transform.position = newPos;

		// update last player position
		m_LastPlayerPosition = m_player.transform.position;
	}
}
