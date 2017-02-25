using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour {
	// Rigidbody
	private Rigidbody2D m_rb2d;

	// Movement Stuff
	private Vector2 m_velocity;
	private Vector3 m_startPosition;
	private Vector3 m_targetPositionHorizontal;
	private Vector3 m_targetPositionVertical;
	public bool m_invert;
	public bool m_activate;
	public bool m_moveUp;
	public bool m_moveDown;
	public bool m_moveLeft;
	public bool m_moveRight;
	public float m_moveSpeed;
	public float m_moveDistance;
	public float m_maxRetractTimer;
	private float m_retractTimer;
	private bool m_reset;
	public bool m_active = false;
	private Animator animator;

	void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();
		m_startPosition = transform.position;
		m_retractTimer = m_maxRetractTimer;
		animator = GetComponent<Animator>();
		if(m_invert) {
			m_targetPositionHorizontal = new Vector3(m_startPosition.x - m_moveDistance, m_startPosition.y, m_startPosition.z);
			m_targetPositionVertical = new Vector3(m_startPosition.x, m_startPosition.y - m_moveDistance, m_startPosition.z);
			m_moveSpeed = -m_moveSpeed;
		} else {
			m_targetPositionHorizontal = new Vector3(m_startPosition.x + m_moveDistance, m_startPosition.y, m_startPosition.z);
			m_targetPositionVertical = new Vector3(m_startPosition.x, m_startPosition.y + m_moveDistance, m_startPosition.z);
		}
	}

	void FixedUpdate() {
		HandleMovement();
	}

	void HandleMovement() {
		if(m_activate && !m_reset) {
			m_active = true;
			m_retractTimer -= Time.fixedDeltaTime;
			if(m_moveUp){
				m_velocity = new Vector2(0, 1);
				MoveUp();
				animator.SetBool("isTriggered", true);
			} else if(m_moveDown) {
				m_velocity = new Vector2(0, 1);
				MoveDown();
			} else if(m_moveLeft){
				m_velocity = new Vector2(1, 0);
				MoveLeft();
			} else if(m_moveRight) {
				m_velocity = new Vector2(1, 0);
				MoveRight();
			} else {
				m_rb2d.position = m_startPosition;
				animator.SetBool("isTriggered", false);
			}

			if(m_retractTimer <= 0) {
				m_reset = true;
				m_activate = false;
				m_retractTimer = m_maxRetractTimer;
			}
		}else{
			transform.position = Vector2.MoveTowards(transform.position, m_startPosition, .01f);
			m_reset = false;
			if(this.transform.position == m_startPosition) {
				m_active = false;
				animator.SetBool("isTriggered", false);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.CompareTag("Player") && !m_active) {
			m_activate = true;
		}
	}
	void OnTriggerStay2D(Collider2D other) {
		if(other.CompareTag("Player") && !m_active) {
			m_activate = true;
		}
	}

	void MoveRight() {
		if(transform.position.x < m_targetPositionHorizontal.x) {
			m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
		} else {
			m_rb2d.velocity = new Vector3(0, 0, 0);
		}
	}

	void MoveLeft() {
		if(transform.position.x > m_targetPositionHorizontal.x) {
			m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
		} else {
			m_rb2d.velocity = new Vector3(0, 0, 0);
		}
	}

	void MoveUp() {
		if(transform.position.y < m_targetPositionVertical.y) {
			m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
		} else {
			m_rb2d.velocity = new Vector3(0, 0, 0);
		}
	}

	void MoveDown() {
		if(transform.position.y > m_targetPositionVertical.y) {
			m_rb2d.velocity += (m_velocity * m_moveSpeed) * Time.fixedDeltaTime;
		} else {
			m_rb2d.velocity = new Vector3(0, 0, 0);
		}
	}
}
